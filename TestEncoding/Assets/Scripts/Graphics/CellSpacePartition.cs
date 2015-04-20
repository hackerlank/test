using System;
using System.Collections.Generic;
using System.Collections;

namespace Graphics
{
    public class CellSpacePartition<T> where T : NavGraphNode
    {

        private Dictionary<int,Cell<T>> m_Cells;

        public Dictionary<int,Cell<T>> Cells { get { return this.m_Cells; } }

        private float m_fSpaceWidth;

        public float CellSpaceWidth 
        {
            get { return this.m_fSpaceWidth; }
        }

        private float m_fSpaceHeight;

        public float CellSpaceHeight 
        {
            get
            {
                return this.m_fSpaceHeight;
            }
        }

        private int m_iNumCellsX;

        public int CellsXNum
        {
            set { this.m_iNumCellsX = value; }
            get { return this.m_iNumCellsX; }
        }

        private int m_iNumCellsY;

        public int CellsYNum 
        {
            set { this.m_iNumCellsY = value; }
            get { return this.m_iNumCellsY; }
        }


        private float m_fCellSizeX;

        public float CellSizeX 
        {
            get { return this.m_fCellSizeX; }
        }


        private float m_fCellSizeY;

        public float CellSizeY 
        {
            get { return this.m_fCellSizeY; }
        }

        public delegate void SetCellColorDelegate(int row, int column, Color color);

        private SetCellColorDelegate SetCellColor;

        public CellSpacePartition(float width,
                                  float height,
                                  int cellsX,
                                  int cellsY,
                                  System.Object mapData,System.Type type, System.Action createMap = null, SetCellColorDelegate _setCellColor = null) 
        {
            this.SetCellColor = _setCellColor;
            InitCells(width, height, cellsX, cellsY,mapData,type, createMap);
        }

        private void InitCells(float width,
                                  float height,
                                  int cellsX,
                                  int cellsY, 
            System.Object mapData, System.Type type, System.Action createMap)
        {
            if (createMap != null) { createMap(); }

            this.m_fSpaceWidth = width;
            this.m_fSpaceHeight = height;

            this.m_fCellSizeX = width / cellsX;
            this.m_fCellSizeY = height / cellsY;

            this.m_iNumCellsX = cellsX;
            this.m_iNumCellsY = cellsY;

            this.m_Cells = new Dictionary<int, Cell<T>>();

            bool haveData = true;

            IList dataList = null;

            if (mapData == null)
            {
                haveData = false;
            }
            else 
            {
                dataList = mapData as IList;
            }

            for (int i = 0; i < this.m_iNumCellsY; i++)
            {

                for (int j = 0; j < this.m_iNumCellsX; j++)
                {
                    float left = j * this.m_fCellSizeX;
                    float right = left + this.m_fCellSizeX;

                    float bot = i * this.m_fCellSizeY;
                    float top = bot + this.m_fCellSizeY;

                    //Vector3 size = new Vector3(this.CellSizeX - padding, 0, this.CellSizeY - padding);

                    Cell<T> cell = null;

                    if (haveData)
                    {
                        cell = new Cell<T>(i, j, new Vector2D(left, top),
                        new Vector2D(right, bot),this, dataList[i * this.m_iNumCellsX + j],type);
                    }
                    else 
                    {
                        cell = new Cell<T>(i, j, new Vector2D(left, top),
                        new Vector2D(right, bot),this,null,type);
                    }

                    //cell.cellType = cellType;

                    this.m_Cells.Add(i * this.m_iNumCellsX + j, cell);
                }
            }
        }


        public int PositionToIndex(Vector2D pos)
        {
            int idx = (int)(this.m_iNumCellsX * pos.x / this.m_fSpaceWidth) + ((int)(this.m_iNumCellsY * pos.y / this.m_fSpaceHeight) * this.m_iNumCellsX);
            
            if(idx >(int)this.m_Cells.Count - 1)
            {
                idx = (int)this.m_Cells.Count - 1;
            }

            return idx;
        }

        public Cell<T> GetCellByPos(Vector2D pos) 
        {
            int index = PositionToIndex(pos);

            if(this.m_Cells != null && this.m_Cells.ContainsKey(index))
            {
                return this.m_Cells[index];
            }

            return null;
        }


        public void EmptyCells() 
        {
            foreach (var item in this.m_Cells)
            {
                item.Value.Members.Clear();
            }
        }

        public Cell<T> GetCellByXY(int x , int y) 
        {
            Cell<T> cell = null;

            if(x <CellsXNum && y < CellsYNum)
            {
                int idx = y * this.m_iNumCellsX + x;

                cell = this.m_Cells[idx];
            }

            return cell;
        }

    }
}
