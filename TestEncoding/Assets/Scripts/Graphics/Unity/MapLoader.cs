using UnityEngine;
using System.Collections.Generic;
using Graphics;
using System.IO;

public sealed class MapLoader
{
   
    #region static methods

    public static void LoadMapConfigData(string name) 
    {
        TextAsset text = Resources.Load(@"GameMaps/" + name) as TextAsset;
        Debug.Log("GameMaps/" + name);
        byte[] bytes = text.bytes;

        using (MemoryStream ms = new MemoryStream(bytes))
        {
            try
            {
                map.GameMapFile gameMap = ProtoBuf.Serializer.Deserialize<map.GameMapFile>(ms);

                int cellNumX = (int)gameMap.fileHeader.width;
                int cellNumY = (int)gameMap.fileHeader.height;

                CurrentMapAccesser.Instance.CellNumX = cellNumX;
                CurrentMapAccesser.Instance.CellNumY = cellNumY;


                float mWidth = gameMap.fileHeader.real_width;
                float mHeight = gameMap.fileHeader.real_height;

                CurrentMapAccesser.Instance.realWidth = mWidth;
                CurrentMapAccesser.Instance.realHeight = mHeight;

                CurrentMapAccesser.Instance.CellSizeX = mWidth / cellNumX;

                CurrentMapAccesser.Instance.CellSizeY = mHeight / cellNumY;

                CellInfos.FillCellInfos(gameMap.tiles);
                //CurrentMapAccesser.Instance.cellSpacePartition = new CellSpacePartition<NavGraphNode>(mWidth, mHeight, cellNumX, cellNumY, gameMap.tiles, typeof(map.NewTile));

                CurrentMapAccesser.Instance.WorldOriginPoint = new Vector3(-mWidth * 0.5f, 0, mHeight * 0.5f);

            }
            catch (System.Exception ex)
            {
                Debug.Log("Error : " + ex.Message);
            }
            finally 
            {
            
            
            }
        }
    }


    public static void LoadMapHightDataByName(string name) 
    {
        TextAsset text = Resources.Load(@"GameMaps/" + name) as TextAsset;

        using (MemoryStream ms = new MemoryStream(text.bytes))
        {
            BinaryReader br = new BinaryReader(ms);

            int tempWidth = br.ReadInt32();
            int tempHeight = br.ReadInt32();
            float realWidth = br.ReadSingle();
            float realHeight = br.ReadSingle();
            float maxHeightDiff = br.ReadSingle();
            float minHeight = br.ReadSingle();
            byte[] bytes = br.ReadBytes(tempWidth * tempHeight);

            MapHightData mhd = new MapHightData();
            mhd.Init(tempWidth,tempHeight,realWidth,realHeight,maxHeightDiff,minHeight,bytes);

            MapHightDataHolder.CurMapData = mhd;
        }


        
    }

    #endregion

}
