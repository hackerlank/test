using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using Net;

public class OutLog : MonoBehaviour
{
    private static List<string> mLines = new List<string>();
    private static List<string> mWriteTxt = new List<string>();
    private string outpath;

    private void HandleLog(string logString, string stackTrace, UnityEngine.LogType type)
    {
        if (LUtil.writeLog)
        {
            mWriteTxt.Add(logString);
            mWriteTxt.Add(stackTrace);
        }
    }

    public static void Log(params object[] objs)
    {
        string item = string.Empty;
        for (int i = 0; i < objs.Length; i++)
        {
            if (i == 0)
            {
                item = item + objs[i].ToString();
            }
            else
            {
                item = item + ", " + objs[i].ToString();
            }
        }
        if (Application.isPlaying)
        {
            if (mLines.Count > 10)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(item);
        }
    }

    public static void MyLog(params object[] objs)
    {
        string item = string.Empty;
        for (int i = 0; i < objs.Length; i++)
        {
            if (i == 0)
            {
                item = item + objs[i].ToString();
            }
            else
            {
                item = item + ", " + objs[i].ToString();
            }
        }
        mWriteTxt.Add(item);
        if (Application.isPlaying)
        {
            if (mLines.Count > 10)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(item);
        }
    }

    private void OnGUI()
    {
        int num = 100;
        int num2 = 50;
        if (GUI.Button(new Rect((float) (Screen.width - num), 70f, (float) num, (float) num2), "退出游戏"))
        {
            Application.Quit();
        }
        if (GUI.Button(new Rect((float) (Screen.width - num), (float) (70 + num2), (float) num, (float) num2), "调试信息"))
        {
            LUtil.showDebugInfo = !LUtil.showDebugInfo;
        }
        if (LUtil.showDebugInfo && GUI.Button(new Rect((float) (Screen.width - num), (float) ((70 + (num2 * 2)) + 20), (float) num, (float) num2), "日志开关"))
        {
            LUtil.writeLog = !LUtil.writeLog;
            if (LUtil.writeLog)
            {
                LUtil.Log("日志路径: " + Application.persistentDataPath + "/ztapp.txt", LUtil.LogType.Normal, false);
            }
        }
        if (LUtil.writeLog)
        {
            GUI.color = Color.red;
            int num3 = 0;
            int count = mLines.Count;
            while (num3 < count)
            {
                if ((num3 % 2) == 0)
                {
                    GUI.color = Color.red;
                }
                else
                {
                    GUI.color = Color.green;
                }
                GUILayout.Label(mLines[num3], new GUILayoutOption[0]);
                num3++;
            }
        }
    }

    private void Start()
    {
        this.outpath = Application.persistentDataPath + "/ztapp.txt";
        if (File.Exists(this.outpath))
        {
            File.Delete(this.outpath);
        }
        Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
    }

    private void Update()
    {
        if (LUtil.writeLog && (mWriteTxt.Count > 0))
        {
            foreach (string str in mWriteTxt.ToArray())
            {
                using (StreamWriter writer = new StreamWriter(this.outpath, true, Encoding.UTF8))
                {
                    writer.WriteLine(str);
                }
                mWriteTxt.Remove(str);
            }
        }
    }
}

