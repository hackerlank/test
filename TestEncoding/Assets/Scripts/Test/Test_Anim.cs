using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Test_Anim : MonoBehaviour
{
    
    public Animator animator;
    private Dictionary<string, int> animHashDictionary = new Dictionary<string, int>();
    private string[] animStrings =
    {
        "a01",
        "a02",
        "h01",
        "r02",
        "w01",
        "d01",
        "e01",
        "m01",
        "r01",
        "q01"
    };
	// Use this for initialization
	void Start ()
	{
	    animator = GetComponent<Animator>();
	    foreach (var animString in animStrings)
	    {
	        animHashDictionary.Add(animString, Animator.StringToHash(animString));
	    }
	}

    public float moveSpeed = 5;
    public float animSpeed = 2;
    //// Update is called once per frame
#if false
    void OnAnimatorMove ()
    {
        Debug.Log(animator.deltaPosition);
        this.transform.rotation = animator.rootRotation;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        foreach (var animString in animStrings)
        {
            if (info.IsName(animString))
            {
                Debug.Log(animString + "=" + animHashDictionary[animString]);
                
            }
        }
       
        if (info.IsName("r01"))
        {
            this.transform.position += moveSpeed * animator.deltaPosition*Time.deltaTime;
        }
        else
        {
            this.transform.position = animator.rootPosition;
        }
        
        
    }
#else
    void OnAnimatorMove ()
    {
        Debug.Log("animator.deltaPosition=" + animator.deltaPosition + ";rootPosition=" + animator.rootPosition);
        this.transform.rotation = animator.rootRotation;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        foreach (var animString in animStrings)
        {
            if (info.IsName(animString))
            {
                Debug.Log(animString + "=" + animHashDictionary[animString]);
                
            }
        }
       
        if (info.IsName("m01"))
        {
            this.transform.position = animator.rootPosition + moveSpeed * transform.forward*Time.deltaTime;
            animator.speed = animSpeed;
        }
        else
        {
            this.transform.position = animator.rootPosition;
            animator.speed = 1;
        }
        
        
    }
#endif
    //void OnAnimatorIK()
    //{
        
    //}

    
    void OnGUI()
    {
        int buttonWidth = 100;
        int buttonHeight = 50;
        int offset = 10;
        int index = 0;
        foreach (var animString in animStrings)
        {
            if (GUI.Button(new Rect(10, buttonHeight*index + offset, buttonWidth, buttonHeight), animString))
            {
               // animator.CrossFade(animHashDictionary[animString], 0.1f);
                animator.Play(animHashDictionary[animString]);
            }
            index++;
        }
        
    }
}
