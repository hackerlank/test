using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

public class ToolSetting : EditorWindow
{
    public class FileTmp
    {
        public string FileName;
        public string Path;
        public string Size;
        public string ModifyTime;
        public string MD5;
    }
    private static ToolSetting window;
    private static string localAssetPathAndroidKey = "assetAndroidPath", localAssetPathWebKey = "assetWebPath", localAssetPathIphoneKey = "assetIphonePath";
    [MenuItem("Build/Tool Setting")]
    public static void ExportItemsToData()
    {
        window = EditorWindow.GetWindow(typeof(ToolSetting), false, "Set Export Path", true) as ToolSetting;
        window.position = new Rect((Screen.currentResolution.width - 400) / 2, (Screen.currentResolution.height - 380) / 2, 400, 380);
        localPathAndroid = GetString(localAssetPathAndroidKey);
        localPathWeb = GetString(localAssetPathWebKey);
        localPathIphone = GetString(localAssetPathIphoneKey);
    }
    private static MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
    [MenuItem("Build/Generate Flist")]
    public static void GenFlist()
    {
        //updateAsset(ToolSetting.LocalPathWeb, ToolSetting.LocalTargetPath);
        //copyFile(Application.dataPath + "/StreamingAssets", path + "/Assets");
        string lastVersion = "";
        makeFileVersion(ToolSetting.LocalTargetPath, ref lastVersion);
        //updateFileVersion(Application.dataPath + "/StreamingAssets/Data/init.dat", lastVersion);
    }
    private static void copyFile(string fromFolder, string toFolder)
    {
        UnityEditor.EditorUtility.DisplayProgressBar("Copy file", "Wait...", 0.5f);
        string tFolder = null, tFileName = null;
        FileInfo tmp = null;
        foreach (DirectoryInfo d in new System.IO.DirectoryInfo(fromFolder).GetDirectories())
        {
            tFolder = toFolder + "/" + d.Name;
            DirectoryInfo targetFolder = new DirectoryInfo(tFolder);
            if (targetFolder.Exists) targetFolder.Delete(true);
            targetFolder.Create();
            foreach (FileInfo f in d.GetFiles())
            {
                if ((f.Name.EndsWith(".dat") && !f.Name.EndsWith("init.dat")) || f.Name.EndsWith(".u") || f.Name.EndsWith(".jpg") || f.Name.EndsWith(".png"))
                {
                    tFileName = tFolder + "/" + f.Name;
                    f.CopyTo(tFileName, true);
                    tmp = new FileInfo(tFileName);
                    tmp.LastAccessTime = f.LastAccessTime;
                    tmp.LastAccessTimeUtc = f.LastAccessTimeUtc;
                    tmp.LastWriteTime = f.LastWriteTime;
                    tmp.LastWriteTimeUtc = f.LastWriteTimeUtc;
                }
            }
        }
        UnityEditor.EditorUtility.ClearProgressBar();
    }
    private static void updateFileVersion(string path, string lastVersion)
    {
        string str = Encoding.UTF8.GetString(File.ReadAllBytes(path));
        string[] strs = str.Split(new char[] { ',' });
        StringBuilder sb = new StringBuilder();
        strs[0] = "";
        strs[2] = lastVersion;
        strs[3] = "0";
        sb.Append(strs[0]);
        sb.Append(",");
        sb.Append(strs[1]);
        sb.Append(",");
        sb.Append(strs[2]);
        sb.Append(",");
        sb.Append(strs[3]);
        sb.Append(",");
        sb.Append(strs[4]);
        File.WriteAllBytes(path, Encoding.UTF8.GetBytes(sb.ToString()));
    }
    private static void makeFileVersion(string versionPath, ref string lastVersion)
    {
        string path = versionPath;
        UnityEditor.EditorUtility.DisplayProgressBar("Make file list", "Ready...", 0);
        DirectoryInfo d = new DirectoryInfo(path);
        FileInfo[] tList = d.GetFiles("*.*", SearchOption.AllDirectories);
        if (tList.Length == 0) return;
        List<FileInfo> flist = new List<FileInfo>();
        FileInfo fi = null;
        for (int i = 0; i < tList.Length; i++)
        {
            fi = tList[i];
            if (fi.Name.EndsWith(".dat") || fi.Name.EndsWith(".u") || fi.Name.EndsWith(".jpg") || fi.Name.EndsWith(".png") || fi.Name.EndsWith(".xml") || fi.Name.EndsWith(".txt"))
            {
                flist.Add(fi);
            }
        }
        List<FileTmp> list = new List<FileTmp>();
        for (int i = 0; i < flist.Count; i++)
        {
            fi = flist[i];
            UnityEditor.EditorUtility.DisplayProgressBar("Make file version process", (i + 1) + "/" + flist.Count + "              " + fi.Name, (i + 1.0f) / flist.Count); ;

            FileTmp t = new FileTmp();
            t.FileName = fi.Name;
            t.Path = fi.Directory.Name;
            t.ModifyTime = fi.LastWriteTimeUtc.ToString("yyyyMMddHHmmss");
            using (FileStream fs = fi.Open(FileMode.Open))
            {
                t.MD5 = System.BitConverter.ToString(provider.ComputeHash(fs));
            }
            t.MD5 = t.MD5.Replace("-", "");
            t.Size = (fi.Length / 1024.0f).ToString("f2");
            list.Add(t);
        }
        orderList(list);
        FileTmp tmp = null;
        StringBuilder sb = new StringBuilder();
        StringBuilder csv = new StringBuilder();
        int ii = 0;
        for (ii = 0; ii < list.Count-1; ii++)
        {
            tmp = list[ii];
            sb.Append(tmp.FileName);
            sb.Append(",");
            sb.Append(tmp.Path);
            sb.Append(",");
            sb.Append(tmp.Size);
            sb.Append(",");
            sb.Append(tmp.ModifyTime);
            sb.Append(",");
            sb.Append(tmp.MD5);
            sb.Append(";");
            lastVersion = tmp.ModifyTime;
            csv.Append(string.Format("{0},{1},{2}#,{3}#,{4}\r\n", tmp.FileName, tmp.Path, tmp.Size, tmp.ModifyTime, tmp.MD5));
        }
        if (ii < list.Count)
        {
            tmp = list[ii];
            sb.Append(tmp.FileName);
            sb.Append(",");
            sb.Append(tmp.Path);
            sb.Append(",");
            sb.Append(tmp.Size);
            sb.Append(",");
            sb.Append(tmp.ModifyTime);
            sb.Append(",");
            sb.Append(tmp.MD5);
            lastVersion = tmp.ModifyTime;
            csv.Append(string.Format("{0},{1},{2}#,{3}#,{4}\r\n", tmp.FileName, tmp.Path, tmp.Size, tmp.ModifyTime, tmp.MD5));
        }

        //sb.Append("?>");
        File.WriteAllBytes(versionPath + "/flist.txt", Encoding.UTF8.GetBytes(sb.ToString()));
        File.WriteAllBytes(versionPath + "/flist.csv", Encoding.UTF8.GetBytes(csv.ToString()));
        Debug.LogWarning("MakeFileVersion file count : " + list.Count);
        UnityEditor.EditorUtility.ClearProgressBar();
    }
    private static void orderList(List<FileTmp> list)
    {
        FileTmp tmp = null;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            for (int j = 1; j <= i; j++)
            {
                if (string.Compare(list[j - 1].ModifyTime, list[j].ModifyTime) > 0)
                {
                    tmp = list[j];
                    list[j] = list[j - 1];
                    list[j - 1] = tmp;
                }
            }
        }
    }
    private static bool checkUpdate(string file)
    {
        /*
        createFolder(ref tPath, "Data");
        createFolder(ref tPath, "Card");
        createFolder(ref tPath, "Icon");
        createFolder(ref tPath, "Terrains");
        createFolder(ref tPath, "Units");
        createFolder(ref tPath, "Audio");
        createFolder(ref tPath, "Item");
        createFolder(ref tPath, "AE");
        createFolder(ref tPath, "Arena");
         */
        return (file.IndexOf("Data/") > -1 && file.IndexOf("init.dat") == -1 && file.IndexOf("GameCore.dat") == -1)
            || file.IndexOf("Card/") > -1
            || file.IndexOf("Item/") > -1
            || file.IndexOf("Arena/") > -1
            || (file.IndexOf("Icon/") > -1 && !file.EndsWith(".u"));
    }
    private static void updateAsset(string webPath, string targetPath)
    {
        string path = webPath + "/version.php?v=0";
        string assetPath = webPath + "/Assets/";
        string verionStr = System.Text.Encoding.UTF8.GetString(updateAssetsItem(path));
        string[] files = verionStr.Split(new char[] { ':' });
        files = files[1].Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        string f = null, tmpF = null, tmpMD5 = null;// lastTick = null;
        string[] fl = null;
        bool needUpdate = false;
        StringBuilder updateList = new StringBuilder();
        int updateCount = 0;
        for (int i = 0; i < files.Length; i++)
        {
            f = files[i];
            fl = f.Split(new char[] { ',' });
            tmpF = fl[1] + "/" + fl[0];
            UnityEditor.EditorUtility.DisplayProgressBar("Update process", (i + 1) + "/" + files.Length + "              " + tmpF, (i + 1.0f) / files.Length);
            if (checkUpdate(tmpF))
            {
                tmpF = targetPath + "/" + tmpF;
                needUpdate = false;
                if (System.IO.File.Exists(tmpF))
                {
                    using (System.IO.FileStream fs = new System.IO.FileStream(tmpF, FileMode.Open))
                    {
                        tmpMD5 = System.BitConverter.ToString(provider.ComputeHash(fs));
                        tmpMD5 = tmpMD5.Replace("-", "");
                    }
                    if (string.Compare(tmpMD5, fl[4]) != 0)
                    {
                        needUpdate = true;
                    }
                }
                else
                {
                    needUpdate = true;
                }
                if (needUpdate)
                {
                    f = fl[1] + "/" + fl[0];
                    updateList.AppendLine(f);
                    System.IO.File.WriteAllBytes(tmpF, updateAssetsItem(assetPath + f));
                    updateCount++;
                }
            }
            //lastTick = fl[3];
        }
        UnityEditor.EditorUtility.ClearProgressBar();
        Debug.LogWarning("Update file count : " + updateCount + ",list:" + updateList.ToString());
    }
    private static byte[] updateAssetsItem(string url)
    {
        byte[] result = null;
        WWW req = new WWW(url);
        while (!req.isDone) { System.Threading.Thread.Sleep(20); }
        result = req.bytes;
        req.Dispose();
        return result;
    }
    private static string localPathAndroid = string.Empty;
    public static string LocalPathAndroid
    {
        get
        {
            if (localPathAndroid == string.Empty)
            {
                localPathAndroid = GetString(localAssetPathAndroidKey);
            }
            return MakePath(localPathAndroid);
        }
    }
    private static string localPathWeb = string.Empty;
    public static string LocalPathWeb
    {
        get
        {
            if (localPathWeb == string.Empty)
            {
                localPathWeb = GetString(localAssetPathWebKey);
            }
            return MakePath(localPathWeb);
        }
    }
    private static string localPathIphone = string.Empty;
    public static string LocalPathIphone
    {
        get
        {
            if (localPathIphone == string.Empty)
            {
                localPathIphone = GetString(localAssetPathIphoneKey);
            }
            return MakePath(localPathIphone);
        }
    }
    public static string LocalTargetPath
    {
        get
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                return LocalPathAndroid;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iPhone)
            {
                return LocalPathIphone;
            }
            else
            {
                return LocalPathWeb;
            }
        }
    }
    static string GetString(string k)
    {
        string path = MakePath(System.Environment.CurrentDirectory) + k + ".init";
        string result = string.Empty;
        if (System.IO.File.Exists(path))
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    result = sr.ReadToEnd();
                }
            }
        }
        return result;
    }
    static void SetString(string k, string v)
    {
        string path = MakePath(System.Environment.CurrentDirectory) + k + ".init";
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter sr = new StreamWriter(fs, Encoding.UTF8))
            {
                sr.Write(v);
            }
        }
    }
    public static string MakePath(string path)
    {
        //if (!path.EndsWith(new string(new char[]{Path.DirectorySeparatorChar}))) path = path + Path.DirectorySeparatorChar;
        if (!path.EndsWith(new string(Path.DirectorySeparatorChar, 1))) return path + new string(Path.DirectorySeparatorChar, 1);
        return path;
    }
    void OnGUI()
    {
        localPathWeb = EditorGUILayout.TextField("Web asset path:", localPathWeb);
        localPathAndroid = EditorGUILayout.TextField("Android asset path:", localPathAndroid);
        localPathIphone = EditorGUILayout.TextField("Iphone asset path:", localPathIphone);
        if (GUILayout.Button("Reset local asset path"))
        {
            SetString(localAssetPathAndroidKey, localPathAndroid);
            SetString(localAssetPathWebKey, localPathWeb);
            SetString(localAssetPathIphoneKey, localPathIphone);
        }
        //stepDistanceStr = EditorGUILayout.TextField("Step:", stepDistanceStr);
        //chectHeightStr = EditorGUILayout.TextField("Check height:", chectHeightStr);
        //InitTransform = EditorGUILayout.ObjectField("Map zero:", InitTransform, typeof(GameObject)) as GameObject;
    }


    //[MenuItem("2D Tools/Unicode/UTF8")]
    public static void ChangeUnicode()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath);
        FileInfo[] fileInfo = dir.GetFiles("*.cs", SearchOption.AllDirectories);
        Debug.Log("开始转换编码:" + fileInfo.Length);

        foreach (var f in fileInfo)
        {
            string s = File.ReadAllText(f.FullName, Encoding.Default);
            File.WriteAllText(f.FullName, s, Encoding.UTF8);
        }
    }
}