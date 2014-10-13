#include "SocketManager.h"
#include "../../common/console.h"
#include "../enc/miniCrypt.h"
#include "zlib/zlib.h"

//在这里修改登录器IP 端口
#define LOGIN_SERVER_IP "101.226.182.5"
#define LOGIN_SERVER_PORT 7000

BYTE key[16] = {0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb};

#define PACKET_MAX_SIZE 0x00ffffff
#define PACKET_FLAG_ENC 0x80000000
#define PACKET_FLAG_ZIP 0x40000000

typedef unsigned long tPackLengthType;

#define HAS_ENCRYPT
#define _USE_PART_ENCRYPT

static SocketManager* instance = NULL;
SocketManager::SocketManager()
{
    m_bShowCmdSrcret = true;
	m_bLoginSend = true;
	m_bLoginRecv = true;
	m_pEncrypt = new CEncrypt;
	for (int i=0; i<sizeof(key); ++i)
	{
		m_loginkey.push_back(key[i]);
		m_key.push_back(key[i]);
	}
	m_uncompressBuffer.reserve(1024*4);
	m_dataBuffer.reserve(1024*4);
	m_uncompressBuffer.reserve(1024 * 2);

	m_dwEncryptMask = 0xffff0000;
	m_iLenSended = 0;
	m_iLenRecved = 0; 
	m_nTimeInit = 0;
}

SocketManager::~SocketManager()
{
	if (m_pEncrypt)
	{
		delete m_pEncrypt;
		m_pEncrypt = NULL;
	}
}

SocketManager* SocketManager::getInstance()
{
    if(instance == NULL)
    {
        instance = new SocketManager();
        
    }
    
    return instance;
}

SocketClient* SocketManager::GetSocketClient()
{
	return _socket;
}

void SocketManager::startSocket()
{
	_socket = new SocketClient(LOGIN_SERVER_IP,LOGIN_SERVER_PORT,1,1,NULL);

	_socket->start();
}


void SocketManager::sendMessage(const char* data,int commandId)
{
    
//    if (_socket->m_iState != SocketClient_OK)
//    {
//        _socket->stop(true);
//        startSocket();
//    }
    
    Message *msg=_socket->constructMessage(data, commandId);
    _socket->sendMessage_(msg, false);
}

 void SocketManager::send(char* buffer, size_t bufferSize)
 {
	 //打印日志
	 if (m_bShowCmdSrcret)
	 {
		 char msg[128];
		 stNullUserCmd* cmd = (stNullUserCmd*)buffer;
		 ConsoleOut("***send*** cmd=%d, Parm =%d****\n", cmd->byCmd, cmd->byParam);
	 }

	 int retval = 0;
	 size_t datasize;

	 tPackLengthType flag = 0;
	 std::vector<BYTE> m_sendBuffer;
	 size_t size1 = ((bufferSize + sizeof(tPackLengthType) + 7) & (~7));
	 m_sendBuffer.resize(size1);

	 //超过32字节压缩
	 if (bufferSize > 32)
	 {
		 flag |= PACKET_FLAG_ZIP;
		 for (;;)
		 {
			 BYTE* compressData = &m_sendBuffer[0] + sizeof(tPackLengthType);
			 datasize = m_sendBuffer.size() - sizeof(tPackLengthType);

			 int zipResult = compress(compressData, (uLongf*)&datasize, (Bytef*)buffer, bufferSize);
			 if (zipResult == Z_BUF_ERROR)
			 {
				 m_sendBuffer.resize(m_sendBuffer.size() + bufferSize);
				 continue;
			 }
			 if (zipResult == Z_OK)
				 break;
		 }
		 
	 }
	 else
	 {
		 datasize = bufferSize;
		 BYTE* data = &m_sendBuffer[0] + sizeof(tPackLengthType);
		 memcpy(data, buffer, bufferSize);
	 }

	size_t sendsize;
	BYTE* senddata;

	//加密
	senddata = &m_sendBuffer[0];
	
#ifdef HAS_ENCRYPT
	//加密调整成8的整数倍
	sendsize = ((datasize + sizeof(tPackLengthType) + 7) & (~7));
	*(tPackLengthType*)senddata = (sendsize - sizeof(tPackLengthType)) | flag;
	*(tPackLengthType*)senddata |= PACKET_FLAG_ENC;
	size_t count = sendsize / 8;
	
	if (m_bLoginSend)
	{
		RC5_32_KEY key;
		BYTE keyData[16];
		Client_GetKey(m_loginkey, keyData);
		m_pEncrypt->RC5_32_set_key(&key, 16, keyData, 12);
		for (size_t i=0; i<count; ++i)
		{
			m_pEncrypt->RC5_32_encrypt((RC5_32_INT *)&senddata[i*8], &key);
		}
	}
	else
	{
		ZES_key_schedule key;
		BYTE keyData[8];
		Client_GetKey(m_key, keyData);
		m_pEncrypt->ZES_set_key((const_ZES_cblock*)keyData, &key);
		for (size_t i=0; i<count; ++i)
		{
			bool bEncrypt = true;
#ifdef _USE_PART_ENCRYPT
			bEncrypt = IsNeedEncrypt(true);
			IncrementSendData();
#endif
			if (bEncrypt)
			{
				m_pEncrypt->ZES_encrypt1((ZES_LONG *)&senddata[i*8], &key, ZES_ENCRYPT);
			}
			
		}
		
	}

#endif

	Message *msg=_socket->constructZtMessage((char*)senddata, sendsize);
	_socket->sendMessage_(msg, false);

 }


 bool SocketManager::onRecvData(BYTE* pData, size_t size)
 {
	 if (GetSocketClient()->m_IsGatewayIP)
	 {
		  cout << "break here" << endl;
	 }
	 if (!m_bLoginRecv)
	 {
		 cout << "break here" << endl;
	 }
	 //-----------------------
	 //读取数据
	 //-----------------------
	 {
		 size_t size1 = m_recvBuffer.size();
		 m_recvBuffer.resize(size1 + size + 1);
		 memcpy(&m_recvBuffer[size1], pData, size);
	 }



	 //-----------------------
	 //解密
	 //-----------------------
	 {

	#ifdef HAS_ENCRYPT
		 if (m_recvBuffer.size()-1 < 8)
			 return true;

		 size_t size2 = (m_recvBuffer.size()-1) & (~7);
		 size_t count = (m_recvBuffer.size()-1)/8;
		 if (m_bLoginRecv)
		 {
			 RC5_32_KEY key;
			 BYTE keyData[16];
			 Client_GetKey(m_loginkey, keyData);
			 m_pEncrypt->RC5_32_set_key(&key, 16, keyData, 12);
			 for (size_t i=0; i<count; ++i)
			 {
				 m_pEncrypt->RC5_32_decrypt((RC5_32_INT *)&m_recvBuffer[i * 8], &key);
			 }
		 
		 }
		 else
		 {
			 ZES_key_schedule key;
			 BYTE keyData[8];
			 Client_GetKey(m_key, keyData);
			 m_pEncrypt->ZES_set_key((const_ZES_cblock*)keyData, &key);

			 for (size_t i=0; i<count; ++i)
			 {
				 bool bEncrypt = true;
#ifdef _USE_PART_ENCRYPT
				 bEncrypt = IsNeedEncrypt(false);
				 IncrementRecvData();
#endif
				 //***** 这里还判断是不是部分加密。。。_USE_PART_ENCRYPT
				//。。。
				//省略需不需要加密

				if (bEncrypt)
					m_pEncrypt->ZES_encrypt1((ZES_LONG *)&m_recvBuffer[i * 8], &key, ZES_DECRYPT);
			 }
		 
		 }
	#endif

		 size_t size1 = m_dataBuffer.size();
		 m_dataBuffer.resize(size1 + size2);
		 memcpy(&m_dataBuffer[size1], &m_recvBuffer[0], size2);

		 size_t size3 = m_recvBuffer.size() -1 -size2;
		 memcpy(&m_recvBuffer[0], &m_recvBuffer[size2], size3);
		 m_recvBuffer.resize(size3);

	 }

	 //////////////////////////////////////////////////////////////////////////
     //解析
	 {
		 size_t offset = 0;
		 size_t datasize = m_dataBuffer.size();
		 BYTE*data = &m_dataBuffer[0];
		 while (offset + sizeof(tPackLengthType) < datasize)
		 {
			 tPackLengthType length = *(tPackLengthType*)&data[offset];
			 tPackLengthType flags = (length & (~PACKET_MAX_SIZE));
			 length &= PACKET_MAX_SIZE;
			 if (offset + sizeof(tPackLengthType) + length <= datasize)
			 {
				 //有一个完整的数据包
				 BYTE* uncompressData;
				 size_t uncompressSize;
				 if (flags & PACKET_FLAG_ZIP)
				 {
					 //数据包是压缩的
					 BYTE* compressData = &data[offset + sizeof(tPackLengthType)];
					 size_t compressSize = length;

					 if(m_uncompressBuffer.size() < compressSize * 3 / 2)
						 m_uncompressBuffer.resize(compressSize *3 /2 );

					 for (;;)
					 {
						 uncompressData = &m_uncompressBuffer[0];
						 uncompressSize = m_uncompressBuffer.size();
						 int iZipResult = uncompress(uncompressData, (uLongf*)&uncompressSize, compressData, compressSize);
						 if (iZipResult == Z_BUF_ERROR)
						 {
							 //缓冲区太小
							 m_uncompressBuffer.resize(m_uncompressBuffer.size() + compressSize * 3 / 2);
							 continue;
						 }
						 if (iZipResult == Z_OK)
						 {
							 break;
						 }

						 //解析错了，断开连接
						 return false;

					 }

				 }
				 else
				 {
					 uncompressData = &data[offset + sizeof(tPackLengthType)];
					 uncompressSize = length;
				 }

				 const stNullUserCmd* pCmd = (const stNullUserCmd*)uncompressData;
				 if (m_bShowCmdSrcret)
				 {
					 char msg[128];
					 ConsoleOut("\n***RecvData*** cmd=%d, Parm =%d****\n", pCmd->byCmd, pCmd->byParam);
				 }

				 //////////////////////////////////////////////////////////////////////////
				 // ...
				 //直接在这里处理消息了
				 if (pCmd->byCmd == LOGON_USERCMD) //登入相关
				 {
					 switch (pCmd->byParam)
					 {
					 case USER_VERIFY_VER_PARA:
						 {
							 const stUserVerifyVerCmd* pTmpCmd = (const stUserVerifyVerCmd*)pCmd;


							 //cout << "========breakpoint==========" << endl;

						 }
					 	break;
					 case USER_REQUEST_LOGIN_PARA:
						 {
							 const stUserRequestLoginCmd* pTmpCmd = (const stUserRequestLoginCmd*)pCmd;


							 //cout << "========breakpoint==========" << endl;
						 }
						 break;
					 case RETURN_CLIENT_IP_PARA:
						 {
							 ConsoleOut("========收到stReturnClientIP==========");
							 const stReturnClientIP* cmd1 = (const stReturnClientIP*)pCmd;
							 SetKeyIP(cmd1->pstrIP);

							 ConsoleOutEx(console::CONSOLE_COLOR_RED, "========登陆登陆服务器===========");
							 stUserRequestLoginCmd cmd;
							 //账号
							 strncpy(cmd.pstrName, "goodluck03@gmail.com", sizeof(cmd.pstrName));
							 //密保密码
							 memcpy(&cmd.passpodPwd[0], "", sizeof(cmd.passpodPwd));
							 //加密后的密码
							 unsigned char szPass[34];
							 memset(szPass, 0, sizeof(szPass));
							 unsigned char szTmpPass[] = {16, 26, 25, 26, 105, 1, 130, 1, 178, 77, 126, 26, 169, 77, 158, 26, 137};
							 for (int i=0; i<sizeof(szTmpPass); ++i)
								 szPass[i] = szTmpPass[i];

							 BYTE length = szPass[0];
							 UseIPEncry(szPass, length+1);
							 memcpy(cmd.pstrPassword, szPass, sizeof(cmd.pstrPassword));

							 //游戏类型 区 线路
							 cmd.game = 10;
							 cmd.zone = 30;
							 cmd.wdNetType = 0;

							 //mac addr
							 strncpy(cmd.mac_addr, "10BF48E2BE21", sizeof(cmd.mac_addr));
							 cmd.mac_addr[sizeof(cmd.mac_addr)-1] = 0;

							 //59
							 unsigned char szUUID[27] = {0};
							 unsigned char szTmpUUID[] = {25, 58, 61, 59, 93, 59, 29, 58, 253, 59, 77, 59, 237, 59, 221, 58, 45, 58, 253, 110, 8, 110, 8, 110, 24, 111, 216};
							 for (int i=0; i<sizeof(szTmpUUID); ++i)
							 {
								 szUUID[i] = szTmpUUID[i];
							 }
							 szUUID[0] = 25;
							 UseIPEncry(szUUID, 25);
							 memcpy(cmd.uuid, szUUID, sizeof(cmd.uuid));

							 SEND_USER_CMD(cmd);

						 }
						 break;
					 case SERVER_RETURN_LOGIN_FAILED:
						 {
							 ConsoleOutEx(console::CONSOLE_COLOR_RED, "========登陆服务器返回失败消息===========");
						 }
						 break;
					 case SERVER_RETURN_LOGIN_OK:
						 {
							 ConsoleOutEx(console::CONSOLE_COLOR_GREEN, "========登陆登陆服务器成功！===========");
							 const stServerReturnLoginSuccessCmd* pSign = (const stServerReturnLoginSuccessCmd*)pCmd;

							 ConsoleOutEx(console::CONSOLE_COLOR_GREEN, "返回网关IP: %s 端口: %d", pSign->pstrIP, pSign->wdPort);

							 memcpy(&m_BackCmd, pCmd, sizeof(stServerReturnLoginSuccessCmd));

							 //重新连接，本地清理
// 							 m_bLoginSend = false;
// 							 m_bLoginRecv = false;
							 m_uncompressBuffer.clear();
							 m_dataBuffer.clear();
							 m_recvBuffer.clear();
							 GetSocketClient()->Destroy();
							 GetSocketClient()->ReConnect(pSign->pstrIP, pSign->wdPort);
							 //GetSocketClient()->ReConnect("127.0.0.1", 4567);

							 //pthread_exit(0);

						 }
						 break;
					 }
					 
				 }
				 else if (pCmd->byCmd == TIME_USERCMD) //时间相关
				 {
					 switch (pCmd->byParam)
					 {
 					 case GAMETIME_TIMER_USERCMD_PARA:
 						 {
 							 const stGameTimeTimerUserCmd* pTmpCmd = (const stGameTimeTimerUserCmd*)pCmd;
							ConsoleOutEx(console::CONSOLE_COLOR_GREEN, "========网关返回游戏时间！===========");
 						 }
					 default:
						 break;
					 }
				 }

				 else if (pCmd->byCmd == DATA_USERCMD) //数据指令
				 {
					 switch (pCmd->byParam)
					 {
					 case MERGE_VERSION_CHECK_USERCMD_PARA:
						 {
							 const stMergeVersionCheckUserCmd* pTmpCmd = (const stMergeVersionCheckUserCmd*)pCmd;
							 ConsoleOutEx(console::CONSOLE_COLOR_GREEN, "========网关返回合并版本号！===========");
						 }
					 default:
						 break;
					 }
				 }

#ifdef HAS_ENCRYPT
				 offset += (sizeof(tPackLengthType) + length);
#else
				 offset += (sizeof(tPackLengthType) + length);
#endif
			 }
			 else
			 {
				 break;
			 }
		 }

		 if (datasize - offset > 1024 * 64)
		 {
			 return false;
		 }

		 memcpy(&data[0], &data[offset], datasize - offset);
		 m_dataBuffer.resize(datasize-offset);
		 
	 }

	
	 return true;

 }

 DWORD SocketManager::GetTimeStamp()
 {
	 return timeGetTime() - m_nTimeInit;
 }
 void SocketManager::InitTimeStamp()
 {
	 m_nTimeInit = timeGetTime();
 }

 void SocketManager::SetKeyIP(const unsigned char* pszip)
 {
	 memcpy( szKeyIP, pszip, sizeof(szKeyIP));
 }

 void SocketManager::UseIPEncry(unsigned char* pszSrc, int iNum)
 {
	 BYTE nKey = sizeof(szKeyIP), rkey = 0;

	 for (int i = 0; i < iNum; i++)
	 {
		 pszSrc[i] ^= szKeyIP[rkey];
		 pszSrc[i]++;

		 if (++rkey >= nKey)
			 rkey = 0;
	 }
	 
 }

 stServerReturnLoginSuccessCmd* SocketManager::GetUserInfoCmd()
 {
	 return &m_BackCmd;
 }

 bool SocketManager::IsNeedEncrypt(bool isSend)
 {
	 int iData = 0;
	 if (isSend) //发送
		 iData = m_iLenSended;
	 else
		 iData = m_iLenRecved;

	 assert(iData >= 0 && iData <= 31);
	 unsigned int i = 0x80000000;
	 i >>= iData;

	 if (m_dwEncryptMask & i)
		 return true;
	 else
		 return false;
 }

 void SocketManager::IncrementSendData()
 {
	 m_iLenSended++;
	 if (m_iLenSended >= 32)
		 m_iLenSended = 0;
 }
 void SocketManager::IncrementRecvData()
 {
	 m_iLenRecved++;
	 if (m_iLenRecved >= 32)
		 m_iLenRecved = 0;
 }

