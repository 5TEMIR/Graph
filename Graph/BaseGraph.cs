using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graph
{
    public class BaseGraph
    {
        private Dictionary<string, Dictionary<string, int>>? _adjacenсyList;
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

        public Dictionary<string, Dictionary<string, int>>? AdjacensyList { get { return _adjacenсyList; } }
        public bool Direct { get { return _direct; } }
        public bool Measured { get { return _measured; } }

        public BaseGraph()
        {
            _adjacenсyList = new();
            _direct = false;
            _measured = false;
        }

        public BaseGraph(BaseGraph graph)
        {
            _adjacenсyList = graph._adjacenсyList;
            _direct = graph._direct;
            _measured = graph._measured;
        }

        public BaseGraph(string fileAdjacensyList)
        {
            _adjacenсyList = new();

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
                    _adjacenсyList.Add(vertex, new Dictionary<string, int>());

                    string adjacensyvertex = "";
                    for (int j = 1; j < lineelements.Length; j++)
                    {
                        if (j % 2 != 0)
                            adjacensyvertex = lineelements[j];
                        else
                            _adjacenсyList[vertex][adjacensyvertex] = Int32.Parse(lineelements[j]);
                    }
                }
                i++;
            }
        }

        public void OutGraph(string filename)
        {
            StreamWriter writer = new StreamWriter(filename, false);

            if (_direct)
                writer.WriteLine("true");
            else
                writer.WriteLine("false");
            if (_measured)
                writer.WriteLine("true");
            else
                writer.WriteLine("false");

            foreach (var vertex in _adjacenсyList)
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
            if (!_adjacenсyList.ContainsKey(vertex))
                _adjacenсyList.Add(vertex, new Dictionary<string, int>());
            else
                Console.WriteLine("Вершина уже существует.");
        }

        public void RemoveVertex(string vertex)
        {
            if (_adjacenсyList.ContainsKey(vertex))
            {
                foreach (var adjacensyvertex in _adjacenсyList)
                    adjacensyvertex.Value.Remove(vertex);
                _adjacenсyList.Remove(vertex);
            }
            else Console.WriteLine("Вершины не существует.");
        }

        public void AddEdge(string vertex1, string vertex2, int weight)
        {
            if (!_adjacenсyList.ContainsKey(vertex1) || !_adjacenсyList.ContainsKey(vertex2))
                Console.WriteLine("Вершин не существует.");
            else
            {
                if (!_adjacenсyList[vertex1].ContainsKey(vertex2) || !_adjacenсyList[vertex2].ContainsKey(vertex1))
                {
                    _adjacenсyList[vertex1][vertex2] = weight;
                    if (!_direct)
                        _adjacenсyList[vertex2][vertex1] = weight;
                }
                else Console.WriteLine("Ребро уже существует.");
            }
        }

        public void RemoveEdge(string vertex1, string vertex2)
        {
            if (!_adjacenсyList.ContainsKey(vertex1) || !_adjacenсyList.ContainsKey(vertex2))
                Console.WriteLine("Вершин не существует.");
            else
            {
                if (_adjacenсyList[vertex1].ContainsKey(vertex2) || _adjacenсyList[vertex2].ContainsKey(vertex1))
                {
                    _adjacenсyList[vertex1].Remove(vertex2);
                    if (!_direct)
                        _adjacenсyList[vertex2].Remove(vertex1);
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
                foreach (var vertex in _adjacenсyList)
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
                Dictionary<string, int> adjacencyvertex = _adjacenсyList[currentvertex];
                foreach (var vertex in _adjacenсyList)
                    if (!adjacencyvertex.ContainsKey(vertex.Key) && vertex.Key != currentvertex)
                        Console.Write($"|{vertex.Key}| ");
                Console.WriteLine();
            }

        }

        public Dictionary<string, Dictionary<string, int>> AllVertexEdges(BaseGraph G2)
        {
            Dictionary<string, Dictionary<string, int>> notincludedvertexedges = new();
            foreach (var vertex in _adjacenсyList)
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

        private bool IsConnected(string start, HashSet<string> visited)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(start);
            visited.Add(start);

            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                foreach (var neighbor in _adjacenсyList[current])
                {
                    if (!visited.Contains(neighbor.Key))
                    {
                        visited.Add(neighbor.Key);
                        queue.Enqueue(neighbor.Key);
                    }
                }
            }

            foreach (var vertex in _adjacenсyList.Keys)
            {
                if (!visited.Contains(vertex))
                    return false;
            }

            return true;
        }

        public bool CanFormTreeByRemovingVertex()
        {
            int edges = 0;
            foreach (var vertex in _adjacenсyList.Keys)
            {
                edges += _adjacenсyList[vertex].Count;
            }
            edges /= 2;

            foreach (var vertex in _adjacenсyList.Keys)
            {
                int remainingEdges = edges - _adjacenсyList[vertex].Count;
                int remainingVertices = _adjacenсyList.Count - 1;

                if (remainingEdges == remainingVertices - 1)
                {
                    bool isConnected = true;
                    HashSet<string> visited = new HashSet<string>();
                    visited.Add(vertex);
                    foreach (var vrtx in _adjacenсyList.Keys)
                    {
                        if (vrtx != vertex)
                            if (!IsConnected(vrtx, visited))
                                isConnected = false;
                    }
                    if (isConnected)
                        Console.WriteLine(vertex);
                }
            }
            return false;
        }

        public void FindSCC()
        {
            var disc = new Dictionary<string, int>();
            var low = new Dictionary<string, int>();
            var stack = new Stack<string>();
            var inStack = new HashSet<string>();
            int time = 0;
            int componentCount = 0;

            foreach (var vertex in _adjacenсyList.Keys)
            {
                if (!disc.ContainsKey(vertex))
                {
                    TarjanDFS(vertex, ref time, ref componentCount, disc, low, stack, inStack);
                }
            }

            Console.WriteLine($"Количество сильно связных компонент: {componentCount}");
        }

        private void TarjanDFS(string vertex, ref int time, ref int componentCount,
                                Dictionary<string, int> disc, Dictionary<string, int> low,
                                Stack<string> stack, HashSet<string> inStack)
        {
            disc[vertex] = low[vertex] = time++;
            stack.Push(vertex);
            inStack.Add(vertex);

            foreach (var neighbor in _adjacenсyList[vertex].Keys)
            {
                if (!disc.ContainsKey(neighbor)) // Если сосед ещё не посещен
                {
                    TarjanDFS(neighbor, ref time, ref componentCount, disc, low, stack, inStack);
                    low[vertex] = Math.Min(low[vertex], low[neighbor]);
                }
                else if (inStack.Contains(neighbor)) // Если сосед в стеке
                {
                    low[vertex] = Math.Min(low[vertex], disc[neighbor]);
                }
            }

            // Если вершина является корнем SCC
            if (low[vertex] == disc[vertex])
            {
                componentCount++;
                Console.Write("Сильно связная компонента: ");
                while (true)
                {
                    var w = stack.Pop();
                    inStack.Remove(w);
                    Console.Write(w + " ");
                    if (w == vertex)
                        break;
                }
                Console.WriteLine();
            }
        }

        public List<(string, string, int)> GetMinimumSpanningTree()
        {
            if (_adjacenсyList == null || _adjacenсyList.Count == 0)
                throw new InvalidOperationException("Граф пуст или не инициализирован.");

            HashSet<string> visited = new();
            List<(string, string, int)> mstEdges = new();
            List<(int weight, string, string)> edgeList = new();

            // Начинаем с произвольной вершины, например, первой из списка
            string startVertex = _adjacenсyList.Keys.First();
            visited.Add(startVertex);

            // Добавляем все рёбра из начальной вершины в приоритетную очередь
            foreach (var neighbor in _adjacenсyList[startVertex])
            {
                edgeList.Add((neighbor.Value, startVertex, neighbor.Key));
            }

            while (edgeList.Count > 0)
            {
                // Сортируем рёбра по весу
                edgeList.Sort((a, b) => a.weight.CompareTo(b.weight));

                // Берем первое ребро с наименьшим весом
                var (weight, vertex1, vertex2) = edgeList[0];
                edgeList.RemoveAt(0); // Удаляем это ребро из списка

                // Если вершина уже была посещена, пропускаем её
                if (visited.Contains(vertex2))
                    continue;

                // Добавляем ребро в MST
                mstEdges.Add((vertex1, vertex2, weight));
                visited.Add(vertex2);

                // Добавляем все рёбра из текущей вершины в список рёбер
                foreach (var neighbor in _adjacenсyList[vertex2])
                {
                    if (!visited.Contains(neighbor.Key))
                    {
                        edgeList.Add((neighbor.Value, vertex2, neighbor.Key));
                    }
                }
            }

            return mstEdges;
        }

        public Dictionary<string, int> Dijkstra(string startVertex)
        {
            Dictionary<string, int> distances = new();
            HashSet<string> visited = new();
            List<(string, int distance)> priorityList = new();

            foreach (var vertex in _adjacenсyList.Keys)
            {
                distances[vertex] = int.MaxValue; // Инициализируем расстояния как бесконечность
            }
            distances[startVertex] = 0;
            priorityList.Add((startVertex, 0));

            while (priorityList.Count > 0)
            {
                // Сортируем по расстоянию
                priorityList.Sort((a, b) => a.distance.CompareTo(b.distance));
                var (currentVertex, currentDistance) = priorityList[0];
                priorityList.RemoveAt(0);

                if (visited.Contains(currentVertex))
                    continue;

                visited.Add(currentVertex);

                foreach (var neighbor in _adjacenсyList[currentVertex])
                {
                    int newDistance = currentDistance + neighbor.Value;

                    if (newDistance < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = newDistance;
                        // Добавляем расстояние в список приоритетов
                        priorityList.Add((neighbor.Key, newDistance));
                    }
                }
            }

            return distances;
        }

        public (int, List<string>) GetGraphCenter()
        {
            int radius = int.MaxValue;
            var eccentricities = new Dictionary<string, int>();

            // Вычисляем эксцентриситеты для каждой вершины
            foreach (var vertex in _adjacenсyList.Keys)
            {
                var distances = Dijkstra(vertex);
                int eccentricity = distances.Values.Max();
                eccentricities[vertex] = eccentricity;
            }

            // Находим радиус графа
            radius = eccentricities.Values.Min();

            // Находим центр графа
            var center = eccentricities
                .Where(e => e.Value == radius)
                .Select(e => e.Key)
                .ToList();

            return (radius, center);
        }

        public Dictionary<string, int> BellmanFord(string startVertex)
        {
            Dictionary<string, int> distances = new();
            foreach (var vertex in _adjacenсyList.Keys)
            {
                distances[vertex] = int.MaxValue;
            }
            distances[startVertex] = 0;

            for (int i = 1; i < _adjacenсyList.Count; i++)
            {
                foreach (var vertex in _adjacenсyList.Keys)
                {
                    foreach (var neighbor in _adjacenсyList[vertex])
                    {
                        if (distances[vertex] != int.MaxValue && distances[vertex] + neighbor.Value < distances[neighbor.Key])
                        {
                            distances[neighbor.Key] = distances[vertex] + neighbor.Value;
                        }
                    }
                }
            }

            return distances;
        }

        public Dictionary<string, Dictionary<string, int>> GetAllPairsShortestPaths()
        {
            var allPairsShortestPaths = new Dictionary<string, Dictionary<string, int>>();

            foreach (var vertex in _adjacenсyList.Keys)
            {
                allPairsShortestPaths[vertex] = BellmanFord(vertex);
            }

            return allPairsShortestPaths;
        }

        public Dictionary<string, Dictionary<string, int>> FloydWarshall()
        {
            // Создаем матрицу расстояний
            var distances = new Dictionary<string, Dictionary<string, int>>();

            // Инициализируем матрицу
            foreach (var vertex in _adjacenсyList.Keys)
            {
                distances[vertex] = new Dictionary<string, int>();
                foreach (var otherVertex in _adjacenсyList.Keys)
                {
                    if (vertex == otherVertex)
                        distances[vertex][otherVertex] = 0; // Расстояние до себя
                    else if (_adjacenсyList[vertex].ContainsKey(otherVertex))
                        distances[vertex][otherVertex] = _adjacenсyList[vertex][otherVertex]; // Вес ребра
                    else
                        distances[vertex][otherVertex] = int.MaxValue; // Нет ребра
                }
            }

            // Алгоритм Флойда-Уоршелла
            foreach (var k in _adjacenсyList.Keys)
            {
                foreach (var i in _adjacenсyList.Keys)
                {
                    foreach (var j in _adjacenсyList.Keys)
                    {
                        if (distances[i][k] != int.MaxValue && distances[k][j] != int.MaxValue)
                        {
                            distances[i][j] = Math.Min(distances[i][j], distances[i][k] + distances[k][j]);
                        }
                    }
                }
            }

            return distances;
        }

        public List<string> GetShortestPath(string start, string end)
        {
            var distances = FloydWarshall();
            var path = new List<string>();

            foreach (var v in distances.Keys)
            {
                if (distances[v][v] < 0)
                {
                    Console.WriteLine("В графе есть цикл отрицательного веса.");
                    return path;
                }

            }

                if (distances[start][end] == int.MaxValue)
            {
                Console.WriteLine("Нет пути между вершинами.");
                return path;
            }

            // Восстановление пути
            var current = start;
            path.Add(current);

            while (current != end)
            {
                string next = null;
                int minDistance = int.MaxValue;

                foreach (var neighbor in _adjacenсyList[current].Keys)
                {
                    if (distances[current][neighbor] + distances[neighbor][end] == distances[current][end])
                    {
                        if (distances[current][neighbor] < minDistance)
                        {
                            minDistance = distances[current][neighbor];
                            next = neighbor;
                        }
                    }
                }

                if (next == null)
                {
                    Console.WriteLine("Не удалось восстановить путь.");
                    return new List<string>();
                }

                path.Add(next);
                current = next;
            }

            return path;
        }

        public int FordFulkerson(string source, string sink)
        {
            // Создание матрицы вместимости
            var capacity = new Dictionary<string, Dictionary<string, int>>();
            foreach (var vertex in _adjacenсyList)
            {
                capacity[vertex.Key] = new Dictionary<string, int>();
                foreach (var adj in vertex.Value)
                {
                    capacity[vertex.Key][adj.Key] = adj.Value;
                    if (!_direct) // для неориентированных графов
                        capacity[adj.Key][vertex.Key] = adj.Value;
                }
            }

            int maxFlow = 0;
            var parent = new Dictionary<string, string>();

            // Ищем увеличивающие пути
            while (DFS(capacity, source, sink, ref parent))
            {
                int pathFlow = int.MaxValue;

                // Находим минимальную вместимость в найденном пути
                for (string v = sink; v != source; v = parent[v])
                {
                    string u = parent[v];
                    pathFlow = Math.Min(pathFlow, capacity[u][v]);
                }

                // Обновляем вместимости и обратные ребра
                for (string v = sink; v != source; v = parent[v])
                {
                    string u = parent[v];
                    capacity[u][v] -= pathFlow;
                    if (!capacity.ContainsKey(v))
                        capacity[v] = new Dictionary<string, int>();
                    if (!capacity[v].ContainsKey(u))
                        capacity[v][u] = 0;
                    capacity[v][u] += pathFlow; // Обратное ребро
                }

                maxFlow += pathFlow; // Увеличиваем общий поток

                parent.Clear();
            }

            return maxFlow;
        }

        private bool DFS(Dictionary<string, Dictionary<string, int>> capacity, string source, string sink, ref Dictionary<string, string> parent)
        {
            var visited = new HashSet<string>();
            var stack = new Stack<string>();
            stack.Push(source);
            visited.Add(source);

            while (stack.Count > 0)
            {
                string u = stack.Pop();

                foreach (var v in capacity[u].Keys)
                {
                    if (!visited.Contains(v) && capacity[u][v] > 0) // Если есть доступная вместимость
                    {
                        stack.Push(v);
                        visited.Add(v);
                        parent[v] = u; // Запоминаем путь

                        if (v == sink)
                            return true; // Достигли стока
                    }
                }
            }
            return false; // Не удалось достичь стока
        }
    }
}
