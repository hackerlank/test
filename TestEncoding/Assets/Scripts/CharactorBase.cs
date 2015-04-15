using msg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using Algorithms;
using Net;

public class CharactorBase
{
    public Animator animator;
    public List<PathFinderNode> AStarPath;
    public Queue<MoveData> AStarPathDataQueue;
    public bool beginMove;
    public bool beginTurn;
    public Vector2 CurrentPosition2D;
    public NGUI_Blood hpBar;
    public bool isMoving;
    public bool isPathfinding;
    public bool isTweening;
    public GameObject ModelObj;
    public Vector3 moveDir = new Vector3();
    public float moveSpeed;
    public Queue<MoveData> RetMoveDataQueue;
    private int rotateDir = 1;
    public float rotateSpeed = 10f;
    public char[] state = new char[6];
    public Vector3 TargetPos = new Vector3();
    private Vector3 TargetRot = new Vector3();

    public virtual void DelayRelive()
    {
    }

    public virtual void DestroyThis()
    {
    }

    public virtual void Die()
    {
    }

    public Quaternion GetDirByServerDir(int dir)
    {
        Quaternion quaternion = new Quaternion();
        Vector3 zero = Vector3.zero;
        switch (dir)
        {
            case 0:
                zero = Vector3.forward;
                break;

            case 1:
            {
                Vector3 vector2 = new Vector3(1f, 0f, 1f);
                zero = vector2.normalized;
                break;
            }
            case 2:
                zero = Vector3.right;
                break;

            case 3:
            {
                Vector3 vector3 = new Vector3(1f, 0f, -1f);
                zero = vector3.normalized;
                break;
            }
            case 4:
                zero = Vector3.back;
                break;

            case 5:
            {
                Vector3 vector4 = new Vector3(-1f, 0f, -1f);
                zero = vector4.normalized;
                break;
            }
            case 6:
                zero = Vector3.left;
                break;

            case 7:
            {
                Vector3 vector5 = new Vector3(-1f, 0f, 1f);
                zero = vector5.normalized;
                break;
            }
        }
        return Quaternion.LookRotation(zero);
    }

    public virtual void Init()
    {
    }

    public bool IsCanMove()
    {
        if (!this.IsInFreeMoveState())
        {
            if (this.animator == null)
            {
                return false;
            }
            AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.a1")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.a2")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.q")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.w")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.e")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.r")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
            if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.HitRevive")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
            {
                return false;
            }
        }
        return true;
    }

    public bool IsCanRotate()
    {
        AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.a1")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
        {
            return false;
        }
        if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.a2")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
        {
            return false;
        }
        if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.q")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
        {
            return false;
        }
        if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.w")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
        {
            return false;
        }
        if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.e")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
        {
            return false;
        }
        if ((currentAnimatorStateInfo.nameHash == Animator.StringToHash("Base Layer.r")) && (currentAnimatorStateInfo.normalizedTime < 0.99))
        {
            return false;
        }
        return true;
    }

    public bool IsInFreeMoveState()
    {
        return LUtil.isset_state(this.state, 3);
    }

    private bool IsInSkillAnim(AnimatorStateInfo info)
    {
        return false;
    }

    public void MoveTo(MoveData data)
    {
        this.MoveTo((int) data.dir, new Vector2((float) data.x, (float) data.y));
    }

    public void MoveTo(int dir, Vector2 destpoint)
    {
        this.isMoving = true;
        Vector2 zero = Vector2.zero;
        switch (dir)
        {
            case 0:
                zero = new Vector2(0f, -1f);
                break;

            case 1:
                zero = new Vector2(1f, -1f);
                break;

            case 2:
                zero = new Vector2(1f, 0f);
                break;

            case 3:
                zero = new Vector2(1f, 1f);
                break;

            case 4:
                zero = new Vector2(0f, 1f);
                break;

            case 5:
                zero = new Vector2(-1f, 1f);
                break;

            case 6:
                zero = new Vector2(-1f, 0f);
                break;

            case 7:
                zero = new Vector2(-1f, -1f);
                break;

            default:
                zero = Vector2.zero;
                break;
        }
        this.TargetPos = GraphUtils.GetWorldPosByServerPos(this.CurrentPosition2D + zero);
        Vector3 vector4 = this.TargetPos - this.ModelObj.transform.position;
        Vector3 normalized = vector4.normalized;
        this.TargetRot = new Vector3(normalized.x, 0f, normalized.z);
        this.moveDir = GraphUtils.GetWorldPosByServerPos(this.CurrentPosition2D + zero) - this.ModelObj.transform.position;
        this.moveDir.y = 0f;
        this.moveDir.Normalize();
        if (Vector3.Cross(this.ModelObj.transform.forward, this.TargetRot).y > 0f)
        {
            this.rotateDir = 1;
        }
        else
        {
            this.rotateDir = -1;
        }
        this.beginTurn = true;
        this.beginMove = true;
        this.isTweening = true;
        this.CurrentPosition2D += zero;
    }

    public void Moving()
    {
        if (this.IsCanMove())
        {
            if (this.beginMove)
            {
                Vector3 vector = this.ModelObj.transform.position + ((Vector3) ((this.moveDir * this.moveSpeed) * Time.smoothDeltaTime));
                Vector3 a = this.TargetPos - this.ModelObj.transform.position;
                a.y = 0f;
                Vector3 vector3 = vector - this.ModelObj.transform.position;
                vector3.y = 0f;
                float num = Vector3.Magnitude(a);
                float num2 = Vector3.Magnitude(vector3);
                Vector3 zero = Vector3.zero;
                if (num2 >= num)
                {
                    zero = this.TargetPos;
                    this.beginMove = false;
                    //SingletonForMono<NetWorkModule>.Instance.StartCoroutine(this.OnTimerToDes());
                    NetWorkModule.Instance.StartCoroutine(OnTimerToDes());
                    this.isTweening = false;
                    if (this.RetMoveDataQueue.Count != 0)
                    {
                        MoveData data = this.RetMoveDataQueue.Dequeue();
                        this.MoveTo(data);
                    }
                    if (this.isPathfinding)
                    {
                        if (this.AStarPathDataQueue.Count == 0)
                        {
                            (this as MainPlayer).EndPathFind();
                            return;
                        }
                        List<MoveData> list = new List<MoveData>();
                        MoveData item = this.AStarPathDataQueue.Dequeue();
                        list.Add(item);
                        LSingleton<GameManager>.Instance.PNetWork.ReqMove(list, false);
                        this.MoveTo(item);
                    }
                }
                else if (Vector3.Dot(a, vector3) < 0f)
                {
                    LUtil.Log("移动异常！!", LUtil.LogType.Normal, false);
                    zero = this.TargetPos;
                    this.beginMove = false;
                    SingletonForMono<NetWorkModule>.Instance.StartCoroutine(this.OnTimerToDes());
                    this.isTweening = false;
                }
                else
                {
                    zero = this.ModelObj.transform.position + ((Vector3) ((this.moveDir * this.moveSpeed) * Time.smoothDeltaTime));
                }
                float mapHeight = MapHightDataHolder.GetMapHeight(zero.x, zero.z);
                this.ModelObj.transform.position = new Vector3(zero.x, mapHeight, zero.z);
            }
            if ((this.TargetRot.x != 0f) || (this.TargetRot.z != 0f))
            {
                Quaternion to = Quaternion.LookRotation(this.TargetRot, Vector3.up);
                Quaternion quaternion2 = Quaternion.Lerp(this.ModelObj.transform.rotation, to, this.rotateSpeed * Time.smoothDeltaTime);
                if (this.IsCanRotate())
                {
                    this.ModelObj.transform.rotation = quaternion2;
                }
            }
        }
    }


    public virtual void PlayAnimation(string aniname, float transitionDuration = 0.2f)
    {
        if (this.animator == null)
        {
            UnityEngine.Debug.LogError("Havent Animator Component!");
        }
        else
        {
            AnimatorStateInfo currentAnimatorStateInfo = this.animator.GetCurrentAnimatorStateInfo(0);
        }
    }

    public void SetRotateTarget(Vector3 targetpos)
    {
        Vector3 vector = targetpos - this.ModelObj.transform.position;
        vector.y = 0f;
        this.TargetRot = vector.normalized;
        this.ModelObj.transform.rotation = Quaternion.LookRotation(this.TargetRot, Vector3.up);
    }

    public void StopMoving()
    {
        this.beginMove = false;
        this.isMoving = false;
        this.isTweening = false;
        this.beginTurn = false;
    }


    IEnumerator OnTimerToDes()
    {
        yield return new WaitForSeconds(0.05f);

        if (!isTweening)
            isMoving = false;
    }
}

