using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using NGUI;

namespace Net
{    
    public delegate void OnSocketCallback();

    public class USocket
    {
        public const int BUFFER_LENGTH = 1024 * 64;
        public static bool m_bLoginSend = true;
        public bool isLostConnection = false;

        private USocketType type;
        private Socket socket;
        
        private byte[] receiveBuffer;

        private OnSocketCallback onConnect;
        private OnSocketCallback onConnectFail;
        private OnSocketCallback onClose;
        private OnSocketCallback onError;

        private Queue<UMessage> SendQueue;                              //发送消息队列

        private Queue<UMessage> ReceiveQueue;                           //接收消息队列

        private int blockingResponseMsgID = 0;                              //当前正在阻塞的回馈消息ID

        private int lastBlockingRequestMsgID = 0;                           //上一条阻塞请求消息ID

        private BlockingState blockingState = BlockingState.Sendable;   //当前阻塞状态，只对阻塞消息起效

        private Thread receiveThread;                                   //接收消息线程

        private IPEndPoint ipEndPoint;

        private int reconnecttimes = 0;                                 //重连次数

        private long lastSendTick = 0;                                  //上次发送阻塞消息的时间

        private bool whetherCacheMsg = false;                                //是否缓存消息

        //public bool isOnline = false;                                   //用于判断能否从发送队列出队进行发送操作


        public USocket(USocketType type)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.type = type;
            if (this.type == USocketType.Fir)
            {
                m_bLoginSend = true;
            }
            receiveBuffer = new byte[BUFFER_LENGTH];
            onConnect = null;
            onClose = null;
            onError = null;
            SendQueue = new Queue<UMessage>();
            ReceiveQueue = new Queue<UMessage>();
        }

        public void Connect(IPEndPoint ipEnd,  OnSocketCallback onConnect = null, OnSocketCallback onConnectFail = null, OnSocketCallback onClose = null, OnSocketCallback onError= null)
        {
            this.ipEndPoint = ipEnd;
            this.onConnect = onConnect;
            this.onConnectFail = onConnectFail;
            this.onError = onError;
            this.onClose = onClose;
            socket.NoDelay = true;
            socket.Bind(new IPEndPoint(IPAddress.Any, 0));
            try
            {
                socket.Connect(ipEnd);
                socket.Blocking = false;
                receiveThread = new Thread(new ThreadStart(OnReceive));
                receiveThread.Start();
                if (onConnect != null) onConnect();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                if (onConnectFail != null) onConnectFail();
            }
        }

        /// <summary>
        /// 断线重连
        /// </summary>
        public void ReConnectGateway()
        {


            //Debug.LogError("ReConnectGateway..." + reconnecttimes);
            //if (reconnecttimes > 2)//重连次数太多大于2，提示断网
            //{
            //    //提示断网
            //    Dispose();
            //    LoginModule.Instance.Login();
            //    return;
            //}
            ////isOnline = true;
            //reconnecttimes++;
            //try
            //{
            //    ResetSocket();
                
            //    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    socket.Connect(ipEndPoint);
            //    socket.Blocking = false;
            //    receiveThread = new Thread(new ThreadStart(OnReceive));
            //    receiveThread.Start();
            //    Debug.Log("Reconnect Success");
            //    if (onConnect != null) onConnect();
            //}
            //catch (Exception e)
            //{
            //    Debug.LogError("Reconnect Exception : " + e.Message);
            //    if (onError != null) onConnectFail();
            //}
        }

        /// <summary>
        /// 外部调用，将消息加入发送队列
        /// </summary>
        /// <param name="message"></param>
        public void Send(UMessage message, bool isToSelf = false)
        {
            //发给自己的消息直接加入
            if(isToSelf)
            {
                message.ResetIndex();
                ReceiveQueue.Enqueue(message);
                return;
            }

            if (NetWorkModule.Instance.BlockingMsgPair.ContainsKey(message.MsgId))
            {
                if (blockingState == BlockingState.Waiting)
                {
                    Debug.LogWarning("Blocking Waiting,Cant Send Msg,Last Msg ID: " + lastBlockingRequestMsgID);
                    return;
                }
                SendQueue.Enqueue(message);
                lastSendTick = DateTime.Now.Ticks;
                blockingResponseMsgID = NetWorkModule.Instance.BlockingMsgPair[message.MsgId];
                blockingState = BlockingState.Waiting;
                lastBlockingRequestMsgID = message.MsgId;
            }
            else
            {
                SendQueue.Enqueue(message);
            }
        }
        /// <summary>
        /// 从消息从消息队列出队后发送
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool send(UMessage message)
        {
            bool sendResult = false;
            Debug.Log("Sned Msg: " + GetCmdId(message) + "   cmd:" + message.MsgCmd + "   para:" + message.MsgParam);
            if (type == USocketType.Fir)
            {
                //message.CompressAndEncrypt();
                message.WriteTimeStamp(GameTime.Instance.GetIntervalMsecond());
            }
            if (type == USocketType.GateWay)
            {
                message.WriteTimeStamp(GameTime.Instance.GetIntervalMsecond());
            }
            try
            {
                socket.Send(message.Buffer, 0, message.Length, SocketFlags.None);
                sendResult = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                //isOnline = false;
                Queue<UMessage> tmpQueue = new Queue<UMessage>();
                while (SendQueue.Count > 0) { tmpQueue.Enqueue(SendQueue.Dequeue()); }
                SendQueue.Enqueue(message);
                while (tmpQueue.Count > 0) { SendQueue.Enqueue(tmpQueue.Dequeue()); }
                if (onError != null) onError();
            }
            return sendResult;
        }

        /// <summary>
        /// 不经过消息队列直接发送，用于重连登陆流程等不需要加入消息队列的操作
        /// </summary>
        /// <param name="message"></param>
        public void SendImmediate(UMessage message)
        {
            if ((type == USocketType.Fir) || m_bLoginSend)
            {
                uint timeStamp = GameTime.Instance.GetIntervalMsecond();
                message.WriteTimeStamp(timeStamp);
                message.CompressAndEncryptRC5();
            }
            else if (type == USocketType.GateWay)
            {
                message.WriteTimeStamp(GameTime.Instance.GetIntervalMsecond());
                message.CompressAndEncrypt();
            }
            try
            {
                socket.Send(message.Buffer, 0, message.Length, SocketFlags.None);
                //Debug.Log("Send Msg Immediate: " + GetCmdId(message) + "   cmd:" + message.MsgCmd + "   para:" + message.MsgParam);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                if (onError != null) onError();

                //登陆以后并且没有框才弹
                if (!isLostConnection && Util.canSendWarningDebugLog)
                {
                    isLostConnection = true;
                    NGUIManager.Instance.AddByName<NGUI_MsgBox>(NGUI_UI.NGUI_MsgBox, NGUIShowType.ONLYONE, delegate(NGUI_MsgBox script)
                    {
                        script.Init();
                        script.InitDesc("与服务器失去连接，请关闭游戏重新登陆！", enumMsgType.MsgType_LostConnection);
                    });
                }
            }
        }

        public static void PrintByteArr(byte[] byteArr, int len)
        {
            string str = "";
            for (int i = 0; i < len; i++)
            {
                str += byteArr[i] + "==";
            }
            Debug.Log(str);
        }

        private void OnReceive()
        {
            while (true)
            {
                try
                {
                    if (socket.Connected && socket.Available > 0)
                    {
                        int count = socket.Receive(receiveBuffer);
                        for (int i = 0; i < count; i++)
                        {
                            byteObject(receiveBuffer[i]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    break;
                }
            }
        }

        #region receiveByte
        
        private UMessage receiveMessage;
        private int offset;
        
        //private void masterbyteObject(byte b)
        //{
        //    if (receiveMessage == null) receiveMessage = new UMessage();
        //    receiveMessage.Buffer[offset] = b;
        //    offset++;
        //    if (offset == 8) receiveMessage.DESDencryptStep1();

        //    if (offset <= 8)
        //        return;

        //    if (offset >= receiveMessage.Length)
        //    {
        //        receiveMessage.DESDencryptStep2();
        //        receiveMessage.Decompress();
        //        receiveMessage.ReadHead();
        //        OnMessage();
        //        Debug.LogError("masterbyteObject");
        //        receiveMessage = null;
        //        offset = 0;
        //    }
        //}

        public class IntervalLayer
        {
            private byte[] buffer = new byte[8];
            private int index = 0;
            private USocket socket;

            public IntervalLayer(USocket socket)
            {
                this.socket = socket;
            }

            public void addbyte(byte b)
            {
                if (index < 8)
                {
                    buffer[index] = b;
                    index++;
                    if (index == 8)
                    {
                        UMessage.DESEncryptHandle.encdec_des(buffer, 0, 8, false);
                        index = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            socket.byteObject2(buffer[i]);
                        }
                    }
                }
            }


            public void addbyterc5(byte b)
            {
                if (index < 8)
                {
                    buffer[index] = b;
                    index++;
                    if (index == 8)
                    {
                        byte[] output = UMessage.rC5Decrypt(buffer);
                        //UMessage.DESEncryptHandle.encdec_des(buffer, 0, 8, false);
                        index = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            socket.byteObject2(output[i]);
                        }
                    }
                }
            }

        }

        private IntervalLayer interlayer;
        private  void byteObject(byte b)
        {
            if (interlayer == null) interlayer = new IntervalLayer(this);
            if (UMessage.DESEncryptHandle.bEnc && type == USocketType.Fir)
            {
                //interlayer.addbyte(b);
                interlayer.addbyterc5(b);
            }
            else if (UMessage.DESEncryptHandle.bEnc && type == USocketType.GateWay)
            {
                interlayer.addbyte(b);
            }
            else 
            {
                byteObject2(b);
            }
            //byteObject2(b);
        }

        private void byteObject2(byte b)
        {
            if (receiveMessage == null) receiveMessage = new UMessage();
            receiveMessage.Buffer[offset] = b;
            offset++;
            if (offset == 2) receiveMessage.ReadLength();

            if (offset <= 2)
                return;

            if (offset >= receiveMessage.Length)
            {
                receiveMessage.Decompress();

                ReceiveQueue.Enqueue(receiveMessage);   //接收的消息加入队列
                receiveMessage = null;
                offset = 0;
            }
        }
        #endregion

        

        private void OnMessage(UMessage msg)
        {
            msg.ReadHead();
            //Debug.Log("Receive Msg Cmd: " + msg.MsgCmd + " Param: " + msg.MsgParam);
            if (NetWorkModule.Instance.CallbackDict.ContainsKey(GetCmdId(msg)))
            {
                //Debug.Log(GetCmdId(msg));
                NetWorkModule.Instance.CallbackDict[GetCmdId(msg)](msg);
                if (msg.MsgId == blockingResponseMsgID)
                {
                    blockingState = BlockingState.Sendable; 
                    blockingResponseMsgID = 0;
                    lastSendTick = 0;
                }
            }
            else
            {
                //Debug.LogError("MessageID " + msg.MsgId + " have not been register" + " Msgcmd :" + msg.MsgCmd + " Msgpara :" + msg.MsgParam);
            }
        }

        private UInt16 GetCmdId(UMessage message)
        {
            //if(type == USocketType.Fir)
            //{
            //    return UMessage.GetMsgId(message.MsgCmd, message.MsgParam);
            //}
            //else
            //{
            //    //return UMessage.GetMsgId(message.MsgCmd, message.MsgParam);
            //    return message.MsgId;   
            //}

            return UMessage.GetMsgId(message.MsgCmd, message.MsgParam);
        }

        
        public void DisConnect()
        {
            try
            {
                socket.Disconnect(true);
            }
            catch (Exception e)
            {
                Debug.Log("连接断开异常：" + e.Message);
            }
        }

        /// <summary>
        /// 重置Socket，以便重连
        /// </summary>
        public void ResetSocket()
        {
            DisConnect();
            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }
            //isOnline = false;
            socket.Close();
            socket = null;
            offset = 0;
            LoginNetWork.ResetSendRevCount();
        }

        public void Dispose()
        {
            Util.LogError("Dispose");
            ResetSocket();
            onClose = null;
            onError = null;
            onConnect = null;
            onConnectFail = null;
            receiveBuffer = null;
            SendQueue.Clear();
            ReceiveQueue.Clear();
            LoginNetWork.ResetSendRevCount();
        }


        /// <summary>
        /// 阻塞消息延迟
        /// </summary>
        private void BlockingDelay()
        {
            long delayTick = 0;
            if (lastSendTick != 0)
            {
                delayTick = DateTime.Now.Ticks - lastSendTick;
                if (delayTick > 70000000)
                {
                    blockingState = BlockingState.Exception;
                    Debug.LogError("Delay>>>" + delayTick);
                    lastSendTick = 0;
                }
                else if (delayTick > 20000000)
                {
                    blockingState = BlockingState.TimeOut;
                    Debug.LogError("Delay>>>" + delayTick);
                }
            }
        }

        /// <summary>
        /// 收发消息队列管理
        /// </summary>
        public void MsgqueueManager()
        {
            if (!GetSocketState())
            {
                //Debug.LogError("网络中断");
                //Util.Log("@@网络中断！");
                return;
            }

            if (GetSocketState() && ReceiveQueue.Count > 0)
            {
                //如果不缓存才出队，缓存的话只入队不出队，缓存结束再出队
                if(!whetherCacheMsg)
                {
                    UMessage msg = ReceiveQueue.Dequeue();
                    OnMessage(msg);
                }
            }
            if (SendQueue.Count > 0)
            {
                UMessage msg = SendQueue.Dequeue();
                if (send(msg))
                {
                    reconnecttimes = 0;
                }
                else
                {
                    ReConnectGateway();
                }
            }
            BlockingDelay();
        }

        /// <summary>
        /// 获得Socket状态
        /// </summary>
        /// <returns></returns>
        public bool GetSocketState()
        {
            if (socket == null)
            {
                return false;
            }

            if (socket.Connected)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 开始缓存
        /// </summary>
        public void BeginCacheMsg()
        {
            whetherCacheMsg = true;
        }

        /// <summary>
        /// 结束缓存
        /// </summary>
        public void FinishCacheMsg()
        {
            whetherCacheMsg = false;
        }
    }


    /// <summary>
    /// 阻塞状态
    /// </summary>
    public enum BlockingState
    {
        Sendable,       //可发送
        Waiting,        //等待中
        TimeOut,        //超过最小等待时间
        Exception,      //异常
    }

    public enum USocketType
    {
        Fir,
        GateWay,
    }
}
