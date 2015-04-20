using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NGUI;
using Net;

public enum enumMainPanelType
{
    MainPanelType_GiftBag = 1,
    MainPanelType_PublishTask = 2,
    MainPanelType_CardActivity = 3,
    MainPanelType_ZhuanShuFuBen = 4,
}

public class UI_MainWindow 
{
    private UISprite sprPic; //头像
    private UILabel lblName; //名字
    private UILabel lblLevel; //等级
    private UISprite sprVIP; //VIP
    private UISprite sprOfficeName;//官职
    private UILabel lblSeptName;//家族
    private UILabel lblUnionName;//帮会
    private UILabel lblCountryName;//国家
    private UILabel lblZoneName;//区

    private GameObject btnMainPageObj;
    private GameObject btnSocialObj;
    private GameObject btnChatObj;
    private GameObject btnBagObj;
    private GameObject btnMailObj;
    private GameObject btnAwardObj;

    private GameObject welcomPicObj;

    public const int AnnounceCount = 6;
    //公告
    private UILabel[] lblAnnouncements = new UILabel[AnnounceCount];
    private List<string> strAnnoucements = new List<string>();
    private List<Color> colorAnnoucements = new List<Color>();
    private Transform uiroot;

    //private GameObject chatDlg;

    private string commonPath = "Camera/Main/";

    private const int MAX_PANELS = 5;
    //主界面滑动面板
    private List<GameObject> mPanelObjects = new List<GameObject>();
    //面板对应内容
    private List<GameObject> mTabItemObjects = new List<GameObject>();
    public void Init()
    {
        uiroot = GameObject.Find("UI_Root").transform;
        sprPic = uiroot.FindChild(commonPath + "Icon/charpic").GetComponent<UISprite>();
        lblName = uiroot.FindChild(commonPath + "Icon/charname").GetComponent<UILabel>();
        lblLevel = uiroot.FindChild(commonPath + "Icon/level").GetComponent<UILabel>();
        sprVIP = uiroot.FindChild(commonPath + "Icon/viptex").GetComponent<UISprite>();
        sprOfficeName = uiroot.FindChild(commonPath + "Icon/officaltitle").GetComponent<UISprite>();

        stUserInfoUserCmd selInfo = GameManager.Instance.MainPlayer.selectUserInfo;
        if ((lblName != null) && (selInfo != null))
        {
            if (selInfo.level >= selInfo.level2)
                lblName.text = selInfo.name;
            else
                lblName.text = selInfo.name2;
        }
        if ((lblLevel != null) && ((selInfo != null)))
        {
            if (selInfo.level >= selInfo.level2)
                lblLevel.text = selInfo.level.ToString();
            else
                lblLevel.text = selInfo.level2.ToString();
        }
        if (sprVIP != null)
            sprVIP.spriteName = "";
        if (sprOfficeName != null)
            sprOfficeName.spriteName = "";

        lblSeptName = uiroot.FindChild(commonPath + "Icon/sept").GetComponent<UILabel>();
        lblUnionName = uiroot.FindChild(commonPath + "Icon/union").GetComponent<UILabel>();
        lblCountryName = uiroot.FindChild(commonPath + "Icon/country").GetComponent<UILabel>();
        lblZoneName = uiroot.FindChild(commonPath + "Icon/zone").GetComponent<UILabel>();

        //chatDlg = uiroot.Find("Camera/Chat").gameObject;

        btnMainPageObj = uiroot.FindChild(commonPath + "Buttons/MainPage").gameObject;
        btnSocialObj = uiroot.FindChild(commonPath + "Buttons/Social").gameObject;
        btnChatObj = uiroot.FindChild(commonPath + "Buttons/Chat").gameObject; 
        btnBagObj = uiroot.FindChild(commonPath + "Buttons/Bag").gameObject; 
        btnMailObj = uiroot.FindChild(commonPath + "Buttons/Mail").gameObject; 
        btnAwardObj = uiroot.FindChild(commonPath + "Buttons/Award").gameObject;
        if (btnMainPageObj != null)
            UIEventListener.Get(btnMainPageObj).onClick = OnClickMainPageBtn;
        if (btnSocialObj != null)
            UIEventListener.Get(btnSocialObj).onClick = OnClickSocialBtn;
        if (btnChatObj != null)
            UIEventListener.Get(btnChatObj).onClick = OnClickChatBtn;
        if (btnBagObj != null)
            UIEventListener.Get(btnBagObj).onClick = OnClickBagBtn;
        if (btnMailObj != null)
            UIEventListener.Get(btnMailObj).onClick = OnClickMailBtn;
        if (btnAwardObj != null)
            UIEventListener.Get(btnAwardObj).onClick = OnClickAwardBtn;

        mPanelObjects.Clear();
        for (int i = 1; i <= MAX_PANELS; ++i)
        {
            GameObject obj = uiroot.FindChild(commonPath + "Panel/Scroll View/UIGrid/" + i.ToString()).gameObject;
            if (obj != null)
            {
                mPanelObjects.Add(obj);
                UIEventListener.Get(obj).onClick = OnClickPanel;
            }
        }

        for (int i = 1; i <= MAX_PANELS; ++i)
        {
            GameObject obj = uiroot.FindChild(commonPath + "TabItems/" + i.ToString()).gameObject;
            if (obj != null)
            {
                mTabItemObjects.Add(obj);
            }
        }

        //礼包相关
        GameObject giftbagObj = uiroot.FindChild(commonPath + "TabItems/1/btn").gameObject;
        if (giftbagObj != null)
            UIEventListener.Get(giftbagObj).onClick = OnClickBuyGiftBag;

        //任务相关
        GameObject countryTaskObj = uiroot.FindChild(commonPath + "TabItems/2/PublishCountryTask").gameObject;
        if (countryTaskObj != null)
            UIEventListener.Get(countryTaskObj).onClick = OnClickPubCountryTask;
        GameObject unionTaskObj = uiroot.FindChild(commonPath + "TabItems/2/PublishUnionTask").gameObject;
        if (unionTaskObj != null)
            UIEventListener.Get(unionTaskObj).onClick = OnClickPubUnionTask;

        //公告
        for (int i=0; i<AnnounceCount; ++i)
        {
            lblAnnouncements[i] = uiroot.FindChild("Camera/Announcement/Label" + (i + 1).ToString()).GetComponent<UILabel>();
        }

        welcomPicObj = uiroot.FindChild(commonPath + "Welcome").gameObject;
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
        for (int i=0; i<strAnnoucements.Count; ++i)
        {
            int index = strAnnoucements.Count - i -1;
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

    public void OnClickPanel(GameObject iterm)
    {
        if (iterm != null)
        {
            uint index = 0;
            uint.TryParse(iterm.name, out index);

            if ((index > 0) && (index <= mPanelObjects.Count))
            {
                //Debug.LogError("@Click panel: " + index);
                if (index == (uint)enumMainPanelType.MainPanelType_GiftBag)
                {
                    Debug.Log("打开购买礼包界面！");
                    NGUIManager.Instance.AutoDeleteUI();
                    NGUIManager.Instance.AddByName<NGUI_GiftBag>(NGUI_UI.NGUI_GiftBag, NGUIShowType.ONLYONE, delegate(NGUI_GiftBag script)
                    {
                        script.Init();
                    });
                }
                else if (index == (uint)enumMainPanelType.MainPanelType_PublishTask)
                {
                    if (welcomPicObj != null)
                        welcomPicObj.SetActive(false);
                }
                else if (index == (uint)enumMainPanelType.MainPanelType_CardActivity)
                {
                    NGUIManager.Instance.AutoDeleteUI();
                    NGUIManager.Instance.AddByName<NGUI_CardActivity>(NGUI_UI.NGUI_CardActivity, NGUIShowType.ONLYONE, delegate(NGUI_CardActivity script)
                    {
                        script.Init();
                    });
                }
                else if (index == (uint)enumMainPanelType.MainPanelType_ZhuanShuFuBen)
                {
                    NGUIManager.Instance.AutoDeleteUI();
                    NGUIManager.Instance.AddByName<NGUI_ZhuanShuFuBen>(NGUI_UI.NGUI_ZhuanShuFuBen, NGUIShowType.ONLYONE, delegate(NGUI_ZhuanShuFuBen script)
                    {
                        script.Init();
                    });
                }

            }
        }

    }

    //点击购买礼包
    public void OnClickBuyGiftBag(GameObject iterm)
    {
        Debug.Log("打开购买礼包界面！");
        NGUIManager.Instance.AutoDeleteUI();
        NGUIManager.Instance.AddByName<NGUI_GiftBag>(NGUI_UI.NGUI_GiftBag, NGUIShowType.ONLYONE, delegate(NGUI_GiftBag script)
        {
            script.Init();
        });

    }

    //点击发布国家任务按钮
    public void OnClickPubCountryTask(GameObject iterm)
    {
        Debug.Log("官员发布任务！");
        if ((GameManager.Instance.MainPlayer.baseUserInfo != null) && (GameManager.Instance.MainPlayer.baseUserInfo.chenghao != "国王")
            && (GameManager.Instance.MainPlayer.baseUserInfo.chenghao != "宰相") && (GameManager.Instance.MainPlayer.baseUserInfo.chenghao != "御史大夫"))
        {
            NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
            {
                script.Init();
                script.InitDesc("您不是国王\\宰相\\御史大夫,无法发布任务。");
            });

            return;
        }

        NGUIManager.Instance.AutoDeleteUI();
        NGUIManager.Instance.AddByName<NGUI_PublishTask>(NGUI_UI.NGUI_PublishTask, NGUIShowType.ONLYONE, delegate(NGUI_PublishTask script)
        {
            script.Init();
        });

    }
    //点击发布帮会任务按钮
    public void OnClickPubUnionTask(GameObject iterm)
    {
        Debug.Log("帮主发布任务！");
        //if (GameManager.Instance.MainPlayer.baseUserInfo.chenghao != "帮主")
        {
            NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
            {
                script.Init();
                script.InitDesc("暂未开发，敬请期待");
            });

            return;
        }

        NGUIManager.Instance.AutoDeleteUI();
        NGUIManager.Instance.AddByName<NGUI_PublishTask>(NGUI_UI.NGUI_PublishTask, NGUIShowType.ONLYONE, delegate(NGUI_PublishTask script)
        {
            script.SetCountryTask(false);
            script.Init();
        });

    }

    public void OnClickMainPageBtn(GameObject iterm)
    {
        Debug.Log("@@主页");
        if (welcomPicObj != null)
            welcomPicObj.SetActive(true);

        foreach (GameObject obj in mTabItemObjects)
        {
            obj.SetActive(false);
        }

        NGUIManager.Instance.AutoDeleteUI();
        NGUIManager.Instance.AutoDeleteUIByName();
        //if (chatDlg != null)
        //    chatDlg.SetActive(false);
    }

    public void OnClickSocialBtn(GameObject iterm)
    {
        Debug.Log("@@社会");
        MainModule.Instance.uiMainWindow.AddGreenTip("即将开放 敬请期待！");
    }

    public void OnClickChatBtn(GameObject iterm)
    {
        Debug.Log("@@聊天");
        GameObject chatObj = NGUIManager.Instance.GetDlgRootByName(NGUI_UI.NGUI_Chat);
        if ((chatObj != null) && (chatObj.active == true))
        {
            NGUIManager.Instance.DisActiveByName(NGUI_UI.NGUI_Chat);
        }
        else
        {
            NGUIManager.Instance.AutoDeleteUI();
            NGUIManager.Instance.AddByName<NGUI_Chat>(NGUI_UI.NGUI_Chat, NGUIShowType.ACTIVE_FALSE, delegate(NGUI_Chat script)
            {
                ChatModule.Instance.uiChat = script;
                script.Init();
            });
        }

    }

    public void OnClickBagBtn(GameObject iterm)
    {
        Debug.Log("@@包裹");
        NGUIManager.Instance.AutoDeleteUI();
        NGUIManager.Instance.AddByName<NGUI_Bag>(NGUI_UI.NGUI_Bag, NGUIShowType.ONLYONE, delegate(NGUI_Bag script)
        {
            NGUIManager.Instance.nguiBag = script;
            script.Init();
        });
    }

    public void OnClickMailBtn(GameObject iterm)
    {
        Debug.Log("@@邮件");
        //MainModule.Instance.uiMainWindow.AddGreenTip("即将开放 敬请期待！");
        //改用签到
        UMessage message = new UMessage();

        stReqDayReward cmd = new stReqDayReward();
        if (message.WriteStruct<stReqDayReward>(cmd))
            NetWorkModule.Instance.SendImmediate(message);
    }

    public void OnClickAwardBtn(GameObject iterm)
    {
        Debug.Log("@@抽奖");
        Application.OpenURL("http://app.zt.ztgame.com");
    }

    public void RefreshBaseInfo()
    {
        stUserInfoUserCmd selInfo = GameManager.Instance.MainPlayer.selectUserInfo;
        stSendUserInfoSC baseInfo = GameManager.Instance.MainPlayer.baseUserInfo;
        if ((selInfo != null) && (baseInfo != null))
        {
            if (sprPic != null)
            {
                string picname = ConfigManager.Instance.mainUIConfig.GetPicNameByID(baseInfo.face);
                if (picname != "")
                    sprPic.spriteName = picname;
            }

            if (lblName != null)
            {
                if (selInfo.level >= selInfo.level2)
                    lblName.text = selInfo.name;
                else
                    lblName.text = selInfo.name2;
            }

            if (sprVIP != null)
            {
                byte vipLevel = baseInfo.vipLevel;
                sprVIP.spriteName = "VIP" + vipLevel.ToString();
            }

            if (sprOfficeName != null)
            {
                if (baseInfo.chenghao == "国王")
                    sprOfficeName.spriteName = "king";
                else if (baseInfo.chenghao == "宰相")
                    sprOfficeName.spriteName = "zaixiang";
                else if (baseInfo.chenghao == "元帅")
                    sprOfficeName.spriteName = "yuanshuai";
                else if (baseInfo.chenghao == "御史大夫")
                {
                    sprOfficeName.spriteName = "ysdf";
                    UIWidget widget = sprOfficeName.GetComponent<UIWidget>();
                    widget.width = 123;
                    widget.height = 35;
                }
                else if (baseInfo.chenghao == "捕头")
                    sprOfficeName.spriteName = "butou";
                else
                    sprOfficeName.spriteName = "";

            }

            if (lblSeptName != null)
            {
                lblSeptName.text = baseInfo.septName;
                if (lblSeptName.text == "")
                    lblSeptName.text = "无";
            }

            if (lblUnionName != null)
            {
                lblUnionName.text = baseInfo.unionName;
                if (lblUnionName.text == "")
                    lblUnionName.text = "无";
            }

            if (lblCountryName != null)
                lblCountryName.text = baseInfo.countryName;

            if (lblZoneName != null)
                lblZoneName.text = baseInfo.zoneName;
        }
    }
}
