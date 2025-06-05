using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Program

//1. Разработать функцию, которая генерирует граф (неориентированный)
//одним из способов представления графов:
//•	матрица смежности;	
//• матрица инцидентности;
//• список смежности;
//• список рёбер. 
//Вершины графа обозначать целыми числами(уникальными).
//
{
    static Random rnd = new Random();

    static void Main()
    {
        Console.WriteLine("Введите число вершин:");
        int vertices = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите число рёбер:");
        int edges = int.Parse(Console.ReadLine());

        Console.WriteLine("Выберите способ представления (1 - матрица, 2 - матрица инцидентности, 3 - список смежности, 4 - список рёбер):");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            string text = GenerateAdjacencyMatrix(vertices, edges);
            SaveToFile("graph.txt", text);
        }
        else if (choice == "2")
        {
            string text = GenerateIncidenceMatrix(vertices, edges);
            SaveToFile("graph.txt", text);
        }
        else if (choice == "3")
        {
            string text = GenerateAdjacencyList(vertices, edges);
            SaveToFile("graph.txt", text);
        }
        else if (choice == "4")
        {
            string text = GenerateEdgeList(vertices, edges);
            SaveToFile("graph.txt", text);
        }
        else
        {
            Console.WriteLine("Такого способа нет");
        }
    }

    static string GenerateAdjacencyMatrix(int vCount, int eCount)
    {
        int[,] matrix = new int[vCount, vCount];
        for (int i = 0; i < eCount; i++)
        {
            int a, b;
            do
            {
                a = rnd.Next(vCount);
                b = rnd.Next(vCount);
            } while (a == b || matrix[a, b] == 1);

            matrix[a, b] = 1;
            matrix[b, a] = 1;
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < vCount; i++)
        {
            for (int j = 0; j < vCount; j++)
            {
                sb.Append(matrix[i, j] + " ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    static string GenerateIncidenceMatrix(int vCount, int eCount)
    {
        int[,] matrix = new int[vCount, eCount];
        for (int i = 0; i < eCount; i++)
        {
            int a, b;
            do
            {
                a = rnd.Next(vCount);
                b = rnd.Next(vCount);
            } while (a == b || matrix[a, i] == 1 || matrix[b, i] == 1);

            matrix[a, i] = 1;
            matrix[b, i] = 1;
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < vCount; i++)
        {
            for (int j = 0; j < eCount; j++)
                sb.Append(matrix[i, j] + " ");
            sb.AppendLine();
        }
        return sb.ToString();
    }

    static string GenerateAdjacencyList(int vCount, int eCount)
    {
        List<int>[] adj = new List<int>[vCount];
        for (int i = 0; i < vCount; i++) adj[i] = new List<int>();

        for (int i = 0; i < eCount; i++)
        {
            int a, b;
            do
            {
                a = rnd.Next(vCount);
                b = rnd.Next(vCount);
            } while (a == b || adj[a].Contains(b));

            adj[a].Add(b);
            adj[b].Add(a);
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < vCount; i++)
        {
            sb.Append(i + ": " + string.Join(", ", adj[i]));
            sb.AppendLine();
        }

        return sb.ToString();
    }

    static string GenerateEdgeList(int vCount, int eCount)
    {
        HashSet<(int, int)> edges = new HashSet<(int, int)>();

        while (edges.Count < eCount)
        {
            int a = rnd.Next(vCount);
            int b;
            do
            {
                b = rnd.Next(vCount);
            } while (a == b || edges.Contains((a, b)) || edges.Contains((b, a)));

            edges.Add((a, b));
        }

        StringBuilder sb = new StringBuilder();
        foreach (var e in edges)
        {
            sb.AppendLine($"{e.Item1} - {e.Item2}");
        }
        return sb.ToString();
    }

    static void SaveToFile(string filename, string content)
    {
        File.WriteAllText(filename, content);
        Console.WriteLine("Граф сохранён в файл " + filename);
    }
}
