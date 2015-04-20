using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;

public class NGUI_Chat : NGUI_Base
{
    private Transform uiroot;
    //private GameObject chatWindow;
    private UIButton btnChat;
    private UITextList textList;
    private UITextList colorTexList;
    private UIButton btnSend;
    private UIInput msgInput;
    private UIButton btnPreInput;
    private UIButton btnNextInput;

    private UIToggle channelFamily;   //家族
    private UIToggle channelUnion;    //帮会
    private UIToggle channelCountry; //国家
    private UIToggle channelWorld;    //世界
    private UIToggle channelAlly;       //盟国
    private UIToggle channelAll;         //全部

    private List<UIButton> sendChannelBtns = new List<UIButton>();
    private List<UIToggle> sendChannels = new List<UIToggle>();

    private GameObject sendChannelsObj;
    //当前频道
    private UISprite curChannelSprite;
    private UIButton curChannelBtn;

    //轻声、好友、家族、帮会、国家、盟国、世界、彩信
    private enumChatType[] sendChannelTypes = new enumChatType[] { enumChatType.CHAT_TYPE_NINE, enumChatType.CHAT_TYPE_FRIEND, enumChatType.CHAT_TYPE_FAMILY, enumChatType.CHAT_TYPE_UNION, enumChatType.CHAT_TYPE_COUNTRY, enumChatType.CHAT_TYPE_OVERMAN, enumChatType.CHAT_TYPE_WORLD, enumChatType.CHAT_TYPE_COLORWORLD };
    //各频道对应的底图名字
    private string[] sendChannelPicNames = new string[] { "send-nine", "send-friend", "send-sept", "send-union", "send-country", "send-ally", "send-world", "send-caixin" };

    private const int MAX_SEND_CHANNELS = 8;
    private string commonPath = "";

    public override void Init()
	{
        uiroot = transform;
        textList = uiroot.FindChild(commonPath + "TextField").GetComponent<UITextList>();
        colorTexList = uiroot.FindChild(commonPath + "ColorTextFiled").GetComponent<UITextList>();
        msgInput = uiroot.FindChild(commonPath + "Input/Input").GetComponent<UIInput>();
        btnSend = uiroot.FindChild(commonPath + "Input/Send").GetComponent<UIButton>();
        btnPreInput = uiroot.FindChild(commonPath + "Input/Pre").GetComponent<UIButton>();
        btnNextInput = uiroot.FindChild(commonPath + "Input/Next").GetComponent<UIButton>();

        channelFamily = uiroot.FindChild(commonPath + "Channels/family").GetComponent<UIToggle>();
        channelUnion = uiroot.FindChild(commonPath + "Channels/union").GetComponent<UIToggle>();
        channelCountry = uiroot.FindChild(commonPath + "Channels/country").GetComponent<UIToggle>();
        channelWorld = uiroot.FindChild(commonPath + "Channels/world").GetComponent<UIToggle>();
        channelAlly = uiroot.FindChild(commonPath + "Channels/ally").GetComponent<UIToggle>();
        channelAll = uiroot.FindChild(commonPath + "Channels/all").GetComponent<UIToggle>();

        sendChannels.Clear();
        sendChannelBtns.Clear();
        for (int i = 1; i <= MAX_SEND_CHANNELS; ++i)
        {
            GameObject obj = uiroot.FindChild(commonPath + "Channels/sendchannels/" + i.ToString()).gameObject;
            if (obj != null)
            {
                sendChannels.Add(obj.GetComponent<UIToggle>());
                sendChannelBtns.Add(obj.GetComponent<UIButton>());
                UIEventListener.Get(obj).onClick = OnSelSendChannel;
            }
        }

        sendChannelsObj = uiroot.FindChild(commonPath + "Channels/sendchannels").gameObject;
        curChannelSprite = uiroot.FindChild(commonPath + "Channels/curchannel").GetComponent<UISprite>();
        curChannelBtn = uiroot.FindChild(commonPath + "Channels/curchannel").GetComponent<UIButton>();
        msgInput.label.maxLineCount = 1;

        //chatWindow = uiroot.FindChild("Camera/Chat").gameObject;
        //btnChat = uiroot.FindChild("Camera/Buttons/Chat").GetComponent<UIButton>();

	    EventDelegate.Add(msgInput.onSubmit, OnSubmit);
	    EventDelegate.Add(btnSend.onClick, OnSubmit);
        //EventDelegate.Add(btnChat.onClick, OpenOrCloseChatWindow);
        EventDelegate.Add(btnPreInput.onClick, OnPreInput);
        EventDelegate.Add(btnNextInput.onClick, OnNextInput);

        GameObject curChannelObj = uiroot.FindChild(commonPath + "Channels/curchannel").gameObject;
        UIEventListener.Get(curChannelObj).onClick = OnShowSendChannels;

        GameObject closeObj = transform.FindChild(commonPath + "Close").gameObject;
        if (closeObj != null)
            UIEventListener.Get(closeObj).onClick = OnDlgClose;

        //channelCountry.value = true;


        //把缓存消息更新到聊天界面
        foreach (ChatMsg msg in ChatModule.Instance.cachedMsg)
        {
            if (ChatModule.Instance.CNetWork != null)
                ChatModule.Instance.CNetWork.ProcessMsg(msg.dwType, msg.channelName, msg.thisname, msg.thischat);
        }
        ChatModule.Instance.cachedMsg.Clear();
	}

    public void OnSubmit()
    {
        if (textList != null)
        {
            // It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
            string text = NGUIText.StripSymbols(msgInput.value);

            enumChatType channelType = GetSendChannelType();
            if (string.IsNullOrEmpty(text))
            {
                msgInput.value = "";
                msgInput.isSelected = false;
                return;
            }

            msgInput.value = "";

            //GM指令
            if ((text == "//debug the game system") || (text == "//debug game"))
            {
                Util.fileLogLevel = 7;
                Util.sendLogLevel = 7;
                Util.logFuncType = 127;
                Util.writeLog = true;
                Util.showDebugInfo = true;

                return;
            }

            ChatModule.Instance.SendChannleChat(text, channelType);

            //AddLabel(GameManager.Instance.MainPlayer.mapUserData.name, text);
            //AddLabel("法宝小号", text);
        }
    }

    public void OnSelSendChannel(GameObject iterm)
    {
        if (iterm != null)
        {
            uint index = 0;
            uint.TryParse(iterm.name, out index);

            if ((index > 0) && (index <= sendChannelPicNames.Length))
            {
                if (curChannelSprite != null)
                {
                    curChannelSprite.spriteName = sendChannelPicNames[index - 1];
                }
            }
        }

        sendChannelsObj.SetActive(false);
    }

    /// <summary>
    /// 取得接收频道类型
    /// </summary>
    /// <returns></returns>
    public enumChatType GetReceiveChannelType()
    {
        if (channelFamily.value)
        {
            return enumChatType.CHAT_TYPE_FAMILY;
        }
        if (channelUnion.value)
        {
            return enumChatType.CHAT_TYPE_UNION;
        }
        if (channelCountry.value)
        {
            return enumChatType.CHAT_TYPE_COUNTRY;
        }
        if (channelWorld.value)
        {
            return enumChatType.CHAT_TYPE_WORLD;
        }
        if (channelAlly.value)
        {
            return enumChatType.CHAT_TYPE_OVERMAN;
        }
        if (channelAll.value)
        {
            return enumChatType.CHAT_TYPE_ALL;
        }

        return enumChatType.CHAT_TYPE_ALL;
    }

    /// <summary>
    /// 取得发送频道类型
    /// </summary>
    /// <returns></returns>
    public enumChatType GetSendChannelType()
    {
        for (int i = 0; i < sendChannels.Count; ++i)
        {
            if (sendChannels[i].value)
            {
                if (i < sendChannelTypes.Length)
                    return sendChannelTypes[i];
            }
        }

        return enumChatType.CHAT_TYPE_NINE;

    }

    public void AddLabel(string name,string content)
    {
        string text = name + ": " + content;
        if (!string.IsNullOrEmpty(text))
        {
            textList.Add(text);
            msgInput.value = "";
            msgInput.isSelected = false;
        }
    }

    public void AddLabel(enumChatType type, string channelName, string thisname, string thischat)
    {
        if (type == enumChatType.CHAT_TYPE_COLORWORLD)
        {
            int index = thischat.IndexOf("\n");
            if (index != -1)
            {
                string zonecountry = thischat.Substring(0, index);
                string content = thischat.Substring(index, thischat.Length - index);
                string text = "[9000ff]" + channelName + "【" + zonecountry + "】" + thisname + ":" + content;
                colorTexList.Add(text);
            }
        }
        //else if (type == enumChatType.CHAT_TYPE_OVERMAN)
        //{
        //    int index = thischat.IndexOf("\n");
        //    string zonecountry = thischat.Substring(0, index);
        //    string content = thischat.Substring(index, thischat.Length - index);
        //    string text = "[6a0c99]" + channelName + "【" + zonecountry + "】" + thisname + ":" + content;
        //    colorTexList.Add(text);
        //}
    }
    public void AddLabel(enumChatType type, string content)
    {
        //收到指定频道 或者 当前设置为全部接收 或者为彩信
        if ((GetReceiveChannelType() == type) || (GetReceiveChannelType() == enumChatType.CHAT_TYPE_ALL) || (type == enumChatType.CHAT_TYPE_COLORWORLD))
        {
            //轻声：普通聊天，显示在全部频道内，RGB值86,53,9
            //好友：好友之间聊天，显示在全区频道内，RGB值22,101,181
            //家族：家族成员之间聊天，显示在家族频道内，RGB值196,47,12
            //帮会：帮会成员之间聊天，显示在帮会频道内，RGB值1,152,101
            //国家：本国内玩家之间聊天，显示在国家频道内，RGB值79,112,0
            //盟国：显示本盟内玩家之间聊天，显示在盟国频道内，RGB值 226,16,153
            //世界：显示本区内所有玩家的世界聊天：显示在世界频道内，RGB值 106,12,153
            //彩信，特殊聊天，显示全部游戏服务器玩家发送的聊天，RGB值 144,0,255。

            //颜色暂时写死
            string text = content;
            if (type == enumChatType.CHAT_TYPE_NINE)
                text = "[563509]" + text;
            else if (type == enumChatType.CHAT_TYPE_FRIEND)
                text = "[1665b5]" + text;
            else if (type == enumChatType.CHAT_TYPE_FAMILY)
                text = "[c42f0c]" + text;
            else if (type == enumChatType.CHAT_TYPE_UNION)
                text = "[019865]" + text;
            else if (type == enumChatType.CHAT_TYPE_COUNTRY)
                text = "[4f7000]" + text;
            else if (type == enumChatType.CHAT_TYPE_OVERMAN)
                text = "[e21099]" + text;
            else if (type == enumChatType.CHAT_TYPE_WORLD)
                text = "[6a0c99]" + text;
            else if (type == enumChatType.CHAT_TYPE_COLORWORLD)
            {
                text = "[9000ff]" + text;
                colorTexList.Add(text);
                return;
            }
            else
                text = "[999999]" + text;

            if (!string.IsNullOrEmpty(text))
            {
                textList.Add(text);
            }
        }
    }

    //public void OpenOrCloseChatWindow()
    //{
    //    bool b = chatWindow.activeSelf;

    //    if (b)
    //    {
    //        msgInput.value = "";
    //        msgInput.isSelected = false;
    //    }
    //    else
    //    {
    //        ChatModule.Instance.CurrentMsgID = ChatModule.Instance.MsgHistory.Count;
    //        msgInput.isSelected = true;
    //    }
        
    //    chatWindow.SetActive(!b);
    //}

    public void OnPreInput()
    {
        string s = ChatModule.Instance.GetPreMsg();
        Debug.Log("Pre:" + s);
        msgInput.isSelected = false;
        if (!string.IsNullOrEmpty(s))
        {
            //msgInput.isSelected = true;
            msgInput.value = s;
        }
    }

    public void OnNextInput()
    {
        string s = ChatModule.Instance.GetNextMsg();
        Debug.Log("Next:" + s);
        msgInput.isSelected = false;
        if (!string.IsNullOrEmpty(s))
        {
            //msgInput.isSelected = true;
            msgInput.value = s;
        }
    }

    public void OnShowSendChannels(GameObject iterm)
    {
        if (sendChannelsObj != null)
            sendChannelsObj.SetActive(!sendChannelsObj.active);
    }

    public void OnDlgClose(GameObject iterm)
    {
        NGUIManager.Instance.DisActiveByName(NGUI_UI.NGUI_Chat);
    }
}
