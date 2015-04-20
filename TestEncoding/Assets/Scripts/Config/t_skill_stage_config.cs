using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_skill_stage_config
{
    [ProtoMember(127)]
    public t_skill_stage_config[] ProtoList;
    [ProtoMember(1)]
    public UInt32 m_id;
    [ProtoMember(2)]
    public string m_title;
    [ProtoMember(3)]
    public UInt32 m_state;
    [ProtoMember(4)]
    public Int32 m_pAtkBase;
    [ProtoMember(5)]
    public float m_pAtkPer;
    [ProtoMember(6)]
    public Int32 m_mAtkBase;
    [ProtoMember(7)]
    public float m_mAtkPer;
    [ProtoMember(8)]
    public Int32 m_addTarget;
    [ProtoMember(9)]
    public Int32 m_addStage;
    [ProtoMember(10)]
    public string m_castStart;
    [ProtoMember(11)]
    public float m_castOffset;
    [ProtoMember(12)]
    public string m_castEnd;
    [ProtoMember(13)]
    public float m_castRange;
    [ProtoMember(14)]
    public Int32 m_castAngle;
    [ProtoMember(15)]
    public float m_castVel;
    [ProtoMember(16)]
    public UInt32 m_castMax;
    [ProtoMember(17)]
    public float m_castGap;
    [ProtoMember(18)]
    public string m_castAi;
    [ProtoMember(19)]
    public float m_castArg;
    [ProtoMember(20)]
    public Int32 m_castSet;
    [ProtoMember(21)]
    public UInt32 m_callId;
    [ProtoMember(22)]
    public float m_hitDelay;
    [ProtoMember(23)]
    public float m_hitDura;
    [ProtoMember(24)]
    public float m_hitGap;
    [ProtoMember(25)]
    public Int32 m_hitSet;
    [ProtoMember(26)]
    public Int32 m_targetCamp;
    [ProtoMember(27)]
    public float m_actTime;
    [ProtoMember(28)]
    public Int32 m_actSet;
    [ProtoMember(29)]
    public UInt32 m_rType;
    [ProtoMember(30)]
    public float m_rWidth;
    [ProtoMember(31)]
    public float m_rArg1;
    [ProtoMember(32)]
    public float m_rArg2;
    [ProtoMember(33)]
    public Int32 m_repeat;
    [ProtoMember(34)]
    public float m_playEx;
    [ProtoMember(35)]
    public UInt32 m_hitKind;
    [ProtoMember(36)]
    public float m_hitDist;
    [ProtoMember(37)]
    public UInt32 m_atkBuff;
    [ProtoMember(38)]
    public UInt32 m_hitBuff;
    [ProtoMember(39)]
    public UInt32 m_atkSp;
    [ProtoMember(40)]
    public UInt32 m_hitSp;
    [ProtoMember(41)]
    public string m_atkAnimName;
    [ProtoMember(42)]
    public string m_atkAnimPost;
    [ProtoMember(43)]
    public UInt32 m_atkCamera;
    [ProtoMember(44)]
    public UInt32 m_hitCamera;
    [ProtoMember(45)]
    public UInt32 m_ownFx;
    [ProtoMember(46)]
    public float m_ownDelay;
    [ProtoMember(47)]
    public UInt32 m_atkFx;
    [ProtoMember(48)]
    public float m_atkDelay;
    [ProtoMember(49)]
    public UInt32 m_atkSound;
    [ProtoMember(50)]
    public UInt32 m_atkCv;
    [ProtoMember(51)]
    public Int32 m_hitAnim;
    [ProtoMember(52)]
    public UInt32 m_hitFx;
    [ProtoMember(53)]
    public UInt32 m_hitSound;
    public t_skill_stage_config() { }
}
