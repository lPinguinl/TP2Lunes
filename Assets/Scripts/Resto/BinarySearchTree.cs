// File: DataStructures/BinarySearchTree.cs
using System;
using System.Collections.Generic;

public class BinarySearchTree <T>
{
    public class Node
    {
        public T Value;
        public Node Left;
        public Node Right;

        public Node(T value)
        {
            Value = value;
            Left = null;
            Right = null;
        }
    }

    private Node root;
    private readonly Comparison<T> comparer;

    public BinarySearchTree(Comparison<T> comparer)
    {
        this.comparer = comparer;
    }

    public void Insert(T value)
    {
        root = Insert(root, value);
    }

    private Node Insert(Node node, T value)
    {
        if (node == null) return new Node(value);

        int comparison = comparer(value, node.Value);
        if (comparison < 0)
            node.Left = Insert(node.Left, value);
        else if (comparison > 0)
            node.Right = Insert(node.Right, value);

        return node;
    }

    public void Remove(T value)
    {
        root = Remove(root, value);
    }

    private Node Remove(Node node, T value)
    {
        if (node == null) return null;

        int comparison = comparer(value, node.Value);
        if (comparison < 0)
            node.Left = Remove(node.Left, value);
        else if (comparison > 0)
            node.Right = Remove(node.Right, value);
        else
        {
            if (node.Left == null) return node.Right;
            if (node.Right == null) return node.Left;

            Node minNode = FindMin(node.Right);
            node.Value = minNode.Value;
            node.Right = Remove(node.Right, minNode.Value);
        }
        return node;
    }

    private Node FindMin(Node node)
    {
        while (node.Left != null) node = node.Left;
        return node;
    }

    public IEnumerable<T> InOrderTraversal()
    {
        var result = new List<T>();
        InOrderTraversal(root, result);
        return result;
    }

    private void InOrderTraversal(Node node, List<T> result)
    {
        if (node == null) return;

        InOrderTraversal(node.Left, result);
        result.Add(node.Value);
        InOrderTraversal(node.Right, result);
    }

    public int Count() => CountNodes(root);

    private int CountNodes(Node node) => node == null ? 0 : 1 + CountNodes(node.Left) + CountNodes(node.Right);
}
