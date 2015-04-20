using UnityEngine;
using System.Collections;

public static class GamePath
{
    private static string _config;
    public static string Resources = "Resources/";

    //public static string Config
    //{
    //    get
    //    {
    //        return GetDataRootPath() + "/Config/";
    //    }
    //}

    public static string GetDataRootPath()
    {
        string dataPath = "";
#if UNITY_EDITOR
        dataPath = Application.dataPath + "/StreamingAssets";
#elif UNITY_STANDALONE_WIN
     dataPath = Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
     dataPath = "jar:file://" + Application.dataPath + "!/assets";
#elif UNITY_IPHONE
    dataPath = Application.dataPath + "/Raw";
#else

#endif
        return dataPath;
    }
}
