using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using Common;

namespace AE_Effect
{
    public class AEFightSkillItemLoader
    {
        public List<AEAssetItem> _lstAEItem = new List<AEAssetItem>();
        private List<AssetRequest> _lstAERequest = new List<AssetRequest>();
        private List<AssetRequest> _lstAEDataRequest = new List<AssetRequest>();

        public void LoadAEAsset(Transform parent, Vector3 position, int inversion, uint type)
        {
            if (type == 37)
            {
                LoadAEAsset(AE_Pack.AE_warEffect_ + type + "A", type, delegate(AEAssetItem item)
                {
                    OnLoadComplete(parent, type, inversion, new Vector3(position.x, position.y, position.z - 50.0f), item,"A");
                });
                LoadAEAsset(AE_Pack.AE_warEffect_ + type + "B", type,  delegate(AEAssetItem item)
                {
                    OnLoadComplete(parent, type, inversion, position, item,"B");
                });
                return;
            }
            LoadAEAsset(AE_Pack.AE_warEffect_ + type, type, delegate(AEAssetItem item)
            {
                OnLoadComplete(parent, type, inversion, position, item,"");
            });

        }
        private void OnLoadComplete(Transform parent, uint type,  int inversion, Vector3 position, AEAssetItem item, string addPath)
        {
            if (item == null)
                return;
			if (item.prefab == null)
			{
				Debug.LogError(item.strAsset+"不存在");
                return;
			}
            _lstAEItem.Add(item);
            item.gameObject = GameObject.Instantiate(item.prefab) as GameObject;
            item.gameObject.transform.parent = parent;
            item.gameObject.transform.localScale = (inversion > 0) ? Vector3.one : new Vector3(-1, 1, 1);
            item.gameObject.transform.localPosition = position;
            item.gameObject.transform.localRotation = Quaternion.identity;
            AfterEffectAnimation arAnimations = item.gameObject.GetComponentInChildren<AfterEffectAnimation>();
            string strName = arAnimations.dataFileName;
            LoadAEDataAsset(item.strAsset, type, delegate(AESpriteScriptable aeData)
            {
                arAnimations.InitFootageSprite(AESpriteScriptable.CloneData(aeData));
                arAnimations.GoToAndPlay(0);
                if (!arAnimations.Loop)
                {
                    arAnimations.onFinished = delegate(AfterEffectAnimation tween)
                    {
                        Destroy();
                    };
                }
            }, addPath);
        }
        private void OnLoadComplete(Transform parent,uint type,  int inversion, AEAssetItem item, string addPath)
        {
            OnLoadComplete(parent, type, inversion, Vector3.zero, item, addPath);
        }
        private void LoadAEAsset(string strAEPath, uint type, Action<AEAssetItem> callBack)
        {
            AEAssetItem item = new AEAssetItem();
            item.itemCallBack = callBack;
            item.isHaveInited = false;
            item.strPack = AE_Pack.AE_FightSkillAE;
            item.strAsset = strAEPath;

            LoadHelp.LoadObject(AEAssetHelp.AssetAbsPath + AE_Pack.AE_FightSkillAE + ".u", delegate(AssetRequest o)
            {
                _lstAERequest.Add(o);
                if (item.strAsset == "")
                    item.prefab = o.mainAsset;
                else
                    item.prefab = o.Load(item.strAsset);
                callBack(item);
            });
        }
        private void LoadAEDataAsset(string strAEPath,uint type, Action<AESpriteScriptable> callBack, string addPath)
        {
            string strName = "warEffect_" + type + addPath;
            strName += ".u";
            LoadSubData(strName, callBack);
        }
        private void LoadSubData(string strName, Action<AESpriteScriptable> callBack)
        {
            LoadHelp.LoadObject(AEAssetHelp.AssetAbsPath + strName, delegate(AssetRequest o)
            {
                _lstAEDataRequest.Add(o);
                AESpriteScriptable ae = o.mainAsset as AESpriteScriptable;
                callBack(ae);
            });

        }
        public void Destroy()
        {
            foreach (AEAssetItem item in _lstAEItem)
            {
                if (item.gameObject != null)
                {
                    GameObject.Destroy(item.gameObject);
                    item.gameObject = null;
                }
                item.prefab = null;
            }
            _lstAEItem.Clear();
            foreach (AssetRequest o in _lstAEDataRequest)
                LoadHelp.DeleteObject(o.Path);
            foreach (AssetRequest o in _lstAERequest)
                LoadHelp.DeleteObject(o.Path);
            _lstAERequest.Clear();
            _lstAEDataRequest.Clear();
        }
    }
}