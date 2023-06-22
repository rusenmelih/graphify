using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graphify.Algorithms
{
    public class DijkstraAlgorithm
    {
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }

        public DijkstraAlgorithm(List<Node> nodes, List<Edge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public List<Node> FindShortestPath(Node startNode, Node endNode)
        {
            Dictionary<Node, double> distance = new Dictionary<Node, double>();
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
            List<Node> unvisitedNodes = new List<Node>();

            foreach (var node in Nodes)
            {
                distance[node] = double.MaxValue;
                previous[node] = null;
                unvisitedNodes.Add(node);
            }

            distance[startNode] = 0;

            while (unvisitedNodes.Count > 0)
            {
                var currentNode = GetNodeWithMinDistance(distance, unvisitedNodes);
                unvisitedNodes.Remove(currentNode);

                if (currentNode == endNode)
                    break;

                List<Edge> currentEdges = this.Edges.FindAll(p => p.Source == currentNode);

                foreach (var edge in currentEdges)
                {
                    var neighborNode = edge.Destination;
                    var edgeWeight = edge.Weight;
                    var newDistance = distance[currentNode] + edgeWeight;

                    if (newDistance < distance[neighborNode])
                    {
                        distance[neighborNode] = newDistance;
                        previous[neighborNode] = currentNode;

                    }
                }
            }

            return ReconstructPath(previous, endNode);
        }

        private Node GetNodeWithMinDistance(Dictionary<Node, double> distance, List<Node> unvisitedNodes)
        {
            return unvisitedNodes.OrderBy(node => distance[node]).First();
        }

        private List<Node> ReconstructPath(Dictionary<Node, Node> previous, Node endNode)
        {
            var path = new List<Node>();
            Node currentNode = endNode;

            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = previous[currentNode];
            }

            return path;
        }
    }
}
