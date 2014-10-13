#ifndef __client1__SocketManager__
#define __client1__SocketManager__


//#include "cocos2d.h"

#include "message.h"
#include "SocketClient.h"
#include <list>
#include "zEncDec.h"
#include "command.h"


USING_NS_CC;



#define Client_GetKey(aKey, pData) { 	size_t size = 0;	for (std::list<BYTE>::iterator it = aKey.begin(); it != aKey.end() && size < sizeof(pData); ++it) 	pData[size++] = (*it);}
#define Client_SendCommand(pCmd, bufferSize) {(pCmd)->dwTimestamp = SocketManager::getInstance()->GetTimeStamp();	SocketManager::getInstance()->send((char*)(pCmd), bufferSize);}
#define SEND_USER_CMD(cmd) Client_SendCommand(&(cmd), sizeof(cmd))

class SocketManager
{
public:
    static SocketManager* getInstance();

	SocketClient* GetSocketClient();

    void startSocket();
    void sendMessage(const char* data,int commandId);

    void send(char* buffer, size_t bufferSize);
	//////////////////////////////////////////////////////////////////////////
	//
	bool onRecvData(BYTE* pData, size_t size);

	//////////////////////////////////////////////////////////////////////////
	//功能相关
	DWORD GetTimeStamp();
	void InitTimeStamp();


	//////////////////////////////////////////////////////////////////////////
	//账号相关
	
	//用IP进行加密
	unsigned char szKeyIP[16];
	void SetKeyIP(const unsigned char* pszip);
	void UseIPEncry(unsigned char* pszSrc, int iNum);

	stServerReturnLoginSuccessCmd* GetUserInfoCmd();
private:
	QWORD m_qwLastConfirmTimer;
	DWORD m_dwTimer;
	DWORD m_nTimeInit;

private:
    SocketManager();
    ~SocketManager();
    
    SocketClient* _socket;
	SocketClient* _socketGateway;
    
	CEncrypt* m_pEncrypt;
	
	bool m_bShowCmdSrcret;


	std::vector<BYTE> m_uncompressBuffer;
	std::vector<BYTE> m_dataBuffer;
	std::vector<BYTE> m_recvBuffer;
	std::list<BYTE> m_loginkey;
	

	stServerReturnLoginSuccessCmd m_BackCmd;
public:

	bool m_bLoginSend;
	bool m_bLoginRecv;


	std::list<BYTE> m_key;
	DWORD m_dwEncryptMask;
	int m_iLenSended;
	int m_iLenRecved;
	bool IsNeedEncrypt(bool isSend);
	void IncrementSendData();
	void IncrementRecvData();
};


#endif /* defined(__client1__SocketManager__) */
