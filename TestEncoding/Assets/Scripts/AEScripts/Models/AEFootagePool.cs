using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AEFootagePoolMgr
{
    private static AEFootagePoolMgr instance = null;
    public static AEFootagePoolMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AEFootagePoolMgr();
                instance.AddPool();
            }
            return instance;
        }
    }

    private List<AEFootagePool> poolList = new List<AEFootagePool>();
    private void AddPool()
    {
        GameObject obj = new GameObject();
        obj.name = "AEFootagePool";
        AEFootagePool pool = obj.AddComponent<AEFootagePool>();
        pool.InitPool();
        poolList.Add(pool);
        _CurPool = pool;
    }

    private AEFootagePool _CurPool;
    public AEFootagePool CurPool
    {
        get { return _CurPool; }
    }
}


public class AEFootagePool : MonoBehaviour
{
    public static void DestroyInstance()
    {
        if (instance == null) return;
        if (instance.gameObject == null) return;

        GameObject.Destroy(instance.gameObject);
    }

    private static AEFootagePool instance = null;
    public static AEFootagePool Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "AEFootagePool";
                AEFootagePool pool = obj.AddComponent<AEFootagePool>();
                pool.InitPool();
                instance = pool;
            }
            return instance;
        }
    }

    private UnityEngine.Object planeOri;
    public void InitPool()
    {
        planeOri = Resources.Load("Art/AEFootage");
    }

    public void PreInstance()
    {
        for (int i = 0; i < 40; i++)
        {
            NewOne();
        }
    }



    public List<AEFootagePoolObj> poolObjList = new List<AEFootagePoolObj>();

    private static string strAEPlane = "AEPlane";
    public AEFootagePoolObj NewOne()
    {
        GameObject objRoot = new GameObject();
        objRoot.name = "pool_" + poolObjList.Count;

        GameObject obj = Object.Instantiate(planeOri) as GameObject;
        obj.transform.parent = objRoot.transform;
        objRoot.transform.parent = transform;

        Transform plane = obj.transform.FindChild(strAEPlane);
        MeshFilter mf = plane.GetComponent<MeshFilter>();
        Mesh resMesh = mf.sharedMesh;
        Mesh newMesh = new Mesh();
        newMesh.vertices = resMesh.vertices;
        newMesh.uv = resMesh.uv;
        newMesh.triangles = resMesh.triangles;
        newMesh.normals = resMesh.normals;
        mf.mesh = newMesh;
        AEFootagePoolObj po = new AEFootagePoolObj(obj, objRoot);
        poolObjList.Add(po);
        return po;
    }

    public AEFootagePoolObj GetPoolOne(AEFootage sp, AfterEffectAnimation _anim)
    {
        AEFootagePoolObj newOne = GetNoneUsed();
        newOne.RenderFootage(sp, _anim);
        return newOne;
    }

    private Vector3 farPos = new Vector3(10000, 10000, 10000);

    public AEFootagePoolObj GetNoneUsed()
    {
        AEFootagePoolObj one = null;
        for (int i = 0; i < poolObjList.Count; i++)
        {
            one = poolObjList[i];
            if (one.AEFootageData == null)
            {
                one.transformRoot.localPosition = farPos;
                return one;
            }
        }

        return NewOne();
    }

    AEFootagePoolObj mid;
    void Update()
    {
        for (int i = 0; i < poolObjList.Count; i++)
        {
            mid = poolObjList[i];

            if (mid.AfterEffectAnimationData == null)
            {
                if (mid.transformRoot != null)
                    mid.transformRoot.localPosition = farPos;

                mid.material.mainTexture = null;
                mid.AEFootageData = null;
                mid.SpritesHolder = null;
            }
            else
            {
                mid.RefreshRootTransform();
            }
        }
    }

    void OnDestroy()
    {
        instance = null;
    }
}


public class AEFootagePoolObj
{
    public Transform transformRoot;
    public GameObject gameObject;
    public Transform transform;
    public Transform plane;
    public GameObject _childAnchor;
    public Material material;
    public Mesh mesh;

    private static string strAEPlane = "AEPlane";
    private static string strChildAnchor = "ChildAnchor";
    private static string strSpritesHolder = "SpritesHolder";

    public AEFootagePoolObj(GameObject obj, GameObject root)
    {
        transformRoot = root.transform;
        gameObject = obj;
        transform = gameObject.transform;
        plane = transform.FindChild(strAEPlane);
        mesh = plane.GetComponent<MeshFilter>().sharedMesh;

        _childAnchor = new GameObject(strChildAnchor);
        _childAnchor.transform.parent = gameObject.transform;
        _childAnchor.transform.localPosition = plane.localPosition;
        material = new Material(AEShaders.shaders[5]);
        plane.GetComponent<MeshRenderer>().material = material;
    }

    private AEFootage _AEFootage;
    public AEFootage AEFootageData
    {
        get { return _AEFootage; }
        set { _AEFootage = value; }
    }

    private AfterEffectAnimation _AfterEffectAnimation;
    public AfterEffectAnimation AfterEffectAnimationData
    {
        get { return _AfterEffectAnimation; }
    }


    private Vector3 farPos = new Vector3(10000, 10000, 10000);
    public void ResetAEFootage()
    {
        _AEFootage = null;
        _AfterEffectAnimation = null;
        material.mainTexture = null;

        if (transformRoot != null)
            transformRoot.localPosition = farPos;
    }


    private Transform spritesHolder;
    public Transform SpritesHolder
    {
        get { return spritesHolder; }
        set { spritesHolder = value; }
    }

    public void RefreshRootTransform()
    {
        transformRoot.localPosition = spritesHolder.position;
        transformRoot.localEulerAngles = spritesHolder.eulerAngles;
        transformRoot.localScale = spritesHolder.lossyScale;
    }
 
    public void RenderFootage(AEFootage sp, AfterEffectAnimation _anim)
    {
        _AEFootage = sp;
        _AfterEffectAnimation = _anim;

        spritesHolder = _AfterEffectAnimation.transform.FindChild(strSpritesHolder);
        RefreshRootTransform();
        plane.gameObject.layer = spritesHolder.gameObject.layer;
    }


}