using ProtoBuf;
using System;

[ProtoContract]
public class t_state_config
{
    [ProtoMember(6)]
    public string m_desc;
    [ProtoMember(5)]
    public uint m_effect;
    [ProtoMember(4)]
    public uint m_ico;
    [ProtoMember(1)]
    public uint m_id;
    [ProtoMember(2)]
    public string m_stateString;
    [ProtoMember(3)]
    public string m_title;
    [ProtoMember(0x7f)]
    public t_state_config[] ProtoList;
}

