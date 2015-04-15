using Mono.Xml;
using msg;
using Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using UnityEngine;

public class LoginNetWork : NetWorkBase
{
    private string account;
    public string clientIP = string.Empty;
    public int ConnectFlTimes;
    public IPEndPoint CurrentIpEndPoint;
    public List<IPEndPoint> FLIPEndPoints = new List<IPEndPoint>();
    private ushort game;
    private Dictionary<int, string> loginErrorDict;
    public LoginModel LoginModel;
    private LoginType loginType;
    public static uint m_dwEncryptMask;
    public static uint m_gameVersion;
    public static uint m_iLenRecved;
    public static uint m_iLenSended;
    public static stPhoneInfo m_phoneinfo;
    private string pwd;
    public List<ServerInfo> SrvLists = new List<ServerInfo>();
    public byte[] szKeyIP = new byte[0x10];
    private ushort userType;
    private string uuid;
    private ushort zone;

    public void ConnectFirServer(IPEndPoint ipEndPoint, LoginType logintype, ushort zone, string pwd, ushort game, string uuid, ushort usertype, string account)
    {
        this.ConnectFirServer(ipEndPoint.Address.ToString(), ipEndPoint.Port, logintype, zone, pwd, game, uuid, usertype, account);
    }

    public void ConnectFirServer(string firip, int firport, LoginType logintype, ushort zone, string pwd, ushort game, string uuid, ushort usertype, string account)
    {
        this.loginType = logintype;
        this.zone = zone;
        this.pwd = pwd;
        this.game = game;
        this.uuid = uuid;
        this.userType = usertype;
        this.account = account;
        SingletonForMono<NetWorkModule>.Instance.Connect(new IPEndPoint(IPAddress.Parse(firip), firport), USocketType.Fir, new OnSocketCallback(this.OnFirSocketConnect), new OnSocketCallback(this.OnFirSocketConnectFail));
    }

    public void ConnnectGateWay()
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(this.LoginModel.gateWayIp), this.LoginModel.gateWayPort);
        SingletonForMono<NetWorkModule>.Instance.Connect(ipEndPoint, USocketType.GateWay, new OnSocketCallback(this.OnGateWayConnect), new OnSocketCallback(this.OnGateWayConnectFail));
    }

    public static void IncrementRecvData()
    {
        m_iLenRecved++;
        if (m_iLenRecved >= 0x20)
        {
            m_iLenRecved = 0;
        }
    }

    public static void IncrementSendData()
    {
        m_iLenSended++;
        if (m_iLenSended >= 0x20)
        {
            m_iLenSended = 0;
        }
    }

    private void InitErrorDict()
    {
        this.loginErrorDict = new Dictionary<int, string>();
        this.loginErrorDict.Add(0, "未知错误");
        this.loginErrorDict.Add(1, "版本错误");
        this.loginErrorDict.Add(2, "UUID登陆方式没有实现");
        this.loginErrorDict.Add(3, "数据库出错");
        this.loginErrorDict.Add(4, "帐号密码错误");
        this.loginErrorDict.Add(5, "修改密码成功");
        this.loginErrorDict.Add(6, "ID正在被使用中");
        this.loginErrorDict.Add(7, "ID被封");
        this.loginErrorDict.Add(8, "网关服务器未开");
        this.loginErrorDict.Add(9, "用户满");
        this.loginErrorDict.Add(10, "账号已经存在");
        this.loginErrorDict.Add(11, "注册账号成功");
        this.loginErrorDict.Add(12, "角色名称重复");
        this.loginErrorDict.Add(13, "用户档案不存在");
        this.loginErrorDict.Add(14, "用户名重复");
        this.loginErrorDict.Add(15, "连接超时");
        this.loginErrorDict.Add(0x10, "计费失败");
        this.loginErrorDict.Add(0x11, "图形验证码输入错误");
        this.loginErrorDict.Add(0x12, "帐号被锁定");
        this.loginErrorDict.Add(0x13, "帐号待激活");
        this.loginErrorDict.Add(20, "新账号不允许登入旧的游戏区");
        this.loginErrorDict.Add(0x15, "登录UUID错误");
        this.loginErrorDict.Add(0x16, "角色已登录战区,不允许创建角色");
        this.loginErrorDict.Add(0x17, "跨区登陆验证失败");
        this.loginErrorDict.Add(0x18, "登录矩阵卡密码错误");
        this.loginErrorDict.Add(0x19, "提示玩家需要输入矩阵卡密码");
        this.loginErrorDict.Add(0x1a, "提示玩家矩阵卡被锁（六个小时后解锁）");
        this.loginErrorDict.Add(0x1b, "与矩阵卡验证服务器失去连接");
        this.loginErrorDict.Add(0x1c, "旧帐号不允许登陆新区");
        this.loginErrorDict.Add(0x1d, "图形验证连续错误3次,角色被锁定");
        this.loginErrorDict.Add(30, "密保密码错误");
        this.loginErrorDict.Add(0x1f, "与密保服务器失去连接");
        this.loginErrorDict.Add(0x20, "服务器繁忙");
        this.loginErrorDict.Add(0x21, "帐号被封停");
        this.loginErrorDict.Add(0x22, "图形验证连续错误9次，角色被锁定");
        this.loginErrorDict.Add(0x23, "游戏区正常维护中，原来的错误码LOGIN_RETURN_GATEWAYNOTAVAILABLE表示非正常维护");
        this.loginErrorDict.Add(0x24, "获取二维码失败");
        this.loginErrorDict.Add(0x25, "二维码服务不可用,请输入帐号密码登陆");
        this.loginErrorDict.Add(0x26, "token验证失败");
        this.loginErrorDict.Add(0x27, "TOKEN验证太快，意思说上次验证还没结束，就开始新的验证");
        this.loginErrorDict.Add(40, "TOKEN验证超时");
        this.loginErrorDict.Add(0x29, "显示后面的错误消息");
        this.loginErrorDict.Add(0x2a, "用户已经登录");
        this.loginErrorDict.Add(0x2b, "昵称含有敏感词汇");
    }

    public override void Initialize()
    {
        m_phoneinfo = new stPhoneInfo();
        this.InitErrorDict();
        this.RegisterMsg();
    }

    public static bool IsNeedEncrypt(USAGE usage)
    {
        uint iLenSended = 0;
        if (usage == USAGE.eSend)
        {
            iLenSended = m_iLenSended;
        }
        else
        {
            iLenSended = m_iLenRecved;
        }
        uint num2 = 0x80000000;
        num2 = num2 >> (int)iLenSended;
        return ((m_dwEncryptMask & num2) != 0);
    }

    private void OnBattleStateUserCmd(UMessage message)
    {
        if (message.ReadByte() == 5)
        {
            Debug.Log("登入本区！");
        }
        else
        {
            Debug.LogError("不在本区，请先在电脑端返回本区！");
        }
    }

    private void OnFirSocketConnect()
    {
        Debug.Log("FirServer连接成功");
        LUtil.Log("FirServer连接成功", LUtil.LogType.Normal, false);
        this.SendFirCheckVersion();
        this.RequestClientIP();
    }

    private void OnFirSocketConnectFail()
    {
        Debug.Log("FirServer连接失败");
        LUtil.Log("FirServer连接失败", LUtil.LogType.Normal, false);
        this.ConnectFlTimes++;
        if (this.ConnectFlTimes < this.FLIPEndPoints.Count)
        {
            this.CurrentIpEndPoint = this.FLIPEndPoints[this.ConnectFlTimes];
            LSingleton<LoginModule>.Instance.Login();
        }
        else
        {
            Debug.Log("所有端口尝试失败，未完成连接");
        }
    }

    private void OnGateWayConnect()
    {
        Debug.Log(string.Concat(new object[] { "连接网关(", this.LoginModel.gateWayIp, ": ", this.LoginModel.gateWayPort, ")成功" }));
        this.SendGateWayCheckVersion();
        this.SendGateWayLogin();
    }

    private void OnGateWayConnectFail()
    {
        Debug.Log(string.Concat(new object[] { "连接网关(", this.LoginModel.gateWayIp, ": ", this.LoginModel.gateWayPort, ")失败" }));
    }

    private void OnGateWayLoginFail(UMessage message)
    {
        byte key = message.ReadByte();
        string str = string.Empty;
        if (this.loginErrorDict.ContainsKey(key))
        {
            str = this.loginErrorDict[key];
        }
        else
        {
            str = "未处理错误";
        }
    }

    private void OnGateWayMergeVersionInfo(UMessage message)
    {
        LUtil.Log("网关返回版本号: " + message.ReadUInt32(), LUtil.LogType.Normal, false);
    }

    private void OnLoginFirError(UMessage message)
    {
        byte key = message.ReadByte();
        string str = string.Empty;
        if (this.loginErrorDict.ContainsKey(key))
        {
            str = this.loginErrorDict[key];
        }
        else
        {
            str = "未处理错误";
        }
        Debug.Log(key + ">>>" + str);
        LUtil.Log(key + ">>>" + str, LUtil.LogType.Normal, false);
    }

    private void onLoginFirSuccess(UMessage message)
    {
        this.LoginModel = new LoginModel();
        this.LoginModel.gatewayUserId = message.ReadUInt32();
        this.LoginModel.gatewayTempId = message.ReadUInt32();
        this.LoginModel.gateWayIp = message.ReadString(0x10);
        this.LoginModel.gateWayPort = message.ReadUInt16();
        this.LoginModel.key = message.ReadBytes(0x100);
        byte index = this.LoginModel.key[0x3a];
        UMessage.DESEncryptHandle.ResetKey(this.LoginModel.key, index);
        m_dwEncryptMask = this.LoginModel.key[index + 2];
        LUtil.Log("m_dwEncryptMask: " + m_dwEncryptMask, LUtil.LogType.Normal, false);
        Debug.Log(string.Concat(new object[] { "deskey :", this.LoginModel.key[index], " ", this.LoginModel.key[index + 1], " ", this.LoginModel.key[index + 2], " ", this.LoginModel.key[index + 3], " ", this.LoginModel.key[index + 4], " ", this.LoginModel.key[index + 5], " ", this.LoginModel.key[index + 6], " ", this.LoginModel.key[index + 7] }));
        LUtil.Log(string.Concat(new object[] { "deskey :", this.LoginModel.key[index], " ", this.LoginModel.key[index + 1], " ", this.LoginModel.key[index + 2], " ", this.LoginModel.key[index + 3], " ", this.LoginModel.key[index + 4], " ", this.LoginModel.key[index + 5], " ", this.LoginModel.key[index + 6], " ", this.LoginModel.key[index + 7] }), LUtil.LogType.Normal, false);
        uint num2 = message.ReadUInt32();
        Debug.Log(string.Concat(new object[] { "Gateway ip :", this.LoginModel.gateWayIp, "    port : ", this.LoginModel.gateWayPort }));
        LUtil.Log(string.Concat(new object[] { "Gateway ip :", this.LoginModel.gateWayIp, "    port : ", this.LoginModel.gateWayPort }), LUtil.LogType.Normal, false);
        this.ConnnectGateWay();
    }

    public void OnNotifyUserKickout(UMessage msg)
    {
        MSG_Ret_NotifyUserKickout_SC t_sc = msg.ReadProto<MSG_Ret_NotifyUserKickout_SC>();
        Debug.LogError("账号在其他地点登陆，被踢下线！");
    }

    private void onRequestIPOK(UMessage message)
    {
        this.clientIP = message.ReadString(0x10);
        this.szKeyIP = Encoding.UTF8.GetBytes(this.clientIP);
        LUtil.Log("Return Request Client IP: " + this.clientIP, LUtil.LogType.Normal, false);
        this.SendFirLogin();
    }

    private void OnReturnUserInfo(UMessage message)
    {
        Debug.Log("返回角色信息！");
        LSingleton<LoginModule>.Instance.uiLogin.RefreshRoleInfo(message);
        LSingleton<LoginModule>.Instance.uiLogin.onClickEnterGame();
    }

    public void OnServerLoginFailed(UMessage msg)
    {
        int returncode = (int) msg.ReadProto<MSG_Ret_ServerLoginFailed_SC>().returncode;
        string str = string.Empty;
        if (this.loginErrorDict.ContainsKey(returncode))
        {
            str = this.loginErrorDict[returncode];
            if (returncode == 6)
            {
                LSingleton<LoginModule>.Instance.Login();
            }
        }
        else
        {
            str = "未处理错误";
        }
        Debug.Log(returncode + ">>>" + str);
    }

    private void OnUserMapInfo(UMessage msg)
    {
        Debug.Log("OnUserMapInfo");
        LSingleton<GameManager>.Instance.CurrentMapData = LSingleton<ConfigManager>.Instance.mapConfig.GetMapDataByID(0x25a);
    }

    public void ParseFLEndpoint(string str)
    {
        if (str != null)
        {
            this.FLIPEndPoints.Clear();
            this.ConnectFlTimes = 0;
            char[] separator = new char[] { ';' };
            string[] strArray = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            char[] chArray2 = new char[] { ',' };
            string[] strArray2 = strArray[0].Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
            char[] chArray3 = new char[] { ',' };
            string[] strArray3 = strArray[1].Split(chArray3, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray2.Length; i++)
            {
                IPAddress address = IPAddress.Parse(strArray2[i].Trim());
                int num2 = int.Parse(strArray3[i]);
                IPEndPoint item = new IPEndPoint(address, num2 % 0x10000);
                this.FLIPEndPoints.Add(item);
            }
            this.CurrentIpEndPoint = this.FLIPEndPoints[0];
        }
    }

    public void ParseFLEndpoint(string addr, string port)
    {
        if (addr != null)
        {
            this.FLIPEndPoints.Clear();
            this.ConnectFlTimes = 0;
            char[] separator = new char[] { ':' };
            string[] strArray = addr.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            char[] chArray2 = new char[] { ':' };
            string[] strArray2 = port.Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray.Length; i++)
            {
                IPAddress address = IPAddress.Parse(strArray[i].Trim());
                int num2 = int.Parse(strArray2[i]);
                IPEndPoint item = new IPEndPoint(address, num2 % 0x10000);
                this.FLIPEndPoints.Add(item);
            }
            this.CurrentIpEndPoint = this.FLIPEndPoints[0];
        }
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
                            LUtil.Log(string.Concat(new object[] { "name: ", item.name, " zone:", item.id, " ip:", item.ip, " port:", item.port }));
                            this.SrvLists.Add(item);
                        }

                    }
                    LoginModule.Instance.uiLogin.RefreshSrvlist();
                }
                else if (child.Tag == "version")
                {
                    m_gameVersion = uint.Parse(child.Attribute("ver"));
                    LUtil.Log("@@CurGame Version: " + m_gameVersion);
                }

            }
        }
    }

    //public void ParseSrvlist(string str)
    //{
    //    if (str != null)
    //    {
    //        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
    //        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
    //        SecurityParser parser = new SecurityParser();
    //        parser.LoadXml(reader.ReadToEnd());
    //        IEnumerator enumerator = parser.ToXml().Children.GetEnumerator();
    //        try
    //        {
    //            while (enumerator.MoveNext())
    //            {
    //                SecurityElement current = (SecurityElement) enumerator.Current;
    //                if (current.Tag == "zones")
    //                {
    //                    IEnumerator enumerator2 = current.Children.GetEnumerator();
    //                    try
    //                    {
    //                        while (enumerator2.MoveNext())
    //                        {
    //                            SecurityElement element3 = (SecurityElement) enumerator2.Current;
    //                            if (element3.Tag == "zone")
    //                            {
    //                                ServerInfo item = new ServerInfo {
    //                                    name = element3.Attribute("name"),
    //                                    id = int.Parse(element3.Attribute("id")),
    //                                    ip = element3.Attribute("addr"),
    //                                    port = element3.Attribute("port")
    //                                };
    //                                LUtil.Log(string.Concat(new object[] { "name: ", item.name, " zone:", item.id, " ip:", item.ip, " port:", item.port }), LUtil.LogType.Normal, false);
    //                                this.SrvLists.Add(item);
    //                            }
    //                        }
    //                    }
    //                    finally
    //                    {
    //                        IDisposable disposable = enumerator2 as IDisposable;
    //                        if (disposable == null)
    //                        {
    //                        }
    //                        disposable.Dispose();
    //                    }
    //                    LSingleton<LoginModule>.Instance.uiLogin.RefreshSrvlist();
    //                }
    //                else if (current.Tag == "version")
    //                {
    //                    m_gameVersion = uint.Parse(current.Attribute("ver"));
    //                    LUtil.Log("@@CurGameVersion: " + m_gameVersion, LUtil.LogType.Normal, false);
    //                }
    //            }
    //        }
    //        finally
    //        {
    //            IDisposable disposable2 = enumerator as IDisposable;
    //            if (disposable2 == null)
    //            {
    //            }
    //            disposable2.Dispose();
    //        }
    //    }
    //}

    public override void RegisterMsg()
    {
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(0x68, 3), new OnMessageCallback(this.OnLoginFirError));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(0x68, 4), new OnMessageCallback(this.onLoginFirSuccess));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(0x68, 0x10), new OnMessageCallback(this.onRequestIPOK));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(3, 0x35), new OnMessageCallback(this.OnGateWayMergeVersionInfo));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(0x36, 1), new OnMessageCallback(this.OnBattleStateUserCmd));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(0x18, 1), new OnMessageCallback(this.OnReturnUserInfo));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(3, 0x1c), new OnMessageCallback(this.OnUserMapInfo));
    }

    private void RequestClientIP()
    {
        UMessage msg = new UMessage();
        msg.WriteHead(0x68, 15);
        SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
    }

    public static void ResetSendRevCount()
    {
        m_iLenSended = 0;
        m_iLenRecved = 0;
    }

    private void SendFirCheckVersion()
    {
        UMessage msg = new UMessage();
        msg.WriteHead(0x68, 120);
        msg.WriteUInt32(0);
        msg.WriteUInt32(m_gameVersion);
        SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
    }

    private void SendFirLogin()
    {
        if (this.loginType == LoginType.UUID)
        {
            UMessage msg = new UMessage();
            msg.WriteHead(0x68, 2);
            msg.WriteString(this.account, 0x30);
            byte num = LSingleton<LoginModule>.Instance.Pass[0];
            byte[] pszSrc = new byte[0x22];
            for (int i = 0; i < (num + 1); i++)
            {
                pszSrc[i] = LSingleton<LoginModule>.Instance.Pass[i];
            }
            this.UseIPEncry(ref pszSrc, num + 1);
            msg.WriteBytes(pszSrc, 0x21);
            msg.WriteUInt16(1);
            msg.WriteUInt16(this.zone);
            msg.WriteString(string.Empty, 7);
            msg.WriteString("74D02BC4CE2B");
            msg.WriteString("))L1?o1?L1????z/ 2?", 0x19);
            msg.WriteUInt16(0);
            msg.WriteString(string.Empty, 9);
            SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
        }
    }

    private void SendGateWayCheckVersion()
    {
        Debug.Log("SendGateWayCheckVersion");
        UMessage msg = new UMessage();
        msg.WriteHead(0x68, 120);
        msg.WriteUInt32(0);
        msg.WriteUInt32(m_gameVersion);
        SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
    }

    private void SendGateWayLogin()
    {
        Debug.Log("SendGateWayLogin");
        UMessage msg = new UMessage();
        msg.WriteHead(0x68, 5);
        msg.WriteUInt32(this.LoginModel.gatewayTempId);
        msg.WriteUInt32(this.LoginModel.gatewayUserId);
        msg.WriteString(this.account, 0x30);
        msg.WriteString(string.Empty, 0x10);
        SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
        USocket.m_bLoginSend = false;
    }

    public void SendRegisterPlayer(string name, byte sex, uint professional)
    {
        UMessage dat = new UMessage();
        dat.WriteHead(0x7fa);
        dat.WriteString(name, 0x20);
        dat.WriteByte(sex);
        dat.WriteUInt32(10);
        m_phoneinfo.WriteCmd(dat);
        dat.WriteUInt32(professional);
        dat.WriteUInt32(this.LoginModel.gatewayUserId);
        base.SendMsg(dat, false);
    }

    private void UseIPEncry(ref byte[] pszSrc, int iNum)
    {
        byte length = (byte) this.clientIP.Length;
        byte index = 0;
        for (int i = 0; i < iNum; i++)
        {
            pszSrc[i] = (byte) (pszSrc[i] ^ this.szKeyIP[index]);
            pszSrc[i] = (byte) (pszSrc[i] + 1);
            if ((index = (byte) (index + 1)) >= length)
            {
                index = 0;
            }
        }
    }
}

