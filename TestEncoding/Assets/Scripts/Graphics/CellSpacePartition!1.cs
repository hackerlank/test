namespace Graphics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class CellSpacePartition<T> where T: NavGraphNode
    {
        private Dictionary<int, Cell<T>> m_Cells;
        private float m_fCellSizeX;
        private float m_fCellSizeY;
        private float m_fSpaceHeight;
        private float m_fSpaceWidth;
        private int m_iNumCellsX;
        private int m_iNumCellsY;

        public delegate void SetCellColorDelegate(int row, int column, Color color);
        private SetCellColorDelegate SetCellColor;

        public CellSpacePartition(float width, float height, int cellsX, int cellsY, object mapData, Type type, Action createMap = null, SetCellColorDelegate _setCellColor = null)
        {
            this.SetCellColor = _setCellColor;
            this.InitCells(width, height, cellsX, cellsY, mapData, type, createMap);
        }

        public void EmptyCells()
        {
            foreach (KeyValuePair<int, Cell<T>> pair in this.m_Cells)
            {
                pair.Value.Members.Clear();
            }
        }

        public Cell<T> GetCellByPos(Vector2D pos)
        {
            int key = this.PositionToIndex(pos);
            if ((this.m_Cells != null) && this.m_Cells.ContainsKey(key))
            {
                return this.m_Cells[key];
            }
            return null;
        }

        public Cell<T> GetCellByXY(int x, int y)
        {
            Cell<T> cell = null;
            if ((x < this.CellsXNum) && (y < this.CellsYNum))
            {
                int num = (y * this.m_iNumCellsX) + x;
                cell = this.m_Cells[num];
            }
            return cell;
        }

        private void InitCells(float width, float height, int cellsX, int cellsY, object mapData, Type type, Action createMap)
        {
            if (createMap != null)
            {
                createMap();
            }
            this.m_fSpaceWidth = width;
            this.m_fSpaceHeight = height;
            this.m_fCellSizeX = width / ((float) cellsX);
            this.m_fCellSizeY = height / ((float) cellsY);
            this.m_iNumCellsX = cellsX;
            this.m_iNumCellsY = cellsY;
            this.m_Cells = new Dictionary<int, Cell<T>>();
            bool flag = true;
            IList list = null;
            if (mapData == null)
            {
                flag = false;
            }
            else
            {
                list = mapData as IList;
            }
            for (int i = 0; i < this.m_iNumCellsY; i++)
            {
                for (int j = 0; j < this.m_iNumCellsX; j++)
                {
                    float x = j * this.m_fCellSizeX;
                    float num4 = x + this.m_fCellSizeX;
                    float y = i * this.m_fCellSizeY;
                    float num6 = y + this.m_fCellSizeY;
                    Cell<T> cell = null;
                    if (flag)
                    {
                        cell = new Cell<T>(i, j, new Vector2D(x, num6), new Vector2D(num4, y), (CellSpacePartition<T>) this, list[(i * this.m_iNumCellsX) + j], type);
                    }
                    else
                    {
                        cell = new Cell<T>(i, j, new Vector2D(x, num6), new Vector2D(num4, y), (CellSpacePartition<T>) this, null, type);
                    }
                    this.m_Cells.Add((i * this.m_iNumCellsX) + j, cell);
                }
            }
        }

        public int PositionToIndex(Vector2D pos)
        {
            int num = ((int) ((this.m_iNumCellsX * pos.x) / this.m_fSpaceWidth)) + (((int) ((this.m_iNumCellsY * pos.y) / this.m_fSpaceHeight)) * this.m_iNumCellsX);
            if (num > (this.m_Cells.Count - 1))
            {
                num = this.m_Cells.Count - 1;
            }
            return num;
        }

        public Dictionary<int, Cell<T>> Cells
        {
            get
            {
                return this.m_Cells;
            }
        }

        public float CellSizeX
        {
            get
            {
                return this.m_fCellSizeX;
            }
        }

        public float CellSizeY
        {
            get
            {
                return this.m_fCellSizeY;
            }
        }

        public float CellSpaceHeight
        {
            get
            {
                return this.m_fSpaceHeight;
            }
        }

        public float CellSpaceWidth
        {
            get
            {
                return this.m_fSpaceWidth;
            }
        }

        public int CellsXNum
        {
            get
            {
                return this.m_iNumCellsX;
            }
            set
            {
                this.m_iNumCellsX = value;
            }
        }

        public int CellsYNum
        {
            get
            {
                return this.m_iNumCellsY;
            }
            set
            {
                this.m_iNumCellsY = value;
            }
        }

    }
}

