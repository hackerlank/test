using System;
using UnityEngine;

public static class GamePath
{
    private static string _config;
    public static string Resources = "Resources/";

    public static string GetDataRootPath()
    {
        string dataPath = "";

#if UNITY_EDITOR || UNITY_STANDALONE
        dataPath = Application.dataPath + "/StreamingAssets";
#elif UNITY_ANDROID
        dataPath = "jar:file://" + Application.dataPath + "!/assets";
#else
        dataPath = Application.dataPath + "/Raw";
#endif

        return dataPath;
    }

    public static string Config
    {
        get
        {
            return GetDataRootPath() + "/Config/";
        }
    }
}

