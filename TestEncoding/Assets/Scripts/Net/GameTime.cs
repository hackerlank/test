using System;
using Net;
using UnityEngine;
using msg;

public class GameTime : LSingleton<GameTime> 
{
	
	private UInt64 m_qwClientMsec = 0;
	private DateTime startTime = new System.DateTime(1970, 1, 1).ToLocalTime();

	public UInt64 GetNowMsecond()
	{
		UInt64 qwResult = 0;
		qwResult = (UInt64)(System.DateTime.Now - startTime).TotalMilliseconds;
		return qwResult;
	}
	
    /// <summary>
    /// 刷新客户端时间
    /// </summary>
	private void UpdateNowMsecond()
	{
		if (0 == m_qwClientMsec)
		{
			m_qwClientMsec = GetNowMsecond();
		}
	}
	
	/// <summary>
    /// 获取间隔毫秒数
	/// </summary>
	/// <returns></returns>
	public UInt32 GetIntervalMsecond()
	{
		UInt64 qwNow = GetNowMsecond();
		if (qwNow >= m_qwClientMsec)
		{
			UInt64 qwInterval = qwNow - m_qwClientMsec;
			
			return (UInt32)(qwInterval);
		}
		
		return 0;
	}
	
	private UInt64 m_dwServerTime = 0;
	private UInt32 m_dwClientTime = 0;
	private UInt64 m_dwServerStartTime = 0;
	
    /// <summary>
    /// 获得服务器时间
    /// </summary>
    /// <returns></returns>
	public UInt64 GetServerTime()
	{
		UInt32 dwCltNowSecond = GetNowSecond();
		if (dwCltNowSecond >= m_dwClientTime)
		{
			return m_dwServerTime + dwCltNowSecond - m_dwClientTime;
		}
		
		return m_dwServerTime;
	}
	
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
	private UInt32 ConvertDateTimeInt(System.DateTime time)
	{
		UInt32 dwResult = 0;
		dwResult = (UInt32)(time - startTime).TotalSeconds;
		return dwResult;
	}
	
	public UInt32 GetNowSecond()
	{
		return ConvertDateTimeInt(System.DateTime.Now);
	}

	public void Init()
	{
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_GameTime_SC, OnServerTimeInit);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Req_UserGameTime_SC, OnServerTimeReq);
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(2, 1), OnServerTimeInit);
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(2, 2), OnServerTimeReq);

	}

    /// <summary>
    /// 网关向用户发送游戏时间
    /// </summary>
    /// <param name="message"></param>
	private void OnServerTimeInit(UMessage message)
	{
        Debug.Log("OnServerTimeInit");
        //MSG_Ret_GameTime_SC msgb = message.ReadProto<MSG_Ret_GameTime_SC>();
        //UInt32 dwNewServerTime = (UInt32)msgb.gametime;

		m_dwServerTime = message.ReadUInt64();
		m_dwClientTime = GetNowSecond(); 
	}

    /// <summary>
    /// 网关向用户请求当前游戏时间
    /// </summary>
    /// <param name="message"></param>
	private void OnServerTimeReq(UMessage message)
	{
        Debug.Log("OnServerTimeReq.." + DateTime.Now);
        UpdateNowMsecond();
        SendUserGameTime();
	}

    /// <summary>
    /// 用户向网关发送当前游戏时间
    /// </summary>
    private void SendUserGameTime()
    {
        UInt64 dwServerTime = GetServerTime();
        UMessage sendMessage = new UMessage();
        //sendMessage.WriteHead((UInt16)CommandID.MSG_Ret_UserGameTime_CS);

        /// 用户向网关发送当前游戏时间
        //const BYTE USERGAMETIME_TIMER_USERCMD_PARA  = 3;
        //struct stUserGameTimeTimerUserCmd : public stTimerUserCmd
        //{
        //    stUserGameTimeTimerUserCmd()
        //    {
        //        byParam = USERGAMETIME_TIMER_USERCMD_PARA;
        //    }

        //    DWORD dwUserTempID;			/**< 用户临时ID */
        //    QWORD qwGameTime;			/**< 用户游戏时间 */
        //};

        //sendMessage.WriteHead(MsgVar.TIME_USERCMD, MsgVar.USERGAMETIME_TIMER_USERCMD_PARA);
        //sendMessage.WriteUInt32(0);
        //sendMessage.WriteUInt64(dwServerTime);

        //NetWorkModule.Instance.Send(sendMessage);

        UMessage message = new UMessage();
        stUserGameTimeTimerUserCmd cmd = new stUserGameTimeTimerUserCmd();
        cmd.dwUserTempID = 0;
        cmd.qwGameTime = dwServerTime;
        byte[] bytes = Converter.StructToBytes(cmd);

        message.WriteStruct(cmd.byCmd, cmd.byParam, bytes);

        NetWorkModule.Instance.SendImmediate(message);
    }
}