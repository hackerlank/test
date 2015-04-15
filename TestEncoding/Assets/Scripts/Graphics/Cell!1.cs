namespace Graphics
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Cell<T> where T: NavGraphNode
    {
        public InvertedAABBox2D BBox;
        private CellSpacePartition<T> cellSpace;
        public int column;
        private GameObject m_uCell;
        public List<T> Members;
        public NavGraphNode node;
        public int row;

        public Cell(int row, int column, Vector2D leftTop, Vector2D rightBot, CellSpacePartition<T> cellSpace, object perCellMapData, System.Type type)
        {
            this.row = -1;
            this.column = -1;
            this.BBox = new InvertedAABBox2D(leftTop, rightBot);
            this.cellSpace = cellSpace;
            this.RefreshSelf(row, column, leftTop, rightBot, perCellMapData, type);
        }

        public void DestroySelf()
        {
            if (this.m_uCell != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_uCell);
            }
        }

        public void RefreshSelf(int row, int column, Vector2D topLeft, Vector2D botright, object perCellMapData, System.Type type)
        {
            if (this.BBox == null)
            {
                this.BBox = new InvertedAABBox2D(topLeft, botright);
            }
            this.row = row;
            this.column = column;
            Vector2D pos = new Vector2D(this.BBox.Center.x, this.BBox.Center.y);
            this.node = new NavGraphNode(this.row * this.cellSpace.CellsXNum, pos, perCellMapData, type);
        }

        public void UpdatePerCellNodeData(object obj)
        {
            this.node.UpdateCellMapData(obj);
        }
    }
}

