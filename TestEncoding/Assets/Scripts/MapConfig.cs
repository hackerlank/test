using System;
using System.Collections.Generic;

public class MapConfig : ConfigBase
{
    private bool isCached;
    private Dictionary<int, MapData> MapConfigData = new Dictionary<int, MapData>();

    public MapData GetMapDataByID(int id)
    {
        if (!this.isCached)
        {
            this.ParseConfig();
        }
        if (!this.MapConfigData.ContainsKey(id))
        {
            return null;
        }
        return this.MapConfigData[id];
    }

    public override void Initialize()
    {
        base.Initialize();
        this.ParseConfig();
    }

    public override void ParseConfig()
    {
        this.MapConfigData.Clear();
        MapData data = new MapData {
            MapID = 0x25a,
            MapName = "test_lol6",
            FileName = "testui"
        };
        this.MapConfigData[data.MapID] = data;
        this.isCached = true;
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }
}

