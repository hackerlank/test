using ProtoBuf;
using System;

[ProtoContract]
public class t_skill_lv_config
{
    [ProtoMember(6)]
    public float m_aiCastDist;
    [ProtoMember(5)]
    public uint m_aiCastType;
    [ProtoMember(8)]
    public float m_cd;
    [ProtoMember(7)]
    public string m_cost;
    [ProtoMember(0x11)]
    public string m_desc;
    [ProtoMember(1)]
    public uint m_id;
    [ProtoMember(3)]
    public uint m_level;
    [ProtoMember(15)]
    public uint m_needItemId;
    [ProtoMember(0x10)]
    public uint m_needItemNum;
    [ProtoMember(11)]
    public uint m_needLv;
    [ProtoMember(13)]
    public uint m_needSkillId;
    [ProtoMember(14)]
    public uint m_needSkillLv;
    [ProtoMember(12)]
    public uint m_needSpirit;
    [ProtoMember(10)]
    public uint m_stack;
    [ProtoMember(4)]
    public uint m_stageMax;
    [ProtoMember(9)]
    public float m_subCd;
    [ProtoMember(2)]
    public string m_title;
    [ProtoMember(0x7f)]
    public t_skill_lv_config[] ProtoList;
}

