using System;
using ProtoBuf;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
[ProtoContract]
public partial class t_object_config
{
    [ProtoMember(127)]
    public t_object_config[] ProtoList;
        [ProtoMember(1)]
    public UInt32 m_id;
        [ProtoMember(2)]
    public string m_name;
        [ProtoMember(3)]
    public UInt32 m_maxcount;
        [ProtoMember(4)]
    public UInt32 m_type;
        [ProtoMember(5)]
    public UInt32 m_needlevel;
        [ProtoMember(6)]
    public Int32 m_pic;
        [ProtoMember(7)]
    public string m_color;
        [ProtoMember(8)]
    public string m_desc;
    public t_object_config(){}
}
