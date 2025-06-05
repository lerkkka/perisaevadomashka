using System;

namespace двоичное_дерево
{
    class Program
    {
        //1.	Разработать класс Двоичное дерево поиска (BTS – Binary tree search),
        //реализующий бинарное дерево поиска. Узлами дерева должны быть пары: ключ-значение (как в словаре).
        //В классе должен быть метод инициализации дерева, метод добавления нового узла и метод удаления узла.
        //Необходимо также разработать метод, вывода дерева в виде словаря, отсортированного по ключам,
        //а также методы обхода узлов дерева (инфиксный, префиксный и постфиксный)


        static void Main(string[] args)
        {
            var tree = new BinarySearchTree();

            Console.WriteLine("Доступные команды: добавить, удалить, инфиксный, префиксный, постфиксный, выход.");

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
                    tree.Add(key, value);
                    Console.WriteLine("Добавлено");
                }
                else if (команда == "удалить")
                {
                    Console.Write("Ключ для удаления: ");
                    int key = int.Parse(Console.ReadLine());
                    tree.Remove(key);
                    Console.WriteLine("Удалено");
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

    class Node
    {
        public int Key;
        public string Value;
        public Node Left, Right;

        public Node(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }

    class BinarySearchTree
    {
        private Node root;

        public void Add(int key, string value)
        {
            root = AddRecursive(root, key, value);
        }

        private Node AddRecursive(Node node, int key, string value)
        {
            if (node == null) return new Node(key, value);
            if (key < node.Key) node.Left = AddRecursive(node.Left, key, value);
            else if (key > node.Key) node.Right = AddRecursive(node.Right, key, value);
            return node;
        }

        public void Remove(int key)
        {
            root = RemoveRecursive(root, key);
        }

        private Node RemoveRecursive(Node node, int key)
        {
            if (node == null) return null;
            if (key < node.Key) node.Left = RemoveRecursive(node.Left, key);
            else if (key > node.Key) node.Right = RemoveRecursive(node.Right, key);
            else
            {
                if (node.Left == null) return node.Right;
                if (node.Right == null) return node.Left;

                var minNode = FindMin(node.Right);
                node.Key = minNode.Key;
                node.Value = minNode.Value;
                node.Right = RemoveRecursive(node.Right, minNode.Key);
            }
            return node;
        }

        private Node FindMin(Node node)
        {
            while (node.Left != null) node = node.Left;
            return node;
        }

        public void InOrder()
        {
            InOrderTraversal(root);
            Console.WriteLine();
        }

        private void InOrderTraversal(Node node)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left);
                Console.Write($"({node.Key}, {node.Value}) ");
                InOrderTraversal(node.Right);
            }
        }

        public void PreOrder()
        {
            PreOrderTraversal(root);
            Console.WriteLine();
        }

        private void PreOrderTraversal(Node node)
        {
            if (node != null)
            {
                Console.Write($"({node.Key}, {node.Value}) ");
                PreOrderTraversal(node.Left);
                PreOrderTraversal(node.Right);
            }
        }

        public void PostOrder()
        {
            PostOrderTraversal(root);
            Console.WriteLine();
        }

        private void PostOrderTraversal(Node node)
        {
            if (node != null)
            {
                PostOrderTraversal(node.Left);
                PostOrderTraversal(node.Right);
                Console.Write($"({node.Key}, {node.Value}) ");
            }
        }
    }
}