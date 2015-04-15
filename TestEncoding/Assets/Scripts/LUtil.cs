using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

public class LUtil
{
#if UNITY_IPHONE || UNITY_ANDROID
    public static bool writeLog = true;
#else
    public static bool writeLog = false;
#endif

    private static float invokeGap = 0.03f;
    private static ulong logIndex = 1L;
    private static int logType = 1;
    private static int LogTypeCount = 4;
    public static bool showDebugInfo = false;

    public enum LogType
    {
        Anim = 0x10,
        CutBoss = 4,
        Fight = 2,
        Guide = 0x40,
        Normal = 1,
        Rank = 0x20,
        Talk = 8
    }

    public static void Log(string log, LogType type = LogType.Normal, bool showStrace = false)
    {
        if (writeLog)
        {
            object[] objArray1 = new object[] { "【", logIndex, "】", log };
            log = string.Concat(objArray1);
            logIndex += (ulong)1L;
            if (With(logType, (int)type))
            {
                log = MakeLog(log, showStrace);
                object[] objs = new object[] { log };
                OutLog.Log(objs);
            }
        }
    }

    public static void LogError(string log, bool showStrace = false)
    {
        log = MakeLog(log, showStrace);
        UnityEngine.Debug.LogError(log);
    }

    public static string MakeLog(string log, bool showStrace)
    {
        string str = Thread.CurrentThread.ManagedThreadId.ToString();
        string str2 = DateTime.Now.ToString("MM-dd HH:mm:ss fff");
        string[] textArray1 = new string[] { log, "\n\t[", str2, "]{thread:", str, "}" };
        log = string.Concat(textArray1);
        if (showStrace)
        {
            string str3 = new StackTrace(true).ToString();
            log = log + "\n\t(** " + str3 + " ** )\n";
            return log;
        }
        log = log + "\n";
        return log;
    }

    public static bool With(int set, int val)
    {
        return ((set & val) > 0);
    }

    public static bool isset_state(char[] state, int teststate)
    {
        return (0 != (state[teststate / 8] & (0xff & (((int)1) << (teststate % 8)))));
    }

    public static void set_state(ref char[] state, int teststate)
    {
        if (teststate >= 6)
        {
            UnityEngine.Debug.LogError("超过最大状态数: " + teststate);
        }
        else
        {
            state[teststate / 8] = (char)(state[teststate / 8] | ((char)(0xff & (((int)1) << (teststate % 8)))));
        }
    }

    public static void clear_state(ref char[] state, int teststate)
    {
        if (teststate >= 6)
        {
            UnityEngine.Debug.LogError("超过最大状态数: " + teststate);
        }
        else
        {
            state[teststate / 8] = (char)(state[teststate / 8] & ((char)(0xff & ~(((int)1) << (teststate % 8)))));
        }
    }

    public static float InvokeGap
    {
        get
        {
            return invokeGap;
        }
    }

    public static string GetTablePath()
    {
        string dataPath = "";
        
#if UNITY_EDITOR || UNITY_STANDALONE
        dataPath = Application.dataPath + "/StreamingAssets/Data/";
#elif UNITY_ANDROID
        dataPath = "jar:file://" + Application.dataPath + "!/assets/Data/";
#else
        dataPath = Application.dataPath + "/Raw/Data/";
#endif

        return dataPath;
    }

    public static string GetDataRootPath()
    {
        string dataPath = "";

#if UNITY_EDITOR || UNITY_STANDALONE
        dataPath = Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
        dataPath = "jar:file://" + Application.dataPath + "!/assets";
#else
        dataPath = Application.dataPath + "/Raw";
#endif

        return dataPath;
    }
}

