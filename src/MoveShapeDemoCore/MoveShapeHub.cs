using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace MoveShapeDemoCore
{

    public class MoveShapeHub : Hub
    {
        static Dictionary<string, int> _dictConnIdToBallId = new Dictionary<string, int>();
        
        IPositionCalculator _positionCalculator = null;

        public MoveShapeHub()
        {
            _positionCalculator = PositionCalculatorFactory.Instance.GetPositionCalculator();
            _positionCalculator.OnBallInfoUpdateCallback += _positionCalculator_OnBallInfoCallback;
            _positionCalculator.OnBallRemoveCallback += _positionCalculator_OnBallRemoveCallback;
        }

        private void _positionCalculator_OnBallRemoveCallback(int id)
        {
            Clients.All.removeBall(id);
        }

        private void _positionCalculator_OnBallInfoCallback(int id, BallInfo bi)
        {
            Clients.All.updateBallInfo(id, bi);
        }

        public override Task OnConnected()
        {
            lock (_dictConnIdToBallId)
            {
                _dictConnIdToBallId[Context.ConnectionId] = _positionCalculator.AddBall();
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            lock (_dictConnIdToBallId)
            {
                if (_dictConnIdToBallId.ContainsKey(Context.ConnectionId))
                {
                    int id = _dictConnIdToBallId[Context.ConnectionId];
                    _positionCalculator.RemoveBall(id);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }

}
