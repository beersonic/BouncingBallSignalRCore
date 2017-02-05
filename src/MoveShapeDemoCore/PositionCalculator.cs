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
        Dictionary<int, BallInfo> _dictBall;
        int _id = 0;
        const int MAX_X = 500;
        const int MAX_Y = 500;
        Random _rand;
        const int REFRESH_RATE_MS = 500;

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
                           foreach (int key in keys)
                           {
                               Thread.Sleep(REFRESH_RATE_MS);

                               var bi = _dictBall[key];
                               CalculateNewBallPosition(ref bi);
                               _context.Clients.All.updateBallInfo(key, bi);
                           }
                       }
                   }
                   
               }
           });
        }

        private void CalculateNewBallPosition(ref BallInfo bi)
        {
            bi.Left = _rand.Next(MAX_X);
            bi.Top = _rand.Next(MAX_Y);
        }
        public void AddBall()
        {
            int x = _rand.Next(MAX_X);
            int y = _rand.Next(MAX_Y);

            var ballInfo = new BallInfo();
            ballInfo.Left = x;
            ballInfo.Top = y;

            lock (_dictBall)
            {
                _dictBall[_id++] = ballInfo;
            }
        }
    }
}
