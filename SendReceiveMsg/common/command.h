#pragma once

#ifndef _COMMAND_H
#define _COMMAND_H

#pragma pack(1)

//空指令
const BYTE NULL_USERCMD = 0;
//时间指令
const BYTE TIME_USERCMD = 2;
//数据指令
const BYTE DATA_USERCMD = 3;
//登陆指令
const BYTE LOGON_USERCMD = 104;

#define MAX_NAMESIZE 32

/**
 * \brief 账号最大长度
 */
#define MAX_ACCNAMESIZE  48

/**
 * \brief IP地址最大长度
 *
 */
#define MAX_IP_LENGTH  16

/**
 * \brief 网关最大容纳用户数目
 *
 */
#define MAX_GATEWAYUSER 4000  

/**
 * \brief 密码最大长度
 *
 */
#define MAX_PASSWORD  16


struct stNullUserCmd
{
	BYTE byCmd;
	BYTE byParam;
	DWORD dwTimestamp;
};

//////////////////////////////////////////////////////////////
// 登陆指令定义开始
//////////////////////////////////////////////////////////////
struct stLogonUserCmd : public stNullUserCmd
{
	stLogonUserCmd()
	{
		byCmd = LOGON_USERCMD;
	}
};

/// 客户端验证版本
const BYTE USER_VERIFY_VER_PARA = 120;
const DWORD GAME_VERSION = 1999;
struct stUserVerifyVerCmd  : public stLogonUserCmd
{
	stUserVerifyVerCmd()
	{
		byParam = USER_VERIFY_VER_PARA;
		version = GAME_VERSION;
	}
	DWORD reserve;//保留字段
	DWORD version;
};

/// 客户端登陆登陆服务器
const BYTE USER_REQUEST_LOGIN_PARA = 2;
struct stUserRequestLoginCmd : public stLogonUserCmd
{
	stUserRequestLoginCmd()
	{
		byParam = USER_REQUEST_LOGIN_PARA;
	}
	char pstrName[MAX_ACCNAMESIZE];    /**< 帐号 */
	char pstrPassword[33];  /**< 用户密码 */
	WORD game;              /**< 游戏类型编号，目前一律添0 */
	WORD zone;              /**< 游戏区编号 */
	char jpegPassport[7];        /**< 图形验证码 */
	char mac_addr[13];
	unsigned char uuid[25];
	WORD wdNetType; //0电信 1网通
	unsigned char passpodPwd[9];//密保密码
};

enum{
	LOGIN_RETURN_UNKNOWN,   /// 未知错误
	LOGIN_RETURN_VERSIONERROR, /// 版本错误
	LOGIN_RETURN_UUID,     /// UUID登陆方式没有实现
	LOGIN_RETURN_DB,     /// 数据库出错
	LOGIN_RETURN_PASSWORDERROR,/// 帐号密码错误
	LOGIN_RETURN_CHANGEPASSWORD,/// 修改密码成功
	LOGIN_RETURN_IDINUSE,   /// ID正在被使用中
	LOGIN_RETURN_IDINCLOSE,   /// ID被封
	LOGIN_RETURN_GATEWAYNOTAVAILABLE,/// 网关服务器未开
	LOGIN_RETURN_USERMAX,   /// 用户满
	LOGIN_RETURN_ACCOUNTEXIST, /// 账号已经存在
	LOGON_RETURN_ACCOUNTSUCCESS,/// 注册账号成功

	LOGIN_RETURN_CHARNAMEREPEAT,/// 角色名称重复
	LOGIN_RETURN_USERDATANOEXIST,/// 用户档案不存在
	LOGIN_RETURN_USERNAMEREPEAT,/// 用户名重复
	LOGIN_RETURN_TIMEOUT,   /// 连接超时
	LOGIN_RETURN_PAYFAILED,   /// 计费失败
	LOGIN_RETURN_JPEG_PASSPORT, /// 图形验证码输入错误
	LOGIN_RETURN_LOCK,         /// 帐号被锁定
	LOGIN_RETURN_WAITACTIVE, /// 帐号待激活
	LOGIN_RETURN_NEWUSER_OLDZONE      ///新账号不允许登入旧的游戏区 
};
/// 登陆失败后返回的信息
const BYTE SERVER_RETURN_LOGIN_FAILED = 3;
struct stServerReturnLoginFailedCmd : public stLogonUserCmd
{
	stServerReturnLoginFailedCmd()
	{
		byParam = SERVER_RETURN_LOGIN_FAILED;
	}
	BYTE byReturnCode;      /**< 返回的子参数 */
	WORD size;
	BYTE data[0];
} ;

/// 登陆成功，返回网关服务器地址端口以及密钥等信息
const BYTE SERVER_RETURN_LOGIN_OK = 4;
struct stServerReturnLoginSuccessCmd : public stLogonUserCmd 
{
	stServerReturnLoginSuccessCmd()
	{
		byParam = SERVER_RETURN_LOGIN_OK;
	}

	DWORD dwUserID;
	DWORD loginTempID;
	char pstrIP[MAX_IP_LENGTH];
	WORD wdPort;

	union{
		struct{
			BYTE randnum[58];
			BYTE keyOffset;  // 密匙在 key 中的偏移
		};
		BYTE key[256];  // 保存密匙，整个数组用随机数填充
	};

	DWORD state; //账号状态
};

/// 客户登陆网关服务器发送账号和密码
const BYTE PASSWD_LOGON_USERCMD_PARA = 5;
struct stPasswdLogonUserCmd : public stLogonUserCmd
{
	stPasswdLogonUserCmd()
	{
		byParam = PASSWD_LOGON_USERCMD_PARA;
	}

	DWORD loginTempID;
	DWORD dwUserID;
	char pstrName[MAX_ACCNAMESIZE];    /**< 帐号 */
	char pstrPassword[MAX_PASSWORD];
};

/// 请求创建账号
const BYTE ACCOUNT_LOGON_USERCMD_PARA = 7;
struct stAccountLogonUserCmd : public stLogonUserCmd 
{
	stAccountLogonUserCmd()
	{
		byParam = ACCOUNT_LOGON_USERCMD_PARA;
	}

	char strName[MAX_ACCNAMESIZE];
	char strPassword[MAX_PASSWORD];
};

/// 请求更改密码
const BYTE PASSWORD_LOGON_USERCMD_PARA = 9;
struct stPasswordLogonUserCmd : public stLogonUserCmd {
	stPasswordLogonUserCmd()
	{
		byParam = PASSWORD_LOGON_USERCMD_PARA;
	}

	char strName[MAX_ACCNAMESIZE];
	char strPassword[MAX_PASSWORD];
	char strNewPassword[MAX_PASSWORD];
};

/// 请求返回选择人物界面
const BYTE BACKSELECT_USERCMD_PARA = 10;
struct stBackSelectUserCmd : public stLogonUserCmd
{
	stBackSelectUserCmd()
	{
		byParam = BACKSELECT_USERCMD_PARA;
	}
};

/// 发送图形验证码到客户端
const BYTE JPEG_PASSPORT_USERCMD_PARA = 11;
struct stJpegPassportUserCmd : public stLogonUserCmd
{
	stJpegPassportUserCmd()
	{
		byParam = JPEG_PASSPORT_USERCMD_PARA;
		size = 0;
	}
	WORD size;
	BYTE data[0];
};
// [ranqd] Add 服务器状态
enum SERVER_STATE 
{
	STATE_SERVICING	=	0, // 维护
	STATE_NOMARL	=	1, // 正常
	STATE_GOOD		=	2, // 良好
	STATE_BUSY		=	3, // 繁忙
	STATE_FULL		=	4, // 爆满
};
// [ranqd] Add 服务器类型
enum SERVER_TYPE
{
	TYPE_GENERAL		=	0, // 普通
	TYPE_PEACE		=	1,     // 和平
};

//发送国家信息
struct  Country_Info
{
	DWORD id;//国家id
	BYTE  enableRegister; //允许注册为1 不允许为0
	BYTE  enableLogin;    //允许登陆为1 不允许为0
	BYTE  Online_Statue;  // [ranqd] add 在线情况 参考 enum SERVER_STATE 
	BYTE  type;           // [ranqd] add 服务器类型 参考 enum SERVER_TYPE
	char pstrName[MAX_NAMESIZE];//国家名称
	Country_Info()
	{
		enableRegister = 0;
		enableLogin = 0;
	}
};
const BYTE SERVER_RETURN_COUNTRY_INFO = 12;
struct stCountryInfoUserCmd : public stLogonUserCmd
{
	stCountryInfoUserCmd()
	{
		byParam = SERVER_RETURN_COUNTRY_INFO;
		size = 0;
	}
	WORD size;
	Country_Info countryinfo[0];
};
// [ranqd] add 用户选择服务器命令
const BYTE CLIENT_SELETCT_COUNTRY = 13;
struct stSelectCountryUserCmd : public stLogonUserCmd
{
	stSelectCountryUserCmd()
	{
		byParam = CLIENT_SELETCT_COUNTRY;
		id = 0;
	}
	DWORD id;  // 选择的国家id 
};

//客户端请求得到自己的IP
const BYTE REQUEST_CLIENT_IP_PARA = 15;
struct stRequestClientIP : public stLogonUserCmd
{
	stRequestClientIP()
	{
		byParam = REQUEST_CLIENT_IP_PARA;
	}
};

//返回客户端的IP
const BYTE RETURN_CLIENT_IP_PARA = 16;
struct stReturnClientIP : public stLogonUserCmd
{
	stReturnClientIP()
	{
		byParam = RETURN_CLIENT_IP_PARA;
		ZeroMemory(pstrIP, MAX_IP_LENGTH);
	}
	unsigned char pstrIP[MAX_IP_LENGTH];
};

//////////////////////////////////////////////////////////////
// 登陆指令定义结束
//////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
/// 时间指令定义开始
//////////////////////////////////////////////////////////////
struct stTimerUserCmd : public stNullUserCmd
{
	stTimerUserCmd()
	{
		byCmd = TIME_USERCMD;
	}
};

/// 网关向用户发送游戏时间
const BYTE GAMETIME_TIMER_USERCMD_PARA = 1;
struct stGameTimeTimerUserCmd : public stTimerUserCmd 
{
	stGameTimeTimerUserCmd()
	{
		byParam = GAMETIME_TIMER_USERCMD_PARA;
	}

	QWORD qwGameTime;      /**< 游戏时间 */
};

/// 网关向用户请求时间
const BYTE REQUESTUSERGAMETIME_TIMER_USERCMD_PARA = 2;
struct stRequestUserGameTimeTimerUserCmd : public stTimerUserCmd
{
	stRequestUserGameTimeTimerUserCmd()
	{
		byParam = REQUESTUSERGAMETIME_TIMER_USERCMD_PARA;
	}

};

/// 用户向网关发送当前游戏时间
const BYTE USERGAMETIME_TIMER_USERCMD_PARA  = 3;
struct stUserGameTimeTimerUserCmd : public stTimerUserCmd
{
	stUserGameTimeTimerUserCmd()
	{
		byParam = USERGAMETIME_TIMER_USERCMD_PARA;
	}

	DWORD dwUserTempID;      /**< 用户临时ID */
	QWORD qwGameTime;      /**< 用户游戏时间 */
};

/// 用户ping命令(服务器原样返回)
const BYTE PING_TIMER_USERCMD_PARA = 4;
struct stPingTimeTimerUserCmd : public stTimerUserCmd
{
	stPingTimeTimerUserCmd()
	{
		byParam = PING_TIMER_USERCMD_PARA;
	}

};
//////////////////////////////////////////////////////////////
/// 时间指令定义结束
//////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
// 数据指令定义开始
//////////////////////////////////////////////////////////////
struct stDataUserCmd : public stNullUserCmd
{
	stDataUserCmd()
	{
		byCmd = DATA_USERCMD;
	}
};

const BYTE MERGE_VERSION_CHECK_USERCMD_PARA = 53;
struct stMergeVersionCheckUserCmd : public stDataUserCmd
{
	stMergeVersionCheckUserCmd()
	{
		byParam = MERGE_VERSION_CHECK_USERCMD_PARA;
		dwMergeVersion = 0;
	}
	DWORD dwMergeVersion;
};

//////////////////////////////////////////////////////////////
// 数据指令定义结束
//////////////////////////////////////////////////////////////

#pragma pack()

#endif //_COMMAND_H