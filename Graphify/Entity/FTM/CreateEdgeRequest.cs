using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.FTM
{
    public class CreateEdgeRequest
    {
        public string Identifier { get; set; }
        public string SourceNodeName { get; set; }
        public string DestinationNodeName { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public string Color { get; set; }
        public double Weight { get; set; }
    }
}
