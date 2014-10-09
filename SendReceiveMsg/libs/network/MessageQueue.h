#ifndef __client1__MessageQueue__
#define __client1__MessageQueue__

#include <iostream>
#include "message.h"
#include "SocketClient.h"

class MessageQueue
{
      MessageQueue();
    SocketClient* sclient;
public:
  
    ~MessageQueue();
static MessageQueue* getMessage();
    //»ñÈ¡°ü
    
    Message* getMessagedata(int commandId);
    void sendMessage(Message * data,bool flag);
    Message * pickReceivedMessage();
    void start();
    void initSocket(char * ip,int port);
};
#endif /* defined(__client1__MessageQueue__) */
