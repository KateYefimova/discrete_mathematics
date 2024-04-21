using System.Collections.Generic;

namespace graph
{
    public class Graph
    {
        public enum RepresentationType
        {
            AdjacencyList,
            AdjacencyMatrix
        }

        public int V; // Number of vertices
        public List<Tuple<int, int>>[] adj; // Adjacency list
        public int[,] adjacencyMatrix; // Adjacency matrix
        public RepresentationType Representation;

        public Graph(int v, double density, RepresentationType representation)
        {
            V = v;
            Representation = representation;

            if (Representation == RepresentationType.AdjacencyList)
            {
                adj = new List<Tuple<int, int>>[V];
                for (int i = 0; i < V; i++)
                {
                    adj[i] = new List<Tuple<int, int>>();
                }
            }
            else if (Representation == RepresentationType.AdjacencyMatrix)
            {
                adjacencyMatrix = new int[V, V];
                UpdateAdjacencyMatrix();
            }

            GenerateRandomGraph(density);
        }

        public void AddEdge(int u, int v, int weight)
        {
            if (Representation == RepresentationType.AdjacencyList)
            {
                adj[u].Add(new Tuple<int, int>(v, weight));
                adj[v].Add(new Tuple<int, int>(u, weight)); // For undirected graph, add both directions
            }
            else if (Representation == RepresentationType.AdjacencyMatrix)
            {
                adjacencyMatrix[u, v] = weight;
                adjacencyMatrix[v, u] = weight;
            }
        }

        private void UpdateAdjacencyMatrix()
        {
            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    if (i != j)
                    {
                        adjacencyMatrix[i, j] = 0; // If vertex is not connected, edge weight is 0
                    }
                }
            }
        }

        private void GenerateRandomGraph(double density)
        {
            Random random = new Random();
            HashSet<Tuple<int, int>> generatedEdges = new HashSet<Tuple<int, int>>();

            for (int i = 0; i < V; i++)
            {
                for (int j = i + 1; j < V; j++)
                {
                    if (i != j && random.NextDouble() < density && !generatedEdges.Contains(new Tuple<int, int>(i, j)))
                    {
                        int weight = random.Next(1, 101); // Generate edge weight (from 1 to 100)
                        AddEdge(i, j, weight); // Add edge between vertices i and j
                        generatedEdges.Add(new Tuple<int, int>(i, j));
                        generatedEdges.Add(new Tuple<int, int>(j, i)); // Add reverse edge for undirected graph
                    }
                }
            }
        } 
        public void PrintAdjacencyLists()
        {
            Console.WriteLine("Списки суміжності:");
            for (int i = 0; i < V; i++)
            {
                Console.Write("Вершина " + i + " з'єднана з: ");
                foreach (var edge in adj[i])
                {
                    Console.Write(edge.Item1 + "(" + edge.Item2 + ") "); // Item1 - вершина, Item2 - вага ребра
                }

                Console.WriteLine();
            }
        }

        // Вивід матриці суміжності
        public void PrintAdjacencyMatrix()
        {
            Console.WriteLine("Матриця суміжності:");
            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    Console.Write(adjacencyMatrix[i, j] + " ");
                }

                Console.WriteLine();
            }
        }
    }
}