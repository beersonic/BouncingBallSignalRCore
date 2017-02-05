using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoveShapeDemoCore
{
    public class BallInfo
    {
        [JsonProperty("left")]
        public double Left { get; set; }

        [JsonProperty("top")]
        public double Top { get; set; }

        [JsonProperty("radius")]
        public double Radius { get; set; }

        public double DirectionX { get; set; }
        public double DirectionY { get; set; }
    }
}
