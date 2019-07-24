using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCommon;

namespace A3
{
    public class Q4FriendSuggestion : Processor
    {
        public Q4FriendSuggestion(string testDataName) : base(testDataName) { }

        public override string Process(string inStr) =>
            TestTools.Process(inStr, (Func<long, long, long[][], long, long[][], long[]>)Solve);
        
        PriorityQueue queue;
        PriorityQueue revQueue;
        
        public long[] Solve(long NodeCount, long EdgeCount,
                              long[][] edges, long QueriesCount,
                              long[][] Queries)
        {
            //Adj and RevAdj
            List<adjancy>[] adjList = new List<adjancy>[NodeCount];
            List<adjancy>[] revAdjList = new List<adjancy>[NodeCount];
            for (int i = 0; i < NodeCount; i++)
            {
                adjList[i] = new List<adjancy>();
                revAdjList[i] = new List<adjancy>();
            }
            for (int i = 0; i < edges.Length; i++)
            {
                adjList[edges[i][0] - 1].Add(new adjancy(edges[i][1] - 1, edges[i][2]));
                revAdjList[edges[i][1] - 1].Add(new adjancy(edges[i][0] - 1, edges[i][2]));
            }
            //result
            List<long> result = new List<long>();
            for (int i = 0; i < QueriesCount; i++)
            {
                if (Queries[i][0] == Queries[i][1])
                    result.Add(0);
                else
                {
                    if (adjList[Queries[i][0] - 1].Count == 0)
                        result.Add(-1);
                    else

                        result.Add(BiDirectionalDijkstra
                            (Queries[i][0], Queries[i][1], adjList, revAdjList, NodeCount, edges));
                }
            }
            /////
            return result.ToArray();
        }
        
        private long BiDirectionalDijkstra
            (long s, long t, List<adjancy>[] adjList
            , List<adjancy>[] revAdjList, long NodeCount, long[][] edges)
        {
            Nodes[] dist = new    Nodes[NodeCount];
            Nodes[] revDist = new Nodes[NodeCount];

            List<long> proc = new List<long>();
            List<long> revProc = new List<long>();

            

            int[] pq = new int[NodeCount];
            int[] revPq = new int[NodeCount];
            
            for (int j = 0; j < NodeCount; j++)
            {
                dist[j] =    new Nodes(long.MaxValue,j);
                revDist[j] = new Nodes(long.MaxValue, j);
                pq[j] =j;
                revPq[j] = j;
            }
        
            dist[s - 1].dist = 0;
            revDist[t - 1].dist = 0;

            queue = new PriorityQueue();
            queue.makeQueue(dist, pq, (int)s-1, (int)t-1);
            revQueue = new PriorityQueue();
            queue.makeQueue(revDist, revPq, (int)t-1, (int)s-1);
            
            for (int a =0;a<NodeCount;a++)
            {
                long node = queue.extractMin(dist, pq, a);
                if (dist[node].dist == long.MaxValue)
                    continue;
                Process(node, adjList, dist, proc,queue,pq);
                proc.Add(node);

               if (revProc.Contains(node))
                {
                    return ShortestPath(s, dist, proc, t, revDist, revProc);
                }
               
                long revNode = revQueue.extractMin(revDist, revPq, a);
                if (revDist[revNode].dist == long.MaxValue)
                    continue;
                Process(revNode, revAdjList, revDist, revProc,revQueue,revPq);
                revProc.Add(revNode);
                if (proc.Contains(revNode))
                {
                    return ShortestPath(s,  dist, proc, t, revDist, revProc);
                }
            }
            return -1;
        }

        
        /// <summary>
        /// finding shortest path
        /// </summary>
        /// <param name="s">stast node</param>
        /// <param name="dist">distances</param>
        /// <param name="proc">processed nodes</param>
        /// <param name="t">target</param>
        /// <param name="revDist"></param>
        /// <param name="revProc"></param>
        /// <returns>lenghth of path</returns>
        private long ShortestPath
            (long s, Nodes[] dist, List<long> proc
            , long t, Nodes[] revDist, List<long> revProc)
        {
            long distance = long.MaxValue;
            foreach (var u in proc)
            {
                
                if (revDist[u].dist != long.MaxValue)
                {
                    if (dist[u].dist + revDist[u].dist < distance)
                    {
                        distance = dist[u].dist + revDist[u].dist;
                    }
                }

            }
            return distance;
        }

        private void Process
            (long node, List<adjancy>[] adjList, Nodes[] dist
            , List<long> proc,PriorityQueue Q,int[] pq)
        {
            for (int i = 0; i < adjList[node].Count; i++)
            {
                Relax(node, adjList[node][i].node, adjList[node][i].value, dist,Q,pq);
            }
            
        }

        private void Relax(long node, long v, long w, Nodes[] dist,PriorityQueue Q,int[] pq)
        {
            if (dist[node].dist != long.MaxValue)
                if (dist[v].dist > dist[node].dist + w)
                {
                    dist[v].dist = dist[node].dist + w;
                    Q.changePriority(dist, pq, dist[v].queuePos);
                }
        }
      
    }
}
