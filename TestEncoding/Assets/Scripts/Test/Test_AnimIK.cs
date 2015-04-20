using UnityEngine;
using System.Collections;

public class Test_AnimIK : MonoBehaviour
{
    public float rightRootPosWeight = 1;
    public float rightRootRotWeight = 1;
    public Transform RightTransform;

    public float leftRootPosWeight = 1;
    public float leftRootRotWeight = 1;
    public Transform LeftTransform;
    public Animator animator;
	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
        RightTransform = GameObject.FindGameObjectWithTag("Player").transform;
        LeftTransform = GameObject.FindGameObjectWithTag("Target").transform;
	}
	
    //// Update is called once per frame
    //void OnAnimatorMove () {
	
    //}

    void OnAnimatorIK()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightRootPosWeight);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, RightTransform.position);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightRootRotWeight);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, RightTransform.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftRootPosWeight);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, LeftTransform.position);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftRootRotWeight);
        animator.SetIKRotation(AvatarIKGoal.LeftFoot, LeftTransform.rotation);
    }
}
