using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;

public enum enumMsgType
{
    MsgType_Default,
    MsgType_NewVersion,     //有新版本
    MsgType_LostConnection,//失去连接
}

public class NGUI_MsgBox : NGUI_Base
{
    public GameObject descObj = null;
    public UISprite mBackground;

    public UITextList mDesc;
    public enumMsgType mMsgType;
    public override void Init()
    {
        //控件初始化
        descObj = transform.FindChild("Content/Panel/TextField").gameObject;
        if (descObj != null)
            mDesc = descObj.GetComponent<UITextList>();

        GameObject closeObj = transform.FindChild("Content/Panel/Close").gameObject;
        if (closeObj != null)
            UIEventListener.Get(closeObj).onClick = OnDlgClose;

        GameObject bkObj = transform.FindChild("Content/Sprite").gameObject;
        if (bkObj != null)
            mBackground = bkObj.GetComponent<UISprite>();

    }

    public void InitDesc(string desc, enumMsgType msgType = enumMsgType.MsgType_Default)
    {
        if (mDesc != null)
            mDesc.Add(desc);

        mMsgType = msgType;
    }
    
    public void OnDlgClose(GameObject iterm)
    {
        Util.Log("关闭NguiMsgBox");
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_MsgBox);
        if (mMsgType == enumMsgType.MsgType_NewVersion)
        {
            Application.OpenURL(Root.apkAddress);
            Application.Quit();
        }
        else if (mMsgType == enumMsgType.MsgType_LostConnection)
        {
            Application.Quit();
        }
        
    }

}
