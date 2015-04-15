using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack=1)]
public class stUserInfoUserCmd : stSelectUserCmd
{
    public uint id;
    public StringLengthFlag L33;
    public string name;
    public ushort type;
    public ushort level;
    public uint mapid;
    public StringLengthFlag L033;
    public string mapName;
    public ushort country;
    public ushort face;
    public StringLengthFlag L0033;
    public string countryName;
    public uint bitmask;
    public ulong forbidTime;
    public uint zone_state;
    public uint target_zone;
    public uint round;
    public uint icqmask;
    public byte acceptPK;
    public uint oldPlayerLastTime;
    public uint id2;
    public StringLengthFlag L00033;
    public string name2;
    public ushort type2;
    public ushort level2;
    public uint mapid2;
    public StringLengthFlag L000033;
    public string mapName2;
    public ushort country2;
    public ushort face2;
    public StringLengthFlag L0000033;
    public string countryName2;
    public uint bitmask2;
    public ulong forbidTime2;
    public uint zone_state2;
    public uint target_zone2;
    public uint round2;
    public uint icqmask2;
    public byte acceptPK2;
    public uint oldPlayerLastTime2;
    public uint size;
    public stUserInfoUserCmd()
    {
        base.byParam = 1;
        this.size = 0;
    }
}

