using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_skill_lv_config
{
    [ProtoMember(127)]
    public t_skill_lv_config[] ProtoList;
    [ProtoMember(1)]
    public UInt32 m_id;
    [ProtoMember(2)]
    public string m_title;
    [ProtoMember(3)]
    public UInt32 m_level;
    [ProtoMember(4)]
    public UInt32 m_stageMax;
    [ProtoMember(5)]
    public UInt32 m_aiCastType;
    [ProtoMember(6)]
    public float m_aiCastDist;
    [ProtoMember(7)]
    public string m_cost;
    [ProtoMember(8)]
    public float m_cd;
    [ProtoMember(9)]
    public float m_subCd;
    [ProtoMember(10)]
    public UInt32 m_stack;
    [ProtoMember(11)]
    public UInt32 m_needLv;
    [ProtoMember(12)]
    public UInt32 m_needSpirit;
    [ProtoMember(13)]
    public UInt32 m_needSkillId;
    [ProtoMember(14)]
    public UInt32 m_needSkillLv;
    [ProtoMember(15)]
    public UInt32 m_needItemId;
    [ProtoMember(16)]
    public UInt32 m_needItemNum;
    [ProtoMember(17)]
    public string m_desc;
    public t_skill_lv_config() { }
}
