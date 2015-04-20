using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class ExportAssetBundles
{
    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
    static void ExportResource()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path,
                              BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
            Selection.objects = selection;
        }
    }

    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies【Android】")]
    static void ExportResourceAndroid()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path,
                              BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.Android);
            Selection.objects = selection;
        }
    }

    [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies【IOS】")]
    static void ExportResourceIOS()
    {
        // Bring up save panel
        string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
        if (path.Length != 0)
        {
            // Build the resource file from the active selection.
            Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path,
                              BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.iPhone);
            Selection.objects = selection;
        }
    }

    //[MenuItem("Plugin/Asset/Build AssetBundle From Selection - No dependency tracking")]
    //static void ExportResourceNoTrack()
    //{
    //    // Bring up save panel
    //    string path = EditorUtility.SaveFilePanel("Save Resource", "", "New Resource", "unity3d");
    //    if (path.Length != 0)
    //    {
    //        // Build the resource file from the active selection.
    //        BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path);
    //    }
    //}
    [MenuItem("2D Tools/____OneKeyExport____")]
    public static void ExportTextureFolderAll()
    {
        if (string.IsNullOrEmpty(ToolSetting.LocalTargetPath))
        {
            UnityEditor.EditorUtility.DisplayDialog("Msg", "ToolSetting.LocalTargetPath is null", "OK");
            return;
        }
        string[] strAtlas = Directory.GetFiles(Application.dataPath + @"/Artworks/Atlas/", "*.png");
        string strFileName = "";
        UnityEngine.Object o = null;
        for (int i = 0; i < strAtlas.Length; i++)
        {
            strFileName = Path.GetFileName(strAtlas[i]);
            UnityEditor.EditorUtility.DisplayProgressBar("Export process", strFileName + ".u", (float)i / strAtlas.Length);
            strFileName = strFileName.Substring(0, strFileName.IndexOf('.'));
            o = Resources.LoadAssetAtPath(@"Assets/Artworks/Atlas/" + strFileName + ".png", typeof(UnityEngine.Object));
            BuildPipeline.BuildAssetBundle(o, null, ToolSetting.LocalTargetPath + "\\NGUI\\" + strFileName + ".u", BuildAssetBundleOptions.CollectDependencies, EditorUserBuildSettings.activeBuildTarget);
        }

        Debug.Log("@@图集打包完成");

        for (int i = 0; i < fList.Length; i++)
        {
            string folder = fList[i];
            ++i;
            string tName = fList[i];
            UnityEditor.EditorUtility.DisplayProgressBar("Export process", ((i + 1) / 2) + "/" + (fList.Length / 2) + "              " + tName, (i + 1.0f) / fList.Length);
            exportObjectsByPath(Application.dataPath + folder, ToolSetting.LocalTargetPath + tName.Replace("/", new string(Path.DirectorySeparatorChar, 1)));
        }
        //ExportAEForder();
        UnityEditor.EditorUtility.ClearProgressBar();
    }

    private static string[] fList = new string[] { "/NUIPrefab", "Icon/ui.u" };
    private static void exportObjectsByPath(string path, string targetPath)
    {
        if (path.IndexOf(".") == -1)
        {
            List<UnityEngine.Object> oList = new List<UnityEngine.Object>();
            string[] list = System.IO.Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < list.Length; i++)
            {
                string f = list[i];
                if (f.EndsWith(".prefab") || f.EndsWith(".png") || f.EndsWith(".jpg"))
                {
                    f = f.Substring(f.IndexOf("/Assets") + 1);
                    UnityEngine.Object o = Resources.LoadAssetAtPath(f, typeof(UnityEngine.Object));
                    oList.Add(o);
                }
            }
            Object[] selections = null;
            if (targetPath.IndexOf(".") == -1)//非去图集
            {
                selections = oList.ToArray();
                for (int i = 0; i < selections.Length; i++)
                {
                    UnityEngine.Object o = selections[i];
                    Debug.LogError(targetPath + Path.DirectorySeparatorChar + o.name + ".u");
                    BuildPipeline.BuildAssetBundle(o, null, targetPath + Path.DirectorySeparatorChar + o.name + ".u", BuildAssetBundleOptions.CollectDependencies, EditorUserBuildSettings.activeBuildTarget);
                }
            }
            else//去图集
            {
                ExportDependency.CopyFolderPrefab(oList.ToArray(), targetPath.Replace(new string(Path.DirectorySeparatorChar, 1), "/"));
                ExportDependency.RemoveMaterialTexReference();
                ExportDependency.ApplyToPrefab();
                selections = ExportDependency.GetNewPrefabs();
                Debug.LogError(targetPath);

                if (selections.Length > 0)
                {
                    ExportDependency.ReMoveALLTexture(selections);
                    BuildPipeline.BuildAssetBundle(null, selections, targetPath, BuildAssetBundleOptions.CollectDependencies, EditorUserBuildSettings.activeBuildTarget);
                    ExportDependency.RevertTexture();
                }
                ExportDependency.RecoverMaterialTexReference();
            }
        }
        else
        {
            string f = path.Substring(path.IndexOf("/Assets") + 1);
            UnityEngine.Object o = Resources.LoadAssetAtPath(f, typeof(UnityEngine.Object));
            BuildPipeline.BuildAssetBundle(o, null, targetPath, BuildAssetBundleOptions.CollectDependencies, EditorUserBuildSettings.activeBuildTarget);
        }


        Debug.Log("EditorUserBuildSettings.activeBuildTarget:   " + EditorUserBuildSettings.activeBuildTarget);
    }


    [MenuItem("2D Tools/____GenAtlas____")]
    public static void GenAtlasFolderAll()
    {
        List<UIAtlasMaker.SpriteEntry> lstEntry = new List<UIAtlasMaker.SpriteEntry>();
        Dictionary<string, UIAtlas> dictAtlasAndSprite = new Dictionary<string, UIAtlas>();

        //找出所有的图集
        Object[] mObjects = Resources.FindObjectsOfTypeAll(typeof(UIAtlas));

        List<Texture> textures = new List<Texture>();

        string fromFolder = Application.dataPath + @"/Artworks/Texture/";

        FileInfo tmp = null;
        foreach (DirectoryInfo d in new System.IO.DirectoryInfo(fromFolder).GetDirectories())
        {
            textures.Clear();
            lstEntry.Clear();
            
            string[] strPngs = Directory.GetFiles(Application.dataPath + @"/Artworks/Texture/" + d.Name + @"/" ,  "*.png");
            string strFileName = "";
            UnityEngine.Object o = null;
            for (int i = 0; i < strPngs.Length; i++)
            {
                strFileName = Path.GetFileName(strPngs[i]);
                UnityEditor.EditorUtility.DisplayProgressBar("Process pic", d.Name + "/" + strFileName, (float)i / strPngs.Length);
                strFileName = strFileName.Substring(0, strFileName.IndexOf('.'));
                o = Resources.LoadAssetAtPath(@"Assets/Artworks/Texture/" + d.Name + @"/" + strFileName + ".png", typeof(UnityEngine.Object));

                Texture tex = o as Texture;
                if (tex == null || tex.name == "Font Texture") continue;

                textures.Add(tex);
            }

            //有图片才更新图集
            if (textures.Count > 0)
            {
                Object atlasobj = null;
                string strAtlasPath = @"Assets/Artworks/Atlas/";

                string strAtlasPrefabPath = strAtlasPath + d.Name.ToLower() + ".prefab";

                
                GameObject goAtlas = AssetDatabase.LoadAssetAtPath(strAtlasPrefabPath, typeof(GameObject)) as GameObject;

                if (goAtlas != null)
                {
                    UIAtlas atlas = goAtlas.GetComponent<UIAtlas>();

                    if (atlas == null)
                        continue;

                    NGUISettings.atlas = atlas;
                    NGUISettings.trueColorAtlas = true;
                    NGUISettings.unityPacking = true;
                    NGUISettings.atlasTrimming = true;

                    lstEntry = UIAtlasMaker.CreateSprites(textures);
                    UIAtlasMaker.UpdateAtlas(atlas, lstEntry);

                    Debug.Log(d.Name + "图集生成完成");
                }
                else//生成新的图集
                {
                    //lstEntry = UIAtlasMaker.CreateSprites(textures);
                    //if (lstEntry.Count > 0)
                    {
                        Shader shader = Shader.Find(NGUISettings.atlasPMA ? "Unlit/Premultiplied Colored" : "Unlit/Transparent Colored");
                        Material atlasMat = new Material(shader);

                        string strAtlasMatPath = strAtlasPath + d.Name.ToLower() + ".mat";
                        AssetDatabase.CreateAsset(atlasMat, strAtlasMatPath);
                        AssetDatabase.Refresh();

                        atlasMat = AssetDatabase.LoadAssetAtPath(strAtlasMatPath, typeof(Material)) as Material;
                        strAtlasPrefabPath = strAtlasPath + d.Name.ToLower() + ".prefab";
                        Object atlasPrefab = PrefabUtility.CreateEmptyPrefab(strAtlasPrefabPath);

                        goAtlas = new GameObject(d.Name.ToLower());
                        UIAtlas atlas = goAtlas.AddComponent<UIAtlas>();
                        atlas.spriteMaterial = atlasMat;

                        PrefabUtility.ReplacePrefab(goAtlas, atlasPrefab);
                        GameObject.DestroyImmediate(goAtlas);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        goAtlas = AssetDatabase.LoadAssetAtPath(strAtlasPrefabPath, typeof(GameObject)) as GameObject;
                        atlas = goAtlas.GetComponent<UIAtlas>();

                        if (atlas != null)
                        {
                            NGUISettings.atlas = atlas;
                            NGUISettings.trueColorAtlas = true;
                            NGUISettings.unityPacking = true;
                            NGUISettings.atlasTrimming = true;

                            lstEntry = UIAtlasMaker.CreateSprites(textures);
                            UIAtlasMaker.UpdateAtlas(atlas, lstEntry);
                            Debug.Log(d.Name + "图集生成完成");
                        }
                    }

                }
            }
        }
    }

}