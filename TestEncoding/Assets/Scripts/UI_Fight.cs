using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Fight
{
    public GameObject NpcNamePrefab;
    public Dictionary<uint, GameObject> NpcNames = new Dictionary<uint, GameObject>();

    public void AddNpcName(uint id)
    {
        Npc npc = LSingleton<GameManager>.Instance.mEntitiesManager.GetNpc(id);
        GameObject obj2 = UnityEngine.Object.Instantiate(this.NpcNamePrefab) as GameObject;
        UIFollowTarget component = obj2.GetComponent<UIFollowTarget>();
        component.target = npc.ModelObj.transform;
        component.offY = 1.88f;
        obj2.transform.FindChild("playername").GetComponent<UILabel>().text = "NPC" + npc.mapNpcData.name + npc.mapNpcData.tempid;
        this.NpcNames.Add((uint) npc.mapNpcData.tempid, obj2);
    }

    public void Init()
    {
        GameObject obj2 = GameObject.Find("UIRoot");
        this.NpcNamePrefab = obj2.transform.FindChild("Camera/NpcName").gameObject;
    }

    public void RemoveNpcName(uint id)
    {
        if (this.NpcNames.ContainsKey(id))
        {
            UnityEngine.Object.Destroy(this.NpcNames[id]);
            this.NpcNames.Remove(id);
        }
    }
}

