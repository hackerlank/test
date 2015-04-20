using Algorithms;
using Net;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using msg;
public class CharactorBase// : EActor 
{
    public GameObject ModelObj;                    //角色模型对象
    public Animator animator;
    public NGUI_Blood hpBar;


    //人物身上的状态，每一位表示一个状态
    public char[] state = new char[(int)UserState.USTATE_MAX];

    ////状态对应的特效
    ////public Dictionary<uint, AssetHandler> stateEffectList = new Dictionary<uint, AssetHandler>();
    ///// <summary>
    ///// 取得状态ID对应的特效
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //public AssetHandler GetStateEffect(uint id)
    //{
    //    AssetHandler asset = null;
    //    if (stateEffectList.ContainsKey(id))
    //        asset = stateEffectList[id];

    //    return asset;
    //}
    //public Dictionary<uint, AssetHandler> GetStateEffectList()
    //{
    //    return stateEffectList;
    //}

   
    #region 移动相关

    public bool isTweening = false;
    public bool isMoving = false;
    public bool isPathfinding = false;

    public Vector2 CurrentPosition2D;           //当前格子坐标

    public Queue<MoveData> RetMoveDataQueue;    //缓存接收玩家移动消息的队列

    public Queue<MoveData> AStarPathDataQueue;  //寻路时缓存寻路路径信息

    public List<PathFinderNode> AStarPath;      //寻路路径

    public float moveSpeed = 0f; //实际移动速度
    public float rotateSpeed = 10f;  //旋转速度

    public Vector3 moveDir = new Vector3();  //实际移动方向

    public bool beginMove = false;  //是否开始移动（update中做判断）
    public bool beginTurn = false;  //是否开始转动

    int rotateDir = 1;      //旋转方向，1是顺时针，-1是逆时针

    public Vector3 TargetPos = new Vector3();      //移动目标点
    Vector3 TargetRot = new Vector3();      //目标旋转

    #endregion

   


    public virtual void Init()
    {
        
    }

    ///// <summary>
    ///// 角色类型，设置取得类型用这个接口，*不要用基类的*
    ///// </summary>
    //public ActorType ActorType
    //{
    //    get { return type; }
    //    set { type = value; }
    //}

    /// <summary>
    /// 服务器方向信息转换成客户端方向信息
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public Quaternion GetDirByServerDir(int dir)
    {
        Quaternion rotation = new Quaternion();
        Vector3 vdir = Vector3.zero;
        switch (dir)
        {
            case 0:
                vdir = Vector3.forward;
                break;
            case 1:
                vdir = new Vector3(1, 0, 1).normalized;
                break;
            case 2:
                vdir = Vector3.right;
                break;
            case 3:
                vdir = new Vector3(1, 0, -1).normalized;
                break;
            case 4:
                vdir = Vector3.back;
                break;
            case 5:
                vdir = new Vector3(-1, 0, -1).normalized;
                break;
            case 6:
                vdir = Vector3.left;
                break;
            case 7:
                vdir = new Vector3(-1, 0, 1).normalized;
                break;
        }
        rotation = Quaternion.LookRotation(vdir);

        return rotation;
    }

    #region 移动相关

    /// <summary>
    /// 是否处在可以自由移动的状态
    /// </summary>
    /// <returns></returns>
    public bool IsInFreeMoveState()
    {
        if (Util.isset_state(this.state, (int)UserState.USTATE_SHENPAN))
            return true;

        return false;
    }

    /// <summary>
    /// 是否可以移动
    /// </summary>
    /// <returns></returns>
    public bool IsCanMove()
    {
        //if (IsDie())
        //{
        //    return false;
        //}

        if (IsInFreeMoveState())
            return true;

        if (animator == null)
        {
            return false;
        }

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.nameHash == Animator.StringToHash("Base Layer.a1") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.a2") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.q") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.w") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.e") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.r") && info.normalizedTime < 0.99)
            return false;


        if (info.nameHash == Animator.StringToHash("Base Layer.HitRevive") && info.normalizedTime < 0.99)
            return false;

        return true;
    }

    /// <summary>
    /// 是否可以转向
    /// </summary>
    /// <returns></returns>
    public bool IsCanRotate()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.nameHash == Animator.StringToHash("Base Layer.a1") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.a2") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.q") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.w") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.e") && info.normalizedTime < 0.99)
            return false;

        if (info.nameHash == Animator.StringToHash("Base Layer.r") && info.normalizedTime < 0.99)
            return false;

        return true;
    }


    public void MoveTo(MoveData data)
    {
        MoveTo((int)data.dir, new Vector2(data.x, data.y));
    }

    public void MoveTo(int dir, Vector2 destpoint)
    {
        isMoving = true;
        Vector2 movedir = Vector2.zero;
        switch (dir)
        {
            case 0:
                movedir = new Vector2(0, -1);
                break;
            case 1:
                movedir = new Vector2(1, -1);
                break;
            case 2:
                movedir = new Vector2(1, 0);
                break;
            case 3:
                movedir = new Vector2(1, 1);
                break;
            case 4:
                movedir = new Vector2(0, 1);
                break;
            case 5:
                movedir = new Vector2(-1, 1);
                break;
            case 6:
                movedir = new Vector2(-1, 0);
                break;
            case 7:
                movedir = new Vector2(-1, -1);
                break;
            default:
                movedir = Vector2.zero;
                break;
        }

        TargetPos = GraphUtils.GetWorldPosByServerPos(CurrentPosition2D + movedir);
        Vector3 tempdir = (TargetPos - ModelObj.transform.position).normalized;
        TargetRot = new Vector3(tempdir.x, 0f, tempdir.z);
        moveDir = (GraphUtils.GetWorldPosByServerPos(CurrentPosition2D + movedir) - ModelObj.transform.position);
        moveDir.y = 0;
        moveDir.Normalize();
        Vector3 temp = Vector3.Cross(ModelObj.transform.forward, TargetRot);
        if (temp.y > 0)
        {
            rotateDir = 1;
        }
        else
        {
            rotateDir = -1;
        }
        beginTurn = true;
        beginMove = true;
        isTweening = true;
        CurrentPosition2D = CurrentPosition2D + movedir;
    }

    /// <summary>
    /// update中调用，用来移动玩家位置
    /// </summary>
    public void Moving()
    {
        if (!IsCanMove())
            return;

        if (beginMove)
        {
            Vector3 nexttemppos = ModelObj.transform.position + moveDir * moveSpeed * Time.smoothDeltaTime;

            Vector3 currenttotarget = TargetPos - ModelObj.transform.position;
            currenttotarget.y = 0;

            Vector3 currenttonext = nexttemppos - ModelObj.transform.position;
            currenttonext.y = 0;
            float currentToTar = Vector3.Magnitude(currenttotarget);
            float currentToNext = Vector3.Magnitude(currenttonext);

            Vector3 nextframePos = Vector3.zero;

            if (currentToNext >= currentToTar)
            {
                //ModelObj.transform.position = TargetPos;
                nextframePos = TargetPos;

                beginMove = false;
                NetWorkModule.Instance.StartCoroutine(OnTimerToDes());
                isTweening = false;
                //移动完一个格子如果队列中还有数据那么继续移动
                if (RetMoveDataQueue.Count != 0)
                {
                    MoveData md = RetMoveDataQueue.Dequeue();
                    MoveTo(md);
                }
                if (isPathfinding)
                {
                    if (AStarPathDataQueue.Count == 0)
                    {
                        (this as MainPlayer).EndPathFind();
                        return;
                    }

                    List<MoveData> mds = new List<MoveData>();
                    //出队发送
                    MoveData md = AStarPathDataQueue.Dequeue();
                    mds.Add(md);
                    GameManager.Instance.PNetWork.ReqMove(mds);
                    MoveTo(md);
                }
            }
            else
            {
                float dot = Vector3.Dot(currenttotarget, currenttonext);
                if (dot < 0) //朝目标的反方向走，异常！
                {
                    Util.Log("移动异常！!");
                    nextframePos = TargetPos;

                    beginMove = false;
                    NetWorkModule.Instance.StartCoroutine(OnTimerToDes());
                    isTweening = false;
                }
                else
                {
                    nextframePos = ModelObj.transform.position + moveDir * moveSpeed * Time.smoothDeltaTime;
                }
            }



            ModelObj.transform.position = new Vector3(nextframePos.x, MapHightDataHolder.GetMapHeight(nextframePos.x, nextframePos.z), nextframePos.z);
        }
        //if (beginTurn)
        {
            //float degree = Vector3.Angle(ModelObj.transform.forward, TargetRot);
            //if (degree < 5)
            //{
            //    ModelObj.transform.rotation = Quaternion.LookRotation(TargetRot);

            //    beginTurn = false;
            //    return;
            //}
            //Debug.Log(new Vector3(0, rotateSpeed, 0) * Time.smoothDeltaTime * rotateDir);
            //Debug.Log((ModelObj.transform.eulerAngles +new Vector3(0, rotateSpeed, 0) * Time.smoothDeltaTime * rotateDir));
            //ModelObj.transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.smoothDeltaTime * rotateDir);
            //Debug.Log(ModelObj.transform.eulerAngles);
            //Debug.Log(ModelObj.transform.forward);
            if (TargetRot.x == 0 && TargetRot.z == 0)
            {
                return;
            }

            Quaternion quaTar = Quaternion.LookRotation(TargetRot, Vector3.up);

            Quaternion newRotation = Quaternion.Lerp(ModelObj.transform.rotation, quaTar, rotateSpeed * Time.smoothDeltaTime);

            if (IsCanRotate())
                ModelObj.transform.rotation = newRotation;
        }
    }


    public void SetRotateTarget(Vector3 targetpos)
    {
        Vector3 temprot = targetpos - ModelObj.transform.position;
        temprot.y = 0;
        TargetRot = temprot.normalized;
        ModelObj.transform.rotation = Quaternion.LookRotation(TargetRot, Vector3.up);
    }


    IEnumerator OnTimerToDes()
    {
        yield return new WaitForSeconds(0.05f);
        if (!isTweening)
            isMoving = false;
    }

    /// <summary>
    /// 结束正在进行的平移
    /// </summary>
    public void StopMoving()
    {
        beginMove = false;
        isMoving = false;
        isTweening = false;
        beginTurn = false;
    }

    #endregion

    public virtual void PlayAnimation(string aniname, float transitionDuration = 0.2f)
    {
        if (animator == null)
        {
            Debug.LogError("Havent Animator Component!");
            return;
        }

        
        //当前动画没播完并且不是移动待机则不处理
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        //if (info.nameHash == Animator.StringToHash("Base Layer.a1"))
        //{
        //    Debug.LogWarning("Base Layer.a1 is play");
        //}

        //if (info.nameHash == Animator.StringToHash("Base Layer.a2"))
        //{
        //    Debug.LogWarning("Base Layer.a2 is play");

        //}


        //if ((info.nameHash != Animator.StringToHash(AnimationConst.PlayerRun))
        //    && (info.nameHash != Animator.StringToHash(AnimationConst.PlayerIdle))
        //    && (info.normalizedTime < 0.99f))
        //    return;

        //要播行走动画
        //if (aniname == AnimationConst.PlayerRun)
        //{
        //    if (!IsInSkillAnim(info) && !IsInRunAnimSelf(info))
        //    {
        //        animator.CrossFade(Animator.StringToHash(AnimationConst.PlayerRun), 0.05f, 0, 0f);
        //        //mState = UState.Move;
        //        base.ctrl.mAnimData.State = UState.Move;
        //    }
        //}

        ////要播待机动画
        //else if (aniname == AnimationConst.PlayerIdle)
        //{
        //    if (!IsInSkillAnim(info) && !IsInIdleAnimSelf(info))
        //    {
        //        //mState = UState.Idle;
        //        animator.CrossFade(Animator.StringToHash(AnimationConst.PlayerIdle), 0.1f, 0, 0f);
        //        base.ctrl.mAnimData.State = UState.Idle;
        //    }
        //}

        //if(animator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash(aniname))
        //{
        //    return;
        //}
        //AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        //if (!animator.IsInTransition(0))
        //{
        //    Util.Log("YiMing aniname : "+aniname);
        //    animator.CrossFade(aniname, 0.1f);
        //}

    }

    bool IsInSkillAnim(AnimatorStateInfo info)
    {
        //if ((base.ctrl.mAnimData.State == UState.Skill))
        //{
        //    return true;
        //}

        //if ((base.ctrl.mAnimData.State == UState.Hit))
        //{
        //    return true;
        //}

        //if ((base.ctrl.mAnimData.State == UState.Die))
        //{
        //    return true;
        //}

        //if ((base.ctrl.mAnimData.State == UState.Relive))
        //{
        //    return true;
        //}

        return false;
    }

    //bool IsInRunAnim(AnimatorStateInfo info)
    //{
    //    if (info.nameHash == Animator.StringToHash("Base Layer.run") && info.normalizedTime < 0.1f)
    //        return true;

    //    return false;

    //}
    //bool IsInRunAnimSelf(AnimatorStateInfo info)
    //{
    //    if (base.ctrl.mAnimData.State == UState.Move)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //bool IsInIdleAnimSelf(AnimatorStateInfo info)
    //{
    //    if (base.ctrl.mAnimData.State == UState.Idle)
    //    {
    //        return true;
    //    }
    //    return false;

    //}

    public virtual void Die()
    {
        //ctrl.PlayDie(URelive.Revive, 0f);
    }

    public virtual void DelayRelive()
    {
        //if (IsDie())
        //{
            //ctrl.SetRelive();
            //foreach (Renderer renderer in this.eActor.avatar.renders)
            //{
            //    renderer.enabled = true;
            //}
        //}
    }


    public virtual void DestroyThis()
    {
 
    }
}
