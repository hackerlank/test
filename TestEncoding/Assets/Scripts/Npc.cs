using msg;
using System;
using UnityEngine;

public class Npc : CharactorBase
{
    public MapNpcData mapNpcData;

    public void CreatNpc(MapNpcData data)
    {
        Debug.Log(string.Concat(new object[] { "CreatNpc....", data.name, "...", data.tempid, "...X:", data.x, " Y:", data.y }));
        this.mapNpcData = data;
        base.CurrentPosition2D = new Vector2((float) data.x, (float) data.y);
        string modePath = this.GetModePath((uint) data.baseid);
        base.ModelObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(modePath)) as GameObject;
        base.animator = base.ModelObj.GetComponent<Animator>();
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(this.GetAnimatorControllerPath((uint) data.baseid));
        base.animator.runtimeAnimatorController = controller;
        base.animator.applyRootMotion = true;
        if (base.animator.applyRootMotion)
        {
            CapsuleCollider collider = base.ModelObj.AddComponent<CapsuleCollider>();
            collider.isTrigger = false;
            collider.material = null;
            collider.center = new Vector3(0f, 1f, 0f);
            collider.radius = 0.6f;
            collider.height = 2f;
            collider.direction = 1;
        }
        base.ModelObj.transform.position = GraphUtils.GetWorldPosByServerPos(base.CurrentPosition2D);
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(base.CurrentPosition2D);
        worldPosByServerPos.y = MapHightDataHolder.GetMapHeight(worldPosByServerPos.x, worldPosByServerPos.z);
        base.ModelObj.transform.position = worldPosByServerPos;
        base.TargetPos = worldPosByServerPos;
        base.ModelObj.transform.rotation = base.GetDirByServerDir((int) data.dir);
        base.ModelObj.name = "NPC" + data.name + data.tempid;
        base.moveSpeed = 0.5f / (((float) this.mapNpcData.movespeed) / 1000f);
        LSingleton<GameManager>.Instance.FightUI.AddNpcName((uint) data.tempid);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public override void DelayRelive()
    {
    }

    public override void DestroyThis()
    {
        base.DestroyThis();
        base.isTweening = false;
        base.isPathfinding = false;
        LSingleton<GameManager>.Instance.FightUI.RemoveNpcName((uint) this.mapNpcData.tempid);
        UnityEngine.Object.Destroy(base.ModelObj);
        this.mapNpcData = null;
    }

    public override void Die()
    {
        base.Die();
    }

    public string GetAnimatorControllerPath(uint id)
    {
        string str = string.Empty;
        t_npc_config config = t_npc_config.GetConfig(id);
        if (config.m_id == 0x7d0)
        {
            return "AnimatorController/gailun";
        }
        if (config.m_id == 0x7d1)
        {
            str = "AnimatorController/jiuweiyaohu";
        }
        return str;
    }

    public string GetModePath(uint id)
    {
        return "Prefabs/gailun";
    }

    public override void Init()
    {
    }

    public void RefreshNPC(MapNpcData data)
    {
        this.mapNpcData = data;
    }

    public void Update()
    {
        base.Moving();
    }
}

