using System.Net;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Net;
using System;
using msg;
using Mono.Xml;
using System.Security;
using System.Text;
using System.IO;
using NGUI;
public enum USAGE
{
    eSend,
    eRecv,
};

public class LoginNetWork: NetWorkBase
{
    private Dictionary<int, string> loginErrorDict;

    private LoginType loginType;
    private string uuid;
    private string pwd;
    private ushort game;
    private ushort zone;
    private ushort userType;
    private string account;

    public string clientIP = "";
    public byte[] szKeyIP = new byte[16];
    public static uint m_dwEncryptMask = 0;
    public static uint m_iLenSended = 0;
    public static uint m_iLenRecved = 0;

    public static uint m_gameVersion = 0;

    public static bool IsNeedEncrypt(USAGE usage)
    {
        uint iData = 0;
	    if( usage == USAGE.eSend)
		    iData = m_iLenSended;
	    else 
		    iData = m_iLenRecved;
	    uint i = 0x80000000;
        i = i >> (int)iData;
        if ((m_dwEncryptMask & i) != 0)
        {
		    return true;
	    }
	    else 
        {
		    return false;
	    }
    }

    public static void IncrementSendData()
    {
        m_iLenSended++;
        if (m_iLenSended >= 32)
            m_iLenSended = 0;
    }
    public static void IncrementRecvData()
    {
        m_iLenRecved++;
        if (m_iLenRecved >= 32)
            m_iLenRecved = 0;
    }

    public static void ResetSendRevCount()
    {
        m_iLenSended = 0;
        m_iLenRecved = 0;
    }



    public static stPhoneInfo m_phoneinfo = null; // 机器信息，推送id,分辨率等

    public List<IPEndPoint> FLIPEndPoints = new List<IPEndPoint>();

    public List<ServerInfo> SrvLists = new List<ServerInfo>();

    public IPEndPoint CurrentIpEndPoint;

    public int ConnectFlTimes = 0;

    public override void Initialize()
    {
        m_phoneinfo = new stPhoneInfo();
        InitErrorDict();
        RegisterMsg();
    }

    public void ParseSrvlist(string str)
    {
        if (str == null)
        {
            return;
        }

        byte[] bytes = Encoding.UTF8.GetBytes(str);
        var fs = new MemoryStream(bytes);

        StreamReader reader = new StreamReader(fs, Encoding.UTF8);

        SecurityParser MonoXmlParser = new SecurityParser();

        MonoXmlParser.LoadXml(reader.ReadToEnd());

        SecurityElement se = MonoXmlParser.ToXml();

        foreach (SecurityElement child in se.Children)
        {
            if (child.Tag == "zones")
            {
                foreach (SecurityElement childchild in child.Children)
                {
                    if (childchild.Tag == "zone")
                    {
                        ServerInfo srvinfo = new ServerInfo();
                        srvinfo.name = childchild.Attribute("name");
                        srvinfo.id = int.Parse(childchild.Attribute("id"));
                        srvinfo.ip = childchild.Attribute("addr");
                        srvinfo.port = childchild.Attribute("port");
                        srvinfo.zone = int.Parse(childchild.Attribute("zone"));

                        //string test = UMessage.utf8_gbk("哈");

                        //string name1 = UMessage.gbk_utf8(srvinfo.name);
                        //string name2 = UMessage.utf8_gbk(srvinfo.name);

                        //Debug.Log("name: " + name1 + " " + name2 + " zone:" + srvinfo.id + " ip:" + srvinfo.ip + " port:" + srvinfo.port);
                        //Util.Log("name: " + srvinfo.name + " zone:" + srvinfo.id + " ip:" + srvinfo.ip + " port:" + srvinfo.port);
                        SrvLists.Add(srvinfo);
                    }
                }

                LoginModule.Instance.uiLogin.RefreshSrvlist();
            }
            else if (child.Tag ==  "version")
            {
                m_gameVersion = uint.Parse(child.Attribute("ver"));
                Util.Log("@@CurGameVersion: " + m_gameVersion);
            }
            else if (child.Tag == "appupdate")
            {
                Root.serverVersion = child.Attribute("ver");
                Root.serverVersionInited = true;

                Root.apkAddress = child.Attribute("addr");

                Root.patchAddress = child.Attribute("patchaddr");

                if (Root.localApkVersionInited && Root.localPersistentVersionInited)
                    Root.CheckNewVersion();
           
            }
 #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            //编辑器模式下ditorlog覆盖log
            else if (child.Tag == "log")
            {

                Util.fileLogLevel = int.Parse(child.Attribute("fileLogLevel"));
                Util.sendLogLevel = int.Parse(child.Attribute("sendLogLevel"));
                Util.logFuncType = int.Parse(child.Attribute("logFuncType"));
                int showDebugInfo = int.Parse(child.Attribute("showDebugInfo"));
                Util.showDebugInfo = (showDebugInfo == 1);

                Util.Log("Util.fileLogLevel: " + Util.fileLogLevel);
                Util.Log("Util.sendLogLevel: " + Util.sendLogLevel);
                Util.Log("Util.logFuncType: " + Util.logFuncType);
                Util.Log("Util.showDebugInfo: " + Util.showDebugInfo);
            }
            else if (child.Tag == "editorlog")
            {
                Util.fileLogLevel = int.Parse(child.Attribute("fileLogLevel"));
                Util.sendLogLevel = int.Parse(child.Attribute("sendLogLevel"));
                Util.logFuncType = int.Parse(child.Attribute("logFuncType"));
                int showDebugInfo = int.Parse(child.Attribute("showDebugInfo"));
                Util.showDebugInfo = (showDebugInfo == 1);

                Util.Log("Util.fileLogLevel: " + Util.fileLogLevel);
                Util.Log("Util.sendLogLevel: " + Util.sendLogLevel);
                Util.Log("Util.logFuncType: " + Util.logFuncType);
                Util.Log("Util.showDebugInfo: " + Util.showDebugInfo);
            }
#else
            else if (child.Tag == "log")
            {

                Util.fileLogLevel = int.Parse(child.Attribute("fileLogLevel"));
                Util.sendLogLevel = int.Parse(child.Attribute("sendLogLevel"));
                Util.logFuncType = int.Parse(child.Attribute("logFuncType"));
                int showDebugInfo = int.Parse(child.Attribute("showDebugInfo"));
                Util.showDebugInfo = (showDebugInfo == 1);

                Util.Log("Util.fileLogLevel: " + Util.fileLogLevel);
                Util.Log("Util.sendLogLevel: " + Util.sendLogLevel);
                Util.Log("Util.logFuncType: " + Util.logFuncType);
                Util.Log("Util.showDebugInfo: " + Util.showDebugInfo);
                
            }
#endif


        }
    }

    public void ParseLocalPersistentVersion(string str)
    {
        if (str == null)
        {
            return;
        }

        byte[] bytes = Encoding.UTF8.GetBytes(str);
        var fs = new MemoryStream(bytes);

        StreamReader reader = new StreamReader(fs, Encoding.UTF8);

        SecurityParser MonoXmlParser = new SecurityParser();

        MonoXmlParser.LoadXml(reader.ReadToEnd());

        SecurityElement se = MonoXmlParser.ToXml();

        foreach (SecurityElement child in se.Children)
        {
            if (child.Tag == "appupdate")
            {
                Root.localPersistentVersion = child.Attribute("ver");
                Root.localPersistentVersionInited = true;

                if (Root.serverVersionInited && Root.localApkVersionInited)
                    Root.CheckNewVersion();
            }
        }
    }

    public void ParseLocalApkVersion(string str)
    {
        if (str == null)
        {
            return;
        }

        byte[] bytes = Encoding.UTF8.GetBytes(str);
        var fs = new MemoryStream(bytes);

        StreamReader reader = new StreamReader(fs, Encoding.UTF8);

        SecurityParser MonoXmlParser = new SecurityParser();

        MonoXmlParser.LoadXml(reader.ReadToEnd());

        SecurityElement se = MonoXmlParser.ToXml();

        foreach (SecurityElement child in se.Children)
        {
            if (child.Tag == "appupdate")
            {
                Root.localApkVersion = child.Attribute("ver");
                Root.localApkVersionInited = true;

                if (Root.serverVersionInited && Root.localPersistentVersionInited)
                    Root.CheckNewVersion();
            }
        }
    }


    public void ParseFLEndpoint(string str)
    {
        if (str == null)
        {
            return;
        }

        FLIPEndPoints.Clear();
        ConnectFlTimes = 0;

        string[] tmp = str.Split(new char[]{';'},StringSplitOptions.RemoveEmptyEntries);
        string[] ips = tmp[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        string[] eps = tmp[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < ips.Length; i++)
        {
            IPAddress ipAddress = IPAddress.Parse(ips[i].Trim());
            int pt = int.Parse(eps[i]);
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, pt % 65536);
            FLIPEndPoints.Add(serverEndPoint);
        }
        CurrentIpEndPoint = FLIPEndPoints[0];
    }

    public void ParseFLEndpoint(string addr, string port)
    {
        if (addr == null)
        {
            return;
        }

        FLIPEndPoints.Clear();
        ConnectFlTimes = 0;

        string[] ips = addr.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        string[] eps = port.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < ips.Length; i++)
        {
            IPAddress ipAddress = IPAddress.Parse(ips[i].Trim());
            int pt = int.Parse(eps[i]);
            IPEndPoint serverEndPoint = new IPEndPoint(ipAddress, pt % 65536);
            FLIPEndPoints.Add(serverEndPoint);
        }
        CurrentIpEndPoint = FLIPEndPoints[0];
    }

    private void InitErrorDict()
    {
        loginErrorDict = new Dictionary<int, string>();
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_UNKNOWN, "未知错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_VERSIONERROR, "版本错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_UUID, "UUID登陆方式没有实现");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_DB, "数据库出错");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_PASSWORDERROR, "帐号密码错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_CHANGEPASSWORD, "修改密码成功");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_IDINUSE, "ID正在被使用中");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_IDINCLOSE, "ID被封");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_GATEWAYNOTAVAILABLE, "网关服务器未开");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_USERMAX, "用户满");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_ACCOUNTEXIST, "账号已经存在");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_ACCOUNTSUCCESS, "注册账号成功");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_CHARNAMEREPEAT, "角色名称重复");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_USERDATANOEXIST, "用户档案不存在");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_USERNAMEREPEAT, "用户名重复");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_TIMEOUT, "连接超时");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_PAYFAILED, "计费失败");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_JPEG_PASSPORT, "图形验证码输入错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_LOCK, "帐号被锁定");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_WAITACTIVE, "帐号待激活");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_NEWUSER_OLDZONE, "新账号不允许登入旧的游戏区");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_UUID_ERROR, "登录UUID错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_USER_TOZONE, "角色已登录战区,不允许创建角色");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_CHANGE_LOGIN, "跨区登陆验证失败");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_MATRIX_ERROR, "登录矩阵卡密码错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_MATRIX_NEED, "提示玩家需要输入矩阵卡密码");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_MATRIX_LOCK, "提示玩家矩阵卡被锁（六个小时后解锁）");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_MATRIX_DOWN, "与矩阵卡验证服务器失去连接");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_OLDUSER_NEWZONE, "旧帐号不允许登陆新区");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_IMG_LOCK, "图形验证连续错误3次,角色被锁定");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_PASSPOD_PASSWORDERROR, "密保密码错误");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_PASSPOD_DOWN, "与密保服务器失去连接");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_BUSY, "服务器繁忙");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_FORBID, "帐号被封停");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_IMG_LOCK2, "图形验证连续错误9次，角色被锁定");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_MAINTAIN, "游戏区正常维护中，原来的错误码LOGIN_RETURN_GATEWAYNOTAVAILABLE表示非正常维护");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_TDCODE_GEN_ERROR, "获取二维码失败");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_TDCODE_DOWN, "二维码服务不可用,请输入帐号密码登陆");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_TOKEN_ERROR, "token验证失败");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_TOKEN_TOO_QUICK, "TOKEN验证太快，意思说上次验证还没结束，就开始新的验证");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_TOKEN_TIMEOUT, "TOKEN验证超时");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_SHOW_MSG, "显示后面的错误消息");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_USERINLOGIN, "用户已经登录");
        loginErrorDict.Add((int)LoginRetCode.LOGIN_RETURN_CHARNAME_INVALID, "昵称含有敏感词汇");
    }

    #region FirServer

    public void ConnectFirServer(IPEndPoint ipEndPoint, LoginType logintype, ushort zone, string pwd, ushort game,
        string uuid, ushort usertype, string account)
    {
        ConnectFirServer(ipEndPoint.Address.ToString(), ipEndPoint.Port, logintype, zone, pwd, game, uuid, usertype, account);
    }

    public void ConnectFirServer(string firip,int firport,LoginType logintype,ushort zone, string pwd, ushort game, string uuid,ushort usertype,string account)
    {
        //firIp = "192.168.172.6";
        //firPort = 1500;

        //loginType = LoginType.UUID;
        //pwd = "123456";
        //game = 7;
        //this.zone = zone;
        //userType = 2;

        this.loginType = logintype;
        this.zone = zone;
        this.pwd = pwd;
        this.game = game;
        this.uuid = uuid;
        this.userType = usertype;
        this.account = account;

        NetWorkModule.Instance.Connect(new IPEndPoint(IPAddress.Parse(firip), firport), USocketType.Fir, OnFirSocketConnect, OnFirSocketConnectFail);
    }

    private void OnFirSocketConnect()
    {
        Util.Log("FirServer连接成功");
        
        SendFirCheckVersion();
        RequestClientIP();
        //SendFirLogin();
    }

    private void OnFirSocketConnectFail()
    {
        Util.Log("FirServer连接失败");
        ConnectFlTimes ++;
        if (ConnectFlTimes < FLIPEndPoints.Count)
        {
            CurrentIpEndPoint = FLIPEndPoints[ConnectFlTimes];
            LoginModule.Instance.Login();
        }
        else
        {
            Debug.Log("所有端口尝试失败，未完成连接");
        }
    }

    private void OnLoginFirError(UMessage message)
    {
        byte error = message.ReadByte();
        string errorStr = "";
        if (loginErrorDict.ContainsKey(error))
        {
            errorStr = loginErrorDict[error];
        }
        else
        {
            errorStr = "未处理错误";
        }
		
        NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
        {
            script.Init();
            script.InitDesc(errorStr);
        });

        Debug.Log(error + ">>>" + errorStr);
    }


	public LoginModel LoginModel;

    private void onLoginFirSuccess(UMessage message)
    {
		LoginModel = new LoginModel ();
		LoginModel.gatewayUserId = message.ReadUInt32();
		LoginModel.gatewayTempId = message.ReadUInt32();
		LoginModel.gateWayIp = message.ReadString(16);
        LoginModel.gateWayPort = message.ReadUInt16();
        //LoginModel.account = message.ReadString(GlobalVar.MAX_ACCNAMESIZE);
        LoginModel.key = message.ReadBytes(256);
        byte index = LoginModel.key[58];
        UMessage.DESEncryptHandle.ResetKey(LoginModel.key, index);

        m_dwEncryptMask = (uint)LoginModel.key[index + 2];

        uint state = message.ReadUInt32();
        Util.Log("Gateway ip :" + LoginModel.gateWayIp + "    port : " + LoginModel.gateWayPort);
        ConnnectGateWay();
    }

    private void onRequestIPOK(UMessage message)
    {
        clientIP = message.ReadString(16);
        szKeyIP = Encoding.UTF8.GetBytes(clientIP);
        Util.Log("Return Request Client IP: " + clientIP);
        SendFirLogin();
    }

    private void SendFirCheckVersion()
    {
        UMessage message = new UMessage();
        message.WriteHead(104, 120);
        message.WriteUInt32(0);
        //message.WriteUInt32(20140729);
        //message.WriteUInt32(20150204);
        message.WriteUInt32(LoginNetWork.m_gameVersion);
        NetWorkModule.Instance.SendImmediate(message);
    }

    /// <summary>
    /// 请求客户端IP
    /// </summary>
    private void RequestClientIP()
    {
        UMessage message = new UMessage();
        message.WriteHead(104, 15);
        NetWorkModule.Instance.SendImmediate(message);
    }

    private void SendFirLogin()
    {
        if (loginType == LoginType.UUID)
        {
            UMessage message = new UMessage();
            message.WriteHead(104, 2);

            message.WriteString(account, GlobalVar.MAX_ACCNAMESIZE);   //account

            ////密码111111对应的异或码
            //byte[] preIPSec = new byte[] { 12, 26, 25, 26, 25, 26, 25, 26, 25, 26, 25, 26, 25 };
            byte length = LoginModule.Instance.Pass[0];
            byte[] szPass = new byte[34];
            for (int i = 0; i < length+1; ++i)
            {
                szPass[i] = LoginModule.Instance.Pass[i];
            }
            UseIPEncry(ref szPass, length + 1);
            message.WriteBytes(szPass, 33);                           //pwd
            message.WriteUInt16(1);                                      //game
            message.WriteUInt16(zone);                                     //zone
            message.WriteString("", 7);                                   //图形验证码
            message.WriteString("74D02BC4CE2B", 13);                  //macaddr
            message.WriteString("))L1?o1?L1????z/ 2?", 25);    //uuid
            message.WriteUInt16(0);                                     //nettype
            message.WriteString("", 9);                                  //密保密码

            NetWorkModule.Instance.SendImmediate(message);
         }
    }

    void UseIPEncry(ref byte[] pszSrc, int iNum )
    {
        byte nKey = (byte)clientIP.Length, rkey = 0;

	    for ( int i = 0; i < iNum; i++ )
	    {
		    pszSrc[i] ^= szKeyIP[rkey];
		    pszSrc[i]++;

		    if ( ++rkey >= nKey )
			    rkey = 0;
	    }
    }

    #endregion

    #region GateWay
    


    public void ConnnectGateWay()
    {
        IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(LoginModel.gateWayIp), LoginModel.gateWayPort);
        NetWorkModule.Instance.Connect(ipEnd, USocketType.GateWay, OnGateWayConnect, OnGateWayConnectFail);
    }

    private void OnGateWayConnect()
    {
        if (LoginModule.Instance.uiLogin != null)
            LoginModule.Instance.uiLogin.AddGreenTip("成功连接服务器！");

		Debug.Log("连接网关(" + LoginModel.gateWayIp + ": " + LoginModel.gateWayPort + ")成功");
        
		SendGateWayCheckVersion();
        SendGateWayLogin();
    }

    private void OnGateWayConnectFail()
    {
		Debug.Log("连接网关(" + LoginModel.gateWayIp + ": " + LoginModel.gateWayPort + ")失败");
    }

    private void SendGateWayCheckVersion()
    {
        Debug.Log("SendGateWayCheckVersion");
        UMessage message = new UMessage();
        message.WriteHead(104, 120);
        //message.WriteHead((UInt16)CommandID.stUserVerifyVerCmd_CS);
        message.WriteUInt32(0);
        //message.WriteUInt32(20140729);
        //message.WriteUInt32(20150204);
        message.WriteUInt32(LoginNetWork.m_gameVersion);
        NetWorkModule.Instance.SendImmediate(message);
    }

    private void SendGateWayLogin()
    {
        Debug.Log("SendGateWayLogin");
        UMessage message = new UMessage();
        message.WriteHead(104, 5);
        //message.WriteUInt32(5898);
        //message.WriteUInt32(121000007);

        message.WriteUInt32(LoginModel.gatewayTempId);
        message.WriteUInt32(LoginModel.gatewayUserId);
          
        message.WriteString(account, GlobalVar.MAX_ACCNAMESIZE);
        message.WriteString("", GlobalVar.MAX_PASSWORD);

        //m_phoneinfo.WriteEmpty(message);
        NetWorkModule.Instance.SendImmediate(message);

        USocket.m_bLoginSend = false;
    }

    private void OnGateWayMergeVersionInfo(UMessage message)
    {
        uint version = message.ReadUInt32();
        Util.Log("网关返回版本号: " + version);

    }

    private void OnBattleStateUserCmd(UMessage message)
    {
        //enum BATTLETYPE
        //{ 
        //    BATTLE_NON = 0,					///空
        //    BATTLE_GLOBALGANGWAR = 1,		///夺城战战场区
        //    BATTLE_ZHENYING = 2,            ///阵营战 战区
        //    BATTLE_NEWGRADE = 3,            ///新段位赛
        //    BATTLE_ONEVSONE = 4,            //1VS1战场
        //    SOURCE_ZONE = 5,                //原区
        //};

        ////设置战区标志,客户端收到该消息,就表示进入的是战区
        //const BYTE BATTLESTATE_PARA     = 1;
        //struct stBattleStateUserCmd : public stBattleUserCmd
        //{
	
        //    stBattleStateUserCmd() : battleType(0)
        //    {
        //        byParam = BATTLESTATE_PARA;
        //    } 
        //    BYTE battleType;	///对应这枚举类型BATTLETYPE
        //};  
        byte flag = message.ReadByte();
        if (flag == 5)
        {
            Debug.Log("登入本区！");
            //Util.Log("登入本区！");
        }
        else
        {
            //Util.Log("不在本区，请先在电脑端返回本区！");
            Debug.LogError("不在本区，请先在电脑端返回本区！");
        }
    }

    private void OnGateWayLoginFail(UMessage message)
    {
        byte error = message.ReadByte();
        string errorStr = "";
        if (loginErrorDict.ContainsKey(error))
        {
            errorStr = loginErrorDict[error];
        }
        else
        {
            errorStr = "未处理错误";
        }
    }

    private void OnUserMapInfo(UMessage msg)
    {
        Debug.Log("OnUserMapInfo");
        //MSG_Ret_UserMapInfo_SC mdata = msg.ReadProto<MSG_Ret_UserMapInfo_SC>();

        //MapData mapdata = new MapData();
        //mapdata.FileName = "test_lol";
        //mapdata.MapID = 602;
        //mapdata.MapName = "test_lol6";
        //GameManager.Instance.CurrentMapData = mapdata;
        ConfigManager.Instance.Initialize();
        GameManager.Instance.CurrentMapData = ConfigManager.Instance.mapConfig.GetMapDataByID((int)602);

        if (LoginModule.Instance.uiLogin != null)
            LoginModule.Instance.uiLogin.AddGreenTip("准备加载地图。。");
    }

    /// <summary>
    /// 登录成功返回玩家信息
    /// </summary>
    /// <param name="message"></param>
    private void OnReturnUserInfo(UMessage message)
    {
        //const BYTE USERINFO_SELECT_USERCMD_PARA = 1;
        //struct stUserInfoUserCmd : public stSelectUserCmd
        //{
        //    stUserInfoUserCmd()
        //    {
        //        byParam = USERINFO_SELECT_USERCMD_PARA;
        //        bzero(charInfo, sizeof(charInfo));
        //        size = 0;
        //    }
        //    SelectUserInfo charInfo[MAX_CHARINFO];
        //    DWORD size;
        //    BYTE data[0];
        //};

        Debug.Log("返回角色信息！");
        LoginModule.Instance.uiLogin.RefreshRoleInfo(message);
        //LoginModule.Instance.uiLogin.ShowResisterUI();
        //直接登游戏
        LoginModule.Instance.uiLogin.onClickEnterGame();

        Util.canSendWarningDebugLog = true;

        //message.ReadUInt64();
        //GameManager.Instance.PlayerInfo.strName = message.ReadString(GlobalVar.MAX_NAMESIZE);
        //GameManager.Instance.PlayerInfo.bySex = message.ReadByte();
        //GameManager.Instance.PlayerInfo.wLevel = message.ReadUInt16();
        //GameManager.Instance.PlayerInfo.qwExp = message.ReadUInt64();
        //GameManager.Instance.PlayerInfo.dwFairyStone = message.ReadUInt32();
        //GameManager.Instance.PlayerInfo.dwGold = message.ReadUInt32();
        //GameManager.Instance.PlayerInfo.dwLeaderShip = message.ReadUInt32();
        //GameManager.Instance.PlayerInfo.dwPhyPower = message.ReadUInt32();
        //GameManager.Instance.PlayerInfo.dwMaxPhyPower = message.ReadUInt32();

        //Debug.Log("Playername:" + GameManager.Instance.PlayerInfo.strName + "   Sex:" + GameManager.Instance.PlayerInfo.bySex);

        ////临时写法
        //if (GameManager.Instance.PlayerInfo.wLevel == 0)
        //{
        //    LoginModule.Instance.isShowLoginUI = false;
        //    LoginModule.Instance.isShowRegisterUI = true;
        //    return;
        //}
    }
    #endregion

    public void SendRegisterPlayer(string name, byte sex,uint professional)
    {
        UMessage message = new UMessage();
        message.WriteHead((UInt16)CommandID.stCreateNewRoleUserCmd_CS);

        message.WriteString(name, GlobalVar.MAX_NAMESIZE);
        message.WriteByte(sex);
        message.WriteUInt32(10);        //平台ID
        m_phoneinfo.WriteCmd(message);
        message.WriteUInt32(professional);         //heroid
        message.WriteUInt32(LoginModel.gatewayUserId);

        base.SendMsg(message);
    }

    /// <summary>
    /// 登陆失败后返回的信息
    /// </summary>
    /// <param name="msg"></param>
    public void OnServerLoginFailed(UMessage msg)
    {
        MSG_Ret_ServerLoginFailed_SC data = msg.ReadProto<MSG_Ret_ServerLoginFailed_SC>();

        int error = (int)data.returncode;
        string errorStr = "";
        if (loginErrorDict.ContainsKey(error))
        {
            errorStr = loginErrorDict[error];


            //重连过程中服务器检测到此ID还在线返回LOGIN_RETURN_IDINUSE，需重连FlServer
            if (error == (int)LoginRetCode.LOGIN_RETURN_IDINUSE)
            {
                LoginModule.Instance.Login();
            }
        }
        else
        {
            errorStr = "未处理错误";
        }

        Debug.Log(error + ">>>" + errorStr);
    }

    /// <summary>
    /// 被T下线
    /// </summary>
    public void OnNotifyUserKickout(UMessage msg)
    {
        MSG_Ret_NotifyUserKickout_SC data = msg.ReadProto<MSG_Ret_NotifyUserKickout_SC>();

        Debug.LogError("账号在其他地点登陆，被踢下线！");
    }

    
	
    public override void RegisterMsg()
    {
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(104, 3), OnLoginFirError);
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(104, 4), onLoginFirSuccess);
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(104, 16), onRequestIPOK);
        //==========================================================================
        //合并版本号
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(3, 53), OnGateWayMergeVersionInfo);
        //战区标志
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(54, 1), OnBattleStateUserCmd);
        //角色信息
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(24, 1), OnReturnUserInfo);

        //地图信息
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(3, 28), OnUserMapInfo);


        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.stServerReturnLoginFailedCmd_SC, OnGateWayLoginFail);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.stReturnUserInfoUserCmd_SC, OnReturnUserInfo);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_UserMapInfo_SC, OnUserMapInfo);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_NotifyUserKickout_SC, OnNotifyUserKickout);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_ServerLoginFailed_SC, OnServerLoginFailed);
    }
}

public enum LoginType
{ 
    UUID,
    PlATFORM,
	NONE
}

public class LoginModel
{
    public uint gatewayUserId;
    public uint gatewayTempId;
    public string gateWayIp;
    public ushort gateWayPort;
    public string account;
    public byte[] key;
}
