using System;
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

        public int V; // Кількість вершин
        public int E; // Кількість ребер
        public List<Tuple<int, int>>[] adj; // Список суміжності
        public int[,] adjacencyMatrix; // Матриця суміжності
        public RepresentationType Representation;

        // Конструктор для створення графу з v вершинами та щільністю density
        public Graph(int v, double density, RepresentationType representation)
        {
            V = v;
            Representation = representation;
            E = 0;

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
            GenerateRandomEdges(density);
        }

        public void AddEdge(int u, int v, int weight)
        {
            if (Representation == RepresentationType.AdjacencyList)
            {
                adj[u].Add(new Tuple<int, int>(v, weight));
                adj[v].Add(new Tuple<int, int>(u, weight)); // Для неорієнтованого графу додаємо обидва напрямки
            }
            else if (Representation == RepresentationType.AdjacencyMatrix)
            {
                adjacencyMatrix[u, v] = weight;
                adjacencyMatrix[v, u] = weight;
            }

            E++; // Збільшуємо кількість ребер при додаванні нового ребра
        }

        private void UpdateAdjacencyMatrix()
        {
            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    if (i != j)
                    {
                        adjacencyMatrix[i, j] = 0; // Якщо вершина не з'єднана, вага ребра - 0
                    }
                }
            }
        }

        // Метод для генерації зваженого неорієнтованого повного графу за заданою щільністю
        private void GenerateRandomGraph(double density)
        {
            Random random = new Random();
            // Створюємо граф з рандомними вагами
            for (int i = 1; i < V; i++)
            {
                int weight = random.Next(1, 101); // Генерація ваги ребра (від 1 до 100)
                int randomVertex = random.Next(0, i); // Вибираємо випадкову вершину, до якої буде додано ребро
                AddEdge(i, randomVertex, weight); // Додаємо ребро між поточною вершиною та випадково обраною
            }
        }

        private void GenerateRandomEdges(double density)
        {
            Random random = new Random();

            for (int i = 0; i < V; i++)
            {
                for (int j = i + 1; j < V; j++)
                {
                    if (i != j && random.NextDouble() < density && adjacencyMatrix[i, j] == 0)
                    {
                        int weight = random.Next(1, 101); // Генерація ваги ребра (від 1 до 100)
                        AddEdge(i, j, weight); // Додавання ребра між вершинами i та j
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
