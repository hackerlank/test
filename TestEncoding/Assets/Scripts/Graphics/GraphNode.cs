namespace Graphics
{
    public class GraphNode
    {
        #region fields
        protected int m_iIndex;

        public int index
        {
            set { this.m_iIndex = value; }
            get { return this.m_iIndex; }
        }

        #endregion

        #region Constructors

        public GraphNode()
        {
            this.m_iIndex = GraphConst.invalid_node_index;
        }

        public GraphNode(int idx)
        {
            this.m_iIndex = idx;
        }

        #endregion
    }
}


