using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public partial class t_object_config
{
    //private static string configPath = System.Environment.CurrentDirectory + "\\Data\\";
    //private static string configPath = Util.GetTablePath();

    public static void Init(TaskHandle t)
    {
        Model.ReadObject = ReadObject1;
        Model.ReadObject("t_object_config.dat", typeof(t_object_config), delegate(object o)
        {
            t_object_config config = o as t_object_config;
            foreach (t_object_config i in config.ProtoList)
            {
                list[i.m_id] = i;
            }
            config = null;
            t();
        });
    }

    public static void ReadObject1(string path, Type t, Action<object> callBack)
    {
        object o = null;
        //Debug.LogError("readObject----->" + path);
        CoreLoadHelp.LoadBytes(MonoThread.Instance, Util.GetTablePath(path), delegate(byte[] dat)
        {
            try
            {
                using (MemoryStream fs = new MemoryStream(dat))
                {
                    fs.Position = 0;
                    o = ProtoDeserialize(fs, t);
                }
                callBack(o);
            }
            catch (Exception ex)
            {
                CoreLoadHelp.AddLog(path + "   " + ex.ToString());
                PlayerHandle.Error("Define", "readObject", path + "   " + ex.ToString());
            }
        });

    }

    private static object ProtoDeserialize(Stream source, Type r)
    {
        return ProtoBuf.Serializer.NonGeneric.Deserialize(r, source);
    }


    public static t_object_config GetConfig(uint id)
    {
        t_object_config config = null;
        if (list.ContainsKey(id))
            config = list[id];
        else
            Util.LogWarning("@@找不到道具ID:" + id);

        return config;
    }
    public static Dictionary<uint, t_object_config> GetConfigList()
    {
        return list;
    }
    private static Dictionary<uint, t_object_config> list = new Dictionary<uint, t_object_config>();



}