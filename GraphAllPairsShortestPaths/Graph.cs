using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

/*
 * Найти самый короткий простой цикл во взвешенном графе. Граф задан взвешенной матрицей смежности.
 */

namespace GraphAllPairsShortestPaths
{
    class Graph
    {
        private List<Vertex> VerticesList;
        private int[,] AdjacencyMatrix; //{ get; set; }
        public int Size { get; }

        private int MinCycleLength = int.MaxValue;
        private Stack<Vertex> MinCycle = new Stack<Vertex>();

        public Graph(int size, string[] strs)
        {
            Size = size;
            AdjacencyMatrix = new int[Size, Size];
            VerticesList = new List<Vertex>(Size);
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

        private int[,] ExtendShortestPaths(int[,] firstMatrix, int[,] secondMatrix)
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
                //int[,] curMatrix = new int[Size, Size];
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        if (matrix[i, j] < matrix[i, k] + matrix[k, j] || matrix[i, k] == int.MaxValue || matrix[k, j] == int.MaxValue)
                            matrix[i, j] = matrix[i, j];//curMatrix[i, j] = matrix[i, j];
                        else
                            matrix[i, j] = matrix[i, k] + matrix[k, j];//curMatrix[i, j] = matrix[i, k] + matrix[k, j];
                    }
                }
                //matrix = (int[,])curMatrix.Clone();
            }

            return matrix;
        }

        public void TransitiveClosure()
        {
            bool[,] matrix = new bool[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (i == j || AdjacencyMatrix[i, j] != int.MaxValue)
                        matrix[i, j] = true;
                    else
                        matrix[i, j] = false;
                }
            }

            for (int k = 0; k < Size; k++)
            {
                //bool[,] curMatrix = new bool[Size, Size];
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        matrix[i, j] = matrix[i, j] || matrix[i, k] && matrix[k, j];
                    }
                }
                //matrix = (bool[,])curMatrix.Clone();
            }
        }


        private bool Relax(Vertex incidentFrom, Vertex incidentTo, int weight)
        {
            if (incidentTo.Distance > incidentFrom.Distance + weight && incidentFrom.Distance != int.MaxValue)
            {
                incidentTo.Distance = incidentFrom.Distance + weight;
                incidentTo.Parent = incidentFrom;
                return true;
            }

            return false;
        }

        private void InitializeSingleSource(Vertex source)
        {
            foreach (var vertex in VerticesList)
            {
                vertex.Parent = null;
                vertex.Distance = int.MaxValue;
            }
            source.Distance = 0;
        }

        /// <summary>
        /// Алгоритм Дейкстры. Сложность 0(V^2)
        /// Работает, если в графе веса ребер, исходящих из некоторого истока s, могут быть отрицательными,
        /// веса всех других ребер неотрицательные, а циклы с отрицательными весами отсутствуют
        /// </summary>
        public void Dijkstra(Vertex source)
        {
            InitializeSingleSource(source);

            //List<Vertex> discoveredVertices = new List<Vertex>();
            List<Vertex> verticesToAddInTree = new List<Vertex>(VerticesList);

            while (verticesToAddInTree.Count != 0)
            {
                Vertex curVertex = ExtractMin(verticesToAddInTree);
                //discoveredVertices.Add(curVertex);
                int i = curVertex.Index - 1;

                for (int j = 0; j < Size; j++)
                {
                    if (i != j && AdjacencyMatrix[i, j] != int.MaxValue)
                        Relax(curVertex, VerticesList[j], AdjacencyMatrix[i, j]);
                }
            }
        }

        private Vertex ExtractMin(List<Vertex> vertices)
        {
            Vertex minVertex = vertices[0];
            int min = minVertex.Distance;

            foreach (var vertex in vertices)
            {
                if (vertex.Distance < min)
                {
                    min = vertex.Distance;
                    minVertex = vertex;
                }
            }

            vertices.Remove(minVertex);
            return minVertex;
        }

        /// <summary>
        /// Поиск в глубину. Сложность 0(V + Е).
        /// </summary>
        public void DepthFirstSearch()
        {
            foreach (var vertex in VerticesList)
            {
                vertex.Discovered = false;
                vertex.Color = false;
                vertex.Parent = null;
            }

            foreach (var vertex in VerticesList)
            {
                if (vertex.Discovered == false)
                    DFSVisit(vertex);
            }
        }

        private void DFSVisit(Vertex vertex)
        {
            vertex.Discovered = true;
            vertex.Color = true;
            int i = vertex.Index - 1;

            for (int j = 0; j < Size; j++)
            {
                if (i != j && AdjacencyMatrix[i, j] != int.MaxValue)
                    if (VerticesList[j].Color == false)
                    {
                        VerticesList[j].Parent = vertex;
                        VerticesList[j].ParentWeight = AdjacencyMatrix[i, j];
                        DFSVisit(VerticesList[j]);
                    }
                    else
                    {
                        Stack<Vertex> cycle = new Stack<Vertex>();
                        int cycleLength = AdjacencyMatrix[i, j];

                        Vertex curVertex = vertex;

                        while (curVertex != VerticesList[j])
                        {
                            cycle.Push(curVertex);
                            cycleLength += curVertex.ParentWeight;
                            curVertex = curVertex.Parent;
                        }

                        cycle.Push(VerticesList[j]);

                        if (cycleLength < MinCycleLength)
                        {
                            MinCycleLength = cycleLength;
                            MinCycle = cycle;
                        }
                    }
            }

            vertex.Color = false;
        }


        /// <summary>
        /// Поиск кратчайшего простого цикла в графе. Сложность 0()
        /// Перебор всех возможных циклов в графе со следующим ограничением:
        /// цикл может проходить через одну и ту же вершину только один раз.
        /// При переборе сравниваем длину каждого цикла с самым минимальным циклом,
        /// и если он а меньше, то делаем самым минимальным текущий цикл.
        /// </summary>
        public string ShortestSimpleCycleSearch()
        {
            DepthFirstSearch();

            Console.WriteLine(MinCycleLength);
            StringBuilder output = new StringBuilder();
            foreach (var vertex in MinCycle)
            {
                output.Append(vertex.Index + " ");
            }
            Console.WriteLine(output);

            return output.ToString();
        }

        public void SaveTxtFormatGraph(string graphFile)
        {
            using (StreamWriter writer = new StreamWriter(graphFile))
            {
                writer.WriteLine(Size);
                writer.WriteLine(ToTxtFile());
            }
        }

        public string ToTxtFile()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (AdjacencyMatrix[i, j] != int.MaxValue)
                    {
                        sb.Append(i + 1 + " ");
                        sb.Append(j + 1 + " ");
                        sb.Append(AdjacencyMatrix[i, j]);
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
