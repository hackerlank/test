using magic;
using msg;
using Net;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public void OnDataCharacterMain(UMessage msg)
    {
        Debug.Log("OnDataCharacterMain..." + msg.MsgId);
        LSingleton<LoginModule>.Instance.CreatMainPlayer();
        LSingleton<GameManager>.Instance.MainPlayer.m_dwTmpID = msg.ReadUInt32();
        if (SingletonForMono<SceneManager>.Instance.NowSceneName != "test_lol")
        {
            LSingleton<LoginModule>.Instance.ChangeToFightingScene();
        }
    }

    public void OnMapScreenBatchRefreshNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenBatchRefreshNpc");
        MSG_Ret_MapScreenBatchRefreshNpc_SC c_sc = msg.ReadProto<MSG_Ret_MapScreenBatchRefreshNpc_SC>();
        for (int i = 0; i < c_sc.data.Count; i++)
        {
            MapNpcData data = c_sc.data[i];
            if (LSingleton<GameManager>.Instance.mEntitiesManager.GetNpc((uint) data.tempid) == null)
            {
                Npc npc = new Npc();
                LSingleton<GameManager>.Instance.mEntitiesManager.AddNpc(npc);
                npc.CreatNpc(data);
            }
        }
    }

    public void OnMapScreenBatchRemoveCharacter(UMessage msg)
    {
        Debug.Log("OnMapScreenBatchRemoveCharacter");
        MSG_Ret_MapScreenBatchRemoveCharacter_SC r_sc = msg.ReadProto<MSG_Ret_MapScreenBatchRemoveCharacter_SC>();
        for (int i = 0; i < r_sc.charids.Count; i++)
        {
            OtherPlayer playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(r_sc.charids[i]);
            if (playerByID == null)
            {
                Debug.LogError("Doesnt exist player with id :" + r_sc.charids[i]);
            }
            else
            {
                LSingleton<GameManager>.Instance.mEntitiesManager.RemoveCharacter(playerByID);
                playerByID.DestroyThis();
            }
        }
    }

    public void OnMapScreenBatchRemoveNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenBatchRemoveNpc");
        MSG_Ret_MapScreenBatchRemoveNpc_SC c_sc = msg.ReadProto<MSG_Ret_MapScreenBatchRemoveNpc_SC>();
        for (int i = 0; i < c_sc.tempids.Count; i++)
        {
            Npc npc = LSingleton<GameManager>.Instance.mEntitiesManager.GetNpc((uint) c_sc.tempids[i]);
            if (npc == null)
            {
                Debug.LogError("Doesnt exist NPC with id :" + c_sc.tempids[i]);
            }
            else
            {
                LSingleton<GameManager>.Instance.mEntitiesManager.RemoveNpc(npc);
                npc.DestroyThis();
            }
        }
    }

    public void OnMapScreenFuncNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenFuncNpc");
        MSG_Ret_MapScreenFuncNpc_SC c_sc = msg.ReadProto<MSG_Ret_MapScreenFuncNpc_SC>();
        LSingleton<GameManager>.Instance.mEntitiesManager.FuncNpcList = c_sc.data;
    }

    public void OnMapScreenRefreshCharacter(UMessage msg)
    {
        Debug.Log("OnMapScreenRefreshCharacter");
        MSG_Ret_MapScreenRefreshCharacter_SC r_sc = msg.ReadProto<MSG_Ret_MapScreenRefreshCharacter_SC>();
        OtherPlayer playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(r_sc.data.charid);
        if (playerByID == null)
        {
            if (r_sc.data.charid != LSingleton<GameManager>.Instance.MainPlayer.mapUserData.charid)
            {
                playerByID = new OtherPlayer();
                playerByID.CreatPlayer(r_sc.data);
                LSingleton<GameManager>.Instance.mEntitiesManager.AddCharacter(playerByID);
            }
        }
        else
        {
            playerByID.RefreshPlayer(r_sc.data);
        }
    }

    public void OnMapScreenRefreshNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenRefreshNpc");
        MSG_Ret_MapScreenRefreshNpc_SC c_sc = msg.ReadProto<MSG_Ret_MapScreenRefreshNpc_SC>();
        Npc npc = LSingleton<GameManager>.Instance.mEntitiesManager.GetNpc((uint) c_sc.data.tempid);
        if (npc == null)
        {
            npc = new Npc();
            LSingleton<GameManager>.Instance.mEntitiesManager.AddNpc(npc);
            npc.CreatNpc(c_sc.data);
        }
        else
        {
            npc.RefreshNPC(c_sc.data);
        }
    }

    public void OnMapScreenRemoveCharacter(UMessage msg)
    {
        Debug.Log("OnMapScreenRemoveCharacter");
        MSG_Ret_MapScreenRemoveCharacter_SC r_sc = msg.ReadProto<MSG_Ret_MapScreenRemoveCharacter_SC>();
        OtherPlayer playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(r_sc.charid);
        if (playerByID == null)
        {
            Debug.LogError("Doesnt exist player with id :" + r_sc.charid);
        }
        else
        {
            LSingleton<GameManager>.Instance.mEntitiesManager.RemoveCharacter(playerByID);
            playerByID.DestroyThis();
        }
    }

    public void OnMapScreenRemoveNpc(UMessage msg)
    {
        Debug.Log("OnMapScreenRemoveNpc");
        MSG_Ret_MapScreenRemoveNpc_SC c_sc = msg.ReadProto<MSG_Ret_MapScreenRemoveNpc_SC>();
        Npc npc = LSingleton<GameManager>.Instance.mEntitiesManager.GetNpc((uint) c_sc.tempid);
        if (npc == null)
        {
            Debug.LogError("Doesnt exist player with id :" + c_sc.tempid);
        }
        else
        {
            LSingleton<GameManager>.Instance.mEntitiesManager.RemoveNpc(npc);
            npc.DestroyThis();
        }
    }

    public void OnNineScreenRefreshPlayer(UMessage msg)
    {
        Debug.Log("OnNineScreenRefreshPlayer..." + msg.MsgId);
        MSG_Ret_NineScreenRefreshPlayer_SC r_sc = msg.ReadProto<MSG_Ret_NineScreenRefreshPlayer_SC>();
        for (int i = 0; i < r_sc.data.Count; i++)
        {
            if (r_sc.data[i].charid != LSingleton<GameManager>.Instance.MainPlayer.mapUserData.charid)
            {
                MapUserData data = r_sc.data[i];
                if (LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(data.charid) == null)
                {
                    OtherPlayer player = new OtherPlayer();
                    player.CreatPlayer(data);
                    LSingleton<GameManager>.Instance.mEntitiesManager.AddCharacter(player);
                    Debug.Log(string.Concat(new object[] { "Playername:", data.name, "   ID:", data.charid }));
                }
            }
        }
    }

    public void OnRetMove(UMessage msg)
    {
        MSG_Ret_Move_SC e_sc = msg.ReadProto<MSG_Ret_Move_SC>();
        OtherPlayer playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(e_sc.charid);
        Debug.Log(string.Concat(new object[] { "OnRetMove....x:", e_sc.movedata[0].x, "  y:", e_sc.movedata[0].y }));
        if (playerByID == null)
        {
            Debug.LogError("Dosent containe player with id:" + e_sc.charid);
        }
        else if (playerByID.mapUserData.charid == LSingleton<GameManager>.Instance.MainPlayer.mapUserData.charid)
        {
            int count = e_sc.movedata.Count;
            (playerByID as MainPlayer).CheckMove(new Vector2((float) e_sc.movedata[count - 1].x, (float) e_sc.movedata[count - 1].y));
        }
        else
        {
            for (int i = 0; i < e_sc.movedata.Count; i++)
            {
                Debug.Log(string.Concat(new object[] { e_sc.movedata[i], "...", i, "...", e_sc.movedata.Count }));
                playerByID.RetMoveDataQueue.Enqueue(e_sc.movedata[i]);
            }
            if (!playerByID.isTweening)
            {
                Debug.Log("Move");
                MoveData data = playerByID.RetMoveDataQueue.Dequeue();
                playerByID.MoveTo(data);
            }
        }
    }

    public void OnUserDeath(UMessage msg)
    {
        Debug.Log("OnUserDeath");
        MSG_Ret_MainUserDeath_SC h_sc = msg.ReadProto<MSG_Ret_MainUserDeath_SC>();
        OtherPlayer playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(h_sc.tempid);
        if (playerByID != null)
        {
            playerByID.Die();
            if (playerByID == LSingleton<GameManager>.Instance.MainPlayer)
            {
                Scheduler.Instance.AddTimer((float) h_sc.waittime, false, new Scheduler.OnScheduler(this.ReqRelive));
            }
        }
    }

    public void OnUserRelive(UMessage msg)
    {
        Debug.Log("OnUserRelive");
        MSG_Ret_MainUserRelive_SC e_sc = msg.ReadProto<MSG_Ret_MainUserRelive_SC>();
        OtherPlayer playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID(e_sc.userid);
        if (playerByID != null)
        {
            playerByID.DelayRelive();
        }
    }

    public override void RegisterMsg()
    {
        SingletonForMono<NetWorkModule>.Instance.RegisterMsg(UMessage.GetMsgId(3, 2), new OnMessageCallback(this.OnDataCharacterMain));
    }

    public void ReqMove(List<MoveData> data, bool istoself = false)
    {
        UMessage msg = new UMessage();
        msg.WriteHead(0x915);
        MSG_Req_Move_CS t = new MSG_Req_Move_CS();
        foreach (MoveData data2 in data)
        {
            t.movedata.Add(data2);
        }
        t.speed = (uint) t.movedata.Count;
        t.charid = LSingleton<GameManager>.Instance.MainPlayer.mapUserData.charid;
        msg.WriteProto<MSG_Req_Move_CS>(t);
        base.SendMsg(msg, istoself);
        Debug.Log(string.Concat(new object[] { "ReqMove....x:", data[0].x, "  y:", data[0].y, "  dir:", data[0].dir }));
    }

    public void ReqRelive()
    {
        UMessage msg = new UMessage();
        msg.WriteHead(0x8fa);
        MSG_Req_MainUserRelive_CS t = new MSG_Req_MainUserRelive_CS();
        msg.WriteProto<MSG_Req_MainUserRelive_CS>(t);
        base.SendMsg(msg, false);
    }
}

