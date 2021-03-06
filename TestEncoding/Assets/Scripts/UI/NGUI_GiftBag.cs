using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;
public class NGUI_GiftBag : NGUI_Base
{
    private List<GameObject> mGiftBagObjects = new List<GameObject>();

    private GameObject mTableObj = null;

    private UInt32 mCurItemID = 0;

    private UILabel mDesc = null;
    public override void Init()
    {
        mTableObj = transform.FindChild("Scroll View/UITable").gameObject;
        mDesc = transform.FindChild("Desc").gameObject.GetComponent<UILabel>();
        if (mTableObj != null)
        {
            mGiftBagObjects.Clear();
            for (int i = 0; i < ConfigManager.Instance.giftbagConfig.GetGiftBagCount(); ++i)
            {
                 GameObject itemObj = null;
                 itemObj = NGUITools.AddChild(mTableObj, (GameObject)(Resources.Load("Prefabs/giftitem")));
                 if (itemObj != null)
                 {
                     GiftBagData giftbagdata = ConfigManager.Instance.giftbagConfig.GetConfigByIndex(i);

                     if (giftbagdata != null)
                     {
                         UIEventListener.Get(itemObj).onClick = onPanelSelect;
                         UILabel lbl = itemObj.transform.FindChild("ID").GetComponent<UILabel>();
                         if (lbl != null)
                         {

                             lbl.text = giftbagdata.id.ToString();
                         }

                         //名字
                         UILabel name = itemObj.transform.FindChild("name").GetComponent<UILabel>();
                         if (name != null)
                         {
                             t_object_config rec = t_object_config.GetConfig(giftbagdata.id);
                             if (rec != null)
                                 name.text = rec.m_name;
                             else
                                 name.text = giftbagdata.id.ToString() + "未找到";
                         }

                         //点数
                         UILabel point = itemObj.transform.FindChild("point").GetComponent<UILabel>();
                         if (point != null)
                         {
                             point.text = "点数: " + giftbagdata.point.ToString();
                         }

                         //查看
                         GameObject lookObj = itemObj.transform.FindChild("btn").gameObject;
                         if (lookObj != null)
                             UIEventListener.Get(lookObj).onClick = onLookGiftBag;
                         GameObject lookIDObj = itemObj.transform.FindChild("btn/ID").gameObject;
                         if (lookIDObj != null)
                         {
                             UILabel lookID = lookIDObj.GetComponent<UILabel>();
                             if (lookID != null)
                             {
                                 lookID.text = giftbagdata.id.ToString();
                             }
                         }
                     }
                 }
            }

        }

        GameObject closeObj = transform.FindChild("Close").gameObject;
        if (closeObj != null)
            UIEventListener.Get(closeObj).onClick = OnDlgClose;


    }

    public void onPanelSelect(GameObject item)
    {
        UILabel lbl = item.transform.FindChild("ID").GetComponent<UILabel>();
        if (lbl != null)
        {
            mCurItemID = 0;
            if (UInt32.TryParse(lbl.text, out mCurItemID))
            {
                Debug.Log("当前选中道具ID: " + mCurItemID);
                if (mDesc != null)
                {
                    mDesc.text = ConfigManager.Instance.giftbagConfig.GetDescByID(mCurItemID);
                }
            }
            else
            {
                Debug.LogError("@@购买礼包中存在不能解析的道具ID");
            }
            
        }
    }

    public void onBuyGiftBag(GameObject item)
    {
        if (mCurItemID != 0)
        {
            Debug.Log("购买礼包！: " + mCurItemID);
            UMessage message = new UMessage();
            stBuyGiftBagUserCmd cmd = new stBuyGiftBagUserCmd();
            cmd.bagID = mCurItemID;
            if (message.WriteStruct<stBuyGiftBagUserCmd>(cmd))
                NetWorkModule.Instance.SendImmediate(message);
        }
    }

    public void onLookGiftBag(GameObject item)
    {
        if (item != null)
        {
            GameObject idObj = item.transform.FindChild("ID").gameObject;
            if (idObj != null)
            {
                UILabel id = idObj.GetComponent<UILabel>();
                uint index = 0;
                if (uint.TryParse(id.text, out index))
                {
                    //每次查看请求下点数
                    UMessage message = new UMessage();
                    stRequestPoint cmd = new stRequestPoint();
                    if (message.WriteStruct<stRequestPoint>(cmd))
                        NetWorkModule.Instance.SendImmediate(message);

                    mCurItemID = index;
                    NGUIManager.Instance.AddByName<NGUI_BuyGiftBag>(NGUI_UI.NGUI_BuyGiftBag, NGUIShowType.ONLYONE, delegate(NGUI_BuyGiftBag script)
                    {
                        script.Init();
                        script.InitItem(mCurItemID);
                    });
                }
            }

        }
    }

    public void OnDlgClose(GameObject iterm)
    {
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_GiftBag);
    }

}
