using System;
using System.Collections.Generic;

namespace краснчерн_дерево
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = new RedBlackTree();

            Console.WriteLine("Доступные команды: добавить, удалить, вывести, инфиксный, префиксный, постфиксный, выход");

            while (true)
            {
                Console.Write("\nВведите команду: ");
                string команда = Console.ReadLine();

                if (команда == "добавить")
                {
                    Console.Write("Ключ: ");
                    int key = int.Parse(Console.ReadLine());
                    Console.Write("Значение: ");
                    string value = Console.ReadLine();
                    tree.Insert(key, value);
                    Console.WriteLine("Добавлено");
                }
                else if (команда == "удалить")
                {
                    Console.Write("Ключ для удаления: ");
                    int key = int.Parse(Console.ReadLine());
                    tree.Delete(key);
                    Console.WriteLine("Удалено");
                }
                else if (команда == "вывести")
                {
                    var dict = tree.ToDictionary();
                    Console.WriteLine("Дерево в виде словаря:");
                    foreach (var kvp in dict)
                        Console.WriteLine($"{{{kvp.Key}: {kvp.Value}}}");
                }
                else if (команда == "инфиксный")
                {
                    Console.WriteLine("Инфиксный обход:");
                    tree.InOrder();
                }
                else if (команда == "префиксный")
                {
                    Console.WriteLine("Префиксный обход:");
                    tree.PreOrder();
                }
                else if (команда == "постфиксный")
                {
                    Console.WriteLine("Постфиксный обход:");
                    tree.PostOrder();
                }
                else if (команда == "выход")
                {
                    break;
                }
            }
        }
    }

    enum Color { Red, Black }

    class Node
    {
        public int Key;
        public string Value;
        public Node Left;
        public Node Right;
        public Color Color;

        public Node(int key, string value)
        {
            Key = key;
            Value = value;
            Color = Color.Red; // новые узлы всегда красные при вставке
        }
    }

    class RedBlackTree
    {
        private Node root;

        // Вставка с балансировкой
        public void Insert(int key, string value)
        {
            root = InsertRecursive(root, key, value);
            root.Color = Color.Black; // корень всегда черный
        }

        private Node InsertRecursive(Node node, int key, string value)
        {
            if (node == null)
                return new Node(key, value);

            if (key < node.Key)
                node.Left = InsertRecursive(node.Left, key, value);
            else if (key > node.Key)
                node.Right = InsertRecursive(node.Right, key, value);
            else
            {
                // Обновляем значение если ключ уже есть
                node.Value = value;
            }

            // Балансировка после вставки
            if (IsRed(node.Right) && !IsRed(node.Left))
                node = RotateLeft(node);

            if (IsRed(node.Left) && IsRed(node.Left.Left))
                node = RotateRight(node);

            if (IsRed(node.Left) && IsRed(node.Right))
                FlipColors(node);

            return node;
        }

        // Удаление с балансировкой
        public void Delete(int key)
        {
            if (root == null) return;

            root = DeleteRecursive(root, key);
            if (root != null)
                root.Color = Color.Black; // корень всегда черный
        }

        private Node DeleteRecursive(Node h, int key)
        {
            if (h == null) return null;

            if (key < h.Key)
            {
                h.Left = DeleteRecursive(h.Left, key);
            }
            else if (key > h.Key)
            {
                h.Right = DeleteRecursive(h.Right, key);
            }
            else
            {
                // нашли узел для удаления
                if (h.Right == null) return h.Left;
                if (h.Left == null) return h.Right;

                // замена на минимальный узел справа
                Node minNode = Min(h.Right);
                h.Key = minNode.Key;
                h.Value = minNode.Value;
                h.Right = DeleteMin(h.Right);
            }

            return Balance(h);
        }

        private Node DeleteMin(Node h)
        {
            if (h.Left == null) return h.Right;

            h.Left = DeleteMin(h.Left);
            return Balance(h);
        }

        private Node Min(Node h)
        {
            while (h.Left != null) h = h.Left;
            return h;
        }

        private bool IsRed(Node node)
        {
            return node != null && node.Color == Color.Red;
        }

        private Node RotateLeft(Node h)
        {
            Node x = h.Right;
            h.Right = x.Left;
            x.Left = h;

            x.Color = h.Color;
            h.Color = Color.Red;

            return x;
        }

        private Node RotateRight(Node h)
        {
            Node x = h.Left;

            h.Left = x.Right;
            x.Right = h;

            x.Color = h.Color;
            h.Color = Color.Red;

            return x;
        }

        private void FlipColors(Node h)
        {
            h.Color = Color.Red;
            if (h.Left != null) h.Left.Color = Color.Black;
            if (h.Right != null) h.Right.Color = Color.Black;
        }

        private Node Balance(Node h)
        {
            // балансировка после удаления или вставки

            if (IsRed(h.Right) && !IsRed(h.Left))
                h = RotateLeft(h);

            if (IsRed(h.Left) && IsRed(h.Left.Left))
                h = RotateRight(h);

            if (IsRed(h.Left) && IsRed(h.Right))
                FlipColors(h);

            return h;
        }

        public Dictionary<int, string> ToDictionary()
        {
            var dict = new Dictionary<int, string>();
            InOrderTraversal(root, dict);
            return dict;
        }

        private void InOrderTraversal(Node node, Dictionary<int, string> dict)
        {
            if (node == null) return;

            InOrderTraversal(node.Left, dict);
            dict[node.Key] = node.Value;
            InOrderTraversal(node.Right, dict);
        }

        public void InOrder()
        {
            InOrderTraversalPrint(root);
            Console.WriteLine();
        }

        private void InOrderTraversalPrint(Node node)
        {
            if (node == null) return;

            InOrderTraversalPrint(node.Left);
            Console.Write($"({node.Key},{node.Value}) ");
            InOrderTraversalPrint(node.Right);
        }

        public void PreOrder()
        {
            PreOrderTraversalPrint(root);
            Console.WriteLine();
        }

        private void PreOrderTraversalPrint(Node node)
        {
            if (node == null) return;

            Console.Write($"({node.Key},{node.Value}) ");
            PreOrderTraversalPrint(node.Left);
            PreOrderTraversalPrint(node.Right);
        }

        public void PostOrder()
        {
            PostOrderTraversalPrint(root);
            Console.WriteLine();
        }

        private void PostOrderTraversalPrint(Node node)
        {
            if (node == null) return;

            PostOrderTraversalPrint(node.Left);
            PostOrderTraversalPrint(node.Right);
            Console.Write($"({node.Key},{node.Value}) ");
        }
    }
}