using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GBKEncoding;
using UnityEngine;
using System.IO;
using System.Text;
using Mono.Xml;
using System.Security;

public class Root : MonoBehaviour
{
    public string filePath;
    public string result = "";

    public UI_Chat uiChat;

    void Awake()
    {
        //LSingleton<LoginModule>.Instance.LoginNetWork.ParseSrvlist("");
        //ParseSrvlist("");
        InitGame();

        LUtil.Log("Load Srvlist");
        //StartCoroutine(LoadSrvlist());
        StartCoroutine(GetSrvList());
        //Initialize();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        LUtil.Log("@@@@@@不锁屏？？");
    }

    void OnGUI()
    {
        //if (GUI.Button(new Rect(100, 220, 100, 40), "srvlist"))
        //{
        //    //ParseSrvlist("");
        //    BeginParseSrvlist();
        //}
    }

    public void Initialize()
    {
        uiChat = new UI_Chat();
        uiChat.Init();
    }

    public void InitGame()
    {
        LUtil.Log("InitGame Start()", LUtil.LogType.Normal, false);
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        try
        {
            LSingleton<GameManager>.Instance.Initialize();
        }
        catch (Exception exception)
        {
            LUtil.Log("Initializes() error message : " + exception.Message, LUtil.LogType.Normal, false);
        }
        Application.targetFrameRate = 30;
        UnityEngine.Debug.Log("start------------");
        
        //NGUI_MainManager.Instance.Init(GameObject.Find("NGUI_Root"));
        //UnityEngine.Object.DontDestroyOnLoad(NGUI_MainManager.Instance.m_rootObj);
        //NGUIManager.Instance.SetNguiRoot(NGUI_MainManager.Instance.m_rootObj);
        LUtil.Log("InitGame End()", LUtil.LogType.Normal, false);
    }
    IEnumerator GetSrvList()
    {
        WWW www;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        www = new WWW("http://192.168.123.220/autopatchztwin/srvlist.xml");
        yield return www;
#elif UNITY_ANDROID
        www = new WWW("http://192.168.123.220/autopatchztios/srvlist.xml");
        yield return www;
#elif UNITY_IPHONE
        www = new WWW("http://192.168.123.220/autopatchztandroid/srvlist.xml");
        yield return www;
#else

#endif

        //InitGame();

        LoginModule.Instance.LoginNetWork.ParseSrvlist(www.text);
    }

    IEnumerator LoadSrvlist()
    {
        string xmlfile = GamePath.Config + "srvlistout.xml";

        if (xmlfile.Contains("://"))
        {
            OutLog.Log("@@path: " + xmlfile);
            WWW www = new WWW(xmlfile);
            yield return www;
            result = www.text;
            //ParseSrvlist(result);
            LoginModule.Instance.LoginNetWork.ParseSrvlist(www.text);
        }
        else
        {
            //result = System.IO.File.ReadAllText(filePath);
            //ParseSrvlist(result);
            LoginModule.Instance.LoginNetWork.ParseSrvlist(System.IO.File.ReadAllText(xmlfile));
        }
    }

    public void BeginParseSrvlist()
    {
        filePath = GamePath.Config + "srvlistout.xml";

        StartCoroutine(this.LoadSrvlist());
    }

    public void ParseSrvlist(string str)
    {
        if (str != null)
        {

            byte[] bytes = Encoding.UTF8.GetBytes(str);
            OutLog.Log("srvlistout length: " + bytes.Length);
            MemoryStream stream = new MemoryStream(bytes);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            SecurityParser parser = new SecurityParser();
            parser.LoadXml(reader.ReadToEnd());

            SecurityElement se = parser.ToXml();
            foreach (SecurityElement child in se.Children)
            {
                if (child.Tag == "zones")
                {
                    foreach (SecurityElement childchild in child.Children)
                    {
                        if (childchild.Tag == "zone")
                        {
                            ServerInfo item = new ServerInfo
                            {
                                name = childchild.Attribute("name"),
                                id = int.Parse(childchild.Attribute("id")),
                                ip = childchild.Attribute("addr"),
                                port = childchild.Attribute("port")
                            };
                            LUtil.Log(string.Concat(new object[]{"name: ", item.name, " zone:", item.id, " ip:", item.ip, " port:", item.port}));
                            //this.SrvLists.Add(item);
                        }

                    }
                }

            }
        }
    }

    void Start()
    {
       
    }

    void Update()
    {
        
    }
   
}

