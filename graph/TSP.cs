using System;
using System.Collections.Generic;
using System.Linq;
using graph;

public static class BruteForceTSP
{
    public static List<int> BfTravellingSalesman(Graph graph)
    {
        int startVertex = FindMostOptimalStartVertex(graph);
        List<List<int>> allPossiblePaths = FindAllPaths(graph, startVertex);
        List<List<int>> allPossibleCycles = FindCycles(graph, allPossiblePaths, startVertex);
        
        return GetOptimalCycle(graph, allPossibleCycles);
    }

    private static int FindMostOptimalStartVertex(Graph graph)
    {
        int mostOptimalVertex = 0;
        int minDistance = int.MaxValue;
        int[] distances = new int[graph.V]; // Cache for total distances

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

            distances[i] = totalDistance; // Cache the total distance

            if (totalDistance < minDistance)
            {
                minDistance = totalDistance;
                mostOptimalVertex = i;
            }
        }

        return mostOptimalVertex;
    }

    private static List<List<int>> FindAllPaths(Graph graph, int startVertex)
    {
        List<List<int>> paths = new List<List<int>>();
        HashSet<int> visitedSet = new HashSet<int>();

        FindPathsDFS(graph, startVertex, new List<int>(), visitedSet, paths);

        return paths;
    }

    private static void FindPathsDFS(Graph graph, int currentVertex, List<int> currentPath, HashSet<int> visitedSet, List<List<int>> paths)
    {
        currentPath.Add(currentVertex);
        visitedSet.Add(currentVertex);

        List<int> unvisitedNeighbors = GetUnvisitedNeighbors(graph, currentVertex, visitedSet);

        if (unvisitedNeighbors.Count == 0)
        {
            paths.Add(new List<int>(currentPath));
        }
        else
        {
            foreach (int neighbor in unvisitedNeighbors)
            {
                FindPathsDFS(graph, neighbor, currentPath, visitedSet, paths);
            }
        }

        visitedSet.Remove(currentVertex);
        currentPath.RemoveAt(currentPath.Count - 1);
    }

    private static List<int> GetUnvisitedNeighbors(Graph graph, int vertex, HashSet<int> visitedSet)
    {
        List<int> unvisitedNeighbors = new List<int>();

        if (graph.Representation == Graph.RepresentationType.AdjacencyList)
        {
            foreach (var edge in graph.adj[vertex])
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
                if (graph.adjacencyMatrix[vertex, i] > 0 && !visitedSet.Contains(i))
                {
                    unvisitedNeighbors.Add(i);
                }
            }
        }

        return unvisitedNeighbors;
    }

    private static List<List<int>> FindCycles(Graph graph, List<List<int>> paths, int startVertex)
    {
        List<List<int>> cycles = new List<List<int>>();

        foreach (List<int> path in paths)
        {
            int lastVertex = path[path.Count - 1];
            if ((graph.Representation == Graph.RepresentationType.AdjacencyList && graph.adj[lastVertex].Exists(edge => edge.Item1 == startVertex))
                || (graph.Representation == Graph.RepresentationType.AdjacencyMatrix && graph.adjacencyMatrix[lastVertex, startVertex] > 0))
            {
                cycles.Add(path);
            }
        }

        return cycles;
    }

    private static List<int> GetOptimalCycle(Graph graph, List<List<int>> cycles)
    {
        List<int> optimalCycle = new List<int>();
        int? minWeight = null;

        foreach (List<int> cycle in cycles)
        {
            int weight = CalculateCycleWeight(graph, cycle);
            if (!minWeight.HasValue || weight < minWeight)
            {
                minWeight = weight;
                optimalCycle = cycle;
            }
        }

        return optimalCycle;
    }

    private static int CalculateCycleWeight(Graph graph, List<int> cycle)
    {
        int weight = 0;

        for (int i = 1; i < cycle.Count; i++)
        {
            int fromVertex = cycle[i - 1];
            int toVertex = cycle[i];
            weight += graph.Representation == Graph.RepresentationType.AdjacencyList ?
                      graph.adj[fromVertex].First(edge => edge.Item1 == toVertex).Item2 :
                      graph.adjacencyMatrix[fromVertex, toVertex];
        }

        // Add weight of the last edge to complete the cycle
        weight += graph.Representation == Graph.RepresentationType.AdjacencyList ?
                  graph.adj[cycle[cycle.Count - 1]].First(edge => edge.Item1 == cycle[0]).Item2 :
                  graph.adjacencyMatrix[cycle[cycle.Count - 1], cycle[0]];

        return weight;
    }
}