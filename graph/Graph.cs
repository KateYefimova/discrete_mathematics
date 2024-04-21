namespace graph;

public class Graph
{
    public int V; // Кількість вершин
    public List<Tuple<int, int>>[] adj; // Список суміжності
    public int[,] adjacencyMatrix; // Матриця суміжності

    // Конструктор для створення графу з v вершинами та щільністю density
    public Graph(int v, double density)
    {
        V = v;
        adj = new List<Tuple<int, int>>[V];
        adjacencyMatrix = new int[V, V];
        for (int i = 0; i < V; i++)
        {
            adj[i] = new List<Tuple<int, int>>();
        }
        GenerateRandomGraph(density);
    }
    public void AddEdge(int u, int v, int weight)
    {
        adj[u].Add(new Tuple<int, int>(v, weight));
        adj[v].Add(new Tuple<int, int>(u, weight)); // Для неорієнтованого графу додаємо обидва напрямки
        adjacencyMatrix[u, v] = weight;
        adjacencyMatrix[v, u] = weight;
    }
    
    private void UpdateAdjacencyMatrix()
    {
        for (int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                if (i != j)
                {
                    if (adj[i].Exists(x => x.Item1 == j))
                    {
                        adjacencyMatrix[i, j] = adj[i].Find(x => x.Item1 == j).Item2;
                    }
                    else
                    {
                        adjacencyMatrix[i, j] = 0; // Якщо вершина не з'єднана, вага ребра - 0
                    }
                }
            }
        }
    }
    // Метод для генерації зваженого неорієнтованого повного графу за заданою щільністю
    private void GenerateRandomGraph(double density)
    {
        Random random = new Random();

        for (int i = 0; i < V; i++)
        {
            for (int j = i+1; j < V; j++)
            {
                if (i != j && random.NextDouble() < density) // Виключив можливість додавання ребра від вершини до самої себе
                {
                    int weight = random.Next(1, 101); // Генерація ваги ребра (від 1 до 100)
                    AddEdge(i, j, weight); // Додавання ребра між вершинами i та j
                }
            }
        }
        UpdateAdjacencyMatrix();
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