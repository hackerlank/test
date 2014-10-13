#ifndef _CDATA_NETSOCKET_H_
#define _CDATA_NETSOCKET_H_

#define CC_PLATFORM_WIN32 3

#ifdef CC_PLATFORM_WIN32

#include "pthread.h"
#include <Winsock2.h>
#include <Wininet.h>
#include <ws2tcpip.h>
#include <stdio.h>
#include <string.h>

#else

#include <sys/types.h>
#include <sys/socket.h>
#include <sys/wait.h>
#include <netdb.h>
#include <arpa/inet.h>
#include <sys/time.h>
#include <pthread.h>
#include <unistd.h>

#endif

#include <string.h>
#include <stdlib.h>
#include <queue>

//#include "cocos2d.h"
#include "ByteBuffer.h"
const int	SocketClient_WAIT_CONNECT = 0;
const int	SocketClient_OK = 1;
const int	SocketClient_DESTROY = 2;


class Message;
class MessageHandler;

using namespace std;

class SocketClient{
public:
	int m_hSocket;

	char m_serverId;
	char m_clientId;
	string m_host;
	int m_iport;
//    static cocos2d::CCDictionary * m_dictionary;    //��Ž��յ�����
	vector<char> m_serverId2;
	vector<char> m_clientId2;
	vector<string> m_host2;
	vector<int> m_iport2;//���ӷ�����1ʧ�ܺ����������2���ڽ���������2
	
	//���ͺͽ��ջ����������ͻ���������ʱ�򣬻�Ͽ����ӣ�����ʾ�źŲ���
	ByteBuffer m_cbRecvBuf;
	ByteBuffer m_cbSendBuf;
	
	//�յ��������Ϣ
	queue<Message*> m_receivedMessageQueue;
	
	//��Ҫ���͵�����˵���Ϣ
	queue<Message*> m_sendMessageQueue;
	
	int m_iState;
	MessageHandler* m_pHandler;
	

	//�����߳�
	bool m_bThreadRecvCreatedGateway;
	pthread_t pthread_t_receive_gateway;

	//�����߳�
	bool m_bThreadRecvCreated;
	pthread_t pthread_t_receive;
	
	//�����߳�
	bool m_bThreadSendCreated;
	pthread_t pthread_t_send;
	pthread_mutex_t m_thread_cond_mutex;//pthread_mutex_t ������
	pthread_cond_t m_threadCond;
	
    //�����߳�
    bool m_bThreadChatCreated;
    pthread_t pthread_t_chat;
    pthread_mutex_t m_thread_chat_mutex;
    //pthread_cond_t m_threadCond;
	//���Ͷ���ͬ����
	pthread_mutex_t m_sendqueue_mutex;
	
	bool m_isvalidSeq;
	long long m_sabcde[6];
	long long getSeq();
private:
	//���ӷ�����
	bool  connectServer();
	
	static void* ThreadReceiveMessageGateway(void *p);
	static void* ThreadReceiveMessage(void *p);
	static void* ThreadSendMessage(void *p);


	
public:
	SocketClient(String host, int port, byte clientId,
				 byte serverId,MessageHandler* netImpl);
	
	~SocketClient();
	void start();
	//void startGateway();
	void stop(boolean b);

	void Destroy();
	void ReConnect(String host, int port);
	bool m_IsGatewayIP;
	
	bool isWaitConnect();
	//��������
	void sendMessage_(Message* msg,bool b);	
	
	Message* popReceivedMessage();
	Message* pickReceivedMessage();
	
	void pushReceivedMessage(Message* msg);
	
	MessageHandler* getHandler(){
        return  m_pHandler;
    }       
	
	void setHandler(MessageHandler* handler){
		this->m_pHandler = handler;
	}
	
    Message* constructMessage(const char* data,int commandId);
	Message* constructZtMessage(const char* data, int sendsize);
    static int bytesToInt(byte* data);
    static byte* intToByte(int i);
    void swhlie(int commandId);
    
};



class SocketServer{
public:
	bool isStartedServer;
    bool isExistOtherServer;
    bool isStartServerBindFail;
    
    int server_socket;    
   
    SocketServer( );
	~SocketServer();
    
	bool startServer();
    void killOtherServer();
    void killed();
};
#endif
