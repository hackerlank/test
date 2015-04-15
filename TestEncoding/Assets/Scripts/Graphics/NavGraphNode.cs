namespace Graphics
{
    using System;
    using System.Reflection;

    public class NavGraphNode : GraphNode
    {
        protected int m_column;
        protected object m_ExtraInfo;
        protected int m_row;
        protected Vector2D m_vPosition;

        public NavGraphNode()
        {
        }

        public NavGraphNode(int idx, Vector2D pos) : base(idx)
        {
        }

        public NavGraphNode(int idx, Vector2D pos, object extraInfo, Type type) : this(idx, pos)
        {
            if (extraInfo == null)
            {
                this.m_ExtraInfo = Activator.CreateInstance(type);
            }
            else
            {
                this.m_ExtraInfo = extraInfo;
            }
        }

        public void UpdateCellMapData(object obj)
        {
            object[] parameters = new object[] { obj };
            this.m_ExtraInfo.GetType().GetMethod("UpdateTileInfo", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Invoke(this.m_ExtraInfo, parameters);
        }

        public int column
        {
            get
            {
                return this.m_column;
            }
            set
            {
                this.m_column = value;
            }
        }

        public object ExtraInfo
        {
            get
            {
                return this.m_ExtraInfo;
            }
            set
            {
                this.m_ExtraInfo = value;
            }
        }

        public Vector2D Pos
        {
            get
            {
                return this.m_vPosition;
            }
            set
            {
                this.m_vPosition = value;
            }
        }

        public int row
        {
            get
            {
                return this.m_row;
            }
            set
            {
                this.m_row = value;
            }
        }
    }
}

