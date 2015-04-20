using UnityEngine;
using System.Collections;
using System;
using Net;
using msg;
using magic;
using System.Collections.Generic;

public class PlayerNetWork : NetWorkBase
{
    public override void Initialize()
    {
        RegisterMsg();
    }

    public override void RegisterMsg()
    {
        //收到主用户数据
        NetWorkModule.Instance.RegisterMsg(UMessage.GetMsgId(3, 2), OnDataCharacterMain);

        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_Move_SC, OnRetMove);

        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenRefreshCharacter_SC, OnMapScreenRefreshCharacter);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenRemoveCharacter_SC, OnMapScreenRemoveCharacter);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenBatchRemoveCharacter_SC, OnMapScreenBatchRemoveCharacter);

        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenBatchRefreshNpc_SC, OnMapScreenBatchRefreshNpc);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenBatchRemoveNpc_SC, OnMapScreenBatchRemoveNpc);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenRefreshNpc_SC, OnMapScreenRefreshNpc);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenRemoveNpc_SC, OnMapScreenRemoveNpc);

        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_DataCharacterMain_SC, OnDataCharacterMain);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_NineScreenRefreshPlayer_SC, OnNineScreenRefreshPlayer);

        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MainUserDeath_SC, OnUserDeath);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MainUserRelive_SC, OnUserRelive);
        //NetWorkModule.Instance.RegisterMsg((UInt16)CommandID.MSG_Ret_MapScreenFuncNpc_SC, OnMapScreenFuncNpc);
    }

    /// <summary>
    /// 发送移动消息
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="destpoint"></param>
    /// <param name="speed"></param>
    public void ReqMove(List<MoveData> data, bool istoself = false)
    {
        UMessage msg = new UMessage();
        msg.WriteHead((UInt16)CommandID.MSG_Req_Move_CS);

        MSG_Req_Move_CS msgb = new MSG_Req_Move_CS();


        foreach(MoveData md in data)
        {
            msgb.movedata.Add(md);
        }

        msgb.speed = (uint)msgb.movedata.Count;

        msgb.charid = GameManager.Instance.MainPlayer.mapUserData.charid;
        msg.WriteProto<MSG_Req_Move_CS>(msgb);

        base.SendMsg(msg,istoself);

        Debug.Log("ReqMove....x:" + data[0].x + "  y:" + data[0].y + "  dir:" + data[0].dir);
    }

    /// <summary>
    /// 收到移动消息
    /// </summary>
    /// <param name="msg"></param>
    public void OnRetMove(UMessage msg)
    {
        MSG_Ret_Move_SC data = msg.ReadProto<MSG_Ret_Move_SC>();
        OtherPlayer player = GameManager.Instance.mEntitiesManager.GetPlayerByID(data.charid);
        Debug.Log("OnRetMove....x:" + data.movedata[0].x + "  y:" + data.movedata[0].y);
        if(player == null)
        {
            Debug.LogError("Dosent containe player with id:" + data.charid);
            return;
        }

        if (player.mapUserData.charid == GameManager.Instance.MainPlayer.mapUserData.charid)
        {
            int i = data.movedata.Count;
            (player as MainPlayer).CheckMove(new Vector2(data.movedata[i - 1].x, data.movedata[i - 1].y));
            return;
        }

        for(int i = 0;i < data.movedata.Count; i++)
        {
            Debug.Log(data.movedata[i] + "..." + i + "..." + data.movedata.Count);
            player.RetMoveDataQueue.Enqueue(data.movedata[i]);
        }

        if(!player.isTweening)
        {
            Debug.Log("Move");
            MoveData md0 = player.RetMoveDataQueue.Dequeue();
            player.MoveTo(md0);
        }
    }

    /// <summary>
    /// 收到主人物数据
    /// </summary>
    /// <param name="msg"></param>
    public void OnDataCharacterMain(UMessage msg)
    {
        Debug.Log("OnDataCharacterMain..." + msg.MsgId);

        //if (GameManager.Instance.MainPlayer != null)
        //{
        //    GameManager.Instance.MainPlayer.ReconnectResetPlayer();
        //}

        //GameManager.Instance.MainPlayerData = cmdata;
        stMainUserDataUserCmd st = new stMainUserDataUserCmd();
        msg.ReadStruct<stMainUserDataUserCmd>(st);

        GameManager.Instance.MainPlayer.mainUserData = st;

        //收到主用户数据才加载配置
        ConfigManager.Instance.Initialize();
        GameManager.Instance.mEntitiesManager = new EntitiesManager();
        GameManager.Instance.mEntitiesManager.Initialize();

        //收到主用户数据才初始化表格
        try
        {
            t_object_config.Init(configInitHandle);
            Util.Log("加载道具表！");
        }

        catch (Exception exception)
        {
            Util.LogError(exception.ToString(), false);
        }

        LoginModule.Instance.CreatMainPlayer();
        {
            GameManager.Instance.MainPlayer.m_dwTmpID = msg.ReadUInt32();
        }
        if (SceneManager.Instance.NowSceneName != "test_lol")
        {
            LoginModule.Instance.ChangeToFightingScene();
        }

        
        //else
        //{
        //    Debug.LogError("Reconnect creat player");
        //    foreach (OtherPlayer p in GameManager.Instance.mEntitiesManager.CurrentNineScreenPlayers)
        //    {
        //        if (p == GameManager.Instance.MainPlayer)
        //        {
        //            continue;
        //        }
        //        GameManager.Instance.mEntitiesManager.RemoveCharacter(p);
        //        p.DestroyThis();
        //    }
        //    foreach (Npc n in GameManager.Instance.mEntitiesManager.NpcList)
        //    {
        //        GameManager.Instance.mEntitiesManager.RemoveNpc(n);
        //        n.DestroyThis();
        //    }
        //    LoginModule.Instance.CreatMainPlayer();
        //}
    }

    private void configInitHandle()
    {

    }

    /// <summary>
    /// 收到九屏数据
    /// </summary>
    /// <param name="msg"></param>
    public void OnNineScreenRefreshPlayer(UMessage msg)
    {
        Debug.Log("OnNineScreenRefreshPlayer..." + msg.MsgId);
        MSG_Ret_NineScreenRefreshPlayer_SC mdata = msg.ReadProto<MSG_Ret_NineScreenRefreshPlayer_SC>();

        for (int i = 0; i < mdata.data.Count; i++)
        {
            if (mdata.data[i].charid == GameManager.Instance.MainPlayer.mapUserData.charid)
            {
                continue;
            }
            MapUserData cmdata = mdata.data[i];

            if (GameManager.Instance.mEntitiesManager.GetPlayerByID(cmdata.charid) == null)
            {
                OtherPlayer p = new OtherPlayer();

                p.CreatPlayer(cmdata);
                GameManager.Instance.mEntitiesManager.AddCharacter(p);
                Debug.Log("Playername:" + cmdata.name + "   ID:" + cmdata.charid);
            }
        }

        //NetWorkModule.Instance.MainSocket.isOnline = true;
    }

    /// <summary>
    /// 刷新地图角色，当地图上的角色新加入，或者外形有所改变时发送。
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenRefreshCharacter(UMessage msg)
    {
        Debug.Log("OnMapScreenRefreshCharacter");
        MSG_Ret_MapScreenRefreshCharacter_SC mdata = msg.ReadProto<MSG_Ret_MapScreenRefreshCharacter_SC>();

        OtherPlayer p = GameManager.Instance.mEntitiesManager.GetPlayerByID(mdata.data.charid);

        if (p == null)
        {
            if (mdata.data.charid == GameManager.Instance.MainPlayer.mapUserData.charid)
            {
                return;
            }
            p = new OtherPlayer();
            p.CreatPlayer(mdata.data);
            GameManager.Instance.mEntitiesManager.AddCharacter(p);
        }
        else
        {
            p.RefreshPlayer(mdata.data);
        }
    }


    /// <summary>
    /// 从地图上删除角色
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenRemoveCharacter(UMessage msg)
    {
        Debug.Log("OnMapScreenRemoveCharacter");
        MSG_Ret_MapScreenRemoveCharacter_SC mdata = msg.ReadProto<MSG_Ret_MapScreenRemoveCharacter_SC>();

        OtherPlayer p = GameManager.Instance.mEntitiesManager.GetPlayerByID(mdata.charid);
        if (p == null)
        {
            Debug.LogError("Doesnt exist player with id :" + mdata.charid);
            return;
        }
        GameManager.Instance.mEntitiesManager.RemoveCharacter(p);
        p.DestroyThis();
    }
    /// <summary>
    /// 批量从地图上删除角色
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenBatchRemoveCharacter(UMessage msg)
    {
        Debug.Log("OnMapScreenBatchRemoveCharacter");
        MSG_Ret_MapScreenBatchRemoveCharacter_SC mdata = msg.ReadProto<MSG_Ret_MapScreenBatchRemoveCharacter_SC>();

        for (int i = 0; i < mdata.charids.Count; i++)
        {
            OtherPlayer p = GameManager.Instance.mEntitiesManager.GetPlayerByID(mdata.charids[i]);

            if (p == null)
            {
                Debug.LogError("Doesnt exist player with id :" + mdata.charids[i]);
                continue;
            }

            GameManager.Instance.mEntitiesManager.RemoveCharacter(p);
            p.DestroyThis();
        }
    }


    /// <summary>
    /// 玩家死亡
    /// </summary>
    /// <param name="msg"></param>
    public void OnUserDeath(UMessage msg)
    {
        Debug.Log("OnUserDeath");
        MSG_Ret_MainUserDeath_SC data = msg.ReadProto<MSG_Ret_MainUserDeath_SC>();

        OtherPlayer p = GameManager.Instance.mEntitiesManager.GetPlayerByID(data.tempid);
        if (p != null)
        {
            p.Die();
            if (p == GameManager.Instance.MainPlayer)
            {
                Scheduler.Instance.AddTimer(data.waittime, false, ReqRelive);
            }
        }
    }

    /// <summary>
    /// 批量刷新NPC
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenBatchRefreshNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenBatchRefreshNpc");
        MSG_Ret_MapScreenBatchRefreshNpc_SC mdata = msg.ReadProto<MSG_Ret_MapScreenBatchRefreshNpc_SC>();

        for (int i = 0; i < mdata.data.Count; i++)
        {
            MapNpcData cmdata = mdata.data[i];

            if (GameManager.Instance.mEntitiesManager.GetNpc((uint)cmdata.tempid) == null)
            {
                Npc npc = new Npc();
                GameManager.Instance.mEntitiesManager.AddNpc(npc);
                npc.CreatNpc(cmdata);
            }
        }
    }
    /// <summary>
    /// 批量删除NPC
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenBatchRemoveNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenBatchRemoveNpc");
        MSG_Ret_MapScreenBatchRemoveNpc_SC mdata = msg.ReadProto<MSG_Ret_MapScreenBatchRemoveNpc_SC>();

        for (int i = 0; i < mdata.tempids.Count; i++)
        {
            Npc n = GameManager.Instance.mEntitiesManager.GetNpc((uint)mdata.tempids[i]);

            if (n == null)
            {
                Debug.LogError("Doesnt exist NPC with id :" + mdata.tempids[i]);
                continue;
            }

            GameManager.Instance.mEntitiesManager.RemoveNpc(n);
            n.DestroyThis();
        }
    }

    /// <summary>
    /// 增加一个NPC，当已存在时，则刷新
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenRefreshNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenRefreshNpc");
        MSG_Ret_MapScreenRefreshNpc_SC mdata = msg.ReadProto<MSG_Ret_MapScreenRefreshNpc_SC>();

        Npc n = GameManager.Instance.mEntitiesManager.GetNpc((uint)mdata.data.tempid);

        if (n == null)
        {
            n = new Npc();
            GameManager.Instance.mEntitiesManager.AddNpc(n);
            n.CreatNpc(mdata.data);
        }
        else
        {
            n.RefreshNPC(mdata.data);
        }
    }

    /// <summary>
    /// 从地图上删除NPC
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenRemoveNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenRemoveNpc");
        MSG_Ret_MapScreenRemoveNpc_SC mdata = msg.ReadProto<MSG_Ret_MapScreenRemoveNpc_SC>();

        

        Npc n = GameManager.Instance.mEntitiesManager.GetNpc((uint)mdata.tempid);

        if (n == null)
        {
            Debug.LogError("Doesnt exist player with id :" + mdata.tempid);
            return;
        }

        GameManager.Instance.mEntitiesManager.RemoveNpc(n);
        n.DestroyThis();
    }
    /// <summary>
    /// 发送本场景内所有功能NPC信息
    /// </summary>
    /// <param name="msg"></param>
    public void OnMapScreenFuncNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenFuncNpc");
        MSG_Ret_MapScreenFuncNpc_SC mdata = msg.ReadProto<MSG_Ret_MapScreenFuncNpc_SC>();

        GameManager.Instance.mEntitiesManager.FuncNpcList = mdata.data;
    }

    /// <summary>
    /// 玩家复活
    /// </summary>
    /// <param name="msg"></param>
    public void OnUserRelive(UMessage msg)
    {
        Debug.Log("OnUserRelive");
        MSG_Ret_MainUserRelive_SC data = msg.ReadProto<MSG_Ret_MainUserRelive_SC>();

        OtherPlayer p = GameManager.Instance.mEntitiesManager.GetPlayerByID(data.userid);
        if (p != null)
        {
            p.DelayRelive();
        }
    }

    /// <summary>
    /// 申请复活
    /// </summary>
    public void ReqRelive()
    {
        UMessage msg = new UMessage();
        msg.WriteHead((UInt16)CommandID.MSG_Req_MainUserRelive_CS);

        MSG_Req_MainUserRelive_CS data = new MSG_Req_MainUserRelive_CS();

        msg.WriteProto<MSG_Req_MainUserRelive_CS>(data);

        base.SendMsg(msg);
    }
}
