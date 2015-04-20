//客户端与服务器的相同枚举


//角色状态信息
public enum UserState
{
	USTATE_NOSTATE  	= 0,	//无状态
	USTATE_HIDE			= 1,	//隐身
	USTATE_DEATH		= 2,	//死亡
	USTATE_SHENPAN		= 3,	//审判
	USTATE_ZHI_MING_DA_JI = 4,	//致命打击
	USTATE_SILENT		= 5,	//沉默
	USTATE_MAX,
};


//npc类型
public enum NpcType
{
	NPC_TYPE_HUMAN		= 0,	///人型
	NPC_TYPE_NORMAL		= 1,	/// 普通类型
	NPC_TYPE_BBOSS		= 2,	/// 大Boss类型
	NPC_TYPE_LBOSS		= 3,	/// 小Boss类型
	NPC_TYPE_BACKBONE	= 4,	/// 精英类型
	NPC_TYPE_GOLD		= 5,	/// 黄金类型
	NPC_TYPE_TRADE		= 6,	/// 买卖类型
	NPC_TYPE_TASK		= 7,	/// 任务类型
	NPC_TYPE_GUARD		= 8,	/// 士兵类型
	NPC_TYPE_PET		= 9,	/// 宠物类型
	NPC_TYPE_BACKBONEBUG= 10,	/// 精怪类型
	NPC_TYPE_SUMMONS	= 11,	/// 召唤类型
	NPC_TYPE_TOTEM		= 12,	/// 图腾类型
	NPC_TYPE_AGGRANDIZEMENT = 13,/// 强化类型
	NPC_TYPE_ABERRANCE	= 14,	/// 变异类型
	NPC_TYPE_STORAGE	= 15,	/// 仓库类型
	NPC_TYPE_ROADSIGN	= 16,	/// 路标类型
	NPC_TYPE_TREASURE	= 17,	/// 宝箱类型
	NPC_TYPE_WILDHORSE	= 18,	/// 野马类型
	NPC_TYPE_MOBILETRADE	= 19,	/// 流浪小贩
	NPC_TYPE_LIVENPC	= 20,	/// 生活npc（不战斗，攻城时消失）
	NPC_TYPE_DUCKHIT	= 21,	/// 蹲下才能打的npc
	NPC_TYPE_BANNER		= 22,	/// 旗帜类型
	NPC_TYPE_TRAP		= 23,	/// 陷阱类型
	NPC_TYPE_MAILBOX	=24,	///邮箱
	NPC_TYPE_AUCTION	=25,	///拍卖管理员
	NPC_TYPE_UNIONGUARD	=26,	///帮会守卫
	NPC_TYPE_SOLDIER	=27,	///士兵，只攻击外国人
	NPC_TYPE_UNIONATTACKER	=28,	///攻方士兵
	NPC_TYPE_SURFACE = 29,	/// 地表类型
	NPC_TYPE_CARTOONPET = 30,	/// 替身宝宝
	NPC_TYPE_PBOSS = 31,	/// 紫色BOSS
	NPC_TYPE_RESOURCE = 32,  /// 资源类NPC
	NPC_TYPE_BUILDING = 33, /// 建筑类NPC
	NPC_TYPE_WOOD =36,       ///木材类npc只可以斧子砍
	NPC_TYPE_SEPT_BUILD = 37,//家族建筑，非家族成员都可攻击
	NPC_TYPE_SEPT_HOME = 38,	///家族房屋，显示名字归属谁
	NPC_TYPE_CANTALKSUBHUMANG =39,    //能说话的的类人NPC
	NPC_TYPE_CANNON = 40,    //大炮类NPC
	NPC_TYPE_INVISIBLE = 41, //透明NPC
	NPC_TYPE_FLOWER_BALL = 42,//绣球
	NPC_TYPE_SPRITE_TRAP = 43,//捕捉宠物的陷进
	NPC_TYPE_TASK_PLANT	 = 44, /// 个人种植任务的果树NPC
	NPC_TYPE_SEPT_TOWER = 45, //家族箭塔NPC
	NPC_TYPE_BLESS_ALLC_CITY = 46, //福满城活动的大树
	NPC_TYPE_WILDMOUNT	= 47,	//可捕捉的野生坐骑
	NPC_TYPE_TANKBOSS = 48, // 仅战车可以攻击
	NPC_TYPE_ARENAROBOT = 49,  // 竞技场机器人
	NPC_TYPE_PARTER = 50,		//伙伴
};

//移动方向定义
public enum MoveDirction
{
	USER_DIR_UP			= 0,	//向上
	USER_DIR_UPRIGHT	= 1,	//右上
	USER_DIR_RIGHT		= 2,	//向右
	USER_DIR_RIGHTDOWN	= 3,	//右下
	USER_DIR_DOWN		= 4,	//向下
	USER_DIR_DOWNLEFT	= 5,	//左下
	USER_DIR_LEFT		= 6,	//向左
	USER_DIR_LEFTUP		= 7,	//左上
	USER_DIR_WRONG		= 8,	//错误方向
};

