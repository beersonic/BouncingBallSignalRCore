using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MoveShapeDemoCore
{
    public class PositionCalculatorFactory
    {
        const int REFRESH_RATE_MS = 20;

        private static PositionCalculatorFactory _instance;
        private static Object _lockConstructor = new object();

        private PositionCalculator _positionCalculator = null;

        Task _task = null;
        bool _isDone;
        private PositionCalculatorFactory()
        {
            _isDone = false;

            _positionCalculator = new PositionCalculator();

            _task = new Task(ThreadMain);
            _task.Start();
        }

        ~PositionCalculatorFactory()
        {
            _isDone = true;
        }
        public static PositionCalculatorFactory Instance
        {
            get
            {
                lock (_lockConstructor)
                {
                    if (_instance == null)
                    {
                        _instance = new PositionCalculatorFactory();
                    }
                    return _instance;
                }
            }
        }

        private void ThreadMain()
        {
            while (!_isDone)
            {
                int c = _positionCalculator.GetBallCount();
                if (c == 0)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(REFRESH_RATE_MS);
                    _positionCalculator.UpdateBallPosition();
                }
            }
        }

        public IPositionCalculator GetPositionCalculator()
        {
            return _positionCalculator;
        }
    }
}
