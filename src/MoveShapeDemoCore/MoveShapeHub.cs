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
        
        public MoveShapeHub() : this(PositionCalculator.Instance) { }
        public MoveShapeHub(PositionCalculator positionCalculator)
        {
            _positionCalculator = positionCalculator;
        }

        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(clientModel.LastUpdatedBy).updateShape(clientModel);
        }

        public override Task OnConnected()
        {



            _positionCalculator.AddBall();
            return base.OnConnected();
        }
    }
    public class ShapeModel
    {
        // We declare Left and Top as lowercase with 
        // JsonProperty to sync the client and server models
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        // We don't want the client to get the "LastUpdatedBy" property
        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }
}
