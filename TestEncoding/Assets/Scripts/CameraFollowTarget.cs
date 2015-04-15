using System;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public Vector3 Offset;
    public float PosDamp = 10f;
    public Vector3 Rotation;
    public float RotDamp = 10f;
    public Transform Target;
    private Transform trans;

    private void Start()
    {
        this.trans = base.transform;
    }

    private void Update()
    {
        if (this.Target != null)
        {
            base.transform.position = Vector3.Lerp(this.trans.position, this.Target.position + this.Offset, this.PosDamp * Time.smoothDeltaTime);
            Quaternion quaternion = Quaternion.LookRotation(this.Target.position - base.transform.position);
            base.transform.eulerAngles = this.Rotation;
        }
    }
}

