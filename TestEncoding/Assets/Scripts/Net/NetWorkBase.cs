using UnityEngine;
using System.Collections;
using Net;
using msg;

public class NetWorkBase
{
    public virtual void Initialize()
    {
 
    }

    public virtual void RegisterMsg()
    {
 
    }

    public virtual void SendMsg(UMessage msg,bool istoself = false)
    {
        NetWorkModule.Instance.Send(msg, istoself);
    }

    public virtual void Uninitialize()
    {
 
    }
}
