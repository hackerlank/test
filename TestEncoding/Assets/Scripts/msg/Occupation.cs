namespace msg
{
    using ProtoBuf;
    using System;

    [ProtoContract(Name="Occupation")]
    public enum Occupation
    {
        [ProtoEnum(Name="Occu_Dema", Value=1)]
        Occu_Dema = 1,
        [ProtoEnum(Name="Occu_Yaohu", Value=2)]
        Occu_Yaohu = 2
    }
}

