using Graphics;
using System;
using System.Collections.Generic;
using UnityEngine;
using map;

public static class GraphUtils
{
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

    public static List<map.TileFlag> GetFlagsByXY(uint x, uint y)
    {
        List<map.TileFlag> list = null;
        if ((x >= LSingleton<CurrentMapAccesser>.Instance.CellNumX) || (y >= LSingleton<CurrentMapAccesser>.Instance.CellNumY))
        {
            throw new IndexOutOfRangeException();
        }
        map.TileFlag flagByRowAndColumn = CellInfos.GetFlagByRowAndColumn(x, y);
        string[] names = Enum.GetNames(typeof(map.TileFlag));
        for (int i = 0; i < names.Length; i++)
        {
            map.TileFlag item = (map.TileFlag)((int)Enum.Parse(typeof(map.TileFlag), names[i], true));
            int num2 = (int) item;
            if ((num2 & (int)flagByRowAndColumn) != 0)
            {
                if (list == null)
                {
                    list = new List<map.TileFlag>();
                }
                list.Add(item);
            }
        }
        return list;
    }

    public static Vector2DForInteger GetServerPosByWorldPos(Vector3 pos)
    {
        Vector2DForInteger integer = new Vector2DForInteger(-1, -1);
        float x = pos.x;
        float z = pos.z;
        if ((Mathf.Abs(x) <= (CurrentMapAccesser.Instance.realWidth / 2f)) && (Mathf.Abs(z) <= (LSingleton<CurrentMapAccesser>.Instance.realHeight / 2f)))
        {
            pos -= CurrentMapAccesser.Instance.WorldOriginPoint;
            pos.z = -pos.z;
            integer.x = (int) ((pos.x * CurrentMapAccesser.Instance.CellNumX) / CurrentMapAccesser.Instance.realWidth);
            if (integer.x >= CurrentMapAccesser.Instance.CellNumX)
            {
                integer.x = LSingleton<CurrentMapAccesser>.Instance.CellNumX - 1;
            }
            integer.y = (int) ((pos.z * CurrentMapAccesser.Instance.CellNumY) / CurrentMapAccesser.Instance.realHeight);
            if (integer.y >= CurrentMapAccesser.Instance.CellNumY)
            {
                integer.y = CurrentMapAccesser.Instance.CellNumY - 1;
            }
        }
        return integer;
    }

    public static Vector3 GetWorldPosByServerPos(Vector2 v)
    {
        return GetWorldPosByServerPos((uint) v.x, (uint) v.y);
    }

    public static Vector3 GetWorldPosByServerPos(uint x, uint y)
    {
        Vector3 vector = new Vector3(float.MaxValue, 0f, float.MaxValue);
        if ((x < LSingleton<CurrentMapAccesser>.Instance.CellNumX) && (y < LSingleton<CurrentMapAccesser>.Instance.CellNumY))
        {
            float cellSizeX = LSingleton<CurrentMapAccesser>.Instance.CellSizeX;
            float num2 = cellSizeX * 0.5f;
            float cellSizeY = LSingleton<CurrentMapAccesser>.Instance.CellSizeY;
            float num4 = cellSizeY * 0.5f;
            vector.x = num2 + (x * cellSizeX);
            vector.z = num4 + (y * cellSizeY);
            vector.z = -vector.z;
            return (LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint + vector);
        }
        Debug.LogError(string.Concat(new object[] { "X:", x, " Y:", y, " MaxX:", LSingleton<CurrentMapAccesser>.Instance.CellNumX, " MaxY:", LSingleton<CurrentMapAccesser>.Instance.CellNumY }));
        Debug.LogWarning("The value of the x or y is euqals or lagger to cellspace cells width or height numbers!");
        return vector;
    }

    public static bool IsContainsFlag(TileFlag rflag, TileFlag flag)
    {
        bool flag2 = false;
        if ((rflag & flag) != ((TileFlag) 0))
        {
            flag2 = true;
        }
        return flag2;
    }

    public static bool IsContainsFlag(uint x, uint y, TileFlag flag)
    {
        if ((x >= LSingleton<CurrentMapAccesser>.Instance.CellNumX) || (y >= LSingleton<CurrentMapAccesser>.Instance.CellNumY))
        {
            throw new IndexOutOfRangeException();
        }
        return IsContainsFlag(CellInfos.GetFlagByRowAndColumn(x, y), flag);
    }

    public static bool LineIntersection2D(Vector2D A, Vector2D B, Vector2D C, Vector2D D, ref float dist, ref Vector2D point)
    {
        float num = ((A.y - C.y) * (D.x - C.x)) - ((A.x - C.x) * (D.y - C.y));
        float num2 = ((B.x - A.x) * (D.y - C.y)) - ((B.y - A.y) * (D.x - C.x));
        if (num2 == 0f)
        {
            return false;
        }
        float num3 = num / num2;
        dist = Vector2D.Distance(A, B) * num3;
        point = A + ((Vector2D) (num3 * (B - A)));
        return true;
    }
}

