using System.Net;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Net
{
    public delegate void OnMessageCallback(UMessage message);

    public class NetWorkModule : SingletonForMono<NetWorkModule>
    {

        public USocket MainSocket;
        private OnSocketCallback onConnect;
        private OnSocketCallback onConnectFail;

        public Dictionary<UInt16, OnMessageCallback> CallbackDict = new Dictionary<UInt16, OnMessageCallback>();        //MsgID回调键值对

        public Dictionary<UInt16, UInt16> BlockingMsgPair = new Dictionary<UInt16, UInt16>();                   //阻塞消息键值对 request-response

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="type"></param>
        /// <param name="onConnect"></param>
        /// <param name="onConnectFail"></param>
        public void Connect(IPEndPoint ipEndPoint, USocketType type, OnSocketCallback onConnect, OnSocketCallback onConnectFail)
        {
            this.onConnect = onConnect;
            this.onConnectFail = onConnectFail;

            if (MainSocket != null)
            {
                MainSocket.Dispose();
                MainSocket = null;
            }
            MainSocket = new USocket(type);
            MainSocket.Connect(ipEndPoint, onSocketConnect, onSocketConnectFail, onSocketClose);
        }

        /// <summary>
        /// 注册消息回调
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="Callback"></param>
        public void RegisterMsg(UInt16 cmdId, OnMessageCallback Callback)
        {
            if (!CallbackDict.ContainsKey(cmdId))
            {
                CallbackDict[cmdId] = Callback;
            }
        }
        /// <summary>
        /// 取消注册
        /// </summary>
        /// <param name="cmdId"></param>
        /// <param name="Callback"></param>
        public void DeRegisterMsg(UInt16 cmdId)
        {
            if (CallbackDict.ContainsKey(cmdId))
            {
                CallbackDict.Remove(cmdId);
            }
        }

        /// <summary>
        /// 注册阻塞消息 RequestID-ResponseID
        /// </summary>
        /// <param name="requestID"></param>
        /// <param name="responseID"></param>
        public void RegisterBlockingMsg(UInt16 requestID, UInt16 responseID)
        {
            if (!BlockingMsgPair.ContainsKey(requestID))
            {
                BlockingMsgPair[requestID] = responseID;
            }
        }

        public void Send(UMessage message,bool isToSelf = false)
        {
            if (MainSocket != null)
            {
                MainSocket.Send(message, isToSelf);
            }
        }

        public void SendImmediate(UMessage msg)
        {
            if (MainSocket != null)
            {
                MainSocket.SendImmediate(msg);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void BeginCacheMsg()
        {
            Debug.Log("BeginCacheMsg");
            Util.Log("@@BeginCacheMsg");
            MainSocket.BeginCacheMsg();
        }

        public void FinishCacheMsg()
        {
            Debug.Log("FinishCacheMsg");
            Util.Log("@@FinishCacheMsg");
            MainSocket.FinishCacheMsg();
        }
        
        private void onSocketConnect()
        {
            if (onConnect != null)
            {
                onConnect();
            }
        }

        private void onSocketConnectFail()
        {
            if (onConnectFail != null)
            {
                onConnectFail();
            }
        }

        private void onSocketClose()
        {

        }

        public void onDisConnected()
        {
 
        }

        public void Update()
        {
            if (MainSocket != null)
            {
                MainSocket.MsgqueueManager();
            }
        }

        void OnDestroy()
        {
            if(MainSocket == null)
            {
                return;
            }
            MainSocket.Dispose();
            Debug.Log("Dispose");
        }
    }
}

