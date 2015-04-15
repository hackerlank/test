using System;
using System.Collections.Generic;
using msg;

public class EntitiesManager
{
    public List<OtherPlayer> CurrentNineScreenPlayers;
    public List<FuncNpcData> FuncNpcList;
    public List<Npc> NpcList;

    public void AddCharacter(OtherPlayer player)
    {
        if (!this.CurrentNineScreenPlayers.Contains(player))
        {
            this.CurrentNineScreenPlayers.Add(player);
        }
    }

    public void AddNpc(Npc npc)
    {
        if (!this.NpcList.Contains(npc))
        {
            this.NpcList.Add(npc);
        }
    }

    public Npc GetNpc(uint id)
    {
        foreach (Npc npc2 in this.NpcList)
        {
            if (id == npc2.mapNpcData.tempid)
            {
                return npc2;
            }
        }
        return null;
    }

    public OtherPlayer GetPlayerByID(ulong charid)
    {
        foreach (OtherPlayer player2 in this.CurrentNineScreenPlayers)
        {
            if (charid == player2.mapUserData.charid)
            {
                return player2;
            }
        }
        return null;
    }

    public void Initialize()
    {
        this.FuncNpcList = new List<FuncNpcData>();
        this.CurrentNineScreenPlayers = new List<OtherPlayer>();
        this.NpcList = new List<Npc>();
    }

    public void RemoveCharacter(OtherPlayer player)
    {
        if (this.CurrentNineScreenPlayers.Contains(player))
        {
            this.CurrentNineScreenPlayers.Remove(player);
        }
    }

    public void RemoveNpc(Npc npc)
    {
        if (this.NpcList.Contains(npc))
        {
            this.NpcList.Remove(npc);
        }
    }

    public void UnInitialize()
    {
        this.CurrentNineScreenPlayers.Clear();
        this.CurrentNineScreenPlayers = null;
        this.NpcList.Clear();
        this.NpcList = null;
        this.FuncNpcList.Clear();
        this.FuncNpcList = null;
    }
}

