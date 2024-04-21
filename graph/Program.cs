using System;
using System.Collections.Generic;
using System.Diagnostics;
using graph;


class Program
{
    static void Main(string[] args)
    {
        int V = 10; // Кількість вершин
        double density = 1; // Щільність
        Graph graph = new Graph(V, density, Graph.RepresentationType.AdjacencyMatrix);

        // Вивід графу
        graph.PrintAdjacencyMatrix();
        Console.WriteLine();

        // Таймер для вимірювання часу виконання алгоритму
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Виклик алгоритму TSP
        List<int> tspPath = BruteForceTSP.BfTravellingSalesman(graph);

        // Зупинка таймера після виконання алгоритму
        stopwatch.Stop();

        // Вивід алгоритму найближчих сусідів
        Console.WriteLine("Travelling Salesman Path:");
        foreach (int vertex in tspPath)
        {
            Console.Write(vertex + " -> ");
        }
        Console.WriteLine(tspPath[0]); // Закінчення циклу з початковою вершиною

        // Вивід часу виконання алгоритму
        Console.WriteLine("Algorithm execution time: " + stopwatch.ElapsedMilliseconds + " ms");
    }
}