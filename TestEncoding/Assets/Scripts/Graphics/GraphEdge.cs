using System;
using System.Collections.Generic;

namespace Graphics
{
    public class GraphEdge
    {

        protected int m_iFrom;

        public int From 
        {
            set { this.m_iFrom = value; }
            get { return this.m_iFrom; }
        }


        protected int m_iTo;
        public int To
        {
            set { this.m_iTo = value; }
            get { return this.m_iTo; }
        }


        protected float m_fCost;

        public float Cost 
        {
            set { this.m_fCost = value; }
            get { return this.m_fCost; }
        }

        public GraphEdge(int from ,
                         int to,
                         float cost) 
        {
            this.m_iFrom = from;
            this.m_iTo = to;
            this.m_fCost = cost;
        }

        public GraphEdge() 
        {
            this.m_fCost = 1f;

            this.m_iFrom = GraphConst.invalid_node_index; ;

            this.m_iTo = GraphConst.invalid_node_index;
        }


        public static bool operator ==(GraphEdge edge0, GraphEdge edge1) 
        {
            return edge0.m_iFrom == edge1.m_iFrom &&
                   edge0.m_iTo == edge1.m_iTo &&
                   edge0.m_fCost == edge1.m_fCost;
        }

        public static bool operator !=(GraphEdge edge0, GraphEdge edge1)
        {
            return edge0.m_iFrom != edge1.m_iFrom ||
                   edge0.m_iTo != edge1.m_iTo ||
                   edge0.m_fCost != edge1.m_fCost;
        }


    }
}
