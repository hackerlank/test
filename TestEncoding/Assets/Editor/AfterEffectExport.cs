using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class AfterEffectExport : EditorWindow
{
    [MenuItem("2D Tools/Refresh All AE")]
    [MenuItem("Assets/Refresh All AE")]
 public   static void RefreshAllAE()
    {

        string strFolder = "/AfterEffect/NewPerfab/commonAE";

        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + strFolder);
        FileInfo[] fileInfo = dir.GetFiles("*.prefab", SearchOption.AllDirectories);

        int i = 0;
        foreach (FileInfo f in fileInfo)
        {
            int index = f.FullName.LastIndexOf("Assets");
            string strPath = f.FullName.Substring(index);

            Object obj = AssetDatabase.LoadMainAssetAtPath(strPath);
            GameObject gameObject = GameObject.Instantiate(obj) as GameObject;

            if (EditorUtility.DisplayCancelableProgressBar("刷新AE数据ing  " + (i) + "/" + fileInfo.Length, "AE名:" + strPath, (float)(i) / fileInfo.Length))
                break;

            i++;
            _target = gameObject.GetComponent<AfterEffectAnimation>();
            if (_target == null)
            {
                Debug.LogWarning(strPath + " 不是AE特效");
                GameObject.DestroyImmediate(gameObject, true);
                continue;
            }

            _target.dataFile = AutoLoadTxt(_target.dataFileName);

            _target.exportActionBegin = ExportBeign;
            _target.exportActionAddAEFootage = ExportAddAEFootage;
            _target.exportActionAddAEComposition = ExportAddAEComposition;
            _target.exportActionFinish = ExportFinish;
            ReloadAnimation(_target);

            GameObject.DestroyImmediate(gameObject, true);
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.Refresh();
    }

    private static TextAsset AutoLoadTxt(string strName)
    {
        TextAsset ta = AssetDatabase.LoadAssetAtPath("Assets/AfterEffect/AEImages/NewAE/" + strName + ".xml", typeof(TextAsset)) as TextAsset;
        if (ta == null)
        {
            ta = AssetDatabase.LoadAssetAtPath("Assets/AfterEffect/AEImages/skillLayer/skillLayer_Art/" + strName + ".xml", typeof(TextAsset)) as TextAsset;
        }
        return ta;
    }

    private static AfterEffectAnimation _target;
    private static void ReloadAnimation(AfterEffectAnimation _target)
    {
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
            Debug.LogError(ex.Message);
        }

        if (isNormal)
        {
            ChangeToAtlas(_target);

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
            string strAssetPath = "Assets/" + strPathWithOutAsset;
            string strFolder = Application.dataPath + "/" + strPathWithOutAsset;

            if (System.IO.Directory.Exists(strFolder)) System.IO.Directory.Delete(strFolder);
        }
    }

    private static IAESpriteScriptable exportSp;
    private static void ExportBeign()
    {
        exportSp = ScriptableObject.CreateInstance("IAESpriteScriptable") as IAESpriteScriptable;
    }

    private static void ExportAddAEFootage(AEFootage sp)
    {
        //sp.gameObject = null;
        //sp.transform = null;
        exportSp.spAEFootage.Add(sp);
    }


    private static void ExportAddAEComposition(AEComposition sp)
    {
        Debug.Log("暂未实现!");
    }

    private static void ExportFinish()
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

        //Debug.Log("ExportFinish:" + strData + exportSp.spAEFootage.Count);
        AssetDatabase.CreateAsset(exportSp, strData);
    }

    private static void ChangeToAtlas(AfterEffectAnimation _target)
    {
        IAESpriteScriptable tmpAEScriptable = LoadData(_target);
        PackParse_AE.PackAE_Atlas(_target);
        IAESpriteScriptable exportSp = ScriptableObject.CreateInstance("IAESpriteScriptable") as IAESpriteScriptable;
        exportSp.spAEFootage = tmpAEScriptable.spAEFootage;
        string strData = _target.strExportDataFolder + _target.dataFileName + ".asset";

        Debug.Log(strData);

        AssetDatabase.CreateAsset(exportSp, strData);
    }


    private static IAESpriteScriptable LoadData(AfterEffectAnimation _target)
    {
        string strData = _target.strExportDataFolder + _target.dataFileName + ".asset";
        Object o = AssetDatabase.LoadAssetAtPath(strData, typeof(AESpriteScriptable));
        return o as IAESpriteScriptable;
    }

}