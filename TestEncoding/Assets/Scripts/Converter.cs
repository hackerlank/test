using System;
using System.Runtime.InteropServices;

public class Converter
{
    public static object BytesToStruct(byte[] bytes, System.Type structType)
    {
        object obj2;
        int cb = Marshal.SizeOf(structType);
        IntPtr destination = Marshal.AllocHGlobal(cb);
        try
        {
            Marshal.Copy(bytes, 0, destination, cb);
            obj2 = Marshal.PtrToStructure(destination, structType);
        }
        finally
        {
            Marshal.FreeHGlobal(destination);
        }
        return obj2;
    }

    public static byte[] StructToBytes(object structure)
    {
        byte[] buffer2;
        int cb = Marshal.SizeOf(structure);
        IntPtr ptr = Marshal.AllocHGlobal(cb);
        try
        {
            Marshal.StructureToPtr(structure, ptr, false);
            byte[] destination = new byte[cb];
            Marshal.Copy(ptr, destination, 0, cb);
            buffer2 = destination;
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }
        return buffer2;
    }
}

