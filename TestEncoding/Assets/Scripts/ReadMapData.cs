using map;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ReadMapData
{
    private Transform lines;
    private int NineScreenNumX;
    private int NineScreenNumY;
    private Transform stoppoints;

    private void DrawLine(Vector3 start, Vector3 end, float width = 0.1f)
    {
        LineRenderer renderer = new GameObject("line") { transform = { parent = this.lines } }.AddComponent<LineRenderer>();
        renderer.SetVertexCount(2);
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);
        renderer.SetWidth(width, width);
    }

    private void DrawNineScreenByLineRender()
    {
        for (int i = 1; i <= this.NineScreenNumX; i++)
        {
            this.DrawLine(GraphUtils.GetWorldPosByServerPos(new Vector2((float) (13 * i), 0f)) + new Vector3(0f, 0.1f, 0f), GraphUtils.GetWorldPosByServerPos(new Vector2((float) (13 * i), (float) (LSingleton<CurrentMapAccesser>.Instance.CellNumY - 1))) + new Vector3(0f, -1f, 0f), 0.1f);
        }
        for (int j = 1; j <= this.NineScreenNumY; j++)
        {
            this.DrawLine(GraphUtils.GetWorldPosByServerPos(new Vector2(0f, (float) (0x13 * j))) + new Vector3(0f, 0.1f, 0f), GraphUtils.GetWorldPosByServerPos(new Vector2((float) (LSingleton<CurrentMapAccesser>.Instance.CellNumX - 1), (float) (0x13 * j))) + new Vector3(0f, -1f, 0f), 0.1f);
        }
    }

    public void ReadMap(string name)
    {
        MapLoader.LoadMapConfigData(name);
        Debug.Log("Read map:" + name);
        this.stoppoints = GameObject.Find("StopPoints").transform;
        this.lines = GameObject.Find("Lines").transform;
        this.NineScreenNumX = LSingleton<CurrentMapAccesser>.Instance.CellNumX / 13;
        this.NineScreenNumY = LSingleton<CurrentMapAccesser>.Instance.CellNumY / 0x13;
        this.ShowBlockPoint();
        MapLoader.LoadMapHightDataByName("MapHightInfo");
    }

    private void ShowBlockPoint()
    {
        for (int i = 0; i <= CellInfos.MapInfos.GetUpperBound(1); i++)
        {
            for (int j = 0; j <= CellInfos.MapInfos.GetUpperBound(0); j++)
            {
                int flagByRowAndColumn = (int) CellInfos.GetFlagByRowAndColumn((uint) i, (uint) j);
                if (GraphUtils.IsContainsFlag(CellInfos.MapInfos[j, i], TileFlag.TILE_BLOCK_NORMAL))
                {
                    GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj2.transform.parent = this.stoppoints;
                    obj2.transform.position = GraphUtils.GetWorldPosByServerPos((uint) j, (uint) i) + new Vector3(0f, -1f, 0f);
                    obj2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }
}

