using ProtoBuf;
using System;

[ProtoContract]
public class t_effect_config
{
    [ProtoMember(2)]
    public string m_asset;
    [ProtoMember(3)]
    public string m_bind1;
    [ProtoMember(6)]
    public string m_bind2;
    [ProtoMember(11)]
    public string m_desc;
    [ProtoMember(4)]
    public uint m_downloadLevel;
    [ProtoMember(10)]
    public float m_dura;
    [ProtoMember(9)]
    public int m_follow;
    [ProtoMember(1)]
    public uint m_id;
    [ProtoMember(5)]
    public string m_offset1;
    [ProtoMember(7)]
    public string m_offset2;
    [ProtoMember(8)]
    public float m_scale;
    [ProtoMember(0x7f)]
    public t_effect_config[] ProtoList;
}

