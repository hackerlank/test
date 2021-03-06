using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;
public class NGUI_BuyGiftBag : NGUI_Base
{
    public GameObject descObj = null;
    public UISprite mBackground;
    //道具ID
    public uint mCurItemID = 0;
    //需要点数
    public uint mNeedPoint = 0;

    public UITextList mDesc;
    public UIScrollBar mSB;
    public override void Init()
    {
        //控件初始化
        descObj = transform.FindChild("Content/Panel/TextField").gameObject;
        if (descObj != null)
            mDesc = descObj.GetComponent<UITextList>();

        GameObject sbObj = transform.FindChild("Content/Panel/TextField/Scroll Bar").gameObject;
        if (sbObj != null)
            mSB = sbObj.GetComponent<UIScrollBar>();
        
        GameObject closeObj = transform.FindChild("Content/Panel/Close").gameObject;
        if (closeObj != null)
            UIEventListener.Get(closeObj).onClick = OnDlgClose;

        GameObject bkObj = transform.FindChild("Content/Sprite").gameObject;
        if (bkObj != null)
            mBackground = bkObj.GetComponent<UISprite>();

        GameObject buyObj = transform.FindChild("btn").gameObject;
        if (buyObj != null)
            UIEventListener.Get(buyObj).onClick = onBuyGiftBag;

    }

    public void InitItem(uint itemID)
    {
        mCurItemID = itemID;
        GiftBagData giftbagdata = ConfigManager.Instance.giftbagConfig.GetConfigByObjectID(mCurItemID);
        if (giftbagdata != null)
        {
            mNeedPoint = giftbagdata.point;
            string desc = giftbagdata.desc;
            if (mDesc != null)
            {
                int countPerLine = 20;

                int i = 0;
                for (i = 0; i < desc.Length / countPerLine; ++i)
                    mDesc.Add(desc.Substring(i * countPerLine, countPerLine));

                if (desc.Length > i * countPerLine)
                {
                    mDesc.Add(desc.Substring(i * countPerLine, desc.Length-i*countPerLine));
                }

                if (mSB != null)
                    mSB.value = 0;
            }
        }
    }
    
    public void OnDlgClose(GameObject iterm)
    {
        Util.Log("关闭NguiMsgBox");
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_BuyGiftBag);
    }

    public void onBuyGiftBag(GameObject item)
    {
        if (mCurItemID != 0)
        {
            if (GameManager.Instance.MainPlayer.pointCardInfo != null)
            {
                if ((mNeedPoint > 0) && (GameManager.Instance.MainPlayer.pointCardInfo.dwPoint < mNeedPoint))
                {
                    MainModule.Instance.uiMainWindow.AddRedTip("点数不足，无法购买!");
                    return;
                }
            }
            Debug.Log("购买礼包！: " + mCurItemID);
            MainModule.Instance.uiMainWindow.AddGreenTip("购买礼包!");
            UMessage message = new UMessage();
            stBuyGiftBagUserCmd cmd = new stBuyGiftBagUserCmd();
            cmd.bagID = mCurItemID;
            if (message.WriteStruct<stBuyGiftBagUserCmd>(cmd))
                NetWorkModule.Instance.SendImmediate(message);
        }
    }

}
