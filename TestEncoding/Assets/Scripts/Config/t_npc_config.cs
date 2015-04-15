using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

[ProtoContract]
public class t_npc_config
{
    private static string configPath = LUtil.GetTablePath();
    private static Dictionary<uint, t_npc_config> list = new Dictionary<uint, t_npc_config>();
    [ProtoMember(5)]
    public uint m_blocktype;
    [ProtoMember(1)]
    public uint m_id;
    [ProtoMember(3)]
    public uint m_kind;
    [ProtoMember(4)]
    public uint m_level;
    [ProtoMember(2)]
    public string m_name;
    [ProtoMember(0x7f)]
    public t_npc_config[] ProtoList;

    public static t_npc_config GetConfig(uint id)
    {
        if (list.ContainsKey(id))
        {
            return list[id];
        }
        LUtil.Log("@@找不到Npc ID:" + id);
        return null;
    }

    public static Dictionary<uint, t_npc_config> GetConfigList()
    {
        return list;
    }

    private static object ProtoDeserialize(Stream source, System.Type r)
    {
        return Serializer.NonGeneric.Deserialize(r, source);
    }

    public static void ReadObject1(string path, System.Type t, Action<object> callBack)
    {
        object obj2 = null;
        try
        {
            using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(configPath + path)))
            {
                if (path.Contains("t_secret_explore_config"))
                {
                    MemoryStream stream2 = stream;
                }
                stream.Position = 0L;
                obj2 = ProtoDeserialize(stream, t);
            }
            callBack(obj2);
        }
        catch (Exception)
        {
        }
    }
}

