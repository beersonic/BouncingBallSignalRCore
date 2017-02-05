using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoveShapeDemoCore
{
    public class PositionCalculator
    {
        const int MAX_X = 500;
        const int MAX_Y = 500;
        const int REFRESH_RATE_MS = 20;
        const int RADIUS = 30;
        const int SPEED = 3;

        Dictionary<int, BallInfo> _dictBall;
        int _id = 0;
        Random _rand;

        // Singleton instance
        private readonly static Lazy<PositionCalculator> _instance = new Lazy<PositionCalculator>(
            () => new PositionCalculator(Startup.ConnectionManager.GetHubContext<MoveShapeHub>()));

        private IHubContext _context;
        Task _task = null;
        public static PositionCalculator Instance
        {
            get
            {
                return _instance.Value;
            }
        }
        private PositionCalculator(IHubContext context)
        {
            _dictBall = new Dictionary<int, BallInfo>();
            _context = context;
            _rand = new Random();

            // async task
            _task = Task.Run(() =>
           {
               while (true)
               {
                   
                   lock(_dictBall)
                   {
                       if (_dictBall.Count == 0)
                       {
                           Thread.Sleep(500);
                       }
                       else
                       {
                           List<int> keys = new List<int>(_dictBall.Keys);
                           Thread.Sleep(REFRESH_RATE_MS);
                           foreach (int key in keys)
                           {
                               var bi = _dictBall[key];
                               CalculateNewBallPosition(ref bi);
                               _context.Clients.All.updateBallInfo(key, bi);
                           }

                           CheckCollision();
                       }
                   }
                   
               }
           });
        }

        private void CheckCollision()
        {
            // TODO:
        }

        private void CalculateNewBallPosition(ref BallInfo bi)
        {
            bi.Left += bi.DirectionX;
            if (bi.Left >= MAX_X || bi.Left <= 0)
            {
                bi.DirectionX *= -1;
                bi.Left += bi.DirectionX;
            }

            bi.Top += bi.DirectionY;
            if (bi.Top >= MAX_Y ||bi.Top <= 0)
            {
                bi.DirectionY *= -1;
                bi.Top += bi.DirectionY;
            }
            
        }

        public int AddBall()
        {
            int x = _rand.Next(MAX_X);
            int y = _rand.Next(MAX_Y);

            var ballInfo = new BallInfo();
            ballInfo.Left = x;
            ballInfo.Top = y;
            ballInfo.Radius = RADIUS;

            double angle = _rand.Next(90);
            ballInfo.DirectionX = Math.Cos(angle) * SPEED;
            ballInfo.DirectionY = Math.Sin(angle) * SPEED;

            int id = _id++;
            lock (_dictBall)
            {
                _dictBall[id] = ballInfo;
            }
            return id;
        }

        public void RemoveBall(int id)
        {
            lock(_dictBall)
            {
                _dictBall.Remove(id);
                _context.Clients.All.removeBall(id);
            }
        }
    }
}
