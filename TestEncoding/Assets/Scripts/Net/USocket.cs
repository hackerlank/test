namespace Net
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UnityEngine;

    public class USocket
    {
        private int blockingResponseMsgID;
        private BlockingState blockingState;
        public const int BUFFER_LENGTH = 0x10000;
        private IntervalLayer interlayer;
        private IPEndPoint ipEndPoint;
        private int lastBlockingRequestMsgID;
        private long lastSendTick;
        public static bool m_bLoginSend = true;
        private int offset;
        private OnSocketCallback onClose;
        private OnSocketCallback onConnect;
        private OnSocketCallback onConnectFail;
        private OnSocketCallback onError;
        private byte[] receiveBuffer;
        private UMessage receiveMessage;
        private Queue<UMessage> ReceiveQueue;
        private Thread receiveThread;
        private int reconnecttimes;
        private Queue<UMessage> SendQueue;
        private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private USocketType type;
        private bool whetherCacheMsg;

        public USocket(USocketType type)
        {
            this.type = type;
            if (this.type == USocketType.Fir)
            {
                m_bLoginSend = true;
            }
            this.receiveBuffer = new byte[0x10000];
            this.onConnect = null;
            this.onClose = null;
            this.onError = null;
            this.SendQueue = new Queue<UMessage>();
            this.ReceiveQueue = new Queue<UMessage>();
        }

        public void BeginCacheMsg()
        {
            //this.whetherCacheMsg = true;
            this.whetherCacheMsg = false;
        }

        private void BlockingDelay()
        {
            long num = 0L;
            if (this.lastSendTick != 0)
            {
                num = DateTime.Now.Ticks - this.lastSendTick;
                if (num > 0x42c1d80L)
                {
                    this.blockingState = BlockingState.Exception;
                    Debug.LogError("Delay>>>" + num);
                    this.lastSendTick = 0L;
                }
                else if (num > 0x1312d00L)
                {
                    this.blockingState = BlockingState.TimeOut;
                    Debug.LogError("Delay>>>" + num);
                }
            }
        }

        private void byteObject(byte b)
        {
            if (this.interlayer == null)
            {
                this.interlayer = new IntervalLayer(this);
            }
            if (UMessage.DESEncryptHandle.bEnc && (this.type == USocketType.Fir))
            {
                this.interlayer.addbyterc5(b);
            }
            else if (UMessage.DESEncryptHandle.bEnc && (this.type == USocketType.GateWay))
            {
                this.interlayer.addbyte(b);
            }
            else
            {
                this.byteObject2(b);
            }
        }

        private void byteObject2(byte b)
        {
            if (this.receiveMessage == null)
            {
                this.receiveMessage = new UMessage();
            }
            this.receiveMessage.Buffer[this.offset] = b;
            this.offset++;
            if (this.offset == 2)
            {
                this.receiveMessage.ReadLength();
            }
            if ((this.offset > 2) && (this.offset >= this.receiveMessage.Length))
            {
                this.receiveMessage.Decompress();
                this.ReceiveQueue.Enqueue(this.receiveMessage);
                this.receiveMessage = null;
                this.offset = 0;
            }
        }

        public void Connect(IPEndPoint ipEnd, OnSocketCallback onConnect = null, OnSocketCallback onConnectFail = null, OnSocketCallback onClose = null, OnSocketCallback onError = null)
        {
            this.ipEndPoint = ipEnd;
            this.onConnect = onConnect;
            this.onConnectFail = onConnectFail;
            this.onError = onError;
            this.onClose = onClose;
            this.socket.NoDelay = true;
            this.socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            try
            {
                this.socket.Connect(ipEnd);
                this.socket.Blocking = false;
                this.receiveThread = new Thread(new ThreadStart(this.OnReceive));
                this.receiveThread.Start();
                if (onConnect != null)
                {
                    onConnect();
                }
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                if (onConnectFail != null)
                {
                    onConnectFail();
                }
            }
        }

        public void DisConnect()
        {
            try
            {
                this.socket.Disconnect(true);
            }
            catch (Exception exception)
            {
                Debug.Log("连接断开异常：" + exception.Message);
            }
        }

        public void Dispose()
        {
            Debug.LogError("Dispose");
            this.ResetSocket();
            this.onClose = null;
            this.onError = null;
            this.onConnect = null;
            this.onConnectFail = null;
            this.receiveBuffer = null;
            this.SendQueue.Clear();
            this.ReceiveQueue.Clear();
            LoginNetWork.ResetSendRevCount();
        }

        public void FinishCacheMsg()
        {
            this.whetherCacheMsg = false;
        }

        private ushort GetCmdId(UMessage message)
        {
            return UMessage.GetMsgId(message.MsgCmd, message.MsgParam);
        }

        public bool GetSocketState()
        {
            if (this.socket == null)
            {
                return false;
            }
            return this.socket.Connected;
        }

        public void MsgqueueManager()
        {
            if ((this.GetSocketState() && (this.ReceiveQueue.Count > 0)) && !this.whetherCacheMsg)
            {
                UMessage msg = this.ReceiveQueue.Dequeue();
                this.OnMessage(msg);
            }
            if (this.SendQueue.Count > 0)
            {
                UMessage message = this.SendQueue.Dequeue();
                if (this.send(message))
                {
                    this.reconnecttimes = 0;
                }
                else
                {
                    this.ReConnectGateway();
                }
            }
            this.BlockingDelay();
        }

        private void OnMessage(UMessage msg)
        {
            msg.ReadHead();
            Debug.Log(string.Concat(new object[] { "Receive Msg Cmd: ", msg.MsgCmd, " Param: ", msg.MsgParam }));
            if (SingletonForMono<NetWorkModule>.Instance.CallbackDict.ContainsKey(this.GetCmdId(msg)))
            {
                SingletonForMono<NetWorkModule>.Instance.CallbackDict[this.GetCmdId(msg)](msg);
                if (msg.MsgId == this.blockingResponseMsgID)
                {
                    this.blockingState = BlockingState.Sendable;
                    this.blockingResponseMsgID = 0;
                    this.lastSendTick = 0L;
                }
            }
            else
            {
                Debug.LogError(string.Concat(new object[] { "MessageID ", msg.MsgId, " have not been register Msgcmd :", msg.MsgCmd, " Msgpara :", msg.MsgParam }));
            }
        }

        private void OnReceive()
        {
            while (true)
            {
                try
                {
                    if (this.socket.Connected && (this.socket.Available > 0))
                    {
                        int num = this.socket.Receive(this.receiveBuffer);
                        for (int i = 0; i < num; i++)
                        {
                            this.byteObject(this.receiveBuffer[i]);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception.Message);
                    return;
                }
            }
        }

        public static void PrintByteArr(byte[] byteArr, int len)
        {
            string message = string.Empty;
            for (int i = 0; i < len; i++)
            {
                message = message + byteArr[i] + "==";
            }
            Debug.Log(message);
        }

        public void ReConnectGateway()
        {
            Debug.LogError("ReConnectGateway..." + this.reconnecttimes);
            if (this.reconnecttimes > 2)
            {
                this.Dispose();
                LSingleton<LoginModule>.Instance.Login();
            }
            else
            {
                this.reconnecttimes++;
                try
                {
                    this.ResetSocket();
                    this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    this.socket.Connect(this.ipEndPoint);
                    this.socket.Blocking = false;
                    this.receiveThread = new Thread(new ThreadStart(this.OnReceive));
                    this.receiveThread.Start();
                    Debug.Log("Reconnect Success");
                    if (this.onConnect != null)
                    {
                        this.onConnect();
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError("Reconnect Exception : " + exception.Message);
                    if (this.onError != null)
                    {
                        this.onConnectFail();
                    }
                }
            }
        }

        public void ResetSocket()
        {
            this.DisConnect();
            if (this.receiveThread != null)
            {
                this.receiveThread.Abort();
                this.receiveThread = null;
            }
            this.socket.Close();
            this.socket = null;
            this.offset = 0;
            LoginNetWork.ResetSendRevCount();
        }

        private bool send(UMessage message)
        {
            bool flag = false;
            Debug.Log(string.Concat(new object[] { "Sned Msg: ", this.GetCmdId(message), "   cmd:", message.MsgCmd, "   para:", message.MsgParam }));
            if (this.type == USocketType.Fir)
            {
                message.WriteTimeStamp(LSingleton<GameTime>.Instance.GetIntervalMsecond());
            }
            if (this.type == USocketType.GateWay)
            {
                message.WriteTimeStamp(LSingleton<GameTime>.Instance.GetIntervalMsecond());
            }
            try
            {
                this.socket.Send(message.Buffer, 0, message.Length, SocketFlags.None);
                flag = true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                Queue<UMessage> queue = new Queue<UMessage>();
                while (this.SendQueue.Count > 0)
                {
                    queue.Enqueue(this.SendQueue.Dequeue());
                }
                this.SendQueue.Enqueue(message);
                while (queue.Count > 0)
                {
                    this.SendQueue.Enqueue(queue.Dequeue());
                }
                if (this.onError != null)
                {
                    this.onError();
                }
            }
            return flag;
        }

        public void Send(UMessage message, bool isToSelf = false)
        {
            if (isToSelf)
            {
                message.ResetIndex();
                this.ReceiveQueue.Enqueue(message);
            }
            else if (SingletonForMono<NetWorkModule>.Instance.BlockingMsgPair.ContainsKey(message.MsgId))
            {
                if (this.blockingState == BlockingState.Waiting)
                {
                    Debug.LogWarning("Blocking Waiting,Cant Send Msg,Last Msg ID: " + this.lastBlockingRequestMsgID);
                }
                else
                {
                    this.SendQueue.Enqueue(message);
                    this.lastSendTick = DateTime.Now.Ticks;
                    this.blockingResponseMsgID = SingletonForMono<NetWorkModule>.Instance.BlockingMsgPair[message.MsgId];
                    this.blockingState = BlockingState.Waiting;
                    this.lastBlockingRequestMsgID = message.MsgId;
                }
            }
            else
            {
                this.SendQueue.Enqueue(message);
            }
        }

        public void SendImmediate(UMessage message)
        {
            if ((this.type == USocketType.Fir) || m_bLoginSend)
            {
                uint intervalMsecond = LSingleton<GameTime>.Instance.GetIntervalMsecond();
                message.WriteTimeStamp(intervalMsecond);
                message.CompressAndEncryptRC5();
            }
            else if (this.type == USocketType.GateWay)
            {
                message.WriteTimeStamp(LSingleton<GameTime>.Instance.GetIntervalMsecond());
                message.CompressAndEncrypt();
            }
            try
            {
                this.socket.Send(message.Buffer, 0, message.Length, SocketFlags.None);
                Debug.Log(string.Concat(new object[] { "Send Msg Immediate: ", this.GetCmdId(message), "   cmd:", message.MsgCmd, "   para:", message.MsgParam }));
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                if (this.onError != null)
                {
                    this.onError();
                }
            }
        }

        public class IntervalLayer
        {
            private byte[] buffer = new byte[8];
            private int index;
            private USocket socket;

            public IntervalLayer(USocket socket)
            {
                this.socket = socket;
            }

            public void addbyte(byte b)
            {
                if (this.index < 8)
                {
                    this.buffer[this.index] = b;
                    this.index++;
                    if (this.index == 8)
                    {
                        UMessage.DESEncryptHandle.encdec_des(this.buffer, 0, 8, false);
                        this.index = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            this.socket.byteObject2(this.buffer[i]);
                        }
                    }
                }
            }

            public void addbyterc5(byte b)
            {
                if (this.index < 8)
                {
                    this.buffer[this.index] = b;
                    this.index++;
                    if (this.index == 8)
                    {
                        byte[] buffer = UMessage.rC5Decrypt(this.buffer);
                        this.index = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            this.socket.byteObject2(buffer[i]);
                        }
                    }
                }
            }
        }
    }
}

