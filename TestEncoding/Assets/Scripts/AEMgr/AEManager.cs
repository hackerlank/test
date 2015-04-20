using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AE_Effect
{
    public class AEManager
    {
        private static AEManager instance = null;
        public static AEManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AEManager();
                }
                return instance;
            }
        }


        //设置ngui根节点
        private GameObject nguiRoot;

        public void SetNguiRoot(GameObject obj)
        {
            nguiRoot = obj.transform.FindChild("Anchor").gameObject;
        }

        //private string strNone = "";

        private Dictionary<string, AEAssetItemMgr> aeAssetItemDic = new Dictionary<string, AEAssetItemMgr>();
        public Dictionary<string, AEAssetItemMgr> AEAssetItemDic
        {
            get { return aeAssetItemDic; }
        }




        public void AddAENGUIAndData(string strPack, string strAsset, Action<AfterEffectAnimation> callBack)//加载资源文件 默认实例化一次 ngui根下 返回动画数据
        {
            AddAENGUI(strPack, strAsset, delegate(AEAssetItem item)
            {
                item.gameObject.transform.localPosition = Vector3.one;
                AfterEffectAnimation arAnimations = item.gameObject.GetComponentInChildren<AfterEffectAnimation>();
                string strName = arAnimations.dataFileName;
                AEAssetHelp.Instance.LoadAssetAEData(strName, delegate(AESpriteScriptable data)
                {
                    arAnimations.InitFootageSprite(AESpriteScriptable.CloneData(data));
                    arAnimations.GoToAndPlay(0);
                    callBack(arAnimations);
                });
            });
        }
        public void AddAENGUIAndData(string strPack, string strAsset, Action<AEAssetItem> callBack)//加载资源文件 默认实例化一次 ngui根下 返回动画数据
        {
            AddAENGUI(strPack, strAsset, delegate(AEAssetItem item)
            {
                AfterEffectAnimation arAnimations = item.gameObject.GetComponentInChildren<AfterEffectAnimation>();
                string strName = arAnimations.dataFileName;
                AEAssetHelp.Instance.LoadAssetAEData(strName, delegate(AESpriteScriptable data)
                {
                    arAnimations.InitFootageSprite(AESpriteScriptable.CloneData(data));
                    arAnimations.GoToAndPlay(0);
                    callBack(item);
                });
            });
        }
        public AfterEffectAnimation AddAEData(AEAssetItem item, Action<AESpriteScriptable> callback)
        {
            AfterEffectAnimation arAnimations = item.gameObject.GetComponentInChildren<AfterEffectAnimation>();
            string strName = arAnimations.dataFileName;
            AEAssetHelp.Instance.LoadAssetAEData(strName, delegate(AESpriteScriptable data)
            {
                arAnimations.InitFootageSprite(AESpriteScriptable.CloneData(data));
                if(callback!=null)
                callback(data);
            });
            return arAnimations;
        
        
        }
        public void AddAENGUI(string strPack, string strAsset, Action<AEAssetItem> callBack)//加载资源文件 默认实例化一次 ngui根下
        {
            AEAssetItemMgr aeAssetItemMgr = AddNewItem(strPack, strAsset, callBack);
            if (!isNeedLoadNewAEPrefab) return;//已经向网络请求过了

            aeAssetItemMgr.parent = nguiRoot.transform;
            LoadNewAEPrefab(aeAssetItemMgr);
        }



        //public void Add(string strPack, string strAsset, Action<AEAssetItem> callBack)//加载资源文件 默认实例化一次
        //{
        //    AEAssetItemMgr aeAssetItemMgr = AddNewItem(strPack, strAsset, callBack);
        //    if (!isNeedLoadNewAEPrefab) return;//已经向网络请求过了
        //    LoadNewAEPrefab(aeAssetItemMgr);
        //}

        private bool isNeedLoadNewAEPrefab = false;//是否需要加载新的prefab
        private void LoadNewAEPrefab(AEAssetItemMgr aeAssetItemMgr)
        {
            AEAssetHelp.Instance.LoadAsset(aeAssetItemMgr, delegate(AEAssetItemMgr itemMgr)
            {
                if (itemMgr.prefab == null)
                {
                    PlayerHandle.Error(this, "Add", "加载不到资源:" + itemMgr.strPack+"   "+itemMgr.strAsset);
                    return;
                }

                if (CheckObjectHasRemoved(itemMgr))
                    return;

                Instance_All_AEAssetItem(itemMgr);

                if (itemMgr.itemMgrCallBack != null)
                {
                    itemMgr.itemMgrCallBack(itemMgr);
                    itemMgr.itemMgrCallBack = null;
                }
            });
        }


        private void Instance_All_AEAssetItem(AEAssetItemMgr itemMgr)
        {
            if (itemMgr.prefab == null)
            {
                PlayerHandle.Warning("AE特效资源:" + itemMgr.strPack + "   " + itemMgr.strAsset);
                return;
            }

            for (int i = 0; i < itemMgr.aeAssetItemList.Count; i++)
            {
                Instance_AEAssetItem(itemMgr.aeAssetItemList[i], itemMgr);
            }
        }

        private void Instance_AEAssetItem(AEAssetItem value, AEAssetItemMgr itemMgr)
        {
            if (value.itemCallBack == null) return;
            if (value.isHaveInited) return;

            value.isHaveInited = true;
            value.prefab = itemMgr.prefab;

            value.gameObject = GameObject.Instantiate(itemMgr.prefab) as GameObject;
            value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);

            SaveNewObjectProperty(value);

            if (itemMgr.parent != null)
                value.gameObject.transform.parent = itemMgr.parent;

            SetNewObjectProperty(value.itemCallBack, value);
        }

        private void CheckHasAEAssetItemPrefab(string strPack)
        {
            isNeedLoadNewAEPrefab = !aeAssetItemDic.ContainsKey(strPack);
        }


        private AEAssetItemMgr AddNewItem(string strPack, string strAsset, Action<AEAssetItem> callBack)
        {
            string strFullName = strPack + strAsset;

            CheckHasAEAssetItemPrefab(strFullName);
            AEAssetItemMgr aeAssetItemMgr = null;

            AEAssetItem newAEAssetItem = new AEAssetItem();
            newAEAssetItem.itemCallBack = callBack;
            newAEAssetItem.isHaveInited = false;
            newAEAssetItem.strPack = strPack;
            newAEAssetItem.strAsset = strAsset;


            if (aeAssetItemDic.ContainsKey(strFullName))//已经有过网络加载请求包 
            {
                //Debug.Log("已经有过网络加载请求包");
                aeAssetItemMgr = aeAssetItemDic[strFullName];

                aeAssetItemMgr.aeAssetItemList.Add(newAEAssetItem);

                Instance_All_AEAssetItem(aeAssetItemMgr);
            }
            else
            {
                aeAssetItemMgr = new AEAssetItemMgr();
                aeAssetItemMgr.strPack = strPack;
                aeAssetItemMgr.strAsset = strAsset;

                aeAssetItemDic.Add(strFullName, aeAssetItemMgr);

                aeAssetItemMgr.aeAssetItemList.Add(newAEAssetItem);
            }

            return aeAssetItemMgr;
        }

        private Vector3 vP, vS, vR;
        private void SaveNewObjectProperty(AEAssetItem value)
        {
            vP = value.gameObject.transform.localPosition;
            vS = value.gameObject.transform.localScale;
            vR = value.gameObject.transform.localEulerAngles;
        }

        private void SetNewObjectProperty(Action<AEAssetItem> callBack, AEAssetItem value)
        {
            value.gameObject.transform.localPosition = vP;
            value.gameObject.transform.localScale = vS;
            value.gameObject.transform.localEulerAngles = vR;

            AfterEffectAnimation ae_effect = value.gameObject.transform.GetComponentInChildren<AfterEffectAnimation>();
            if (ae_effect != null)
                ae_effect.UpdateColor();

            if (callBack != null)
                callBack(value);
        }

        private bool CheckObjectHasRemoved(AEAssetItemMgr item)//玩家快速切换界面  还没初始化完成 界面就删了 
        {
            if (!item.isRemoved)
                return false;

            DisposeAllItemData(item);

            return true;
        }

        public void DeleteByPackAsset(string strPack)
        {
            DeleteByName(strPack);
        }

        public void DeleteByPackAssetWWW(string strPack)
        {
            AEAssetItemMgr item = DeleteByName(strPack);
            if (item == null) return;

            AEAssetHelp.Instance.DeleteAssetBundleAndWWW(strPack);
        }

        public void DeleteByNameWWW(string strPack)//删除解压缩文件 并删除www文件
        {
            AEAssetItemMgr item = DeleteByName(strPack);
           if (item == null) return;

           AEAssetHelp.Instance.DeleteAssetBundleAndWWW(strPack);
        }

        public void DeleteByObjListAssetBundle(string strPack, AEAssetItem aeItem)//根据加载的列表数 剩余为零自动 删除解压缩文件
        {
            AEAssetItemMgr item = DeleteByObjListItem(strPack, aeItem);

            if (item == null) return;

            if (item.aeAssetItemList.Count != 0) return;

            DeleteByName(strPack);
        }

        public void DeleteByObjListWWW(string strPack, AEAssetItem aeItem)//根据加载的列表数 剩余为零自动 删除解压缩文件 并删除www文件
        {
            AEAssetItemMgr item = DeleteByObjListItem(strPack, aeItem);

            if (item == null) return;

            if (item.aeAssetItemList.Count != 0) return;

            DeleteByNameWWW(strPack);
        }

        public AEAssetItemMgr DeleteByObjListItem(string strPack, AEAssetItem aeItem)////删除列表对象
        {
            if (aeItem == null) return null;

            if (!aeAssetItemDic.ContainsKey(strPack))
                return null;

            AEAssetItemMgr item = aeAssetItemDic[strPack];

            if (!item.aeAssetItemList.Contains(aeItem)) return null;
            item.aeAssetItemList.Remove(aeItem);

            if (aeItem.gameObject != null)
            {
                GameObject.Destroy(aeItem.gameObject);
            }
            aeItem.prefab = null;
            aeItem = null;

            return item;
        }

        public AEAssetItemMgr DeleteByName(string strPack)//默认删除解压缩文件 不删除www文件
        {


            if (!aeAssetItemDic.ContainsKey(strPack))
                return null;

            AEAssetItemMgr itm = aeAssetItemDic[strPack];
            itm.isRemoved = true;
            aeAssetItemDic.Remove(strPack);
            DisposeAllItemData(itm);
            
            AEAssetHelp.Instance.DeleteAssetBundleAndWWW(strPack);
            return itm;
        }

        public bool ContainAEByName(string strName)
        {
            return aeAssetItemDic.ContainsKey(strName);
        }


        public AEAssetItem GetFirstAEAssetItem(string strName)
        {
            if (!aeAssetItemDic.ContainsKey(strName))
                return null;

            AEAssetItemMgr itmMgr = aeAssetItemDic[strName];
            if (itmMgr.aeAssetItemList.Count == 0)
                return null;

            return itmMgr.aeAssetItemList[0];
        }

        public void DeleteAllAEBeside(List<string> AEitemkeys)//删除给定以外的所有ae特效
        {
            List<string> Keylist = new List<string>();
        

            foreach (KeyValuePair<string, AEAssetItemMgr> item in aeAssetItemDic) 
            {
                Keylist.Add(item.Key);
            }

            for (int i = 0; i < Keylist.Count; i++)
            {
                string str = Keylist[i];
                if (AEitemkeys.Contains(str)) continue;
                DeleteByName(str);
            }

            Keylist.Clear();



            //int nDeleteCount = aeAssetItemDic.Count;
            //while (nDeleteCount > 0)
            //{
            //    nDeleteCount = aeAssetItemDic.Count;
            //    foreach (KeyValuePair<string, AEAssetItemMgr> item in aeAssetItemDic)
            //    {
            //        nDeleteCount--;

            //        if (!AEitemkeys.Contains(item.Key))
            //        DeleteByName(item.Key);
            //        break;
            //    }
            //}
        }

        public void DeleteAllAE()//删除所有ae特效
        {
            int nDeleteCount = aeAssetItemDic.Count;
            while (nDeleteCount > 0)
            {
                nDeleteCount = aeAssetItemDic.Count;
                foreach (KeyValuePair<string, AEAssetItemMgr> item in aeAssetItemDic)
                {
                    nDeleteCount--;
                   
               
                    DeleteByName(item.Key);
                    
                    break;
                }
            }
        }

        public static void DisposeAllItemData(AEAssetItemMgr itm)
        {
            for (int i = 0; i < itm.aeAssetItemList.Count; i++)
            {
                AEAssetItem ae = itm.aeAssetItemList[i];
                if (ae.gameObject != null)
                {
                    GameObject.Destroy(ae.gameObject);
                }
                ae.prefab = null;
                ae = null;
            }

            itm.aeAssetItemList.Clear();
            itm.prefab = null;
        }

    }
}
