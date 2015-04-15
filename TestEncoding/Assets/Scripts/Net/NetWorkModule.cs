namespace Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class NetWorkModule : SingletonForMono<NetWorkModule>
    {
        public Dictionary<ushort, ushort> BlockingMsgPair = new Dictionary<ushort, ushort>();
        public Dictionary<ushort, OnMessageCallback> CallbackDict = new Dictionary<ushort, OnMessageCallback>();
        public USocket MainSocket;
        private OnSocketCallback onConnect;
        private OnSocketCallback onConnectFail;

        public void BeginCacheMsg()
        {
            Debug.Log("BeginCacheMsg");
            this.MainSocket.BeginCacheMsg();
        }

        public void Connect(IPEndPoint ipEndPoint, USocketType type, OnSocketCallback onConnect, OnSocketCallback onConnectFail)
        {
            this.onConnect = onConnect;
            this.onConnectFail = onConnectFail;
            if (this.MainSocket != null)
            {
                this.MainSocket.Dispose();
                this.MainSocket = null;
            }
            this.MainSocket = new USocket(type);
            this.MainSocket.Connect(ipEndPoint, new OnSocketCallback(this.onSocketConnect), new OnSocketCallback(this.onSocketConnectFail), new OnSocketCallback(this.onSocketClose), null);
        }

        public void DeRegisterMsg(ushort cmdId)
        {
            if (this.CallbackDict.ContainsKey(cmdId))
            {
                this.CallbackDict.Remove(cmdId);
            }
        }

        public void FinishCacheMsg()
        {
            Debug.Log("FinishCacheMsg");
            this.MainSocket.FinishCacheMsg();
        }

        private void OnDestroy()
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Dispose();
                Debug.Log("Dispose");
            }
        }

        public void onDisConnected()
        {
        }

        private void onSocketClose()
        {
        }

        private void onSocketConnect()
        {
            if (this.onConnect != null)
            {
                this.onConnect();
            }
        }

        private void onSocketConnectFail()
        {
            if (this.onConnectFail != null)
            {
                this.onConnectFail();
            }
        }

        public void RegisterBlockingMsg(ushort requestID, ushort responseID)
        {
            if (!this.BlockingMsgPair.ContainsKey(requestID))
            {
                this.BlockingMsgPair[requestID] = responseID;
            }
        }

        public void RegisterMsg(ushort cmdId, OnMessageCallback Callback)
        {
            if (!this.CallbackDict.ContainsKey(cmdId))
            {
                this.CallbackDict[cmdId] = Callback;
            }
        }

        public void Send(UMessage message, bool isToSelf = false)
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Send(message, isToSelf);
            }
        }

        public void SendImmediate(UMessage msg)
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.SendImmediate(msg);
            }
        }

        public void Update()
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.MsgqueueManager();
            }
        }
    }
}

