using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoveShapeDemoCore
{

    public class MoveShapeHub : Hub
    {
        private readonly PositionCalculator _positionCalculator;

        static Dictionary<string, int> _dictConnIdToBallId = new Dictionary<string, int>();
        public MoveShapeHub() : this(PositionCalculator.Instance) { }
        public MoveShapeHub(PositionCalculator positionCalculator)
        {
            _positionCalculator = positionCalculator;
        }

        public override Task OnConnected()
        {
            _dictConnIdToBallId[Context.ConnectionId] = _positionCalculator.AddBall();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (_dictConnIdToBallId.ContainsKey(Context.ConnectionId))
            {
                int id = _dictConnIdToBallId[Context.ConnectionId];
                _positionCalculator.RemoveBall(id);
            }
            return base.OnDisconnected(stopCalled);
        }
    }

}
