using UnityEngine;
using System.Collections;

public class CameraFollowTarget : MonoBehaviour {
    public Transform Target;
    public Vector3 Offset;
    public Vector3 Rotation;

    private Transform trans;
	// Use this for initialization
	void Start () {
        trans = transform;
	}


    public float PosDamp = 10f;
    public float RotDamp = 10f;

	// Update is called once per frame
	void Update () {
        if(Target == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(trans.position, Target.position + Offset,PosDamp * Time.smoothDeltaTime);


        Quaternion rot = Quaternion.LookRotation(Target.position - transform.position);

        transform.eulerAngles = Rotation;  
	}
}
