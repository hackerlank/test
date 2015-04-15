using System;
using UnityEngine;

public class CurrentMapAccesser : LSingleton<CurrentMapAccesser>
{
    private int m_cellNumX = -1;
    private int m_cellNumY = -1;
    private float m_fCellSizeX;
    private float m_fCellSizeY;
    private float m_realHeight;
    private float m_realWidth;
    private Vector3 m_WorldOriginPoint;

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

    public int CellNumY
    {
        get
        {
            return this.m_cellNumY;
        }
        set
        {
            this.m_cellNumY = value;
        }
    }

    public float CellSizeX
    {
        get
        {
            return this.m_fCellSizeX;
        }
        set
        {
            this.m_fCellSizeX = value;
        }
    }

    public float CellSizeY
    {
        get
        {
            return this.m_fCellSizeY;
        }
        set
        {
            this.m_fCellSizeY = value;
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

    public Vector3 WorldOriginPoint
    {
        get
        {
            return this.m_WorldOriginPoint;
        }
        set
        {
            this.m_WorldOriginPoint = value;
        }
    }
}

