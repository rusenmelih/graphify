using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Graphify.Algorithms
{
    public class GeneticAlgorithm
    {
        private readonly List<Node> nodes;
        private readonly List<Edge> edges;
        private Random random;
        private int populationSize;
        private double mutationRate;
        private int maxGenerations;

        public GeneticAlgorithm(List<Node> nodes, List<Edge> edges, int populationSize, double mutationRate, int maxGenerations)
        {
            this.nodes = nodes;
            this.edges = edges;
            this.random = new Random();
            this.populationSize = populationSize;
            this.mutationRate = mutationRate;
            this.maxGenerations = maxGenerations;
        }

        public List<Node> Solve(Node startNode, Node endNode)
        {
            List<List<Node>> population = InitializePopulation(startNode, endNode);

            for (int generation = 0; generation < maxGenerations; generation++)
            {
                List<List<Node>> nextGeneration = new List<List<Node>>();

                while (nextGeneration.Count < populationSize)
                {
                    List<Node> parent1 = SelectParent(population);
                    List<Node> parent2 = SelectParent(population);

                    List<Node> child = Crossover(parent1, parent2);
                    Mutate(child);

                    nextGeneration.Add(child);
                }

                population = nextGeneration;
            }

            List<Node> bestRoute = population.OrderBy(route => GetRouteLength(route)).First();
            return bestRoute;
        }

        private List<List<Node>> InitializePopulation(Node startNode, Node endNode)
        {
            List<List<Node>> population = new List<List<Node>>();

            while (population.Count < populationSize)
            {
                List<Node> route = GenerateRandomRoute(startNode, endNode);
                population.Add(route);
            }

            return population;
        }

        private List<Node> GenerateRandomRoute(Node startNode, Node endNode)
        {
            List<Node> route = new List<Node>();
            Node currentNode = startNode;

            while (currentNode != endNode)
            {
                route.Add(currentNode);

                List<Edge> validEdges = edges.Where(edge => edge.Source == currentNode && !route.Contains(edge.Destination)).ToList();
                if (validEdges.Count == 0)
                    break;

                Edge selectedEdge = validEdges[random.Next(validEdges.Count)];
                currentNode = selectedEdge.Destination;
            }

            route.Add(endNode);
            return route;
        }

        private List<Node> SelectParent(List<List<Node>> population)
        {
            double totalFitness = population.Sum(route => 1 / GetRouteLength(route));
            double randomNumber = random.NextDouble() * totalFitness;

            double cumulativeFitness = 0;
            foreach (List<Node> route in population)
            {
                cumulativeFitness += 1 / GetRouteLength(route);
                if (cumulativeFitness >= randomNumber)
                    return route;
            }

            return population.Last();
        }

        private List<Node> Crossover(List<Node> parent1, List<Node> parent2)
        {
            int crossoverPoint1 = random.Next(1, parent1.Count - 1);
            int crossoverPoint2 = random.Next(1, parent1.Count - 1);

            if (crossoverPoint2 < crossoverPoint1)
                (crossoverPoint1, crossoverPoint2) = (crossoverPoint2, crossoverPoint1);

            List<Node> child = parent1.GetRange(0, crossoverPoint1);
            child.AddRange(parent2.GetRange(crossoverPoint1, crossoverPoint2 - crossoverPoint1 + 1));
            child.AddRange(parent1.GetRange(crossoverPoint2 + 1, parent1.Count - crossoverPoint2 - 1));

            return child;
        }

        private void Mutate(List<Node> route)
        {
            for (int i = 1; i < route.Count - 1; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    int randomIndex = random.Next(1, route.Count - 1);
                    (route[i], route[randomIndex]) = (route[randomIndex], route[i]);
                }
            }
        }

        private double GetRouteLength(List<Node> route)
        {
            double totalLength = 0;

            for (int i = 0; i < route.Count - 1; i++)
            {
                Node currentNode = route[i];
                Node nextNode = route[i + 1];

                Edge edge = edges.FirstOrDefault(e => (e.Source == currentNode && e.Destination == nextNode) || (e.Source == nextNode && e.Destination == currentNode));
                if (edge != null)
                    totalLength += edge.Weight;
            }

            return totalLength;
        }
    }
}
