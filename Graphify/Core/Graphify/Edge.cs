using System;

namespace Core.Graphify
{
    public class Edge
    {
        public Node Source { get; set; }
        public Node Destination { get; set; }
        public double Weight { get; set; }
        public string Name { get; set; }
        public float Width { get; set; }
        public string Color { get; set; }

        public Edge()
        {
            
        }

        public Edge(Node source, Node destination, string name)
        {
            this.Source = source;
            this.Destination = destination;
            this.Name = name;
        }

        public Edge(Node source, Node destination, string name, double weight)
        {
            this.Source = source;
            this.Destination = destination;
            this.Name = name;
            this.Weight = weight;
        }

    }
}
