using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class TestMSG
{
    public uint cmd_type;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string username;

    public uint dstID;
    public uint srcID;

    public TestMSG(string s)
    {
        cmd_type = 0;
        username = s;
        dstID = 0;
        srcID = 0;
    }
    
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class TestMSG2
{
    public uint cmd_type;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string username;

    public uint dstID;
    public uint srcID;

    public TestMSG2(string s)
    {
        cmd_type = 0;
        username = s;
        dstID = 0;
        srcID = 0;
    }

}
