using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
using System.IO;
public delegate void ReadObjectHandle(string path, Type t, Action<object> callBack);
public delegate void TaskHandle();
public class Model
{
    public static ReadObjectHandle ReadObject;
}