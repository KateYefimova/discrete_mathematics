using graph;

class Program
{
    static void Main(string[] args)
    {
        int V = 5; // Кількість вершин
        double density = 1; // Щільність
        Graph graph = new Graph(V, density);

        // Вивід графу
        graph.PrintAdjacencyLists();
        Console.WriteLine();
        graph.PrintAdjacencyMatrix();
        
    }
}
