#if DebugFromWyb
using UnityEngine;
#endif

using System.Collections;

public sealed class MapHightDataHolder
{

    #region Static Fields

    private static MapHightData m_cur_mapData;

    public static MapHightData CurMapData 
    {
        set 
        {
            m_cur_mapData = value;
        }
        get 
        {
            return m_cur_mapData;
        }
    }

    #endregion


    public static float GetMapHeight(float x , float z) 
    {

        float height = 0;
        if(m_cur_mapData != null)
        {
            height = m_cur_mapData.GetMapHeight(x,z);
        }

        //height = height - 1.5f;
        return height;
    }


#if DebugFromWyb

    public static Vector3[] GetPoints() 
    {
        if(m_cur_mapData != null)
        {
            return new Vector3[] {
                m_cur_mapData._A,
                m_cur_mapData._B,
                m_cur_mapData._C,
                m_cur_mapData._D,
                m_cur_mapData._E,
            };
        }
        return null;
    }

                
#endif
    


}
