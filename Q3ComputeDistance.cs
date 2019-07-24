using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A4
{
    public class Q3ComputeDistance : Processor
    {
        public Q3ComputeDistance(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long,long, long[][], long[][], long, long[][], long[]>)Solve);


        public long[] Solve(long nodeCount, 
                            long edgeCount,
                            long[][] points,
                            long[][] edges,
                            long queriesCount,
                            long[][] queries)
        {
            int[] pq = new int[nodeCount];

            Vertex[] graph = new Vertex[nodeCount];
            for(int i=0;i<nodeCount;i++)
            {
                graph[i] = new Vertex(i, points[i][0], points[i][1]);
            }
            for(int i=0;i<edgeCount;i++)
            {
                long src = edges[i][0] - 1;
                long des = edges[i][1] - 1;
                long w = edges[i][2];
                graph[src].adj.Add(new adjancy(des, w));
            }
            //result
            List<long> result = new List<long>();
            for (int i = 0; i < queriesCount; i++)
            {
                long start = queries[i][0] - 1;
                long end = queries[i][1]-1;
                if (start == end)
                    result.Add(0);
                else
                {
                    if (graph[start].adj.Count == 0)
                        result.Add(-1);
                    else
                        result.Add(computeDistance(graph, start, end));
                }

            }

            return result.ToArray();
        }

        private long computeDistance(Vertex[] graph, long start, long end)
        {
            int[] pq = new int[graph.Length];
            //init
            for(int i=0;i<graph.Length;i++)
            {
                graph[i].processed = false;
                graph[i].queuePos = i;
                graph[i].distance = long.MaxValue;
                graph[i].distWithPpotential = long.MaxValue;
                graph[i].potential = Distance(graph[i].x, graph[i].y, graph[end].x, graph[end].y);
                pq[i] = i;
            }
            graph[start].distance = 0;
            graph[start].distWithPpotential = 0;

            PriorityQueue queue = new PriorityQueue();
            queue.makeQueue(graph, pq, (int)start,(int) end);

            for(int i=0;i<graph.Length;i++)
            {
                Vertex v = queue.extractMin(graph, pq, i);

                if (graph[v.index].distance == long.MaxValue)
                    return -1;
                if (v.index == end)
                    return graph[v.index].distance;

                Relax(graph, v, pq, queue);
            }
            return -1;
        }

        private void Relax(Vertex[] graph, Vertex v, int[] pq, PriorityQueue queue)
        {
            for(int i=0;i<v.adj.Count;i++)
            {
                long adjIndex = v.adj[i].Node;
                if(graph[adjIndex].distance > graph[v.index].distance + v.adj[i].Value)
                {
                    graph[adjIndex].distance = graph[v.index].distance + (long) v.adj[i].Value;
                    graph[adjIndex].distWithPpotential =
                       graph[adjIndex].distance + graph[adjIndex].potential;
                    queue.changePriority(graph, pq, graph[adjIndex].queuePos);
                }
            }
        }

        private double Distance(long x1, long y1, long x2, long y2)
        {
            double x = x1 - x2;
            double y = y1 - y2;
            return Math.Sqrt((x * x) + (y * y));
        }
    }
}
