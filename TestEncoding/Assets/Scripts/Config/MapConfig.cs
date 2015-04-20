using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Xml;
using System.Security;
using System.Text;

public class MapData
{
    public int MapID;
    public string MapName;
    public string FileName;
}

public class MapConfig : ConfigBase
{
    private bool isCached = false;

    private Dictionary<int, MapData> MapConfigData = new Dictionary<int, MapData>();

    public override void Initialize()
    {
        base.Initialize();
        ParseConfig();
    }

    public override void ParseConfig()
    {
        MapConfigData.Clear();

        //string xmlfile = GamePath.Config + "scenesinfo.xml";

        //var fs = new MemoryStream(File.ReadAllBytes(xmlfile));

        //StreamReader reader = new StreamReader(fs, Encoding.UTF8);

        //SecurityParser MonoXmlParser = new SecurityParser();

        //MonoXmlParser.LoadXml(reader.ReadToEnd());

        //SecurityElement se = MonoXmlParser.ToXml();

        //foreach (SecurityElement child in se.Children)
        //{
        //    if (child.Tag == "mapinfo")
        //    {
        //        MapData md = new MapData();
        //        md.MapID = int.Parse(child.Attribute("mapID"));
        //        md.MapName = child.Attribute("name");
        //        md.FileName = child.Attribute("fileName");

        //        Debug.Log("mapID:" + md.MapID + " name:" + md.MapName + " fileName:" + md.FileName);

        //        MapConfigData[md.MapID] = md;
        //    }
        //}

        MapData md = new MapData();
        md.MapID = 602;
        md.MapName = "test_lol6";
        md.FileName = "test_lol";

        MapConfigData[md.MapID] = md;

        isCached = true;
    }

    public MapData GetMapDataByID(int id)
    {
        if (!isCached)
        {
            ParseConfig();
        }
        if(!MapConfigData.ContainsKey(id))
        {
            return null;
        }
        return MapConfigData[id];
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }
}
