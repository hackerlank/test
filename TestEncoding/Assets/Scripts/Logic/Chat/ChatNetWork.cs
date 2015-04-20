/*
***********************************************************************************************************
* CLR version ：$clrversion$
* Machine name ：$machinename$
* Creation time ：#time#
* Author ：hym
* Version number : 1.0
***********************************************************************************************************
*/

using System;
using chat;
using Net;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using NGUI;

using GBKEncoding;


public class ChatNetWork : NetWorkBase
{
    public override void Initialize()
    {
        RegisterMsg();
    }

    public override void RegisterMsg()
    {
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_ChannelChat_SC, OnRetChannelChat);
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(14, 1), OnRetChannelChat);
    }

    public void ReqChannleChat(string contens, enumChatType channelType = enumChatType.CHAT_TYPE_COUNTRY)
    {
        Debug.Log("发送聊天消息");
        UMessage message = new UMessage();
        //message.WriteHead(14, 1);



        //dwType
        //message.WriteUInt32((uint)channelType);
        //dwSysInfoType
        //message.WriteUInt32(0);
        //dwCharType
        //message.WriteUInt32(1);
        //dwChannelID
        //message.WriteUInt32(0);
        //dwFromID
        //message.WriteUInt32(GameManager.Instance.MainPlayer.m_dwTmpID);
        //Debug.Log("聊天发送主角tmpID: " + GameManager.Instance.MainPlayer.m_dwTmpID);
        //dwChatTime
        //message.WriteUInt32(GameTime.Instance.GetIntervalMsecond());
        //size
        //message.WriteByte(0);
        ////name
        //message.WriteString(GameManager.Instance.MainPlayer.m_name, GlobalVar.MAX_NAMESIZE);
        ////contents
        //message.WriteString(contens, GlobalVar.MAX_CHATINFO);

        ////dwThisID
        //message.WriteUInt32(0);

        stChannelChatUserCmd cmd = new stChannelChatUserCmd();

        cmd.dwType = (uint)channelType;
        cmd.pstrChat = contens;


        //cmd.size1 = 123;
        if (message.WriteStruct<stChannelChatUserCmd>(cmd))
            NetWorkModule.Instance.SendImmediate(message);

        /////////////////////////////////////////////////////////////
        //
        //UMessage message2 = new UMessage();
        //stChannelChatUserCmd cmd = new stChannelChatUserCmd();
        //cmd.dwFromID = GameManager.Instance.MainPlayer.m_dwTmpID;

        //cmd.pstrName = GameManager.Instance.MainPlayer.m_name;

        //cmd.pstrChat = contens;

        //byte[] bytes = Converter.StructToBytes(cmd);

        //message2.WriteStruct(cmd.byCmd, cmd.byParam, bytes);

        //NetWorkModule.Instance.SendImmediate(message);
    }

    public void OnRetChannelChat(UMessage msg)
    {
        Debug.Log("收到聊天消息！");
        //MSG_Ret_ChannelChat_SC data = msg.ReadProto<MSG_Ret_ChannelChat_SC>();
        //ChatModule.Instance.OnReceiveChatMsg(msg);

        stServerChannelChatUserCmd st = new stServerChannelChatUserCmd();
        msg.ReadStruct<stServerChannelChatUserCmd>(st);

        //stServerChannelChatUserCmd cmd = new stServerChannelChatUserCmd();
        //byte[] bytes = msg.ReadStruct(Marshal.SizeOf(cmd));
        //cmd = (stServerChannelChatUserCmd)Converter.BytesToStruct(bytes, cmd.GetType());

        byte[] name = new byte[GlobalVar.MAX_NAMESIZE];
        int index = 0;
        while (true)
        {
            name[index] = msg.ReadByte();
            if (name[index] == 0)
            {
                break;
            }
            else
            {
                index++;
            }
        }

        byte[] chat = new byte[GlobalVar.MAX_CHATINFO];
        index = 0;
        while (true)
        {
            chat[index] = msg.ReadByte();
            if (chat[index] == 0)
            {
                break;
            }
            else
            {
                index++;
            }
        }

        string thisname = GBKEncoder.Read(name);
        string thischat = GBKEncoder.Read(chat);

        //string thisname = UMessage.gbk.GetString(name);
        //string thischat = UMessage.gbk.GetString(chat);
        //string thisname = Encoding.UTF8.GetString(name);
        //string thischat = Encoding.UTF8.GetString(chat);

        //Encoding gb2312 = Encoding.GetEncoding("gb2312");
        //string thischat = Encoding.ASCII.GetString(chat);

        //if (cmd.dwType == 12)
        //{
        //Util.Log(thisname + ": " + thischat);
        //ChatModule.uiChat.AddLabel(thisname, thischat);
        //}

        string channelName = "";
        if (ChatModule.Instance.typeToChannelName.TryGetValue((enumChatType)st.dwType, out channelName))
        {
            //Util.Log(channelName + thisname + ": " + thischat);
            if (ChatModule.Instance.uiChat != null)
            {
                ProcessMsg((enumChatType)st.dwType, channelName, thisname, thischat);
            }
            else//缓存聊天消息
            {
                ChatMsg rec = new ChatMsg();
                rec.dwType = (enumChatType)st.dwType;
                rec.channelName = channelName;
                rec.thisname = thisname;
                rec.thischat = thischat;
                if (ChatModule.Instance.cachedMsg.Count > 100)
                    ChatModule.Instance.cachedMsg.RemoveAt(0);

                ChatModule.Instance.cachedMsg.Add(rec);
            }
        }
        else
        {
            //系统公告
            if (st.dwType == (uint)enumChatType.CHAT_TYPE_SYSTEM)
            {
                switch (st.dwSysInfoType)
                {
                    case (uint)enumSysInfoType.INFO_TYPE_FAIL:
                        {
                            if (MainModule.Instance.uiMainWindow != null)
                                MainModule.Instance.uiMainWindow.AddRedTip(thischat);
                        }
                        break;
                    case (uint)enumSysInfoType.INFO_TYPE_PHONE_GAME:
                        {
                            if (MainModule.Instance.uiMainWindow != null)
                                MainModule.Instance.uiMainWindow.AddGreenTip(thischat);
                        }
                        break;
                    case (uint)enumSysInfoType.INFO_TYPE_PHONE_REWARD_DAY:
                        {
                            NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
                            {
                                script.Init();
                                script.InitDesc(thischat);
                            });
                        }
                        break;
                    
                }
            }
         }
    }

    public void ProcessMsg(enumChatType dwType, string channelName, string thisname, string thischat)
    {
        //彩信
        if ((dwType == enumChatType.CHAT_TYPE_COLORWORLD))// || (dwType == enumChatType.CHAT_TYPE_OVERMAN))
            ChatModule.Instance.uiChat.AddLabel(dwType, channelName, thisname, thischat);
        else
            ChatModule.Instance.uiChat.AddLabel(dwType, channelName + thisname + ": " + thischat);
    }
}
