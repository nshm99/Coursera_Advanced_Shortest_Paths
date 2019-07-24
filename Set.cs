namespace A4
{
    internal class Set
    {
        public long rank;
        public long parent;

        public Set(long parent,long rank)
        {
            this.parent = parent;
            this.rank = rank;
        }
    }
}
