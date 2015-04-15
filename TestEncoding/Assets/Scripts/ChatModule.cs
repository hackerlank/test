using Net;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChatModule : LSingleton<ChatModule>
{
    public ChatNetWork CNetWork;
    public int CurrentMsgID;
    public const int MaxCacheMsgCount = 20;
    public List<string> MsgHistory;
    public Dictionary<enumChatType, string> typeToChannelName = new Dictionary<enumChatType, string>();
    public UI_Chat uiChat;

    public void AddMsgToHistory(string content)
    {
        if (this.MsgHistory.Count >= 20)
        {
            this.MsgHistory.RemoveAt(0);
        }
        this.MsgHistory.Add(content);
        this.CurrentMsgID = this.MsgHistory.Count;
    }

    public string GetNextMsg()
    {
        string str = null;
        if (this.MsgHistory != null)
        {
            if (this.MsgHistory.Count == 0)
            {
                return str;
            }
            if (this.CurrentMsgID < (this.MsgHistory.Count - 1))
            {
                this.CurrentMsgID++;
                str = this.MsgHistory[this.CurrentMsgID];
            }
            Debug.Log(this.CurrentMsgID + "   " + this.MsgHistory.Count);
        }
        return str;
    }

    public string GetPreMsg()
    {
        string str = null;
        if (this.MsgHistory != null)
        {
            if (this.MsgHistory.Count == 0)
            {
                return str;
            }
            if (this.CurrentMsgID > 0)
            {
                this.CurrentMsgID--;
                str = this.MsgHistory[this.CurrentMsgID];
            }
            Debug.Log(this.CurrentMsgID + "   " + this.MsgHistory.Count);
        }
        return str;
    }

    public void Initialize()
    {
        this.uiChat = new UI_Chat();
        this.uiChat.Init();
        this.CNetWork = new ChatNetWork();
        this.CNetWork.Initialize();
        this.MsgHistory = new List<string>();
        this.CurrentMsgID = -1;
        this.typeToChannelName[enumChatType.CHAT_TYPE_COLORWORLD] = "【彩】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_PROVINCE] = "【省】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_CITY] = "【市】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_ALL_ZONE] = "【全区】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_OVERMAN] = "【盟】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_WORLD] = "【世】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_FAMILY] = "【家】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_AREA] = "【区】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_COUNTRY] = "【国】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_FRIEND] = "【友】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_TEAM] = "【队】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_UNION] = "【帮】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_NINE] = "【轻】";
        this.typeToChannelName[enumChatType.CHAT_TYPE_LOUDSPEAKER] = "【喇叭】";
    }

    public void OnReceiveChatMsg(UMessage msg)
    {
    }

    public void SendChannleChat(string content, enumChatType channelType = enumChatType.CHAT_TYPE_COUNTRY)
    {
        this.CNetWork.ReqChannleChat(content, channelType);
        this.AddMsgToHistory(content);
    }
}

