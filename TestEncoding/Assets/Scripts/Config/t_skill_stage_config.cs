using ProtoBuf;
using System;

[ProtoContract]
public class t_skill_stage_config
{
    [ProtoMember(0x1c)]
    public int m_actSet;
    [ProtoMember(0x1b)]
    public float m_actTime;
    [ProtoMember(9)]
    public int m_addStage;
    [ProtoMember(8)]
    public int m_addTarget;
    [ProtoMember(0x29)]
    public string m_atkAnimName;
    [ProtoMember(0x2a)]
    public string m_atkAnimPost;
    [ProtoMember(0x25)]
    public uint m_atkBuff;
    [ProtoMember(0x2b)]
    public uint m_atkCamera;
    [ProtoMember(50)]
    public uint m_atkCv;
    [ProtoMember(0x30)]
    public float m_atkDelay;
    [ProtoMember(0x2f)]
    public uint m_atkFx;
    [ProtoMember(0x31)]
    public uint m_atkSound;
    [ProtoMember(0x27)]
    public uint m_atkSp;
    [ProtoMember(0x15)]
    public uint m_callId;
    [ProtoMember(0x12)]
    public string m_castAi;
    [ProtoMember(14)]
    public int m_castAngle;
    [ProtoMember(0x13)]
    public float m_castArg;
    [ProtoMember(12)]
    public string m_castEnd;
    [ProtoMember(0x11)]
    public float m_castGap;
    [ProtoMember(0x10)]
    public uint m_castMax;
    [ProtoMember(11)]
    public float m_castOffset;
    [ProtoMember(13)]
    public float m_castRange;
    [ProtoMember(20)]
    public int m_castSet;
    [ProtoMember(10)]
    public string m_castStart;
    [ProtoMember(15)]
    public float m_castVel;
    [ProtoMember(0x33)]
    public int m_hitAnim;
    [ProtoMember(0x26)]
    public uint m_hitBuff;
    [ProtoMember(0x2c)]
    public uint m_hitCamera;
    [ProtoMember(0x16)]
    public float m_hitDelay;
    [ProtoMember(0x24)]
    public float m_hitDist;
    [ProtoMember(0x17)]
    public float m_hitDura;
    [ProtoMember(0x34)]
    public uint m_hitFx;
    [ProtoMember(0x18)]
    public float m_hitGap;
    [ProtoMember(0x23)]
    public uint m_hitKind;
    [ProtoMember(0x19)]
    public int m_hitSet;
    [ProtoMember(0x35)]
    public uint m_hitSound;
    [ProtoMember(40)]
    public uint m_hitSp;
    [ProtoMember(1)]
    public uint m_id;
    [ProtoMember(6)]
    public int m_mAtkBase;
    [ProtoMember(7)]
    public float m_mAtkPer;
    [ProtoMember(0x2e)]
    public float m_ownDelay;
    [ProtoMember(0x2d)]
    public uint m_ownFx;
    [ProtoMember(4)]
    public int m_pAtkBase;
    [ProtoMember(5)]
    public float m_pAtkPer;
    [ProtoMember(0x22)]
    public float m_playEx;
    [ProtoMember(0x1f)]
    public float m_rArg1;
    [ProtoMember(0x20)]
    public float m_rArg2;
    [ProtoMember(0x21)]
    public int m_repeat;
    [ProtoMember(0x1d)]
    public uint m_rType;
    [ProtoMember(30)]
    public float m_rWidth;
    [ProtoMember(3)]
    public uint m_state;
    [ProtoMember(0x1a)]
    public int m_targetCamp;
    [ProtoMember(2)]
    public string m_title;
    [ProtoMember(0x7f)]
    public t_skill_stage_config[] ProtoList;
}

