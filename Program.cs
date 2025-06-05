using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace последовательности
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Задача 1: Наибольшая общая подпоследовательность (НОП)");
            // Ввод двух строк для поиска НОП
            Console.Write("Введите первую строку: ");
            string str1 = Console.ReadLine();
            Console.Write("Введите вторую строку: ");
            string str2 = Console.ReadLine();

            string lcs = LongestCommonSubsequence(str1, str2);
            Console.WriteLine($"Наибольшая общая подпоследовательность: {lcs}");
            Console.WriteLine();

            Console.WriteLine("Задача 2: Наибольшая возрастающая подпоследовательность (НВП)");
            // Ввод последовательности чисел через запятую
            Console.Write("Введите последовательность чисел через запятую (например, 10,22,9,33): ");
            string inputSequence = Console.ReadLine();
            int[] sequence = Array.ConvertAll(inputSequence.Split(','), s => int.Parse(s.Trim()));

            List<int> lis = LongestIncreasingSubsequence(sequence);
            Console.WriteLine($"Наибольшая возрастающая подпоследовательность: {string.Join(", ", lis)}");
            Console.WriteLine();

            Console.WriteLine("Задача 3: Расстояние Левенштейна");
            // Ввод двух строк для вычисления расстояния
            Console.Write("Введите первую строку: ");
            string s1 = Console.ReadLine();
            Console.Write("Введите вторую строку: ");
            string s2 = Console.ReadLine();

            int distance = LevenshteinDistance(s1, s2);
            Console.WriteLine($"Расстояние Левенштейна между \"{s1}\" и \"{s2}\": {distance}");
        }

        // Метод для поиска НОП (LCS) двух строк с помощью динамического программирования
        static string LongestCommonSubsequence(string s1, string s2)
        {
            int m = s1.Length;
            int n = s2.Length;
            int[,] dp = new int[m + 1, n + 1];

            // Заполняем таблицу dp значениями длины НОП
            for (int i = 0; i <= m; i++)
                dp[i, 0] = 0;
            for (int j = 0; j <= n; j++)
                dp[0, j] = 0;

            for (int i = 1; i <= m; i++)
                for (int j = 1; j <= n; j++)
                    if (s1[i - 1] == s2[j - 1])
                        dp[i, j] = dp[i - 1, j - 1] + 1; // символ совпал — увеличиваем длину
                    else
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]); // выбираем максимум из соседних вариантов

            // Восстановление НОП по таблице
            int index = dp[m, n];
            char[] lcsChars = new char[index];
            int iIndex = m;
            int jIndex = n;

            while (iIndex > 0 && jIndex > 0)
            {
                if (s1[iIndex - 1] == s2[jIndex - 1])
                {
                    lcsChars[index - 1] = s1[iIndex - 1];
                    iIndex--;
                    jIndex--;
                    index--;
                }
                else if (dp[iIndex - 1, jIndex] > dp[iIndex, jIndex - 1])
                    iIndex--;
                else
                    jIndex--;
            }

            return new string(lcsChars);
        }

        // Метод для поиска НВП (LIS) последовательности чисел с помощью динамического программирования
        static List<int> LongestIncreasingSubsequence(int[] sequence)
        {
            int n = sequence.Length;
            int[] lengths = new int[n]; // длины НВП заканчивающихся в каждом элементе
            int[] predecessors = new int[n]; // для восстановления последовательности

            for (int i = 0; i < n; i++)
                lengths[i] = 1; // минимальная длина — каждый элемент сам по себе

            for (int i = 0; i < n; i++)
                predecessors[i] = -1;

            int maxLength = 0;
            int maxIndex = -1;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < i; j++)
                    if (sequence[j] < sequence[i] && lengths[j] + 1 > lengths[i])
                    {
                        lengths[i] = lengths[j] + 1;
                        predecessors[i] = j;
                    }

            // Находим индекс конца самой длинной НВП
            for (int i = 0; i < n; i++)
                if (lengths[i] > maxLength)
                {
                    maxLength = lengths[i];
                    maxIndex = i;
                }

            // Восстановление НВП по предшественникам
            List<int> lis = new List<int>();
            while (maxIndex != -1)
            {
                lis.Add(sequence[maxIndex]);
                maxIndex = predecessors[maxIndex];
            }
            lis.Reverse(); // порядок восстанавливается в обратную сторону

            return lis;
        }
        static int LevenshteinDistance(string s, string t)
        {
            int m = s.Length;
            int n = t.Length;
            int[,] d = new int[m + 1, n + 1];

            // Инициализация границ матрицы
            for (int i = 0; i <= m; i++) d[i, 0] = i;
            for (int j = 0; j <= n; j++) d[0, j] = j;

            for (int i = 1; i <= m; i++)
                for (int j = 1; j <= n; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1; // если символы совпадают, то замена нулевая

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            return d[m, n];
        }
    }
}
