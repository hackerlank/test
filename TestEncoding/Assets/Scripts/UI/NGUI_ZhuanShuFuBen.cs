using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;


public class NGUI_ZhuanShuFuBen : NGUI_Base
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

   
    public void OnDlgClose(GameObject iterm)
    {
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_ZhuanShuFuBen);
    }

}
