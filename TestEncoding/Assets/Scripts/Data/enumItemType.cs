using UnityEngine;
using System.Collections;

enum enumItemType
{
    ItemType_None = 0,

    ItemType_Resource = 16,	//16代表原料类
    ItemType_Leechdom,	//17代表药品类
    ItemType_FoodRes,	//18代表食物原料类
    ItemType_Food,		//19代表食物类
    ItemType_Tools,		//20代表劳动工具类
    ItemType_Arrow,		//21代表配合弓使用的箭支类
    ItemType_FightHorse,	//22枣红战马
    ItemType_Pack,		//23代表包裹类
    ItemType_Money,		//24代表银子类
    ItemType_Scroll = 25,	//25代表转移卷轴类
    ItemType_Move,		//26代表特殊移动道具类
    ItemType_LevelUp,	//27代表道具升级需要的材料类
    ItemType_AddLevelUpPercent,	//28代表增加道具升级成功率的物品.
    ItemType_Union,	//29代表创建帮会需要的道具.
    ItemType_Tonic,	//30表示自动补药类道具.
    ItemType_Gift,	//31代表礼品类物品.
    ItemType_Other,
    ItemType_MASK = 33, 	//33代表蒙面巾
    ItemType_Quest = 34,
    ItemType_HORSE = 35,	//枣红马
    ItemType_SOULSTONE = 37, //37代表魂魄石类
    ItemType_Change = 41,   //41 代表合成道具
    ItemType_SkillUp = 44,   //43 代表技能升级道具
    ItemType_PackHide = 45,		// 代表仓库
    ItemType_ClearPoint,  // 46代表只能〈40级的玩家使用的洗点，40级以上为34(任务物品)
    ItemType_Repair = 47, //47代表修复宝石类
    ItemType_DoubleExp = 52, //52代表自动补给类型
    ItemType_Honor = 53, //53代表荣誉之星类型
    ItemType_State = 51, //51代表状态道具
    ItemType_ZhenSkillUp = 57,	//代表阵发书籍
    ItemType_FillHP = 58, //58代表大剂量道具
    ItemType_ClearPoint2 = 59, // 洗5点属性之一
    ItemType_Amulet = 61, // 护身符类道具
    ItemType_FillMP = 62, //59代表自动补兰道具
    ItemType_Exp = 66, // 1.5经验道具
    ItemType_Exp1 = 67, // 1.25经验道具
    ItemType_Exp2 = 68, // 1.75经验道具
    ItemType_Exp3 = 69, // 1.5经验道具 (按照分钟来算)
    ItemType_CanNotDestroy = 70,	//不可销毁的道具
    ItemType_BicycleItem = 71, //自行车大赛时使用的状态道具

    ItemType_HomePetFood = 72, //充物食物
    ItemType_GiveGift = 73,		//官方礼包类道具

    ItemType_New_OneTwoFiveExp = 74,  //新1.25倍经验道具
    ItemType_New_OneFiveExp = 75,  //新1.5倍经验道具	
    ItemType_New_OneSevenFiveExp = 76,  //新1.75倍经验道具
    ItemType_New_ShenYao = 77,  //新神药道具
    ItemType_BlessStone = 78,   //祝福水
    ItemType_SeaWarItem = 79,  //海战道具

    ItemType_HuShenFu = 80,  //护身符
    ItemType_TaskObj = 81,		//时效道具

    ItemType_YiJianHuanZhuang = 82,  //一键换装类道具
    ItemType_GeniusSkillBook = 83,	//天赋技能书
    ItemType_Furniture = 99,		//99代表家具

    ItemType_ClothBody = 101,		//101代表布质加生命类服装
    ItemType_FellBody = 102,		    //102代表皮甲加魔防类服装
    ItemType_MetalBody = 103,		//103代表金属铠甲加物防类服装
    ItemType_Blade = 104,		    //104代表武术刀类武器
    ItemType_Sword = 105,	        //105代表武术剑类武器
    ItemType_Axe = 106,	           //106代表武术斧类武器
    ItemType_Hammer = 107,	        //107代表武术斧类武器
    ItemType_Staff = 108,		    //108代表法术杖类武器
    ItemType_Bow = 109,	          //109代表箭术弓类武器
    ItemType_Fan = 110,	           //110代表美女扇类
    ItemType_Stick = 111,	        //111代表召唤棍类武器
    ItemType_Shield = 112,	//112代表盾牌类
    ItemType_Helm = 113,		//113代表角色头盔类
    ItemType_Caestus = 114,	//114代表角色腰带类
    ItemType_Cuff = 115,		//115代表角色护腕类
    ItemType_Shoes = 116,		//116代表角色鞋子类
    ItemType_Necklace = 117,	//117代表角色项链类
    ItemType_Fing = 118,		//118代表角色戒指类
    ItemType_FashionBody = 119,		//119代表时装
    ItemType_Flower = 120,           //玫瑰花
    ItemType_BMW = 121,             //119代表宝马

    ItemType_FashionBody2 = 122,		//122代表时装
    ItemType_FashionBody3 = 123,		//123代表不改变外型的时装
    ItemType_FashionBody4 = 124,         //124类型时装//-------------------李兴钢------------------124时装,现在添加一种新类型,可以装备在右上角饰品(13)的那种......单号:14535
    ItemType_FashionBody5 = 125,         //125类型时装

    ItemType_GiftBox = 126,     //宝盒开奖用的宝箱
    ItemType_GiftBox_Key = 127,     //宝盒开奖用的钥匙
    ItemType_Add_Exploit = 128,     //增加功勋道具
    ItemType_TankObject = 129,		//战车辅助物品
    ItemType_HorseShoe = 130,     //130 马蹄铁
    ItemType_HorseRope = 131,     //131 马绳
    ItemType_HorseSaddle = 132,     //132 马鞍
    ItemType_HorseSafe = 133,     //133 护马铠
    ItemType_HorseIron = 134,     //134 马镫
    ItemType_QuMoFu = 135,   //驱魔符类物品
    ItemType_LiRen = 136,		//新武器利刃类型
    ItemType_Jian = 137,		//新武器剑的类型
    ItemType_HuXinMirror = 138,//新装备护心镜类型
    ItemType_HorseFashion = 140,//马匹时装
    ItemType_Gem = 141,			//旗(法师)
    ItemType_ArrowBag = 142,			//旗(弓手)
    ItemType_Bottle = 143,			//旗(仙术)
    ItemType_Flag = 144,			//旗(召唤)
    ItemType_BladeFlag = 145,			//旗(战士)
    ItemType_DaggerFlag = 146,			//旗(刺客)
    ItemType_SwordFlag = 147,			//旗(卫士)
    ItemType_NewMedicine = 148,			//一种新的药品类型,可以转动灰色的冷却时间

    ItemType_ShengLingFeather = 149,      //圣灵羽道具
    ItemType_MagicYaoShui = 150,        //魔力药水
    ItemType_GradeMedal = 151,              //段位勋章道具

    ItemType_DragonBless = 152,           //龙道具类型

    ItemType_HorseAppearance = 153,       //马匹外形兑换


    ItemType_Pike = 155, //新职业：长枪武器
    ItemType_Javalin = 156, //新职业：短枪护盾

    ItemType_Knife = 157, //进阶战士：小刀

    ItemType_HorseFashion2 = 200,  // 马匹时装2

    ItemType_Perfection_UpStone = 201, //完美的升级宝石	
    ItemType_PurpleEquipStone = 203,   //补天神石
    ItemType_GreenEquipStone = 204,		//补天灵石
    ItemType_GoldEquipStone = 205,		//补天石
    ItemType_Liaozhai = 206,            //聊斋系列任务道具
    ItemType_Fangqi = 207,              //房契	
    ItemType_HomeTroubleMaker = 208,        //房屋捣乱类道具

    ItemType_VirtualObj = 209,			//礼包宝箱中，虚拟道具类型
    ItemType_GeographyFound = 210,          //探险地图道具
    ItemType_Resource_ZLP6 = 211,           //资料片6资源
    ItemType_TankHeart = 212,       //战车装备-核心
    //ItemType_TankCannon = 213,      //战车装备-火炮
    //ItemType_TankShield = 214,      //战车装备-护甲
    //ItemType_TankWheel = 215,       //战车装备-轮子
    //ItemType_TankChassis = 216,     //战车装备-底盘
    ItemType_AntiquityStone_4 = 217,    //强化补天神石
    ItemType_NewBox = 218,	//新宝箱
    ItemType_SaintEquipStone = 219,		//补天圣石
    ItemType_ChildFashion = 220,    //小孩时装
    ItemType_ChangeFace = 221,      //换头像道具
    ItemType_BaoMingDan = 222,		//保命丹
    ItemType_GouHuo = 223,			//篝火类道具
    ItemType_Skill = 224,   //技能类道具
    ItemType_ChildEffect = 225,     //小孩特效道具
    ItemType_Composition = 226,		//合成出来的道具 baiyu
    ItemType_YaoShui_ShanBi = 227,          //闪避药水
    ItemType_YaoShui_ShenSheng = 228,       //神圣药水
    ItemType_YaoShui_QiangGong = 229,       //强攻药水
    ItemType_YaoShui_JingHua = 230,         //净化药水
    ItemType_TingKeZheng = 232,				//听课证
    ItemType_DynamicHeadCard = 231,			//动态头像卡
    ItemType_Sound = 233,					//语音卡
    ItemType_GhostPill = 234,				//灵魂丹1
    ItemType_SleightDrug = 235,				//熟练丹
    ItemType_AdvancedStone = 236,           //淬炼宝石

    ItemType_Yaodao = 237,		//南蛮妖刀
    ItemType_Shending = 238,	//霸王神鼎	
    ItemType_CountryDare = 239,	//国战道具
    ItemType_Dart = 240,        //圣诞元旦活动道具 飞镖

    ItemType_TaskExpAcc = 241,  //任务经验加速丹
    ItemType_QilinYu = 242,                 //寒冰麒麟装备类型
    ItemType_Farm = 250,                    //农场道具
    ItemType_Star = 252,					//空星

    ItemType_GodStone = 251,             //神器碎片
    ItemType_SpeakerBrush = 253,            //喇叭系统油漆桶
    ItemType_Wing = 254,                    //翅膀装备类型
    ItemType_GenGoldPointObject = 255,		//通用金子点数道具	
};
