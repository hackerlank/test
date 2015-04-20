using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Net;
using System;

public class MsgVar
{
    public const byte NULL_USERCMD_PARA = 0;

    /// <summary>
    /// 聊天相关消息
    /// </summary>
    public const byte CHAT_USERCMD = 14;
    public const byte ALL_CHAT_USERCMD_PARAMETER = 1;


    /// <summary>
    /// 时间指令
    /// </summary>
    public const byte TIME_USERCMD			= 2;
    public const byte USERGAMETIME_TIMER_USERCMD_PARA  = 3;

    //选择指令
    public const byte SELECT_USERCMD  = 24;
    public const byte USERINFO_SELECT_USERCMD_PARA = 1;

    // 登陆指令
    public  const byte LOGON_USERCMD = 104;
    public const byte LOGIN_SELECT_USERCMD_PARA = 3;
    public const byte USER_RELOGIN_PARA = 14;        


    //任务指令
    public const byte TASK_USERCMD = 23;
    public const byte REQUEST_QUEST_PARA = 13;

    //礼包指令
    public const byte GIFTBAG = 79;
    public const byte BUY_GIFTBAG_USERCMD_PARA = 1;

    //主界面PHONE指令
    public const byte PHONE_USERCMD = 165;
    public const byte SEND_USERINFO_SC = 1;
    public const byte SEND_OFFICIAL_SC = 2;
    public const byte SEND_ERRORINFO_CS = 3;
    //请求每日登陆奖励
    public const byte REQ_DAY_REWARD_CS = 4;


    //////////////////////////////////////////////////////////////////////////
    //数据指令
    public const byte DATA_USERCMD			= 3;
    public const byte MAIN_USER_DATA_USERCMD_PARA = 2;


    //////////////////////////////////////////////////////////////////////////
    //道具指令
    public const byte PROPERTY_USERCMD = 4;
    //添加用户道具
    public const byte ADDUSEROBJECT_PROPERTY_USERCMD_PARAMETER = 1;
    //批量添加道具
    public const byte ADDUSEROBJECT_LIST_PROPERTY_USERCMD_PARAMETER = 41;
    //删除道具
    public const byte REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER = 2;


    ////////////////////////////////////////////////////////////////////////
    //新版指令处理 
    public const byte NEWZT_MASSIVE_USERCMD = 200;
    //移动物品到寄存包裹
    public const byte REQ_VIP_MOVE_OBJECT_TOCS_CS = 154;
    //VIP信息
    public const byte SEND_VIP_INFO_SC = 152;

    ////////////////////////////////////////////////////////////////////////
    //点卡金子指令
    public const byte GOLD_USERCMD         = 31;
    //查询点卡
    public const byte REQUEST_POINT_PARA = 8;
    //查询返回
    public const byte RETURN_REQUEST_POINT_PARA = 9;

}


[StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
public class stNullUserCmd
{
	public byte byCmd;
    public byte byParam;
	public uint  dwTimestamp;
};

[StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
public class stChatUserCmd : stNullUserCmd
{
    public stChatUserCmd()
    {
        byCmd = MsgVar.CHAT_USERCMD;
    }
};

[StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
public class stChannelChatUserCmd : stChatUserCmd
{
    public uint dwType;           /**< 消息类型 */
    public uint dwSysInfoType;        /**< 系统消息的类型 */ //表情类型
    public uint dwCharType;       /**< 角色类型 */    /*-1: 表示是从征途嘟嘟中发来的消息*/
    public uint dwChannelID;

    public uint dwFromID;         ///发送者的ID,是临时id
    public uint dwChatTime;           // 谈话时间
    public byte size1;              // 物品数量
    public StringLengthFlag L32;
    public string pstrName;
    public StringLengthFlag L256;
    public string pstrChat;
    public uint dwThisID;

    public stChannelChatUserCmd()
    {
        size1 = 0;
        dwFromID = 0;
        //dwType = 12;
        //dwChannelID = 0;
        dwSysInfoType = 0;
        dwCharType = 1;
        //dwThisID = 0;
        dwChatTime = 0;
        byParam = MsgVar.ALL_CHAT_USERCMD_PARAMETER;
    }
};

//从服务器发过来的消息
[StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
public class stServerChannelChatUserCmd: stNullUserCmd
{
	public uint dwType;           /**< 消息类型 */
    public uint dwSysInfoType;        /**< 系统消息的类型 */ //表情类型
    public uint dwCharType;       /**< 角色类型 */    /*-1: 表示是从征途嘟嘟中发来的消息*/
    public uint dwChannelID;
    public uint dwFromID;         ///发送者的ID,是临时id
    public uint dwChatTime;           // 谈话时间
    public byte size;              // 物品数量
    public uint dwThisID;
 
    public stServerChannelChatUserCmd()
	{
        byCmd = MsgVar.CHAT_USERCMD;
        byParam = MsgVar.ALL_CHAT_USERCMD_PARAMETER;

        dwType = 12;
        dwChannelID = 0;
        dwSysInfoType = 0;
        dwCharType = 1;
        dwThisID = 0;
        size = 0;
        dwChatTime = 0;
	}
};



// 用户向网关发送当前游戏时间
[StructLayout(LayoutKind.Sequential, Pack = 1)]
class stUserGameTimeTimerUserCmd : stNullUserCmd
{
    public stUserGameTimeTimerUserCmd()
    {
        byCmd = MsgVar.TIME_USERCMD;
        byParam = MsgVar.USERGAMETIME_TIMER_USERCMD_PARA;
    }

    public uint dwUserTempID;			/**< 用户临时ID */
    public UInt64 qwGameTime;		/**< 用户游戏时间 */
};

//选择指令
public class stSelectUserCmd : stNullUserCmd
{
	public stSelectUserCmd()
	{
		byCmd = MsgVar.SELECT_USERCMD;
	}
};

/// 角色信息
//[StructLayout(LayoutKind.Sequential, Pack = 1), Serializable]
//public class SelectUserInfo
//{
//        public UInt32 id;                                        /// 角色编号
//        StringLengthFlag L33;
//        public string name;                             /// 角色名称
//        public UInt16 type;                                      /// 角色类型
//        public UInt16 level;                                     /// 角色等级
//        public UInt32 mapid;                          /// 角色所在地图编号
//        StringLengthFlag L33;
//        public string mapName;                  /// 角色所在地图名称
//        public UInt16 country;                                   ///     国家ID
//        public UInt16 face;
//        StringLengthFlag L33;
//        public string countryName;              /// 国家名称
//        public UInt32 bitmask;                                  /// 角色掩码
//        public UInt64 forbidTime;	                            //封号时间
//        public UInt32 zone_state;                               /// 状态0,2时可以登录，为1时，表示角色已到战区，为3时，表示已回原区
//        public UInt32 target_zone;                              /// 目标区ID, 当zone_state,为1时，有效
//        //      public UInt16 five;                                              ///     五行主属性
//        //      public UInt32 unionid;        /// 帮会ID

//        public UInt32 round;							         //是否转生
//        public UInt32 icqmask;
//        public byte acceptPK; //是否同意PK 0:反对 1:赞成
//        public UInt32 oldPlayerLastTime; //是否为老玩家
//};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stUserInfoUserCmd : stSelectUserCmd
{
	public stUserInfoUserCmd()
	{
		byParam = MsgVar.USERINFO_SELECT_USERCMD_PARA;
		size = 0;
        //charInfo = new SelectUserInfo[MsgVar.MAX_CHARINFO];
	}
    //不支持内嵌结构体，先这样处理
    //碰到字符串或用来表示名字的字符数组统一用string处理，string前面字段为长度标志字段，格式为 StringLengthFlag Lxxx;
    //其中xxx为字符串长度数字,如果名字冲突则在L后面加0,如L0xxx L00xxx ...
    public UInt32 id;       
    public StringLengthFlag L33;
    public string name; 
    public UInt16 type; 
    public UInt16 level;
    public UInt32 mapid;
    public StringLengthFlag L033;
    public string mapName;
    public UInt16 country;
    public UInt16 face;
    public StringLengthFlag L0033;
    public string countryName;
    public UInt32 bitmask;
    public UInt64 forbidTime;
    public UInt32 zone_state;
    public UInt32 target_zone;
    public UInt32 round;
    public UInt32 icqmask;
    public byte acceptPK;
    public UInt32 oldPlayerLastTime;

    public UInt32 id2;
    public StringLengthFlag L00033;
    public string name2;
    public UInt16 type2;
    public UInt16 level2;
    public UInt32 mapid2;
    public StringLengthFlag L000033;
    public string mapName2;
    public UInt16 country2;
    public UInt16 face2;
    public StringLengthFlag L0000033;
    public string countryName2;
    public UInt32 bitmask2;
    public UInt64 forbidTime2;
    public UInt32 zone_state2;
    public UInt32 target_zone2;
    public UInt32 round2;
    public UInt32 icqmask2;
    public byte acceptPK2;
    public UInt32 oldPlayerLastTime2;

	public uint size;
};

public class stLoginSelectUserCmd : stSelectUserCmd
{
	uint charNo;
	int x1;
	int y1;
	int x2;
	int y2;

	public stLoginSelectUserCmd(uint _charNo, int _x1, int _y1, int _x2, int _y2)
	{
		byParam = MsgVar.LOGIN_SELECT_USERCMD_PARA;
		charNo = _charNo;
		x1 = _x1;
		y1 = _y1;
		x2 = _x2;
		y2 = _y2;
	}
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stQuestUserCmd : stNullUserCmd
{
	public stQuestUserCmd()
	{
		byCmd = MsgVar.TASK_USERCMD;	
	}
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stRequestQuestUserCmd : stQuestUserCmd
{
	public stRequestQuestUserCmd()
	{
		byParam = MsgVar.REQUEST_QUEST_PARA;
	}

    public UInt32 id; //任务id
    public StringLengthFlag L16;
	public string target; //目标
	public byte offset; //任务分支
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stGiftBagUserCmd : stNullUserCmd 
{
	public stGiftBagUserCmd() 
	{       
		byCmd = MsgVar.GIFTBAG;
	}
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stBuyGiftBagUserCmd : stGiftBagUserCmd 
{
    public UInt32 bagID;    //道具ID
    public byte type;// 购买类型，1-红包抵扣, 2-消费推送, 其它的目前都为0  
    public stBuyGiftBagUserCmd()
    {
        byParam = MsgVar.BUY_GIFTBAG_USERCMD_PARA;
        type = 0;
    }
};

[StructLayout(LayoutKind.Sequential, Pack = 1)]
 public class stPhoneCommand : stNullUserCmd
 {
      public stPhoneCommand()
      {
          byCmd = MsgVar.PHONE_USERCMD;
      }
};

//服务器-》手机端 发送玩家基本数据
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stSendUserInfoSC : stPhoneCommand
{
    public stSendUserInfoSC()
    {
        byParam = MsgVar.SEND_USERINFO_SC;
        vipLevel = 0;
        face = 0;
        zoneName = "";
        septName = "";
        unionName = "";
        countryName = "";
        chenghao = "";
    }
    public byte vipLevel;
    public UInt16 face;
    public StringLengthFlag L33;
    public string zoneName;
    public StringLengthFlag L033;
    public String septName;
    public StringLengthFlag L0033;
    public String unionName;
    public StringLengthFlag L5;
    public String countryName;
    public StringLengthFlag L00033;
    public String chenghao;
};

//发送玩家称号 如果为空就是取消称号
public class stSendOfficialSC : stPhoneCommand
{
    public stSendOfficialSC()
    {
        byParam = MsgVar.SEND_OFFICIAL_SC;
        chenghao = "";
    }
    public StringLengthFlag L33;
    public string chenghao;                                                             
};  


//发送玩家错误信息
public class stSendErrorInfo : stPhoneCommand
{
    public stSendErrorInfo()
    {
        byParam = MsgVar.SEND_ERRORINFO_CS;
        size = 0;
    }
    
    public UInt16 size;
    //char info[0];
};

public class stReqDayReward : stPhoneCommand
{       
    public stReqDayReward()
   {       
        byParam = MsgVar.REQ_DAY_REWARD_CS;
   }
};


[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class stPropertyUserCmd : stNullUserCmd
{
    public stPropertyUserCmd()
    {
        byCmd = MsgVar.PROPERTY_USERCMD;
    }
};

/// 用户道具数据
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public class  stAddObjectPropertyUserCmd : stPropertyUserCmd
{
    public stAddObjectPropertyUserCmd()
    {
        byParam = MsgVar.ADDUSEROBJECT_PROPERTY_USERCMD_PARAMETER;
    }
    //不支持结构体嵌套 分开2个类读
    //StObject obj
};

public class StObject
{
    public byte byActionType; /**< 物品动作类型 */
    
    public UInt32 qwThisID;   //物品唯一id
	public UInt32 dwObjectID;  ////物品类别id
    public StringLengthFlag L32;
    public string strName;

    public UInt32 dwLocation;//格子类型
    public UInt32 dwTableID;	// 包袱ID
    public UInt16 x;
    public UInt16 y;

	public UInt32 dwNum;	// 数量
	public byte upgrade;//物品升级等级
	public byte kind;	//物品类型, 0普通, 1蓝色, 2金色, 4神圣, 8套装
	public UInt32 exp;  //道具经验

	public UInt16 needlevel;				// 需要等级

	public UInt16 maxhp;					// 最大生命值
	public UInt16 maxmp;					// 最大法术值
	public UInt16 maxsp;					// 最大体力值

	public UInt16 pdamage;				// 最小攻击力
	public UInt16 maxpdamage;			// 最大攻击力
	public UInt16 mdamage;				// 最小法术攻击力
	public UInt16 maxmdamage;			// 最大法术攻击力

	public UInt16 pdefence;				// 物防
	public UInt16 mdefence;				// 魔防
	public byte damagebonus;			// 伤害加成 x% from 道具基本表
	public byte damage;				// 增加伤害值x％ from 神圣装备表

	public UInt16 akspeed;				// 攻击速度 
	public UInt16 mvspeed;				// 移动速度
	public UInt16 atrating;				// 命中率
	public UInt16 akdodge;				// 躲避率

	public UInt32 color;				// 颜色	 //在"新中立区运镖令"中,color表示该道具的到期时间,剩余时间需要自己计算

	public UInt16 str;  // 力量
	public UInt16 inte;  // 智力
	public UInt16 dex;  // 敏捷
	public UInt16 spi;  // 精神
	public UInt16 con;  // 体质(斗魂护佑 0 未护佑，1 护佑过)

	public UInt16 fivetype;  // 五行属性
	public UInt16 fivepoint; // 五行属性

	public UInt16 hpr;  // 生命值恢复
	public UInt16 mpr;  // 法术值恢复
	public UInt16 flag;  // 体力值恢复

	public UInt16 holy;  //神圣一击	
	public UInt16 bang;  //重击
	public UInt16 pdam;  // 增加物理攻击力
	public UInt16 pdef;  // 增加物理防御力
	public UInt16 mdam;  // 增加魔法攻击力
	public UInt16 mdef;  // 增加魔法防御力

	public UInt16 poisondef; //抗毒增加  //在"新中立区运镖令"中,poisondef表示最大时间
	public UInt16 lulldef; //抗麻痹增加
	public UInt16 reeldef; //抗眩晕增加

    public UInt16 evildef; // aptitude herodef 抗噬魔增加,资质,英雄强化:高2位--01,抗反射,10,抗忽视；11，抗重击。最大值50

	public UInt16 bitedef; //额外增加属性值，197装备独有
	public UInt16 chaosdef; //抗混乱增加
	public UInt16 colddef; //抗冰冻增加
	public UInt16 petrifydef; //抗石化增加
	public UInt16 blinddef; //抗失明增加
	public UInt16 stabledef; //抗定身增加-------------------李兴钢:祈福包,会将抗定身和抗减速合起来表示一个隐藏属性(获得祈福包的时间)
	public UInt16 slowdef; //抗减速增加
	public UInt16 luredef; //抗诱惑增加

	public UInt16 durpoint; //恢复装备耐久度点数
	public UInt16 dursecond; //恢复装备耐久度时间单位

    /////////////////////////////////////////////////////////
    //    struct skillbonus {
    //    public UInt16 id; //技能 id
    //    public UInt16 point; // 技能点数
    //} skill[10]; //技能加成（最后两个用于时效性装备）
    //
    //技能40个字节用string解析
    public StringLengthFlag L40;
    public string skill;

    //struct skillsbonus {
    //    public UInt16 id; //技能 id
    //    public UInt16 point; // 技能点数
    //} skills;	//全系技能加成
    public UInt16 skillsbonus_id;
    public UInt16 skillsbonus_point;


	public UInt16 poison; //中毒增加   	李兴钢:在挖宝铲子中,这个值变成,金铲子的x坐标
	public UInt16 lull; //麻痹增加			李兴钢:在挖宝铲子中,这个值变成,金铲子的y坐标
	public UInt16 reel; //眩晕增加
	public UInt16 evil; //噬魔增加
	public UInt16 bite; //噬力增加
	public UInt16 chaos; //混乱增加
	public UInt16 cold; //冰冻增加
	public UInt16 petrify; //石化增加
	public UInt16 blind; //失明增加
	public UInt16 stable; //定身增加
	public UInt16 slow; //减速增加
	public UInt16 lure; //诱惑增加

    //struct leech
    //{
    //    BYTE odds;    //x
    //    public UInt16 effect;	//y
    //};
    
    public byte hpleech_odds;
    public UInt16 hpleech_effect;

    public byte mpleech_odds;
    public UInt16 mpleech_effect;

    //leech hpleech; //x%吸收生命值y
    //leech mpleech; //x%吸收法术值y

	public byte hptomp; //转换生命值为法术值x％
	public byte dhpp; //物理伤害减少x%	
	public byte dmpp; //法术伤害值减少x%      （这个属性值 变为资质的值） 	

	public byte incgold; //增加银子掉落x%		(1:神石; 2:灵石; 3:补天石; 4:强化神石; 5:再强化; 6:神化; 7,8,9,10:神化三强)
	public byte doublexp; //x%双倍经验		
	public byte mf; //增加掉宝率x%

	public byte bind;  //装备是否绑定

	//五行套装相关属性
	public byte dpdam; //物理伤害减少%x
	public byte dmdam; //法术伤害减少%x
	public byte bdam; //增加伤害x%
	public byte rdam; //伤害反射%x
	public byte ignoredef; //%x忽视目标防御

	//public UInt16 fiveset[5]; //五行套装, 按顺序排列
    public StringLengthFlag L10;
    public string fiveset;

	//...
	public byte width;  //宽度
	public byte height; //高度
	public UInt16 dur;    //当前耐久
	public UInt16 maxdur; //最大耐久

    public StringLengthFlag L24;
    public string socket; //孔
	//public UInt32 socket[6]; //孔
	public UInt32 price;     //价格
	public UInt32 cardpoint; //点卡   高16为为转生等级

    public StringLengthFlag L032;
    public string maker;
	//char maker[MAX_NAMESIZE]; //打造者
};


public class stAddObjectListPropertyUserCmd : stPropertyUserCmd
{
	public stAddObjectListPropertyUserCmd()
	{
		byParam = MsgVar.ADDUSEROBJECT_LIST_PROPERTY_USERCMD_PARAMETER;
		num=0;
	}
	public UInt16 num;
    //struct
    //{
    //    BYTE byActionType;          /**< 物品动作类型 */
    //    t_Object object;              /**< 物品数据 */
    //}list[0];
};

public class stRemoveObjectPropertyUserCmd : stPropertyUserCmd 
{
    public stRemoveObjectPropertyUserCmd()
    {
        byParam = MsgVar.REMOVEUSEROBJECT_PROPERTY_USERCMD_PARAMETER;
    }
    public UInt32 qwThisID;
};


//新版指令处理 
public class stNewZTMassiveCommand : stNullUserCmd
{
        public stNewZTMassiveCommand()
        {
                byCmd = MsgVar.NEWZT_MASSIVE_USERCMD;
        }
};


//移动物品到寄存包裹
public class stReqVipMoveObjectToCS_CS : stNewZTMassiveCommand
{
	public stReqVipMoveObjectToCS_CS()
	{
		byParam = MsgVar.REQ_VIP_MOVE_OBJECT_TOCS_CS;
		thisID = 0;
	}
	public UInt32 thisID;
};


public class stGoldUserCmd : stNullUserCmd
{
	public stGoldUserCmd()
	{
		byCmd = MsgVar.GOLD_USERCMD;
	}
};

//查询点卡
public class stRequestPoint :  stGoldUserCmd
{
	public stRequestPoint()
	{
		byParam = MsgVar.REQUEST_POINT_PARA;
		type = 0;
	}
	byte type;//0为巨人加征途一起请求 1为只请求巨人点数
};

//查询点卡返回
public class stReturnRequestPoint :  stGoldUserCmd
{
	public stReturnRequestPoint()
	{
		byParam =MsgVar. RETURN_REQUEST_POINT_PARA;
		dwPoint = 0;
		byReturn = 0;
		ZTPoint = 0;
	}
	public UInt32 dwPoint;//剩余点数(巨人点数+征途点数)
	public byte byReturn;//查询返回类型
	public UInt32 ZTPoint; //征途点数
};


//发送VIP信息到玩家
public class stSendVipInfo_SC : stNewZTMassiveCommand
{
	public stSendVipInfo_SC()
	{
		byParam = MsgVar.SEND_VIP_INFO_SC;
		level = 0;
		usedFreeCS = 0;
		giveFlag = 0;
		point = 0;
	}
	public byte level;     // VIP等级
	public byte usedFreeCS;        // 已使用免费彩世次数 
	public UInt16 giveFlag;  // 奖励领取标识  右起10位表示1-10级 0.未领取1.已领取
	public UInt32 point;    // 当前消费点数
};

//数据指令
public class stDataUserCmd : stNullUserCmd
{
	public stDataUserCmd()
	{
		byCmd = MsgVar.DATA_USERCMD;
	}
};

/// 主用户数据 
public class stMainUserDataUserCmd : stDataUserCmd 
{
	public stMainUserDataUserCmd()
	{
		byParam = MsgVar.MAIN_USER_DATA_USERCMD_PARA;
	}

	public UInt32 dwUserTempID;             /// 用户临时ID
	public UInt16  level;                    /// 角色等级
	public UInt16  monarchLevel;				/// 帝王觉醒等级
	public UInt32 round;                    ///第几次转生
	public UInt32 hp;                       /// 当前生命值
	public UInt32 maxhp;                    /// 最大生命值
	public UInt32 resumehp;                 /// 生命值恢复
	public UInt32 mp;                       /// 当前法术值
	public UInt32 maxmp;                    /// 最大法术值
	public UInt32 resumemp;                 /// 法术值恢复
	public UInt32 sp;                       /// 当前体力值
	public UInt32 maxsp;                    /// 最大体力值
	public UInt32 resumesp;                 /// 体力值恢复
	public UInt32 pdamage;                  /// 最小物理攻击力
	public UInt32 maxpdamage;               /// 最大物理攻击力
	public UInt32 mdamage;                  /// 最小法术攻击力
	public UInt32 maxmdamage;               /// 最大法术攻击力
	public UInt32 pdefence;                 /// 物理防御力
	public UInt32 mdefence;                 /// 法术防御力
	public UInt64 exp;                      /// 当前经验值
	public UInt64 nextexp;                  /// 升级经验值
	public UInt16  attackrating;             /// 攻击命中
	public UInt16  attackdodge;              /// 攻击躲避
	public UInt16  bang;                     /// 重击
	public UInt16  lucky;                    /// 幸运值
	public UInt16  charm;                    /// 魅力值
                                             /// 
	public UInt16 wdCon; //体质
	public UInt16 wdStr; //体力
	public UInt16 wdDex; //敏捷
	public UInt16 wdInt; //智力
	public UInt16 wdMen; //精神

	public UInt16  skillPoint;                   /// 技能点数
	public UInt16  points;                       /// 点数
	public UInt32 country;                      /// 国家
	public UInt16  pkmode;                       /// pk模式


	public UInt32 stdpdamage;                   /// 标准物理攻击力
	public UInt32 stdmdamage;                   /// 标准法术攻击力
	public UInt32 stdpdefence;                  /// 标准物理防御力
	public UInt32 stdmdefence;                  /// 标准法术防御力
	public UInt16  stdbang;                      /// 标准重击率

	public UInt16 wdStdCon;  //体质
	public UInt16 wdStdStr;  //体力
	public UInt16 wdStdDex;  //敏捷
	public UInt16 wdStdInt;  //智力
	public UInt16 wdStdMen;  //精神

	public UInt16 wdTire; /// 疲劳状态 0为非 1为疲劳
	public UInt32 fivetype; ///五行类型
	public UInt32 fivepoint;///五行点数
	public UInt32 honor;///荣誉值
	public UInt32 maxhonor;///最大荣誉值
	public UInt32 gold; ///金币数
	public UInt32 ticket; //点券数
	public UInt32 bitmask;                  /// 角色掩码
	public UInt32 salary; // 工资数
	public UInt32 bind_money;       //绑定银币
	public UInt32 travel_time; // 进入旅游区的时间
	public UInt32 grace;
	public UInt32 winBuffTime; //风雷翼buff结束时间
	public UInt32 juejiAttackTime;  //绝技攻击祝福
	public UInt16  newProfessionFlag; // 新职业进阶标识,按位右起标识 1.战士(后续职业进阶可用该字段)
};


public class stLogonUserCmd : stNullUserCmd
{
	public stLogonUserCmd()
	{
		byCmd = MsgVar.LOGON_USERCMD;
	}
};

public class stUserReLoginCmd : stLogonUserCmd
{
	public stUserReLoginCmd()                 
	{
		byParam = MsgVar.USER_RELOGIN_PARA;
	}
}; 



