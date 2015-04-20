using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Common;

/// <summary>
/// 战斗角色数据管理器
/// </summary>
public class AEDecompressHelp 
{
    private static AEDecompressHelp instance = null;
    public static AEDecompressHelp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AEDecompressHelp();
            }
            return instance;
        }
    }


    private string AssetAbsPath = @"Units_skill/";

    private Dictionary<string, UnityEngine.Object> allUnDic = new Dictionary<string, UnityEngine.Object>();//所有数据缓存管理器

    public class CacheListData
    {
        public string strName;
        public Action<AESpriteScriptable> callBack;
        public CacheListData(string str,Action<AESpriteScriptable> call)
        {
            strName = str;
            callBack = call;
        }
    }

    private Dictionary<string, List<CacheListData>> loadCacheList = new Dictionary<string, List<CacheListData>>();//资源包还没解压出来请求的缓存队列

    public void LoadObject(string strName, Action<AESpriteScriptable> callBack)
    {
        strName += ".u";

        if (allUnDic.ContainsKey(strName))//解压资源还没产生出来
        {
            CallBackObject(strName,callBack);
        }
        else
        {
            if (loadCacheList.ContainsKey(strName))//缓存请求的队列
            {
                loadCacheList[strName].Add(new CacheListData(strName,callBack));
            }
            else//只加载一次
            {
                List<CacheListData> newCache = new List<CacheListData>();

                newCache.Add(new CacheListData(strName,callBack));

                loadCacheList.Add(strName, newCache);

                LoadHelp.LoadObject(AssetAbsPath + strName, o =>
                {
                    UnityEngine.Object newObj = o.mainAsset;

                    allUnDic.Add(strName, newObj);

                    LoadHelp.DeleteWWW(AssetAbsPath + strName);

                    List<CacheListData> cacheList = loadCacheList[strName];//处理请求的队列

                    try
                    {
                        for (int i = 0; i < cacheList.Count; i++)
                        {
                            CacheListData cache = cacheList[i];

                            CallBackObject(cache.strName, cache.callBack);
                        }
                    }

                    catch (System.Exception ex)
                    {
                        PlayerHandle.Error(strName + "资源包数据回调 出现异常消息:" + ex.Message + "\n" + ex.StackTrace);
                    }

                    cacheList.Clear();
                    loadCacheList.Remove(strName);
                });
            }

        }
    }

    private void CallBackObject(string strName, Action<AESpriteScriptable> callBack)//处理请求队列
    {
        AESpriteScriptable ae = allUnDic[strName] as AESpriteScriptable;
        callBack(ae);
    }

}
