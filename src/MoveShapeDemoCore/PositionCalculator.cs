using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoveShapeDemoCore
{
    public class PositionCalculator : IPositionCalculator
    {
        const int MAX_X = 500;
        const int MAX_Y = 500;
        const int RADIUS = 30;
        const int SPEED = 3;

        Dictionary<int, BallInfo> _dictBall = new Dictionary<int, BallInfo>();
        int _id = 0;
        Random _rand;

        public delegate void OnUpdateBallInfo(int id, BallInfo bi);
        public delegate void OnBallRemove(int id);

        public event OnBallRemove OnBallRemoveCallback = delegate { } ;
        public event OnUpdateBallInfo OnBallInfoUpdateCallback = delegate { };

        public Dictionary<int, BallInfo> GetBalls()
        {
            return _dictBall;
        }

        public PositionCalculator()
        {
            _dictBall = new Dictionary<int, BallInfo>();
            _rand = new Random();
        }

        public void UpdateBallPosition()
        {
            lock (_dictBall)
            {
                foreach (var kvp in _dictBall)
                {
                    var bi = _dictBall[kvp.Key];
                    CalculateNewBallPosition(ref bi);
                    CheckCollision();
                    OnBallInfoUpdateCallback(kvp.Key, kvp.Value);
                }
            }
        }

        private void CheckCollision()
        {
            foreach (var kvp1 in _dictBall)
            {
                foreach (var kvp2 in _dictBall)
                {
                    if (kvp1.Key != kvp2.Key)
                    {
                        var bi1 = kvp1.Value;
                        var bi2 = kvp2.Value;

                        double x1 = bi1.Left;
                        double y1 = bi1.Top;

                        double x2 = bi2.Left;
                        double y2 = bi2.Top;

                        double dx = Math.Abs(x1 - x2);
                        double dy = Math.Abs(y1 - y2);
                        double diff = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));
                        if (diff <= RADIUS * 2)
                        {
                            // collision logic
                            if (x1 > x2)
                            {
                                bi1.DirectionX *= -1;
                                bi1.DirectionY *= -1;

                                bi2.DirectionX *= -1;
                                bi2.DirectionY *= -1;
                            }
                        }

                        if (diff < RADIUS * 2)
                        {
                            double dx2 = Math.Sqrt(Math.Pow(RADIUS * 2, 2) - Math.Pow(dy, 2));
                            if (x2 > x1)
                            {
                                bi2.Left = x1 + dx2;
                            }
                            else
                            {
                                bi2.Left = x1 - dx2;   
                            }
                        }
                    }
                }
            }
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
                OnBallRemoveCallback(id);
            }
        }

        public int GetBallCount()
        {
            return _dictBall.Count;
        }
    }
}
