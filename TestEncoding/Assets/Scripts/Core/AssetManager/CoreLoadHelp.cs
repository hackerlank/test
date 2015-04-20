using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Reflection;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public class CoreLoadHelp
{
    public static string AppVersion = "";
    public static string OpenGLVersion = "";
    public static string NotificationID = "";
    public static string DownloadPage = "";
    public static string[] InitString = null;

    public static Action<float> ChangeProcess;
    public static Texture2D texLoginBack;
    public static GUIStyle styPlanBack;
    public static Texture2D texLogo;
    public static GUIStyle styCurPlan;
    public static GUIStyle styCurPlanPoint;

    public static GUIStyle styAskBack;
    public static GUIStyle btnAskCancel;
    public static GUIStyle btnAskSure;
    public static GUIStyle btnAskClose;
    public static GUIStyle styAskLoding;
    public static Action<byte> ExitHandle;
    public static LoadItem LoadBytes(MonoBehaviour mo, string path, Action<byte[]> callback)
    {
        LoadItem item = new LoadItem(path);
        AddLog("LoadBytes:" + path);
        mo.StartCoroutine(loadBytes(item, callback));
        return item;
    }
    public static bool Ready = false;
    private static IEnumerator loadBytes(LoadItem item, Action<byte[]> callback)
    {
        yield return 0;
        item.Req = new WWW(item.Path);
        float lastProcess = 0, duration = 0, lastTime = Time.realtimeSinceStartup;
        while (!item.Req.isDone)
        {
            yield return new WaitForSeconds(0.01f);
            if (lastProcess == item.Req.progress)
            {
                duration += Time.realtimeSinceStartup - lastTime;
            }
            else
            {
                duration = 0;
                lastProcess = item.Req.progress;
            }
            if (duration > 3)
            {
                break;
            }
            lastTime = Time.realtimeSinceStartup;
        }
        yield return new WaitForEndOfFrame();
        if (duration < 3 && string.IsNullOrEmpty(item.Req.error))
        {
            callback(item.Req.bytes);
        }
        else
        {
            callback(null);
            AddLog("LoadBytes fail:" + item.Path + ", error:" + item.Req.error);
        }
        item.Req.Dispose();
    }

    public static void LoadText(MonoBehaviour mo, string path, Action<string> callback)
    {
        mo.StartCoroutine(loadText(path, callback));
    }
    private static IEnumerator loadText(string path, Action<string> callback)
    {
        yield return 0;
        WWW request = new WWW(path);
        float lastProcess = 0, duration = 0, lastTime = Time.realtimeSinceStartup;
        while (!request.isDone)
        {
            yield return new WaitForSeconds(0.2f);
            if (lastProcess == request.progress)
            {
                duration += Time.realtimeSinceStartup - lastTime;
            }
            else
            {
                duration = 0;
                lastProcess = request.progress;
            }
            if (duration > 3)
            {
                break;
            }
            lastTime = Time.realtimeSinceStartup;
        }
        yield return new WaitForEndOfFrame();
        if (duration < 3 && string.IsNullOrEmpty(request.error))
        {
            callback(request.text);
        }
        else
        {
            callback(string.Empty);
            AddLog(path + ":" + request.error);
        }
        request.Dispose();
    }
    public static void AddLog(string log)
    {
        logLineCount++;
        if (logLineCount > 30)
        {
            logLineCount = 0;
            logSb.Length = 0;
        }
        logSb.AppendLine(log);
        Debug.Log(log);
        LogStr = logSb.ToString();
    }
    private static byte logLineCount = 0;
    private static StringBuilder logSb = new StringBuilder();
	public static string StoragePath = "";
	public static string FileVersionConfirmPath = "";
	public static string FileVersionAssetPath = "";
	public static string FileVersionAssetPath1 = "";
	public static string NoticePage = "";
	public static string ServerListPath = ""; 
    public static string LogStr = "";
    private static MonoBehaviour mono;
    public static void Init(MonoBehaviour _mono)
    {
        mono = _mono;
    }
    public static void LoadApp(string path, Action<IMonoHandle> gamehandle)
    {
        LoadBytes(mono, path, delegate(byte[] data)
        {
            buffer = new byte[1024 * 1024 * 5];
            data = deCompress(data);
            int index = 0;
            readDll(data, ref index);
            readDll(data, ref index);
            readDll(data, ref index);
            gamehandle(readDll(data, ref index));
            buffer = null;
        });
    }

    private static byte[] buffer = null;
    private static int length = 0;
    private static byte[] compress(byte[] data)
    {
        byte[] result = null;
        using (MemoryStream ms = new MemoryStream(buffer))
        {
            using (DeflaterOutputStream zipStream = new DeflaterOutputStream(ms))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Finish();
                length = (int)ms.Position;
                result = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = buffer[i];
                }
            }
        }
        return result;
    }
    private static byte[] deCompress(byte[] data)
    {
        byte[] result = null;
        using (MemoryStream ms = new MemoryStream(data, 0, data.Length))
        {
            ms.Seek(0, SeekOrigin.Begin);
            using (InflaterInputStream zipStream = new InflaterInputStream(ms))
            {
                length = zipStream.Read(buffer, 0, buffer.Length);
            }
        }
        result = new byte[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = buffer[i];
        }
        return result;
    }
    private static IMonoHandle readDll(byte[] buffer, ref int index)
    {
        IMonoHandle app = null;
        byte[] data = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            data[i] = buffer[index];
            index++;
        }
        uint length = ReadUInt32(data);
        data = new byte[length];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = buffer[index];
            index++;
        }
        Assembly assembly = Assembly.Load(data);
        if (assembly.FullName.IndexOf("GameCore") > -1)
        {
            app = assembly.CreateInstance("MonoHandle") as IMonoHandle;
        }
        return app;
    }
    public static uint ReadUInt32(byte[] data)
    {
        uint result = (uint)(data[0] + (data[1] << 8) + (data[2] << 16) + (data[3] << 24));
        return result;
    }
}
public class LoadItem
{
    public LoadItem(string path)
    {
        Path = path;
    }
    public string Path;
    public WWW Req;
    public float Process
    {
        get
        {
            if (Req == null) return 0;
            else return Req.progress;
        }
    }
}
