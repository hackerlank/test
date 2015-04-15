using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack=1)]
public class stNullUserCmd
{
    public byte byCmd;
    public byte byParam;
    public uint dwTimestamp;
}

