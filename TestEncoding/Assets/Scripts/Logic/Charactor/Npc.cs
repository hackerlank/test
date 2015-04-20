/*
***********************************************************************************************************
* CLR version ：$clrversion$
* Machine name ：$machinename$
* Creation time ：#time#
* Author ：hym
* Version number : 1.0
***********************************************************************************************************
*/

using msg;
using UnityEngine;
using System.Collections;

public class Npc : CharactorBase 
{
    //NPC地图九屏总数据
    public MapNpcData mapNpcData;

    public override void Init()
    {
        
    }

    /// <summary>
    /// 得到NPC模型预制路径，先写死
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string GetModePath(uint id)
    {
        //string str = "";
        //t_npc_config config = t_npc_config.GetConfig(id);

        //if (config.m_id == 2000)
        //{
        //    str = "Prefabs/gailun";
        //}
        //else if (config.m_id == 2001)
        //{
        //    str = "Prefabs/jiuweiyaohu";
        //}

        return "Prefabs/gailun"; 
    }
    /// <summary>
    /// 根据id取得动画控制器路径，先写死后面可配
    /// </summary>
    /// <param name="occupation"></param>
    /// <returns></returns>
    public string GetAnimatorControllerPath(uint id)
    {
        string str = "";
        t_npc_config config = t_npc_config.GetConfig(id);

        if (config.m_id == 2000)
        {
            str = "AnimatorController/gailun";
        }
        else if (config.m_id == 2001)
        {
            str = "AnimatorController/jiuweiyaohu";
        }

        return str;
    }
	

    public void CreatNpc(MapNpcData data)
    {
        Debug.Log("CreatNpc" + "...." + data.name + "..." + data.tempid + "...X:" + data.x + " Y:" + data.y);

        
        
        this.mapNpcData = data;

        CurrentPosition2D = new Vector2(data.x, data.y);

        string modePath = GetModePath((uint)data.baseid);
        ModelObj = GameObject.Instantiate(Resources.Load<GameObject>(modePath)) as GameObject;

        //模型根节点，现在模型实例化出来直接挂根节点，后面可能调整
        //this.UnityGameObject = ModelObj;
        animator = ModelObj.GetComponent<Animator>();
        string anictrPath = GetAnimatorControllerPath((uint)data.baseid);
        RuntimeAnimatorController rac = Resources.Load<RuntimeAnimatorController>(anictrPath);
        animator.runtimeAnimatorController = rac;
        animator.applyRootMotion = true;
        if (animator.applyRootMotion)
        {
            CapsuleCollider cc = ModelObj.AddComponent<CapsuleCollider>();
            cc.isTrigger = false;
            cc.material = null;
            cc.center = new Vector3(0, 1, 0);
            cc.radius = .6f;
            cc.height = 2;
            cc.direction = 1;
        }

        //UActorCtrl ctrl = ModelObj.AddComponent<UActorCtrl>();
        //this.ctrl = ctrl;
        //ctrl.Init(this, false, 0f);

        /////////////////////////////////////////////////////
        //// avatar
        //EAvatar ava = new EAvatar();
        //ava.tempbind = this.UnityGameObject.transform;
        //this.avatar = ava;

        ModelObj.transform.position = GraphUtils.GetWorldPosByServerPos(CurrentPosition2D);
        Vector3 bornpos = GraphUtils.GetWorldPosByServerPos(CurrentPosition2D);
        bornpos.y = MapHightDataHolder.GetMapHeight(bornpos.x, bornpos.z);
        ModelObj.transform.position = bornpos;

        TargetPos = bornpos;

        ModelObj.transform.rotation = GetDirByServerDir((int)data.dir);

        ModelObj.name = "NPC" + data.name + data.tempid;

        moveSpeed = 0.5f / (mapNpcData.movespeed / 1000f);

        GameManager.Instance.FightUI.AddNpcName((uint)data.tempid);
        
        Scheduler.Instance.AddUpdator(Update);      //自定义添加一个Update，销毁玩家要Remove掉
    }

    public void Update()
    {
        Moving();
    }


    public override void Die()
    {
        base.Die();
    }

    public override void DelayRelive()
    {
        
    }

    /// <summary>
    /// 刷新NPC
    /// </summary>
    /// <param name="data"></param>
    public void RefreshNPC(MapNpcData data)
    {
        mapNpcData = data;
    }

    /// <summary>
    /// 销毁玩家（不在九屏范围内）
    /// </summary>
    public override void DestroyThis()
    {
        base.DestroyThis();
        isTweening = false;
        isPathfinding = false;
        GameManager.Instance.FightUI.RemoveNpcName((uint)this.mapNpcData.tempid);
        GameObject.Destroy(ModelObj);
        mapNpcData = null;
    }
    
}
