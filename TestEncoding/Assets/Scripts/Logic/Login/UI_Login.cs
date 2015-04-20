using UnityEngine;
using System.Collections;
using Net;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using GBKEncoding;

public class UI_Login
{
    public const int AnnounceCount = 1;
    //公告
    private UILabel[] lblAnnouncements = new UILabel[AnnounceCount];
    private List<string> strAnnoucements = new List<string>();
    private List<Color> colorAnnoucements = new List<Color>();
    private Transform uiroot;
    public enum enumRoleSel
    {
	    eRoleSel_ONE = 0,
	    eRoleSel_TWO = 1,
	    eRoleSel_NULL = 2,
    };

    //保存最近账号最大数量
    public const int MAX_RECENT_ACCOUNT_SIZE = 5;
    //保存最近网通区最大数量
    public const int MAX_RECENT_UNIZONE_SIZE = 2;
    //保存最近电信区最大数量
    public const int MAX_RECENT_TELZONE_SIZE = 2;

    public Transform uiRoot;

    private GameObject LoginUI;
    private GameObject ResigterUI;

    //private UIToggle tgServer1;
    //private UIToggle tgServer2;
    //private UIToggle tgServer3;

    private List<UIToggle> SrvToggleLists = new List<UIToggle>();
    private List<UIToggle> RoleToggleLists = new List<UIToggle>();
    private GameObject serverParentObject;

    private GameObject serverParentTelecom;
    private UIGrid telecomGrid;
    private GameObject serverParentNetcom;
    private UIGrid netcomGrid;

    private UIInput accountInput;
    private UIInput passwordInput;
    private UIButton loginButton;

    private UIButton tabTelecomBtn;
    private UIButton tabUnicomBtn;

    private UIButton curZoneBtn;
    private UILabel curZoneLbl;

    private UIToggle tgpro1;
    private UIToggle tgpro2;

    private UIToggle sex1;
    private UIToggle sex2;

    private UIInput nameInput;
    private UIButton entergameButton;

    private int defaultServer = 0;

    //每个服务器标签占用多少宽度
    public const int cellwidth = 150;
    //每个服务器标签占用多少高度
    public const int cellhight = 50;
    //每行多少个服务器标签
    public const int srvCountPerLing = 8;
    //水平偏移
    public const int xoffset = -540;
    //竖直偏移
    public const int yoffset = 318;

    public int defaultSrvIndex = 0;

    public List<Role> roles = new List<Role>();
    public enumRoleSel curRoleSel;     //当前选择人物位置

    //显示最近登入的账号
    private UIPopupList mList = null;

    //2个以上角色中大的角色等级和索引
    private UInt32 biggerCharacterLevel = 0;
    private UInt32 biggerCharacterLevelIndex = 0;

    //最近登入的账号
    private List<string> recentAccounts = new List<string>();
    //最近登入区-电信
    private List<int> recentTelecomZones = new List<int>();
    //最近登入区-网通
    private List<int> recentUnicomZones = new List<int>();

	// Use this for initialization
	public void Init ()
	{
        uiRoot = GameObject.Find("UI_Root").transform;

        LoginUI = uiRoot.FindChild("Camera/Login").gameObject;
        ResigterUI = uiRoot.FindChild("Camera/Register").gameObject;

        LoginUI.SetActive(false);
        ResigterUI.SetActive(false);

        //tgServer1 = uiRoot.FindChild("Camera/Login/Servers/LX").GetComponent<UIToggle>();
        //tgServer2 = uiRoot.FindChild("Camera/Login/Servers/XQ").GetComponent<UIToggle>();
        //tgServer3 = uiRoot.FindChild("Camera/Login/Servers/OUT").GetComponent<UIToggle>();
        

        serverParentObject = uiRoot.FindChild("Camera/Login/Servers").gameObject;

        serverParentTelecom = uiRoot.FindChild("Camera/Login/Servers/TelecomServers/Scroll View/UIGrid").gameObject;
        telecomGrid = serverParentTelecom.GetComponent<UIGrid>();
        serverParentNetcom = uiRoot.FindChild("Camera/Login/Servers/NetcomServers/Scroll View/UIGrid").gameObject;
        netcomGrid = serverParentNetcom.GetComponent<UIGrid>();

        accountInput = uiRoot.FindChild("Camera/Login/Input").GetComponent<UIInput>();
        passwordInput = uiRoot.FindChild("Camera/Login/Pass").GetComponent<UIInput>();
        loginButton = uiRoot.FindChild("Camera/Login/Login").GetComponent<UIButton>();

        tabTelecomBtn = uiRoot.FindChild("Camera/Login/Servers/tabTelecom").GetComponent<UIButton>();
        tabUnicomBtn = uiRoot.FindChild("Camera/Login/Servers/tabNetcom").GetComponent<UIButton>();;

        curZoneBtn = uiRoot.FindChild("Camera/Login/CurZone").GetComponent<UIButton>();
        curZoneLbl = uiRoot.FindChild("Camera/Login/CurZone/Label").GetComponent<UILabel>(); 

        tgpro1 = uiRoot.FindChild("Camera/Register/Profession/GL").GetComponent<UIToggle>();
        tgpro2 = uiRoot.FindChild("Camera/Register/Profession/JW").GetComponent<UIToggle>();

        sex1 = uiRoot.FindChild("Camera/Register/Sex/male").GetComponent<UIToggle>();
        sex2 = uiRoot.FindChild("Camera/Register/Sex/female").GetComponent<UIToggle>();

        nameInput = uiRoot.FindChild("Camera/Register/Name").GetComponent<UIInput>();
        entergameButton = uiRoot.FindChild("Camera/Register/EnterGame").GetComponent<UIButton>();


	    EventDelegate.Add(loginButton.onClick, onClickLogin);
        EventDelegate.Add(entergameButton.onClick, onClickEnterGame);

        EventDelegate.Add(tabTelecomBtn.onClick, onClickTelecomBtn);
        EventDelegate.Add(tabUnicomBtn.onClick, onClickUnicomBtn);

        EventDelegate.Add(curZoneBtn.onClick, onClickCurZoneBtn);

        string acc = PlayerPrefs.GetString("Account");
        if (acc != null)
        {
            accountInput.value = acc;
        }
        //读取最近账号列表
        ReadRecentAccount();
        GameObject RecentAccObj = uiRoot.FindChild("Camera/Login/RecentAccountBtn").gameObject;
        if (RecentAccObj != null)
        {
            mList = RecentAccObj.GetComponent<UIPopupList>();
            if (mList != null)
            {
                for (int i = 0; i < recentAccounts.Count; ++i)
                {
                    mList.items.Add(recentAccounts[i]);
                }
                if (recentAccounts.Count >= 1)
                    mList.selection = mList.items[recentAccounts.Count-1];

                EventDelegate.Add(mList.onChange, OnAccountSelection);
            }
        }

       // roles = new Role[GlobalVar.MAX_CHARINFO];   //申请2个角色数据
        roles.Add(new Role());
        roles.Add(new Role());
        curRoleSel = enumRoleSel.eRoleSel_NULL;


#if UNITY_IPHONE || UNITY_ANDROID
        //tgServer3.value = true;
        //PlayerPrefs.SetInt("DefaultServer", 3);
#else
        int server = PlayerPrefs.GetInt("DefaultServer");
        //if (server == 1)
        //{
        //    tgServer1.value = true;
        //}
        //if (server == 2)
        //{
        //    tgServer2.value = true;
        //}
        //if (server == 3)
        //{
        //    tgServer3.value = true;
        //}
#endif

        uiroot = GameObject.Find("UI_Root").transform;
        //公告
        for (int i = 0; i < AnnounceCount; ++i)
        {
            lblAnnouncements[i] = uiroot.FindChild("Camera/Announcement/Label" + (i + 1).ToString()).GetComponent<UILabel>();
        }
	}

    public void AddGreenTip(string tip)
    {
        AddTip(tip, Color.green);
    }

    public void AddYellowTip(string tip)
    {
        AddTip(tip, Color.yellow);
    }

    public void AddRedTip(string tip)
    {
        AddTip(tip, Color.red);
    }
    public void AddTip(string tip, Color colour)
    {
        AddAnnounceMent(tip, colour);
    }
    public void AddAnnounceMent(string tip, Color colour)
    {
        if (strAnnoucements.Count >= AnnounceCount)
        {
            strAnnoucements.RemoveAt(0);
        }
        strAnnoucements.Add(tip);

        if (colorAnnoucements.Count >= AnnounceCount)
        {
            colorAnnoucements.RemoveAt(0);
        }
        colorAnnoucements.Add(colour);

        RefreshAnnounceMent();
    }

    public void RefreshAnnounceMent()
    {
        for (int i = 0; i < strAnnoucements.Count; ++i)
        {
            int index = strAnnoucements.Count - i - 1;
            lblAnnouncements[i].color = colorAnnoucements[index];
            lblAnnouncements[i].text = strAnnoucements[index];
            TweenAlpha ta = lblAnnouncements[i].GetComponent<TweenAlpha>();
            if (ta != null)
            {
                ta.enabled = true;
                ta.ResetToBeginning();
            }
        }
    }

    void OnAccountSelection()
    {
        accountInput.value = mList.selection;
    }

    public void RefreshSrvlist()
    {
        int srvcount = LoginModule.Instance.LoginNetWork.SrvLists.Count;
        int i = 0;
        int j = 0;
        int index = 0;
        int defaultServer = PlayerPrefs.GetInt("DefaultServer");

        //读取最近登陆的区
        ReadRecentZone();
        //处理区列表，把最近登陆的区插再前面
        ProcessServerList();

        SrvToggleLists.Clear();

        foreach(ServerInfo srvinfo in LoginModule.Instance.LoginNetWork.SrvLists)
        {
            //GameObject server = NGUITools.AddChild(serverParentObject, (GameObject)(Resources.Load("Prefabs/server")));
            GameObject server = null;
            //电信
            if (srvinfo.zone == 0)
            {
                server = NGUITools.AddChild(serverParentTelecom, (GameObject)(Resources.Load("Prefabs/server")));
            }
            else //网通
            {
                server = NGUITools.AddChild(serverParentNetcom, (GameObject)(Resources.Load("Prefabs/server")));
            }
            if (server != null)
            {
                UIEventListener.Get(server.gameObject).onClick = onZoneSelect;
                UILabel lbl = server.transform.FindChild("Label").GetComponent<UILabel>();
                if (lbl != null)
                {
                    int id = srvinfo.id;
                    lbl.text = srvinfo.name;
                }
                //i = index / srvCountPerLing;
                //j = index % srvCountPerLing;

                //Vector3 pos = new Vector3(xoffset + j * cellwidth, yoffset - i * cellhight, server.transform.position.z);
                //server.transform.localPosition = pos;

                UIToggle toggle = server.GetComponent<UIToggle>();
                SrvToggleLists.Add(toggle);
                //设置默认服务器
                if (srvinfo.id == defaultServer)
                {
                    //SrvToggleLists[index].active = true;
                    //SrvToggleLists[index].startsActive = true;
                    defaultSrvIndex = index;
                    if (curZoneLbl != null)
                    {
                        curZoneLbl.text = srvinfo.name;
                    }
                }

                index++;

            }
        }

        netcomGrid.Reposition();
        telecomGrid.Reposition();
    }

    public ushort GetTypeFromHead(ushort head)
    {
        if (head == ushort.MaxValue + 1)
        {
            return (ushort)enumProfession.PROFESSION_NONE;
        }
        return (head % 2) == 0 ? (ushort)enumProfession.PROFESSION_2 : (ushort)enumProfession.PROFESSION_1; 
    }

    public void RefreshRoleInfo(UMessage msg)
    {
        //UInt32 id = msg.ReadUInt32();
        //byte[] name = msg.ReadBytes(33);
        //string tmpName = GBKEncoder.Read(name);

        stUserInfoUserCmd st = new stUserInfoUserCmd();
        msg.ReadStruct<stUserInfoUserCmd>(st);
        GameManager.Instance.MainPlayer.selectUserInfo = st;
        //选择等级高的角色登陆
        if (st.level2 > st.level)
            biggerCharacterLevelIndex = 1;

        //for (int i = 0; i < GlobalVar.MAX_CHARINFO; ++i)
        //{
        //    int msgBodyLength = msg.BodyLength;
        //   //stUserInfoUserCmd cmd = new stUserInfoUserCmd();
        //    SelectUserInfo userinfo = new SelectUserInfo();
        //    int length = Marshal.SizeOf(userinfo);
        //    byte[] bytes = msg.ReadBytes(length);
        //    userinfo = (SelectUserInfo)Converter.BytesToStruct(bytes, userinfo.GetType());
        //    string thisname = GBKEncoder.Read(bytes, 4, 33);
        //    if ( userinfo.name != null)
        //    {
        //        if (roles[i] != null)
        //        {
        //            roles[i].name = userinfo.name;
        //            roles[i].type = GetTypeFromHead(userinfo.type);
        //            roles[i].conntryId = ushort.MaxValue;
        //            roles[i].countryName = userinfo.countryName != null ? userinfo.countryName : "";
        //            roles[i].level = userinfo.level;
        //            roles[i].state = enumRoleState.eRoleState_DONE;
        //            if (userinfo.level > biggerCharacterLevel)
        //            {
        //                biggerCharacterLevel = (UInt32)userinfo.level;
        //                biggerCharacterLevelIndex = (UInt32)i;
        //            }
        //        }
        //        else
        //        {

        //            roles[i].state = enumRoleState.eRoleState_NULL;
        //        }
        //    }
        //}

        //刷新角色选择界面
        //UpdateSelectRole();
    }

    public void UpdateSelectRole()
    {
        int number = 0;
        if( sex1 != null )
        {
            UILabel lbl = sex1.transform.FindChild("Label").GetComponent<UILabel>();
            if (lbl != null )
            {
                lbl.text = "名称：" + roles[number].name + "国家：" + roles[number].countryName + " 等级：" + roles[number].level;
            }

            //设置默认角色
            //sex1.active = true;
            sex1.startsActive = true;
            curRoleSel = enumRoleSel.eRoleSel_ONE;
            ++number;
        }

        if( sex2 != null )
        {
            if (roles[number].name != null && roles[number].name.Length != 0)
            {
                UILabel lbl = sex2.transform.FindChild("Label").GetComponent<UILabel>();
                if (lbl != null)
                {
                    lbl.text = "名称：" + roles[number].name + "国家：" + roles[number].countryName + "等级：" + roles[number].level;
                }
            }
            else
                sex2.active = false;
           // sex2.active = false;
            ++number;
        }
    }

    void onClickLogin()
    {
        if (!Root.canClickLogin)
        {
            AddRedTip("请更新完毕再登陆！");
            return;
        }

        if (accountInput.value != "")
        {
            string account = accountInput.value;
            PlayerPrefs.SetString("Account", account);   //账号写入存档
            LoginModule.Instance.Account = account; //账号写入内存
            //保存最近账号信息
            SaveRecentAccount(account);

            string pass = passwordInput.value;
            LoginModule.Instance.Encrypt(pass);

            int zone = 0;

            int index = 0;

            bool haveSelectServer = false;
            foreach (UIToggle toggle in SrvToggleLists)
            {
                if (toggle.value)
                {
                    ServerInfo srvinfo= LoginModule.Instance.LoginNetWork.SrvLists[index];
                    zone = srvinfo.id;
                    
                    PlayerPrefs.SetInt("DefaultServer", srvinfo.id);
                    SaveRecentZone(srvinfo.id, srvinfo.zone);
                    haveSelectServer = true;
                    LoginModule.Instance.LoginNetWork.ParseFLEndpoint(srvinfo.ip, srvinfo.port);

                    break;
                }
                index++;
            }

            if (!haveSelectServer)
            {
                ServerInfo srvinfo = LoginModule.Instance.LoginNetWork.SrvLists[defaultSrvIndex];
                zone = srvinfo.id;
                PlayerPrefs.SetInt("DefaultServer", srvinfo.id);
                SaveRecentZone(srvinfo.id, srvinfo.zone);
                LoginModule.Instance.LoginNetWork.ParseFLEndpoint(srvinfo.ip, srvinfo.port);
            }
            //if (tgServer1.value)
            //{
            //    zone = 797;
            //    //LoginModule.Instance.FLIP = "192.168.172.55";
            //    //LoginModule.Instance.FLPort = 1500;
            //    PlayerPrefs.SetInt("DefaultServer", 1);
            //}
            //if (tgServer2.value)
            //{
            //    zone = 2300;
            //    //LoginModule.Instance.FLIP = "192.168.172.55";
            //    //LoginModule.Instance.FLPort = 1500;
            //    PlayerPrefs.SetInt("DefaultServer", 2);
            //}
            //if (tgServer3.value)
            //{
            //    zone = 1354;
            //    //LoginModule.Instance.FLIP = "119.97.171.115";
            //    //LoginModule.Instance.FLPort = 2500;
            //    PlayerPrefs.SetInt("DefaultServer", 3);
            //}
            PlayerPrefs.SetInt("Zone", zone);


            LoginModule.Instance.Login();
        }
    }

    public void onClickEnterGame()
    {
        Util.Log("进入游戏！");

        /// 请求登陆
        //const BYTE LOGIN_SELECT_USERCMD_PARA = 3;
        //struct stLoginSelectUserCmd : public stSelectUserCmd
        //{
        //    DWORD charNo;
        //    int x1;
        //    int y1;
        //    int x2;
        //    int y2;
        //}
        UMessage message = new UMessage();
        //ushort uCurSel = 0;
        //int group = sex1.group;  //组别
        //UIToggle roleToggle = UIToggle.GetActiveToggle(group);
        //if ( object.ReferenceEquals(roleToggle,sex2) == true )
        //    uCurSel = 1;
        message.WriteHead(24, 3);
        message.WriteUInt32(biggerCharacterLevelIndex);
        message.WriteUInt32(0);
        message.WriteUInt32(0);
        message.WriteUInt32(0);
        message.WriteUInt32(0);
        NetWorkModule.Instance.SendImmediate(message);

        //byte sex = 0;

        //if (sex1.value)
        //{
        //    sex = 1;
        //}
        //if (sex2.value)
        //{
        //    sex = 2;
        //}

        //uint _pro = 0;
        //if (tgpro1.value)
        //{
        //    _pro = (uint)msg.Occupation.Occu_Dema;
        //}
        //if (tgpro2.value)
        //{
        //    _pro = (uint)msg.Occupation.Occu_Yaohu;
        //}

        //string name = LoginModule.Instance.Account;
        //if (name == null)
        //{
        //    name = nameInput.value;
        //}
        
        //LoginModule.Instance.RegisterPlayer(name, sex, _pro);
    }

     public void onClickTelecomBtn()
     {
         serverParentTelecom.SetActive(true);
         serverParentNetcom.SetActive(false);
     }
      
    public void onClickUnicomBtn()
    {
        serverParentTelecom.SetActive(false);
        serverParentNetcom.SetActive(true);
    }

    public void onClickCurZoneBtn()
    {
        if (serverParentObject != null)
        {
            serverParentObject.SetActive(!serverParentObject.active);
        }
    }

    public void onZoneSelect(GameObject server)
    {
        UILabel lbl = server.transform.FindChild("Label").GetComponent<UILabel>();
        if ((lbl != null) && (curZoneLbl != null))
        {
            curZoneLbl.text = lbl.text;
            serverParentObject.SetActive(false);
        }
    }

    public void ShowLoginUI()
    {
        LoginUI.SetActive(true);
        ResigterUI.SetActive(false);
    }

    public void ShowResisterUI()
    {
        nameInput.value = LoginModule.Instance.Account;
        LoginUI.SetActive(false);
        ResigterUI.SetActive(true);
    }

    public void ReadRecentAccount()
    {
        recentAccounts.Clear();
        for (int i = 0; i < MAX_RECENT_ACCOUNT_SIZE; ++i)
        {
            string account = PlayerPrefs.GetString("Account" + i.ToString(), "");
            if (account != "")
                recentAccounts.Add(account);
        }
    }

    public void SaveRecentAccount(string curAcccount)
    {
        //PlayerPrefs.SetString("Account", account);   //账号写入存档
        //MAX_RECENT_ACCOUNT_SIZE;
        for (int i=0; i<recentAccounts.Count; ++i)
        {
            //列表上有就不用额外保存
            if (curAcccount == recentAccounts[i])
                return;
        }
        //列表未满
        if (recentAccounts.Count < MAX_RECENT_ACCOUNT_SIZE)
        {
            
            int i = 0;
            for (i=0; i<recentAccounts.Count; ++i)
            {
                PlayerPrefs.SetString("Account" + i.ToString(), recentAccounts[i]);
            }
            PlayerPrefs.SetString("Account" + i.ToString(), curAcccount);
        }
        else //列表已满，替换掉第一个
        {
            for (int i=1; i<MAX_RECENT_ACCOUNT_SIZE; ++i)
            {
                PlayerPrefs.SetString("Account" + (i-1).ToString(), recentAccounts[i]);
            }
            PlayerPrefs.SetString("Account" + (MAX_RECENT_ACCOUNT_SIZE-1).ToString(), curAcccount);
        }
    }

    public void ReadRecentZone()
    {
        //电信
        recentTelecomZones.Clear();
        for (int i = 0; i < MAX_RECENT_TELZONE_SIZE; ++i)
        {
            int zoneID = PlayerPrefs.GetInt("TelecomZone" + i.ToString(), 0);
            if (zoneID != 0)
                recentTelecomZones.Add(zoneID);
        }
        //网通
        recentUnicomZones.Clear();
        for (int i=0; i<MAX_RECENT_UNIZONE_SIZE; ++i)
        {
            int zoneID = PlayerPrefs.GetInt("UnicomZone" + i.ToString(), 0);
            if (zoneID != 0)
                recentUnicomZones.Add(zoneID);
        }
    }
    /// <summary>
    /// 保存最近登入的区
    /// </summary>
    /// <param name="zoneID"></param>
    /// <param name="net">0：电信  1：网通</param>

    public void SaveRecentZone(int zoneID, int net)
    {
        if (net == 0)
        {
            for (int i = 0; i < recentTelecomZones.Count; ++i)
            {
                //列表上有就不用额外保存
                if (zoneID == recentTelecomZones[i])
                    return;
            }
            //列表未满
            if (recentTelecomZones.Count < MAX_RECENT_TELZONE_SIZE)
            {

                int i = 0;
                for (i = 0; i < recentTelecomZones.Count; ++i)
                {
                    PlayerPrefs.SetInt("TelecomZone" + i.ToString(), recentTelecomZones[i]);
                }
                PlayerPrefs.SetInt("TelecomZone" + i.ToString(), zoneID);
            }
            else //列表已满，替换掉第一个
            {
                for (int i = 1; i < MAX_RECENT_TELZONE_SIZE; ++i)
                {
                    PlayerPrefs.SetInt("TelecomZone" + (i - 1).ToString(), recentTelecomZones[i]);
                }
                PlayerPrefs.SetInt("TelecomZone" + (MAX_RECENT_TELZONE_SIZE - 1).ToString(), zoneID);
            }
        }
        else
        {
            for (int i = 0; i < recentUnicomZones.Count; ++i)
            {
                //列表上有就不用额外保存
                if (zoneID == recentUnicomZones[i])
                    return;
            }
            //列表未满
            if (recentUnicomZones.Count < MAX_RECENT_UNIZONE_SIZE)
            {

                int i = 0;
                for (i = 0; i < recentUnicomZones.Count; ++i)
                {
                    PlayerPrefs.SetInt("UnicomZone" + i.ToString(), recentUnicomZones[i]);
                }
                PlayerPrefs.SetInt("UnicomZone" + i.ToString(), zoneID);
            }
            else //列表已满，替换掉第一个
            {
                for (int i = 1; i < MAX_RECENT_UNIZONE_SIZE; ++i)
                {
                    PlayerPrefs.SetInt("UnicomZone" + (i - 1).ToString(), recentUnicomZones[i]);
                }
                PlayerPrefs.SetInt("UnicomZone" + (MAX_RECENT_UNIZONE_SIZE - 1).ToString(), zoneID);
            }
        }

    }

    public void ProcessServerList()
    {
        List<ServerInfo> telecomServerList = new List<ServerInfo>();
        List<ServerInfo> unicomServerList = new List<ServerInfo>();
        
        //电信
        foreach(int zoneid in recentTelecomZones)
        {
            foreach (ServerInfo srvinfo in LoginModule.Instance.LoginNetWork.SrvLists)
            {
                if ((srvinfo.id == zoneid) && (srvinfo.zone == 0))
                {
                    telecomServerList.Add(srvinfo);
                }
            }
        }

        //网通
        foreach (int zoneid in recentUnicomZones)
        {
            foreach (ServerInfo srvinfo in LoginModule.Instance.LoginNetWork.SrvLists)
            {
                if ((srvinfo.id == zoneid) && (srvinfo.zone == 1))
                {
                    unicomServerList.Add(srvinfo);
                }
            }
        }

        //插入前面
        foreach (ServerInfo srvinfo in telecomServerList)
        {
            LoginModule.Instance.LoginNetWork.SrvLists.Insert(0, srvinfo);
        }
        foreach (ServerInfo srvinfo in unicomServerList)
        {
            LoginModule.Instance.LoginNetWork.SrvLists.Insert(0, srvinfo);
        }

    }
}
