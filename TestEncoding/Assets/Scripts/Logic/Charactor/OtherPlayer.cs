using NGUI;
using UnityEngine;
using System.Collections;
using Graphics;
using System.Collections.Generic;
using System;
using msg;
using Net;
using Algorithms;

public class OtherPlayer : CharactorBase 
{
    
    //人物地图九屏总数据
    public MapUserData mapUserData;

   

    public override void Init()
    {
        
    }


	/// <summary>
	/// 取得CharID
	/// </summary>
	/// <returns></returns>
    public ulong GetCharID()
    {
        return mapUserData.charid;
    }
    /// <summary>
    /// 取得职业
    /// </summary>
    /// <returns></returns>
    public uint GetOccupation()
    {
        return mapUserData.mapshow.occupation;
    }
    /// <summary>
    /// 根据职业取得模型路径，先写死后面可配
    /// </summary>
    /// <param name="occupation"></param>
    /// <returns></returns>
    public string GetModePath(uint occupation)
    {
        if (occupation == (uint)Occupation.Occu_Dema)
        {
            return "Prefabs/gailun";
        }
        else if (occupation == (uint)Occupation.Occu_Yaohu)
        {
            return "Prefabs/jiuweiyaohu";
        }

        return "Prefabs/jiuweiyaohu";
    }
    /// <summary>
    /// 根据职业取得动画控制器路径，先写死后面可配
    /// </summary>
    /// <param name="occupation"></param>
    /// <returns></returns>
    public string GetAnimatorControllerPath(uint occupation)
    {
        if (occupation == (uint)Occupation.Occu_Dema)
        {
            return "AnimatorController/gailun";
        }
        else if (occupation == (uint)Occupation.Occu_Yaohu)
        {
            return "AnimatorController/jiuweiyaohu";
        }

        return "AnimatorController/jiuweiyaohu";
    }
	

    public void RefreshPlayer(MapUserData data)
    {
        mapUserData = data;
    }

    public void CreatPlayer(MapUserData data)
    {
        Debug.Log("CreatPlayer" + "...." + data.mapshow.occupation);

        Scheduler.Instance.AddUpdator(Update);      //自定义添加一个Update，销毁玩家要Remove掉
        RetMoveDataQueue = new Queue<MoveData>();
        AStarPathDataQueue = new Queue<MoveData>();
        AStarPath = new List<PathFinderNode>();
        this.mapUserData = data;

        CurrentPosition2D = new Vector2(data.mapdata.x, data.mapdata.y);

		string modePath = GetModePath(GetOccupation());
        ModelObj = GameObject.Instantiate(Resources.Load<GameObject>(modePath)) as GameObject;
        
		//模型根节点，现在模型实例化出来直接挂根节点，后面可能调整
        //this.UnityGameObject = ModelObj;
        animator = ModelObj.GetComponent<Animator>();
		string anictrPath = GetAnimatorControllerPath(GetOccupation());
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

        ///////////////////////////////////////////////////
        // avatar
        //EAvatar ava = new EAvatar();
        //ava.tempbind = this.UnityGameObject.transform;
        //this.avatar = ava;
		
        ModelObj.transform.position = GraphUtils.GetWorldPosByServerPos(CurrentPosition2D);
        Vector3 bornpos = GraphUtils.GetWorldPosByServerPos(CurrentPosition2D);
        bornpos.y = MapHightDataHolder.GetMapHeight(bornpos.x, bornpos.z);
        ModelObj.transform.position = bornpos;

        TargetPos = bornpos;

        ModelObj.transform.rotation = GetDirByServerDir((int)data.mapdata.dir);

        moveSpeed = 0.5f / (mapUserData.mapdata.movespeed/1000f);
        if (GameManager.Instance.MainPlayer == this)
        {
            Camera.main.GetComponent<CameraFollowTarget>().Target = ModelObj.transform;
            //this.UnityGameObject.tag = "MainPlayer";
            Debug.Log("Creat main player");

            NGUIManager.Instance.AddByName<NGUI_Blood>(NGUI_UI.NGUI_Blood, NGUIShowType.MULTI, delegate( NGUI_Blood script )
            {
                this.hpBar = script;
                script.Init();

                script.MaxBlood = mapUserData.mapdata.maxhp;
                script.CurrentBlood = mapUserData.mapdata.hp;
                script.MaxSprite = 0;
                script.CurrentSprite = 0;
                script.Playername = mapUserData.name;
                script.Playerlevel = (int)mapUserData.mapdata.level;
                UIFollowTarget followTarget = script.gameObject.AddComponent<UIFollowTarget>();
                followTarget.target = ModelObj.transform;
                followTarget.offY = 2.3f;
                script.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //这里要做一个素材规范。
                script.BloodSlider.backgroundWidget.GetComponent<UISprite>().spriteName = "st0001";
                script.BloodSlider.foregroundWidget.GetComponent<UISprite>().spriteName = "st0004";
            });


        }
        else
        {
            //this.UnityGameObject.tag = "OtherPlayer";

            NGUIManager.Instance.AddByName<NGUI_Blood>(NGUI_UI.NGUI_Blood, NGUIShowType.MULTI, delegate( NGUI_Blood script )
            {
                this.hpBar = script;
                script.Init();
                script.MaxBlood = mapUserData.mapdata.maxhp;
                script.CurrentBlood = mapUserData.mapdata.hp;
                script.MaxSprite = 1;
                script.CurrentSprite = 0;
                script.Playername = mapUserData.name;
                script.Playerlevel = (int)mapUserData.mapdata.level;
                UIFollowTarget followTarget = script.gameObject.AddComponent<UIFollowTarget>();
                followTarget.target = ModelObj.transform;
                followTarget.offY = 2.3f;
                script.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //这里要做一个素材规范。
                script.BloodSlider.backgroundWidget.GetComponent<UISprite>().spriteName = "st0002";
                script.BloodSlider.foregroundWidget.GetComponent<UISprite>().spriteName = "st0006";
            });
        }

        //this.UnityGameObject.name = data.charid.ToString();

        //*****临时给角色技能
        //Util.Log("角色上线客户端临时给技能！");
        //LSingleton<SkillManager>.Instance.OnSkillInfo(this);
    }


    public virtual void Update()
    {
        Moving();
    }


    public void ReconnectResetPlayer()
    {
        DelayRelive();
        beginTurn = false;
        isTweening = false;
        beginMove = false;
    }

   

    public override void Die()
    {
        base.Die();
        StopMoving();
        if (GameManager.Instance.MainPlayer.SelectTarget == this)
        {
            GameManager.Instance.MainPlayer.SelectTarget = null;
        }
        Debug.Log("Other Player Die!");
    }

    public override void DelayRelive()
    {
        base.DelayRelive();
        hpBar.CurrentBlood = mapUserData.mapdata.hp;
    }

    /// <summary>
    /// 销毁玩家（不在九屏范围内）
    /// </summary>
    public override void DestroyThis()
    {
        Scheduler.Instance.RemoveUpdator(Update);      //销毁玩家RemoveUpdate
        Debug.LogError("Destroy Player..." + mapUserData.charid + "..." + mapUserData.name);
        base.DestroyThis();
        RetMoveDataQueue.Clear();
        isTweening = false;
        isPathfinding = false;

        GameObject.Destroy(ModelObj);
        mapUserData = null;

        if (GameManager.Instance.MainPlayer.SelectTarget == this)
        {
            GameManager.Instance.MainPlayer.SelectTarget = null;
        }
        //删除当前玩家的血量
        NGUIManager.Instance.DeleteByNameAndScript(NGUI_UI.NGUI_Blood, hpBar);
    }
}