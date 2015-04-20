using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_effect_config
{
    [ProtoMember(127)]
    public t_effect_config[] ProtoList;
    [ProtoMember(1)]
    public UInt32 m_id;
    [ProtoMember(2)]
    public string m_asset;
    [ProtoMember(3)]
    public string m_bind1;
    [ProtoMember(4)]
    public UInt32 m_downloadLevel;
    [ProtoMember(5)]
    public string m_offset1;
    [ProtoMember(6)]
    public string m_bind2;
    [ProtoMember(7)]
    public string m_offset2;
    [ProtoMember(8)]
    public float m_scale;
    [ProtoMember(9)]
    public Int32 m_follow;
    [ProtoMember(10)]
    public float m_dura;
    [ProtoMember(11)]
    public string m_desc;
    public t_effect_config() { }
}
