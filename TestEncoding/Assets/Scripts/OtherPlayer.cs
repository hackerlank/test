using msg;
using NGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Algorithms;

public class OtherPlayer : CharactorBase
{
    public MapUserData mapUserData;

    public void CreatPlayer(MapUserData data)
    {
        Debug.Log("CreatPlayer...." + data.mapshow.occupation);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        base.RetMoveDataQueue = new Queue<MoveData>();
        base.AStarPathDataQueue = new Queue<MoveData>();
        base.AStarPath = new List<PathFinderNode>();
        this.mapUserData = data;
        base.CurrentPosition2D = new Vector2((float) data.mapdata.x, (float) data.mapdata.y);
        string modePath = this.GetModePath(this.GetOccupation());
        base.ModelObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(modePath)) as GameObject;
        base.animator = base.ModelObj.GetComponent<Animator>();
        RuntimeAnimatorController controller = Resources.Load<RuntimeAnimatorController>(this.GetAnimatorControllerPath(this.GetOccupation()));
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
        base.ModelObj.transform.rotation = base.GetDirByServerDir((int) data.mapdata.dir);
        base.moveSpeed = 0.5f / (((float) this.mapUserData.mapdata.movespeed) / 1000f);
        if (LSingleton<GameManager>.Instance.MainPlayer == this)
        {
            Camera.main.GetComponent<CameraFollowTarget>().Target = base.ModelObj.transform;
            Debug.Log("Creat main player");
            //NGUIManager.Instance.AddByName<NGUI_Blood>("NGUI_Blood", NGUIShowType.MULTI, delegate (NGUI_Blood script) {
            //    base.hpBar = script;
            //    script.Init();
            //    script.MaxBlood = this.mapUserData.mapdata.maxhp;
            //    script.CurrentBlood = this.mapUserData.mapdata.hp;
            //    script.MaxSprite = 0f;
            //    script.CurrentSprite = 0f;
            //    script.Playername = this.mapUserData.name;
            //    script.Playerlevel = (int) this.mapUserData.mapdata.level;
            //    UIFollowTarget target = script.gameObject.AddComponent<UIFollowTarget>();
            //    target.target = base.ModelObj.transform;
            //    target.offY = 2.3f;
            //    script.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            //    script.BloodSlider.backgroundWidget.GetComponent<UISprite>().spriteName = "st0001";
            //    script.BloodSlider.foregroundWidget.GetComponent<UISprite>().spriteName = "st0004";
            //});
        }
        else
        {
            //NGUIManager.Instance.AddByName<NGUI_Blood>("NGUI_Blood", NGUIShowType.MULTI, delegate (NGUI_Blood script) {
            //    base.hpBar = script;
            //    script.Init();
            //    script.MaxBlood = this.mapUserData.mapdata.maxhp;
            //    script.CurrentBlood = this.mapUserData.mapdata.hp;
            //    script.MaxSprite = 1f;
            //    script.CurrentSprite = 0f;
            //    script.Playername = this.mapUserData.name;
            //    script.Playerlevel = (int) this.mapUserData.mapdata.level;
            //    UIFollowTarget target = script.gameObject.AddComponent<UIFollowTarget>();
            //    target.target = base.ModelObj.transform;
            //    target.offY = 2.3f;
            //    script.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            //    script.BloodSlider.backgroundWidget.GetComponent<UISprite>().spriteName = "st0002";
            //    script.BloodSlider.foregroundWidget.GetComponent<UISprite>().spriteName = "st0006";
            //});
        }
    }

    public override void DelayRelive()
    {
        base.DelayRelive();
        base.hpBar.CurrentBlood = this.mapUserData.mapdata.hp;
    }

    public override void DestroyThis()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        Debug.LogError(string.Concat(new object[] { "Destroy Player...", this.mapUserData.charid, "...", this.mapUserData.name }));
        base.DestroyThis();
        base.RetMoveDataQueue.Clear();
        base.isTweening = false;
        base.isPathfinding = false;
        UnityEngine.Object.Destroy(base.ModelObj);
        this.mapUserData = null;
        if (LSingleton<GameManager>.Instance.MainPlayer.SelectTarget == this)
        {
            LSingleton<GameManager>.Instance.MainPlayer.SelectTarget = null;
        }
        //NGUIManager.Instance.DeleteByNameAndScript("NGUI_Blood", base.hpBar);
    }

    public override void Die()
    {
        base.Die();
        base.StopMoving();
        if (LSingleton<GameManager>.Instance.MainPlayer.SelectTarget == this)
        {
            LSingleton<GameManager>.Instance.MainPlayer.SelectTarget = null;
        }
        Debug.Log("Other Player Die!");
    }

    public string GetAnimatorControllerPath(uint occupation)
    {
        if (occupation == 1)
        {
            return "AnimatorController/gailun";
        }
        if (occupation == 2)
        {
            return "AnimatorController/jiuweiyaohu";
        }
        return "AnimatorController/jiuweiyaohu";
    }

    public ulong GetCharID()
    {
        return this.mapUserData.charid;
    }

    public string GetModePath(uint occupation)
    {
        if (occupation == 1)
        {
            return "Prefabs/gailun";
        }
        if (occupation == 2)
        {
            return "Prefabs/jiuweiyaohu";
        }
        return "Prefabs/jiuweiyaohu";
    }

    public uint GetOccupation()
    {
        return this.mapUserData.mapshow.occupation;
    }

    public override void Init()
    {
    }

    public void ReconnectResetPlayer()
    {
        this.DelayRelive();
        base.beginTurn = false;
        base.isTweening = false;
        base.beginMove = false;
    }

    public void RefreshPlayer(MapUserData data)
    {
        this.mapUserData = data;
    }

    public virtual void Update()
    {
        base.Moving();
    }
}

