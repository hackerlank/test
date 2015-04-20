using System.Net;
using Common;
using NGUI;
using UnityEngine;
using System.Collections;
using Net;
using System.IO;
using ProtoBuf;
using System;

public class Root : MonoBehaviour 
{
    //能不能进行登陆
    public static bool canClickLogin = true;

    //检测完才走小版本更新流程
    public static bool newVersionCheckOK = false;

    public static bool newBigVersion = false;

    public static bool isApkNewestVersion = true;
    public static bool isApkNewestVersionInited = false;
    //APK(ipa)包里的版本
    public static bool localApkVersionInited = false;
    //更新目录的版本
    public static bool localPersistentVersionInited = false;

    public static bool serverVersionInited = false;

    public static string localApkVersion = "0.0.0.0";
    public static string localPersistentVersion = "0.0.0.0";
    public static string serverVersion = "0.0.0.0";

    public static string apkAddress ="";
    public static string patchAddress = "";
    //private static AssetLoader assetLoader;

    public static void CheckNewVersion()
    {
        Util.Log("检测新版本");
         string[] serverVersion = Root.serverVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        string[] currentVersion = null;
        if (IsApkNewestVersion())
            currentVersion = Root.localApkVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        else
            currentVersion = Root.localPersistentVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

         if (string.Compare(serverVersion[0], currentVersion[0]) > 0
                || (string.Compare(serverVersion[0], currentVersion[0]) == 0 && string.Compare(serverVersion[1], currentVersion[1]) > 0)
                || (string.Compare(serverVersion[0], currentVersion[0]) == 0 && string.Compare(serverVersion[1], currentVersion[1]) == 0 && string.Compare(serverVersion[2], currentVersion[2]) > 0))
         {
             Util.Log("发现新版本");
             NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
             {
                 script.Init();
                 script.InitDesc("新版本出炉了，点击确定进行下载！", enumMsgType.MsgType_NewVersion);
             });
         }
         //小版本更新
         else if (string.Compare(serverVersion[0], currentVersion[0]) == 0 && string.Compare(serverVersion[1], currentVersion[1]) == 0 && string.Compare(serverVersion[2], currentVersion[2]) == 0 && string.Compare(serverVersion[3], currentVersion[3]) > 0)
         {
             //等更新完
             canClickLogin = false;
             newVersionCheckOK = true;
         }
    }

    public static bool IsApkNewestVersion()
    {
        if (!Root.isApkNewestVersionInited)
        {
            string[] apkversion = Root.localApkVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string[] persistentversion = Root.localPersistentVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (string.Compare(apkversion[0], persistentversion[0]) > 0
                || (string.Compare(apkversion[0], persistentversion[0]) == 0 && string.Compare(apkversion[1], persistentversion[1]) > 0)
                || (string.Compare(apkversion[0], persistentversion[0]) == 0 && string.Compare(apkversion[1], persistentversion[1]) == 0 && string.Compare(apkversion[2], persistentversion[2]) > 0)
                || (string.Compare(apkversion[0], persistentversion[0]) == 0 && string.Compare(apkversion[1], persistentversion[1]) == 0 && string.Compare(apkversion[2], persistentversion[2]) == 0 && string.Compare(apkversion[3], persistentversion[3]) >= 0))
            {
                isApkNewestVersion = true;
                isApkNewestVersionInited = true;
            }
            else
            {
                isApkNewestVersion = false;
                isApkNewestVersionInited = true;
            }
        }

        return isApkNewestVersion;
    }

    public static string GetLastVersion()
    {
        if (IsApkNewestVersion())
            return localApkVersion;
        else
            return localPersistentVersion;
    }
    void Awake()
    {
        //IPAddress ipAddress0 = IPAddress.Parse("﻿192.168.172.55");
        //Util.Log("StartCoroutine(GetSrvlist());");
        //StartCoroutine(GetSrvlist());
        //StartCoroutine(GetFlServerEndPoint());

        InitGame();

        Util.Log("ParseSrvlist");
        //LoginModule.Instance.LoginNetWork.ParseSrvlist("");
        if (File.Exists(Application.persistentDataPath + "/Config/srvlist.xml"))
        {
            Util.Log("Persintent存在 srvlist.xml");
            StartCoroutine(GetLocalPersistentVersion());
        }
        else
        {
            localPersistentVersionInited = true;
        }

        StartCoroutine(GetLocalApkVersion());
        StartCoroutine(GetSrvlist());

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

	void InitGame () 
    {
        Util.Log("InitGame Start()");

        DontDestroyOnLoad(gameObject);


        try
        {
            GameManager.Instance.Initialize();
        }
        catch (Exception ex)
        {

            Util.Log("Initializes() error message : " + ex.Message);
        }

        
        Application.targetFrameRate = 30;
        Debug.Log("start------------");
        PlayerHandle.ErrorHandle = UnityEngine.Debug.LogError;
        PlayerHandle.DebugHandle = UnityEngine.Debug.Log;
        PlayerHandle.WarningHandle = UnityEngine.Debug.LogWarning;
        //PlayerHandle.SetLogFilter(new string[] { "MonoHandle.cs", "PlatformAPI.cs" });
        PlayerHandle.ErrorHandle += delegate( object msg )
        {
            //if (sendError != null)
            //    sendError(msg.ToString());
        };
        LoadHelp.Init(MonoThread.Instance);
        NGUI_MainManager.Instance.Init(GameObject.Find(NGUI_UI.NGUI_Root));
        DontDestroyOnLoad(NGUI_MainManager.Instance.m_rootObj);
        NGUIManager.Instance.SetNguiRoot(NGUI_MainManager.Instance.m_rootObj);

        GameAudioManager.Instance.playGameBgAudio("ztsound");
        Util.Log("InitGame End()");
    }

    IEnumerator GetFlServerEndPoint()
    {
        WWW www = new WWW("http://172.17.2.80");

        yield return www;
        InitGame();

        LoginModule.Instance.LoginNetWork.ParseFLEndpoint(www.text);
    }

    IEnumerator GetSrvlist()
    {
        WWW www;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        //www = new WWW("http://172.17.2.93//autopatchztwin/srvlist.xml");
        www = new WWW("http://101.226.182.184/autopatchztandroid/srvlist.xml");
#elif UNITY_ANDROID
        www = new WWW("http://101.226.182.184/autopatchztandroid/srvlist.xml");
#elif UNITY_IPHONE
        www = new WWW("http://101.226.182.184/autopatchztios/srvlist.xml");
#else
        
#endif

        yield return www;
        //InitGame();

        Util.Log("ParseSrvlist");
        LoginModule.Instance.LoginNetWork.ParseSrvlist(www.text);
    }

    IEnumerator GetLocalPersistentVersion()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        string xmlfile = Application.persistentDataPath + "/Config/srvlist.xml";
#elif UNITY_IPHONE
        string xmlfile = Application.persistentDataPath + "/Config/srvlist.xml";
#elif UNITY_ANDROID
        string xmlfile = "file:///" + Application.persistentDataPath + "/Config/srvlist.xml";
#endif

        if (xmlfile.Contains("://"))
        {
            WWW www = new WWW(xmlfile);
            yield return www;
            LoginModule.Instance.LoginNetWork.ParseLocalPersistentVersion(www.text);
        }
        else
        {
            LoginModule.Instance.LoginNetWork.ParseLocalPersistentVersion(System.IO.File.ReadAllText(xmlfile));
        }

    }

    IEnumerator GetLocalApkVersion()
    {
        string xmlfile = Util.GetStreamingAssetPath() + "/Config/srvlist.xml";

        if (xmlfile.Contains("://"))
        {
            WWW www = new WWW(xmlfile);
            yield return www;
            LoginModule.Instance.LoginNetWork.ParseLocalApkVersion(www.text);
        }
        else
        {
            LoginModule.Instance.LoginNetWork.ParseLocalApkVersion(System.IO.File.ReadAllText(xmlfile));
        }

    }

//    IEnumerator LoadlocalSrvlist()
//    {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//        string xmlfile = GamePath.Config + "srvlistin.xml";
//#else
//        string xmlfile = GamePath.Config + "srvlistout.xml";
//#endif
        
//        if (xmlfile.Contains("://"))
//        {
//            WWW www = new WWW(xmlfile);
//            yield return www;
//            LoginModule.Instance.LoginNetWork.ParseSrvlist(www.text);
//        }
//        else
//        {
//            LoginModule.Instance.LoginNetWork.ParseSrvlist(System.IO.File.ReadAllText(xmlfile));
//        }

//    }
    

    void Update()
    {
        Scheduler.Instance.Update();
        LoadHelp.Update();
    }

    void Start ()
    {
        //assetLoader = base.GetComponent<AssetLoader>();
        LoadedInitAssets();
    }

    //public static AssetLoader AssetLoader
    //{
    //    get
    //    {
    //        return assetLoader;
    //    }
    //}


    public void LoadedInitAssets()
    {
        try
        {
            //相关表格初始化
            //t_effect_config.Init(configInitHandle);
            //t_skill_config.Init(configInitHandle);
            //t_skill_lv_config.Init(configInitHandle);
            //t_skill_stage_config.Init(configInitHandle);
            //t_state_config.Init(configInitHandle);
            //t_npc_config.Init(configInitHandle);
            //t_object_config.Init(configInitHandle);
        }
        catch (Exception exception)
        {
            Util.LogError(exception.ToString(), false);
        }

        //AssetLoader.Init();
    }

    private void configInitHandle()
    {

    }
}
