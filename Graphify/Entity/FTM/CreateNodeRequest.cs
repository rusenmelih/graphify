using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.FTM
{
    public class CreateNodeRequest
    {
        public string Identifier { get; set; }
        public string NodeName { get; set; }
        public double? Latitude { get; set; }
        public double? Altitude { get; set; }
        public double? Longitude { get; set; }
        public int NodeType { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Radius { get; set; }
        public string Color { get; set; }
        public float LineWidth { get; set; }
        public string LineColor { get; set; }
    }
}
