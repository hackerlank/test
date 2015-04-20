////////////////////////////////////////////////////////////////////////////////
//  
// @module Affter Effect Importer
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(IAfterEffectAnimation))]
public class AfterEffectAnimationEditor : Editor
{

    IAfterEffectAnimation _target;
    private float _timeScale;
    private AESettingsMode _mode;

    private SerializedProperty _GizmosColor;
    private SerializedProperty _MaterialColor;

    private SerializedProperty _dataFile;
    private SerializedProperty _dataFileName;

    private SerializedProperty _imagesFolder;

    private SerializedProperty _PlayOnStart;
    private SerializedProperty _Loop;

    //private SerializedProperty _IsAtlas;
    //private SerializedProperty _IsPool;

    private SerializedProperty _currentFrame;

    private SerializedProperty _opacity;
    private SerializedProperty _IsForceSelected;

    private SerializedProperty _pivotCenterX;
    private SerializedProperty _pivotCenterY;



    //--------------------------------------
    // PUBLIC METHODS
    //--------------------------------------

    private void OnEnable()
    {
        _target = (IAfterEffectAnimation)serializedObject.targetObject;

        _GizmosColor = serializedObject.FindProperty("GizmosColor");


        _imagesFolder = serializedObject.FindProperty("imagesFolder");
        _dataFile = serializedObject.FindProperty("dataFile");
        _dataFileName = serializedObject.FindProperty("dataFileName");

        _PlayOnStart = serializedObject.FindProperty("PlayOnStart");
        _Loop = serializedObject.FindProperty("Loop");

        //_IsAtlas = serializedObject.FindProperty("IsAtlas");
        //_IsPool = serializedObject.FindProperty("isPool");

        _currentFrame = serializedObject.FindProperty("currentFrame");

        _opacity = serializedObject.FindProperty("opacity");

        _IsForceSelected = serializedObject.FindProperty("IsForceSelected");

        _pivotCenterX = serializedObject.FindProperty("pivotCenterX");
        _pivotCenterY = serializedObject.FindProperty("pivotCenterY");

        _MaterialColor = serializedObject.FindProperty("MaterialColor");

        _target.exportActionBegin = ExportBeign;
        _target.exportActionAddAEFootage = ExportAddAEFootage;
        _target.exportActionAddAEComposition = ExportAddAEComposition;
        _target.exportActionFinish = ExportFinish;

    }


    public override void OnInspectorGUI()
    {

        serializedObject.Update();

        EditorGUILayout.Separator();

        if (targets.Length > 1)
        {
            EditorGUILayout.HelpBox("Multiedition Mode", MessageType.Info);
        }
        else
        {
            if (anim.dataFile != null)
            {
                if (anim.animationData != null)
                {
                    float duration = anim.animationData.duration / anim.timeScale;

                    string info = "";
                    info += "Total Frames: " + anim.animationData.totalFrames + ", ";
                    info += "Duration: " + duration + " sec ";
                    EditorGUILayout.HelpBox(info, MessageType.Info);

                }
                else
                {
                    EditorGUILayout.HelpBox("Calculating.....", MessageType.Info);
                }


            }
            else
            {
                EditorGUILayout.HelpBox("No Animation Data", MessageType.Warning);
            }
        }




        EditorGUILayout.Separator();


        EditorGUI.BeginChangeCheck();


        EditorGUILayout.PropertyField(_dataFile);
        EditorGUILayout.PropertyField(_dataFileName);

        EditorGUILayout.PropertyField(_imagesFolder);
        if (EditorGUI.EndChangeCheck())
        {
            ReloadAnimation();
        }

        Atlas_TextTexture();

        EditorGUILayout.PropertyField(_GizmosColor);

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_MaterialColor);
        if (EditorGUI.EndChangeCheck())
        {
            foreach (Object t in targets)
            {
                (t as AfterEffectAnimation).UpdateColor();
            }
        }


        if (anim.totalFrames != 0)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.IntSlider(_currentFrame, 0, anim.totalFrames - 1);
            if (EditorGUI.EndChangeCheck())
            {
                OnEditorFrameChange();
            }
        }
        else
        {
            if (anim.dataFile != null)
            {
                //	ReloadAnimation ();
            }
        }

        _timeScale = EditorGUILayout.Slider("Time Scale", anim.timeScale, 0.1f, 2f);
        foreach (Object t in targets)
        {
            (t as AfterEffectAnimation).timeScale = _timeScale;
        }




        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_opacity);
        if (EditorGUI.EndChangeCheck())
        {
            OnEditorFrameChange();
        }


        EditorGUILayout.PropertyField(_Loop);

        //EditorGUILayout.PropertyField(_IsAtlas);
        //EditorGUILayout.PropertyField(_IsPool);

        EditorGUILayout.PropertyField(_PlayOnStart);

        EditorGUILayout.Separator();
        ExtendedOptions();
        EditorGUILayout.Separator();



        _mode = (AESettingsMode)EditorGUILayout.EnumPopup("Mode", anim.mode);
        foreach (Object t in targets)
        {
            (t as AfterEffectAnimation).mode = _mode;
        }

        if (anim.mode == AESettingsMode.Advanced)
        {

            EditorGUILayout.PropertyField(_IsForceSelected, new GUIContent("Force Selection"));



            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Slider(_pivotCenterX, 0f, 1f);
            EditorGUILayout.Slider(_pivotCenterY, 0f, 1f);


            if (EditorGUI.EndChangeCheck())
            {
                OnPivotPositionChnage();
            }


            EditorGUI.BeginChangeCheck();
            anim.normal_mode_shader = EditorGUILayout.Popup("Normal Mode Shader", anim.normal_mode_shader, AEShaders.importedShaders);
            anim.add_mode_shader = EditorGUILayout.Popup("Add Mode Shader", anim.add_mode_shader, AEShaders.importedShaders);
            if (EditorGUI.EndChangeCheck())
            {
                ReloadAnimation();
            }
        }

        EditorGUILayout.Separator();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(new GUIContent("Update"), GUILayout.Width(100)))
        {
            ReloadAnimation();
        }

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

    }


    public List<TextAsset> atlasFileList = new List<TextAsset>();
    public List<Texture> atlasTextureList = new List<Texture>();

    private void Atlas_TextTexture()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Atlas Count");
        _target.atlasCount = EditorGUILayout.IntSlider(_target.atlasCount, 0, 10);
        EditorGUILayout.EndHorizontal();



        if (_target.atlasCount > _target.atlasFileList.Count)
        {
            for (int i = 0; i < _target.atlasCount - _target.atlasFileList.Count; i++)
            {
                _target.atlasFileList.Add(null);
                _target.atlasTextureList.Add(null);
            }
        }

        if (_target.atlasCount < _target.atlasFileList.Count)
        {
            int removeCount = _target.atlasFileList.Count - _target.atlasCount;
            _target.atlasFileList.RemoveRange(_target.atlasFileList.Count - removeCount, removeCount);
            _target.atlasTextureList.RemoveRange(_target.atlasTextureList.Count - removeCount, removeCount);
        }

        if (atlasFileList.Count != _target.atlasFileList.Count)
        {
            atlasFileList = new List<TextAsset>();
            atlasTextureList = new List<Texture>();

            for (int i = 0; i < _target.atlasFileList.Count; i++)
            {
                if (_target.atlasFileList[i] == null)
                {
                    atlasFileList.Add(null);
                }
                else
                    atlasFileList.Add(_target.atlasFileList[i]);

                if (_target.atlasTextureList[i] == null)
                {
                    atlasTextureList.Add(null);
                }
                else
                    atlasTextureList.Add(_target.atlasTextureList[i]);
            }
        }

        for (int i = 0; i < _target.atlasFileList.Count; i++)
        {
            atlasFileList[i] = (TextAsset)EditorGUILayout.ObjectField("Altas TextAsset " + (i + 1), atlasFileList[i], typeof(TextAsset), true);
            atlasTextureList[i] = (Texture)EditorGUILayout.ObjectField("Altas Texture " + (i + 1), atlasTextureList[i], typeof(Texture), true);
        }

        //if (GUILayout.Button(new GUIContent("更新图集"), GUILayout.Width(100)))
        //{
        //    ChangeToAtlas();
        //}

        if (GUILayout.Button(new GUIContent("删除图集数据"), GUILayout.Width(100)))
        {
            //_target.atlasCount = 0;
            for (int i = 0; i < _target.atlasFileList.Count; i++)
            {
                _target.atlasFileList[i] = null;
                atlasFileList[i] = null;
            }
        }

        ExportScriptableObjectData();
    }

    private void ChangeToAtlas()
    {
        _target.atlasFileList = atlasFileList;
        _target.atlasTextureList = atlasTextureList;
        LoadData();
        PackParse_AE.PackAE_Atlas(_target);
        exportSp = ScriptableObject.CreateInstance("IAESpriteScriptable") as IAESpriteScriptable;
        exportSp.spAEFootage = tmpAEScriptable.spAEFootage;
        string strData = _target.strExportDataFolder + _target.dataFileName + ".asset";
        AssetDatabase.CreateAsset(exportSp, strData);
    }

    private IAESpriteScriptable exportSp;
    private void ExportBeign()
    {
        exportSp = ScriptableObject.CreateInstance("IAESpriteScriptable") as IAESpriteScriptable;
    }

    private void ExportAddAEFootage(AEFootage sp)
    {
        //sp.gameObject = null;
        //sp.transform = null;
        exportSp.spAEFootage.Add(sp);
    }


    private void ExportAddAEComposition(AEComposition sp)
    {
        Debug.Log("暂未实现!");
    }

    private void ExportFinish()
    {

        for (int i = 0; i < exportSp.spAEFootage.Count; i++)
        {
            AESprite sp = exportSp.spAEFootage[i];
            sp.gameObject = null;
            sp.transform = null;
            sp.plane = null;
            sp.material = null;
        }



        string strData = _target.strExportDataFolder + _target.dataFileName + ".asset";

        Debug.Log("ExportFinish:" + strData + exportSp.spAEFootage.Count);
        AssetDatabase.CreateAsset(exportSp, strData);
    }


    private void ExportScriptableObjectData()
    {
        if (GUILayout.Button(new GUIContent("加载数据 "), GUILayout.Width(100)))
        {
            //if (!_target.IsHasExportData)
            //{
            //    Debug.LogError("没有数据加载!");
            //    return;
            //}

            LoadData();
        }
    }

    private IAESpriteScriptable tmpAEScriptable;
    private void LoadData()
    {
        string strData = _target.strExportDataFolder + _target.dataFileName + ".asset";
        Object o = AssetDatabase.LoadAssetAtPath(strData, typeof(AESpriteScriptable));
#if test
        IAESpriteScriptable tmp = o as IAESpriteScriptable;
        tmpAEScriptable = new IAESpriteScriptable();
        tmpAEScriptable.spAEFootage = IAESpriteScriptable.CloneData(tmp);
        Debug.Log("InScriptableObjectData: " + tmpAEScriptable.spAEFootage.Count);

        Debug.Log("hash code : " + this.GetHashCode());
#else
        tmpAEScriptable = o as IAESpriteScriptable;
#endif
        _target.sprites.Clear();
        _target.InitFootageSprite(tmpAEScriptable.spAEFootage);
        _target.Stop();
        _target.GoToAndPlay(0);
    }

    //--------------------------------------
    // GET / SET
    //--------------------------------------

    public AfterEffectAnimation anim
    {
        get
        {
            return target as AfterEffectAnimation;
        }
    }

    //--------------------------------------
    // EVENTS
    //--------------------------------------


    //--------------------------------------
    // PRIVATE METHODS
    //--------------------------------------

    private void OnPivotPositionChnage()
    {
        serializedObject.ApplyModifiedProperties();
        foreach (Object t in targets)
        {
            (t as AfterEffectAnimation).OnPivotPositionChnage();
        }
    }

    private void OnEditorFrameChange()
    {
        serializedObject.ApplyModifiedProperties();

        foreach (Object t in targets)
        {
            (t as AfterEffectAnimation).OnEditorFrameChange();
        }
    }

    protected virtual void ReloadAnimation()
    {
        serializedObject.ApplyModifiedProperties();

        if (!CheckResourcesEnough()) return;

        bool isNormal = true;

        try
        {
            _target.IsAtlas = false;
            _target.isPool = false;
            _target.OnAnimationDataChange();
        }
        catch (System.Exception ex)
        {
            isNormal = false;
            Debug.Log(ex.Message);
        }

        if (isNormal)
        {
            ChangeToAtlas();

            _target.IsAtlas = true;
            _target.isPool = true;
            List<Transform> _childs = new List<Transform>();
            Transform root = _target.transform.FindChild("SpritesHolder");

            foreach (Transform child in root)
            {
                _childs.Add(child);
            }

            foreach (Transform c in _childs)
            {
                DestroyImmediate(c.gameObject);
            }

            string strPathWithOutAsset = AEFootage.strPath + _target.imagesFolder + _target.dataFileName + "/";
            //string strAssetPath = "Assets/" + strPathWithOutAsset;
            string strFolder = Application.dataPath + "/" + strPathWithOutAsset;

            if (System.IO.Directory.Exists(strFolder)) System.IO.Directory.Delete(strFolder);
        }
    }

    protected virtual void ExtendedOptions()
    {

    }


    private bool CheckResourcesEnough()
    {
        if (atlasFileList.Count == 0 ||
            atlasTextureList.Count == 0 ||
            atlasTextureList.Count != atlasFileList.Count)
        {
            Debug.LogError("图集数据出错!");
            return false;
        }

        for (int i = 0; i < atlasFileList.Count; i++)
        {
            if (atlasFileList[i] == null || atlasTextureList[i] == null)
            {
                Debug.LogError("图集数据出错!");
                return false;
            }
        }

        return true;
    }

}
