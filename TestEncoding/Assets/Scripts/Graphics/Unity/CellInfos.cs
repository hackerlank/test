using UnityEngine;
using System.Collections.Generic;

public sealed class CellInfos
{
    private static map.TileFlag[,] infos;

    public static map.TileFlag[,] MapInfos
    {
        get
        {
            return infos;
        }
    }

    public static void FillCellInfos(List<map.NewTile> tiles) 
    {
        if (tiles != null)
        {
            int count = tiles.Count;

            infos = new map.TileFlag[CurrentMapAccesser.Instance.CellNumY, CurrentMapAccesser.Instance.CellNumX];
            
            for (int i = 0; i < CurrentMapAccesser.Instance.CellNumY; i++)
            {
                for (int j = 0; j < CurrentMapAccesser.Instance.CellNumX; j++)
                {
                    map.NewTile tile = tiles[i * CurrentMapAccesser.Instance.CellNumY + j];

                    infos[i, j] = (map.TileFlag)tile.flags;
                }
            }
        }
        else 
        {
            //Logger Error
        }
    }



    public static map.TileFlag GetFlagByRowAndColumn(uint row , uint column) 
    {
        map.TileFlag result = (map.TileFlag)0;

        if (infos != null)
        {
            if (row < CurrentMapAccesser.Instance.CellNumY && column < CurrentMapAccesser.Instance.CellNumX)
            {
                result = infos[row, column];
            }
            else
            {
                throw new System.IndexOutOfRangeException();
            }
        }
        else 
        {
            throw new System.NullReferenceException();
        }

        return result;
    }


}
