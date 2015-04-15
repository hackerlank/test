namespace msg
{
    using ProtoBuf;
    using System;

    [ProtoContract(Name="MapDataType")]
    public enum MapDataType
    {
        [ProtoEnum(Name="MAP_DATATYPE_NPC", Value=1)]
        MAP_DATATYPE_NPC = 1,
        [ProtoEnum(Name="MAP_DATATYPE_USER", Value=0)]
        MAP_DATATYPE_USER = 0
    }
}

