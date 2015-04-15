using System;
using System.Text;
using Net;
using UnityEngine;

public class LoginModule : LSingleton<LoginModule>
{
    public string Account;
    public string FLIP;
    public int FLPort;
    public bool isShowLoginUI;
    public bool isShowRegisterUI;
    public LoginNetWork LoginNetWork;
    public string NickName;
    public byte[] Pass;
    public byte Sex;
    public UI_Login uiLogin;
    public int Zone;

    public void ChangeToFightingScene()
    {
        SingletonForMono<NetWorkModule>.Instance.BeginCacheMsg();
        SingletonForMono<SceneManager>.Instance.ChangeScene(LSingleton<GameManager>.Instance.CurrentMapData.FileName, new SceneManager.ChangeSceneOver(this.OnChangeSceneOver));
    }

    public void CreatMainPlayer()
    {
        if (LSingleton<GameManager>.Instance.MainPlayer == null)
        {
            MainPlayer player = new MainPlayer();
            LSingleton<GameManager>.Instance.MainPlayer = player;
            Debug.Log("Creat MainPlayer");
        }
    }

    public void Encrypt(string s)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        this.Pass = new byte[(bytes.Length * 2) + 1];
        int index = 1;
        for (int i = 0; i < bytes.Length; i++)
        {
            this.EncryptChar(ref this.Pass, index, bytes[i]);
            index += 2;
        }
        this.Pass[0] = (byte) (bytes.Length * 2);
    }

    public void EncryptChar(ref byte[] pszDes, int index, byte pszSrc)
    {
        byte num4 = 1;
        byte[] buffer = new byte[] { 210, 0x29, 0xb6, 0x8d, 14, 0xf2, 120, 0xb2 };
        pszSrc = (byte) (pszSrc + 2);
        pszDes[index] = pszSrc;
        byte num = (byte) (pszDes[index + 1] >> 4);
        num = (byte) (num & 15);
        byte num2 = (byte) (pszDes[index] << 4);
        num2 = (byte) (num2 & 240);
        num = (byte) (num | num2);
        num2 = (byte) (pszDes[index] >> 4);
        num2 = (byte) (num2 & 15);
        byte num3 = (byte) (pszDes[index + 1] << 4);
        num3 = (byte) (num3 & 240);
        pszDes[index] = (byte) (num2 | num3);
        pszDes[index + 1] = num;
        pszDes[index] = (byte) (pszDes[index] ^ buffer[num4]);
        pszDes[index + 1] = (byte) (pszDes[index + 1] ^ buffer[num4]);
        num2 = (byte) (pszDes[index] & 15);
        num3 = (byte) (num4 << 4);
        num3 = (byte) (num3 & 240);
        pszDes[index] = (byte) (num2 | num3);
    }

    private int getCharCount(string str)
    {
        int num = 0;
        for (int i = 0; i < str.Length; i++)
        {
            char ch = str[i];
            if ((ch >= 'a') && (ch <= 'z'))
            {
                num++;
            }
            else if ((ch >= 'A') && (ch <= 'Z'))
            {
                num++;
            }
            else if ((ch >= '0') && (ch <= '9'))
            {
                num++;
            }
            else
            {
                num += 2;
            }
        }
        return num;
    }

    public void Init()
    {
        this.uiLogin = new UI_Login();
        this.uiLogin.Init();
        this.uiLogin.ShowLoginUI();
        this.LoginNetWork = new LoginNetWork();
        this.LoginNetWork.Initialize();
    }

    public void Login()
    {
        this.LoginNetWork.ConnectFirServer(this.LoginNetWork.CurrentIpEndPoint, LoginType.UUID, (ushort) PlayerPrefs.GetInt("Zone"), "111111", 1, "0", 0, this.Account);
    }

    public void Login(string account, ushort zone)
    {
        this.LoginNetWork.ConnectFirServer(this.LoginNetWork.CurrentIpEndPoint, LoginType.UUID, zone, "1", 0x2a, "0", 0, account);
    }

    public void OnChangeSceneOver(string scene)
    {
        LSingleton<ChatModule>.Instance.Initialize();
        SingletonForMono<NetWorkModule>.Instance.FinishCacheMsg();
    }

    public void RegisterPlayer(string name, byte sex, uint pro)
    {
        if (name == string.Empty)
        {
            Debug.LogError("昵称不能为空");
        }
        else if (this.getCharCount(name) > 14)
        {
            Debug.LogError("昵称不能超过14个英文字符或7个汉字");
        }
        else
        {
            KeyWordFilter.InitFilter();
            if (string.Compare(name, KeyWordFilter.TextFilter(name)) != 0)
            {
                Debug.LogError("人物名字有敏感字符");
            }
            else
            {
                this.LoginNetWork.SendRegisterPlayer(name, sex, pro);
            }
        }
    }
}

