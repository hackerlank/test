using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;


public class Host : MonoBehaviour
{
    //private Rect backgroundRect = new Rect(0, 0, Screen.width, Screen.height);
    //private Rect retryRect = new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2);
    public PlatformType Platform = PlatformType.WINDOWS_STANDALONE;
    void HostAwake()
    {
        StringBuilder getPathResult = new StringBuilder();//apk外部的版本
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Application.targetFrameRate = 30;
        Platform = PlatformType.IOS_APPSTORE;
        getPathResult.Length = 0;
        getPathResult.Append(Application.persistentDataPath);
        getPathResult.Append("/");
        CoreLoadHelp.ServerListPath = Root.patchAddress;
        CoreLoadHelp.StoragePath = getPathResult.ToString();
#elif UNITY_IPHONE
        Application.targetFrameRate = 30;
        Platform = PlatformType.IOS_APPSTORE;
        getPathResult.Length = 0;
        getPathResult.Append(Application.persistentDataPath);
        getPathResult.Append("/");
		CoreLoadHelp.ServerListPath = Root.patchAddress;
		CoreLoadHelp.StoragePath = getPathResult.ToString();
#elif UNITY_ANDROID
        Application.targetFrameRate = 30;
      //  Platform = PlatformType.ANDROID_91;
        getPathResult.Length = 0;
        getPathResult.Append(Application.persistentDataPath);
        getPathResult.Append("/");
        CoreLoadHelp.ServerListPath = Root.patchAddress;
        CoreLoadHelp.StoragePath = getPathResult.ToString();
#endif

		string menuPath = "Gui/mainFramework/";
        CoreLoadHelp.styPlanBack = createStyle(menuPath, "st0380");
        setStyleBoarder(CoreLoadHelp.styPlanBack, 30, 30, 0, 0);
        CoreLoadHelp.styCurPlan = createStyle(menuPath, "st0381");
        setStyleBoarder(CoreLoadHelp.styCurPlan, 15, 15, 0, 0);
        CoreLoadHelp.styCurPlanPoint = createStyle(menuPath, "st0382");
        CoreLoadHelp.styAskBack = createStyle(menuPath, "st0209");
        CoreLoadHelp.styAskBack.border.left = 42;
        CoreLoadHelp.styAskBack.border.right = 42;
        CoreLoadHelp.styAskBack.border.top = 42;
        CoreLoadHelp.styAskBack.border.bottom = 42;

        CoreLoadHelp.styAskLoding = createStyle(menuPath, "tyk0028");
        CoreLoadHelp.styAskLoding.normal.textColor = new Color(1, 1, 1, 0.5f);


        CoreLoadHelp.btnAskSure = createStyle(menuPath, "btn0039", true);
        CoreLoadHelp.btnAskCancel = createStyle(menuPath, "btn0061", true);
        CoreLoadHelp.btnAskClose = createStyle(menuPath, "ic0124", true);
        font_MCWhite_18 = loadGUIStyle(Color.white, TextAnchor.MiddleCenter, 18, true);
        font_MCGrey_16 = loadGUIStyle(new Color(0.7f, 0.7f, 0.7f), TextAnchor.MiddleCenter, 16, true);
        font_MCGrey_18 = loadGUIStyle(new Color(0.3f, 0.3f, 0.3f), TextAnchor.MiddleCenter, 18, true);
        font_MCYellow_26 = loadGUIStyle(new Color(1.0f, 244.0f / 255.0f, 0.5f), TextAnchor.MiddleCenter, 24, true);
        font_MCRed_26 = loadGUIStyle(new Color(81.0f / 255.0f, 38.0f / 255.0f, 3.0f / 255.0f), TextAnchor.MiddleCenter, 24, true);


        Init();
        init();
        HasInit = true;


    }
    public static IEnumerator YieldWaitForTime(float fTime, Action callBack)
    {
        yield return new WaitForSeconds(fTime);
        callBack();
    }

    Boolean HasInit = false;
    public GUIStyle font_MCBlackMiaobian_30;

    public GUIStyle font_MCBlack_26;
    public GUIStyle font_MCBlackMiaobian_26;
    public GUIStyle font_MCGreenMiaobian_36;
    public GUIStyle font_MCRedMiaobian_36;
    public GUIStyle font_MCWhite_36;

    public GUIStyle font_MCWhite_30_1;
    public GUIStyle font_MCBlack_30;

    private Rect labelRect;

    private Font GBK_Font;

    private void Init()
    {
        font_MCBlackMiaobian_30 = LoadGUIStyle(new Color(59.0f / 255.0f, 47.0f / 255.0f, 47.0f / 255.0f), TextAnchor.MiddleCenter, 30, true);

        font_MCBlack_26 = LoadGUIStyle(Color.black, TextAnchor.MiddleCenter, 26, true);
        font_MCBlackMiaobian_26 = LoadGUIStyle(new Color(186.0f / 255.0f, 163.0f / 255.0f, 128.0f / 255.0f), TextAnchor.MiddleCenter, 26, true);
        font_MCGreenMiaobian_36 = LoadGUIStyle(new Color(68.0f / 255.0f, 92.0f / 255.0f, 0.0f), TextAnchor.MiddleCenter, 36, true);
        font_MCRedMiaobian_36 = LoadGUIStyle(new Color(92.0f / 255.0f, 22.0f / 255.0f, 0.0f), TextAnchor.MiddleCenter, 36, true);
        font_MCWhite_36 = LoadGUIStyle(Color.white, TextAnchor.MiddleCenter, 36, true);

        font_MCWhite_30_1 = LoadGUIStyle(Color.white, TextAnchor.MiddleCenter, 30, true);
        font_MCBlack_30 = LoadGUIStyle(Color.black, TextAnchor.MiddleCenter, 30, true);

        labelRect = new Rect();

        GBK_Font = Resources.Load("DFYuanW7-GBK") as Font;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public void GUILabelBorder(Rect rect, float offset, string value, GUIStyle back, GUIStyle front, int count)
    {
        labelRect = rect;
        labelRect.x = rect.x - offset;
        labelRect.y = rect.y - offset;

        GUI.Label(labelRect, value, back);
        labelRect = rect;
        labelRect.x = labelRect.x + offset;
        labelRect.y = rect.y - offset;
        GUI.Label(labelRect, value, back);
        labelRect = rect;
        labelRect.x = labelRect.x - offset;
        labelRect.y = rect.y + offset;

        GUI.Label(labelRect, value, back);
        labelRect = rect;
        labelRect.x = labelRect.x + offset;
        labelRect.y = rect.y + offset;
        GUI.Label(labelRect, value, back);

        labelRect = rect;
        labelRect.x = rect.x - offset;
        GUI.Label(labelRect, value, back);
        labelRect = rect;
        labelRect.x = labelRect.x + offset;
        GUI.Label(labelRect, value, back);
        labelRect = rect;
        labelRect.y = rect.y + offset;

        GUI.Label(labelRect, value, back);
        labelRect = rect;
        labelRect.y = rect.y - offset;
        GUI.Label(labelRect, value, back);
        for (int i = 0; i < count; i++)
            GUI.Label(rect, value, front);
    }


    public GUIStyle LoadGUIStyle(Color co, TextAnchor anchor, int size, bool state)
    {
        GUIStyle gui = new GUIStyle();
        gui.normal.textColor = co;
        gui.hover.textColor = co;
        gui.active.textColor = co;
        gui.alignment = anchor;
        gui.fontSize = size;
        gui.wordWrap = state;
        return gui;
    }
    
    public  void LoadMain()
    {
        GameObject resRoot = (GameObject)Resources.Load("NGUI_Root");
        GameObject rootObj = (GameObject)GameObject.Instantiate(resRoot, resRoot.transform.position, resRoot.transform.rotation);
        GameObject mainParent = rootObj.transform.FindChild("Anchor").gameObject;

        GameObject resMain = (GameObject)Resources.Load("NGUI_Main");
        GameObject mainObj = (GameObject)GameObject.Instantiate(resMain, rootObj.transform.position, Quaternion.identity);

        mainObj.layer = mainParent.layer;
        mainObj.transform.parent = mainParent.transform;
        mainObj.transform.localScale = Vector3.one;
        mainObj.transform.localPosition = new Vector3(0, 0, 1000.0f);
   
    }
    void Start()
    {
        //HostAwake();
    }
	bool hasWake=false;
	int frame=0;
    void Update()
    {  
        if (Root.newVersionCheckOK)
        {
            if (!hasWake)
            {
                frame++;
                if (frame > 2)
                {
                    HostAwake();

                    hasWake = true;
                }
            }

            if (!HasInit)
                return;

            updateStateChange();
        }
    }
    void OnGUI()
    {
        if (Root.newVersionCheckOK)
        {
            if (HasInit)
            {
                GUI.skin.font = GBK_Font;
                drawBack();
            }
            else
            {
                //GUI.backgroundColor = Color.red;
                //if (GUI.Button(new Rect(0, 0, 2000, 2000), ""))
                //{
                //    Debug.LogError("buttion---------------->");
                //    AndroidCommandMgr.Single.CommandToAndroid("stopMedia", "0");
                //}
            }
        }
    }

    void init()
    {
        CoreLoadHelp.Init(this);

        string tPath = "";
        createFolder(ref tPath, "Data");
        createFolder(ref tPath, "NGUI");
        createFolder(ref tPath, "Icon");
        createFolder(ref tPath, "Audio");
        createFolder(ref tPath, "Item");
        createFolder(ref tPath, "Config");

        getUserInfoStorage();
        UserInfoStorage.StorageInfo.SetHandle = setUserInfoStorage;

        CoreLoadHelp.ExitHandle += delegate(byte res)
        {
            if (res == 1)
            {
                UpdateState = 14;
            }
            else if (res == 2)
            {
                UpdateState = 15;
            }
            else if (res == 3)
            {
                uiLoadDuration = 0;
                loadProcess = 0;
                UpdateState = 16;
            }
            else if (res == 4)
            {
                this.enabled = false;
            }
        };

        UpdateState = -6;
    }
    void setStyleBoarder(GUIStyle sty, int left, int right, int top, int bottom)
    {
        sty.border.left = left;
        sty.border.right = right;
        sty.border.top = top;
        sty.border.bottom = bottom;
    }
    GUIStyle createStyle(string path, string normalName, bool isBtn)
    {
        GUIStyle sty = new GUIStyle();
        if (isBtn)
        {
            sty.normal.background = Resources.Load(path + normalName) as Texture2D;
            sty.hover.background = sty.normal.background;
            sty.active.background = sty.normal.background;
        }
        return sty;
    }
    GUIStyle createStyle(string path, string name)
    {
        GUIStyle sty = new GUIStyle();
        sty.normal.background = Resources.Load(path + name) as Texture2D;
        return sty;
    }
    GUIStyle loadGUIStyle(Color co, TextAnchor anchor, int size, bool state)
    {
        GUIStyle gui = new GUIStyle();
        gui.normal.textColor = co;
        gui.hover.textColor = co;
        gui.active.textColor = co;
        gui.alignment = anchor;
        gui.fontSize = size;
        gui.wordWrap = state;
        return gui;
    }
    GUIStyle font_MCWhite_18 = null;
    GUIStyle font_MCGrey_18 = null;
    GUIStyle font_MCYellow_26 = null;
    GUIStyle font_MCRed_26 = null;
    GUIStyle font_MCGrey_16 = null;
    void showMsgbox(string desc, string sure, Action<int> callback)
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 636 / 2, Screen.height / 2 - 120, 636, 338));
        GUI.Label(new Rect(150, 0, 336, 338), "", CoreLoadHelp.styAskBack);
        GUILabelBorder(new Rect(30, 46, 575, 40), 1.0f, "提示", font_MCBlackMiaobian_30, font_MCWhite_30_1, 1);
        GUILabelBorder(new Rect(154, 96, 330, 80), 1.0f, desc, font_MCBlackMiaobian_26, font_MCBlack_26, 1);
        if (GUI.Button(new Rect(252, 222, 135, 44), "", CoreLoadHelp.btnAskSure))
        {
            callback(1);
        }
        GUILabelBorder(new Rect(252, 222, 135, 44), 2.0f, sure, font_MCGreenMiaobian_36, font_MCWhite_36, 3);
        GUI.EndGroup();
    }
    void updateStateChange()
    {
        if (!CoreLoadHelp.Ready) return;
        switch (UpdateState)
        {
            case -6:
                {
                    if (!string.IsNullOrEmpty(UserInfoStorage.StorageInfo.ConfigPath))
                    {
#if UNITY_IPHONE && !UNITY_EDITOR
                    CoreLoadHelp.ServerListPath = UserInfoStorage.StorageInfo.ConfigPath;
#elif UNITY_ANDROID && !UNITY_EDITOR
                    CoreLoadHelp.ServerListPath = UserInfoStorage.StorageInfo.ConfigPath;
              
#else
                    CoreLoadHelp.ServerListPath = UserInfoStorage.StorageInfo.ConfigPath;
#endif

                    }
                    UpdateState = -5;
                    break;
                }
            case -5:
                {
                    CoreLoadHelp.AddLog("CoreLoadHelp.getVersion:" + CoreLoadHelp.ServerListPath);
                    //CoreLoadHelp.LoadText(this, CoreLoadHelp.ServerListPath + Platform, getVersion);
                    //*****
                    //CoreLoadHelp.LoadText(this, "http://127.17.2.93/version.txt", getVersion);
                    UpdateState = -4;
                    getVersion();
                    break;
                }
            case -4:
                {
                    calAnimationStr();
                    break;
                }
            case 1:
                {
                    CoreLoadHelp.AddLog("CoreLoadHelp.LoadText:" + CoreLoadHelp.FileVersionConfirmPath + UserInfoStorage.StorageInfo.LastFileTick);
                    //CoreLoadHelp.LoadText(this, CoreLoadHelp.FileVersionConfirmPath + UserInfoStorage.StorageInfo.LastFileTick, delegate(string r)
                    //*****
                    //CoreLoadHelp.LoadText(this, "http://172.17.2.93/flist.txt", delegate(string r)
                    CoreLoadHelp.LoadText(this, CoreLoadHelp.FileVersionConfirmPath + "flist.txt", delegate(string r)
                    {
                        verionStr = "faint:" + r;
                        UpdateState = 3;
                    });
                    UpdateState = 2;
                    break;
                }
            case 2:
                {
                    calAnimationStr();
                    break;
                }
            case 3:
                {
                    if (verionStr.Length == 0)
                    {
                        UpdateState = 4;
                    }
                    else
                    {
                        fileList.Clear();
                        allSize = 0;
                        string[] files = verionStr.Split(new char[] { ':' });
                        files = files[1].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (string f in files)
                        {
                            f.Replace("\r\n", "");

                            string[] fs = f.Split(new char[] { ',' });
                            FileTmp t = new FileTmp();
                            t.FileName = fs[0];
                            t.FileName.Trim();
                            t.Path = fs[1];
                            float.TryParse(fs[2], out t.Size);
                            allSize += t.Size;
                            t.ModifyTime = fs[3];
                            t.MD5 = fs[4];
                            t.FullName = CoreLoadHelp.StoragePath + t.Path + "/" + t.FileName;
                            fileList.Add(t);
                        }
                        allSize = allSize / 1024.0f;
                        if (fileList.Count > 0)
                        {
                            UpdateState = 12;
                            updateIndex = 0;
                        }
                        else
                        {
                            UpdateState = 9;
                        }
                    }
                    break;
                }
            case 5:
                {
                    if (updateIndex < fileList.Count)
                    {
                        updateTmp = fileList[updateIndex];
                        loadRetry = 0;
                        if (checkFile(updateTmp))
                        {
                            UpdateState = 6;
                        }
                        else
                        {
                            UpdateState = 8;
                        }
                    }
                    else
                    {
                        UpdateState = 9;
                    }
                    calAnimationStr();
                    break;
                }
            case 6:
                {
                    if (loadRetry < 2)
                    {
                        itemTmp = CoreLoadHelp.LoadBytes(this, string.Concat(CoreLoadHelp.FileVersionAssetPath, updateTmp.Path, "/", updateTmp.FileName), loadCallBack);
                        //*****
                        //itemTmp = CoreLoadHelp.LoadBytes(this, string.Concat("http://172.17.2.93/", updateTmp.Path, "/", updateTmp.FileName), loadCallBack);
                        //itemTmp = CoreLoadHelp.LoadBytes(this, string.Concat("http://127.0.0.1/", updateTmp.Path, "/", updateTmp.FileName), loadCallBack);
                    }
                    else
                    {
                        itemTmp = CoreLoadHelp.LoadBytes(this, string.Concat(CoreLoadHelp.FileVersionAssetPath1, updateTmp.Path, "/", updateTmp.FileName), loadCallBack);
                        //*****
                        //itemTmp = CoreLoadHelp.LoadBytes(this, string.Concat("http://172.17.2.93/", updateTmp.Path, "/", updateTmp.FileName), loadCallBack);
                        //itemTmp = CoreLoadHelp.LoadBytes(this, string.Concat("http://127.0.0.1/", updateTmp.Path, "/", updateTmp.FileName), loadCallBack);
                    }
                    UpdateState = 7;
                    calAnimationStr();
                    break;
                }
            case 7:
                {
                    calAnimationStr();
                    break;
                }
            case 8:
                {
                    updateIndex++;
                    UpdateState = 5;
                    calAnimationStr();
                    break;
                }
            case 9://下载成功处理
                {
                    Root.canClickLogin = true;
                    UpdateState = 10;
                    CoreLoadHelp.ChangeProcess = delegate(float f) { calAnimationStrChange(); loadProcess = f; };
                    //GetComponent<Main>().enabled = true;
                    //if (updateTmp != null) UserInfoStorage.StorageInfo.LastFileTick = updateTmp.ModifyTime;
                    //UserInfoStorage.StorageInfo.SetHandle(true);
                    break;
                }
            case 10:
                {
                    calAnimationStr();
                    break;
                }
            case 11://下载失败处理
                {
                    calAnimationStr();
                    break;
                }
            case 12://判断是否是WIFI
                {
                    if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                    {
                        UpdateState = 5;
                    }
                    else
                    {
                        UpdateState = 13;
                    }
                    break;
                }
            case 13://等待用户选择是否下载更新
                {
                    calAnimationStr();
                    break;
                }
            case 14://加载UI资源
            case 16:
                {
                    calAnimationStr();
                    uiLoadDuration += Time.deltaTime;
                    if (uiLoadDuration >= uiLoadRefresh)
                    {
                        uiLoadDuration = 0;
                        loadProcess = loadProcess + ((1 - loadProcess) / 8f);
                    }
                    break;
                }
        }
    }
    private float animationStrDuration = 0;
    private string animationStr = string.Empty;
    private void calAnimationStr()
    {
        animationStrDuration += Time.deltaTime;
        if (animationStrDuration >= 0.60f)
        {
            animationStr = ".";
            animationStrDuration = 0;
        }
        if (animationStrDuration >= 0.40f)
        {
            animationStr = "...";
        }
        else if (animationStrDuration >= 0.20f)
        {
            animationStr = "..";
        }
        else if (animationStrDuration >= 0.0f)
        {
            animationStr = ".";
        }
    }
    private void calAnimationStrChange()
    {
        animationStrDuration += 0.2f;
        if (animationStrDuration >= 0.60f)
        {
            animationStr = ".";
            animationStrDuration = 0;
        }
        if (animationStrDuration >= 0.40f)
        {
            animationStr = "...";
        }
        else if (animationStrDuration >= 0.20f)
        {
            animationStr = "..";
        }
        else if (animationStrDuration >= 0.0f)
        {
            animationStr = ".";
        }
    }
    //private void getVersion(string paras)
    //{
    //    if (paras.Length == 0)
    //    {
    //        UpdateState = -2;
    //    }
    //    else
    //    {
    //        CoreLoadHelp.AddLog("paras :" + paras);
    //        string[] paraStrs = paras.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
    //        CoreLoadHelp.InitString = paraStrs;
    //        string[] serverVersion = paraStrs[0].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
    //        string[] currentVersion = CoreLoadHelp.AppVersion.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
    //        CoreLoadHelp.AddLog("CoreLoadHelp.AppVersion " + CoreLoadHelp.AppVersion);

    //        string[] fileVersionAssetPathList = paraStrs[1].Split(new char[] { '$' });
    //        CoreLoadHelp.FileVersionAssetPath = fileVersionAssetPathList[0];
    //        if (fileVersionAssetPathList.Length > 1)
    //        {
    //            CoreLoadHelp.FileVersionAssetPath1 = fileVersionAssetPathList[1];
    //        }
    //        else
    //        {
    //            CoreLoadHelp.FileVersionAssetPath1 = CoreLoadHelp.FileVersionAssetPath;
    //        }

    //        CoreLoadHelp.FileVersionConfirmPath = paraStrs[2];
    //        CoreLoadHelp.DownloadPage = paraStrs[3];
    //        CoreLoadHelp.NoticePage = paraStrs[4];
    //        if (string.Compare(serverVersion[0], currentVersion[0]) > 0
    //            || (string.Compare(serverVersion[0], currentVersion[0]) == 0 && string.Compare(serverVersion[1], currentVersion[1]) > 0)
    //            || (string.Compare(serverVersion[0], currentVersion[0]) == 0 && string.Compare(serverVersion[1], currentVersion[1]) == 0 && string.Compare(serverVersion[2], currentVersion[2]) > 0))
    //        {
    //            UpdateState = -1;
    //            CoreLoadHelp.AddLog("updateState = -1;");
    //        }
    //        else
    //        {
    //            StartUpdate();
    //        }
    //    }
    //}

    private void getVersion()
    {
        //先用一个更新地址
        CoreLoadHelp.FileVersionAssetPath = Root.patchAddress;
        CoreLoadHelp.FileVersionAssetPath1 = Root.patchAddress;
        CoreLoadHelp.FileVersionConfirmPath = Root.patchAddress;
        CoreLoadHelp.DownloadPage = Root.apkAddress;
        CoreLoadHelp.NoticePage = "http://ztapp.zt.ztgame.com";

       
        if (Root.newVersionCheckOK)
        {
            StartUpdate();
        }
    }

    private void loadCallBack(byte[] r)
    {
        if (r != null)
        {
            //if (updateTmp.FullName.EndsWith("Data/init.dat"))
            //{
            //    UserInfoStorage st = new UserInfoStorage();
            //    UserInfoStorage.Read(st, System.Text.Encoding.UTF8.GetString(r));
            //    UserInfoStorage.StorageInfo.LastVersion = st.LastVersion;
            //    CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
            //    setUserInfoStorage(true);
            //}
            //else
            {
                Util.LogError(updateTmp.FullName);
                File.WriteAllBytes(updateTmp.FullName, r);
            }
            r = null;
            loadRetry = 0;
            UpdateState = 8;
        }
        else
        {
            loadRetry++;
            switch(loadRetry)
            {
                case 1:
                case 2:
                    {
                        UpdateState = 6;
                        CoreLoadHelp.AddLog("load fail, retry:" + updateTmp.FileName);
                        break;
                    }
                default:
                    {
                        UpdateState = 11;
                        break;
                    }
            }
        }
    }
    private static bool checkFile(FileTmp f)
    {
        bool result = true;
        if (File.Exists(f.FullName))
        {
            tmpMD5 = MD5Help.GetMD5(f.FullName);
            if (string.Compare(tmpMD5, f.MD5) == 0)
            {
                result = false;
            }
        }
        return result;
    }
    private static int updateState = -7, loadRetry = 0;

    public static int UpdateState
    {
        get { return Host.updateState; }
        set 
        {
            Debug.Log("@@更新状态：" + Host.updateState + "==>" + value);
            Host.updateState = value;
        }
    }
    private static string verionStr = "", tmpMD5 = "";
    private static List<FileTmp> fileList = new List<FileTmp>();
    private static FileTmp updateTmp = null;
    private static LoadItem itemTmp;
    private static int updateIndex = 0;
    private static float allSize = 0, _loadProcess=0, uiLoadDuration = 0, uiLoadRefresh = .4f;
     
    private static float loadProcess
    {
        get 
        {
            return _loadProcess;
        }
         set
        {
            if (value < 1) _loadProcess = value;
            else _loadProcess = 1;

        }


    }
    private Vector2 vecScale;
    void drawBack()
    {
        float offset = Screen.height / 640.0f;
        switch (UpdateState)
        {
            case -6:
            case -5:
            case -4:
                {
                    GUILabelBorder(new Rect(0, Screen.height / 2 + 240 * offset, Screen.width, 30), 1.5f, string.Concat(animationStr, "正在验证版本", animationStr), font_MCGrey_18, font_MCWhite_18, 2);
                    break;
                }
            case -2:
                {
                    showMsgbox("获取服务器配置失败!", "重 试", delegate(int res)
                    {
                        if (res == 1) UpdateState = -5;
                    });
                    break;
                }
            case -1:
                {
                    showMsgbox("新版本出炉了,快去更新吧!时间打开房间独守空房", "确 定", delegate(int res)
                    {
                        if (res == 1)
                        {
                            Application.OpenURL(CoreLoadHelp.DownloadPage);
                            Application.Quit();
                        }
                    });
                    break;
                }
            case 0:
                {
                    break;
                }
            case 1:
            case 2:
                {
                    GUILabelBorder(new Rect(0, Screen.height / 2 + 240 * offset, Screen.width, 30), 1.5f, "正在检查文件列表版本", font_MCGrey_18, font_MCWhite_18, 2);
                    break;
                }
            case 4:
                {
                    showMsgbox("检查文件版本失败!", "重 试", delegate(int res)
                    {
                        if (res == 1)
                            StartUpdate();
                    });
                    break;
                }
            case 5:
            case 6:
            case 8:
                {
                    if (updateIndex < fileList.Count)
                    {
                        drawProcess(string.Concat(animationStr, "努力加载中", animationStr), string.Concat("更新资源(", (updateIndex + 1), "/", fileList.Count, ")"), 1);
                    }
                    break;
                }
            case 7:
                {
                    drawProcess(string.Concat(animationStr, "努力加载中", animationStr), string.Concat("更新资源(", (updateIndex + 1), "/", fileList.Count, ")"), itemTmp.Process);
                    break;
                }
            case 10:
                {
                    drawProcess(string.Concat(animationStr, "更新完成", animationStr), "更新完成, 可以进入游戏了！", 1);
                    break;
                }
            case 11:
                {
                    drawProcess(string.Concat(animationStr, "努力加载中", animationStr), "正在下载更新", loadProcess);
                    showMsgbox("您的网络不给力!", "重 试", delegate(int res)
                    {
                        if (res == 1)
                        {
                            UpdateState = 6;
                            loadRetry = 0;
                        }
                    });
                    break;
                }
            case 13:
                {
                    GUILabelBorder(new Rect(0, Screen.height / 2 + 240 * offset, Screen.width, 30), 1.5f, string.Concat(animationStr, "等待更新确认", animationStr), font_MCRed_26, font_MCYellow_26, 2);
                    showMsgbox("您的当前网络使用的是3G/GPRS,需要下载" + allSize.ToString("f1") + "MB更新文件吗?", "确定", delegate(int res)
                    {
                        if (res == 1) UpdateState = 5;
                    });
                    break;
                }
            case 14:
                {
                    drawProcess(string.Concat(animationStr, "努力加载中", animationStr), "正在加载UI资源", loadProcess);
                    break;
                }
            case 15:
                {

                    break;
                }
            case 16:
                {
                    drawProcess(string.Concat(animationStr, "努力加载中", animationStr), "正在加载玩家数据", loadProcess);
                    break;
                }
        }
        //GUILayout.Label(CoreLoadHelp.LogStr);
    }
    private void drawProcess(string _title, string _item, float _process)
    {
        float offset = Screen.height / 640.0f;
        GUILabelBorder(new Rect(0, Screen.height / 2 + 240 * offset, Screen.width, 30), 1.5f, _item, font_MCGrey_18, font_MCWhite_18, 2);
        GUI.Label(new Rect(Screen.width / 2 - 215, Screen.height / 2 + 210 * offset, 430, 19), "", CoreLoadHelp.styPlanBack);
        GUI.Label(new Rect(Screen.width / 2 - 213, Screen.height / 2 + 210 * offset + 3, 30 + 396 * _process, 13), "", CoreLoadHelp.styCurPlan);
        GUILabelBorder(new Rect(Screen.width / 2 - 315, Screen.height / 2 + 210 * offset, 630, 21), 1.5f, string.Concat((_process * 100).ToString("f2"), "%"), font_MCGrey_18, font_MCWhite_18, 2);
        GUI.Label(new Rect(0, Screen.height / 2 + 280 * offset, Screen.width, 30), "提示：为了使您的游戏运行更为流畅，游戏时请关闭不必要的应用", font_MCGrey_16);
    }
    public static void StartUpdate()
    {
        UpdateState = 1;
    }

    static void createFolder(ref string tPath, string name)
    {
        tPath = CoreLoadHelp.StoragePath + name;
        if (!System.IO.Directory.Exists(tPath)) System.IO.Directory.CreateDirectory(tPath);
    }
    static void clearFolder(ref string tPath, string name, bool initFile)
    {
        tPath = CoreLoadHelp.StoragePath + name;
        foreach (string f in Directory.GetFiles(tPath))
        {
            if (initFile && f.EndsWith("init.dat")) { }
            else
            {
                File.Delete(f);
            }
        }
    }
    private void getUserInfoStorage()
    {
        UserInfoStorage.StorageInfo = new UserInfoStorage();
        CoreLoadHelp.Ready = true;
        CoreLoadHelp.AppVersion = Root.GetLastVersion();
        CoreLoadHelp.AddLog("Ready = true;");


//        string userInfoPath = CoreLoadHelp.StoragePath + "Data/init.dat";
//        UserInfoStorage.StorageInfo = new UserInfoStorage();
//        try
//        {
//            if (File.Exists(userInfoPath))
//            {
//                using (FileStream fs = new FileStream(userInfoPath, FileMode.Open))
//                {
//                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
//                    {
//                        UserInfoStorage.Read(UserInfoStorage.StorageInfo, sr.ReadToEnd());
//                    }
//                }
//#if UNITY_EDITOR
//                CoreLoadHelp.Ready = true;
//                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
//                CoreLoadHelp.AddLog("Ready = true;");
//#elif UNITY_STANDALONE_WIN
//                CoreLoadHelp.Ready = true;
//                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
//                CoreLoadHelp.AddLog("Ready = true;");
//#elif UNITY_ANDROID
//                updateMobileApp("jar:file://" + Application.dataPath + "!/assets/Data/init.dat");//apk 内的版本
//#elif UNITY_IPHONE
//                updateMobileApp("file:///" + Application.dataPath + "/Raw/Data/init.dat");
//#endif
//            }
//            else
//            {

//#if UNITY_EDITOR && UNITY_STANDALONE_WIN
//                CoreLoadHelp.Ready = true;
//                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
//                CoreLoadHelp.AddLog("Ready = true;");
//#elif UNITY_IPHONE && UNITY_EDITOR
//                CoreLoadHelp.Ready = true;
//                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
//#elif UNITY_ANDROID && UNITY_EDITOR
//                CoreLoadHelp.Ready = true;
//                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
//                CoreLoadHelp.AddLog("Ready = true;");
//#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
//                CoreLoadHelp.Ready = true;
//                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
//                CoreLoadHelp.AddLog("Ready = true;");
//#elif UNITY_IPHONE && !UNITY_EDITOR
//                firstLoadInit("file:///" + Application.dataPath + "/Raw/Data/init.dat");
//#elif UNITY_ANDROID  && !UNITY_EDITOR
//                firstLoadInit("jar:file://" + Application.dataPath + "!/assets/Data/init.dat");
//#endif
//            }
//        }
//        catch (Exception ex)
//        {
//            CoreLoadHelp.AddLog(ex.ToString());
//        }
    }
    private int CompareVersion(string v1, string v2)
    {
        if (v1 == v2) return 0;
        string[] v1arr = v1.Split('.');
        string[] v2arr = v2.Split('.');
        for (int i = 0; i < 4; i++)
        {
            //if (int.Parse(v1arr[i]) = int.Parse(v1arr[i])) return 1;
            int ret = int.Parse(v1arr[i]) - int.Parse(v1arr[i]);
            if (ret != 0) return ret;

        }
        return 0;
    }
    private void updateMobileApp(string path)
    {
        CoreLoadHelp.LoadBytes(this, path, delegate(byte[] b)
        {
            try
            {
                UserInfoStorage st = new UserInfoStorage();//apk 内的版本
                UserInfoStorage.Read(st, System.Text.Encoding.UTF8.GetString(b));
                if (CompareVersion(st.LastVersion, UserInfoStorage.StorageInfo.LastVersion) > 0)
                {
                    CoreLoadHelp.AddLog(string.Format("version form {0} to {1} update, clear tmp folder!", UserInfoStorage.StorageInfo.LastVersion, st.LastVersion));
                    string tPath = "";
                    clearFolder(ref tPath, "Data", true);
                    clearFolder(ref tPath, "NGUI", false);
                    clearFolder(ref tPath, "Card", false);
                    clearFolder(ref tPath, "Icon", false);
                    clearFolder(ref tPath, "Terrains", false);
                    clearFolder(ref tPath, "Units", false);
                    clearFolder(ref tPath, "Units_skill", false);
                    clearFolder(ref tPath, "Audio", false);
                    clearFolder(ref tPath, "Item", false);
                    clearFolder(ref tPath, "AE", false);
                    clearFolder(ref tPath, "Arena", false);
                    UserInfoStorage.StorageInfo.LastFileTick = st.LastFileTick;
                    UserInfoStorage.StorageInfo.LastVersion = st.LastVersion;
                    UserInfoStorage.StorageInfo.SetHandle(true);
                }
                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
                CoreLoadHelp.Ready = true;
                CoreLoadHelp.AddLog("Ready = true;");
            }
            catch (Exception ex)
            {
                CoreLoadHelp.AddLog(ex.ToString());
            }
        });
    }
    private void firstLoadInit(string path)
    {
        CoreLoadHelp.LoadBytes(this, path, delegate(byte[] b)
        {
            try
            {
                UserInfoStorage.Read(UserInfoStorage.StorageInfo, System.Text.Encoding.UTF8.GetString(b));
                CoreLoadHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
                CoreLoadHelp. Ready = true;
                UserInfoStorage.StorageInfo.SetHandle(true);
                CoreLoadHelp.AddLog("Ready = true;");
            }
            catch (Exception ex)
            {
                CoreLoadHelp.AddLog(ex.ToString());
            }
        });
    }
    private static void setUserInfoStorage(bool b)
    {
        try
        {
            using (FileStream fs = new FileStream(CoreLoadHelp.StoragePath + "Data/init.dat", FileMode.Create))
            {
                using (StreamWriter sr = new StreamWriter(fs, Encoding.UTF8))
                {
                    sr.Write(UserInfoStorage.Write(UserInfoStorage.StorageInfo));
                }
            }
        }
        catch (Exception ex)
        {
            CoreLoadHelp.AddLog(ex.ToString());
        }
    }
    //public void CommandFromAndroid(string para)
    //{
        
    //    AndroidCommandMgr.Single.HandleCommand(para);
    //}
}
public class FileTmp
{
    public string FileName;
    public string Path;
    public float Size;
    public string ModifyTime;
    public string MD5;
    public string FullName;
}
public enum PlatformType
{
    WINDOWS_STANDALONE = 0,
    IOS_91 = 50,
    IOS_APPSTORE,

    ANDROID_GIANT = 450,
    ANDROID_BAIDU,
    ANDROID_YINGYONGBAO,
    ANDROID_UC,
    ANDROID_CHUKONG,
    ANDROID_ADSAGE,

}
public class AndroidCommandMgr// : IAndroidCommand
{

    static AndroidCommandMgr _single = null;
 /// <summary>
 /// Android消息接收
 /// </summary>
 /// <param name="s"></param>
    public  void HandleCommand(string s)
    {
        
            String[] cmds = s.Split(':');
            if (cmds.Length > 1)
            {
                int len = cmds[0].Length + 1;
                AndroidCommandHandle(cmds[0], s.Substring(len));
            }


        
    }
    Dictionary<string, Action<string>> HandleMap = new Dictionary<string, Action<string>>();
    private AndroidCommandMgr()
    {
   
    }
    public static AndroidCommandMgr Single
    {
        get
        {
            if (_single == null)
            {
                _single = new AndroidCommandMgr();

            }
            return _single;
        }

    }
    /// <summary>
    /// 注册消息代理
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="fun"></param>
    public void RegionAndroidCommandHandle(string mod, Action<string> fun)
    {
        HandleMap[mod] = fun;
    }
    private void AndroidCommandHandle(string mod, string msg)
    {
        // HandleMap["MediaPlayOver"] = MediaPlayOver;
        Debug.LogError("AndroidCommandHandle" + mod + "--" + msg);
        if (HandleMap.ContainsKey(mod))
        {
            HandleMap[mod](msg);
        }
        else
        {

            Debug.LogError("UnRegion Handle:" + mod + ":" + msg);
        }

    }

    
    /// <summary>
    /// /向Android发消息
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="msg"></param>
    public void CommandToAndroid(string mod,string msg) 
    {
        #if UNITY_ANDROID||UNITY_EDITOR
        androidCall("Command", mod+":"+msg);
        #endif
    }
#if UNITY_ANDROID||UNITY_EDITOR
    private  AndroidJavaObject _jo = null;
	 	#endif
    private  void androidCall(string m, string p)
    {
#if UNITY_ANDROID||UNITY_EDITOR
        (jo as AndroidJavaObject).Call(m, p);
#endif
    }


    #region IAndroidCommand 成员



    #endregion

    #region IAndroidCommand 成员


    public System.Object jo
    {
#if UNITY_ANDROID||UNITY_EDITOR
        get
        {
            if (_jo == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                _jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return _jo;
        }
        set
        {
            _jo = (AndroidJavaObject)value;
        }
#else
        get;
        set;
#endif

    }

    #endregion
}


