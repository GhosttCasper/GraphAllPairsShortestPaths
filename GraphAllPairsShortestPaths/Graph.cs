using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphAllPairsShortestPaths
{
    class Graph
    {
        public List<Vertex> VerticesList = new List<Vertex>();
        public int[,] AdjacencyMatrix;
        public int Size;

        public Graph(int size, string[] strs)
        {
            Size = size;
            AdjacencyMatrix = new int[Size, Size];
            for (int i = 1; i <= Size; i++)
                VerticesList.Add(new Vertex(i));

            try
            {
                for (int i = 0; i < Size; i++)
                {
                    var array = strs[i].Split();
                    if (!string.IsNullOrEmpty(strs[i]))
                        for (int j = 0; j < Size; j++)
                        {
                            int intVar = array[j] == "?" ? int.MaxValue : int.Parse(array[j]);
                            AdjacencyMatrix[i, j] = intVar;
                        }
                }
            }
            catch (Exception ex)
            {
                if (ex is NullReferenceException || ex is FormatException)
                    Console.WriteLine("String is empty (Graph)"); //throw new Exception("String is empty (Graph)"); 
                else
                    throw new Exception(ex.Message);
            }
        }

        public int[,] ExtendShortestPaths(int[,] firstMatrix, int[,] secondMatrix)
        {
            int[,] curMatrix = new int[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    curMatrix[i, j] = int.MaxValue;
                    for (int k = 0; k < Size; k++)
                    {
                        if (firstMatrix[i, k] + secondMatrix[k, j] < curMatrix[i, j] && secondMatrix[k, j] != int.MaxValue && firstMatrix[i, k] != int.MaxValue)
                            curMatrix[i, j] = firstMatrix[i, k] + secondMatrix[k, j];
                    }
                }
            }

            return curMatrix;
        }

        /// <summary>
        /// Вычисление кратчайших путей между всеми парами вершин. Repeated squaring. Сложность 0(V^3*lgV)
        /// </summary>
        public int[,] FasterAllPairsShortestPaths()
        {
            int[,] matrix = AdjacencyMatrix;
            int m = 1;

            while (m < Size - 1)
            {
                int[,] curMatrix = ExtendShortestPaths(matrix, matrix);
                matrix = (int[,])curMatrix.Clone();
                m <<= 1; // *2
            }

            return matrix;
        }

        /// <summary>
        /// Алгоритм Флойда-Уоршелла. Сложность 0(V^3).
        /// </summary>
        public int[,] FloydWarshall()
        {
            int[,] matrix = AdjacencyMatrix;
            for (int k = 0; k < Size; k++)
            {
                int[,] curMatrix = new int[Size, Size];
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (matrix[i, j] < matrix[i, k] + matrix[k, j] || matrix[i, k] == int.MaxValue || matrix[k, j] == int.MaxValue)
                            curMatrix[i, j] = matrix[i, j];
                        else
                            curMatrix[i, j] = matrix[i, k] + matrix[k, j];
                    }
                }
                matrix = (int[,])curMatrix.Clone();
            }

            return matrix;
        }
    }
}
