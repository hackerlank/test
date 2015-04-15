using GBKEncoding;
using Net;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ChatNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public void OnRetChannelChat(UMessage msg)
    {
        Debug.Log("收到聊天消息！");
        stServerChannelChatUserCmd t = new stServerChannelChatUserCmd();
        msg.ReadStruct<stServerChannelChatUserCmd>(t);
        byte[] buffer = new byte[0x20];
        int index = 0;
        while (true)
        {
            buffer[index] = msg.ReadByte();
            if (buffer[index] == 0)
            {
                break;
            }
            index++;
        }
        byte[] buffer3 = new byte[0x100];
        index = 0;
        while (true)
        {
            buffer3[index] = msg.ReadByte();
            if (buffer3[index] == 0)
            {
                break;
            }
            index++;
        }
        string str = GBKEncoder.Read(buffer);
        string str2 = GBKEncoder.Read(buffer3);
        string str3 = string.Empty;
        if (LSingleton<ChatModule>.Instance.typeToChannelName.TryGetValue((enumChatType)t.dwType, out str3))
        {
            LUtil.Log(str3 + str + ": " + str2, LUtil.LogType.Normal, false);
            ChatModule.Instance.uiChat.AddLabel(str3 + str + ": " + str2);
        }
    }

    public override void RegisterMsg()
    {
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(14, 1), new OnMessageCallback(this.OnRetChannelChat));
    }

    public void ReqChannleChat(string contens, enumChatType channelType = enumChatType.CHAT_TYPE_COUNTRY)
    {
        Debug.Log("发送聊天消息");
        UMessage msg = new UMessage();
        stChannelChatUserCmd t = new stChannelChatUserCmd {
            dwType = (uint) channelType,
            pstrChat = contens,
            dwChatTime = 11,
            pstrName = "@@我的名字"
        };
        if (msg.WriteStruct<stChannelChatUserCmd>(t))
        {
            SingletonForMono<NetWorkModule>.Instance.SendImmediate(msg);
        }
    }
}

