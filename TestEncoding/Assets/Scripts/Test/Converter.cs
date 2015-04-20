using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class Converter 
{
    public static byte[] StructToBytes(System.Object structure)
    {
        int size = Marshal.SizeOf(structure);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.StructureToPtr(structure, buffer, false);
            byte[] bytes = new byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    public static System.Object BytesToStruct(byte[] bytes, Type structType)
    {
        int size = Marshal.SizeOf(structType);
        IntPtr buffer = Marshal.AllocHGlobal(size);
        try
        {
            Marshal.Copy(bytes, 0, buffer, size);
            return Marshal.PtrToStructure(buffer, structType);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

}
