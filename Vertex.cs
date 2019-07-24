using System.Collections.Generic;

namespace A4
{
    internal class Vertex
    {
        public long x;
        public long y;
        public int index;
        public long distance;
        public double potential;
        public double distWithPpotential;
        public int queuePos;
        public bool processed;
        public List<adjancy> adj
        {
            get;set;
        }


        public Vertex()
        {
        }

        public Vertex(int index,long x,long y)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.adj = new List<adjancy>();
        }
    }
}