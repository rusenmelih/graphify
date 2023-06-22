using GeoCoordinatePortable;
using System;


namespace Core.Graphify
{
    public enum NodeType
    {
        DepotNode = 1,
        CustomerNode = 2,
        ChargingStationNode = 3,
    }

    public class Node
    {
        //Yapısal özellikler
        public string Name { get; set; }
        public GeoCoordinate Coordinate { get; set; }
        public NodeType Type { get; set; }
        public List<Edge> Edges { get; set; }

        //Grafiksel özellikler
        public Vector2D Position { get; set; }
        public float Radius { get; set; }
        public string Color { get; set; }
        public float LineWidth { get; set; }
        public string LineColor { get; set; }

        public Node()
        {
            
        }

        public Node(string name)
        {
            this.Name = name;
            this.Coordinate = new GeoCoordinate();
            this.Type = NodeType.CustomerNode;
            this.Edges = new List<Edge>();
        }
    }
}
