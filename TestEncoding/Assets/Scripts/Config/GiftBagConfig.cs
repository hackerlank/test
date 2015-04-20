using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Xml;
using System.Security;
using System.Text;
using System;
using Net;

public class GiftBagData
{
    public UInt32 id;
    public UInt32 point;//点数
    public string desc;
}

public class GiftBagConfig : ConfigBase
{
    private bool isCached = false;

    public List<GiftBagData> GiftNagConfigData = new List<GiftBagData>();

    public override void Initialize()
    {
        base.Initialize();
        ParseConfig();
    }

    public override void ParseConfig()
    {
        NetWorkModule.Instance.StartCoroutine(__ParseConfig());

    }

    IEnumerator __ParseConfig()
    {
        GiftNagConfigData.Clear();
        string xmlfile = Util.GetConfigPath("giftbagconfig.xml");

        byte[] stream = null;
        if (xmlfile.Contains("://"))
        {
            WWW www = new WWW(xmlfile);
            yield return www;
            stream = www.bytes;
        }
        else
        {
            stream = File.ReadAllBytes(xmlfile);
        }

        var fs = new MemoryStream(stream);

        StreamReader reader = new StreamReader(fs, Encoding.UTF8);

        SecurityParser MonoXmlParser = new SecurityParser();

        MonoXmlParser.LoadXml(reader.ReadToEnd());

        SecurityElement se = MonoXmlParser.ToXml();

        foreach (SecurityElement child in se.Children)
        {
            if (child.Tag == "item")
            {
                GiftBagData md = new GiftBagData();
                md.id = UInt32.Parse(child.Attribute("id"));
                md.point = UInt32.Parse(child.Attribute("point"));
                md.desc = child.Attribute("desc");

                Debug.Log("itemID:" + md.id + " desc: " + md.desc);

                GiftNagConfigData.Add(md);
            }
        }

        isCached = true;
    }

    public string GetDescByID(UInt32 id)
    {
        if (!isCached)
        {
            ParseConfig();
        }

        foreach (GiftBagData data in GiftNagConfigData)
        {
            if (data.id == id)
            {
                return data.desc;
            }
        }
        return "";
    }

    public UInt32 GetGiftBagCount()
    {
        return (UInt32)GiftNagConfigData.Count;
    }

    public UInt32 GetIDByIndex(Int32 index)
    {
        if (index < GiftNagConfigData.Count)
            return GiftNagConfigData[index].id;

        return 0;
    }

    public GiftBagData GetConfigByIndex(Int32 index)
    {
        if (index < GiftNagConfigData.Count)
            return GiftNagConfigData[index];

        return null;
    }

    public GiftBagData GetConfigByObjectID(uint id)
    {
        foreach (GiftBagData rec in GiftNagConfigData)
        {
            if (rec.id == id)
                return rec;
        }

        return null;
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }
}
