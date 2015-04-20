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

//[ExecuteInEditMode]
public class AfterEffectAnimation : EventDispatcher
{
    public const string ANIMATION_COMPLETE = "animation_complete";
    public const string ENTER_FRAME = "enter_frame";

    public TextAsset dataFile;

    public bool IsConvert;

    public Vector3 vPosOffset;//角色位置偏移

    [SerializeField]
    public string dataFileName;

    public string imagesFolder = "";
    public Color GizmosColor = Color.green;

    public int atlasCount = 0;
    public List<TextAsset> atlasFileList = new List<TextAsset>();
    public List<Texture> atlasTextureList = new List<Texture>();

    [SerializeField]
    public Color MaterialColor = Color.white;

    public int currentFrame = 0;

    public delegate void OnFinished(AfterEffectAnimation tween);
	public delegate void GetFrame(int frame);
    public delegate void KeyFrameEvent();

    private Dictionary<int, KeyFrameEvent> keyFrameEventDic = new Dictionary<int, KeyFrameEvent>();
    public void AddEvent(int keyFrame, KeyFrameEvent evt)
    {
        keyFrameEventDic[keyFrame] = evt;
    }
    public OnFinished BeforFinished;
    public OnFinished onFinished;
	public GetFrame getFrame;

    public bool PlayOnStart = true;
    public bool Loop = true;
    public bool IsAtlas = false;
    public bool isPool = false;

    public bool IsForceSelected = true;

    public float opacity = 1f;


    public float pivotCenterX = 0f;
    public float pivotCenterY = 0f;


    [SerializeField]
    public AESettingsMode mode = AESettingsMode.Simple;

    public int normal_mode_shader = 5;
    public int add_mode_shader = 6;
    public int screen_mode_shader = 7;
    public int lighten_mode_shader = 8;
    public int multiply_mode_shader = 9;

    [SerializeField]
    private AEAnimationData _animationData = null;

    [SerializeField]
    private GameObject _zIndexHolder = null;

    [SerializeField]
    private GameObject _spritesHolder = null;

    [SerializeField]
    private float _timeScale = 25;

    [SerializeField]
    private float _frameDuration = 0.04f;



    private bool _isPlaying = false;

    //[SerializeField]
    private List<AESprite> _sprites = new List<AESprite>();

    public string strPack = "";

    public Action exportActionBegin;
    public Action<AEFootage> exportActionAddAEFootage;
    public Action<AEComposition> exportActionAddAEComposition;
    public Action exportActionFinish;
    //--------------------------------------
    // INITIALIZE
    //--------------------------------------
    public string strExportDataFolder = "Assets/AfterEffect/AEExportData/";
    // public  string strExportDataFolder = "Assets/Resources/Prefabs/AE_skillLayer_data/";
    void Awake()
    {
        foreach (AESprite sprite in sprites)
        {
            sprite.WakeUp();
        }
    }




    void Start()
    {
        if (Application.isPlaying)
        {
            if (PlayOnStart)
            {
                Play();
            }
        }
    }


    //--------------------------------------
    // PUBLIC METHODS
    //--------------------------------------


    public void Play()
    {
        if (_animationData == null)
        {
            return;
        }

        //		CheckInit ();
        if (!_isPlaying)
        {
            fCurUseTime = 0;
            PlayQueue();
            _isPlaying = true;
        }

    }

    public void Stop()
    {
        if (_isPlaying)
        {
            // CancelInvoke("PlayQueue");
            _isPlaying = false;
        }
    }

    public void GoToAndStop(int index)
    {
        Stop();
        currentFrame = index;
        GoToFrame(currentFrame);
    }

    public void GoToAndPlay(int index)
    {
        currentFrame = index;
        GoToFrame(currentFrame);
        Play();
    }

    private float fCurUseTime = 0;

    void Update()
    {

        if (!_isPlaying) return;

        fCurUseTime += Time.deltaTime;
        if (fCurUseTime >= _frameDuration)
        {
            fCurUseTime = 0;
            PlayQueue();
        }
    }

    private void PlayQueue()
    {

        if (keyFrameEventDic.ContainsKey(currentFrame))
        {
            if (keyFrameEventDic[currentFrame] != null)
            {
                keyFrameEventDic[currentFrame]();
            }
        }

        if (currentFrame < totalFrames)
            GoToFrame(currentFrame);

        dispatch(ENTER_FRAME, currentFrame);

        currentFrame++;

        if (getFrame != null)
        {
            getFrame(currentFrame);
        }

        if (currentFrame == totalFrames)//延迟一帧回调
        {
            //Invoke(strPlayQueue, _frameDuration);

            if (BeforFinished != null) { BeforFinished(this); }
            if (onFinished != null) {  onFinished(this); }          
          
            if (!Loop)
            {
                _isPlaying = false;
                onFinished = null;
              //BeforFinished = null;
            }
            else
            {
                currentFrame = 0;
            }
        }

        //else
        //{
        //    if (Loop)
        //    {
        //        currentFrame = 0;
        //        //Invoke(strPlayQueue, _frameDuration);
        //    }
        //    else
        //    {
        //        dispatch(ANIMATION_COMPLETE);
        //    }

        //    if (onFinished != null) onFinished(this);//zjy 改
        //}
    }

    public void GoToFrame(int index)
    {
        foreach (AESprite sprite in sprites)
        {
            sprite.GoToFrame(currentFrame);
        }
    }


    public AESprite GetSpriteByLayerId(int layerId)
    {
        foreach (AESprite sprite in sprites)
        {
            if (sprite.layerId == layerId)
            {
                return sprite;
            }
        }

        Debug.LogWarning("GetSpriteByLayerId  -> sprite not found, layer: " + layerId);
        return null;

    }

    public Shader GetNormalShader()
    {
        return AEShaders.shaders[normal_mode_shader];
    }

    public Shader GetAddShader()
    {
        return AEShaders.shaders[add_mode_shader];
    }

    public Shader GetScreenShader()
    {
        return AEShaders.shaders[screen_mode_shader];
    }

    public Shader GetLightenShader()
    {
        return AEShaders.shaders[lighten_mode_shader];
    }

    public Shader GetMultiplyShader()
    {
        return AEShaders.shaders[multiply_mode_shader];
    }

    public void AnimateOpacity(float valueFrom, float valueTo, float time)
    {
        AETween tw = AETween.Create(transform);
        tw.MoveTo(valueFrom, valueTo, time, OnOpacityAnimationEvent);
    }


    //--------------------------------------
    // EVENTS
    //--------------------------------------
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (_animationData != null)
        {
            Gizmos.color = GizmosColor;

            Vector3 pos = Vector3.zero;

            pos.x += width / 2f;
            pos.y -= height / 2f;


            pos.x -= width * pivotCenterX;
            pos.y += height * pivotCenterY;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, GetWorldScale());

            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireCube(pos, new Vector3(width, height, 0.01f));
        }
        else
        {
            if (dataFile != null)
            {
                OnAnimationDataChange();
            }
        }
    }
#endif
    public virtual void OnAnimationDataChange()
    {
        if (dataFile != null)
        {
            gameObject.name = "AE " + dataFile.name;
            _animationData = AEDataParcer.ParceAnimationData(dataFile.text);
            dataFileName = dataFile.name;

            //cash.animationData = _animationData;
            InitSprites();
        }

        timeScale = 1f;
        OnPivotPositionChnage();
        SetColor(MaterialColor);

        int f = currentFrame;
        currentFrame = 0;
        OnEditorFrameChange();
        currentFrame = f;
        OnEditorFrameChange();
        dataFile = null;
        if (exportActionFinish != null)
            exportActionFinish();

    }


    public void UpdateColor()
    {
        SetColor(MaterialColor);
    }

    public void SetColor(Color c)
    {
        foreach (AESprite sprite in sprites)
        {
            sprite.SetColor(c);
        }
    }




    public void OnEditorFrameChange()
    {
        foreach (AESprite sprite in sprites)
        {
            sprite.GoToFrame(currentFrame);
        }

        OnPivotPositionChnage();
    }

    public void OnPivotPositionChnage()
    {
        if (_animationData != null)
        {
            Vector3 pos = Vector3.zero;
            pos.x = -width * pivotCenterX;
            pos.y = height * pivotCenterY;
            spritesHolder.transform.localPosition = pos;
        }

    }



    private void OnOpacityAnimationEvent(float val)
    {
        opacity = val;
    }




    //--------------------------------------
    // GET / SET
    //--------------------------------------

    public bool isPlaying
    {
        get
        {
            return _isPlaying;
        }
    }

    public float width
    {
        get
        {
            return _animationData.composition.width;
        }
    }

    public float height
    {
        get
        {
            return _animationData.composition.heigh;
        }
    }

    public AECompositionTemplate composition
    {
        get
        {
            return _animationData.composition;
        }
    }

    public int totalFrames
    {
        get
        {
            if (_animationData != null)
            {
                return _animationData.totalFrames;
            }
            else
            {
                return 0;
            }
        }
    }

    public AEAnimationData animationData
    {
        get
        {
            return _animationData;
        }
    }

    public GameObject zIndexHolder
    {
        get
        {
            if (_zIndexHolder == null)
            {
                _zIndexHolder = new GameObject("ZIndexHolder");
                _zIndexHolder.transform.parent = transform;
                _zIndexHolder.transform.localScale = Vector3.one;
                _zIndexHolder.transform.localPosition = Vector3.zero;
            }
            return _zIndexHolder;
        }
    }

    public GameObject spritesHolder
    {
        get
        {
            if (_spritesHolder == null)
            {
                _spritesHolder = new GameObject("SpritesHolder");
                _spritesHolder.transform.parent = transform;
                _spritesHolder.transform.localScale = Vector3.one;
                _spritesHolder.transform.localPosition = Vector3.zero;
            }
            return _spritesHolder;
        }
    }


    public float GetLayerGlobalZ(float index)
    {
        zIndexHolder.transform.localPosition = new Vector3(0, 0, index);
        return zIndexHolder.transform.position.z;
    }



    public AfterrEffectCash cash
    {
        get
        {
            AfterrEffectCash c = GetComponent<AfterrEffectCash>();
#if UNITY_EDITOR
            if (c == null)
            {
                c = gameObject.AddComponent<IAfterrEffectCash>();
            }
#endif
            return c;
        }

    }



    public List<AESprite> sprites
    {
        get
        {
            return _sprites;
        }

        set
        {
            _sprites = value;
        }
    }




    public float timeScale
    {
        get
        {
            return _timeScale;
        }

        set
        {
            _timeScale = value;
            if (_animationData != null)
            {
                _frameDuration = _animationData.frameDuration / _timeScale;
            }

        }
    }


    public float frameDuration
    {
        get
        {
            return _frameDuration;
        }
    }


    //--------------------------------------
    // PRIVATE METHODS
    //--------------------------------------

    private void InitSprites()
    {
        if (exportActionBegin != null)
            exportActionBegin();

        _sprites.Clear();

        List<Transform> _childs = new List<Transform>();
        foreach (Transform child in transform)
        {
            _childs.Add(child);
        }

        foreach (Transform c in _childs)
        {
            DestroyImmediate(c.gameObject);
        }





        foreach (AELayerTemplate layer in _animationData.composition.layers)
        {
            AESprite sprite = null;
            GameObject obj = null;
            switch (layer.type)
            {
                case AELayerType.FOOTAGE:
                    sprite = new AEFootage();
                    obj = CreateFootage((AEFootage)sprite, IsAtlas);
                    if (exportActionAddAEFootage != null)
                        exportActionAddAEFootage((AEFootage)sprite);

                    break;
                case AELayerType.COMPOSITION:
                    sprite = new AEComposition();
                    obj = CreateComposition((AEComposition)sprite);
                    if (exportActionAddAEComposition != null)
                        exportActionAddAEComposition((AEComposition)sprite);
                    break;
                default:
                    Debug.LogError("Unsupported layer type: " + layer.type.ToString());
                    break;

            }

            sprite.gameObject = obj;
            sprite.transform = obj.transform;

            sprite.IsAtlas = IsAtlas;

            _sprites.Add(sprite);

            sprite.transform.parent = spritesHolder.transform;

            if (layer.parent != 0)
            {
                sprite.layerId = layer.index;
            }
            else
            {
                sprite.init(layer, this);
            }

        }



        foreach (AELayerTemplate layer in _animationData.composition.layers)
        {
            if (layer.parent != 0)
            {
                AESprite p = GetSpriteByLayerId(layer.parent);
                AESprite c = GetSpriteByLayerId(layer.index);
                p.AddChild(c);
                c.init(layer, this);
            }
        }

        OnEditorFrameChange();

    }


    public void InitFootageSprite(List<AEFootage> spAEFootage)
    {
        _sprites.Clear();

        for (int i = 0; i < spAEFootage.Count; i++)
        {
            AESprite sp = (AESprite)spAEFootage[i];
            //if (!isPool)
            //{
            //    sp.gameObject = _objFootageList[i];
            //    sp.transform = sp.gameObject.transform;
            //    sp.plane = sp.transform.FindChild("AEPlane");
            //    //sp._childAnchor = sp.transform.FindChild("ChildAnchor").gameObject;
            //}

            sp._anim = this;
            _sprites.Add(sp);
        }
    }


    public virtual GameObject CreateFootage(AEFootage sp, bool isAtlas)
    {
        return AEResourceManager.CreateFootage(sp, isAtlas);
    }

    public virtual GameObject CreateComposition(AEComposition sp)
    {
        return AEResourceManager.CreateComposition(sp);
    }



    protected Vector3 GetWorldScale()
    {

        Vector3 worldScale = transform.localScale;
        Transform parent = transform.parent;


        while (parent != null)
        {
            worldScale = Vector3.Scale(worldScale, parent.localScale);
            parent = parent.parent;
        }
        return worldScale;
    }

    protected void OnDisable()
    {
        foreach (AESprite sprite in sprites)
        {
            sprite.disableRenderer();
        }
    }

    protected void OnEnable()
    {
        foreach (AESprite sprite in sprites)
        {
            sprite.enableRenderer();
        }
    }

    /*
    private void CheckInit() {
        if(!isInited) {
            foreach(AESprite sprite in sprites) {
                sprite.initInPlayMode (composition.GetLayer (sprite.layerId), this);
            } 

            isInited = true;
        }
    }
    */

}
