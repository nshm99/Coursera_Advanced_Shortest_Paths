namespace A4
{
    internal class graph
    {
        public node s;
        public node t;
        public double w;
        public int queuePos;
        public graph(node s, node t , double w,int q)
        {
            this.s = s;
            this.t = t;
            this.w = w;
            this.queuePos = q;
        }

    }
}