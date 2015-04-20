using System;
using UnityEngine;
using System.Collections.Generic;
using Graphics;

public class CurrentMapAccesser : LSingleton<CurrentMapAccesser>
{

    #region Instance fields

    /// <summary>
    /// 以世界坐标系标记的地图原点
    /// </summary>
    private Vector3 m_WorldOriginPoint;


    public Vector3 WorldOriginPoint
    {
        set { this.m_WorldOriginPoint = value; }
        get { return m_WorldOriginPoint; }
    }

    /// <summary>
    /// 存放地图中的所有格子信息
    /// </summary>
    //private CellSpacePartition<NavGraphNode> m_cellSpacePartition;

    //public CellSpacePartition<NavGraphNode> cellSpacePartition
    //{
    //    set
    //    {
    //        m_cellSpacePartition = value;
    //    }
    //    get
    //    {
    //        return m_cellSpacePartition;
    //    }
    //}

    /// <summary>
    /// 地图水平宽度,即水平格子个数
    /// </summary>
    private int m_cellNumX = -1;
    public int CellNumX 
    {
        set { this.m_cellNumX = value; }
        get { return this.m_cellNumX; }
    }


    /// <summary>
    /// 地图垂直宽度，即垂直格子个数
    /// </summary>
    private int m_cellNumY = -1;

    public int CellNumY 
    {
        set { this.m_cellNumY = value; }
        get { return this.m_cellNumY; }
    }

    /// <summary>
    /// 地形在Untiy编辑器下的实际宽度
    /// </summary>
    private float m_realWidth = 0f;

    public float realWidth 
    {
        set
        {
            this.m_realWidth = value;
        }
        get
        {
            return this.m_realWidth;
        }
    }

    /// <summary>
    /// 地形在Untiy编辑器下的实际高度
    /// </summary>
    private float m_realHeight = 0;

    public float realHeight
    {
        set { this.m_realHeight = value; }
        get { return this.m_realHeight; }
    }

    /// <summary>
    /// Cell width
    /// </summary>
    private float m_fCellSizeX;

    public float CellSizeX
    {
        set{this.m_fCellSizeX = value;}
        get{return this.m_fCellSizeX;}
    }

    /// <summary>
    /// Cell height
    /// </summary>
    private float m_fCellSizeY;
    public float CellSizeY
    {
        set{this.m_fCellSizeY = value;}
        get{return this.m_fCellSizeY;}
    }


    #endregion




}

