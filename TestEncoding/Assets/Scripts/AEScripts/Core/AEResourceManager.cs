////////////////////////////////////////////////////////////////////////////////
//  
// @module Affter Effect Importer
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class AEResourceManager  {


    public static GameObject CreateFootage(AEFootage sp, bool isAtlas)
    {
        GameObject obj = Object.Instantiate(Resources.Load("Art/AEFootage")) as GameObject;
        Transform plane = obj.transform.FindChild("AEPlane");
        if (isAtlas)
        {
            MeshFilter mf = plane.GetComponent<MeshFilter>();
            Mesh resMesh = mf.sharedMesh;
            Mesh newMesh = new Mesh();
            newMesh.vertices = resMesh.vertices;
            newMesh.uv = resMesh.uv;
            newMesh.triangles = resMesh.triangles;
            newMesh.normals = resMesh.normals;

            mf.mesh = newMesh;
        }

        sp.plane = plane;
        return obj;
    }


    public static GameObject CreateComposition(AEComposition sp)
    {
        GameObject comp = new GameObject("AEComposition");
        //AEComposition comp = new GameObject ("AEComposition").AddComponent<AEComposition> ();

        GameObject p = new GameObject("Composition");
        p.transform.parent = comp.transform;
        p.transform.localPosition = Vector3.zero;
        p.transform.localScale = Vector3.one;

        sp.plane = p.transform;

        return comp;
    }
}
