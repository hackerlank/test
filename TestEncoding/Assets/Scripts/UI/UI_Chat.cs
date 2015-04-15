using System;
using UnityEngine;

public class UI_Chat
{
    private UIButton btnChat;
    private UIButton btnNextInput;
    private UIButton btnPreInput;
    private UIButton btnSend;
    private UIToggle channelCountry;
    private UIToggle channelFamily;
    private UIToggle channelUnion;
    private UIToggle channelWorld;
    private GameObject chatWindow;
    private UIInput msgInput;
    private UITextList textList;
    private Transform uiroot;

    public void AddLabel(string info)
    {
        string str = info;
        if (!string.IsNullOrEmpty(str))
        {
            this.textList.Add(str);
            //this.msgInput.value = string.Empty;
            //this.msgInput.isSelected = false;
        }
    }

    public void AddLabel(string name, string content)
    {
        string str = name + ": " + content;
        if (!string.IsNullOrEmpty(str))
        {
            this.textList.Add(str);
            this.msgInput.value = string.Empty;
            this.msgInput.isSelected = false;
        }
    }

    public enumChatType GetChannelType()
    {
        //if (this.channelFamily.value)
        //{
        //    return enumChatType.CHAT_TYPE_FAMILY;
        //}
        //if (this.channelUnion.value)
        //{
        //    return enumChatType.CHAT_TYPE_UNION;
        //}
        //if (!this.channelCountry.value && this.channelWorld.value)
        //{
        //    return enumChatType.CHAT_TYPE_WORLD;
        //}
        return enumChatType.CHAT_TYPE_COUNTRY;
    }

    public void Init()
    {
        this.uiroot = GameObject.Find("UI Root").transform;
        this.textList = this.uiroot.FindChild("Camera/Chat/TextField").GetComponent<UITextList>();
        this.msgInput = this.uiroot.FindChild("Camera/Input/Input").GetComponent<UIInput>();
        this.btnSend = this.uiroot.FindChild("Camera/Input/Send").GetComponent<UIButton>();
        //this.btnPreInput = this.uiroot.FindChild("Camera/Chat/Input/Pre").GetComponent<UIButton>();
        //this.btnNextInput = this.uiroot.FindChild("Camera/Chat/Input/Next").GetComponent<UIButton>();
        //this.channelFamily = this.uiroot.FindChild("Camera/Chat/Channels/family").GetComponent<UIToggle>();
        //this.channelUnion = this.uiroot.FindChild("Camera/Chat/Channels/union").GetComponent<UIToggle>();
        //this.channelCountry = this.uiroot.FindChild("Camera/Chat/Channels/country").GetComponent<UIToggle>();
        //this.channelWorld = this.uiroot.FindChild("Camera/Chat/Channels/world").GetComponent<UIToggle>();
        this.msgInput.label.maxLineCount = 1;
        this.chatWindow = this.uiroot.FindChild("Camera/Chat").gameObject;
        //this.btnChat = this.uiroot.FindChild("Camera/Buttons/Chat").GetComponent<UIButton>();
        //EventDelegate.Add(this.msgInput.onSubmit, new EventDelegate.Callback(this.OnSubmit));
        EventDelegate.Add(this.btnSend.onClick, new EventDelegate.Callback(this.OnSubmit));
        //EventDelegate.Add(this.btnChat.onClick, new EventDelegate.Callback(this.OpenOrCloseChatWindow));
        //EventDelegate.Add(this.btnPreInput.onClick, new EventDelegate.Callback(this.OnPreInput));
        //EventDelegate.Add(this.btnNextInput.onClick, new EventDelegate.Callback(this.OnNextInput));
        //this.channelCountry.value = true;
    }

    //public void OnNextInput()
    //{
    //    string nextMsg = LSingleton<ChatModule>.Instance.GetNextMsg();
    //    Debug.Log("Next:" + nextMsg);
    //    this.msgInput.isSelected = false;
    //    if (!string.IsNullOrEmpty(nextMsg))
    //    {
    //        this.msgInput.value = nextMsg;
    //    }
    //}

    //public void OnPreInput()
    //{
    //    string preMsg = LSingleton<ChatModule>.Instance.GetPreMsg();
    //    Debug.Log("Pre:" + preMsg);
    //    this.msgInput.isSelected = false;
    //    if (!string.IsNullOrEmpty(preMsg))
    //    {
    //        this.msgInput.value = preMsg;
    //    }
    //}

    public void OnSubmit()
    {
        if (this.textList != null)
        {
            string str = NGUIText.StripSymbols(this.msgInput.value);
            enumChatType channelType = this.GetChannelType();
            if (string.IsNullOrEmpty(str))
            {
                this.msgInput.value = string.Empty;
                this.msgInput.isSelected = false;
            }
            else
            {
                LUtil.Log("@@@@@发送聊天消息： " + str);
                ChatModule.Instance.SendChannleChat(str, channelType);
                //this.msgInput.value = string.Empty;
                //this.msgInput.isSelected = false;
                //this.AddLabel("老鬼", str);
            }
        }
    }

    //public void OpenOrCloseChatWindow()
    //{
    //    bool activeSelf = this.chatWindow.activeSelf;
    //    if (activeSelf)
    //    {
    //        this.msgInput.value = string.Empty;
    //        this.msgInput.isSelected = false;
    //    }
    //    else
    //    {
    //        LSingleton<ChatModule>.Instance.CurrentMsgID = LSingleton<ChatModule>.Instance.MsgHistory.Count;
    //        this.msgInput.isSelected = true;
    //    }
    //    this.chatWindow.SetActive(!activeSelf);
    //}
}

