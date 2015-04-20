using System;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Graphics
{
    public class NavGraphNode : GraphNode
    {

        #region instance fields

        protected int m_row;

        public int row 
        {
            set { this.m_row = value; }

            get { return this.m_row; }
        }

        protected int m_column;

        public int column
        {
            set 
            {
                this.m_column = value;
            }
            get 
            {
                return this.m_column;
            }
        }

        protected Vector2D m_vPosition;

        public Vector2D Pos
        {
            set { this.m_vPosition = value; }
            get { return this.m_vPosition; }
        }

        protected System.Object m_ExtraInfo;

        public System.Object ExtraInfo
        {
            set { this.m_ExtraInfo = value; }
            get { return this.m_ExtraInfo; }
        }

        #endregion


        #region instance constuctors

        public NavGraphNode() : base(){ }

        public NavGraphNode(int idx, Vector2D pos)
            : base(idx) 
        {

        }

        public NavGraphNode(int idx, Vector2D pos, System.Object extraInfo,System.Type type)
            : this(idx,pos)
        {
            if (extraInfo == null)
            {
                this.m_ExtraInfo = System.Activator.CreateInstance(type);
            }
            else 
            {
                this.m_ExtraInfo = extraInfo;
            }
        }

        #endregion

        #region instance methods

        //public void SetCellTypeForExtraInfo(CellType type) 
        //{
        //    NavNodeInfo info = this.m_ExtraInfo as NavNodeInfo;

        //    info.type = type;
        //}

        public void UpdateCellMapData(System.Object obj)
        {
            Type type = this.m_ExtraInfo.GetType();

            MethodInfo info = type.GetMethod("UpdateTileInfo",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            info.Invoke(this.m_ExtraInfo,new System.Object[]{obj});
        }

        #endregion

    }
}
