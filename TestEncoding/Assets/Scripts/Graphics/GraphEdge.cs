namespace Graphics
{
    using System;

    public class GraphEdge
    {
        protected float m_fCost;
        protected int m_iFrom;
        protected int m_iTo;

        public GraphEdge()
        {
            this.m_fCost = 1f;
            this.m_iFrom = -1;
            this.m_iTo = -1;
        }

        public GraphEdge(int from, int to, float cost)
        {
            this.m_iFrom = from;
            this.m_iTo = to;
            this.m_fCost = cost;
        }

        public static bool operator ==(GraphEdge edge0, GraphEdge edge1)
        {
            return (((edge0.m_iFrom == edge1.m_iFrom) && (edge0.m_iTo == edge1.m_iTo)) && (edge0.m_fCost == edge1.m_fCost));
        }

        public static bool operator !=(GraphEdge edge0, GraphEdge edge1)
        {
            return (((edge0.m_iFrom != edge1.m_iFrom) || (edge0.m_iTo != edge1.m_iTo)) || !(edge0.m_fCost == edge1.m_fCost));
        }

        public float Cost
        {
            get
            {
                return this.m_fCost;
            }
            set
            {
                this.m_fCost = value;
            }
        }

        public int From
        {
            get
            {
                return this.m_iFrom;
            }
            set
            {
                this.m_iFrom = value;
            }
        }

        public int To
        {
            get
            {
                return this.m_iTo;
            }
            set
            {
                this.m_iTo = value;
            }
        }
    }
}

