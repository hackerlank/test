using map;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine;

public sealed class MapLoader
{
    public static void LoadMapConfigData(string name)
    {
        TextAsset asset = Resources.Load("GameMaps/" + name) as TextAsset;
        Debug.Log("GameMaps/" + name);
        MemoryStream source = new MemoryStream(asset.bytes);
        try
        {
            GameMapFile file = Serializer.Deserialize<GameMapFile>(source);
            int width = (int) file.fileHeader.width;
            int height = (int) file.fileHeader.height;
            LSingleton<CurrentMapAccesser>.Instance.CellNumX = width;
            LSingleton<CurrentMapAccesser>.Instance.CellNumY = height;
            float num3 = file.fileHeader.real_width;
            float num4 = file.fileHeader.real_height;
            LSingleton<CurrentMapAccesser>.Instance.realWidth = num3;
            LSingleton<CurrentMapAccesser>.Instance.realHeight = num4;
            LSingleton<CurrentMapAccesser>.Instance.CellSizeX = num3 / ((float) width);
            LSingleton<CurrentMapAccesser>.Instance.CellSizeY = num4 / ((float) height);
            CellInfos.FillCellInfos(file.tiles);
            LSingleton<CurrentMapAccesser>.Instance.WorldOriginPoint = new Vector3(-num3 * 0.5f, 0f, num4 * 0.5f);
        }
        catch (Exception exception)
        {
            Debug.Log("Error : " + exception.Message);
        }
        finally
        {
            if (source != null)
            {
                source.Dispose();
            }
        }
    }

    public static void LoadMapHightDataByName(string name)
    {
        TextAsset asset = Resources.Load("GameMaps/" + name) as TextAsset;
        using (MemoryStream stream = new MemoryStream(asset.bytes))
        {
            BinaryReader reader = new BinaryReader(stream);
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            float realWidth = reader.ReadSingle();
            float realHeight = reader.ReadSingle();
            float maxHeightDiff = reader.ReadSingle();
            float minHeight = reader.ReadSingle();
            byte[] bytes = reader.ReadBytes(width * height);
            MapHightData data = new MapHightData();
            data.Init(width, height, realWidth, realHeight, maxHeightDiff, minHeight, bytes);
            MapHightDataHolder.CurMapData = data;
        }
    }
}

