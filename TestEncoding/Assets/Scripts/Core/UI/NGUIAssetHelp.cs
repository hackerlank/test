using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using Common;
using AE_Effect;

namespace NGUI
{

    public class NGUI_UI
    {
        public const string NGUI_Root = "NGUI_Root";
        public const string NGUI_FontLoad = "NGUI_FontLoad";
        public const string NGUI_TestUI = "TestUI";
        public const string NGUI_Digit = "NGUI_Digit";
        public const string NGUI_Blood = "NGUI_Blood";
        public const string NGUI_BackGround = "NGUI_BackGround";//游戏背景
        public const string NGUI_GiftBag = "NGUI_GiftBag";
        public const string NGUI_PublishTask = "NGUI_PublishTask";
        public const string NGUI_Bag = "NGUI_Bag";
        public const string NGUI_Tooltip = "NGUI_Tooltip";
        public const string NGUI_MsgBox = "NGUI_MsgBox";
        public const string NGUI_BuyGiftBag = "NGUI_BuyGiftBag";
        public const string NGUI_Chat = "NGUI_Chat";
        public const string NGUI_Ask = "NGUI_Ask";
        public const string NGUI_CardActivity = "NGUI_CardActivity";
        public const string NGUI_ZhuanShuFuBen = "NGUI_ZhuanShuFuBen";
        
    }
    /// <summary>
    /// 添加注释：王庆国 2015/1/6
    /// 资源包中的资源信息单位的管理器，包括加载，删除
    /// </summary>
    public class NGUIAssetHelp
    {
        public const string AssetAbsPath = @"Icon/ui.u";
        public const string AssetFightPath = @"Icon/Fright.u";
        public const string AssetCommonPath = @"Icon/Common.u";
        public const string AssetShopPath = @"Icon/Shop.u";
        public const string AssetEffectSecretPath = @"Icon/Effect_Secret.u";
        public const string AssetLoginPath = @"Icon/Login.u";
        public const string AssetSecretUIPath = @"Icon/SecretUI.u";
        public const string AssetSecretMapPath = @"Icon/SecretMap.u";
        public const string AssetNewMapPath = @"Icon/NewMap.u";
        /// <summary>
        /// 加载NGUIAssetItem对象，加载完成后执行回调函数处理该对象
        /// </summary>
        /// <param name="item">要加载的资源单位信息</param>
        /// <param name="callBack">回调函数</param>
        public static void LoadAsset(NGUIAssetItem item, Action<NGUIAssetItem> callBack)
        {
            string strPath = item.AssetName;

            item.fStartTime_FromAsset = Time.realtimeSinceStartup;
            item.strPack = AssetAbsPath;
            //PlayerHandle.Debug("NGUIAssetHelp.LoadAsset  " + item.strPack + " = " + item.AssetName);
            NGUIDecompressionHelp.Instance.LoadObject(AssetAbsPath, strPath, obj =>
            {
                NGUIAtlasHelp.Instance.CheckNeedLoadAtlas(AssetAbsPath, strPath,delegate
                {
                    //PlayerHandle.Debug("NGUIAssetHelp.LoadAsset.CheckNeedLoadAtlas.callback  " + item.strPack + " = " + item.AssetName);
                    item.prefab = obj;
                    callBack(item);
                });
            });
        }

        public static void LoadAsset(NGUIAssetItem item, string strAssetAbsPath, Action<NGUIAssetItem> callBack)
        {
            string strPath = item.AssetName;

            item.fStartTime_FromAsset = Time.realtimeSinceStartup;

            item.strPack = strAssetAbsPath;

            NGUIDecompressionHelp.Instance.LoadObject(strAssetAbsPath, strPath, obj =>
            {
                //PlayerHandle.Debug("NGUIAssetHelp", "LoadAsset", "从I/O加载NGUI资源包:" + AssetAbsPath + strPath + ",  总消耗时间:" + item.GetFromAsset(Time.realtimeSinceStartup));

                NGUIAtlasHelp.Instance.CheckNeedLoadAtlas(strAssetAbsPath, strPath, delegate
                {
                    item.prefab = obj;

                    //PlayerHandle.Debug("NGUIAssetHelp", "LoadAsset", "从AssetBundle: " + AssetAbsPath + " 解压NGUI资源:" + strPath + ",  总消耗时间:" + item.GetDecAsset(Time.realtimeSinceStartup));

                    callBack(item);
                });
            });
        }


        public static void PreDecompressData(string strAssetAbsPath,string strUIName, Action callBack)
        {
            NGUIDecompressionHelp.Instance.LoadObject(strAssetAbsPath, strUIName, obj =>
            {
                NGUIAtlasHelp.Instance.CheckNeedLoadAtlas(strAssetAbsPath, strUIName, delegate
                {
                    callBack();
                });
                
            });
        }


        public static void DeleteAssetBundle()
        {

            //LoadHelp.DeleteObjectAssetBundle(AssetAbsPath);

            //NGUIAtlasHelp.Instance.DeleteByPack(AssetAbsPath);

            ////Resources.UnloadUnusedAssets();
            //NGUIManager.Instance.PrefabDeleteByLoadHelp();
            GC.Collect();
          
        }
        public static void DeleteSecretResources()
        {
            //Debug.LogError("DeleteSecretAssetBundle");  
            NGUIDecompressionHelp.Instance.DeleteByName(AssetSecretUIPath);
            
            LoadHelp.DeleteForceObject(AssetSecretUIPath);
           
            LoadHelp.DeleteForceObject("NGUI/newSecret.u");
            LoadHelp.DeleteForceObject("NGUI/newSecret_treasure.u");
            LoadHelp.DeleteForceObject("NGUI/newSecret_collection.u");
            LoadHelp.DeleteForceObject("NGUI/SecretBottleAtlas.u");
            LoadHelp.DeleteForceObject("NGUI/SecretBottleAtlas.u");
            LoadHelp.DeleteForceObject("NGUI/effectSecret_jpg.u");
            LoadHelp.DeleteForceObject("NGUI/effectSecret_png.u");
            LoadHelp.DeleteForceObject("NGUI/sea.u");
            LoadHelp.DeleteForceObject("NGUI/SecretBoxAtlas.u");
            LoadHelp.DeleteForceObject("NGUI/SecretCollectionMoneyAtlas.u");
            LoadHelp.DeleteForceObject("NGUI/treasurepng.u");
            AEManager.Instance.DeleteAllAE();
            
            NGUI_Font.Instance.Dispose(PackageFontType.SECRET);
            NGUIManager.Instance.DeleteByName_ForceUI("NGUI_Secret");

            NGUIManager.Instance.PrefabDeleteByLoadHelp();
            GC.Collect();

        }
        public static void DeleteMapResources() 
        {
            NGUIDecompressionHelp.Instance.DeleteByName(AssetNewMapPath);

            LoadHelp.DeleteForceObject(AssetNewMapPath);
            NGUI_Font.Instance.Dispose(PackageFontType.MAP);
         

            NGUIManager.Instance.PrefabDeleteByLoadHelp();
            GC.Collect();
        }
        public static void DeleteSecretMapResources() 
        {
            NGUIDecompressionHelp.Instance.DeleteByName(AssetSecretMapPath);

            LoadHelp.DeleteForceObject(AssetSecretMapPath);
            NGUI_Font.Instance.Dispose(PackageFontType.SECRETMAP);
            NGUIManager.Instance.PrefabDeleteByLoadHelp();
            GC.Collect();
        
        } 

        public static void DeleteAssetBundle(string strPath)
        {
            LoadHelp.DeleteObject(strPath);
            NGUIDecompressionHelp.Instance.DeleteByName(strPath);
            NGUIAtlasHelp.Instance.DeleteByPack(strPath);

            //Resources.UnloadUnusedAssets();
            NGUIManager.Instance.PrefabDeleteByLoadHelp();
        }

        public static void DeleteAssetBundleAndWWW(string path)
        {
            //PlayerHandle.Debug("NGUIAssetHelp", "DeleteAssetBundle", "删除战斗Bundle");
            NGUIDecompressionHelp.Instance.DeleteByName(path);

            //LoadHelp.DeleteForceObject(path);

            NGUIAtlasHelp.Instance.DeleteByPackWWW(path);

            

            //Resources.UnloadUnusedAssets();
            NGUIManager.Instance.PrefabDeleteByLoadHelp();
        }

        public static void PreLoadAssetBundleAndWWW(string strName,Action callBack)
        {
            LoadHelp.LoadObject(strName, o =>
            {
                callBack();
            });
        }
    }
    /// <summary>
    /// 添加注释：王庆国 2015/1/6
    /// 资源包中的资源信息单位,即NGUI资源信息,如果已经加载,则包含加载的对象prefab.
    /// </summary>
    public class NGUIAssetItem
    {
        public Vector3 vOriginPos=NGUIManager.vNGUIDefault;
        /// <summary>
        /// 资源包strPack中的预设资源AssetName加载完成后的资源对象。加载完设置。
        /// </summary>
        public UnityEngine.Object prefab;
        /// <summary>
        /// 实例化的资源对象，这个只支持场景中同时存在一个UI。如果nguiShowType为NGUIShowType.MULTI，
        /// 则代表最后一个实例化的对象
        /// </summary>
        public GameObject gameObject;
        /// <summary>
        /// 实例化的资源对象列表，可以支持场景中同时存在多个UI.
        /// </summary>
        public List<GameObject> GameObjects = new List<GameObject>();
        /// <summary>
        /// 初次加载多个血条时，资源还没有加载进来，但已经加入字典中。第一个加载的资源与资源加载完成中间的几个几个创建申请将会报错，
        /// 导致初始时有多个血条加载不出来，但主角的一直可以加载。
        /// 这里将所有回调都加入到CallbackAction中。当资源加载完成时，统一调用。
        /// </summary>
        public Action<NGUIAssetItem> CallbackAction; 
        /// <summary>
        /// 资源名
        /// </summary>
        public string AssetName;
        /// <summary>
        /// 资源包名
        /// </summary>
        public string strPack;//资源包
        public NGUIShowType nguiShowType = NGUIShowType.ONLYONE;
        public NGUIAssetType nguiAssetType = NGUIAssetType.INSCENE;
        /// <summary>
        /// 是否禁止 自动删除 自动不显示界面
        /// </summary>
        public bool bIsForbiddenAuto = false;//是否禁止 自动删除 自动不显示界面

        public float fStartTime_FromAsset;//从assetbundle 获取资源开始时间
        public float fEndTime_FromAsset;//从硬盘 获取assetbundle资源结束时间

        public float fEndTime_DecAsset;//从assetbundle解压资源结束时间

        public bool isRemoved=false;//该对象是否标记已删除 瞬间产生多个对象 同时又带删除动作 引发bug
        public float GetFromAsset(float fEnd)
        {
            fEndTime_FromAsset = fEnd;
            float f = fEndTime_FromAsset - fStartTime_FromAsset;
            return f;
        }

        public float GetDecAsset(float fEnd)
        {
            fEndTime_DecAsset = fEnd;
            float f = fEndTime_DecAsset - fStartTime_FromAsset;
            return f;
        }


        public float fStartTime_Instantiate;//实例化开始时间
        public float fEndTime_Instantiate;//实例化结束时间
        public float GetInstantiate(float fEnd)
        {
            fEndTime_Instantiate = fEnd;
            float f = fEndTime_Instantiate - fStartTime_Instantiate;
            return f;
        }
    }

    /// <summary>
    /// 界面的显示方式
    /// </summary>
    public enum NGUIShowType
    {
        ONLYONE,//界面的唯一性 可以自动关闭
        SELF,//自定义界面关闭打开
        ACTIVE_FALSE,//界面状态自动false
        MULTI,//游戏中存在多个实例化的资源
    }

    /// <summary>
    /// 界面的资源的删除方式
    /// </summary>
    public enum NGUIAssetType
    {
        ALLSTAY,//整个游戏一直存在
        INSCENE,//只在场景中存在
        SELF,//自定义管理删除
    }
}
