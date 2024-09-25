namespace Graph
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseGraph baseGraph = new BaseGraph("C:\\Users\\stemir\\source\\repos\\Graph\\Graph\\1.txt");
            BaseGraph baseGraph2 = new BaseGraph("C:\\Users\\stemir\\source\\repos\\Graph\\Graph\\1_2.txt");

            while (true)
            {
                Console.Write("Commands: outConsole,\n" +
                              "          outFile,\n" +
                              "          addVertex,\n" +
                              "          addEdge,\n" +
                              "          removeVertex,\n" +
                              "          removeEdge\n" +
                              "          task2\n" +
                              "          task3\n" +
                              "\nGraph>");
                string command = Console.ReadLine();

                if (command == "outConsole")
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
                else if (command == "addEdg")
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
                    Console.Write("filename: ");
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
            }
        }
    }
}