using System;
using System.Collections.Generic;
using System.Linq;
using graph;

public static class BruteForceTSP
{
    public static List<int> BfTravellingSalesman(Graph graph)
    {
        // Шукання вершини з мінімальними відстанями до всіх інших вершин.
        int startVertex = FindMostOptimalStartVertex(graph);
        
        // Генерація всіх можливих шляхів, починаючи з вигідної вершини.
        List<List<int>> allPossiblePaths = FindAllPaths(graph, startVertex);

        // Фільтрація шляхів, які не є циклами.
        List<List<int>> allPossibleCycles = allPossiblePaths.FindAll(path =>
        {
            int lastVertex = path[path.Count - 1];
            if (graph.Representation == Graph.RepresentationType.AdjacencyList)
                return graph.adj[lastVertex].Exists(edge => edge.Item1 == startVertex);
            else
                return graph.adjacencyMatrix[lastVertex, startVertex] > 0;
        });

        // Проходження усіх можливих циклів і вибір того, який має мінімальну вагу.
        List<int> salesmanPath = new List<int>();
        int? salesmanPathWeight = null;

        foreach (List<int> currentCycle in allPossibleCycles)
        {
            int currentCycleWeight = GetCycleWeight(graph, currentCycle);

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

    // Знаходження оптимальної вершини початку - обираємо вершину
    // з якої йдуть найменшої веги ребра до усіх інших вершин
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
        if (graph.Representation == Graph.RepresentationType.AdjacencyList)
        {
            foreach (var edge in graph.adj[startVertex])
            {
                int neighbor = edge.Item1;
                if (!visitedSet.Contains(neighbor))
                {
                    unvisitedNeighbors.Add(neighbor);
                }
            }
        }
        else if (graph.Representation == Graph.RepresentationType.AdjacencyMatrix)
        {
            for (int i = 0; i < graph.V; i++)
            {
                if (graph.adjacencyMatrix[startVertex, i] > 0 && !visitedSet.Contains(i))
                {
                    unvisitedNeighbors.Add(i);
                }
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

    private static int GetCycleWeight(Graph graph, List<int> cycle)
    {
        int weight = 0;

        if (graph.Representation == Graph.RepresentationType.AdjacencyList)
        {
            for (int cycleIndex = 1; cycleIndex < cycle.Count; cycleIndex++)
            {
                int fromVertex = cycle[cycleIndex - 1];
                int toVertex = cycle[cycleIndex];
                foreach (var edge in graph.adj[fromVertex])
                {
                    if (edge.Item1 == toVertex)
                    {
                        weight += edge.Item2;
                        break;
                    }
                }
            }
        }
        else if (graph.Representation == Graph.RepresentationType.AdjacencyMatrix)
        {
            for (int cycleIndex = 1; cycleIndex < cycle.Count; cycleIndex++)
            {
                int fromVertex = cycle[cycleIndex - 1];
                int toVertex = cycle[cycleIndex];
                weight += graph.adjacencyMatrix[fromVertex, toVertex];
            }
        }

        return weight;
    }
}
