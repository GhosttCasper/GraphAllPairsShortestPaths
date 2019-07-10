using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Найти самый короткий простой цикл во взвешенном графе. Граф задан взвешенной матрицей смежности.
 */


namespace GraphAllPairsShortestPaths
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = ReadFileWithAdjacencyMatrix("input.txt");
            int[,] matrix = graph.FasterAllPairsShortestPaths();
            //int[,] matrix = graph.FloydWarshall();

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < graph.Size; i++)
            {
                for (int j = 0; j < graph.Size; j++)
                {
                    output.Append(matrix[i, j] + " ");
                }
                output.Append("\n");
            }
            Console.WriteLine(output);
        }

        private static Graph ReadFileWithAdjacencyMatrix(string fileName)
        {
            Graph graph;
            using (StreamReader reader = new StreamReader(fileName))
            {
                int size = ReadNumber(reader);

                string[] numbersStrs = new string[size];
                for (int i = 0; i < size; i++)
                {
                    numbersStrs[i] = reader.ReadLine();
                }
                graph = new Graph(size, numbersStrs);
            }
            return graph;
        }

        private static int ReadNumber(StreamReader reader)
        {
            var numberStr = reader.ReadLine();
            if (numberStr == null)
                throw new Exception("String is empty (ReadNumber)");
            var array = numberStr.Split();
            int number = int.Parse(array[0]);
            return number;
        }
    }
}
