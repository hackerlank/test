using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginGUI : MonoBehaviour {
    string account = "";
    string[] servernames = new string[] { "FreeZT44", "FreeZT90", "外网" };
    int gridint = 0;    //服务器列表
    string nickname = "";
    string[] Sexes = new string[] { "Male", "Female" };
    int sex = 0;
    string[] professional = new string[] { "弓手", "法师" };
    int pro = 0;
    Dictionary<int, ushort> gameZone = new Dictionary<int, ushort>();

	// Use this for initialization
	void Start () {
        gameZone.Add(0, 797);
        gameZone.Add(1, 2300);
        gameZone.Add(2, 1354);

        account = PlayerPrefs.GetString("Account");
        gridint = PlayerPrefs.GetInt("ZoneID");
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnGUI()
    {
        if(LoginModule.Instance.isShowLoginUI)
        {
            account = GUI.TextField(new Rect(0, Screen.height - 100, 200, 100), account, 15);
            if (GUI.Button(new Rect(200, Screen.height - 100, 100, 100), "LogIn"))
            {
                if (account != "")
                {
                    PlayerPrefs.SetString("Account",account);   //账号写入存档
                    LoginModule.Instance.Account = account; //账号写入内存
                    
                    PlayerPrefs.SetInt("ZoneID", gridint);
                    PlayerPrefs.SetInt("Zone", gameZone[gridint]);

                    if (gridint == 2)       //外网ip
                    {
                        LoginModule.Instance.FLIP = "101.226.182.5";
                        LoginModule.Instance.FLPort = 7000;
                    }
                    else
                    {
                        LoginModule.Instance.FLIP = "192.168.5.46";
                        LoginModule.Instance.FLPort = 7000;
                    }

                    LoginModule.Instance.Login();
                }
                Debug.Log(gameZone[gridint]);
            }

            gridint = GUI.SelectionGrid(new Rect(25, 25, 200, 50), gridint, servernames, 2);
        }
        if (LoginModule.Instance.isShowRegisterUI)
        {
            sex = GUI.SelectionGrid(new Rect(Screen.width/2-100,Screen.height-200, 200, 50), sex, Sexes, 2);
            byte sex0;
            if(sex == 0)
            {
                sex0 = 1;
            }
            else
            {
                sex0 = 2;
            }

            uint _pro = 0;
            if(pro == 0)
            {
                _pro = (uint)msg.Occupation.Occu_Dema;
            }
            else if (pro == 1)
            {
                _pro = (uint)msg.Occupation.Occu_Yaohu;
            }

            pro = GUI.SelectionGrid(new Rect(Screen.width / 2 - 100, Screen.height - 400, 200, 50), pro, professional, 2);

            nickname = GUI.TextField(new Rect(Screen.width / 2 - 100, Screen.height - 300, 200, 100), nickname, 15);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height - 100,200,100),"进入游戏"))
            {
                LoginModule.Instance.RegisterPlayer(nickname, sex0,_pro);
            }
        }
    }
}
