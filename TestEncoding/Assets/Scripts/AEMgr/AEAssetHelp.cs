using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using Common;

namespace AE_Effect
{

    public class AE_Pack//特效包 
    {
        public const string AE_zhandoukaishi = "AE_zhandoukaishi";
        public const string AE_Secret_fortune = "AE_Secret_fortune";//算命事件特效包
        public const string AE_heroEvolve = "AE_heroEvolve";
        public const string AE_heroLevelUp = "AE_heroLevelUp";
        public const string AE_skillLevelUp = "AE_skillLevelUp";
        public const string AE_Secret_Gate = "AE_Secret_Gate";
        public const string AE_Secret_Enter = "kuosan";//秘境入口
        public const string AE_Secret_choose = "AE_Secret_choose"; //AE秘境选择道路
        public const string AE_Secret_chooseExplode = "AE_Secret_chooseExplode"; //AE秘境选择道路爆炸
        public const string AE_Secret_chooseRun = "AE_Secret_chooseRun"; //AE秘境选择道路逃跑
        public const string AE_Secret_chooseCry = "AE_Secret_chooseCry"; //AE秘境选择道路小妖哭


		public const string AE_Secret_bottle = "AE_Secret_bottle";
		public const string AE_Secret_collection="AE_Secret_collection";
		public const string AE_Secret_guochang="AE_Secret_guochang";
		public const string AE_Secret_TalkBurst = "saqian1";
        public const string AE_Secret_completeA = "AE_Secret_completeA";
        public const string AE_Secret_completeB = "AE_Secret_completeB";
        public const string AE_mijingtongguanjiesuanA = "AE mijingtongguanjiesuanA";
        public const string AE_mijingtongguanjiesuanB = "AE mijingtongguanjiesuanB";

        /*秘境炉子特效*/
        public const string AE_Secret_Stove_Idle = "AE_Stove_Idle";
        public const string AE_Secret_Stove_Crush1 = "AE_Stove_Crush1";
        public const string AE_Secret_Stove_Crush2 = "AE_Stove_Crush2";
        public const string AE_Secret_Stove_Crush3 = "AE_Stove_Crush3";
        public const string AE_Secret_Stove_Crush4 = "AE_Stove_Crush4";
        public const string AE_Secret_Stove_Explore = "AE_Stove_Explore";
        public const string AE_Secret_Stove_Click = "AE_Stove_Click";
        //英雄合成特效.
        public const string AE_juesehecheng = "AE_juesehecheng";
        //
        public const string AE_guangxiao = "AE_guangxiao";

        /*秘境地宫特效*/
        public const string AE_Secret_Door_Idle = "AE_Door_Idle";
        public const string AE_Secret_Door_Crush1 = "AE_Door_Crush1";
        public const string AE_Secret_Door_Crush2 = "AE_Door_Crush2";
        public const string AE_Secret_Door_Crush3 = "AE_Door_Crush3";
        public const string AE_Secret_Door_Crush4 = "AE_Door_Crush4";
        public const string AE_Secret_Door_Explore = "AE_Door_Explore";
        public const string AE_Secret_Door_Click = "AE_Door_Click";

        /*秘境火球特效*/
        public const string AE_Fireball_RoleIdle = "AE_Fire_RoleIdle";
        public const string AE_Fireball_RoleHit = "AE_Fire_RoleHit";
        public const string AE_Fireball_Res1 = "AE_Fire_Res1";
        public const string AE_Fireball_Res2 = "AE_Fire_Res2";
        public const string AE_Fireball_Res3 = "AE_Fire_Res3";
        public const string AE_Fireball_LoseMoney = "AE_Fire_Losegold";
        public const string AE_Fireball_LoseLingqi = "AE_Fire_LoseLq";
        //装备碎片合成
        public const string AE_zhuangbeihecheng = "AE zhuangbeihecheng";
		
		//关卡和副本特效解锁
        public const string AE_UnlockEffect = "AE UnlockEffect";

		//藏宝图特效
		public const string AE_cangbaotu01 = "AE cangbaotu01";
		public const string AE_cangbaotu02 = "AE cangbaotu02";
		public const string AE_cangbaotu03 = "AE cangbaotu03";
        /*秘境躲箭矢特效*/
        public const string AE_Arrow_RoleIdle = "AE_Arrow_RoleIdle";
        public const string AE_Arrow_RoleHit = "AE_Arrow_RoleHit";

        /*秘境火山特效*/
        public const string AE_Volcano_Idle = "AE_Volcano_FanIdle";
        public const string AE_Volcano_Click = "AE_Volcano_FanMove";

        /*秘境金箍棒*/
        public const string AE_Jingubang_Explore = "AE_Jingubang_Explore";
        public const string AE_Jingubang_Win = "AE_Jingubang_Win";

        /*秘境滑动引导特效*/
        public const string AE_Guide_HandDrag = "AE_Guide_HandDrag";


        //装备特效
        public const string AE_Equip_LevelUp = "AE_Equip_LevelUp";
        public const string AE_Equip_Strength = "AE_Equip_Strength";    //强化成功
        public const string AE_Equip_Strength_Fail = "AE_Equip_Strength_Fail";  //强化失败
        public const string AE_Equip_Resolve = "AE_Equip_Resolve";      //分解
        public const string AE_Equip_Inherit = "AE_Equip_Inherit";      //融合

        //------------技能升级------------------
		public const string AE_skillNegBtn = "AE beidonganniu";//被动技能
        public const string AE_skillNegClick = "AE beidonganniudianji";//被动技能按下
        public const string AE_skillPosBtn = "AE zhudonganniu";//主动技能
        public const string AE_skillPosClick = "AE zhudonganniudianji";//主动技能按下
		public const string AE_skillUpgradeSuccess = "AE jinengshenji";//技能升级成功
        public const string AE_skillUpgradeFail = "AE jinengshengjishibai";//技能升级失败

        public const string AE_zhandoulitisheng = "AE_zhandoulitisheng";//战斗力提升


        public const string AE_UNLOCK_FASHE = "fashe";
        public const string AE_UNLOCK_FANGSHE = "fangshe";

        //----------------战斗-----------------
        public const string AE_FightBtn = "AE_fightBtn";
        //---------------角色技能公共特效-------------------

        public const string AE_skillLayer = "AE_skillLayer";
        public const string AE_skillLayer_data = "AE_skillLayer_data";

        public const string UnitsData_Nb = "UnitsData_Nb";
        //-------------------------------------------------------

        //--------------商城抽奖--------------------------------
        public const string AE_RandCard = "AE_RandCard";
        public const string AE_TenRandCard = "AE_TenRandCard";

        //-------------------common new--------
        public const string AE_commonAE = "commonAE";
        public const string AE_PveWin = "AE zhandoujiesuan";//pve战斗结算 胜利
        public const string AE_PveLost = "AE shibaijiesuan";//pve战斗结算  失败

        public const string AE_FrightFriend = "AE youqingjiesuan";//友情结算
        public const string AE_FrightLevelUP = "AE shengjijiesuan";//战斗结算升级

        public const string AE_SpecialHero = "AE teshujuese";//特殊 道具
        public const string AE_SpecialEquip = "AE teshuzhuangbei";//特殊 英雄



        public const string AE_FirstPassStorage = "AE shouciguoguanwupin";//首次通关 道具
        public const string AE_FirstPassHero = "AE shouciguoguanjuese";//首次通关 英雄

        public const string AE_HeroLevelUP = "AE shengjiA";//新的升级
        public const string AE_HeroEvolve = "AE jinhua";//  新的进化

        public const string AE_GetSpecialHero = "AE teshujiangpin";//抽奖 特殊英雄
        public const string AE_GetHeroCardTen = "AE choujiang10";//新的抽英雄10个

        public const string AE_MiGingJueseShengJiA = "AE mijingjueseshengjiA";//秘境升级特效    
        public const string AE_mijingshengjiA = "AE mijingshengjiA";//秘境角色升级特效    
        public const string AE_mijingshengjiB = "AE mijingshengjiB";//秘境角色升级特效    

        public const string AE_dengchangA = "AE_dengchangA";//进战斗云
        public const string AE_PreLoadDecompress = "AE 004jineng";//预加载prefab
        public const string AE_dengchangB = "AE_dengchangB";//进战斗云1
        public const string AE_BOSSchuxian = "AE BOSSchuxian";//boss出现
        public const string AE_Secret_none = "AE mijingxishizhenbao";//秘境特殊装备
        ///secret new
        public const string AE_Secrect_108choose = "choose";//秘境逻辑事件包
        public const string AE_108cry = "AE 108cry";//（未使用 替换为 失望）
        public const string AE_108mijingA = "AE 108mijingA";//（出现箱子）
        public const string AE_108mijingdisappoint = "AE 108mijingdisappoint";//失望
        public const string AE_108run = "AE 108run";///逃跑
        public const string AE_bomb = "AE bomb";//开箱子爆炸
        public const string AE_Guide = "AE_Guide";

        public const string AE_Secret_BottlePack = "bottle";//秘境逻辑事件包
        public const string AE_Secret_StovePack = "stove";//秘境炉子包
        public const string AE_Secret_DoorPack = "door";//秘境地宫包
        public const string AE_Secret_FireballPack = "fireball";//秘境火球包
        public const string AE_Secret_VolcanoPack = "volcano";//秘境火山包
        public const string AE_Secret_ArrowPack = "arrow";//秘境躲箭矢包
        public const string AE_Secret_JingubangPack = "jingubang";//秘境金箍棒包
        public const string AE_Secret_GuideDragPack = "guide";//秘境滑动引导手势

      	//图标特效
		public const string AE_tubiaotexiao = "AE tubiaotexiao";
		
		//天庭盗宝特效
		public const string AE_fanzhuang = "AE fanzhuang";
		public const string AE_tiantingdaobaokanji = "AE tiantingdaobaokanji";
		public const string AE_tiangongdaobaozuihoubaoxiang = "AE tiangongdaobaozuihoubaoxiang";

        #region 战斗增益特效
        public const string AE_FightSkillAE = "FightSkillAE";
        public const string AE_warEffect_ = "AE_warEffect_";
        #endregion
    }

    public class AE_Pack_Asset//每个特效包 对应的资源列表
    {
        public const string AE_None = "";
    }


    public class AEAssetHelp
    {
        public const string AssetAbsPath = @"AE/";

        private string strNone = "";
        private static AEAssetHelp instance = null;
        public static AEAssetHelp Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AEAssetHelp();
                }
                return instance;
            }
        }

        public void LoadAssetAEData(string strPack,string strName, Action<AESpriteScriptable> callBack)
        {
            strPack += ".u";
            LoadHelp.LoadObject(AssetAbsPath + strPack, delegate(AssetRequest o)
            {
                AESpriteScriptable ae = o.Load(strName) as AESpriteScriptable;
                callBack(ae);
            });
        }

        public void LoadAssetAnimationData(string strName,Action<UnityEngine.Object> callBack)
        {
            string strPack = AE_Pack.UnitsData_Nb;
            strPack += ".u";
            LoadHelp.LoadObject(AssetAbsPath + strPack, delegate(AssetRequest o)
            {
                callBack(o.Load(strName));
            });
        }

        public void LoadAssetAnimationData(string strPack, string strName, IFrameAnimation frameAnimation, Action callBack)
        {
            strPack += ".u";
            LoadHelp.LoadObject(AssetAbsPath + strPack, delegate(AssetRequest o)
            {
                frameAnimation.InitData(o.Load(strName));
                callBack();
            });
        }

        public void LoadAssetAEData(string strName, Action<AESpriteScriptable> callBack)
        {
            strName += ".u";
            LoadHelp.LoadObject(AssetAbsPath + strName, delegate(AssetRequest o)
            {
                AESpriteScriptable ae = o.mainAsset as AESpriteScriptable;
                callBack(ae);
            });
        }

        public void LoadAsset(AEAssetItemMgr item, Action<AEAssetItemMgr> callBack)
        {
            LoadHelp.LoadObject(AssetAbsPath + item.strPack+".u", delegate(AssetRequest o)
            {
                if (item.strAsset == strNone)
                    item.prefab = o.mainAsset;
                else
                    item.prefab = o.Load(item.strAsset);

                callBack(item);
            });
        }

        public void PreLoadAEPrefab(string strPack, string strName, Action<UnityEngine.Object> callBack)
        {
            LoadHelp.LoadObject(AssetAbsPath + strPack + ".u", delegate(AssetRequest o)
            {
                UnityEngine.Object obj = o.Load(strName);
                callBack(obj);
            });
        }


        public void DeleteAssetBundle(string strPath)
        {
            if (strPath.Substring(strPath.Length - 2, 2) != ".u")strPath=strPath + ".u";
            //PlayerHandle.Debug(this, "DeleteAssetBundle", AssetAbsPath+strPath);

            LoadHelp.DeleteObject(AssetAbsPath + strPath);
            //Resources.UnloadUnusedAssets();
        }

        public void DeleteAssetBundleAndWWW(string strPath)
        {
            if (strPath.Substring(strPath.Length - 2, 2) != ".u") strPath = strPath + ".u";
            //PlayerHandle.Debug(this, "DeleteAssetBundleAndWWW", AssetAbsPath + strPath);
            LoadHelp.DeleteForceObject(AssetAbsPath + strPath);
            //Resources.UnloadUnusedAssets();
        }

        #region Pre-Load AE
        public  List<string> requestCache = new List<string>();
        public  void PreLoadAEFile(string packPath,Action callBack)
        {
            if (requestCache.Contains(packPath))
            {
                callBack();
                return;
            }
            requestCache.Add(packPath);
            LoadHelp.LoadObject(AssetAbsPath + packPath + ".u", delegate(AssetRequest o)
            {
                callBack();
            });
        }
       
        public  void ClearRequestCache()
        {
            foreach (string packPath in requestCache)
            {
                LoadHelp.DeleteObject(AssetAbsPath + packPath + ".u");
            }
            requestCache.Clear();
        }


        public void ClearRequestWWWByName(string strName)
        {
            requestCache.Remove(strName);
            //string strPath = AssetAbsPath + strName + ".u";
            DeleteAssetBundleAndWWW(strName);
            //Resources.UnloadUnusedAssets();
        }

        #endregion
    }


    public class AEAssetItemMgr
    {
        public UnityEngine.Object prefab;

        public Transform parent;

        public string strPack;//包文件名  
        public string strAsset;//特效名字

        public bool isRemoved = false;//该对象是否标记已删除 瞬间产生多个对象 同时又带删除动作 引发bu

        public List<AEAssetItem> aeAssetItemList = new List<AEAssetItem>();

        public Action<AEAssetItemMgr> itemMgrCallBack;

    }


    public class AEAssetItem
    {
        public UnityEngine.Object prefab;

        public GameObject gameObject;
        public Action<AEAssetItem> itemCallBack;

        public string strPack;//包文件名  
        public string strAsset;//特效名字

        public bool isHaveInited = false;//是否已经初始化过

        public void Destroy()
        {
            //if (gameObject) GameObject.Destroy(gameObject);
            //PlayerHandle.Debug(strPack + "/" + strAsset);
            //PlayerHandle.Debug(strFullName);
            AEManager.Instance.DeleteByObjListWWW(strPack + strAsset, this);
        }

        public void DestroyGameObject()
        {
            AEManager.Instance.DeleteByObjListItem(strPack + strAsset, this);
        }
    }
}
