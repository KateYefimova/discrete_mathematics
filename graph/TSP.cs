using System;
using System.Collections.Generic;
using System.Linq;
using graph;

public static class BruteForceTSP
{
    // Словник для зберігання обчислених шляхів
    private static Dictionary<Tuple<int, List<int>>, List<List<int>>> memoPaths = new Dictionary<Tuple<int, List<int>>, List<List<int>>>();

    // Головний метод для пошуку оптимального шляху комівояжера
    public static List<int> BfTravellingSalesman(Graph graph)
    {
        int startVertex = FindMostOptimalStartVertex(graph);
        List<int> salesmanPath = FindNearestNeighborPath(graph, startVertex);
        return salesmanPath;
    }

    // Метод для пошуку найоптимальнішої стартової вершини
    private static int FindMostOptimalStartVertex(Graph graph)
    {
        int mostOptimalVertex = 0;
        int minDistance = int.MaxValue;

        for (int i = 0; i < graph.V; i++)
        {
            int totalDistance = 0;
            if (graph.Representation == Graph.RepresentationType.AdjacencyList)
            {
                foreach (var edge in graph.adj[i])
                {
                    totalDistance += edge.Item2;
                }
            }
            else if (graph.Representation == Graph.RepresentationType.AdjacencyMatrix)
            {
                for (int j = 0; j < graph.V; j++)
                {
                    totalDistance += graph.adjacencyMatrix[i, j];
                }
            }

            if (totalDistance < minDistance)
            {
                minDistance = totalDistance;
                mostOptimalVertex = i;
            }
        }

        return mostOptimalVertex;
    }

    // Метод для пошуку шляху за алгоритмом найближчого сусіда
    private static List<int> FindNearestNeighborPath(Graph graph, int startVertex)
    {
        List<int> path = new List<int>();
        HashSet<int> visited = new HashSet<int>();
        int currentVertex = startVertex;

        while (visited.Count < graph.V)
        {
            path.Add(currentVertex);
            visited.Add(currentVertex);

            int minWeight = int.MaxValue;
            int nearestNeighbor = -1;

            if (graph.Representation == Graph.RepresentationType.AdjacencyList && graph.adj != null)
            {
                foreach (var edge in graph.adj[currentVertex])
                {
                    int neighbor = edge.Item1;
                    int weight = edge.Item2;
                    if (!visited.Contains(neighbor) && weight < minWeight)
                    {
                        minWeight = weight;
                        nearestNeighbor = neighbor;
                    }
                }
            }
            else if (graph.Representation == Graph.RepresentationType.AdjacencyMatrix)
            {
                for (int i = 0; i < graph.V; i++)
                {
                    int weight = graph.adjacencyMatrix[currentVertex, i];
                    if (!visited.Contains(i) && weight > 0 && weight < minWeight)
                    {
                        minWeight = weight;
                        nearestNeighbor = i;
                    }
                }
            }

            if (nearestNeighbor != -1)
            {
                currentVertex = nearestNeighbor;
            }
            else
            {
                // Якщо немає найближчого сусіда, переходимо до будь-якої невідвіданої вершини
                foreach (var vertex in Enumerable.Range(0, graph.V))
                {
                    if (!visited.Contains(vertex))
                    {
                        currentVertex = vertex;
                        break;
                    }
                }
            }
        }

        // Додавання початкової вершини в кінець шляху, щоб утворити цикл
        path.Add(startVertex);
        return path;
    }

    public static bool IsTSPSolvable(Graph graph)
    {
        bool hasEdges = false;
        if (graph.Representation == Graph.RepresentationType.AdjacencyList)
        {
            hasEdges = graph.adj.Any(list => list.Count > 0);
        }
        else if (graph.Representation == Graph.RepresentationType.AdjacencyMatrix)
        {
            hasEdges = Enumerable.Range(0, graph.V)
                .Any(i => Enumerable.Range(0, graph.V)
                    .Any(j => graph.adjacencyMatrix[i, j] > 0));
        }

        if (!hasEdges)
        {
            Console.WriteLine("Задача комівояжера не може бути вирішена, оскільки граф не має ребер.");
            return false;
        }

        // Перевірка зв'язності графу
        bool[] visited = new bool[graph.V];
        DFS(graph, 0, visited);

        if (visited.All(v => v))
        {
            Console.WriteLine("Задача комівояжера може бути вирішена.");
            return true;
        }
        else
        {
            Console.WriteLine("Задача комівояжера не може бути вирішена, оскільки граф не є зв'язним.");
            return false;
        }
    }

    private static void DFS(Graph graph, int vertex, bool[] visited)
    {
        // Додаткова перевірка наявності adj
        if (graph.Representation == Graph.RepresentationType.AdjacencyList && graph.adj != null)
        {
            visited[vertex] = true;

            foreach (var edge in graph.adj[vertex])
            {
                int neighbor = edge.Item1;
                if (!visited[neighbor])
                {
                    DFS(graph, neighbor, visited);
                }
            }
        }
    }
}
