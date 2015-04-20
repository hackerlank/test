using UnityEngine;
using System.Collections;

public class Speres : MonoBehaviour {

    public Color color = Color.red;

    public float radius = 1;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
