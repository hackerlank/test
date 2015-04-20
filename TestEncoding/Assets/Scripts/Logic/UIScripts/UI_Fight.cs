/*
***********************************************************************************************************
* CLR version ：$clrversion$
* Machine name ：$machinename$
* Creation time ：#time#
* Author ：hym
* Version number : 1.0
***********************************************************************************************************
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Fight
{
    public GameObject NpcNamePrefab;

    public Dictionary<uint, GameObject> NpcNames = new Dictionary<uint, GameObject>();

    public void Init()
    {
        GameObject uiroot = GameObject.Find("UI_Root");
        NpcNamePrefab = uiroot.transform.FindChild("Camera/NpcName").gameObject;
    }

    public void AddNpcName(uint id)
    {
        Npc npc = GameManager.Instance.mEntitiesManager.GetNpc(id);
        GameObject nameboard = GameObject.Instantiate(NpcNamePrefab) as GameObject;
        UIFollowTarget uiFollow = nameboard.GetComponent<UIFollowTarget>();
        uiFollow.target = npc.ModelObj.transform;
        uiFollow.offY = 1.88f;
        nameboard.transform.FindChild("playername").GetComponent<UILabel>().text = "NPC" + npc.mapNpcData.name +
                                                                                   npc.mapNpcData.tempid;

        NpcNames.Add((uint)npc.mapNpcData.tempid, nameboard);
    }

    public void RemoveNpcName(uint id)
    {
        if (NpcNames.ContainsKey(id))
        {
            GameObject.Destroy(NpcNames[id]);
            NpcNames.Remove(id);
        }        
    }
}
