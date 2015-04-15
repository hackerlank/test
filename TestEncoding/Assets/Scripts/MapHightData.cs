using Graphics;
using System;

public class MapHightData
{
    private int m_cellNumX = -1;
    private int m_cellNumZ = -1;
    private float m_cellSizeX;
    private float m_cellSizeZ;
    private byte[,] m_datas;
    private int m_height = -1;
    private float m_maxHeightDiff = -1f;
    private float m_minHeight = -1f;
    private float m_originX;
    private float m_originZ;
    private float m_realHeight = -1f;
    private float m_realWidth = -1f;
    private int m_width = -1;

    public float GetMapHeight(float x, float z)
    {
        x -= this.m_originX;
        z -= this.m_originZ;
        if ((x > this.m_realWidth) || (z > this.m_realHeight))
        {
            return 0f;
        }
        int column = (int) ((x / this.m_realWidth) * this.m_cellNumX);
        if (column >= this.m_width)
        {
            column = this.m_width - 1;
        }
        int row = (int) ((z / this.m_realHeight) * this.m_cellNumZ);
        if (row >= this.m_height)
        {
            row = this.m_height - 1;
        }
        float num3 = (this.m_realWidth * column) / ((float) (this.m_width - 1));
        float y = (this.m_realHeight * row) / ((float) (this.m_height - 1));
        float vetexHeight = 0f;
        if ((Math.Abs((float) (num3 - x)) < float.Epsilon) && (Math.Abs((float) (y - x)) < float.Epsilon))
        {
            return this.GetVetexHeight(row, column);
        }
        float num6 = 0f;
        if ((((x - num3) + z) - y) <= this.CellSizeX)
        {
            Vector2D vectord = new Vector2D(num3, y);
            Vector2D vectord2 = new Vector2D(num3, y + this.m_cellSizeZ);
            Vector2D vectord3 = new Vector2D(num3 + this.m_cellSizeX, y);
            Vector2D vectord4 = new Vector2D(x, z);
            float num7 = 0f;
            Vector2D point = Vector2D.Zero;
            if (!GraphUtils.LineIntersection2D(vectord, vectord4, vectord2, vectord3, ref num7, ref point))
            {
                return num6;
            }
            vetexHeight = this.GetVetexHeight(row, column);
            float num8 = Vector2D.Distance(point, vectord2) / Vector2D.Distance(vectord3, vectord2);
            float num9 = this.GetVetexHeight(row + 1, column);
            float num10 = this.GetVetexHeight(row, column + 1);
            float num11 = (num9 * (1f - num8)) + (num8 * num10);
            float num12 = Vector2D.Distance(vectord, vectord4) / Vector2D.Distance(vectord, point);
            return ((vetexHeight * (1f - num12)) + (num12 * num11));
        }
        Vector2D a = new Vector2D(num3 + this.m_cellSizeX, y + this.m_cellSizeZ);
        Vector2D c = new Vector2D(num3, y + this.m_cellSizeZ);
        Vector2D d = new Vector2D(num3 + this.m_cellSizeX, y);
        Vector2D b = new Vector2D(x, z);
        float dist = 0f;
        Vector2D zero = Vector2D.Zero;
        if (GraphUtils.LineIntersection2D(a, b, c, d, ref dist, ref zero))
        {
            vetexHeight = this.GetVetexHeight(row + 1, column + 1);
            float num14 = Vector2D.Distance(zero, c) / Vector2D.Distance(d, c);
            num14 = GraphUtils.Clamp01(num14);
            float num15 = this.GetVetexHeight(row + 1, column);
            float num16 = this.GetVetexHeight(row, column + 1);
            float num17 = (num15 * (1f - num14)) + (num14 * num16);
            float num18 = Vector2D.Distance(a, b) / Vector2D.Distance(a, zero);
            num18 = GraphUtils.Clamp01(num18);
            num6 = (vetexHeight * (1f - num18)) + (num18 * num17);
        }
        return num6;
    }

    public float GetVetexHeight(int row, int column)
    {
        return (((this.m_datas[row, column] * this.m_maxHeightDiff) / 255f) + this.minHeight);
    }

    public void Init(int width, int height, float realWidth, float realHeight, float maxHeightDiff, float minHeight, byte[] bytes)
    {
        this.m_width = width;
        this.m_height = height;
        this.m_realWidth = realWidth;
        this.m_realHeight = realHeight;
        this.m_maxHeightDiff = maxHeightDiff;
        this.minHeight = minHeight;
        this.m_cellSizeX = this.m_realWidth / ((float) this.m_width);
        this.m_cellSizeZ = this.m_realHeight / ((float) this.m_height);
        this.m_cellNumX = this.m_width - 1;
        this.m_cellNumZ = this.m_height - 1;
        this.m_originX = -this.m_realWidth / 2f;
        this.m_originZ = -this.m_realHeight / 2f;
        this.m_datas = new byte[this.m_height, this.m_width];
        for (int i = 0; i < this.m_height; i++)
        {
            for (int j = 0; j < this.m_width; j++)
            {
                this.m_datas[i, j] = bytes[(i * this.m_width) + j];
            }
        }
    }

    public int CellNumX
    {
        get
        {
            return this.m_cellNumX;
        }
        set
        {
            this.m_cellNumX = value;
        }
    }

    public int CellNumZ
    {
        get
        {
            return this.m_cellNumZ;
        }
        set
        {
            this.m_cellNumZ = value;
        }
    }

    public float CellSizeX
    {
        get
        {
            return this.m_cellSizeX;
        }
        set
        {
            this.m_cellSizeX = value;
        }
    }

    public float CellSizeZ
    {
        get
        {
            return this.m_cellSizeZ;
        }
        set
        {
            this.m_cellSizeZ = value;
        }
    }

    public byte[,] datas
    {
        get
        {
            return this.m_datas;
        }
        set
        {
            this.m_datas = value;
        }
    }

    public int height
    {
        get
        {
            return this.m_height;
        }
        set
        {
            this.m_height = value;
        }
    }

    public float maxHeightDiff
    {
        get
        {
            return this.m_maxHeightDiff;
        }
        set
        {
            this.m_maxHeightDiff = value;
        }
    }

    public float minHeight
    {
        get
        {
            return this.m_minHeight;
        }
        set
        {
            this.m_minHeight = value;
        }
    }

    public float originX
    {
        get
        {
            return this.m_originX;
        }
        set
        {
            this.m_originX = value;
        }
    }

    public float originZ
    {
        get
        {
            return this.m_originZ;
        }
        set
        {
            this.m_originZ = value;
        }
    }

    public float realHeight
    {
        get
        {
            return this.m_realHeight;
        }
        set
        {
            this.m_realHeight = value;
        }
    }

    public float realWidth
    {
        get
        {
            return this.m_realWidth;
        }
        set
        {
            this.m_realWidth = value;
        }
    }

    public int width
    {
        get
        {
            return this.m_width;
        }
        set
        {
            this.m_width = value;
        }
    }
}

