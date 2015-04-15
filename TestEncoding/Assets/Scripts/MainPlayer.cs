using Algorithms;
using Graphics;
using msg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MainPlayer : OtherPlayer
{
    public bool IsSendSingleMove;
    public uint m_dwTmpID;
    public string m_name = string.Empty;
    private OtherPlayer m_selectTarget;
    public AttributeData MainAttributeData;
    public CharacterBaseData MainBaseData;
    public PathFinder mPathFinder;
    private List<GameObject> PathObjs;
    public Queue<MoveData> ReqMoveDataQueue;
    private GameObject targetPosObj;
    public t_skill_lv_config trackSkillLv;
    public t_skill_stage_config trackSkillStage;
    public bool trackState;

    private void _Track()
    {
    }

    public void BeginFindPath(Vector3 tar)
    {
        Vector2DForInteger serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(tar);
        this.BeginFindPath(new Point((int) this.CurrentPosition2D.x, (int) this.CurrentPosition2D.y), new Point(serverPosByWorldPos.x, serverPosByWorldPos.y));
    }

    public void BeginFindPath(Point scrp, Point dstp)
    {
        if ((scrp.X == dstp.X) && (scrp.Y == dstp.Y))
        {
            UnityEngine.Debug.Log("Destpoint equal to Scrpoint");
            this.EndPathFind();
        }
        else
        {
            Point end = dstp;
            if (GraphUtils.IsContainsFlag(CellInfos.MapInfos[dstp.X, dstp.Y], map.TileFlag.TILE_BLOCK_NORMAL))
            {
                UnityEngine.Debug.Log("Destpoint is BLOCK");
                Point bestAccessiblePoint = this.mPathFinder.GetBestAccessiblePoint(scrp, dstp);
                if (bestAccessiblePoint == null)
                {
                    UnityEngine.Debug.Log("Not Found BestAccessiblePoint");
                    this.EndPathFind();
                    return;
                }
                end = bestAccessiblePoint;
            }
            if ((scrp.X == end.X) && (scrp.Y == end.Y))
            {
                UnityEngine.Debug.Log("Destpoint equal to Scrpoint");
                this.EndPathFind();
            }
            else
            {
                if (base.isPathfinding)
                {
                    base.isPathfinding = false;
                    base.AStarPathDataQueue.Clear();
                    foreach (GameObject obj2 in this.PathObjs)
                    {
                        UnityEngine.Object.Destroy(obj2);
                    }
                }
                base.isPathfinding = true;
                base.AStarPath = this.mPathFinder.FindPath(scrp, end);
                if (base.AStarPath == null)
                {
                    UnityEngine.Debug.LogError("Not find path");
                    this.EndPathFind();
                }
                else
                {
                    for (int i = 1; i < base.AStarPath.Count; i++)
                    {
                        MoveData item = new MoveData();
                        PathFinderNode node = base.AStarPath[i - 1];
                        PathFinderNode node2 = base.AStarPath[i - 1];
                        PathFinderNode node3 = base.AStarPath[i];
                        PathFinderNode node4 = base.AStarPath[i];
                        item.dir = this.GetDir(node.X, node2.Y, node3.X, node4.Y);
                        PathFinderNode node5 = base.AStarPath[i];
                        item.x = (uint) node5.X;
                        PathFinderNode node6 = base.AStarPath[i];
                        item.y = (uint) node6.Y;
                        base.AStarPathDataQueue.Enqueue(item);
                    }
                    if (!base.beginMove)
                    {
                        MoveData data = base.AStarPathDataQueue.Dequeue();
                        UnityEngine.Debug.Log(string.Concat(new object[] { "path find to ", data.x, ",", data.y, "  dir:", data.dir }));
                        List<MoveData> list = new List<MoveData> {
                            data
                        };
                        LSingleton<GameManager>.Instance.PNetWork.ReqMove(list, false);
                        base.MoveTo(data);
                    }
                }
            }
        }
    }

    public void BeginFindPath(int srcx, int srcy, int dstx, int dsty)
    {
        this.BeginFindPath(new Point(srcx, srcy), new Point(dstx, dsty));
    }

    private bool CastTrackSkill()
    {
        return false;
    }

    public void CheckMove(Vector2 serverpos)
    {
        if ((Mathf.Abs((float) (serverpos.x - this.CurrentPosition2D.x)) > 2f) || (Mathf.Abs((float) (serverpos.y - this.CurrentPosition2D.y)) > 2f))
        {
            UnityEngine.Debug.Log(string.Concat(new object[] { "Force Set position to X:", serverpos.x, " Y:", serverpos.y }));
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(serverpos);
            float mapHeight = MapHightDataHolder.GetMapHeight(worldPosByServerPos.x, worldPosByServerPos.z);
            base.ModelObj.transform.position = new Vector3(worldPosByServerPos.x, mapHeight, worldPosByServerPos.z);
            base.CurrentPosition2D = serverpos;
            base.TargetPos = base.ModelObj.transform.position;
            if (base.isPathfinding)
            {
                this.EndPathFind();
            }
        }
    }

    public bool ClickedOnActor()
    {
        return false;
    }

    public override void Die()
    {
        base.Die();
        this.EndPathFind();
        UnityEngine.Debug.Log("Main Player Die!");
    }

    public void EndPathFind()
    {
        base.isPathfinding = false;
        base.AStarPathDataQueue.Clear();
        foreach (GameObject obj2 in this.PathObjs)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        if (this.targetPosObj != null)
        {
            UnityEngine.Object.Destroy(this.targetPosObj);
            this.targetPosObj = null;
        }
    }

    public void EndTrackTarget()
    {
    }

    public Vector3 GetClickPoint(float height)
    {
        float num;
        Vector3 zero = Vector3.zero;
        Plane plane = new Plane(Vector3.up, new Vector3(0f, height, 0f));
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out num))
        {
            zero = ray.GetPoint(num);
            if (this.SelectTarget != null)
            {
                this.EndTrackTarget();
            }
        }
        GameObject targetPosObj = this.targetPosObj;
        this.targetPosObj = null;
        if (targetPosObj != null)
        {
            UnityEngine.Object.Destroy(targetPosObj);
        }
        this.targetPosObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        this.targetPosObj.transform.position = zero;
        this.targetPosObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        this.targetPosObj.renderer.material.color = UnityEngine.Color.red;
        return zero;
    }

    public uint GetDir(int resx, int resy, int dstx, int dsty)
    {
        uint num = 8;
        int num2 = dstx - resx;
        int num3 = dsty - resy;
        switch (num2)
        {
            case 0:
                if (num3 == 1)
                {
                    return 4;
                }
                if (num3 != -1)
                {
                    return num;
                }
                return 0;

            case -1:
                switch (num3)
                {
                    case 1:
                        return 5;

                    case -1:
                        return 7;
                }
                if (num3 != 0)
                {
                    return num;
                }
                return 6;
        }
        if (num2 == 1)
        {
            switch (num3)
            {
                case 1:
                    return 3;

                case -1:
                    return 1;

                case 0:
                    return 2;
            }
        }
        return num;
    }

    public OtherPlayer GetPlayerFromTransform(Transform trans)
    {
        OtherPlayer playerByID = null;
        uint result = 0;
        uint.TryParse(trans.gameObject.name, out result);
        if (result > 0)
        {
            playerByID = LSingleton<GameManager>.Instance.mEntitiesManager.GetPlayerByID((ulong) result);
        }
        if ((playerByID != null) && (playerByID != LSingleton<GameManager>.Instance.MainPlayer))
        {
            return playerByID;
        }
        LUtil.Log("碰撞体无效！！", LUtil.LogType.Normal, false);
        return null;
    }

    public void Goto(Vector3 pos)
    {
        this.BeginFindPath(pos);
    }

    public void InitMainPlayer(CharacterMainData data)
    {
        this.ReqMoveDataQueue = new Queue<MoveData>();
        this.ReqMoveDataQueue.Clear();
        this.MainBaseData = data.basedata;
        this.MainAttributeData = data.attridata;
        this.PathObjs = new List<GameObject>();
        this.mPathFinder = new PathFinder(CellInfos.MapInfos);
    }

    public void MoveTo(MoveDirction dir)
    {
        if (base.isPathfinding)
        {
            this.EndPathFind();
        }
        if (!base.isTweening)
        {
            if (dir == MoveDirction.USER_DIR_WRONG)
            {
                UnityEngine.Debug.LogError("方向错误");
            }
            else
            {
                Vector2 zero = Vector2.zero;
                switch (dir)
                {
                    case MoveDirction.USER_DIR_UP:
                        zero = new Vector2(0f, -1f);
                        break;

                    case MoveDirction.USER_DIR_UPRIGHT:
                        zero = new Vector2(1f, -1f);
                        break;

                    case MoveDirction.USER_DIR_RIGHT:
                        zero = new Vector2(1f, 0f);
                        break;

                    case MoveDirction.USER_DIR_RIGHTDOWN:
                        zero = new Vector2(1f, 1f);
                        break;

                    case MoveDirction.USER_DIR_DOWN:
                        zero = new Vector2(0f, 1f);
                        break;

                    case MoveDirction.USER_DIR_DOWNLEFT:
                        zero = new Vector2(-1f, 1f);
                        break;

                    case MoveDirction.USER_DIR_LEFT:
                        zero = new Vector2(-1f, 0f);
                        break;

                    case MoveDirction.USER_DIR_LEFTUP:
                        zero = new Vector2(-1f, -1f);
                        break;

                    default:
                        zero = Vector2.zero;
                        break;
                }
                Vector2 vector2 = base.CurrentPosition2D + zero;
                if ((((vector2.x >= 0f) && (vector2.y >= 0f)) && ((vector2.x < LSingleton<CurrentMapAccesser>.Instance.CellNumX) && (vector2.y < LSingleton<CurrentMapAccesser>.Instance.CellNumY))) && !GraphUtils.IsContainsFlag((uint) vector2.x, (uint) vector2.y, map.TileFlag.TILE_BLOCK_NORMAL))
                {
                    MoveData item = new MoveData {
                        dir = (uint) dir,
                        x = (uint) vector2.x,
                        y = (uint) vector2.y
                    };
                    this.ReqMoveDataQueue.Enqueue(item);
                    if (this.ReqMoveDataQueue.Count == 1)
                    {
                        Scheduler.Instance.AddTimer(0.2f, false, new Scheduler.OnScheduler(this.OnTimerSendSingleMove));
                    }
                    base.MoveTo(item);
                }
            }
        }
    }

    private void OnTimerSendSingleMove()
    {
        if (this.ReqMoveDataQueue.Count == 1)
        {
            this.IsSendSingleMove = true;
        }
        else
        {
            this.IsSendSingleMove = false;
        }
    }

    public void ResetTrackTargetSkill()
    {
    }

    private void SendMoveReqManager()
    {
        if ((this.ReqMoveDataQueue.Count != 0) && (this.ReqMoveDataQueue.Count == 1))
        {
            List<MoveData> list = new List<MoveData>();
            MoveData item = this.ReqMoveDataQueue.Dequeue();
            list.Add(item);
            LSingleton<GameManager>.Instance.PNetWork.ReqMove(list, false);
        }
    }

    public bool StartTrackTarget(t_skill_lv_config skillLv = null)
    {
        return true;
    }

    [DebuggerHidden]
    public IEnumerator Track()
    {
        while (this.trackState)
        {
            _Track();
            yield return new WaitForSeconds(LUtil.InvokeGap);
        }
    }

    public override void Update()
    {
        base.Update();
        this.SendMoveReqManager();
        if ((((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !NGUITools.IsMouseOverUI) && (GUIUtility.hotControl == 0)) && !this.ClickedOnActor())
        {
            this.BeginFindPath(this.GetClickPoint(-1f));
        }
    }

    public OtherPlayer SelectTarget
    {
        get
        {
            return this.m_selectTarget;
        }
        set
        {
            this.m_selectTarget = value;
        }
    }

}

