using Net;
using System;
using System.Runtime.InteropServices;

public class NetWorkBase
{
    public virtual void Initialize()
    {
    }

    public virtual void RegisterMsg()
    {
    }

    public virtual void SendMsg(UMessage msg, bool istoself = false)
    {
        SingletonForMono<NetWorkModule>.Instance.Send(msg, istoself);
    }

    public virtual void Uninitialize()
    {
    }
}

