using UnityEngine;
using System.Collections.Generic;
using Graphics;

public static class GraphUtils
{

    public static Vector2DForInteger GetServerPosByWorldPos(Vector3 pos) 
    {
        Vector2DForInteger v = new Vector2DForInteger(-1,-1);

        float x = pos.x;
        float y = pos.z;

        //CellSpacePartition<NavGraphNode> cellSpace = MapDataHolder.Instance.cellSpacePartition;

        if(Mathf.Abs(x) <= CurrentMapAccesser.Instance.realWidth/2 && Mathf.Abs(y) <= CurrentMapAccesser.Instance.realHeight/2)
        {
            pos = pos - CurrentMapAccesser.Instance.WorldOriginPoint;
            pos.z = -pos.z;

            v.x = (int)(pos.x * CurrentMapAccesser.Instance.CellNumX / CurrentMapAccesser.Instance.realWidth);
            if (v.x >= CurrentMapAccesser.Instance.CellNumX)
            {
                v.x = CurrentMapAccesser.Instance.CellNumX - 1;
            }

            v.y = (int)(pos.z * CurrentMapAccesser.Instance.CellNumY / CurrentMapAccesser.Instance.realHeight);
            if (v.y >= CurrentMapAccesser.Instance.CellNumY)
            {
                v.y = CurrentMapAccesser.Instance.CellNumY - 1;
            }
        }

        return v;
    }

    public static Vector3 GetWorldPosByServerPos(Vector2 v)
    {
        return GetWorldPosByServerPos((uint)v.x,(uint)v.y);
    }

    public static Vector3 GetWorldPosByServerPos(uint x , uint y) 
    {
        Vector3 pos = new Vector3(float.MaxValue,0,float.MaxValue);
        //CellSpacePartition<NavGraphNode> cellSpace = CurrentMapAccesser.Instance.cellSpacePartition;
        //Debug.Log("cell x num : "+cellSpace.CellsXNum+" cell y num : "+cellSpace.CellsYNum);
        if (x < CurrentMapAccesser.Instance.CellNumX && y < CurrentMapAccesser.Instance.CellNumY)
        {
            float cellWidth = CurrentMapAccesser.Instance.CellSizeX;
            float halfCellWidth = cellWidth * 0.5f;

            float cellHeight = CurrentMapAccesser.Instance.CellSizeY;
            float halfCellHeight = cellHeight * 0.5f;

            pos.x = halfCellWidth + x * cellWidth;
            pos.z = halfCellHeight + y * cellHeight;
            pos.z = -pos.z;

            pos = CurrentMapAccesser.Instance.WorldOriginPoint + pos;

        }
        else 
        {
            Debug.LogError("X:" + x + " Y:" + y + " MaxX:" + CurrentMapAccesser.Instance.CellNumX + " MaxY:" + CurrentMapAccesser.Instance.CellNumY);
            Debug.LogWarning("The value of the x or y is euqals or lagger to cellspace cells width or height numbers!");
        }

        //pos.y = MapHightDataHolder.GetMapHeight(pos.x,pos.z);

        return pos;
    }

    /// <summary>
    /// 判断某个格子是否属于某种阻挡标记
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static bool IsContainsFlag(uint x , uint y , map.TileFlag flag) 
    {
        //bool result = false;

        //Cell<NavGraphNode> cell = CurrentMapAccesser.Instance.cellSpacePartition.GetCellByXY((int)x,(int)y);

        //if(cell != null)
        //{
        //    map.NewTile tile = cell.node.ExtraInfo as map.NewTile;
            
        //    if (((int)tile.flags & (int)flag) != 0)
        //    {
        //        result = true;
        //    }
        //}

        if (x >= CurrentMapAccesser.Instance.CellNumX || y >= CurrentMapAccesser.Instance.CellNumY)
        {
            throw new System.IndexOutOfRangeException();
            //Debug Error
        }
        else 
        {
            map.TileFlag _flag = CellInfos.GetFlagByRowAndColumn(x,y);

            //if(((int)_flag & (int)flag) != 0 )
            //{
            //    result = true;
            //}

            return IsContainsFlag(_flag, flag);

        }

        //return result;
    }


    public static bool IsContainsFlag(map.TileFlag rflag, map.TileFlag flag)
    {
        bool result = false;
        if (((int)rflag & (int)flag) != 0)
        {
            result = true;
        }
        return result;
    }



    public static List<map.TileFlag> GetFlagsByXY(uint x , uint y) 
    {
        List<map.TileFlag> flagList = null;

        //Cell<NavGraphNode> cell = CurrentMapAccesser.Instance.cellSpacePartition.GetCellByXY((int)x, (int)y);

        //if (cell != null)
        //{
        //    map.NewTile tile = cell.node.ExtraInfo as map.NewTile;

        //    string[] names = System.Enum.GetNames(typeof(map.TileFlag));

        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        map.TileFlag tempFlag = (map.TileFlag)System.Enum.Parse(typeof(map.TileFlag), names[i], true);
        //        int v = (int)tempFlag;

        //        if ((v & (int)tile.flags) != 0)
        //        {
        //            if(flagList == null)
        //            {
        //                flagList = new List<map.TileFlag>();
        //            }
        //            flagList.Add(tempFlag);
        //        }
        //    }
        //}

        if (x >= CurrentMapAccesser.Instance.CellNumX || y >= CurrentMapAccesser.Instance.CellNumY)
        {
            throw new System.IndexOutOfRangeException();
            //Debug Error
        }
        else
        {
            map.TileFlag _flag = CellInfos.GetFlagByRowAndColumn(x, y);

            string[] names = System.Enum.GetNames(typeof(map.TileFlag));

            for (int i = 0; i < names.Length; i++)
            {
                map.TileFlag tempFlag = (map.TileFlag)System.Enum.Parse(typeof(map.TileFlag), names[i], true);
                int v = (int)tempFlag;

                if ((v & (int)_flag) != 0)
                {
                    if (flagList == null)
                    {
                        flagList = new List<map.TileFlag>();
                    }
                    flagList.Add(tempFlag);
                }
            }

        }

        return flagList;
    }



    public static bool LineIntersection2D(Vector2D   A,
                               Vector2D   B,
                               Vector2D   C, 
                               Vector2D   D,
                               ref float     dist,
                               ref Vector2D  point)
    {

        float rTop = (A.y-C.y)*(D.x-C.x)-(A.x-C.x)*(D.y-C.y);
        float rBot = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);

        //float sTop = (A.y - C.y) * (B.x - A.x) - (A.x - C.x) * (B.y - A.y);
        //float sBot = (B.x - A.x) * (D.y - C.y) - (B.y - A.y) * (D.x - C.x);

        //if ( (rBot == 0) || (sBot == 0))   
        //{
        //    //lines are parallel 
        //    return false;
        //}

        if ((rBot == 0))
        {
            //lines are parallel 
            return false;
        }

        float r = rTop / rBot;
        //float s = sTop / sBot;

        dist = Vector2D.Distance(A, B) * r;

        point = A + r * (B - A);

        return true;

        //if ((s > 0) && (s < 1))
        //{
        //    dist = Vector2D.Distance(A, B) * r;

        //    point = A + r * (B - A);

        //    return true;
        //}
        //else
        //{
        //    dist = 0;

        //    return false;
        //}

        //if( (r > 0) && (r < 1) && (s > 0) && (s < 1) )
        //{
        //    dist =Vector2D.Distance(A,B) * r;

        //    point = A + r * (B - A);

        //    return true;
        //}
        //else
        //{
        //    dist = 0;

        //    return false;
        //}
    }



    public static float Clamp(float value, float min, float max)
    {
        if (value < min)
        {
            value = min;
            return value;
        }
        if (value > max)
        {
            value = max;
        }
        return value;
    }

    public static int Clamp(int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
            return value;
        }
        if (value > max)
        {
            value = max;
        }
        return value;
    }

    public static float Clamp01(float value)
    {
        if (value < 0f)
        {
            return 0f;
        }
        if (value > 1f)
        {
            return 1f;
        }
        return value;
    }

}
