using System;
using System.Collections.Generic;
using System.IO;

namespace Graph
{
    public class BaseGraph
    {
        private Dictionary<string, Dictionary<string, int>>? _adjacensyList;
        private bool _direct;
        private bool _measured;

        private static IEnumerable<string> ReadFrom(string file)
        {
            string? line;
            using var reader = File.OpenText(file);
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        public Dictionary<string, Dictionary<string, int>>? AdjacensyList { get { return _adjacensyList; } }
        public bool Direct { get { return _direct; } }
        public bool Measured { get { return _measured; } }

        public BaseGraph()
        {
            _adjacensyList = new();
            _direct = false;
            _measured = false;
        }

        public BaseGraph(BaseGraph graph)
        {
            _adjacensyList = graph._adjacensyList;
            _direct = graph._direct;
            _measured = graph._measured;
        }

        public BaseGraph(string fileAdjacensyList)
        {
            _adjacensyList = new();

            IEnumerable<string> textfile = ReadFrom(fileAdjacensyList);
            int i = 0;
            foreach (string line in textfile)
            {
                if (i == 0)
                    _direct = bool.Parse(line);
                else if (i == 1)
                    _measured = bool.Parse(line);
                else
                {
                    string[] lineelements = line.Split(" ");
                    string vertex = lineelements[0];
                    _adjacensyList.Add(vertex, new Dictionary<string, int>());

                    string adjacensyvertex = "";
                    for (int j = 1; j < lineelements.Length; j++)
                    {
                        if (j % 2 != 0)
                            adjacensyvertex = lineelements[j];
                        else
                            _adjacensyList[vertex][adjacensyvertex] = Int32.Parse(lineelements[j]);
                    }
                }
                i++;
            }
        }

        public void OutGraph(string filename)
        {
            StreamWriter writer = new StreamWriter(filename, false);

            foreach (var vertex in _adjacensyList)
            {
                writer.Write($"{vertex.Key} ");
                int i = 1;
                int adjVertexCount = vertex.Value.Count;
                foreach (var adjacensyvertex in vertex.Value)
                {
                    if (i != adjVertexCount)
                        writer.Write($"{adjacensyvertex.Key} {adjacensyvertex.Value} ");
                    else
                        writer.Write($"{adjacensyvertex.Key} {adjacensyvertex.Value}");
                    i++;
                }
                writer.WriteLine();
            }
            writer.Close();
        }

        public void AddVertex(string vertex)
        {
            if (!_adjacensyList.ContainsKey(vertex))
                _adjacensyList.Add(vertex, new Dictionary<string, int>());
            else
                Console.WriteLine("Вершина уже существует.");
        }

        public void RemoveVertex(string vertex)
        {
            if (_adjacensyList.ContainsKey(vertex))
            {
                foreach (var adjacensyvertex in _adjacensyList)
                    adjacensyvertex.Value.Remove(vertex);
                _adjacensyList.Remove(vertex);
            }
            else Console.WriteLine("Вершины не существует.");
        }

        public void AddEdge(string vertex1, string vertex2, int weight)
        {
            if (!_adjacensyList.ContainsKey(vertex1) || !_adjacensyList.ContainsKey(vertex2))
                Console.WriteLine("Вершин не существует.");
            else
            {
                if (!_adjacensyList[vertex1].ContainsKey(vertex2) || !_adjacensyList[vertex2].ContainsKey(vertex1))
                {
                    _adjacensyList[vertex1][vertex2] = weight;
                    if (!_direct)
                        _adjacensyList[vertex2][vertex1] = weight;
                }
                else Console.WriteLine("Ребро уже существует.");
            }
        }

        public void RemoveEdge(string vertex1, string vertex2)
        {
            if (!_adjacensyList.ContainsKey(vertex1) || !_adjacensyList.ContainsKey(vertex2))
                Console.WriteLine("Вершин не существует.");
            else
            {
                if (_adjacensyList[vertex1].ContainsKey(vertex2) || _adjacensyList[vertex2].ContainsKey(vertex1))
                {
                    _adjacensyList[vertex1].Remove(vertex2);
                    if (!_direct)
                        _adjacensyList[vertex2].Remove(vertex1);
                }
                else Console.WriteLine("Ребра не существует.");
            }
        }

        public void AllNotAdjacencyVertexDirect(string currentvertex)
        {
            if (!_direct)
                Console.WriteLine("Граф не ориентированный.");
            else
            {
                foreach (var vertex in _adjacensyList)
                {
                    if (!vertex.Value.ContainsKey(currentvertex) && vertex.Key != currentvertex)
                        Console.Write($"|{vertex.Key}| ");
                }
                Console.WriteLine();
            }

        }

        public void AllNotAdjacencyVertex(string currentvertex)
        {
            if (_direct)
                Console.WriteLine("Граф ориентированный.");
            else
            {
                Dictionary<string, int> adjacencyvertex = _adjacensyList[currentvertex];
                foreach (var vertex in _adjacensyList)
                    if (!adjacencyvertex.ContainsKey(vertex.Key) && vertex.Key != currentvertex)
                        Console.Write($"|{vertex.Key}| ");
                Console.WriteLine();
            }

        }

        public Dictionary<string, Dictionary<string, int>> AllVertexEdges(BaseGraph G2)
        {
            Dictionary<string, Dictionary<string, int>> notincludedvertexedges = new();
            foreach (var vertex in _adjacensyList)
            {
                if (G2.AdjacensyList.ContainsKey(vertex.Key))
                {
                    foreach (var adjvertex in vertex.Value)
                    {
                        if (!G2.AdjacensyList[vertex.Key].ContainsKey(adjvertex.Key))
                        {
                            if (!notincludedvertexedges.ContainsKey(vertex.Key))
                            {
                                notincludedvertexedges.Add(vertex.Key, new Dictionary<string, int>());
                            }
                            notincludedvertexedges[vertex.Key].Add(adjvertex.Key, adjvertex.Value);
                        }
                    }
                }
                else
                {
                    if (!notincludedvertexedges.ContainsKey(vertex.Key))
                    {
                        notincludedvertexedges.Add(vertex.Key, new Dictionary<string, int>());
                    }
                }
            }
            return notincludedvertexedges;
        }


    }
}
