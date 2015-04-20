/*
***********************************************************************************************************
* CLR version ：$clrversion$
* Machine name ：$machinename$
* Creation time ：#time#
* Author ：hym
* Version number : 1.0
***********************************************************************************************************
*/
using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class t_npc_config 
{

    //private static string configPath = Util.GetTablePath();

    //public static void Init(TaskHandle t)
    //{
    //    Model.ReadObject = ReadObject1;
    //    Model.ReadObject("t_npc_config.dat", typeof(t_npc_config), delegate(object o)
    //    {
    //        t_npc_config config = o as t_npc_config;
    //        foreach (t_npc_config i in config.ProtoList)
    //        {
    //            list[i.m_id] = i;
    //        }
    //        config = null;
    //        t();
    //    });
    //}

    public static void ReadObject1(string path, Type t, Action<object> callBack)
    {
        object o = null;
        try
        {
            using (MemoryStream fs = new MemoryStream(File.ReadAllBytes(Util.GetTablePath(path))))
            {
                if (path.Contains("t_secret_explore_config"))
                {

                    var temp = fs;
                }

                fs.Position = 0;
                o = ProtoDeserialize(fs, t);

            }
            callBack(o);
        }
        catch (Exception ex)
        {
            //PlayerHandle.Error("Form1", "ReadObject", ex.ToString());
        }
    }

    private static object ProtoDeserialize(Stream source, Type r)
    {
        return ProtoBuf.Serializer.NonGeneric.Deserialize(r, source);
    }


    public static t_npc_config GetConfig(uint id)
    {
        t_npc_config config = null;
        if (list.ContainsKey(id))
            config = list[id];
        else
            Util.LogWarning("@@找不到Npc ID:" + id);

        return config;
    }
    public static Dictionary<uint, t_npc_config> GetConfigList()
    {
        return list;
    }
    private static Dictionary<uint, t_npc_config> list = new Dictionary<uint, t_npc_config>();
}
