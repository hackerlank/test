//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using UnityEngine;

/// <summary>
/// Attaching this script to an object will make it visibly follow another object, even if the two are using different cameras to draw them.
/// </summary>

[AddComponentMenu("NGUI/Examples/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
	public Transform target;
	public bool disableIfInvisible = true;
    public float offY = 0.4f;
	Transform mTrans;
	public Camera mGameCamera;
    public Camera mUICamera;
	public bool mIsVisible = false;
	void Awake ()
	{
	    mTrans = transform;
        mGameCamera = Camera.main;
        mUICamera = UICamera.currentCamera;
	}

    public void UpdateMainCamera()
    {
        if (null == mGameCamera)
            mGameCamera = Camera.main;
        if (mGameCamera.enabled == false)
        {
            mGameCamera = Camera.main;
        }

        if (null == mUICamera)
            mUICamera = UICamera.currentCamera;
        if (mUICamera.enabled == false)
        {
            mUICamera = UICamera.currentCamera;
        }
       
    }

    void LateUpdate()
	{
        if (target != null)
        {
            UpdateMainCamera();
            if (mGameCamera == null || mUICamera == null)
                return;
            Vector3 off = target.position + new Vector3(0.0F, offY, 0.0F);
            Vector3 temp = mGameCamera.WorldToScreenPoint(off);
            Vector3 v = mUICamera.ScreenToWorldPoint(temp);
            mTrans.position = new Vector3(v.x, v.y, 0); 
        }
	}
}
