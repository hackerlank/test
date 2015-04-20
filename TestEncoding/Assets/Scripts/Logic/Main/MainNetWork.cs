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
public class MainNetWork : NetWorkBase
{
    public override void Initialize()
    {
        RegisterMsg();
    }

    public override void RegisterMsg()
    {
        //基本信息
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stSendUserInfoSC()), OnRetUserInfo);
        //称号变更
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stSendOfficialSC()), OnReturnOfficialTitle);

        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stAddObjectPropertyUserCmd()), OnAddObjectPropertyUserCmd);
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stAddObjectListPropertyUserCmd()), OnAddObjectListPropertyUserCmd);
        //点数
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stReturnRequestPoint()), OnReturnRequestPoint);
        //VIP
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stSendVipInfo_SC()), OnReturnVipInfo);
        //异端登陆通知
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(new stUserReLoginCmd()), OnUserReLoginCmd);
    }


    /// <summary>
    /// 收到主人物基本信息
    /// </summary>
    /// <param name="msg"></param>
    public void OnRetUserInfo(UMessage msg)
    {

        stSendUserInfoSC st = new stSendUserInfoSC();
        msg.ReadStruct<stSendUserInfoSC>(st);

        if (GameManager.Instance.MainPlayer != null)
            GameManager.Instance.MainPlayer.baseUserInfo = st;

        MainModule.Instance.uiMainWindow.RefreshBaseInfo();
        Util.Log("@@区： " + st.zoneName + " 国家 " + st.countryName + " 头像: " + st.face + " 称号： " + st.chenghao);


    }


    /// <summary>
    /// 添加单个道具
    /// </summary>
    /// <param name="msg"></param>
    public void OnAddObjectPropertyUserCmd(UMessage msg)
    {
        stAddObjectPropertyUserCmd st = new stAddObjectPropertyUserCmd();
        msg.ReadStruct<stAddObjectPropertyUserCmd>(st);

        StObject obj = new StObject();
        msg.ReadStruct<StObject>(obj);

        GameManager.Instance.MainPlayer.UpdateItem(obj);
    }

    /// <summary>
    /// 批量添加道具
    /// </summary>
    /// <param name="msg"></param>
    public void OnAddObjectListPropertyUserCmd(UMessage msg)
    {
        stAddObjectListPropertyUserCmd st = new stAddObjectListPropertyUserCmd();
        msg.ReadStruct<stAddObjectListPropertyUserCmd>(st);

        for (int i = 0; i < st.num; ++i)
        {
            StObject obj = new StObject();

            msg.ReadStruct<StObject>(obj);

            GameManager.Instance.MainPlayer.UpdateItem(obj);
        }
            
    }

    /// <summary>
    /// 点数返回
    /// </summary>
    /// <param name="msg"></param>
    public void OnReturnRequestPoint(UMessage msg)
    {
        stReturnRequestPoint st = new stReturnRequestPoint();
        msg.ReadStruct<stReturnRequestPoint>(st);

        GameManager.Instance.MainPlayer.pointCardInfo = st;
        Util.Log("返回点数信息： " + st.dwPoint);
    }

    public void OnReturnVipInfo(UMessage msg)
    {
        stSendVipInfo_SC st = new stSendVipInfo_SC();
        msg.ReadStruct<stSendVipInfo_SC>(st);

        GameManager.Instance.MainPlayer.vipInfo = st;
        Util.Log("返回VIP信息，VIP等级: " + st.level + "使用彩信次数： " + st.usedFreeCS);
    }

    public void OnUserReLoginCmd(UMessage msg)
    {
        NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
        {
            script.Init();
            script.InitDesc("此账号正在被登录！");
        });
    }

    public void OnReturnOfficialTitle(UMessage msg)
    {
        stSendOfficialSC st = new stSendOfficialSC();
        msg.ReadStruct<stSendOfficialSC>(st);

        
        Debug.Log("@@官员变更：  " + st.chenghao);
        if (GameManager.Instance.MainPlayer.baseUserInfo != null)
        {
            GameManager.Instance.MainPlayer.baseUserInfo.chenghao = st.chenghao;
        }

        MainModule.Instance.uiMainWindow.RefreshBaseInfo();
    }
}
