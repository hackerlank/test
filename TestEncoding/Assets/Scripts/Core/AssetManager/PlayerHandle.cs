using System;
using System.Collections.Generic;
using System.Text;
//using NetworkCommon;
using System.IO;
using System.Diagnostics;

public delegate void UpdateHandle();
public delegate object ProtoDeserialize(Stream source, Type t);
public delegate void DebugLogHandle(object msg);
public class PlayerHandle
{
    public static PlayerInfo PlayerInfo;
    public static LogLevel OutputLogLevel = LogLevel.Debug;
    public static DebugLogHandle DebugHandle;
    private static string[] logFilter = null, tmpStacks = null;
    private static string printTitle = "", filterFileName = "";
    public static void SetLogFilter(string[] fileList)
    {
        logFilter = new string[fileList.Length];
        for (int i = 0; i < fileList.Length; i++)
        {
            logFilter[i] = string.Concat("   at ", fileList[i], ".");
        }
    }
    private static bool checkLogFilter()
    {
        return true;
        if (logFilter != null && logFilter.Length > 0)
        {
            filterFileName = Environment.StackTrace;
            if (filterFileName != null && filterFileName.Length > 0)
            {
                tmpStacks = filterFileName.Split(new char[] { '\n' });
                if (tmpStacks.Length > 3)
                {
                    for (int i = 0; i < logFilter.Length; i++)
                    {
                        filterFileName = logFilter[i];
                        if (tmpStacks[3].IndexOf(filterFileName) > -1)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }
    public static void Debug(object obj, object method, object msg)
    {
        if (DebugHandle == null) return;

        if (OutputLogLevel > LogLevel.Debug) return;
        //string strValue = "对象:" + GetFixedLength(obj) + " 方法:" + GetFixedLength(method) + " 打印:   " + msg;
        string strValue = msg.ToString();
        if (checkLogFilter()) DebugHandle(strValue);
    }

    public static void Debug(object msg)
    {
        if (DebugHandle == null) return;

        if (OutputLogLevel > LogLevel.Debug) return;
        string strValue = printTitle + msg;
        if (checkLogFilter()) DebugHandle(strValue);
    }

    public static DebugLogHandle ErrorHandle;
    public static void Error(object obj, object method, object msg)
    {
        if (ErrorHandle == null) return;

        if (OutputLogLevel > LogLevel.Error) return;
        string strValue = "对象:" + GetFixedLength(obj) + " 方法:" + GetFixedLength(method) + " 打印:   " + msg;
        if (checkLogFilter()) ErrorHandle(strValue);
    }

    public static void Error(object msg)
    {
        if (ErrorHandle == null) return;

        if (OutputLogLevel > LogLevel.Error) return;
        string strValue = printTitle + msg;
        if (checkLogFilter()) ErrorHandle(strValue);
    }

    public static DebugLogHandle WarningHandle;
    public static void Warning(object obj, object method, object msg)
    {
        if (WarningHandle == null) return;

        if (OutputLogLevel > LogLevel.Warning) return;
        string strValue = "对象:" + GetFixedLength(obj) + " 方法:" + GetFixedLength(method) + " 打印:   " + msg;
        if (checkLogFilter()) WarningHandle(strValue);
    }

    public static void Warning(object msg)
    {
        if (WarningHandle == null) return;

        if (OutputLogLevel > LogLevel.Warning) return;
        string strValue = printTitle + msg;
        if (checkLogFilter()) WarningHandle(strValue);
    }

    private static string GetFixedLength(object obj)
    {
        string strValue = obj.ToString();
        for (int i = strValue.Length; i < 20; i++)
        {
            strValue +=" ";
        }

        return strValue;
    }

    public static Action<UpdateHandle> InvokeHandle;
    public static Action<byte> OnRestartServer; // 当服务器重启时
    public static void Invoke<T>(Action<T> f, T t)
    {
        InvokeHandle(delegate()
        {
            f(t);
        });
    }
    public static void Invoke(UpdateHandle handle)
    {
        InvokeHandle(handle);
    }
#if false //wangqingguo
    public static void Init(ProtoDeserialize _protoDeserializeHandle, ReadObjectHandle readObj)
    {
        ProtoDeserializeHandle = _protoDeserializeHandle;
        Model.ReadObject = readObj;
    }
    public static ProtoDeserialize ProtoDeserializeHandle;
#endif
}
public enum LogLevel
{
    Debug = 0,
    Warning,
    Error,
    NoLog
}