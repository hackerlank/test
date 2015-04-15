using Net;
using System;
using UnityEngine;

public class GameTime : LSingleton<GameTime>
{
    private uint m_dwClientTime;
    private ulong m_dwServerStartTime;
    private ulong m_dwServerTime;
    private ulong m_qwClientMsec;
    private DateTime startTime = new DateTime(0x7b2, 1, 1).ToLocalTime();

    private uint ConvertDateTimeInt(DateTime time)
    {
        TimeSpan span = (TimeSpan) (time - this.startTime);
        return (uint) span.TotalSeconds;
    }

    public uint GetIntervalMsecond()
    {
        ulong nowMsecond = this.GetNowMsecond();
        if (nowMsecond >= this.m_qwClientMsec)
        {
            ulong num2 = nowMsecond - this.m_qwClientMsec;
            return (uint) num2;
        }
        return 0;
    }

    public ulong GetNowMsecond()
    {
        TimeSpan span = (TimeSpan) (DateTime.Now - this.startTime);
        return (ulong) span.TotalMilliseconds;
    }

    public uint GetNowSecond()
    {
        return this.ConvertDateTimeInt(DateTime.Now);
    }

    public ulong GetServerTime()
    {
        uint nowSecond = this.GetNowSecond();
        if (nowSecond >= this.m_dwClientTime)
        {
            return ((this.m_dwServerTime + nowSecond) - this.m_dwClientTime);
        }
        return this.m_dwServerTime;
    }

    public void Init()
    {
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(2, 1), new OnMessageCallback(this.OnServerTimeInit));
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(2, 2), new OnMessageCallback(this.OnServerTimeReq));
    }

    private void OnServerTimeInit(UMessage message)
    {
        Debug.Log("OnServerTimeInit");
        this.m_dwServerTime = message.ReadUInt64();
        this.m_dwClientTime = this.GetNowSecond();
    }

    private void OnServerTimeReq(UMessage message)
    {
        Debug.Log("OnServerTimeReq.." + DateTime.Now);
        this.UpdateNowMsecond();
        this.SendUserGameTime();
    }

    private void SendUserGameTime()
    {
        ulong serverTime = this.GetServerTime();
        UMessage message = new UMessage();
        message.WriteHead(2, 3);
        message.WriteUInt32(0);
        message.WriteUInt64(serverTime);
        SingletonForMono<NetWorkModule>.Instance.SendImmediate(message);
    }

    private void UpdateNowMsecond()
    {
        if (this.m_qwClientMsec == 0)
        {
            this.m_qwClientMsec = this.GetNowMsecond();
        }
    }
}

