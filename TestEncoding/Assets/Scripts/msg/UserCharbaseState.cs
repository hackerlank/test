namespace msg
{
    using ProtoBuf;
    using System;

    [ProtoContract(Name="UserCharbaseState")]
    public enum UserCharbaseState
    {
        [ProtoEnum(Name="CHARBASE_FORBID", Value=2)]
        CHARBASE_FORBID = 2,
        [ProtoEnum(Name="CHARBASE_FORBID_NAME", Value=4)]
        CHARBASE_FORBID_NAME = 4,
        [ProtoEnum(Name="CHARBASE_OK", Value=1)]
        CHARBASE_OK = 1
    }
}

