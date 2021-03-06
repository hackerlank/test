using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;
public class NGUI_Tooltip : NGUI_Base
{
    public GameObject descObj = null;
    public UISprite mBackground;

    public UILabel mName;
    public UILabel mBind;
    public UILabel mNeedLevel;
    public UITextList mDesc;
    public override void Init()
    {
        //控件初始化
        GameObject nameObj = transform.FindChild("Content/Panel/name").gameObject;
        if (nameObj != null)
            mName = nameObj.GetComponent<UILabel>();

        GameObject bindObj = transform.FindChild("Content/Panel/bind").gameObject;
        if (bindObj != null)
            mBind = bindObj.GetComponent<UILabel>();

        GameObject levelObj = transform.FindChild("Content/Panel/level").gameObject;
        if (levelObj != null)
            mNeedLevel = levelObj.GetComponent<UILabel>();

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

    public void InitItemData(uint dwThisiD)
    {
        //数据初始化
        StObject item = GameManager.Instance.MainPlayer.GetObjectByThisID(dwThisiD);
        if (item != null)
        {
            t_object_config rec = t_object_config.GetConfig(item.dwObjectID);

            if (mName != null)
                mName.text = NGUI_Bag.GetItemName(item);

            if (mBind != null)
            {
                if (item.bind != 0)
                    mBind.text = "已绑定";
                else
                    mBind.text = "未绑定";
            }

            if (mNeedLevel != null)
            {
                if (rec != null)
                {
                    if (rec.m_needlevel > 0)
                        mNeedLevel.text = "需要等级 " + rec.m_needlevel.ToString();
                    else
                        mNeedLevel.text = "";
                }
            }

            if (mDesc != null)
            {
                if (rec != null)
                {
                    mDesc.Add(rec.m_desc);
                    if (mBackground != null)
                        mBackground.GetComponent<UIWidget>().height = 200 + rec.m_desc.Length;// / 20 * 20;
                }
            }
            
        }
    }



    public void OnDlgClose(GameObject iterm)
    {
        Util.Log("关闭NguiTooltip");
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_Tooltip);
    }

}
