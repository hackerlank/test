using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;


public class ExportDependency : EditorWindow
{

    #region 图片名字
    private static List<string> allTextures = new List<string>()
    {"Assets/LuanDunUI/Atlas/Tex_new/effectSecret_jpg.jpg",
	"Assets/LuanDunUI/Atlas/Tex_new/effectSecret_png.png",
    "Assets/LuanDunUI/Atlas/Tex_new/new_map2.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newMainBack.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newSecret.png",
    "Assets/LuanDunUI/Atlas/Tex_new/new_map_2.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newMap3.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newShop.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newSecret_collection.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newSecret_treasure.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newMain.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newSkillUp.png",
    "Assets/LuanDunUI/Atlas/Tex_new/newSecret_treasure111.png",
    };

    private static string GetTextureName(string strName)
    {
        for (int i = 0; i < allTextures.Count; i++)
        {
            string strItem = allTextures[i];
            if (strItem.Contains(strName))
            {
                int nItem = strItem.LastIndexOf("/");

                return strItem.Substring(nItem + 1);
            }
        }

        return "";
    }

    #endregion

    #region 图片的删除添加
    public static void RemoveMaterialTex()//移除材质图片
    {

        for (int i = 0; i < allTextures.Count; i++)
        {
            string strTex = allTextures[i];
            AssetDatabase.CopyAsset(strTex, strTex.Replace(".", "_tmp."));

            AssetDatabase.DeleteAsset(strTex);

        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
    public static void RecoverMaterialTex()// 恢复材质图片
    {
        for (int i = 0; i < allTextures.Count; i++)
        {
            string strTex = allTextures[i];

            string strNewTex = strTex.Replace(".", "_tmp.");

            AssetDatabase.CopyAsset(strNewTex, strTex);

            AssetDatabase.DeleteAsset(strNewTex);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }    
    #endregion

    #region 新建prefab文件夹
    private static string strNewFolder = "/TmpPrefab";
    private static string strSelectFolder = Application.dataPath + strNewFolder;
    private static void CreateSelectFolder()
    {
        if (System.IO.Directory.Exists(strSelectFolder))
            System.IO.Directory.Delete(strSelectFolder,true);
        System.IO.Directory.CreateDirectory(strSelectFolder);

       
    }


    private static Dictionary<GameObject, List<GameObject>> allCloneObj = new Dictionary<GameObject, List<GameObject>>();
    private static Dictionary<GameObject, List<UIWidget>> allCloneWidget = new Dictionary<GameObject, List<UIWidget>>();

    private static List<Object> selectPrefabs = new List<Object>();

    public static void CopyFolderPrefab(Object[] selection,string strPackPath)//把当前选中的prefab复制到新的文件夹 并且保存disactive的prefab
    {
        selectPrefabs.Clear();
        allCloneObj.Clear();
        allCloneWidget.Clear();

        CreateSelectFolder();

        BeginScriptable(strPackPath.Substring(strPackPath.LastIndexOf("/") + 1).Replace(".u", ""));

        SetMaterialTexReference();//获取材质与图片的引用关系


        AssetDatabase.Refresh();

        for (int i = 0; i < selection.Length; i++)
        {
            string strPath = AssetDatabase.GetAssetPath(selection[i]);

            if (!strPath.Contains(".prefab")) continue;

            string strNewPath = "Assets" + strNewFolder + strPath.Substring(strPath.LastIndexOf("/"));

            AssetDatabase.CopyAsset(strPath, strNewPath);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();

            Object newPrefab = AssetDatabase.LoadMainAssetAtPath(strNewPath);

            SetObjectDependency(newPrefab);

            selectPrefabs.Add(newPrefab);

            Debug.Log(strPath+"   "+strNewPath);

            AddActiveGameObject(PrefabUtility.InstantiatePrefab(newPrefab) as GameObject, newPrefab);
        }
    }


    private static void AddActiveGameObject(GameObject obj, Object prefab)//设置当前不显示的组件
    {

        Transform[] allTransform = obj.GetComponentsInChildren<Transform>(true);

        List<GameObject> disActive = new List<GameObject>();

        foreach (Transform child in allTransform)
        {
            GameObject ga = child.gameObject;

            if (ga.activeSelf) continue;

            disActive.Add(ga);
            ga.SetActive(true);
        }

        UIWidget[] allWidget = obj.GetComponentsInChildren<UIWidget>(true);
        List<UIWidget> enables = new List<UIWidget>();

        foreach (UIWidget child in allWidget)
        {

            if (child.enabled) continue;

            child.enabled = true;

            enables.Add(child);
        }

        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);

        allCloneObj.Add(obj, disActive);

        allCloneWidget.Add(obj, enables);

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static void ApplyToPrefab()//数据 保存到prefab
    {
        int nIndex = 0;
        foreach (KeyValuePair<GameObject, List<GameObject>> key in allCloneObj)
        {
            for (int i = 0; i < key.Value.Count; i++)
            {
                key.Value[i].SetActive(false);
            }

            List<UIWidget> allWidget = allCloneWidget[key.Key];

            for (int i = 0; i < allWidget.Count; i++)
            {
                allWidget[i].enabled = false;
            }

            PrefabUtility.ReplacePrefab(key.Key, selectPrefabs[nIndex], ReplacePrefabOptions.ConnectToPrefab);

            nIndex++;

            GameObject.DestroyImmediate(key.Key);
        }

        EndScriptable();

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }


    public static Object[] GetNewPrefabs()//获取新产生的prefab
    {
        Object configPrefab = AssetDatabase.LoadMainAssetAtPath(strData);//配置prefab

        selectPrefabs.Add(configPrefab);

        return selectPrefabs.ToArray();
    }


    #endregion

    public static void SetObjectDependency(Object _Object)//获取该prefab的引用图片
    {
        Object[] objs = EditorUtility.CollectDependencies(new Object[] { _Object });

        if (objs.Length == 0) return;

        NGUIConfig_Data config = new NGUIConfig_Data();
        config.strName = _Object.name;

        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];

            if (obj == null)
            {
               continue;
            }

            if (obj.GetType() != typeof(Material)) continue;

            Material mat = obj as Material;

            if (mat.mainTexture == null) continue;

            if (!CheckMaterial(mat.mainTexture)) continue;
            Debug.LogError(mat.name);
            NGUIConfig_Property property = new NGUIConfig_Property();

            property.strMaterialName = mat.name;

            property.strMaterialTex = mat.mainTexture.name;

            config.property.Add(property);
        }

        Add_NGUIConfig_Data(config);
    }


    #region 材质的图片引用关系
    private static string[] strMatListFolder = new string[] 
    {
       "Artworks/Atlas",
    };

    private static FileInfo[] GetAllFileInfo()
    {
        List<FileInfo> allFileInfo = new List<FileInfo>();

        for (int i = 0; i < strMatListFolder.Length; i++)
        {
            DirectoryInfo dir = new DirectoryInfo(Application.dataPath+"/" + strMatListFolder[i]);

            FileInfo[] fileInfo = dir.GetFiles("*.mat", SearchOption.AllDirectories);

            allFileInfo.AddRange(fileInfo);
        }

        return allFileInfo.ToArray();
    }


    private static Dictionary<string, Texture> allMaterialTex = new Dictionary<string, Texture>();
    private static bool CheckMaterial(Texture tex)
    {
        foreach (KeyValuePair<string, Texture> key in allMaterialTex)
        {
            if (key.Value == tex)
                return true;
        }

        return false;
    }


    private static string[] SameNameTexName = new string[]
    {"Atlas/newMainBack/newMainBack.png",
    "Atlas/newMain/newMain.png",
    "Atlas/Fight/Fight.png",
    "Atlas/newLogin/newLogin.png",
    "Atlas/newShop/newShop.png",
    "Atlas/Common/Common.png",
    "NGUI_WarMap/Texture/",
    "NGUI_WorldMap/Texture/",
    "NGUI_SecretGame/Texture/",
    "Resources/",
    "StreamingAssets/",
    };

    static Dictionary<UITexture, Texture> TexMap = new Dictionary<UITexture, Texture>();
    public static void ReMoveALLTexture(Object[] Uobjects)
    {
        TexMap.Clear();
        List<string> NotReMove=new List<string>();
        NotReMove.Add("NGUI_NewMap");
        NotReMove.Add("NGUI_SecretMap");
        foreach (var obj in Uobjects)
        {
            if (NotReMove.Contains(obj.name)) continue;
            if (obj is GameObject)
            {


                ReMoveTexture(obj as GameObject);
                Debug.LogError("GameObject-->" + obj.name + TexMap.Count);
            }
        }
     //   AssetDatabase.Refresh();
      //  AssetDatabase.SaveAssets();
    }

    public static void ReMoveTexture(GameObject obj) 
    {
      UITexture tex  =obj.GetComponent<UITexture>();
      if (tex != null)
      { 
          
          
          TexMap[tex] = tex.mainTexture;
          Texture none = new Texture();
          tex.mainTexture = none;
      }
     
        for (int i = 0; i < obj.transform.childCount; i++) 
        {
            GameObject chobj=obj.transform.GetChild(i).gameObject;
            ReMoveTexture(chobj);
        
        }


  

    }
    public static void RevertTexture()
    {
        foreach (var item in TexMap)
        {
            item.Key.mainTexture = item.Value;

        }

    }

    public static void ReNameTex() //防止图片被打进AssetBundle的图先重命名
    {
       
        List<string> Fileist = new List<string>();
        foreach (string sp in SameNameTexName)
        {
            string path = Application.dataPath + "/" + sp;
            if (File.Exists(path))
            {
                if (!Fileist.Contains(path))
                {
                    Fileist.Add(path);
                }
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] pngfiles = dir.GetFiles("*.png", SearchOption.AllDirectories);
                FileInfo[] jpgfiles = dir.GetFiles("*.jpg", SearchOption.AllDirectories);
                foreach (var file in pngfiles)
                {
                    if (!file.FullName.Contains("*.meta"))
                    if (!file.FullName.Contains("*.tmp"))
                    if (!Fileist.Contains(file.FullName))
                        Fileist.Add(file.FullName);
                }
                foreach (var file in jpgfiles)
                {
                    if (!file.FullName.Contains("*.meta"))
                    if (!file.FullName.Contains("*.tmp"))
                    if (!Fileist.Contains(file.FullName))
                        Fileist.Add(file.FullName);
                }
            }

        }
    
        foreach (string path in Fileist)
        {
            string fp = path;
            string tp = "";
            if (path.Contains(".png"))
            {
                tp = path.Replace(".png", ".png.tmp");
            }
            else if (path.Contains(".png"))
            {
                tp = path.Replace(".jpg", ".jpg.tmp");
            }

            Debug.Log(tp);


            if (File.Exists(fp))
            {
                if (File.Exists(tp))
                {

                    File.Delete(tp);
                }

                try
                {
                    File.Move(fp, tp);
                }
                catch(System.Exception err) 
                {

                    Debug.LogError(err);
                }
            }


        }
       
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }
    public static void RevertTex()
    {
        List<string> Fileist = new List<string>();
        foreach (string sp in SameNameTexName)
        {
            string path = Application.dataPath + "/" + sp;
            if (File.Exists(path+".tmp"))
            {
                if (!Fileist.Contains(path + ".tmp"))
                {
                    Fileist.Add(path + ".tmp");
                }
            }
            else if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FileInfo[] tmpfiles = dir.GetFiles("*.tmp", SearchOption.AllDirectories);

                foreach (var file in tmpfiles)
                {
                    if (!file.FullName.Contains("*.meta"))
                    if (!Fileist.Contains(file.FullName))
                        Fileist.Add(file.FullName);
                }
         
            }

        }
        foreach (string path in Fileist)
        {
            string tp = "";
            string fp = path;
            if (path.Contains(".png.tmp"))
            {
                tp = path.Replace(".png.tmp", ".png");
            }
            else if (path.Contains(".jpg.tmp"))
            {
                tp = path.Replace(".jpg.tmp", ".jpg");
            }

            if (File.Exists(fp))
            {
                Debug.Log(tp);
                File.Move(fp, tp);
            }


        }
        
    
    }
    public static void SetMaterialTexReference()//获取材质引用图片
    {
        allMaterialTex.Clear();

        FileInfo[] fileInfo = GetAllFileInfo();

        foreach (FileInfo f in fileInfo)
        {
            int index = f.FullName.LastIndexOf("Assets");
            string strPath = f.FullName.Substring(index);

            Material mat = AssetDatabase.LoadMainAssetAtPath(strPath) as Material;

            allMaterialTex.Add(strPath, mat.mainTexture);
        }

    }



    public static void RemoveMaterialTexReference()//移除材质图片
    {
        FileInfo[] fileInfo=GetAllFileInfo();

        foreach (FileInfo f in fileInfo)
        {
            int index = f.FullName.LastIndexOf("Assets");
            string strPath = f.FullName.Substring(index);

            Material mat = AssetDatabase.LoadMainAssetAtPath(strPath) as Material;

            mat.mainTexture = null;
        }


        AssetDatabase.Refresh();
    }


    public static void RecoverMaterialTexReference()//恢复材质图片
    {
        FileInfo[] fileInfo = GetAllFileInfo();

        foreach (FileInfo f in fileInfo)
        {
            int index = f.FullName.LastIndexOf("Assets");
            string strPath = f.FullName.Substring(index);

            Material mat = AssetDatabase.LoadMainAssetAtPath(strPath) as Material;

            if (allMaterialTex.ContainsKey(strPath))
            {
                mat.mainTexture = allMaterialTex[strPath];
            }
        }

        allMaterialTex.Clear();

        if (System.IO.Directory.Exists(strSelectFolder))
            System.IO.Directory.Delete(strSelectFolder, true);

        AssetDatabase.Refresh();
    }

    #endregion

    #region 产生NGUI配置
    private static NGUIConfig exportCfg;
    private static string strData = "Assets/StreamingAssets/NGUI/NGUIConfig.asset";
    public static void BeginScriptable(string strPackName)
    {
   
        strData = "Assets/StreamingAssets/NGUI/" + strPackName + "_config.asset";
        
        string strFolder = Application.streamingAssetsPath + "/NGUI";
        if (!System.IO.Directory.Exists(strFolder)) System.IO.Directory.CreateDirectory(strFolder);

        exportCfg = ScriptableObject.CreateInstance("NGUIConfig") as NGUIConfig;

    }
    public static void Add_NGUIConfig_Data(NGUIConfig_Data dat)
    {
        for (int i = 0; i < exportCfg.UIConfig.Count; i++)
        {
            NGUIConfig_Data itm = exportCfg.UIConfig[i];
            if (itm.strName != dat.strName) continue;
            itm.property = dat.property;
            return;
        }
        string log = "<---propertylist-->";
        foreach (var prop in dat.property) 
        {

            log += "\nMaterial:" + prop.strMaterialName + "  Tex:" + prop.strMaterialTex;
        }
        Debug.Log("Add_NGUIConfig_Data:  " + dat.strName + " Count:" + dat.property.Count + log);

        exportCfg.UIConfig.Add(dat);
    }

    public static void EndScriptable()
    {
        string strFolder = Application.streamingAssetsPath + "/NGUI";
        if (!System.IO.Directory.Exists(strFolder)) System.IO.Directory.CreateDirectory(strFolder);
        string log = "<---EndScriptable----Count:" + exportCfg.UIConfig.Count + "-->";
            foreach(var UIConfig in exportCfg.UIConfig)
            {
            log+="\n"+UIConfig.strName;
            }
        Debug.Log(log);

        AssetDatabase.CreateAsset(exportCfg, strData);
    }
    #endregion
}