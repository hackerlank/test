#ifndef _H_MessageHandler_H
#define _H_MessageHandler_H
class SocketClient;
class Message;
class MessageHandler {	
public:	
	virtual SocketClient* getGateway() = 0;
	virtual SocketClient* getServer() = 0;
	/**
	 * ���������յ�һ����Ϣ������Ϣ������������Ϣ��Ҳ��������Ӧ��Ϣ
	 * ��Ҫ����һ����Ӧ�� �������ֹ���ӣ�����ѡ���׳�ConnectionException�쳣�������쳣������ֹ���ӡ�
	 * 
	 * @param gc
	 * @param request
	 * @return
	 * @throws ConnectionException
	 */
	virtual void receiveMessage(SocketClient* gc,Message* message)=0;
	
	virtual void clearReceivedMessage()=0;
	/**
	 * ��ʱ��û����Ϣ���͸������������ô˷����� �˷������Է���һ����Ϣ���˷��ص���Ϣ�����͸���������
	 * 
	 * @return
	 */
	virtual Message* idleTimeout()=0;
	
	/**
	 * �յ�HTTPӦ��
	 * @param type
	 * @param data
	 * @param header
	 */
	virtual void httpReceived(int type,ByteBuffer* data,String header)=0;
	
	static const int ILLEGAL_ARGUMENT = 0;
	static const int CONNECTION_NOT_FOUND = 1;
	static const int SECURITY_PERMISSION = 2;
	static const int OTHER_IO_ERROR = 3;       
};


#endif
