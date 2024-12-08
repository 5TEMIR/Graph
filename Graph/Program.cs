using System;
using System.Collections.Generic;
using System.IO;

namespace Graph
{
    internal class Program
    {
        static void Main()
        {
            BaseGraph? baseGraph = null;
            BaseGraph? baseGraph2 = null;

            while (true)
            {
                if (baseGraph == null)
                {
                    Console.Write("FilePathG1: ");
                    string namefile = Console.ReadLine();
                    if (File.Exists(namefile))
                    {
                        baseGraph = new BaseGraph(namefile);
                        Console.WriteLine();
                    }
                    else Console.WriteLine("Указанного пути / файла не существует.");
                }
                else if (baseGraph2 == null)
                {
                    Console.Write("FilePathG2: ");
                    string namefile = Console.ReadLine();
                    if (File.Exists(namefile))
                    {
                        baseGraph2 = new BaseGraph(namefile);
                        Console.WriteLine();
                    }
                    else Console.WriteLine("Указанного пути / файла не существует.");
                }
                else
                {
                    Console.Write("Commands: outCon,\n" +
                                  "          outFile,\n" +
                                  "          addVertex,\n" +
                                  "          addEdge,\n" +
                                  "          removeVertex,\n" +
                                  "          removeEdge,\n" +
                                  "          task2,\n" +
                                  "          task3,\n" +
                                  "          task4,\n" +
                                  "          task5,\n" +
                                  "          task6,\n" +
                                  "          task7,\n" +
                                  "          task8,\n" +
                                  "          task9,\n" +
                                  "          task10,\n" +
                                  "          task11,\n" +
                                  "          clearEnv\n" +
                                  "\nGraph>");
                    string command = Console.ReadLine();

                    if (command == "outCon")
                    {
                        Dictionary<string, Dictionary<string, int>> adjList = baseGraph.AdjacensyList;

                        foreach (var vertex in adjList)
                        {
                            Console.Write($"{vertex.Key} => ");

                            if (baseGraph.Measured)
                            {
                                foreach (var adjvertex in vertex.Value)
                                    Console.Write($"|{adjvertex.Key} ({adjvertex.Value})| ");
                            }
                            else
                            {
                                foreach (var adjvertex in vertex.Value)
                                    Console.Write($"|{adjvertex.Key}| ");
                            }

                            Console.WriteLine();
                        }
                        Console.WriteLine();
                    }
                    else if (command == "addVertex")
                    {
                        Console.Write("vertex: ");
                        string vertex = Console.ReadLine();
                        baseGraph.AddVertex(vertex);
                        Console.WriteLine();
                    }
                    else if (command == "removeVertex")
                    {
                        Console.Write("vertex: ");
                        string vertex = Console.ReadLine();
                        baseGraph.RemoveVertex(vertex);
                        Console.WriteLine();
                    }
                    else if (command == "addEdge")
                    {
                        Console.Write("vertex1: ");
                        string vertex1 = Console.ReadLine();
                        Console.Write("vertex2: ");
                        string vertex2 = Console.ReadLine();
                        int weight;
                        if (baseGraph.Measured)
                        {
                            Console.Write("weight: ");
                            weight = Int32.Parse(Console.ReadLine());
                        }
                        else weight = 0;
                        baseGraph.AddEdge(vertex1, vertex2, weight);
                        Console.WriteLine();
                    }
                    else if (command == "removeEdge")
                    {
                        Console.Write("vertex1: ");
                        string vertex1 = Console.ReadLine();
                        Console.Write("vertex2: ");
                        string vertex2 = Console.ReadLine();
                        baseGraph.RemoveEdge(vertex1, vertex2);
                        Console.WriteLine();
                    }
                    else if (command == "outFile")
                    {
                        Console.Write("filepath: ");
                        string filename = Console.ReadLine();
                        baseGraph.OutGraph(filename);
                        Console.WriteLine();
                    }
                    else if (command == "task2")
                    {
                        Console.Write("vertex: ");
                        string vertex = Console.ReadLine();
                        baseGraph.AllNotAdjacencyVertexDirect(vertex);
                    }
                    else if (command == "task3")
                    {
                        Console.Write("vertex: ");
                        string vertex = Console.ReadLine();
                        baseGraph.AllNotAdjacencyVertex(vertex);
                    }
                    else if (command == "task4")
                    {
                        Dictionary<string, Dictionary<string, int>> adjList = baseGraph.AdjacensyList;

                        foreach (var vertex in adjList)
                        {
                            Console.Write($"{vertex.Key} => ");

                            if (baseGraph.Measured)
                            {
                                foreach (var adjvertex in vertex.Value)
                                    Console.Write($"|{adjvertex.Key} ({adjvertex.Value})| ");
                            }
                            else
                            {
                                foreach (var adjvertex in vertex.Value)
                                    Console.Write($"|{adjvertex.Key}| ");
                            }

                            Console.WriteLine();
                        }
                        Console.WriteLine();

                        Dictionary<string, Dictionary<string, int>> adjList2 = baseGraph2.AdjacensyList;

                        foreach (var vertex in adjList2)
                        {
                            Console.Write($"{vertex.Key} => ");

                            if (baseGraph2.Measured)
                            {
                                foreach (var adjvertex in vertex.Value)
                                    Console.Write($"|{adjvertex.Key} ({adjvertex.Value})| ");
                            }
                            else
                            {
                                foreach (var adjvertex in vertex.Value)
                                    Console.Write($"|{adjvertex.Key}| ");
                            }

                            Console.WriteLine();
                        }
                        Console.WriteLine();

                        Dictionary<string, Dictionary<string, int>> notincludedvertexedges = baseGraph.AllVertexEdges(baseGraph2);
                        if (notincludedvertexedges.Count == 0)
                        {
                            Console.Write("Все вершины и все рёбра графа G1 содержатся в графе G2.");
                        }
                        else
                        {
                            Console.Write("Отсутствуют:\n");

                            foreach (var vertex in notincludedvertexedges)
                            {
                                Console.Write($"{vertex.Key} => ");

                                if (baseGraph.Measured)
                                {
                                    foreach (var adjvertex in vertex.Value)
                                        Console.Write($"|{adjvertex.Key} ({adjvertex.Value})| ");
                                }
                                else
                                {
                                    foreach (var adjvertex in vertex.Value)
                                        Console.Write($"|{adjvertex.Key}| ");
                                }

                                Console.WriteLine();
                            }
                            Console.WriteLine();
                        }
                    }
                    else if (command == "task5")
                    {
                        baseGraph.CanFormTreeByRemovingVertex();
                    }
                    else if (command == "task6")
                    {
                        baseGraph.FindSCC();
                    }
                    else if (command == "task7")
                    {
                        var mst = baseGraph.GetMinimumSpanningTree();
                        Console.WriteLine("каркас минимального веса: ");
                        foreach (var (vertex1, vertex2, weight) in mst)
                        {
                            Console.WriteLine($"{vertex1} - {vertex2} : {weight}");
                        }
                    }
                    else if (command == "task8")
                    {
                        var (radius, center) = baseGraph.GetGraphCenter();
                        Console.WriteLine($"Радиус: {radius}");
                        Console.WriteLine("Центр: " + string.Join(", ", center));
                    }
                    else if (command == "task9")
                    {
                        var allPairsShortestPaths = baseGraph.GetAllPairsShortestPaths();

                        foreach (var startVertex in allPairsShortestPaths.Keys)
                        {
                            Console.WriteLine($"Кратчайшие пути от {startVertex}:");
                            foreach (var destination in allPairsShortestPaths[startVertex])
                            {
                                Console.WriteLine($" до {destination.Key}: {destination.Value}");
                            }
                        }
                    }
                    else if (command == "task10")
                    {
                        Console.Write("vertex1: ");
                        string vertex1 = Console.ReadLine();
                        Console.Write("vertex2: ");
                        string vertex2 = Console.ReadLine();
                        var shortestPath = baseGraph.GetShortestPath(vertex1, vertex2);
                        Console.WriteLine("Кратчайший путь: " + string.Join(" -> ", shortestPath));
                    }
                    else if (command == "task11")
                    {
                        Console.Write("source: ");
                        string vertex1 = Console.ReadLine();
                        Console.Write("sink: ");
                        string vertex2 = Console.ReadLine();
                        Console.WriteLine("Максимальный поток: " + baseGraph.FordFulkerson(vertex1, vertex2));
                    }
                    else if (command == "clearEnv")
                    {
                        baseGraph = null;
                        baseGraph2 = null;
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}