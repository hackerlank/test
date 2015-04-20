using msg;
using UnityEngine;
using System.Collections.Generic;
using System;

public class EntitiesManager
{
    /// <summary>
    /// 当前九屏玩家
    /// </summary>
    public List<OtherPlayer> CurrentNineScreenPlayers;
    /// <summary>
    /// 客户端维护的所有NPC
    /// </summary>
    public List<Npc> NpcList;

    public List<FuncNpcData> FuncNpcList; 

    public void Initialize()
    {
        FuncNpcList = new List<FuncNpcData>();
        CurrentNineScreenPlayers = new List<OtherPlayer>();
        NpcList = new List<Npc>();
    }
    /// <summary>
    /// 加入玩家
    /// </summary>
    /// <param name="player"></param>
    public void AddCharacter(OtherPlayer player)
    {
        if (!CurrentNineScreenPlayers.Contains(player))
        {
            CurrentNineScreenPlayers.Add(player);
        }
    }

    /// <summary>
    /// 移除玩家
    /// </summary>
    /// <param name="player"></param>
    public void RemoveCharacter(OtherPlayer player)
    {
        if (CurrentNineScreenPlayers.Contains(player))
        {
            CurrentNineScreenPlayers.Remove(player);
        }
    }

    /// <summary>
    /// 通过ID获得玩家引用
    /// </summary>
    /// <param name="charid"></param>
    /// <returns></returns>
    public OtherPlayer GetPlayerByID(ulong charid)
    {
        OtherPlayer player = null;

        foreach (OtherPlayer p in CurrentNineScreenPlayers)
        {
            if(charid == p.mapUserData.charid)
            {
                player = p;
                break;
            }
        }

        return player;
    }
    /// <summary>
    /// 链表中添加NPC
    /// </summary>
    /// <param name="npc"></param>
    public void AddNpc(Npc npc)
    {
        if (!NpcList.Contains(npc))
        {
            NpcList.Add(npc);
        }
    }
    /// <summary>
    /// 链表中移除NPC
    /// </summary>
    /// <param name="npc"></param>
    public void RemoveNpc(Npc npc)
    {
        if (NpcList.Contains(npc))
        {
            NpcList.Remove(npc);
        }
    }
    /// <summary>
    /// 获得NPC
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Npc GetNpc(uint id)
    {
        Npc npc = null;

        foreach (Npc n in NpcList)
        {
            if (id == n.mapNpcData.tempid)
            {
                npc = n;
                break;
            }
        }

        return npc;
    }

    public void UnInitialize()
    {
        CurrentNineScreenPlayers.Clear();
        CurrentNineScreenPlayers = null;
        NpcList.Clear();
        NpcList = null;
        FuncNpcList.Clear();
        FuncNpcList = null;
    }
}

