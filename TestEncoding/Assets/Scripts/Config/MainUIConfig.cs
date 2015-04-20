using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mono.Xml;
using System.Security;
using System.Text;
using System;
using Net;

public class MainUIData
{
    public UInt32 id;
    public string pic;
}

public class MainUIConfig : ConfigBase
{
    private bool isCached = false;

    public List<MainUIData> mConfigData = new List<MainUIData>();

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
        mConfigData.Clear();
        string xmlfile = Util.GetConfigPath("mainuiconfig.xml");

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
                MainUIData md = new MainUIData();
                md.id = UInt32.Parse(child.Attribute("id"));
                md.pic = child.Attribute("pic");

                Util.Log("itemID:" + md.id + " pic: " + md.pic);

                mConfigData.Add(md);
            }
        }

        isCached = true;
    }

    public string GetPicNameByID(UInt32 id)
    {
        if (!isCached)
        {
            ParseConfig();
        }

        foreach (MainUIData data in mConfigData)
        {
            if (data.id == id)
            {
                return data.pic;
            }
        }
        return "";
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
    }
}
