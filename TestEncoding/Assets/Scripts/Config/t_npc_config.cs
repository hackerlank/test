using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_npc_config
{
    [ProtoMember(127)]
    public t_npc_config[] ProtoList;
        [ProtoMember(1)]
    public UInt32 m_id;
        [ProtoMember(2)]
    public string m_name;
        [ProtoMember(3)]
    public UInt32 m_kind;
        [ProtoMember(4)]
    public UInt32 m_level;
        [ProtoMember(5)]
    public UInt32 m_blocktype;
    public t_npc_config(){}
}
