using UnityEngine;
using System.Collections;
using msg;
using System.Collections.Generic;
using Algorithms;
using Graphics;
using System;
using NGUI;
public class MainPlayer : OtherPlayer
{
    //角色选择信息
    public stUserInfoUserCmd selectUserInfo = null;
    //主角基本信息
    public stSendUserInfoSC baseUserInfo = null;

    //包裹道具信息
    public Dictionary<UInt32, StObject> mItems = new Dictionary<UInt32, StObject>();

    //点卡信息
    public stReturnRequestPoint pointCardInfo = null;

    //VIP信息
    public stSendVipInfo_SC vipInfo = null;

    //主用户数据
    public  stMainUserDataUserCmd mainUserData = null;

    //非绑银子
    public uint money = 0;

    public uint m_dwTmpID = 0; //临时ID
    public string m_name = "";
    public CharacterBaseData MainBaseData;  //基础属性
    public AttributeData MainAttributeData; //攻防数据属性

    public Queue<MoveData> ReqMoveDataQueue;

    public bool IsSendSingleMove = false;

    private OtherPlayer m_selectTarget; //目标

    public PathFinder mPathFinder;          //寻路

    List<GameObject> PathObjs;

    GameObject targetPosObj;

    //锁定目标用的技能
    public t_skill_lv_config trackSkillLv = null;
    //技能阶段
    public t_skill_stage_config trackSkillStage = null;
    //是否处在追踪模式
    public bool trackState = false;

    public void UpdateItem(StObject obj)
    {
        //特殊道具不显示
        if (IsSpeicalItem(obj))
            return;

        //只显示主包裹和副包裹的道具
        if (!((obj.dwLocation == (uint)enumCellType.OBJECTCELLTYPE_COMMON) || (obj.dwLocation == (uint)enumCellType.OBJECTCELLTYPE_PACKAGE)))
            return;

        t_object_config config = null;
        if (mItems.ContainsKey(obj.qwThisID))
        {
            //存在就替换
            mItems[obj.qwThisID] = obj;
        }
        else
        {
            //找不到添加
            mItems.Add(obj.qwThisID, obj);
        }

        //更新
        GameObject dlgRootObj = NGUIManager.Instance.GetDlgRootByName(NGUI_UI.NGUI_Bag);
        if (dlgRootObj != null)
        {
            NGUI_Bag dlg = dlgRootObj.GetComponent<NGUI_Bag>();
            if (dlg != null)
                dlg.UpdateItem();
        }
      
    }

    public StObject GetObjectByThisID(uint dwThisID)
    {
        if (mItems.ContainsKey(dwThisID))
            return mItems[dwThisID];
        else
            return null;
    }

    public bool IsSpeicalItem(StObject obj)
    {
        //银子
        if (obj.dwObjectID == 665)
        {
            money = obj.dwNum;
            return true;
        }

        return false;
    }

    /// <summary>
    /// 走向目标点
    /// </summary>
    /// <param name="pos"></param>
    public void Goto(Vector3 pos)
    {
        BeginFindPath(pos);
    }

    /// <summary>
    /// 主角当前选择的目标
    /// </summary>
    public OtherPlayer SelectTarget
    {
        get { return m_selectTarget; }
        set { m_selectTarget = value; }
    }

    /// <summary>
    /// 从transform中获取玩家
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public OtherPlayer GetPlayerFromTransform(Transform trans)
    {
        OtherPlayer  player = null;
        uint num = 0;
        uint.TryParse(trans.gameObject.name, out num);
        if (num > 0)
        player= GameManager.Instance.mEntitiesManager.GetPlayerByID(num);
        if ((player != null) && (player != GameManager.Instance.MainPlayer))
        {
            return player;
        }
        else
        {
            Util.Log("碰撞体无效！！");
            return null;
        }

    }

    //开始追踪选定目标
    public bool StartTrackTarget(t_skill_lv_config skillLv = null)
    {
        //if (skillLv != null)
        //    trackSkillLv = skillLv;
        //else
        //    trackSkillLv = LSingleton<SkillManager>.Instance.GetTrackSkillLv();

        //if ((trackSkillLv != null) && !LSingleton<SkillManager>.Instance.IsOutRange(this, SelectTarget, trackSkillLv.aiCastDist))
        //{
        //    CastTrackSkill();
        //}

        ////if (trackSkillLv == null)
        ////{
        ////    trackSkillLv = LSingleton<SkillManager>.Instance.GetTrackSkillLv();
        ////}
        ////重新用普攻技能追踪
        //if (trackSkillLv != null)
        //{
        //    trackState = true;
        //    ctrl.StartCoroutine(this.Track());
        //    Util.Log("@@开始追踪目标:!");
        //}

        return true;
    }
    public void EndTrackTarget()
    {
        //trackSkillLv = null;
        //trackSkillStage = null;
        //trackState = false;
        //ctrl.StopCoroutine(this.Track());
        //Util.Log("@@结束追踪目标:!");
        //EndPathFind();
    }

    /// <summary>
    /// 重新设置追踪目标使用的技能
    /// </summary>
    public void ResetTrackTargetSkill()
    {
        //if (trackState)
        //{
        //    Util.Log("@@重置追踪目标用的技能！");
        //    t_skill_lv_config aiSkillLv = LSingleton<SkillManager>.Instance.GetTrackSkillLv();
        //    trackSkillLv = aiSkillLv;
        //}
    }

    public IEnumerator Track()
    {
        while (this.trackState)
        {
            _Track();
            yield return new WaitForSeconds(Util.InvokeGap*20);
        }
    }

    private void _Track()
    {
        //丢失锁定目标，停止追踪
        //if (SelectTarget == null)
        //{
        //    EndTrackTarget();
        //    return;
        //}

        ////目标死亡，停止追踪
        //if (SelectTarget.IsDie())
        //{
        //    EndTrackTarget();
        //    return;
        //}

        ////主角死亡，停止追踪
        //if (IsDie())
        //{
        //    EndTrackTarget();
        //    return;
        //}

        //if ((trackSkillLv != null) && (SelectTarget != null))
        //{
        //    if (LSingleton<SkillManager>.Instance.IsOutRange(this, SelectTarget, trackSkillLv.aiCastDist))
        //    {
        //        //移动
        //        Goto(SelectTarget.xform.position);
        //    }
        //    else
        //    {
        //        CastTrackSkill();
        //    }
        //}
    }

    /// <summary>
    /// 释放追踪技能
    /// </summary>
    /// <returns></returns>
    private bool CastTrackSkill()
    {
        //Util.Log("@@设置目标转向！");
        //SetRotateTarget(SelectTarget.xform.position);
        ////StopMoving();
        ////释放技能
        //if (trackSkillStage != null)
        //{
        //    if (LSingleton<SkillManager>.Instance.CastStateStageSkill())
        //    {
        //        //EndTrackTarget();
        //        //释放完继续用普攻追踪
        //        trackSkillStage = null;
        //        trackSkillLv = LSingleton<SkillManager>.Instance.GetTrackSkillLv();
        //        return true;
        //    }
        //}
        //else
        //{
        //    if (ctrl.Fire(trackSkillLv.id / 100, trackSkillLv.id % 100, this.xform.position, this.xform.forward, true))
        //    {
        //        //普攻连续攻击
        //        //EndTrackTarget();
        //        return true;
        //    }
        //}

        return false;
    }


    /// <summary>
    /// 填充玩家数据
    /// </summary>
    public void InitMainPlayer(CharacterMainData data)
    {
        ReqMoveDataQueue = new Queue<MoveData>();
        ReqMoveDataQueue.Clear();
        MainBaseData = data.basedata;
        MainAttributeData = data.attridata;

        PathObjs = new List<GameObject>();

        mPathFinder = new PathFinder(CellInfos.MapInfos);
    }

    public override void Update()
    {
 	    base.Update();
        SendMoveReqManager();

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if(NGUITools.IsMouseOverUI)
            {
                return;
            }
            if(GUIUtility.hotControl != 0)
            {
                return;
            }
            //没点到角色，直接寻路
            if (!ClickedOnActor())
                BeginFindPath(GetClickPoint(-1f));
        }
    }

    /// <summary>
    /// 是否点到游戏中的Actor(角色、NPC)
    /// </summary>
    /// <returns></returns>
    public bool ClickedOnActor()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //float enter;

        //RaycastHit hit;
        //if (Physics.Raycast(ray, out hit))
        //{
        //    Vector3 pos = hit.point;
        //    SelectTarget = GetPlayerFromTransform(hit.transform);
        //    if (SelectTarget != null && !SelectTarget.IsDie())
        //    {
        //        Util.Log("@@选中角色:" + SelectTarget.GetCharID());
        //        //追踪
        //        if (LSingleton<SkillManager>.Instance.IsCanCast(this, SelectTarget))
        //        {
        //            StartTrackTarget();
        //            //if (!StartTrackTarget())
        //            //{
        //            //    EndPathFind();
        //            //}
        //        }

        //        return true;
        //    }
        //}

         return false;
    }
    public void BeginFindPath(Vector3 tar)
    {
        Vector2DForInteger v2 = GraphUtils.GetServerPosByWorldPos(tar);
        BeginFindPath(new Point((int)CurrentPosition2D.x, (int)CurrentPosition2D.y), new Point(v2.x, v2.y));
    }

    /// <summary>
    /// 开始寻路
    /// </summary>
    /// <param name="srcx"></param>
    /// <param name="srcy"></param>
    /// <param name="dstx"></param>
    /// <param name="dsty"></param>
    public void BeginFindPath(int srcx, int srcy, int dstx, int dsty)
    {
        BeginFindPath(new Point(srcx, srcy), new Point(dstx, dsty));
    }

    public void BeginFindPath(Point scrp, Point dstp)
    {
        if(scrp.X == dstp.X && scrp.Y == dstp.Y)
        {
            Debug.Log("Destpoint equal to Scrpoint");
            EndPathFind();
            return;
        }

        Point dest = dstp;

        if (GraphUtils.IsContainsFlag(CellInfos.MapInfos[dstp.X, dstp.Y], map.TileFlag.TILE_BLOCK_NORMAL))
        {
            Debug.Log("Destpoint is BLOCK");

            Point newdstp = mPathFinder.GetBestAccessiblePoint(scrp, dstp);

            if (newdstp == null)
            {
                Debug.Log("Not Found BestAccessiblePoint");
                EndPathFind();
                return;
            }

            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            //go.transform.position = GraphUtils.GetWorldPosByServerPos((uint)newdstp.X, (uint)newdstp.Y);
            //go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //go.renderer.material.color = UnityEngine.Color.green;

            dest = newdstp;
        }

        if (scrp.X == dest.X && scrp.Y == dest.Y)
        {
            Debug.Log("Destpoint equal to Scrpoint");
            EndPathFind();
            return;
        }

        if (isPathfinding)
        {
            isPathfinding = false;
            AStarPathDataQueue.Clear();

            foreach (GameObject g in PathObjs)
            {
                GameObject.Destroy(g);
            }
        }

        isPathfinding = true;
        AStarPath = mPathFinder.FindPath(scrp, dest);

        if (AStarPath == null)
        {
            Debug.LogError("Not find path");
            EndPathFind();
            return;
        }

        for (int i = 1; i < AStarPath.Count; i ++ )
        {
            MoveData data = new MoveData();

            data.dir = GetDir(AStarPath[i - 1].X, AStarPath[i - 1].Y, AStarPath[i].X, AStarPath[i].Y);
            
            data.x = (uint)AStarPath[i].X;
            data.y = (uint)AStarPath[i].Y;

            AStarPathDataQueue.Enqueue(data);


            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            //go.transform.position = GraphUtils.GetWorldPosByServerPos((uint)AStarPath[i].X, (uint)AStarPath[i].Y); ;
            //go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            //go.renderer.material.color = UnityEngine.Color.red;

            //PathObjs.Add(go);
        }

        
        //寻路完第一步移动
        if (!beginMove)
        {
            MoveData md = AStarPathDataQueue.Dequeue();  //出队发送
            Debug.Log("path find to " + md.x + "," + md.y + "  dir:" + md.dir);
            List<MoveData> mds = new List<MoveData>();
            mds.Add(md);
            GameManager.Instance.PNetWork.ReqMove(mds);
            MoveTo(md);
        }
    }

    /// <summary>
    /// 停止寻路
    /// </summary>
    public void EndPathFind()
    {
        isPathfinding = false;
        AStarPathDataQueue.Clear();

        foreach (GameObject g in PathObjs)
        {
            GameObject.Destroy(g);
        }

        if (targetPosObj != null)
        {
            GameObject go = targetPosObj;
            GameObject.Destroy(go);
            targetPosObj = null;
        }
            
    }

    /// <summary>
    /// 由起点终点获得方向信息
    /// </summary>
    /// <param name="resx"></param>
    /// <param name="resy"></param>
    /// <param name="dstx"></param>
    /// <param name="dsty"></param>
    /// <returns></returns>
    public uint GetDir(int resx,int resy,int dstx,int dsty)
    {
        uint dir = 8;

        int dx = dstx - resx;
        int dy = dsty - resy;

        if (dx == 0)
        {
            if (dy == 1)
            {
                dir = (uint)MoveDirction.USER_DIR_DOWN;
            }
            else if (dy == -1)
            {
                dir = (uint)MoveDirction.USER_DIR_UP;
            }
        }
        else if (dx == -1)
        {
            if (dy == 1)
            {
                dir = (uint)MoveDirction.USER_DIR_DOWNLEFT;
            }
            else if (dy == -1)
            {
                dir = (uint)MoveDirction.USER_DIR_LEFTUP;
            }
            else if (dy == 0)
            {
                dir = (uint)MoveDirction.USER_DIR_LEFT;
            }
        }
        else if (dx == 1)
        {
            if (dy == 1)
            {
                dir = (uint)MoveDirction.USER_DIR_RIGHTDOWN;
            }
            else if (dy == -1)
            {
                dir = (uint)MoveDirction.USER_DIR_UPRIGHT;
            }
            else if (dy == 0)
            {
                dir = (uint)MoveDirction.USER_DIR_RIGHT;
            }
        }
        return dir;
    }


    /// <summary>
    /// 主人物移动
    /// </summary>
    /// <param name="dir"></param>
    public void MoveTo(MoveDirction dir)
    {
        if(isPathfinding)
        {
            EndPathFind();
        }

        if (isTweening)
            return;

        if (dir == MoveDirction.USER_DIR_WRONG)
        {
            Debug.LogError("方向错误");
            return;
        }
        Vector2 movedir = Vector2.zero;
        switch (dir)
        {
            case MoveDirction.USER_DIR_DOWN:
                movedir = new Vector2(0, 1);
                break;
            case MoveDirction.USER_DIR_DOWNLEFT:
                movedir = new Vector2(-1, 1);
                break;
            case MoveDirction.USER_DIR_LEFT:
                movedir = new Vector2(-1, 0);
                break;
            case MoveDirction.USER_DIR_LEFTUP:
                movedir = new Vector2(-1, -1);
                break;
            case MoveDirction.USER_DIR_RIGHT:
                movedir = new Vector2(1, 0);
                break;
            case MoveDirction.USER_DIR_RIGHTDOWN:
                movedir = new Vector2(1, 1);
                break;
            case MoveDirction.USER_DIR_UP:
                movedir = new Vector2(0, -1);
                break;
            case MoveDirction.USER_DIR_UPRIGHT:
                movedir = new Vector2(1, -1);
                break;
            default:
                movedir = Vector2.zero;
                break;
        }

        Vector2 target2d = CurrentPosition2D + movedir;

        //ModelObj.transform.rotation = Quaternion.Euler(GetPlayerDir((int)dir));

        if (target2d.x < 0 || target2d.y < 0 || target2d.x >= CurrentMapAccesser.Instance.CellNumX || target2d.y >= CurrentMapAccesser.Instance.CellNumY)
        {
            //Debug.LogError("已到达地图边界，无法移动");
            return;
        }

        if (GraphUtils.IsContainsFlag((uint)target2d.x, (uint)target2d.y, map.TileFlag.TILE_BLOCK_NORMAL))
        {
            //Debug.LogError("目标位置为阻挡点，无法移动");
            return;
        }

        MoveData data = new MoveData();

        data.dir = (uint)dir;
        data.x = (uint)target2d.x;
        data.y = (uint)target2d.y;

        ReqMoveDataQueue.Enqueue(data);

        if (ReqMoveDataQueue.Count == 1)
        {
            Scheduler.Instance.AddTimer(0.2f, false, OnTimerSendSingleMove);
        }

        //GameManager.Instance.PNetWork.ReqMove((int)dir, target2d, 1);
        //GameManager.Instance.PNetWork.ReqMove((int)dir, target2d, 1,true);

        MoveTo(data);
    }


    /// <summary>
    /// 检查移动是否合法
    /// </summary>
    /// <param name="serverpos"></param>
    public void CheckMove(Vector2 serverpos)
    {
        //如果本地坐标与服务器坐标相差超过一个格子,强制拉回服务器坐标,如果在寻路状态，直接停止寻路
        if (Mathf.Abs(serverpos.x - CurrentPosition2D.x) > 2 || Mathf.Abs(serverpos.y - CurrentPosition2D.y) > 2)
        {
            Debug.Log("Force Set position to X:" + serverpos.x + " Y:" + serverpos.y);
            Vector3 newpos = GraphUtils.GetWorldPosByServerPos(serverpos);
            ModelObj.transform.position = new Vector3(newpos.x, MapHightDataHolder.GetMapHeight(newpos.x, newpos.z), newpos.z);

            CurrentPosition2D = serverpos;

            TargetPos = ModelObj.transform.position;

            if (isPathfinding)
            {
                EndPathFind();
            }
        }
    }

    /// <summary>
    /// 检测是否在持续输入，是否需要发送单个消息
    /// </summary>
    void OnTimerSendSingleMove()
    {
        if (ReqMoveDataQueue.Count == 1)
        {
            IsSendSingleMove = true;
        }
        else
        {
            IsSendSingleMove = false;
        }
    }

    void SendMoveReqManager()
    {
        if (ReqMoveDataQueue.Count == 0)
        {
            return;
        }

        if (ReqMoveDataQueue.Count == 1)
        {
            List<MoveData> mds = new List<MoveData>();
            //出队发送
            MoveData md = ReqMoveDataQueue.Dequeue();
            mds.Add(md);
            GameManager.Instance.PNetWork.ReqMove(mds);
            return;
        }

        /*两步一条消息*/
        //if (ReqMoveDataQueue.Count == 1)
        //{
        //    if (IsSendSingleMove)      //如果没有持续输入，发送当前移动信息
        //    {
        //        List<MoveData> mds = new List<MoveData>();
        //        //出队发送
        //        MoveData md = ReqMoveDataQueue.Dequeue();
        //        mds.Add(md);
        //        Debug.Log(ReqMoveDataQueue.Count);
        //        GameManager.Instance.PNetWork.ReqMove(mds);
        //        return;
        //    }
        //}
        //if(ReqMoveDataQueue.Count >= 2)
        //{
        //    List<MoveData> mds = new List<MoveData>();
        //    //出队发送
        //    MoveData md = ReqMoveDataQueue.Dequeue();
        //    mds.Add(md);
        //    MoveData md1 = ReqMoveDataQueue.Dequeue();
        //    mds.Add(md1);
        //    Debug.Log(ReqMoveDataQueue.Count);
        //    GameManager.Instance.PNetWork.ReqMove(mds);
        //}
    }


    /// <summary>
    /// 获取点击位置
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
    public Vector3 GetClickPoint(float height)
    {
        Vector3 pos = Vector3.zero;

        Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;
        if (plane.Raycast(ray, out enter))
        {
            pos = ray.GetPoint(enter);
            if (SelectTarget != null)
            {
                EndTrackTarget();
            }
        }

        GameObject go = targetPosObj;
        targetPosObj = null;
        if(go != null)
        {
            GameObject.Destroy(go);
        }

        targetPosObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        targetPosObj.transform.position = pos;
        targetPosObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        targetPosObj.renderer.material.color = UnityEngine.Color.red;
        return pos;
    }


    public override void Die()
    {
        base.Die();
        EndPathFind();
        Debug.Log("Main Player Die!");
    }
}
