////////////////////////////////////////////////////////////////////////////////
//  
// @module Affter Effect Importer
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

[System.Serializable]
public class AEFootage : AESprite
{
    public float opacity = 1f;

    public Color materialColor = Color.white;

    public bool _isEnabled = true;

    public static Color defaultColor = new Color(0.5f, 0.5f, 0.5f, 1);
    public static AEFootage CloneData(AEFootage sour)
    {
        AEFootage newData = new AEFootage();
        newData.w = sour.w;
        newData.h = sour.h;
        newData.opacity = sour.opacity;
        //newData._isEnabled = sour._isEnabled;
        newData.layerId = sour.layerId;
        newData.zIndex = sour.zIndex;
        newData.parentIndex = sour.parentIndex;
        newData.indexModifayer = sour.indexModifayer;
        newData._layer = sour._layer;
        newData.blending = sour.blending;
        newData.IsAtlas = sour.IsAtlas;
        newData.strAtlasName = sour.strAtlasName;
        newData.uvs = sour.uvs;
        //newData.materialColor = sour.materialColor;
        newData.materialColor = defaultColor;
        newData.anchorOffset = sour.anchorOffset;

        return newData;
    }

    //--------------------------------------
    // INITIALIZE
    //--------------------------------------


    public override void WakeUp()
    {

        if (IsAtlas) return;

        if (plane.renderer.sharedMaterial == null && _layer != null)
        {
            SetMaterial(false);
        }
    }


    public override void init(AELayerTemplate layer, AfterEffectAnimation animation, AELayerBlendingType forcedBlending)
    {

        base.init(layer, animation, forcedBlending);

        gameObject.name = layer.name + " (Footage)";
        SetMaterial(true);

        color = _anim.MaterialColor;

        GoToFrame(0);

        //#if UNITY_EDITOR
        //        IAESpriteRenderer r = plane.gameObject.AddComponent<IAESpriteRenderer>();
        //#else
        //        AESpriteRenderer r = plane.gameObject.AddComponent<AESpriteRenderer>();
        //#endif

        //r.anim = _anim;
        //r.enabled = false;
    }


    //--------------------------------------
    // PUBLIC METHODS
    //--------------------------------------


    public override void disableRenderer()
    {
        //if (_isEnabled)
        //{
        //    if (plane != null)
        //        plane.renderer.enabled = false;

        //    _isEnabled = false;
        //}
        _isEnabled = false;

        if (poolObj != null)
        {
            poolObj.ResetAEFootage();
            poolObj = null;
        }

    }

    public override void enableRenderer()
    {
        _isEnabled = true;

        //if (!_isEnabled)
        //{
        //    plane.renderer.enabled = true;
        //    _isEnabled = true;
        //}

    }

    private AEFootagePoolObj poolObj;
    public void RenderData(AEFootagePoolObj pool)
    {
        gameObject = pool.gameObject;
        transform = pool.transform;
        plane = pool.plane;
        _childAnchor = pool._childAnchor;
        pool.material.shader = shader;

        for (int i = 0; i < _anim.atlasTextureList.Count; i++)
        {
            Texture tex = _anim.atlasTextureList[i];
            if (tex.name == strAtlasName)
            {
                pool.material.mainTexture = tex;

                break;
            }
        }

        pool.mesh.uv = UVS;

        poolObj = pool;
    }

    public override void GoToFrame(int index)
    {
        if (!Application.isPlaying || !_isEnabled) return;

        if (index < _layer.inFrame || index >= _layer.outFrame)
        {
            if (!_anim.isPool)
            {
                disableRenderer();
            }
            else
            {
                if (poolObj != null)
                {
                    poolObj.ResetAEFootage();
                    poolObj = null;
                }
                return;
            }
        }
        else
        {
            if (!_anim.isPool)
            {
                enableRenderer();
            }
            else
            {
                if (poolObj == null)
                    RenderData(AEFootagePool.Instance.GetPoolOne(this, _anim));
            }
        }

        AEFrameTemplate frame = _layer.GetFrame(index);
        if (frame == null)
        {
            return;
        }


        //if(frame.IsPositionChanged) 
        {
            transform.localPosition = frame.positionUnity;
        }


        //if(frame.IsPivotChnaged) 
        {
            plane.localPosition = new Vector3(-frame.pivot.x + anchorOffset.x, frame.pivot.y + anchorOffset.y, 0f);

            childAnchor.transform.localPosition = plane.transform.localPosition;
            childAnchor.transform.localScale = Vector3.one;


            Vector3 pos = plane.position;


            pos.z = _anim.GetLayerGlobalZ(zIndex);

            if (_anim.IsConvert)
                pos.z = +pos.z;

            pos += _anim.vPosOffset;

            plane.position = pos;
        }

        //if(frame.IsRotationChanged) 
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, -frame.rotation));
        }



        //if(frame.IsScaleChanged) 
        {
            transform.localScale = frame.scale;
        }


        plane.localScale = new Vector3(w, h, 1);

        SetOpcity(parentOpacity * frame.opacity * 0.01f * _anim.opacity);

    }


    public void SetOpcity(float op)
    {
        //if(opacity != op) {
        opacity = op;

        materialColor.a = opacity;
        color = materialColor;
        //}
    }

    public override void SetColor(Color c)
    {
        materialColor = c;
        float a = color.a;
        c.a = a;
        color = c;
    }

    public static string strPath = "AfterEffect/";
    public virtual void SetMaterial(bool isEditor)
    {
        string textureName = "Assets/" + strPath + _anim.imagesFolder + _layer.source;

#if UNITY_EDITOR
        if (texture2D == null)
            texture2D = AssetDatabase.LoadAssetAtPath(textureName, typeof(Texture2D)) as Texture2D;
#endif

        InitTex(textureName, texture2D, isEditor);

        //Debug.Log(textureName);
    }

    private void InitTex(string textureName, Texture2D tex, bool isEditor)
    {
        if (tex != null)
        {
            try
            {
                if (material == null)
                    material = new Material(shader);

                if (isEditor)
                    CreateAsset();

            }
            catch (System.Exception ex)
            {
                //Shader[] _shaders = (Shader[])UnityEngine.Resources.FindObjectsOfTypeAll(typeof(Shader));
                //Debug.Log(_shaders.Length);
                //for (int i = 0; i < _shaders.Length; i++)
                //{
                //    Debug.Log(_shaders[i]);
                //}

                //Debug.LogError("Shader Mode :" + blending);
                Debug.LogError("InitTex shader: " + ex.Message + "\n" + ex.StackTrace);

            }


            //Debug.Log(material + "   " + tex);
            plane.renderer.sharedMaterial = material;
            plane.renderer.sharedMaterial.SetTexture("_MainTex", tex);

            plane.renderer.sharedMaterial.name = tex.name;

            w = _layer.width;
            h = _layer.height;

            plane.localScale = new Vector3(w, h, 1);
        }
        else
        {
            Debug.LogWarning("Affter Effect: Texture " + textureName + " not found");
        }
    }


    private void CreateAsset()
    {
#if UNITY_EDITOR
        string strPathWithOutAsset = strPath + _anim.imagesFolder + _anim.dataFileName + "/";
        string strAssetPath = "Assets/" + strPathWithOutAsset;

        string strFolder = Application.dataPath + "/" + strPathWithOutAsset;

        string name = layerId + "_" + _layer.sourceNoExt;

        string matName = strAssetPath + name + ".mat";
        string meshName = strAssetPath + name + ".asset";
        AssetDatabase.DeleteAsset(matName);
        AssetDatabase.DeleteAsset(meshName);

        if (!IsAtlas) return;
        if (material == null) return;
        if (plane == null) return;
        Mesh mesh = plane.GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) return;

        if (!System.IO.Directory.Exists(strFolder)) System.IO.Directory.CreateDirectory(strFolder);

        AssetDatabase.CreateAsset(material, matName);
        AssetDatabase.CreateAsset(mesh, meshName);
        texture2D = null;
#endif
    }


    //--------------------------------------
    // GET / SET
    //--------------------------------------



    public Color color
    {
        get
        {
            Material m = plane.renderer.sharedMaterial;
            if (m.HasProperty("_Color"))
            {
                return m.color;
            }
            else
            {
                if (m.HasProperty("_TintColor"))
                {
                    return m.GetColor("_TintColor");
                }
                else
                {
                    return Color.white;
                }

            }
        }

        set
        {
            if (plane.renderer.sharedMaterial.HasProperty("_Color"))
            {
                plane.renderer.sharedMaterial.color = value;
            }
            else
            {
                if (plane.renderer.sharedMaterial.HasProperty("_TintColor"))
                {
                    plane.renderer.sharedMaterial.SetColor("_TintColor", value);
                }

            }
        }
    }

    private static bool isUseNormalShader = false;
    public static bool IsUseNormalShader
    {
        get { return isUseNormalShader; }
        set { isUseNormalShader = value; }
    }

    public Shader shader
    {
        get
        {

            Shader sh;

            if (isUseNormalShader)
                return _anim.GetNormalShader();

            switch (blending)
            {
                case AELayerBlendingType.ADD:
                    sh = _anim.GetAddShader();
                    break;
                case AELayerBlendingType.SCREEN:
                    sh = _anim.GetScreenShader();
                    break;
                case AELayerBlendingType.LIGHTEN:
                    sh = _anim.GetLightenShader();
                    break;
                case AELayerBlendingType.MULTIPLY:
                    sh = _anim.GetMultiplyShader();
                    break;
                default:
                    sh = _anim.GetNormalShader();
                    break;
            }
            return sh;
        }
    }







}
