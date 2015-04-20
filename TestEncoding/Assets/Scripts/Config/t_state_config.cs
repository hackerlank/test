using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_state_config
{
    [ProtoMember(127)]
    public t_state_config[] ProtoList;
        [ProtoMember(1)]
    public UInt32 m_id;
        [ProtoMember(2)]
    public string m_stateString;
        [ProtoMember(3)]
    public string m_title;
        [ProtoMember(4)]
    public UInt32 m_ico;
        [ProtoMember(5)]
    public UInt32 m_effect;
        [ProtoMember(6)]
    public string m_desc;
    public t_state_config(){}
}
