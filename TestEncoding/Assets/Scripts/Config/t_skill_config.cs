using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_skill_config
{
    [ProtoMember(127)]
    public t_skill_config[] ProtoList;
    [ProtoMember(1)]
    public UInt32 m_id;
    [ProtoMember(2)]
    public string m_title;
    [ProtoMember(3)]
    public UInt32 m_ico;
    [ProtoMember(4)]
    public UInt32 m_type;
    [ProtoMember(5)]
    public UInt32 m_job;
    [ProtoMember(6)]
    public UInt32 m_kind;
    [ProtoMember(7)]
    public UInt32 m_priority;
    [ProtoMember(8)]
    public UInt32 m_castType;
    [ProtoMember(9)]
    public UInt32 m_lvMax;
    [ProtoMember(10)]
    public UInt32 m_stage2Id;
    [ProtoMember(11)]
    public UInt32 m_stage2LvMax;
    [ProtoMember(12)]
    public UInt32 m_stage3Id;
    [ProtoMember(13)]
    public UInt32 m_stage3LvMax;
    public t_skill_config() { }
}