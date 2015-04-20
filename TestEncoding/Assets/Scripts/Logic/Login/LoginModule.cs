using UnityEngine;
using System.Collections;
using System;
using Net;
using System.Text;
public class LoginModule : LSingleton<LoginModule>
{
    public LoginNetWork LoginNetWork;
    
    public string NickName;
    public byte Sex;

    public bool isShowLoginUI = false;
    public bool isShowRegisterUI = false;

    public string Account;
    public byte[] Pass;

    public string FLIP;
    public int FLPort;

    public int Zone;

    public UI_Login uiLogin;

    public void Init()
    {
        uiLogin = new UI_Login();
        uiLogin.Init();
        uiLogin.ShowLoginUI();
        LoginNetWork = new LoginNetWork();
        LoginNetWork.Initialize();
    }

    /// <summary>
    /// 连接FLServer
    /// </summary>
    public void Login(string account,ushort zone)
    {
        //LoginNetWork.ConnectFirServer("192.168.172.55", 1500, LoginType.UUID, 16, "1", 42, "0", 0, PlayerPrefs.GetString("Account"));
        LoginNetWork.ConnectFirServer(LoginNetWork.CurrentIpEndPoint, LoginType.UUID, zone, "1", 42, "0", 0, account);
    }

    /// <summary>
    /// 连接FLServer
    /// </summary>
    public void Login()
    {
        //LoginNetWork.ConnectFirServer("192.168.172.55", 1500, LoginType.UUID, 16, "1", 42, "0", 0, PlayerPrefs.GetString("Account"));
        LoginNetWork.ConnectFirServer(LoginNetWork.CurrentIpEndPoint, LoginType.UUID, (ushort)PlayerPrefs.GetInt("Zone"), "111111", 1, "0", 0, Account);
    }

    public void Encrypt(string s)
    {
        //byte[] bytes = UMessage.gbk.GetBytes(s);
        byte[] bytes = Encoding.UTF8.GetBytes(s);
        Pass = new byte[bytes.Length*2+1];

        int index = 1;
        for (int i=0; i<bytes.Length; ++i)
        {
            EncryptChar(ref Pass, index, bytes[i]);
            index += 2;
        }

        Pass[0] = (byte)(bytes.Length * 2);
    }

    //加密一个字符
    public void EncryptChar(ref byte[] pszDes, int index, byte pszSrc)
    {
        byte btmp, btmp1, btmp2;

	    byte array = 1;	// 用四位表示，值不能大于32
        byte[] keyData = new byte[8] {210, 41, 182, 141, 14, 242, 120, 178};

	    pszSrc += 2;

        pszDes[index] = pszSrc;

        btmp = (byte)((int)pszDes[index + 1] >> 4);
	    btmp &= 0x0F;
	    btmp1 = (byte)((int)pszDes[index + 0] << 4);
	    btmp1 &= 0xF0;
	    btmp |= btmp1;
	    btmp1 = (byte)((int)pszDes[index + 0] >> 4);
	    btmp1 &= 0x0F;
	    btmp2 = (byte)((int)pszDes[index + 1] << 4);
	    btmp2 &= 0xF0;
	    btmp1 |= btmp2;

	    pszDes[index + 0] = btmp1;
        pszDes[index + 1] = btmp;

        pszDes[index + 0] ^= keyData[array];
        pszDes[index + 1] ^= keyData[array];

        btmp1 = (byte)((int)pszDes[index + 0] & 0x0F);
	    btmp2 = (byte)((int)array << 4);
	    btmp2 &= 0xF0;
	    btmp1 |= btmp2;
        pszDes[index + 0] = btmp1;
    }
    public void RegisterPlayer(string name, byte sex,uint pro)
    {
        if(name == "")
        {
            Debug.LogError("昵称不能为空");
            return;
        }

        if(getCharCount(name) > 14)
        {
            Debug.LogError("昵称不能超过14个英文字符或7个汉字");
            return;
        }
        KeyWordFilter.InitFilter();//初始化 屏蔽字
        if (string.Compare(name, KeyWordFilter.TextFilter(name)) != 0)
        {
            Debug.LogError("人物名字有敏感字符");
            return;
        }
            
        LoginNetWork.SendRegisterPlayer(name,sex,pro);
    }
    private int getCharCount(string str)
    {
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            if (c >= 'a' && c <= 'z')
            {
                count++;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                count++;
            }
            else if (c >= '0' && c <= '9')
            {
                count++;
            }
            else
            {
                count += 2;
            }
        }
        return count;
    }

    /// <summary>
    /// 跳转到战斗场景
    /// </summary>
    public void ChangeToFightingScene()
    {
        if (LoginModule.Instance.uiLogin != null)
            LoginModule.Instance.uiLogin.AddGreenTip("正在加载地图。。");
        //开始缓存消息,场景切换完成关闭缓存
        NetWorkModule.Instance.BeginCacheMsg();
        if (GameManager.Instance.CurrentMapData == null)
            GameManager.Instance.CurrentMapData = ConfigManager.Instance.mapConfig.GetMapDataByID((int)602);
        SceneManager.Instance.ChangeScene(GameManager.Instance.CurrentMapData.FileName, OnChangeSceneOver);
    }
    /// <summary>
    /// 加载场景结束后回调
    /// </summary>
    /// <param name="scene"></param>
    public void OnChangeSceneOver(string scene)
    {
        //GameManager.Instance.MapDataReader.ReadMap(scene);      //读取地图数据绘制阻挡点
        //CreatMainPlayer();
        ChatModule.Instance.Initialize();
        MainModule.Instance.Initialize();
        //GameManager.Instance.FightUI.Init();
        //场景切换完成关闭缓存
        NetWorkModule.Instance.FinishCacheMsg();
        GameAudioManager.Instance.StopGameBgAudio();
    }

    public void CreatMainPlayer()
    {
        if (GameManager.Instance.MainPlayer == null)
        {
            MainPlayer p = new MainPlayer();
            GameManager.Instance.MainPlayer = p;
            //p.CreatPlayer(GameManager.Instance.MainPlayerData.mapdata);
            //((MainPlayer)p).InitMainPlayer(GameManager.Instance.MainPlayerData);
            //GameManager.Instance.mEntitiesManager.AddCharacter(p);

            Debug.Log("Creat MainPlayer");
        }

    }
}
