using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;

public enum enumAskType
{
    AdkType_Default,
    AskType_SendCaiXin,
    AskType_SendWorld,
}

public class NGUI_Ask : NGUI_Base
{
    public GameObject descObj = null;
    public UISprite mBackground;

    public UITextList mDesc;
    public enumAskType mAskType;
    public string mParam;
    public override void Init()
    {
        //控件初始化
        descObj = transform.FindChild("Content/Panel/TextField").gameObject;
        if (descObj != null)
            mDesc = descObj.GetComponent<UITextList>();

        GameObject okObj = transform.FindChild("Content/Panel/OK").gameObject;
        if (okObj != null)
            UIEventListener.Get(okObj).onClick = OnOK;

        GameObject cancelObj = transform.FindChild("Content/Panel/Cancel").gameObject;
        if (cancelObj != null)
            UIEventListener.Get(cancelObj).onClick = OnCancel;

        GameObject bkObj = transform.FindChild("Content/Sprite").gameObject;
        if (bkObj != null)
            mBackground = bkObj.GetComponent<UISprite>();

    }

    public void InitDesc(string desc, enumAskType askType = enumAskType.AdkType_Default, string param = "")
    {
        if (mDesc != null)
            mDesc.Add(desc);

        mAskType = askType;
        mParam = param;
    }
    
    public void OnOK(GameObject iterm)
    {
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_Ask);
        if (mAskType == enumAskType.AskType_SendCaiXin)
        {
            ChatModule.Instance.CNetWork.ReqChannleChat(mParam, enumChatType.CHAT_TYPE_COLORWORLD);
        }
        else if (mAskType == enumAskType.AskType_SendWorld)
        {
            ChatModule.Instance.CNetWork.ReqChannleChat(mParam, enumChatType.CHAT_TYPE_WORLD);
        }
    }

    public void OnCancel(GameObject iterm)
    {
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_Ask);
    }

}
