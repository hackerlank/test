namespace Graphics
{
    using System;

    public class GraphNode
    {
        protected int m_iIndex;

        public GraphNode()
        {
            this.m_iIndex = -1;
        }

        public GraphNode(int idx)
        {
            this.m_iIndex = idx;
        }

        public int index
        {
            get
            {
                return this.m_iIndex;
            }
            set
            {
                this.m_iIndex = value;
            }
        }
    }
}

