using UnityEngine;
using System.Collections;
using Graphics;
using System.Collections.Generic;

public class ReadMapData 
{
    int NineScreenNumX;
    int NineScreenNumY;

    Transform stoppoints;
    Transform lines;

	// Use this for initialization
	public void ReadMap(string name)
    {
        //MapLoader.LoadMapConfigData("test_shuijing");
        MapLoader.LoadMapConfigData(name);
        Debug.Log("Read map:" + name);
        stoppoints = GameObject.Find("StopPoints").transform;

        lines = GameObject.Find("Lines").transform;

        NineScreenNumX = CurrentMapAccesser.Instance.CellNumX / 13;
        NineScreenNumY = CurrentMapAccesser.Instance.CellNumY / 19;
        ShowBlockPoint();
        MapLoader.LoadMapHightDataByName("MapHightInfo");

#if UNITY_IPHONE || UNITY_ANDROID
        
#else
        DrawNineScreenByLineRender();
#endif

    }


    void ShowBlockPoint()
    {
        for (int i = 0; i <= CellInfos.MapInfos.GetUpperBound(1); i++)
        {
            for (int j = 0; j <= CellInfos.MapInfos.GetUpperBound(0); j++)
            {
                int flag = (int)CellInfos.GetFlagByRowAndColumn((uint)i,(uint)j);

                if (GraphUtils.IsContainsFlag(CellInfos.MapInfos[j,i],map.TileFlag.TILE_BLOCK_NORMAL))
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.parent = stoppoints;
                    go.transform.position = GraphUtils.GetWorldPosByServerPos((uint)j, (uint)i) + new Vector3(0,-1,0);
                    go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }

    void DrawNineScreenByLineRender()
    {
        for (int i = 1; i <= NineScreenNumX; i++)
        {
            DrawLine(GraphUtils.GetWorldPosByServerPos(new Vector2(13 * i, 0)) + new Vector3(0, 0.1f, 0), GraphUtils.GetWorldPosByServerPos(new Vector2(13 * i, CurrentMapAccesser.Instance.CellNumY - 1)) + new Vector3(0, -1f, 0));
        }
        for (int i = 1; i <= NineScreenNumY; i++)
        {
            DrawLine(GraphUtils.GetWorldPosByServerPos(new Vector2(0, 19 * i)) + new Vector3(0, 0.1f, 0), GraphUtils.GetWorldPosByServerPos(new Vector2(CurrentMapAccesser.Instance.CellNumX - 1, 19 * i)) + new Vector3(0, -1f, 0));
        }
    }

    void DrawLine(Vector3 start,Vector3 end,float width = 0.1f)
    {
        GameObject line = new GameObject("line");
        line.transform.parent = lines;
        LineRenderer lr = line.AddComponent<LineRenderer>();
        lr.SetVertexCount(2);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.SetWidth(width,width);
    }
}
