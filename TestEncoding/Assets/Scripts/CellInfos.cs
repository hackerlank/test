using map;
using System;
using System.Collections.Generic;

public sealed class CellInfos
{
    private static TileFlag[,] infos;

    public static void FillCellInfos(List<NewTile> tiles)
    {
        if (tiles != null)
        {
            int count = tiles.Count;
            infos = new TileFlag[LSingleton<CurrentMapAccesser>.Instance.CellNumY, LSingleton<CurrentMapAccesser>.Instance.CellNumX];
            for (int i = 0; i < LSingleton<CurrentMapAccesser>.Instance.CellNumY; i++)
            {
                for (int j = 0; j < LSingleton<CurrentMapAccesser>.Instance.CellNumX; j++)
                {
                    NewTile tile = tiles[(i * CurrentMapAccesser.Instance.CellNumY) + j];
                    infos[i, j] = (TileFlag) tile.flags;
                }
            }
        }
    }

    public static TileFlag GetFlagByRowAndColumn(uint row, uint column)
    {
        TileFlag flag = (TileFlag) 0;
        if (infos == null)
        {
            throw new NullReferenceException();
        }
        if ((row >= LSingleton<CurrentMapAccesser>.Instance.CellNumY) || (column >= LSingleton<CurrentMapAccesser>.Instance.CellNumX))
        {
            throw new IndexOutOfRangeException();
        }
        return infos[row, column];
    }

    public static TileFlag[,] MapInfos
    {
        get
        {
            return infos;
        }
    }
}

