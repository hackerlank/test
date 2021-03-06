using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;
public class NGUI_Bag : NGUI_Base
{
    public const int TOTAL_ITEM_COUNT = 126;
    public const int PAGE_ITEM_COUNT = 13;

    public int curPage = 0;

    private List<GameObject> mLeftTabObjects = new List<GameObject>();
    private List<GameObject> mItemSaveObjects = new List<GameObject>();
    private List<GameObject> mItemInfoObjects = new List<GameObject>();

    private List<UILabel> mItemNames = new List<UILabel>();
    private List<UILabel> mItemCounts = new List<UILabel>();
    private List<UILabel> mID4ItemInfos = new List<UILabel>();
    private List<UILabel> mID4ItemSaves = new List<UILabel>();

    private GameObject mTabParentObj = null;
    private GameObject mItemParentObj = null;

    private UInt32 mCurItemID = 0;

    private UILabel mDesc = null;

    public static string[] upgradeName = new string[]{ " 一等", " 二等", " 三等", " 四等", " 五等"," 六等", " 七等", " 八等", " 九等", " 十等", "十一等", "十二等", "十三等", "十四等", "十五等" };
    public override void Init()
    {
        GameObject closeObj = transform.FindChild("Close").gameObject;
        if (closeObj != null)
            UIEventListener.Get(closeObj).onClick = OnDlgClose;

        mTabParentObj = transform.FindChild("Tab/Scroll View/UIGrid").gameObject;
        if (mTabParentObj != null)
        {
            mLeftTabObjects.Clear();
            for (int i = 0; i < TOTAL_ITEM_COUNT/PAGE_ITEM_COUNT + 1; ++i)
            {
                 GameObject itemObj = null;
                 itemObj = NGUITools.AddChild(mTabParentObj, (GameObject)(Resources.Load("Prefabs/bag/bagtab")));
                 if (itemObj != null)
                 {
                     mLeftTabObjects.Add(itemObj);

                     UIEventListener.Get(itemObj).onClick = onPanelSelect;
                     UILabel lbl = itemObj.transform.FindChild("Label").GetComponent<UILabel>();
                     if (lbl != null)
                     {
                         lbl.text = (i+1).ToString();
                         
                     }
                 }
            }

        }

        mItemParentObj = transform.FindChild("List/Scroll View/UIGrid").gameObject;
        if (mItemParentObj != null)
        {
            mItemSaveObjects.Clear();
            mItemInfoObjects.Clear();
            mItemNames.Clear();
            mItemCounts.Clear();
            mID4ItemInfos.Clear();
            mID4ItemSaves.Clear();

            for (int i = 0; i < PAGE_ITEM_COUNT; ++i)
            {
                GameObject itemObj = null;
                itemObj = NGUITools.AddChild(mItemParentObj, (GameObject)(Resources.Load("Prefabs/bag/bagitem")));
                if (itemObj != null)
                {
                    GameObject infoobj = itemObj.transform.FindChild("btninfo").gameObject;
                    GameObject saveobj = itemObj.transform.FindChild("btnsave").gameObject;


                    if (infoobj != null)
                    {
                        UIEventListener.Get(infoobj).onClick = onShowItemInfo;
                        mItemInfoObjects.Add(infoobj);

                        UILabel iteminfo = infoobj.transform.FindChild("ID").GetComponent<UILabel>();
                        if (iteminfo != null)
                            mID4ItemInfos.Add(iteminfo);

                    }

                    if (saveobj != null)
                    {
                        UIEventListener.Get(saveobj).onClick = onSaveItem;
                        mItemSaveObjects.Add(saveobj);

                        UILabel itemsave = saveobj.transform.FindChild("ID").GetComponent<UILabel>();
                        if (itemsave != null)
                            mID4ItemSaves.Add(itemsave);
                    }

                    UILabel name = itemObj.transform.FindChild("name").gameObject.GetComponent<UILabel>();
                    if (name != null)
                    {
                        name.text = "道具名称";
                        mItemNames.Add(name);
                    }

                    UILabel count = itemObj.transform.FindChild("count").gameObject.GetComponent<UILabel>();
                    if (count != null)
                    {
                        count.text = "999";
                        mItemCounts.Add(count);
                    }

                }
            }
        }

        //默认显示第一页
        UpdateItem(1);

    }

    public void onPanelSelect(GameObject item)
    {
        UILabel lbl = item.transform.FindChild("Label").GetComponent<UILabel>();
        if (lbl != null)
        {
            curPage = 0;
            if (int.TryParse(lbl.text, out curPage))
            {
                UpdateItem(curPage);
            }

        }
    }

    public void UpdateItem()
    {
        UpdateItem(curPage);
    }

    //清理
    public void ClearPack()
    {
        for (int i = 0; i < PAGE_ITEM_COUNT; ++i)
        {
            if ((i < mItemNames.Count) && (i < mItemCounts.Count) && (i < mID4ItemInfos.Count) && (i < mID4ItemSaves.Count) && (i < mItemSaveObjects.Count) && (i < mItemInfoObjects.Count))
            {
                mItemNames[i].text = "";
                mItemCounts[i].text = "";
                mID4ItemInfos[i].text = "";
                mID4ItemSaves[i].text = "";
                mItemSaveObjects[i].SetActive(false);
                mItemInfoObjects[i].SetActive(false);
            }

        }
    }
    public void UpdateItem(int page)
    {
        ClearPack();

        List<StObject> obj = new List<StObject>(GameManager.Instance.MainPlayer.mItems.Values);

        int havepages = obj.Count / PAGE_ITEM_COUNT ;
        if (obj.Count > havepages * PAGE_ITEM_COUNT)
            havepages += 1;

        for (int pageIndex = 0; (pageIndex < havepages) && (pageIndex < mLeftTabObjects.Count); ++pageIndex)
        {
            mLeftTabObjects[pageIndex].SetActive(true);
        }

        for (int pageIndex = havepages; pageIndex < mLeftTabObjects.Count; ++pageIndex )
        {
            mLeftTabObjects[pageIndex].SetActive(false);
        }

        for (int i = 0; i < PAGE_ITEM_COUNT; ++i)
        {
            int index = (page - 1) * PAGE_ITEM_COUNT + i;
            if ((index < obj.Count) && (i < mItemNames.Count) && (i < mItemCounts.Count) && (i < mID4ItemInfos.Count) && (i < mID4ItemSaves.Count) && (i < mItemSaveObjects.Count) && (i < mItemInfoObjects.Count))
            {
                mItemNames[i].text = GetItemName(obj[index]);
                mItemCounts[i].text = obj[index].dwNum.ToString();
                mID4ItemInfos[i].text = obj[index].qwThisID.ToString();
                mID4ItemSaves[i].text = obj[index].qwThisID.ToString();
                mItemInfoObjects[i].SetActive(true);
                mItemSaveObjects[i].SetActive(false);
            }
        }
    }

    //得到道具名称
    public static string GetItemName(StObject stObj)
    {
        string result = "";
        if (stObj != null)
        {
            if (IsEquip(stObj))
            {
                uint starCount = stObj.upgrade;
                if (starCount > 15)
                    starCount -= 15;
                result = stObj.strName + " (" + starCount.ToString() + "星)";
            }
            else if (IsMaterial(stObj))
            {
                if (stObj.upgrade < 15)
                    result = stObj.strName + upgradeName[stObj.upgrade];
                else
                    result = stObj.strName;
            }
            else
                result = stObj.strName;
        }

        return result;
    }
    public static bool IsEquip(StObject stObj)
    {
        t_object_config rec = t_object_config.GetConfig(stObj.dwObjectID);
        if (rec != null)
        {
            if ((rec.m_type >= (uint)enumItemType.ItemType_ClothBody) && (rec.m_type <= (uint)enumItemType.ItemType_Fing))
                return true;

            if ((rec.m_type >= (uint)enumItemType.ItemType_LiRen) && (rec.m_type <= (uint)enumItemType.ItemType_HuXinMirror))
                return true;

            if ((rec.m_type >= (uint)enumItemType.ItemType_Gem) && (rec.m_type <= (uint)enumItemType.ItemType_SwordFlag))
                return true;
        }
        return false;
    }

    public static bool IsMaterial(StObject stObj)
    {
        t_object_config rec = t_object_config.GetConfig(stObj.dwObjectID);
        if (rec != null)
        {
            if( (rec.m_type ==  (uint)enumItemType.ItemType_Resource)
			&& ( (rec.m_id== 506 )
			|| (rec.m_id== 507)
			|| (rec.m_id== 516)
			|| (rec.m_id== 517)
			|| (rec.m_id== 526)
			|| (rec.m_id== 527)
			|| (rec.m_id== 536)
			|| (rec.m_id== 537)
			|| (rec.m_id== 1507)  //tim_nm
			|| (rec.m_id== 1517)
			|| (rec.m_id== 1527)
			|| (rec.m_id== 1537)
			|| (rec.m_id== 1547)
			|| rec.m_id== 40506 || rec.m_id== 40507 
			|| rec.m_id== 40516 || rec.m_id== 40517 || rec.m_id== 40526
			|| rec.m_id== 40527 || rec.m_id== 40536 
			|| rec.m_id== 40537 || rec.m_id== 40546 || rec.m_id== 40547
			|| (rec.m_id== 546)
			|| (rec.m_id== 957)
			|| (rec.m_id== 547)
			|| (rec.m_id== 877)
			|| (rec.m_id== 2270) ))
            {
                return true;
            }
        }

        return false;

    }

    public void onShowItemInfo(GameObject item)
    {
        UILabel lbl = item.transform.FindChild("ID").GetComponent<UILabel>();
        if (lbl != null)
        {
            uint id = 0;
            if (uint.TryParse(lbl.text, out id))
            {
                Util.Log("显示道具信息ID: " + id);
                NGUIManager.Instance.AddByName<NGUI_Tooltip>(NGUI_UI.NGUI_Tooltip, NGUIShowType.ONLYONE, delegate(NGUI_Tooltip script)
                {
                    script.Init();
                    script.InitItemData(id);
                });
            }
        }
    }

    public void onSaveItem(GameObject item)
    {
        UILabel lbl = item.transform.FindChild("ID").GetComponent<UILabel>();
        if (lbl != null)
        {
            uint id = 0;
            if (uint.TryParse(lbl.text, out id))
            {
                Util.Log("寄存道具信息ID: " + id);

                UMessage message = new UMessage();

                stReqVipMoveObjectToCS_CS cmd = new stReqVipMoveObjectToCS_CS();
                cmd.thisID = id;
                if (message.WriteStruct<stReqVipMoveObjectToCS_CS>(cmd))
                    NetWorkModule.Instance.SendImmediate(message);
            }
        }
    }

    public void OnDlgClose(GameObject iterm)
    {
        NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_Bag);
    }


}
