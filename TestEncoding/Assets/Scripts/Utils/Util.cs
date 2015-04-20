using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Util
{
    private static StringBuilder getPathResult = new StringBuilder();
    private static string tmpPath = string.Empty;
    //warning日志过早发服务器，影响登陆
    public static bool canSendWarningDebugLog = false;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public static bool writeLog = true; //是否写日志;
#else
    public static bool writeLog = true; //是否写日志
#endif
    public static bool showDebugInfo = true;//是否显示调试信息

    private static float invokeGap = 0.03f;

    //fileLogLevel:写日志等级    0不写日志 1写Normal日志 2写Warning日志 4写Error日志 7(1+2+4)表示同时写Normal Warning Error日志
    public static int fileLogLevel = 7;
    //发送日志等级  0不发日志 1发Normal日志 2发Warning日志 4发Error日志 7(1+2+4)表示同时写Normal Warning Error日志
    public static int sendLogLevel = 0;
    //Normal日志的功能类型
    public static int logFuncType = 1;

    private static ulong logIndex = 1;

    /// <summary>
    /// 检查状态列表里是否有某个状态
    /// </summary>
    /// <param name="state">状态列表</param>
    /// <param name="teststate">要检查的状态</param>
    /// <returns></returns>
    public static bool isset_state(char[] state, int teststate)
    {
        return 0 != (state[teststate / 8] & (0xff & (1 << (teststate % 8))));
    }

    /// <summary>
    /// 设置某个状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="teststate"></param>
    public static void set_state(ref char[] state, int teststate)
    {
        if (teststate >= (int)UserState.USTATE_MAX)
        {
            Debug.LogError("超过最大状态数: " + teststate);
            return;
        }
        state[teststate / 8] |= (char)(0xff & (1 << (teststate % 8)));
    }

    /// <summary>
    /// 清除某个状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="teststate"></param>
    public static void clear_state(ref char[] state, int teststate)
    {
        if (teststate >= (int)UserState.USTATE_MAX)
        {
            Debug.LogError("超过最大状态数: " + teststate);
            return;
        }
        state[teststate / 8] &= (char)(0xff & (~(1 << (teststate % 8))));
    }

    /// <summary>
    /// 客户端方向转服务器方向
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static uint GetServerDir(Vector3 dir)
    {
        float angel = Vector3.Angle(new Vector3(0, 0, 1), new Vector3(dir.x, 0, dir.z));
        //三四象限处理
        if ((dir.x < 0) && (angel < 180f))
            angel = 360 - angel;
        return (uint)((angel + 22.5) / 45);
    }

    /// <summary>
    /// 服务器方向转客户端方向
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Vector3 GetClientDir(uint dir)
    {
        if (dir == 0)
            return new Vector3(0, 0, 1);
        if (dir == 1)
            return new Vector3(1, 0, 1);
        if (dir == 2)
            return new Vector3(1, 0, 0);
        if (dir == 3)
            return new Vector3(1, 0, -1);
        if (dir == 4)
            return new Vector3(0, 0, -1);
        if (dir == 5)
            return new Vector3(-1, 0, -1);
        if (dir == 6)
            return new Vector3(-1, 0, 0);
        if (dir == 7)
            return new Vector3(-1, 0, 1);

        return Vector3.zero;
    }


    public static string GetTablePath(string path)
    {
//        string dataPath = "";
//#if UNITY_EDITOR
//        dataPath = Application.dataPath + "/StreamingAssets/Data/";
//#elif UNITY_STANDALONE_WIN
//     dataPath = Application.dataPath + "/StreamingAssets/Data/";
//#elif UNITY_ANDROID
//     dataPath = "jar:file://" + Application.dataPath + "!/assets/Data/";
//#elif UNITY_IPHONE
//    dataPath = Application.dataPath + "/Raw/Data/";
//#else

//#endif
//        return dataPath;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        getPathResult.Length = 0;
        getPathResult.Append("file:///");
        getPathResult.Append(Application.dataPath);
        getPathResult.Append("/StreamingAssets/Data/");
        getPathResult.Append(path);
        tmpPath = getPathResult.ToString();

#elif UNITY_IPHONE
            getPathResult.Length = 0;
            getPathResult.Append(Application.persistentDataPath);
            getPathResult.Append("/Data/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
            if (!Root.IsApkNewestVersion() && File.Exists(tmpPath))
            {
                getPathResult.Length = 0;
                getPathResult.Append("file:///");
                getPathResult.Append(Application.persistentDataPath);
                getPathResult.Append("/Data/");
                getPathResult.Append(path);
                tmpPath = getPathResult.ToString();
                return tmpPath;
            }
            getPathResult.Length = 0;
            getPathResult.Append("file:///");
            getPathResult.Append(Application.dataPath);
            getPathResult.Append("/Raw/Data/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
#elif UNITY_ANDROID
            getPathResult.Length = 0;
            getPathResult.Append(Application.persistentDataPath);
            getPathResult.Append("/Data/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
            if (!Root.IsApkNewestVersion() && File.Exists(tmpPath))
            {
                getPathResult.Length = 0;
                getPathResult.Append("file:///");
                getPathResult.Append(Application.persistentDataPath);
                getPathResult.Append("/Data/");
                getPathResult.Append(path);
                tmpPath = getPathResult.ToString();
                return tmpPath;
            }
            getPathResult.Length = 0;
            getPathResult.Append("jar:file://");
            getPathResult.Append(Application.dataPath);
            getPathResult.Append("!/assets/Data/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
#endif
        return tmpPath;


    }

    public static string GetConfigPath(string path)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        getPathResult.Length = 0;
        getPathResult.Append("file:///");
        getPathResult.Append(Application.dataPath);
        getPathResult.Append("/StreamingAssets/Config/");
        getPathResult.Append(path);
        tmpPath = getPathResult.ToString();

#elif UNITY_IPHONE
            getPathResult.Length = 0;
            getPathResult.Append(Application.persistentDataPath);
            getPathResult.Append("/Config/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
            if (!Root.IsApkNewestVersion() && File.Exists(tmpPath))
            {
                getPathResult.Length = 0;
                getPathResult.Append("file:///");
                getPathResult.Append(Application.persistentDataPath);
                getPathResult.Append("/Config/");
                getPathResult.Append(path);
                tmpPath = getPathResult.ToString();
                return tmpPath;
            }
            getPathResult.Length = 0;
            getPathResult.Append("file:///");
            getPathResult.Append(Application.dataPath);
            getPathResult.Append("/Raw/Config/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
#elif UNITY_ANDROID
            getPathResult.Length = 0;
            getPathResult.Append(Application.persistentDataPath);
            getPathResult.Append("/Config/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
            if (!Root.IsApkNewestVersion() && File.Exists(tmpPath))
            {
                getPathResult.Length = 0;
                getPathResult.Append("file:///");
                getPathResult.Append(Application.persistentDataPath);
                getPathResult.Append("/Config/");
                getPathResult.Append(path);
                tmpPath = getPathResult.ToString();
                return tmpPath;
            }
            getPathResult.Length = 0;
            getPathResult.Append("jar:file://");
            getPathResult.Append(Application.dataPath);
            getPathResult.Append("!/assets/Config/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
#endif
        return tmpPath;


    }

    public static string GetDataRootPath(string path)
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        getPathResult.Length = 0;
        getPathResult.Append("file:///");
        getPathResult.Append(Application.dataPath);
        getPathResult.Append("/StreamingAssets/");
        getPathResult.Append(path);
        tmpPath = getPathResult.ToString();
#elif UNITY_IPHONE
            getPathResult.Length = 0;
            getPathResult.Append(Application.persistentDataPath);
            getPathResult.Append("/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
            if (!Root.IsApkNewestVersion() && File.Exists(tmpPath))
            {
                getPathResult.Length = 0;
                getPathResult.Append("file:///");
                getPathResult.Append(Application.persistentDataPath);
                getPathResult.Append("/");
                getPathResult.Append(path);
                tmpPath = getPathResult.ToString();
                return tmpPath;
            }
            getPathResult.Length = 0;
            getPathResult.Append("file:///");
            getPathResult.Append(Application.dataPath);
            getPathResult.Append("/Raw/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
#elif UNITY_ANDROID
            getPathResult.Length = 0;
            getPathResult.Append(Application.persistentDataPath);
            getPathResult.Append("/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
            if (!Root.IsApkNewestVersion() && File.Exists(tmpPath))
            {
                getPathResult.Length = 0;
                getPathResult.Append("file:///");
                getPathResult.Append(Application.persistentDataPath);
                getPathResult.Append("/");
                getPathResult.Append(path);
                tmpPath = getPathResult.ToString();
                return tmpPath;
            }
            getPathResult.Length = 0;
            getPathResult.Append("jar:file://");
            getPathResult.Append(Application.dataPath);
            getPathResult.Append("!/assets/");
            getPathResult.Append(path);
            tmpPath = getPathResult.ToString();
#endif
        return tmpPath;
    }

    public static string GetStreamingAssetPath()
    {
        string dataPath = "";
#if UNITY_EDITOR
        dataPath = Application.dataPath + "/StreamingAssets";
#elif UNITY_STANDALONE_WIN
     dataPath = Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
     dataPath = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IPHONE
    dataPath = Application.dataPath + "/Raw";
#else

#endif
        return dataPath;
    }

    public static Vector3 String2Vector3(string left)
    {
        string[] files = left.Split(new char[] { ',' });
        if (files.Length > 2)
        {
            float x = float.Parse(files[0]);
            float y = float.Parse(files[1]);
            float z = float.Parse(files[2]);
            return new Vector3(x, y, z);
        }
        return Vector3.zero;
    }

    public static void Log(string log, LogType type = LogType.Normal, bool showStrace = false)
    {
#if UNITY_ANDROID
     if (!Util.writeLog)
        return;
#elif UNITY_IPHONE
     if (!Util.writeLog)
        return;
#endif

        log = "【" + logIndex + "】" + log;
        logIndex++;

        if (With(logFuncType, (int)type) && With(fileLogLevel, (int)Util.FileLogLevel.Normal))
        {
            log = MakeLog(log, showStrace);
            OutLog.LogScreen(log);
            Debug.Log(log);
        }
    }

    public static void LogError(string log, bool showStrace = false)
    {
        log = MakeLog(log, showStrace);
        OutLog.LogScreen(log);
        Debug.Log(log);
    }

    public static void LogWarning(string log, bool showStrace = false)
    {
        if (With(Util.fileLogLevel, (int)FileLogLevel.Warning))
        {
            log = MakeLog(log, showStrace);
            OutLog.LogScreen(log);
            Debug.LogWarning(log);
        }
    }

    public static string MakeLog(string log, bool showStrace)
    {
        string str = Thread.CurrentThread.ManagedThreadId.ToString();
        string str2 = DateTime.Now.ToString("MM-dd HH:mm:ss fff");
        log = log + "\n\t[" + str2 + "]{thread:" + str + "}";
        if (showStrace)
        {
            string str3 = new StackTrace(true).ToString();
            log = log + "\n\t(** " + str3 + " ** )\n";
            return log;
        }
        log = log + "\n";
        return log;
    }


    public static void SetLayer(Transform xform, int layer)
    {
        xform.gameObject.layer = layer;
        foreach (Transform transform in xform)
        {
            SetLayer(transform, layer);
        }
    }


    public static bool With(int set, int val)
    {
        return ((set & val) > 0);
    }


    public static float InvokeGap
    {
        get
        {
            return invokeGap;
        }
    }


    public enum LogType
    {
        Normal = 1,
        UI = 2,
        Fight = 4,
    }

    public enum FileLogLevel
    {
        Normal = 1,
        Warning = 2,
        Error = 4,
    }

    public enum SendLogLevel
    {
        Normal = 1,
        Warning = 2,
        Error = 4,
    }

}
   


