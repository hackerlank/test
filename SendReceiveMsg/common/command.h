#pragma once

#ifndef _COMMAND_H
#define _COMMAND_H

#pragma pack(1)

//��ָ��
const BYTE NULL_USERCMD = 0;
//ʱ��ָ��
const BYTE TIME_USERCMD = 2;
//����ָ��
const BYTE DATA_USERCMD = 3;
//��½ָ��
const BYTE LOGON_USERCMD = 104;

#define MAX_NAMESIZE 32

/**
 * \brief �˺���󳤶�
 */
#define MAX_ACCNAMESIZE  48

/**
 * \brief IP��ַ��󳤶�
 *
 */
#define MAX_IP_LENGTH  16

/**
 * \brief ������������û���Ŀ
 *
 */
#define MAX_GATEWAYUSER 4000  

/**
 * \brief ������󳤶�
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
// ��½ָ��忪ʼ
//////////////////////////////////////////////////////////////
struct stLogonUserCmd : public stNullUserCmd
{
	stLogonUserCmd()
	{
		byCmd = LOGON_USERCMD;
	}
};

/// �ͻ�����֤�汾
const BYTE USER_VERIFY_VER_PARA = 120;
const DWORD GAME_VERSION = 1999;
struct stUserVerifyVerCmd  : public stLogonUserCmd
{
	stUserVerifyVerCmd()
	{
		byParam = USER_VERIFY_VER_PARA;
		version = GAME_VERSION;
	}
	DWORD reserve;//�����ֶ�
	DWORD version;
};

/// �ͻ��˵�½��½������
const BYTE USER_REQUEST_LOGIN_PARA = 2;
struct stUserRequestLoginCmd : public stLogonUserCmd
{
	stUserRequestLoginCmd()
	{
		byParam = USER_REQUEST_LOGIN_PARA;
	}
	char pstrName[MAX_ACCNAMESIZE];    /**< �ʺ� */
	char pstrPassword[33];  /**< �û����� */
	WORD game;              /**< ��Ϸ���ͱ�ţ�Ŀǰһ����0 */
	WORD zone;              /**< ��Ϸ����� */
	char jpegPassport[7];        /**< ͼ����֤�� */
	char mac_addr[13];
	unsigned char uuid[25];
	WORD wdNetType; //0���� 1��ͨ
	unsigned char passpodPwd[9];//�ܱ�����
};

enum{
	LOGIN_RETURN_UNKNOWN,   /// δ֪����
	LOGIN_RETURN_VERSIONERROR, /// �汾����
	LOGIN_RETURN_UUID,     /// UUID��½��ʽû��ʵ��
	LOGIN_RETURN_DB,     /// ���ݿ����
	LOGIN_RETURN_PASSWORDERROR,/// �ʺ��������
	LOGIN_RETURN_CHANGEPASSWORD,/// �޸�����ɹ�
	LOGIN_RETURN_IDINUSE,   /// ID���ڱ�ʹ����
	LOGIN_RETURN_IDINCLOSE,   /// ID����
	LOGIN_RETURN_GATEWAYNOTAVAILABLE,/// ���ط�����δ��
	LOGIN_RETURN_USERMAX,   /// �û���
	LOGIN_RETURN_ACCOUNTEXIST, /// �˺��Ѿ�����
	LOGON_RETURN_ACCOUNTSUCCESS,/// ע���˺ųɹ�

	LOGIN_RETURN_CHARNAMEREPEAT,/// ��ɫ�����ظ�
	LOGIN_RETURN_USERDATANOEXIST,/// �û�����������
	LOGIN_RETURN_USERNAMEREPEAT,/// �û����ظ�
	LOGIN_RETURN_TIMEOUT,   /// ���ӳ�ʱ
	LOGIN_RETURN_PAYFAILED,   /// �Ʒ�ʧ��
	LOGIN_RETURN_JPEG_PASSPORT, /// ͼ����֤���������
	LOGIN_RETURN_LOCK,         /// �ʺű�����
	LOGIN_RETURN_WAITACTIVE, /// �ʺŴ�����
	LOGIN_RETURN_NEWUSER_OLDZONE      ///���˺Ų��������ɵ���Ϸ�� 
};
/// ��½ʧ�ܺ󷵻ص���Ϣ
const BYTE SERVER_RETURN_LOGIN_FAILED = 3;
struct stServerReturnLoginFailedCmd : public stLogonUserCmd
{
	stServerReturnLoginFailedCmd()
	{
		byParam = SERVER_RETURN_LOGIN_FAILED;
	}
	BYTE byReturnCode;      /**< ���ص��Ӳ��� */
	WORD size;
	BYTE data[0];
} ;

/// ��½�ɹ����������ط�������ַ�˿��Լ���Կ����Ϣ
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
			BYTE keyOffset;  // �ܳ��� key �е�ƫ��
		};
		BYTE key[256];  // �����ܳף�������������������
	};

	DWORD state; //�˺�״̬
};

/// �ͻ���½���ط����������˺ź�����
const BYTE PASSWD_LOGON_USERCMD_PARA = 5;
struct stPasswdLogonUserCmd : public stLogonUserCmd
{
	stPasswdLogonUserCmd()
	{
		byParam = PASSWD_LOGON_USERCMD_PARA;
	}

	DWORD loginTempID;
	DWORD dwUserID;
	char pstrName[MAX_ACCNAMESIZE];    /**< �ʺ� */
	char pstrPassword[MAX_PASSWORD];
};

/// ���󴴽��˺�
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

/// �����������
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

/// ���󷵻�ѡ���������
const BYTE BACKSELECT_USERCMD_PARA = 10;
struct stBackSelectUserCmd : public stLogonUserCmd
{
	stBackSelectUserCmd()
	{
		byParam = BACKSELECT_USERCMD_PARA;
	}
};

/// ����ͼ����֤�뵽�ͻ���
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
// [ranqd] Add ������״̬
enum SERVER_STATE 
{
	STATE_SERVICING	=	0, // ά��
	STATE_NOMARL	=	1, // ����
	STATE_GOOD		=	2, // ����
	STATE_BUSY		=	3, // ��æ
	STATE_FULL		=	4, // ����
};
// [ranqd] Add ����������
enum SERVER_TYPE
{
	TYPE_GENERAL		=	0, // ��ͨ
	TYPE_PEACE		=	1,     // ��ƽ
};

//���͹�����Ϣ
struct  Country_Info
{
	DWORD id;//����id
	BYTE  enableRegister; //����ע��Ϊ1 ������Ϊ0
	BYTE  enableLogin;    //�����½Ϊ1 ������Ϊ0
	BYTE  Online_Statue;  // [ranqd] add ������� �ο� enum SERVER_STATE 
	BYTE  type;           // [ranqd] add ���������� �ο� enum SERVER_TYPE
	char pstrName[MAX_NAMESIZE];//��������
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
// [ranqd] add �û�ѡ�����������
const BYTE CLIENT_SELETCT_COUNTRY = 13;
struct stSelectCountryUserCmd : public stLogonUserCmd
{
	stSelectCountryUserCmd()
	{
		byParam = CLIENT_SELETCT_COUNTRY;
		id = 0;
	}
	DWORD id;  // ѡ��Ĺ���id 
};

//�ͻ�������õ��Լ���IP
const BYTE REQUEST_CLIENT_IP_PARA = 15;
struct stRequestClientIP : public stLogonUserCmd
{
	stRequestClientIP()
	{
		byParam = REQUEST_CLIENT_IP_PARA;
	}
};

//���ؿͻ��˵�IP
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
// ��½ָ������
//////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
/// ʱ��ָ��忪ʼ
//////////////////////////////////////////////////////////////
struct stTimerUserCmd : public stNullUserCmd
{
	stTimerUserCmd()
	{
		byCmd = TIME_USERCMD;
	}
};

/// �������û�������Ϸʱ��
const BYTE GAMETIME_TIMER_USERCMD_PARA = 1;
struct stGameTimeTimerUserCmd : public stTimerUserCmd 
{
	stGameTimeTimerUserCmd()
	{
		byParam = GAMETIME_TIMER_USERCMD_PARA;
	}

	QWORD qwGameTime;      /**< ��Ϸʱ�� */
};

/// �������û�����ʱ��
const BYTE REQUESTUSERGAMETIME_TIMER_USERCMD_PARA = 2;
struct stRequestUserGameTimeTimerUserCmd : public stTimerUserCmd
{
	stRequestUserGameTimeTimerUserCmd()
	{
		byParam = REQUESTUSERGAMETIME_TIMER_USERCMD_PARA;
	}

};

/// �û������ط��͵�ǰ��Ϸʱ��
const BYTE USERGAMETIME_TIMER_USERCMD_PARA  = 3;
struct stUserGameTimeTimerUserCmd : public stTimerUserCmd
{
	stUserGameTimeTimerUserCmd()
	{
		byParam = USERGAMETIME_TIMER_USERCMD_PARA;
	}

	DWORD dwUserTempID;      /**< �û���ʱID */
	QWORD qwGameTime;      /**< �û���Ϸʱ�� */
};

/// �û�ping����(������ԭ������)
const BYTE PING_TIMER_USERCMD_PARA = 4;
struct stPingTimeTimerUserCmd : public stTimerUserCmd
{
	stPingTimeTimerUserCmd()
	{
		byParam = PING_TIMER_USERCMD_PARA;
	}

};
//////////////////////////////////////////////////////////////
/// ʱ��ָ������
//////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////
// ����ָ��忪ʼ
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
// ����ָ������
//////////////////////////////////////////////////////////////

#pragma pack()

#endif //_COMMAND_H