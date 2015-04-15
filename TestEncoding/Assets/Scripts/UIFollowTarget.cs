using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Follow Target")]
public class UIFollowTarget : MonoBehaviour
{
    public bool disableIfInvisible = true;
    public Camera mGameCamera;
    public bool mIsVisible;
    private Transform mTrans;
    public Camera mUICamera;
    public float offY = 0.4f;
    public Transform target;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mGameCamera = Camera.main;
        this.mUICamera = UICamera.currentCamera;
    }

    private void LateUpdate()
    {
        if (this.target != null)
        {
            this.UpdateMainCamera();
            if ((this.mGameCamera != null) && (this.mUICamera != null))
            {
                Vector3 position = this.target.position + new Vector3(0f, this.offY, 0f);
                Vector3 vector2 = this.mGameCamera.WorldToScreenPoint(position);
                Vector3 vector3 = this.mUICamera.ScreenToWorldPoint(vector2);
                this.mTrans.position = new Vector3(vector3.x, vector3.y, 0f);
            }
        }
    }

    public void UpdateMainCamera()
    {
        if (null == this.mGameCamera)
        {
            this.mGameCamera = Camera.main;
        }
        if (!this.mGameCamera.enabled)
        {
            this.mGameCamera = Camera.main;
        }
        if (null == this.mUICamera)
        {
            this.mUICamera = UICamera.currentCamera;
        }
        if (!this.mUICamera.enabled)
        {
            this.mUICamera = UICamera.currentCamera;
        }
    }
}

