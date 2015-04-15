using Net;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Login
{
    private UIInput accountInput;
    public const int cellhight = 50;
    public const int cellwidth = 150;
    private int defaultServer;
    public int defaultSrvIndex;
    private UIButton entergameButton;
    private UIButton loginButton;
    private GameObject LoginUI;
    private UIInput nameInput;
    private UIInput passwordInput;
    private GameObject ResigterUI;
    private GameObject serverParentObject;
    private UIToggle sex1;
    private UIToggle sex2;
    public const int srvCountPerLing = 8;
    private List<UIToggle> SrvToggleLists = new List<UIToggle>();
    private UIToggle tgpro1;
    private UIToggle tgpro2;
    public Transform uiRoot;
    public const int xoffset = -540;
    public const int yoffset = 0x13e;

    public void Init()
    {
        this.uiRoot = GameObject.Find("UIRoot").transform;
        this.LoginUI = this.uiRoot.FindChild("Camera/Login").gameObject;
        this.ResigterUI = this.uiRoot.FindChild("Camera/Register").gameObject;
        this.LoginUI.SetActive(false);
        this.ResigterUI.SetActive(false);
        this.serverParentObject = this.uiRoot.FindChild("Camera/Login/Servers").gameObject;
        this.accountInput = this.uiRoot.FindChild("Camera/Login/Input").GetComponent<UIInput>();
        this.passwordInput = this.uiRoot.FindChild("Camera/Login/Pass").GetComponent<UIInput>();
        this.loginButton = this.uiRoot.FindChild("Camera/Login/Login").GetComponent<UIButton>();
        //this.tgpro1 = this.uiRoot.FindChild("Camera/Register/Profession/GL").GetComponent<UIToggle>();
        //this.tgpro2 = this.uiRoot.FindChild("Camera/Register/Profession/JW").GetComponent<UIToggle>();
        //this.sex1 = this.uiRoot.FindChild("Camera/Register/Sex/male").GetComponent<UIToggle>();
        //this.sex2 = this.uiRoot.FindChild("Camera/Register/Sex/female").GetComponent<UIToggle>();
        this.nameInput = this.uiRoot.FindChild("Camera/Register/Name").GetComponent<UIInput>();
        this.entergameButton = this.uiRoot.FindChild("Camera/Register/EnterGame").GetComponent<UIButton>();
        EventDelegate.Add(this.loginButton.onClick, new EventDelegate.Callback(this.onClickLogin));
        EventDelegate.Add(this.entergameButton.onClick, new EventDelegate.Callback(this.onClickEnterGame));
        string str = PlayerPrefs.GetString("Account");
        if (str != null)
        {
            this.accountInput.value = str;
        }
    }

    public void onClickEnterGame()
    {
        Debug.Log("进入游戏！");
        LUtil.Log("进入游戏！", LUtil.LogType.Normal, false);
        UMessage msg = new UMessage();
        msg.WriteHead(0x18, 3);
        msg.WriteUInt32(0);
        msg.WriteUInt32(0);
        msg.WriteUInt32(0);
        msg.WriteUInt32(0);
        SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
    }

    public void RefreshRoleInfo(UMessage msg)
    {
        stUserInfoUserCmd st = new stUserInfoUserCmd();
        msg.ReadStruct<stUserInfoUserCmd>(st);
        LUtil.Log("@@@@名字：  " + st.name);
        LUtil.Log("@@@@名字：  " + st.name);
        LUtil.Log("@@@@名字：  " + st.name);
        LUtil.Log("@@@@名字：  " + st.name);
    }

    private void onClickLogin()
    {
        if (this.accountInput.value != string.Empty)
        {
            string str = this.accountInput.value;
            PlayerPrefs.SetString("Account", str);
            LSingleton<LoginModule>.Instance.Account = str;
            string s = this.passwordInput.value;
            LSingleton<LoginModule>.Instance.Encrypt(s);
            int id = 0;
            int num2 = 0;
            bool flag = false;
            foreach (UIToggle toggle in this.SrvToggleLists)
            {
                if (toggle.value)
                {
                    ServerInfo info = LSingleton<LoginModule>.Instance.LoginNetWork.SrvLists[num2];
                    id = info.id;
                    PlayerPrefs.SetInt("DefaultServer", info.id);
                    flag = true;
                    LSingleton<LoginModule>.Instance.LoginNetWork.ParseFLEndpoint(info.ip, info.port);
                    break;
                }
                num2++;
            }
            if (!flag)
            {
                ServerInfo info2 = LSingleton<LoginModule>.Instance.LoginNetWork.SrvLists[this.defaultSrvIndex];
                id = info2.id;
                PlayerPrefs.SetInt("DefaultServer", info2.id);
                LSingleton<LoginModule>.Instance.LoginNetWork.ParseFLEndpoint(info2.ip, info2.port);
            }
            PlayerPrefs.SetInt("Zone", id);
            LSingleton<LoginModule>.Instance.Login();
        }
    }

    public void RefreshSrvlist()
    {
        int count = LSingleton<LoginModule>.Instance.LoginNetWork.SrvLists.Count;
        int num2 = 0;
        int num3 = 0;
        int num4 = 0;
        int @int = PlayerPrefs.GetInt("DefaultServer");
        this.SrvToggleLists.Clear();
        foreach (ServerInfo info in LSingleton<LoginModule>.Instance.LoginNetWork.SrvLists)
        {
            GameObject obj2 = NGUITools.AddChild(this.serverParentObject, (GameObject) Resources.Load("Prefabs/server"));
            if (obj2 != null)
            {
                UILabel component = obj2.transform.FindChild("Label").GetComponent<UILabel>();
                if (component != null)
                {
                    int id = info.id;
                    component.text = info.name;
                }
                num2 = num4 / 3;
                num3 = num4 % 3;
                Vector3 vector = new Vector3((float) (-390 + (num3 * 150)), (float) (219 - (num2 * 50)), obj2.transform.position.z);
                obj2.transform.localPosition = vector;
                UIToggle item = obj2.GetComponent<UIToggle>();
                this.SrvToggleLists.Add(item);
                if (info.id == @int)
                {
                    this.SrvToggleLists[num4].active = true;
                    this.defaultSrvIndex = num4;
                }
                num4++;
            }
        }
    }

    public void ShowLoginUI()
    {
        this.LoginUI.SetActive(true);
        this.ResigterUI.SetActive(false);
    }

    public void ShowResisterUI()
    {
        this.LoginUI.SetActive(false);
        this.ResigterUI.SetActive(true);
        nameInput.value = LoginModule.Instance.Account;
    }
}

