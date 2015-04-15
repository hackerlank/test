namespace map
{
    using ProtoBuf;
    using System;

    [ProtoContract(Name="TileFlag")]
    public enum TileFlag
    {
        [ProtoEnum(Name="TILE_BLOCK_ENTRY", Value=4)]
        TILE_BLOCK_ENTRY = 4,
        [ProtoEnum(Name="TILE_BLOCK_MAGIC", Value=2)]
        TILE_BLOCK_MAGIC = 2,
        [ProtoEnum(Name="TILE_BLOCK_NORMAL", Value=1)]
        TILE_BLOCK_NORMAL = 1
    }
}

