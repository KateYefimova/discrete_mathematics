using graph;


class Program
{
    static void Main(string[] args)
    {
        int V = 5; // Кількість вершин
        double density = 1; // Щільність
        Graph graph = new Graph(V, density, Graph.RepresentationType.AdjacencyList);

        // Вивід графу
        graph.PrintAdjacencyLists();
        Console.WriteLine();
        //graph.PrintAdjacencyMatrix();
        //Console.WriteLine();
        //Console.WriteLine();
        
        List<int> tspPath = BruteForceTSP.BfTravellingSalesman(graph);

        // Вивід алгоритму найближчих сусідів
        Console.WriteLine("Travelling Salesman Path:");
        foreach (int vertex in tspPath)
        {
            Console.Write(vertex + " -> ");
        }
        Console.WriteLine(tspPath[0]); // Закінчення циклу з початковою вершиною
    }
}