using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGUI;
using Common;
using System.Reflection;
using UnityEngine;

namespace MGUI
{
    public class GUIWindowState
    {
        #region ui事件注册函数
        public delegate void ChangeState();
        private static ChangeState changeStateEvent;
        private static Dictionary<MethodInfo, ChangeState> changeStateEventDic = new Dictionary<MethodInfo, ChangeState>();
        public static ChangeState ChangeStateEvent
        {
            set
            {
                changeStateEventDic[value.Method] = value;
                changeStateEvent += value;
            }

            get
            {
                return changeStateEvent;
            }
        }


        public static void ExcuteChangeStateEvent()
        {
            if (changeStateEvent == null) return;
            changeStateEvent();
        }

        public static ChangeState RealStateEvent
        {
            set
            {
                changeStateEvent = value;
            }
            get
            {
                return changeStateEvent;
            }
        }
        public static void RemoveStateEvent(MethodInfo mInfo)
        {
            if (!changeStateEventDic.ContainsKey(mInfo))
                return;
            changeStateEvent -= changeStateEventDic[mInfo];
            changeStateEventDic.Remove(mInfo);
        }

        #endregion




        public static EWindowState WindowState = EWindowState.Login;
        public static EWindowState LastState;

        private static void CommonState(EWindowState changeToState)
        {
            PlayerHandle.Debug("SetState " + changeToState + "   " + LastState);

            if (GUIWindowState.WindowState != EWindowState.ViewHeroDetail)
                LastState = WindowState;
            WindowState = changeToState;

        }

        //设置状态
        public static void SetState(EWindowState changeToState)
        {
            CommonState(changeToState);
            if (ChangeStateEvent != null)
                ChangeStateEvent();
        }

        //自动打开界面
        public static void SetStateAutoOpen(EWindowState changeToState)
        {
            CommonState(changeToState);
            LoadNGUI(changeToState, false);
            //if (ChangeStateEvent != null)
            //    ChangeStateEvent();
        }

        //自动关闭窗口
        public static void SetStateAutoDelete(EWindowState changeToState)
        {
            CommonState(changeToState);
            NGUIManager.Instance.AutoDeleteUI();
            if (ChangeStateEvent != null)
                ChangeStateEvent();
        }


        //自动关闭并且打开窗口
        public static void SetStateAutoDeleteOpen(EWindowState changeToState)
        {
            CommonState(changeToState);
            NGUIManager.Instance.AutoDeleteUI();
            LoadNGUI(changeToState, false);
            //if (ChangeStateEvent != null)
            //    ChangeStateEvent();
        }

        //自动关闭并且打开窗口
        public static void SetStateAutoDeleteOpen(EWindowState changeToState, Action callBack)
        {
            CommonState(changeToState);
            NGUIManager.Instance.AutoDeleteUI();
#if wqg
            LoadNGUIActionCallBack(changeToState, false, callBack);
#endif
            //if (ChangeStateEvent != null)
            //    ChangeStateEvent();
        }

        // 自动关闭并且打开窗口
        public static void SetStateAutoDeleteOpen(EWindowState changeToState, bool bIsForbiddenAuto)
        {
            CommonState(changeToState);
            //NGUIManager.Instance.AutoDeleteUI();
            LoadNGUI(changeToState, bIsForbiddenAuto);
            //if (ChangeStateEvent != null)
            //    ChangeStateEvent();
        }

        public static void LoadNGUI(EWindowState changeToState, bool bIsForbiddenAuto)
        {
#if wqg
            LoadNGUIActionCallBack(changeToState, bIsForbiddenAuto, null);
#endif
        }
#if wqg
        private static void LoadNGUIActionCallBack(EWindowState changeToState, bool bIsForbiddenAuto,Action callBack)
        {
            GUIModule guiModule = AppInterface.GUIModule as GUIModule;
            MenuUI menuUI = guiModule.GetMenuUI;
          //  RechargeUI m_recharge;
            switch (changeToState)
            {
                case EWindowState.FriendList:
                    menuUI.ChangeFriendListState();
                    break;
                case EWindowState.Menu:
                    menuUI.InitMenuUI(delegate(Transform root)
                    {
                        if (callBack != null) callBack();
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);

                    }, bIsForbiddenAuto);
                    break;
                case EWindowState.Storage:
                    menuUI.InitStorageUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.HeroBreak:
                    menuUI.SetHeroBreakState();
                    menuUI.InitHeroCommon(changeToState);
                    break;
                case EWindowState.HeroLevelUp:
                    menuUI.SetHeroLevelUpState();
                    menuUI.InitHeroCommon(changeToState);
                    break;
                case EWindowState.SkillLevelUp:
                    menuUI.SetHeroSkillLevelUpState();
                    menuUI.InitHeroCommon(changeToState);
                    break;
                case EWindowState.HeroDetail:
                    menuUI.SetHeroDetailState();
                    menuUI.InitHeroCommon(changeToState);
                    break;
                case EWindowState.HeroDestiny:
                    menuUI.SetHeroDestinyState();
                    menuUI.InitHeroCommon(changeToState);
                    break;
                case EWindowState.HeroBagList:
                    menuUI.SetHeroBagListState();
                    menuUI.InitHeroMergeCommon(changeToState);
                    break;
                case EWindowState.PatchCompund:
                    menuUI.InitHeroMergeCommon(changeToState);
                    menuUI.SetHeroPatchCompundListState();
                    break;
                case EWindowState.PatchDecompose:
                    menuUI.InitHeroMergeCommon(changeToState);
                    menuUI.SetHeroPatchDecomposeListState();
                    break;
                //case EWindowState.WorldBoss:
                //    menuUI.InitWorldBoss(delegate(Transform root)
                //    {
                //        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                //    });
                //    break;
                case EWindowState.ArenaFormation:
                    menuUI.m_mguiAreanUI.InitAreanFomration(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.Formation:
                    menuUI.m_mguiForamtionUI.InitFormation(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.PVEFrightAward:
                    menuUI.InitFrightAward(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.WorldBoss_PVEResult:
                    menuUI.InitWorldBossResult(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.PVPFrightResult:
                    menuUI.InitPVPResult(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.Task://实例化任务/福利窗口
                    menuUI.InitTaskUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
				case EWindowState.Sign:
					menuUI.InitSignUI(delegate(Transform root)
					{
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
					break;
                case EWindowState.ArenaList:
                    menuUI.m_mguiAreanUI.InitArenaCommon(changeToState);
                    menuUI.m_mguiAreanUI.InitListAreanRank(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.ArenaCountryList:
                    menuUI.m_mguiAreanUI.InitArenaCommon(changeToState);
                    menuUI.m_mguiAreanUI.m_mguiAreanRankCountryUI.InitAreanCuntry();
                    break;
                case EWindowState.ArenaReportList:
                    menuUI.m_mguiAreanUI.InitArenaCommon(changeToState);
                    menuUI.m_mguiAreanUI.m_mguiAreanHistoryReportUI.InitAreanHistory();
                    break;

                case EWindowState.AreanAward:
                    menuUI.m_mguiAreanUI.InitArenaCommon(changeToState);
                    menuUI.m_mguiAreanUI.m_mguiAreanRankCountryUI.InitAreanAward();
                    break;
                case EWindowState.ArenaShop:
                    menuUI.m_mguiAreanUI.InitArenaCommon(changeToState);
                    menuUI.m_randCardUI.EnterPVPStoreItem();
                    break;
                case EWindowState.FriendEnemy:
                    menuUI.InitFriendEnemy(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                //case EWindowState.WorldBoss_Enemy:
                //    menuUI.InitWorldBossEnemy(delegate(Transform root)
                //    {
                //        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                //    });
                //    break;
                //case EWindowState.WorldBoss_Oppent:
                //    menuUI.InitWorldBossOppent(delegate(Transform root)
                //    {
                //        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                //    });
                //    break;
                case EWindowState.GFriendList:
                    menuUI.ChangeGayFriendListState();
                    break;
                case EWindowState.GFriendListSecret:
                    PlayerHandle.Debug("--------init   friend  gay  list------------");
                    menuUI.ChangeSecretGayFriendListState();
                    break;
                //case EWindowState.WorldBoss_AwardList:
                //    menuUI.InitWorldBossAward(delegate(Transform root)
                //    {
                //        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                //    });
                //    break;
                //case EWindowState.WorldBoss_RankList:
                //    menuUI.InitWorldBossRange(delegate(Transform root)
                //    {
                //        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                //    });
                //    break;
                case EWindowState.Activity:
                    menuUI.InitInvitCodeUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.PVEFrightFriend:
                    menuUI.InitFrightFriend(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.Equip:
                    menuUI.InitSmithyUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });

                    break;
                case EWindowState.EquipSell:
                    menuUI.InitEquipSellUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.EquipSelect:
                    menuUI.InitEquipSelectUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.Setting:
                    menuUI.InitSettingUI(delegate(Transform root)
                    {
                        AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                    });
                    break;
                case EWindowState.SkillUpPopWindow:
                        menuUI.SetHeroSkillLevelUpPopState();
                    break;
                case EWindowState.Relation:
                    {
                        menuUI.InitRelationUI(delegate(Transform root)
                        {
                        });
                    }
                    break;
                case EWindowState.SeptPushList:
                    {
                        menuUI.InitSeptPushUI(delegate(Transform root)
                        {
                        });
                    }
                    break;
                case EWindowState.Sept:
                    {
                        menuUI.InitSeptUI(delegate(Transform root)
                        {
                        });
                    }
                    break;
                case EWindowState.Mail:
                    {
                        menuUI.InitMailUI(delegate(Transform root)
                        {
                        });
                    }
                    break;

                case EWindowState.GuardSelect:
                    {
                        menuUI.InitGuardSelectUI(delegate(Transform root)
                        {
                        });
                    }
                    break;

                case EWindowState.EquipSelectStrength:
                    {
                        menuUI.InitEquipSelectStrengthUI(delegate(Transform root)
                        {
                        });
                    }
                    break;

                case EWindowState.EquipSelectInherit:
                    {
                        menuUI.InitEquipSelectInheritUI(delegate(Transform root)
                        {
                        });
                    }
                    break;

                case EWindowState.EquipSelectInheritMat:
                    {
                        menuUI.InitEquipSelectInheritMatUI(delegate(Transform root)
                        {
                        });
                    }
                    break;

                //case EWindowState.Recharge:
                //    {
                //        menuUI.InitRechargeUI(delegate(Transform root)
                //        {
                //        });
                //    }
                //    break;

                case EWindowState.ChatCollect:
                    {
                        menuUI.InitChatCollectUI(delegate(Transform root)
                        {
                        });
                    }
                    break;
                case EWindowState.Vip:
                    {

                    }
                    break;
                case EWindowState.HeroBagArtifice:
                    {
                        menuUI.InitHeroMergeCommon(changeToState);
                        menuUI.SetHeroArtificeState();
                    }
                    break;
                case EWindowState.HeroCollection:
                    {
                        menuUI.InitCardCollectionUI(delegate(Transform root)
                        {
                        });
                    }
                    break;
                case EWindowState.SecretMap:
                    {
                        menuUI.InitSecretMapUI(delegate(Transform root) 
                        {
                            
                        });
                    }
                    break;
                case EWindowState.SecretAdventure:
                    {
                      //  NGUI_GameMainManager.Instance.ChangeState<NGUI_SecretAdventureNew>(NGUI_UI.NGUI_SecretAdventureNew, delegate() { });
                        NGUIManager.Instance.AddByName<NGUI_SecretAdventureNew>(NGUI_UI.NGUI_Font_SecretUI, NGUIAssetHelp.AssetSecretUIPath, NGUI_UI.NGUI_SecretAdventureNew, NGUIShowType.ONLYONE, delegate(NGUI_SecretAdventureNew script) { });
                        //menuUI.InitAdventureCodeUI(delegate(Transform root)
                        //{
                        //    AppInterface.GUIModule.CheckPlayGuide(changeToState, root, 0);
                        //});
                    }
                    break;
                //case EWindowState.Recharge:
                //    {
                //        menuUI.m_MguiRechargeUI.InitRechargeUI();
                //    }
                //    break;
				case EWindowState.Shop:
					menuUI.m_randCardUI.EnterShopUI();
					break;
				case EWindowState.Arean:
					menuUI.m_mguiAreanUI.enterArena();
					break;
            }
        }
#endif
        public static void ClearChangeState()
        {
            changeStateEvent = null;
            changeStateEventDic.Clear();
        }
    }

    public enum wLeadOk
    {
        Team = 1,
        ArenaTeam = 2,
        SecretTeam = 3,
        TeamAndArena = 4,
        TeamAndSecret = 5,
        ArenaAndSecret = 6,
        TeamAndArenaAndSecret = 7,
    }

    public enum EWindowState
    {
        None,
        Login = 1,  //登陆 
        Copy = 2,   //副本
        ActiveCopy = 3, //活动副本
        Stage = 4,  //关卡
        Menu = 5,   //主界面
        Fight = 6,  //战斗        
        Help = 7,   //帮助
        SelectHero = 8, //选择英雄
        ViewHeroDetail = 9,  //查看英雄
        Storage = 10,    //仓库
        HeroLevelUp = 11,//英雄升级
        HeroBreak = 12,  //英雄突破
        SkillLevelUp = 13,   //技能升级

        Friend = 14, //好友
        FriendList = 15, //好友列表
        FriendDetails = 16,  //好友详情
        GFriendList = 17,    //好基友

        AddFriend = 18,  //添加好友
        AskForFriend = 19,   //申请列表
        Formation = 20,  //阵型

        Shop = 21,   //商城
        DonghaiSearchTreasure = 22, //东海寻宝
        FriendSearchTreasure = 23, //友情寻宝
        FrightResult = 24,   //战斗结算
        PVEFrightAward = 25,
        PVEFrightFriend = 26,
        PVPFrightResult = 27, //PVP战斗结算

        ArenaFormation = 29, //竞技场阵型
        ArenaList = 30,  //竞技场挑战列表
        ArenaCountryList = 31,  //竞技场排名
        ArenaReportList = 32,    //竞技场战报


        WorldBoss = 33,  //世界boss主页
        WorldBoss_RankList = 34, //积分排名列表
        WorldBoss_AwardList = 35, //积分奖励列表
        WorldBoss_Enemy = 36,    //世界boss挑战玩家列表
        WorldBoss_Oppent = 37,
        WorldBoss_PVEResult = 38,


        Task = 40,           //任务
        EquipSelect = 41,    //装备选择
        EquipLevelUp = 42,   //神兵突破(临时)
        Equip = 43,          //装备
        EquipSell = 44,      //装备出售
        Activity = 45,      //活动

        Secret = 46,         //秘境

        HeroDetail,        //英雄详情（在包裹里）
        HeroBag = 47,        //查看英雄包裹
        HeroBagList,
        HeroBagArtifice,
        HeroBagSkillLevelUp,
        HeroBagPVELeader,
        HeroBagPVEMember,
        HeroBagPVPLeader,
        HeroBagPVPMember,
        HeroBagSecretLeader,
        HeroBagSecretMember,
        HeroBagBreak,
        HeroBagBreakMaterial,
        HeroBagLevelUp,

        FriendEnemy = 148,          //好友仇人

        Setting = 151,        //设置

        FightLevelUp = 152,   //过关奖励经验升级

        GFriendListSecret = 153,    //好基友秘境
        SkillUpPopWindow = 154,  //技能升级弹出

        RMBStore = 155,
        FPStore = 156,
        Mail = 157,      //邮件
        Sept = 158,      //家族
        SeptPushList = 159,  //推荐家族
        Relation = 160,  //社交
        ChatCollect = 161,  //聊天汇总主界面

        GuardSelect = 162,   //选择护驾英雄

        EquipSmithy = 163,    //铁匠铺
        EquipSelectStrength = 164,     //装备强化选择
        EquipSelectInherit = 165,        //装备融合选择
        EquipSelectInheritMat = 166,     //装备融合材料选择

        Patch = 167,
        PatchCompund,
        PatchDecompose,

        Recharge = 190,      //充值

        Billboard = 169,  //公告板
        Vip = 170,   //VIP
        HeroCollection = 171,   //英雄卡片图鉴
        SecretMap=172,
        SecretAdventure = 173,
        SecretLogin = 174,
        OtherActivity = 175,//开服活动
        HeroDestiny = 180,
        FestivalActivity,//节日活动
        AreanAward=176,//竞技场排名奖励
        ArenaShop,//竞技场商城
        
		Sign,//签到
		Arean,//竞技场
		
        #region 引导窗口
        NGUI_ListHero = 100,//
        NGUI_ListHeroLevelUp = 101,//
        NGUI_NewShopCommon = 102,//
        NGUI_SingleRandCard = 103,//
        NGUI_ChallengueBegin = 104,//
        NGUI_CardDetail=105,//
        NGUI_ListEquip=106,//
        #endregion

    }
    public class GUIWindowID
    {
        public const int Menu = 1;          //主界面
        public const int UpCommand = 2;     //上共同界面
        public const int DownCommand = 3;   //下共同界面
        public const int RightSlected = 4;  //右边点选区域
        public const int ShopUpUI = 5;      //商店上界面
        public const int StorageDownUi = 6; //仓库下界面

        public const int LoginUI = 7;       //登录
        public const int ShowTips = 8;      //二次确认

        public const int ShengLongSearch = 20;    //神龙
        public const int DongHaiSearch = 21;      //东海龙宫
        public const int FriendDongHaiSearch = 22;//好友抽奖

        public const int upTips = 33;           //上菜单提示
        public const int taskTips = 34;         //任务提示
        public const int worldBossDesc = 35;    //世界boss说明

        public const int cloudShow = 36;        //云显示
        public const int ComposeHero = 1000;    //英雄碎片合成
        public const int askUI = 11211;          //二次确认

        public const int DialogueWindow = 4000; //战中对白
    }
}
