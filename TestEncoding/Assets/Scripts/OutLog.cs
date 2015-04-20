using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NGUI;
using Net;
using System;
using GBKEncoding;

public class OutLog : MonoBehaviour
{
    static List<string> mLines = new List<string>();
    static List<string> mWriteTxt = new List<string>();
    private string outpath;
    public const int MAX_SEND_BYTE = 1024;
    void Start()
    {
        //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
        outpath = Application.persistentDataPath + "/ztapp.txt";
        //每次启动客户端删除之前保存的Log
        if (System.IO.File.Exists(outpath))
        {
            File.Delete(outpath);
        }
        //在这里做一个Log的监听
        Application.RegisterLogCallback(HandleLog);
    }

    void Update()
    {
        ///停掉再开启会保存之前在内存的日志，为了调试方便，先这样写
        if (!Util.writeLog)
            return;

        //因为写入文件的操作必须在主线程中完成，所以在Update中哦给你写入文件。
        if (mWriteTxt.Count > 0)
        {
            string[] temp = mWriteTxt.ToArray();
            foreach (string t in temp)
            {
                using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                {
                    writer.WriteLine(t);
                }
                mWriteTxt.Remove(t);
            }
        }
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if ((type == LogType.Error) || (type == LogType.Exception))
        {
            if (Util.With(Util.fileLogLevel , (int)Util.FileLogLevel.Error))
            {
                mWriteTxt.Add(logString);
                mWriteTxt.Add(stackTrace);

                //if (Util.With(Util.sendLogLevel, (int)Util.SendLogLevel.Error))
                //{
                //    SendErrorMsg(logString);
                //    SendErrorMsg(stackTrace);
                //}
            }
        }
        else if (type == LogType.Warning)
        {
            if (Util.With(Util.fileLogLevel, (int)Util.FileLogLevel.Warning))
            {
                mWriteTxt.Add(logString);
                mWriteTxt.Add(stackTrace);

                if (Util.With(Util.sendLogLevel, (int)Util.SendLogLevel.Warning))
                {
                    //if (Util.canSendWarningDebugLog)
                    //{
                    //    SendErrorMsg(logString);
                    //    SendErrorMsg(stackTrace);
                    //}
                }
            }
        }
        else if (type == LogType.Log)
        {
            if (Util.With(Util.fileLogLevel, (int)Util.FileLogLevel.Normal))
            {
                mWriteTxt.Add(logString);
                mWriteTxt.Add(stackTrace);
                if (Util.With(Util.sendLogLevel, (int)Util.SendLogLevel.Normal))
                {
                    //if (Util.canSendWarningDebugLog)
                    //{
                    //    SendErrorMsg(logString);
                    //    SendErrorMsg(stackTrace);
                    //}
                }
            }
        }
    }

    public void SendErrorMsg(string info)
    {
        UMessage message = new UMessage();

        info = info.Replace("\n", ";");

        stSendErrorInfo cmd = new stSendErrorInfo();
        byte[] tmp = GBKEncoder.ToBytes(info);
        int strLen = tmp.Length+1;
        if (strLen > MAX_SEND_BYTE)
            strLen = MAX_SEND_BYTE;

        cmd.size = (UInt16)strLen;
        if (message.WriteStruct<stSendErrorInfo>(cmd))
        {
            message.WriteString(info, strLen);
            NetWorkModule.Instance.SendImmediate(message);
        }

    }

    //这里把错误的信息保存起来，用来输出在手机屏幕上
    static public void Log(params object[] objs)
    {
        string text = "";
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }

        if (Application.isPlaying)
        {
            if (mLines.Count > 10)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);
        }


    }

    static public void LogScreen(params object[] objs)
    {
        string text = "";
        for (int i = 0; i < objs.Length; ++i)
        {
            if (i == 0)
            {
                text += objs[i].ToString();
            }
            else
            {
                text += ", " + objs[i].ToString();
            }
        }
        //mWriteTxt.Add(text);

        if (Application.isPlaying)
        {
            if (mLines.Count > 10)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);
        }


    }

    void OnGUI()
    {
#if UNITY_IPHONE
        int bwidht = 100;
        int bheight = 50;
#elif UNITY_ANDROID
        int bwidht = 100;
        int bheight = 50;
#else
        int bwidht = 100;
        int bheight = 50;
#endif
        //日志开关
        if (Util.showDebugInfo)
        {
            if (GUI.Button(new Rect(Screen.width - bwidht, 70, bwidht, bheight), "退出游戏"))
            {
                Application.Quit();
            }

            //调试信息总开关
            if (GUI.Button(new Rect(Screen.width - bwidht, 70 + bheight, bwidht, bheight), "关闭调试"))
            {
                Util.showDebugInfo = !Util.showDebugInfo;
            }

            if (GUI.Button(new Rect(Screen.width - bwidht, 70 + bheight * 2 + 20, bwidht, bheight), "日志开关"))
            {
                Util.writeLog = !Util.writeLog;
                if (Util.writeLog)
                {
                    Util.Log("日志路径: " + Application.persistentDataPath + "/ztapp.txt");
                }
            }
        }

        if (!Util.writeLog || !Util.showDebugInfo)
            return;

        GUI.color = Color.red;
        for (int i = 0, imax = mLines.Count; i < imax; ++i)
        {
            if (i % 2 == 0)
                GUI.color = Color.red;
            else
                GUI.color = Color.green;

            GUILayout.Label(mLines[i]);
        }
    }
}