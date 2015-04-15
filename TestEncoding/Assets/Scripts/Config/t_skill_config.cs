using ProtoBuf;
using System;

[ProtoContract]
public class t_skill_config
{
    [ProtoMember(8)]
    public uint m_castType;
    [ProtoMember(3)]
    public uint m_ico;
    [ProtoMember(1)]
    public uint m_id;
    [ProtoMember(5)]
    public uint m_job;
    [ProtoMember(6)]
    public uint m_kind;
    [ProtoMember(9)]
    public uint m_lvMax;
    [ProtoMember(7)]
    public uint m_priority;
    [ProtoMember(10)]
    public uint m_stage2Id;
    [ProtoMember(11)]
    public uint m_stage2LvMax;
    [ProtoMember(12)]
    public uint m_stage3Id;
    [ProtoMember(13)]
    public uint m_stage3LvMax;
    [ProtoMember(2)]
    public string m_title;
    [ProtoMember(4)]
    public uint m_type;
    [ProtoMember(0x7f)]
    public t_skill_config[] ProtoList;
}

