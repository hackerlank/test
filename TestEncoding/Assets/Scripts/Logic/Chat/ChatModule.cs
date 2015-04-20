/*
***********************************************************************************************************
* CLR version ：$clrversion$
* Machine name ：$machinename$
* Creation time ：#time#
* Author ：hym
* Version number : 1.0
***********************************************************************************************************
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Net;
using System.Text;
using NGUI;

public enum enumChatType{
    CHAT_TYPE_PRIVATE = 1,			/// 私聊频道
    CHAT_TYPE_NINE,					/// 轻聊频道
    CHAT_TYPE_TEAM,                 /// 队伍频道
    CHAT_TYPE_FRIEND,               /// 好友频道
    CHAT_TYPE_GM,                   /// GM聊频道
    CHAT_TYPE_SYSTEM,               /// 系统频道
    CHAT_TYPE_UNION,                /// 帮会频道
    CHAT_TYPE_POP,                  /// 弹出式系统提示
    CHAT_TYPE_PERSON,               /// 个人频道
    CHAT_TYPE_WHISPER,              ///悄悄话
    CHAT_TYPE_WHISPERTOME,            ///悄悄话
    CHAT_TYPE_COUNTRY,              /// 国家频道
    CHAT_TYPE_AREA,						///区域频道
    CHAT_TYPE_FAMILY,               /// 家族频道

    CHAT_TYPE_FRIEND_AFFICHE,       /// 好友公告
    CHAT_TYPE_UNION_AFFICHE,        /// 帮会公告
    CHAT_TYPE_OVERMAN_AFFICHE,      /// 师门公告
    CHAT_TYPE_FAMILY_AFFICHE,       /// 家族公告

    CHAT_TYPE_FRIEND_PRIVATE,       /// 好友私聊
    CHAT_TYPE_UNION_PRIVATE,        /// 帮会私聊
    CHAT_TYPE_OVERMAN_PRIVATE,      /// 师门私聊
    CHAT_TYPE_FAMILY_PRIVATE,       /// 家族私聊

    CHAT_TYPE_NPC,                   ///npc说话

    CHAT_TYPE_EMOTION,      ///表情
    CHAT_TYPE_SHOPADV,      ///摆摊广告
    CHAT_TYPE_WORLD,        ///世界频道
    CHAT_TYPE_OVERMAN,      /// 师门频道
    CHAT_TYPE_AUTO,         /// 自动回复
    CHAT_TYPE_COUNTRY_PK,   /// 外国人入侵PK消息
    CHAT_TYPE_BLESS_MSG,    /// 个人祝福消息
    CHAT_TYPE_COUNTRY_MARRY, /// 结婚消息广播
    CHAT_TYPE_ERROR_GM, ///发送到GM工具的警告信息
    //CHAT_TYPE_MINIGAME  /// 玩小游戏聊天
    CHAT_TYPE_CITY = 36,	//城市频道
    CHAT_TYPE_PROVINCE = 37,//省频道

    CHAT_TYPE_CITY_NEW_WORLD = 38,  //新世界城市频道
    CHAT_TYPE_PROVINCE_NEW_WORLD = 39,  //新世界省频道
    CHAT_TYPE_R_NEW_WORLD = 40, //新世界R聊

    CHAT_TYPE_MYSTERY = 41, //神秘人聊天
    CHAT_TYPE_ALL_ZONE = 42, //全区聊天
    CHAT_TYPE_LOUDSPEAKER = 43, //小喇叭
    CHAT_TYPE_GOLDSPEAKER = 45,     //金子喇叭
    CHAT_TYPE_FIREWORKSSPEAKER = 46,//烟花道具频道
    CHAT_TYPE_ALL_ZONE_WAR_UNION = 47, //全区夺城战：阵营频道
    CHAT_TYPE_COLORWORLD = 48,  //彩色世界聊天频道
    CHAT_TYPE_NEW_NPC = 49, // npc 说话
    CHAT_TYPE_MENGZHAN_SOURCE_ZHENYING = 50,//盟战频道说话返回原区为该盟战阵营频道

    CHAT_TYPE_ALL = 10000, //全部
};

public enum enumSysInfoType
{
    INFO_TYPE_SYS = 1,  /// 系统信息、GM信息，在聊天窗口
    INFO_TYPE_GAME,         /// 游戏信息，屏幕左上
    INFO_TYPE_STATE,        /// 状态转换，屏幕左上
    INFO_TYPE_FAIL,         /// 失败信息，屏幕左上
    INFO_TYPE_EXP,          /// 特殊信息,获得经验、物品，在人物头上
    INFO_TYPE_MSG,          /// 弹出用户确认框的系统消息
    INFO_TYPE_KING,         /// 国王发出的聊天消息
    INFO_TYPE_CASTELLAN,    /// 城主发出的聊天消息
    INFO_TYPE_EMPEROR,		/// 皇帝发出的聊天消息
    INFO_TYPE_SCROLL,       /// 屏幕上方滚动的系统信息
    INFO_TYPE_ADDFRIEND,    /// 加好友任务显示和动画播放
    INFO_TYPE_BREAKFRIEND,  /// 割袍断义任务显示和动画播放
    INFO_TYPE_SKYROCKET1,   /// 焰火类型1
    INFO_TYPE_SKYROCKET2,   /// 焰火类型2
    INFO_TYPE_ATT_FLAG,		/// 旗子被攻击
    INFO_TYPE_YUANSHUAI,    /// 元帅说话
    INFO_TYPE_ZAIXIANG,		/// 宰相说话
    INFO_TYPE_BUTOU,		/// 捕头说话
    INFO_TYPE_YUSHI,		/// 御史说话
    INFO_TYPE_WAGNHOU,		/// 王后说话
    INFO_TYPE_WANGFU,		/// 王夫说话
    INFO_TYPE_HUANGHOU,		/// 皇后说话
    INFO_TYPE_HAUNGFU,		/// 皇夫说话
    INFO_TYPE_NVHUANG,		/// 女皇说话
    INFO_TYPE_NVWANG,		/// 女王说话
    INFO_TYPE_MAYOR,		/// 市长聊天
    INFO_TYPE_STADHOLDER,   /// 省长聊天
    INFO_TYPE_NO_1,         /// 天下第一说话
    INFO_TYPE_PLEDGE_PURPLE,					/// 功勋地图消息,紫色电视
    INFO_TYPE_PLEDGE_YELLOW,					/// 功勋地图消息，黄色电视
    INFO_TYPE_DARK_YELLOW,						/// 深黄色的电视公告
    INFO_TYPE_COUNTRYDARE_PURPLE_KING,			/// 紫色电视，国王在国战期间说话，使用该类型，右下脚和头顶都要显示
    INFO_TYPE_COUNTRYDARE_PURPLE_NVWANG,       /// 紫色电视，国王在国战期间说话，使用该类型，右下脚和头顶都要显示
    INFO_TYPE_COUNTRYDARE_PURPLE_EMPEROR,      /// 紫色电视，国王在国战期间说话，使用该类型，右下脚和头顶都要显示
    INFO_TYPE_COUNTRYDARE_PURPLE_NVHUANG,		/// 紫色电视，国王在国战期间说话，使用该类型，右下脚和头顶都要显示


    //下面是几个公告优化的消息类型
    INFO_TYPE_WAR,//战争类消息
    INFO_TYPE_BOSS,     //boss类,怪物攻城
    INFO_TYPE_OTHER,    //其他类型的消息，和战争类一个级别
    INFO_TYPE_RIGHCORNER, //重要的消息，在右下角弹出
    INFO_TYPE_GM,	//gm说话
    INFO_TYPE_IMPORTANCE, //gm工具，重要信息提示
    INFO_TYPE_ZHENGTUBAOBEI,		//征途宝贝说话
    INFO_TYPE_SCREEN_DOWN,		//屏幕下方
    INFO_TYPE_MEILIBAOBEI,		//魅力宝贝
    INFO_TYPE_XIANHUABAOBEI,		//鲜花宝贝
    INFO_TYPE_RENQIBAOBEI,			//人气宝贝
    INFO_TYPE_NUMBERONE,    //天下第一公告
    INFO_TYPE_SHENGSHOU,    //圣兽公告
    INFO_TYPE_MMVOTE,		//沉鱼落雁 美女聊天类型
    INFO_TYPE_COUNTRYDARE_PURPLE_YUANSHUAI, //元帅的
    INFO_TYPE_SEPTBATTLE,  //家族争霸赛
    INFO_TYPE_TOPKILLER,   //绝顶高手  =  52
    INFO_TYPE_EXCELLENT_KING,				// 优秀国王
    INFO_TYPE_EXCELLENT_QUEEN,  //优秀女王

    // 添加帝国公告类型，其它公告被屏蔽
    INFO_TYPE_DIGUO_COMMON,//普通公告－－类INFO_TYPE_EXP
    INFO_TYPE_DIGUO_LEADER,//领袖公告－－类INFO_TYPE_COUNTRYDARE_PURPLE_KING
    INFO_TYPE_DAFUDAGUI,// 大富大贵发言
    INFO_TYPE_SNOWTANK,// 打雪杖10连击
    INFO_TYPE_PURPLE, // 右下 紫色消息
    INFO_TYPE_YUSHULINFENG, //玉树临风  = 60
    INFO_TYPE_QIYUXUANANG,  //器宇轩昂
    INFO_TYPE_FENGLIUTITANG,  //风流倜傥
    INFO_TYPE_SCHOOLMASTER, //校长聊天
    INFO_TYPE_TWINKLE,      //闪烁的感叹号 加在枚举最后
    INFO_TYPE_CHILD_RED,    //孩子地图专用公告类型，其它被屏蔽
    INFO_TYPE_CHILD,      //小孩聊天框
    INFO_TYPE_BAOBOSS,		//  上角和右下角都为绿色提示

    INFO_TYPE_TIANMA,  //上角为黄色提示， 右下角为绿色提示
    INFO_TYPE_3TOWNS_OWNER,         //三城之主
    INFO_TYPE_5TOWNS_OWNER,         //五城之主
    INFO_TYPE_7TOWNS_OWNER,         //七城之主
    INFO_TYPE_15TOWNS_OWNER,        //15城之主
    INFO_TYPE_TOWN_PREFECTURE,      //太守讲话,
    INFO_TYPE_ZONGDUO1,          //泰山总舵占领帮主讲话
    INFO_TYPE_ZONGDUO2,          //华山总舵占领帮主讲话
    INFO_TYPE_ZONGDUO3,          //恒山总舵占领帮主讲话
    INFO_TYPE_ZONGDUO4,          //嵩山总舵占领帮主讲话
    INFO_TYPE_ZONGDUO5,          //衡山总舵占领帮主讲话       
    INFO_TYPE_ZONGDUO6,          //蜀山总舵占领帮主讲话
    INFO_TYPE_ZONGDUO7,          //昆仑总舵占领帮主讲话
    INFO_TYPE_OLDPLAYER,         //老玩家讲话
    INFO_TYPE_HAPPYCARD,         //欢乐卡牌
    INFO_TYPE_FIREWORKS,          //烟花说话 左边 + 中下方公告 
    INFO_TYPE_WEDDING = 84,        //天作之和
    INFO_TYPE_POPUP = 85,        // 弹出公告 具体形式客户端可配
    INFO_TYPE_POPLOOP = 86,		// 弹出公告 表示淡出
    INFO_TYPE_NEWBET_MAP = 87, //新押宝游戏地图公告
    INFO_TYPE_NEWBET_ZONE = 88, //新押宝游戏全服公告
    INFO_TYPE_DRAGON_RESTORE = 89,	//龙潭活动存储在客户端 且公告提示
    INFO_TYPE_SHIJIE = 90, //使节讲话
    INFO_TYPE_GOLD_BUSINESS = 91, //金牌商号讲话
    INFO_TYPE_WARLORD = 92,       // 武神讲话
    INFO_GD_PALYER_LEVEL1 = 93,//古董账号1级
    INFO_GD_PALYER_LEVEL2 = 94,//古董账号2级
    INFO_GD_PALYER_LEVEL3 = 95,//古董账号3级
    INFO_GD_PALYER_LEVEL4 = 96,//古董账号4级
    INFO_GD_PALYER_LEVEL5 = 97,//古董账号5级
    INFO_GD_PALYER_LEVEL6 = 98,//古董账号6级
    INFO_GD_PALYER_LEVEL7 = 99,//古董账号7级
    INFO_TYPE_MENGMIANREN = 100,//蒙面人
    INFO_TYPE_EXCELLENT_OFFICIAL = 101,//优秀官员
    INFO_TYPE_ALLYWAR_MASTER = 102,//战场盟主说话
    INFO_TYPE_AW_EXCELLENT_MASTER = 103, // 盟战优秀盟主讲话
    INFO_TYPE_AW_EXCELLENT_VICE_MASTER = 104, // 盟战优秀副盟主讲话
    INFO_TYPE_ALLYWAR_SECMASTER = 105,//盟战副盟主讲话
    INFO_TYPE_AW_HERO = 106, //盟战英雄
    INFO_TYPE_AW_GOLD_CONDUCTOR = 107, //盟战金牌指挥讲话
    INFO_TYPE_YELLOW_RIGHT_DOWN = 108, // 黄色右下角提示
    INFO_TYPE_SEPT_GIANTS_GAME = 109,  //家族豪门战公告
    INFO_TYPE_FREE_RETURN = 110, //消费卡免单
    INFO_TYPE_YULIN_JUN = 111,	//羽林军讲话
    INFO_TYPE_INSTRUCTOR = 112, //游戏指导员讲话
    INFO_TYPE_BOSS_BATTLE = 114, //boss争夺战公告类型
    INFO_TYPE_MONARCH_EXP = 115, //帝王经验获得右下角淡紫色公告
    INFO_TYPE_TAIMIAO = 116, //太庙搬砖
    INFO_TYPE_KILL_GREEN_BBOSS = 117, //击杀绿色大BOSS	
    INFO_TYPE_LED = 118, //中央LED格式从右到左公告
    INFO_TYPE_PHONE_GAME = 119,   /// 手机端发来的游戏信息
    INFO_TYPE_PHONE_REWARD_DAY = 120, //手机App每日登陆奖励公告
};

public class ChatMsg
{
    public enumChatType dwType;
    public string channelName;
    public string thisname;
    public string thischat;
}

public class ChatModule : LSingleton<ChatModule>
{
    public ChatNetWork CNetWork;
    public NGUI_Chat uiChat = null;
    public List<string> MsgHistory;
    public int CurrentMsgID;
    public const int MaxCacheMsgCount = 20;

    public List<ChatMsg> cachedMsg = new List<ChatMsg>();

    public Dictionary<enumChatType, string> typeToChannelName = new Dictionary<enumChatType, string>();

    public void Initialize()
    {
        //uiChat = new UI_Chat();
        //uiChat.Init();
        CNetWork = new ChatNetWork();
        CNetWork.Initialize();
        MsgHistory = new List<string>();
        CurrentMsgID = -1;

        typeToChannelName[enumChatType.CHAT_TYPE_COLORWORLD] = "【彩】";
        typeToChannelName[enumChatType.CHAT_TYPE_ALL_ZONE] = "【全区】";
        typeToChannelName[enumChatType.CHAT_TYPE_OVERMAN] = "【盟】";
        typeToChannelName[enumChatType.CHAT_TYPE_WORLD] = "【世】";
        typeToChannelName[enumChatType.CHAT_TYPE_FAMILY] = "【家】";
        typeToChannelName[enumChatType.CHAT_TYPE_AREA] = "【区】";
        typeToChannelName[enumChatType.CHAT_TYPE_COUNTRY] = "【国】";
        typeToChannelName[enumChatType.CHAT_TYPE_FRIEND] = "【友】";
        typeToChannelName[enumChatType.CHAT_TYPE_TEAM] = "【队】";
        typeToChannelName[enumChatType.CHAT_TYPE_UNION] = "【帮】";
        typeToChannelName[enumChatType.CHAT_TYPE_NINE] = "【轻】";
        typeToChannelName[enumChatType.CHAT_TYPE_LOUDSPEAKER] = "【喇叭】";
        typeToChannelName[enumChatType.CHAT_TYPE_MENGZHAN_SOURCE_ZHENYING] = "【盟战】";
        //typeToChannelName[enumChatType.CHAT_TYPE_SYSTEM] = "【系统】";
        
    }

    public void SendChannleChat(string content, enumChatType channelType = enumChatType.CHAT_TYPE_COUNTRY)
    {
        //彩信频道弹出确认框
        if (channelType == enumChatType.CHAT_TYPE_COLORWORLD)
        {
            int leftsendtimes = 0;
            int totaltimes = 0;
            if (GameManager.Instance.MainPlayer.vipInfo != null)
            {
                stSendVipInfo_SC vipInfo = GameManager.Instance.MainPlayer.vipInfo;
                if (vipInfo.level >= 6)
                    totaltimes = vipInfo.level - 5;

                leftsendtimes = totaltimes - vipInfo.usedFreeCS;
                if (leftsendtimes < 0)
                    leftsendtimes = 0;
            }

            if (leftsendtimes > 0)
            {
                NGUIManager.Instance.AddByName<NGUI_Ask>(NGUI_UI.NGUI_Ask, NGUIShowType.ONLYONE, delegate(NGUI_Ask script)
                {
                    script.Init();
                    script.InitDesc("尊贵的VIP,此条彩信免费，今日还剩余" + leftsendtimes.ToString() + "次免费", enumAskType.AskType_SendCaiXin, content);
                });
            }
            else
            {
                Util.Log("人物金子： " + GameManager.Instance.MainPlayer.mainUserData.gold);
                if ((GameManager.Instance.MainPlayer.mainUserData != null) && (GameManager.Instance.MainPlayer.mainUserData.gold <= 5000))
                {
                    NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script1)
                    {
                        script1.Init();
                        script1.InitDesc("金子不足50两！");
                    });
                }
                else
                {
                    NGUIManager.Instance.AddByName<NGUI_Ask>(NGUI_UI.NGUI_Ask, NGUIShowType.ONLYONE, delegate(NGUI_Ask script)
                    {
                        script.Init();
                        script.InitDesc("发送一条彩色信息需要花费50两金子,", enumAskType.AskType_SendCaiXin, content);
                    });
                }
            }
        }
        else if (channelType == enumChatType.CHAT_TYPE_WORLD)
        {
            if (GameManager.Instance.MainPlayer.mainUserData.level < 120)
            {
                MainModule.Instance.uiMainWindow.AddRedTip("您不满120级，不可以使用世界频道聊天");
                return;
            }

            Util.Log("money: " + GameManager.Instance.MainPlayer.money);
            if (GameManager.Instance.MainPlayer.money < 1000)
            {
                NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script1)
                {
                    script1.Init();
                    script1.InitDesc("银子不足10两！");
                });
            }
            else
            {
                NGUIManager.Instance.AddByName<NGUI_Ask>(NGUI_UI.NGUI_Ask, NGUIShowType.ONLYONE, delegate(NGUI_Ask script)
                {
                    script.Init();

                    script.InitDesc("世界频道将花费10两，你确认？", enumAskType.AskType_SendWorld, content);

                });
            }
        }
        else
        {
            if (channelType == enumChatType.CHAT_TYPE_COUNTRY)
            {
                if (GameManager.Instance.MainPlayer.mainUserData.level < 25)
                {
                    MainModule.Instance.uiMainWindow.AddRedTip("您不满25级，不可以使用国家频道聊天");
                    return;
                }
            }

            CNetWork.ReqChannleChat(content, channelType);
            AddMsgToHistory(content);
        }
    }

    public void OnReceiveChatMsg(UMessage msg)
    {
        
    }

    public void AddMsgToHistory(string content)
    {
        if (MsgHistory.Count >= 20)
        {
            MsgHistory.RemoveAt(0);
        }
        MsgHistory.Add(content);
        CurrentMsgID = MsgHistory.Count;
    }

    public string GetPreMsg()
    {
        string str = null;
        if (MsgHistory == null)
        {
            return str;
        }
        if (MsgHistory.Count == 0)
        {
            return str;
        }
        if (CurrentMsgID > 0)
        {
            CurrentMsgID--;
            str = MsgHistory[CurrentMsgID];
        }
        Debug.Log(CurrentMsgID + "   " + MsgHistory.Count);
        return str;
    }

    public string GetNextMsg()
    {
        string str = null;
        if (MsgHistory == null)
        {
            return str;
        }
        if (MsgHistory.Count == 0)
        {
            return str;
        }
        if (CurrentMsgID < MsgHistory.Count-1)
        {
            CurrentMsgID++;
            str = MsgHistory[CurrentMsgID];
        }
        Debug.Log(CurrentMsgID + "   " + MsgHistory.Count);

        return str;
    }
}
