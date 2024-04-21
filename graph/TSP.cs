using System;
using System.Collections.Generic;
using graph;

public static class BruteForceTSP
{
    public static List<int> BfTravellingSalesman(Graph graph)
    {
        // Шукання вершини з мінімальними відстанями до всіх інших вершин.
        int startVertex = FindMostOptimalStartVertex(graph);

        // Груба сила.
        // Генерація всіх можливих шляхів, починаючи з вигідної вершини.
        List<List<int>> allPossiblePaths = FindAllPaths(graph, startVertex);

        // Фільтрація шляхів, які не є циклами.
        List<List<int>> allPossibleCycles = allPossiblePaths.FindAll(path =>
        {
            int lastVertex = path[path.Count - 1];
            return graph.adj[lastVertex].Exists(edge => edge.Item1 == startVertex);
        });

        // Проходження усіх можливих циклів і вибір того, який має мінімальну вагу.
        int[,] adjacencyMatrix = graph.adjacencyMatrix;
        List<int> salesmanPath = new List<int>();
        int? salesmanPathWeight = null;

        foreach (List<int> currentCycle in allPossibleCycles)
        {
            int currentCycleWeight = GetCycleWeight(adjacencyMatrix, currentCycle);

            // Якщо вага поточного циклу менша за попередні, вважати поточний цикл найоптимальнішим.
            if (salesmanPathWeight == null || currentCycleWeight < salesmanPathWeight)
            {
                salesmanPath = currentCycle;
                salesmanPathWeight = currentCycleWeight;
            }
        }

        // Повернення рішення.
        return salesmanPath;
    }

    private static int FindMostOptimalStartVertex(Graph graph)
    {
        int mostOptimalVertex = 0;
        int minDistance = int.MaxValue;

        for (int i = 0; i < graph.V; i++)
        {
            int totalDistance = 0;
            for (int j = 0; j < graph.V; j++)
            {
                totalDistance += graph.adjacencyMatrix[i, j];
            }

            if (totalDistance < minDistance)
            {
                minDistance = totalDistance;
                mostOptimalVertex = i;
            }
        }

        return mostOptimalVertex;
    }

    private static List<List<int>> FindAllPaths(Graph graph, int startVertex, List<List<int>> paths = null, List<int> path = null)
    {
        if (paths == null)
        {
            paths = new List<List<int>>();
            path = new List<int>();
        }

        List<int> currentPath = new List<int>(path);
        currentPath.Add(startVertex);

        HashSet<int> visitedSet = new HashSet<int>(currentPath);

        List<int> unvisitedNeighbors = new List<int>();
        foreach (var edge in graph.adj[startVertex])
        {
            int neighbor = edge.Item1;
            if (!visitedSet.Contains(neighbor))
            {
                unvisitedNeighbors.Add(neighbor);
            }
        }

        if (unvisitedNeighbors.Count == 0)
        {
            paths.Add(currentPath);
            return paths;
        }

        foreach (int currentUnvisitedNeighbor in unvisitedNeighbors)
        {
            FindAllPaths(graph, currentUnvisitedNeighbor, paths, currentPath);
        }

        return paths;
    }

    private static int GetCycleWeight(int[,] adjacencyMatrix, List<int> cycle)
    {
        int weight = 0;

        for (int cycleIndex = 1; cycleIndex < cycle.Count; cycleIndex++)
        {
            int fromVertex = cycle[cycleIndex - 1];
            int toVertex = cycle[cycleIndex];
            weight += adjacencyMatrix[fromVertex, toVertex];
        }

        return weight;
    }
}
