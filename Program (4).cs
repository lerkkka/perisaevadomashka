using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Создаем список рёбер
        List<string> edges = new List<string>();

        Console.WriteLine("Введите количество рёбер");
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите рёбра через пробел: ");
        for (int i = 0; i < n; i++)
        {
            string line = Console.ReadLine();
            edges.Add(line);
        }

        // Создаем список всех вершин
        List<int> allVertices = new List<int>();
        foreach (string e in edges)
        {
            string[] parts = e.Split(' ');
            int a = int.Parse(parts[0]);
            int b = int.Parse(parts[1]);
            if (!allVertices.Contains(a))
                allVertices.Add(a);
            if (!allVertices.Contains(b))
                allVertices.Add(b);
        }

        // Проверяем, связен ли граф
        bool connected = true;
        for (int i = 0; i < allVertices.Count; i++)
        {
            for (int j = 0; j < allVertices.Count; j++)
            {
                if (i != j)
                {
                    if (!IsPath(allVertices[i], allVertices[j], edges))
                    {
                        connected = false;
                        break;
                    }
                }
            }
            if (!connected)
                break;
        }
        Console.WriteLine("Граф " + (connected ? "связный" : "несвязный"));

        // Спрашиваем, откуда и куда искать путь
        Console.WriteLine("От какой вершины обход графа: ");
        int start = int.Parse(Console.ReadLine());
        Console.WriteLine("До какой вершины продолжается обход графа: ");
        int end = int.Parse(Console.ReadLine());

        // Ищем путь
        List<int> path = FindPath(start, end, edges);

        if (path == null)
            Console.WriteLine("Пути нет");
        else
            Console.WriteLine("Путь: " + string.Join(" ", path));
    }

    static bool IsPath(int a, int b, List<string> edges)
    {
        // Проверяем есть ли путь между двумя вершинами
        foreach (string e in edges)
        {
            string[] parts = e.Split(' ');
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            if ((x == a && y == b) || (x == b && y == a))
                return true;
        }
        return false;
    }

    static List<int> FindPath(int start, int end, List<string> edges)
    {
        // Просто ищем путь в ширину
        Queue<int> q = new Queue<int>();
        Dictionary<int, int> parent = new Dictionary<int, int>();

        q.Enqueue(start);

        HashSet<int> visited = new HashSet<int>();
        visited.Add(start);

        while (q.Count > 0)
        {
            int current = q.Dequeue();
            if (current == end)
                break;

            foreach (string e in edges)
            {
                string[] parts = e.Split(' ');
                int x = int.Parse(parts[0]);
                int y = int.Parse(parts[1]);

                if (x == current && !visited.Contains(y))
                {
                    visited.Add(y);
                    parent[y] = current;
                    q.Enqueue(y);
                }
                else if (y == current && !visited.Contains(x))
                {
                    visited.Add(x);
                    parent[x] = current;
                    q.Enqueue(x);
                }
            }
        }

        if (!parent.ContainsKey(end))
            return null;

        List<int> path = new List<int>();

        for (int v = end; v != start; v = parent[v])
            path.Add(v);

        path.Add(start);

        path.Reverse();

        return path;
    }
}