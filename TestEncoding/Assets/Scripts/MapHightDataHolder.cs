using System;

public sealed class MapHightDataHolder
{
    private static MapHightData m_cur_mapData;

    public static float GetMapHeight(float x, float z)
    {
        float mapHeight = 0f;
        if (m_cur_mapData != null)
        {
            mapHeight = m_cur_mapData.GetMapHeight(x, z);
        }
        return mapHeight;
    }

    public static MapHightData CurMapData
    {
        get
        {
            return m_cur_mapData;
        }
        set
        {
            m_cur_mapData = value;
        }
    }
}

