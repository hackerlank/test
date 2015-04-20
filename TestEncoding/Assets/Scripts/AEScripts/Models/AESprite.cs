////////////////////////////////////////////////////////////////////////////////
//  
// @module <module_name>
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class AESprite
{
    [System.NonSerialized]
    public GameObject gameObject;
    [System.NonSerialized]
    public Transform transform;

    public float w;
    public float h;

    public int layerId;
    public float zIndex;
    public float parentIndex = 0;
    public float indexModifayer = 1f;

    public Vector3 anchorOffset = new Vector3(0, 0, 0);

    public string strAtlasName;
    public Vector2[] uvs;
    public Vector2[] UVS
    {
        get
        {
            if (uvs == null || uvs.Length == 0)
            {
                uvs = new Vector2[4] { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
            }

            return uvs;
        }
    }

    [System.NonSerialized]
    public Transform plane;


    [System.NonSerialized]
    public Material material;

    [System.NonSerialized]
    public Texture2D texture2D;

    //[SerializeField]
    public AELayerTemplate _layer;

    [System.NonSerialized]
    public AfterEffectAnimation _anim;

    [System.NonSerialized]
    public GameObject _childAnchor = null;

    [System.NonSerialized]
    public AEComposition parentComposition = null;


    //[SerializeField]
    public AELayerBlendingType blending = AELayerBlendingType.NORMAL;


    //[SerializeField]
    public bool IsAtlas = false;

    //--------------------------------------
    // INITIALIZE
    //--------------------------------------

    public abstract void WakeUp();



    public virtual void init(AELayerTemplate layer, AfterEffectAnimation animation)
    {
        init(layer, animation, AELayerBlendingType.NORMAL);
    }

    public virtual void init(AELayerTemplate layer, AfterEffectAnimation animation, AELayerBlendingType forcedBlending)
    {
        _layer = layer;
        _anim = animation;

        layerId = layer.index;

        zIndex = parentIndex + (layer.index) * indexModifayer;

        if (forcedBlending == AELayerBlendingType.NORMAL)
        {
            blending = _layer.blending;
        }
        else
        {
            blending = forcedBlending;
        }

    }



    public abstract void GoToFrame(int index);

    public abstract void disableRenderer();
    public abstract void enableRenderer();
    public abstract void SetColor(Color c);

    //--------------------------------------
    //  PUBLIC METHODS
    //--------------------------------------

    public void AddChild(AESprite sprite)
    {
        sprite.transform.parent = childAnchor.transform;
    }


    //--------------------------------------
    //  GET/SET
    //--------------------------------------


    public float parentOpacity
    {
        get
        {
            if (parentComposition != null)
            {
                return parentComposition.opacity;
            }
            else
            {
                return 1f;
            }
        }
    }


    public GameObject childAnchor
    {
        get
        {
            if (_childAnchor == null)
            {
                _childAnchor = new GameObject("ChildAnchor");
                _childAnchor.transform.parent = gameObject.transform;
                _childAnchor.transform.localPosition = plane.localPosition;
            }

            return _childAnchor;

        }
    }


    //--------------------------------------
    //  EVENTS
    //--------------------------------------

    //--------------------------------------
    //  PRIVATE METHODS
    //--------------------------------------

    //--------------------------------------
    //  DESTROY
    //--------------------------------------

}
