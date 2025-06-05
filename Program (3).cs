using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Ввод алфавита (Пункт 1)
        Console.WriteLine("Введите алфавит:");
        string alphabet = Console.ReadLine();
        var order = new Dictionary<char, int>();
        for (int i = 0; i < alphabet.Length; i++) order[alphabet[i]] = i;

        // Ввод двух строк для сравнения 
        Console.WriteLine("Введите первую строку:");
        string s1 = Console.ReadLine();
        Console.WriteLine("Введите вторую строку:");
        string s2 = Console.ReadLine();

        // Сравнение строк по алфавиту
        Console.WriteLine($"Сравнение: {Compare(s1, s2, order)}");

        // Ввод текста и подстроки для поиска 
        Console.WriteLine("Введите текст:");
        string text = Console.ReadLine();
        Console.WriteLine("Введите подстроку:");
        string pattern = Console.ReadLine();

        // Наивный поиск
        Console.WriteLine($"Наивный: {NaiveSearch(text, pattern)}");

        // Предварительная обработка для алгоритма КМП
        int[] lps = ComputeLPS(pattern);

        // Алгоритм КМП
        Console.WriteLine($"КМП: {KMP(text, pattern, lps)}");

        // Алгоритм Бойера-Мура
        Console.WriteLine($"Бойер-Мур: {BoyerMoore(text, pattern)}");
    }

    // Пункт 2: сравнение двух строк по алфавиту
    static int Compare(string a, string b, Dictionary<char, int> order)
    {
        int len = Math.Min(a.Length, b.Length);
        for (int i = 0; i < len; i++)
            if (order.ContainsKey(a[i]) && order.ContainsKey(b[i]))
            {
                if (order[a[i]] != order[b[i]])
                    return order[a[i]] - order[b[i]];
            }
            else
                return a[i] - b[i]; // если символ не в алфавите
        return a.Length - b.Length;
    }

    // наивный поиск подстроки в тексте без встроенных методов
    static int NaiveSearch(string text, string pattern)
    {
        for (int i = 0; i <= text.Length - pattern.Length; i++)
        {
            bool match = true;
            for (int j = 0; j < pattern.Length; j++)
                if (text[i + j] != pattern[j])
                {
                    match = false;
                    break;
                }
            if (match) return i;
        }
        return -1;
    }

    //вычисление массива lps для алгоритма КМП
    static int[] ComputeLPS(string pattern)
    {
        int[] lps = new int[pattern.Length];
        int len = 0, i = 1;
        while (i < pattern.Length)
        {
            if (pattern[i] == pattern[len])
            {
                len++;
                lps[i] = len;
                i++;
            }
            else
            {
                if (len != 0) len = lps[len - 1];
                else { lps[i] = 0; i++; }
            }
        }
        return lps;
    }

    //алгоритм КМП поиска подстроки
    static int KMP(string text, string pattern, int[] lps)
    {
        int i = 0, j = 0;
        while (i < text.Length)
        {
            if (text[i] == pattern[j])
            {
                i++; j++;
                if (j == pattern.Length) return i - j; // найдено в позиции i-j
            }
            else
            {
                if (j != 0) j = lps[j - 1];
                else i++;
            }
        }
        return -1; // не найдено
    }

    // алгоритм Бойера-Мура поиска подстроки без встроенных методов
    static int BoyerMoore(string text, string pattern)
    {
        var badChar = new Dictionary<char, int>();

        // Построение таблицы "плохого символа"
        for (int i = 0; i < pattern.Length; i++) badChar[pattern[i]] = i;

        int shift = 0;

        while (shift <= text.Length - pattern.Length)
        {
            int j = pattern.Length - 1;

            // Сравнение с конца шаблона
            while (j >= 0 && text[shift + j] == pattern[j]) j--;

            if (j < 0) return shift; // найдено

            char c = text[shift + j];

            int badShift = badChar.ContainsKey(c) ? badChar[c] : -1;

            shift += Math.Max(1, j - badShift);
        }

        return -1; // не найдено
    }
}