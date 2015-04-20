using System.Collections.Generic;
using map;

#region Unity相关
using UnityEngine;
#endregion

namespace Graphics
{
    public class Cell<T> where T : NavGraphNode
    {

        public int row = -1;

        public int column = -1;

        public List<T> Members;

        public InvertedAABBox2D BBox;

        public NavGraphNode node;

        private CellSpacePartition<T> cellSpace;

        public Cell(int row, int column, Vector2D leftTop, Vector2D rightBot,CellSpacePartition<T> cellSpace, System.Object perCellMapData, System.Type type) 
        {
            this.BBox = new InvertedAABBox2D(leftTop , rightBot);

            this.cellSpace = cellSpace;

            #region Unity相关

            RefreshSelf(row,column,leftTop,rightBot,perCellMapData,type);
            
            #endregion
        }


        //private CellType m_cellType = CellType.Normal;

        //public CellType cellType 
        //{
        //    set 
        //    { 
        //        this.m_cellType = value;

        //        this.node.SetCellTypeForExtraInfo(this.m_cellType);

        //        //if(this.m_uCell != null)
        //        //{
        //            //Cube cube = this.m_uCell.GetComponent<Cube>();

        //            //switch (value)
        //            //{
        //            //    case CellType.Normal:
        //            //        cube.color = Color.green;
        //            //        break;
        //            //    case CellType.Obstacle0:
        //            //        cube.color = Color.red;
        //            //        break;
        //            //    case CellType.Obstacle1:
        //            //        cube.color = Color.gray;
        //            //        break;
        //            //    case CellType.Unknow:
        //            //        cube.color = Color.black;
        //            //        break;
        //            //}

                    
        //        //}

        //    }
        //    get { return this.m_cellType; }
        //}


        #region Unity相关

        private GameObject m_uCell;

        public void RefreshSelf(int row, int column, Vector2D topLeft, Vector2D botright, System.Object perCellMapData, System.Type type)
        {
            if (this.BBox == null)
            {
                this.BBox = new InvertedAABBox2D(topLeft , botright);
            }

            this.row = row;
            this.column = column;


            //Vector3 center = new Vector3(this.BBox.Center.x, 0, this.BBox.Center.y);
            Vector2D center = new Vector2D(this.BBox.Center.x, this.BBox.Center.y);
            //this.node = new NavGraphNode(this.row * this.cellSpace.CellsXNum,center,new NavNodeInfo(row,column,cellType));
            this.node = new NavGraphNode(this.row * this.cellSpace.CellsXNum, center, perCellMapData,type);
        }


        public void DestroySelf() 
        {
            if (m_uCell != null)
            {
                UnityEngine.Object.DestroyImmediate(this.m_uCell);
            }
        }

        #endregion

        public void UpdatePerCellNodeData(System.Object obj) 
        {
            this.node.UpdateCellMapData(obj);
        }

    }
}


