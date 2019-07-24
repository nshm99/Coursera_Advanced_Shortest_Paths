namespace A4
{
    public class node
    {
        public long x;
        public long y;
        public long index;
        public long parnt;
        public node (long x,long y,long index)
        {
            this.x = x;
            this.y = y;
            this.index = index;
            parnt = index;
        }
    }
}