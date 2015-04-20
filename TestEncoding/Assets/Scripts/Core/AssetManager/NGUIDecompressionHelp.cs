using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Common;

///添加注释：王庆国 2015/1/5
/// 下载压缩资源，并在下载完成后，调用回调函数。回调函数里面可以对资源进行解压。
/// 下载的资源是压缩资源，保存在allUnDic字典中，等待解压缩。
///=================================================  
/// <summary>
/// ngui解压assetbundle文件管理器
/// </summary>
public class NGUIDecompressionHelp
{
    private static NGUIDecompressionHelp instance = null;
    public static NGUIDecompressionHelp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new NGUIDecompressionHelp();
            }
            return instance;
        }
    }
    /// <summary>
    /// 添加注释：王庆国 2015/1/5 
    /// 加载的压缩资源，等待解压缩
    /// <key,value>=<资源包名，资源包中所有的加载的资源对象>
    /// </summary>
    private Dictionary<string, List<UnityEngine.Object>> allUnDic = new Dictionary<string, List<UnityEngine.Object>>();
    /// <summary>
    /// 获取指定资源包strPack中所有资源(均为压缩资源，需要解压)
    /// </summary>
    /// <param name="strPack">资源包名</param>
    /// <returns></returns>
    public List<UnityEngine.Object> GetPackObject(string strPack)
    {
        if (!allUnDic.ContainsKey(strPack))
            return null;
        
        return allUnDic[strPack];
    }

    public class CacheListData
    {
        /// <summary>
        /// 添加注释：王庆国 2015/1/5 
        /// 资源包中包含的资源名字
        /// </summary>
        public string strItem;
        /// <summary>
        /// 添加注释：王庆国 2015/1/5 
        /// 加载完资源后需要调用的回调函数
        /// </summary>
        public Action<UnityEngine.Object> callBack;
        public CacheListData(string str, Action<UnityEngine.Object> call)
        {
            strItem = str;
            callBack = call;
        }
    }
    /// <summary>
    /// 添加注释：王庆国 2015/1/5 
    /// <key, value>=<资源包名， 缓存数据列表>
    /// </summary>
    private Dictionary<string, List<CacheListData>> loadCacheList = new Dictionary<string, List<CacheListData>>();//资源包还没解压出来请求的缓存队列
    
     
    /// <summary>
    /// 添加注释：王庆国 2015/1/5 
    /// 加载strPack资源包中的所有对象，并在加载完成后调用回调函数
    /// </summary>
    /// <param name="strPack">资源包名</param>
    /// <param name="strItem">资源包中的资源名</param>
    /// <param name="callBack">回调函数</param>
    public void LoadObject(string strPack, string strItem, Action<UnityEngine.Object> callBack)
    {
       // Debug.LogError("LoadObject  -->" + strPack + "---" + strItem + "---" + allUnDic.ContainsKey(strPack));
        if (allUnDic.ContainsKey(strPack))//解压资源还没产生出来
        {
            CallBackObject(strPack, strItem, callBack);
        }
        else
        {
            if (loadCacheList.ContainsKey(strPack))//缓存请求的队列
            {
                loadCacheList[strPack].Add(new CacheListData(strItem, callBack));
            }
            else//只加载一次
            {
                //缓存要加载的对象和回调函数
                List<CacheListData> newCache = new List<CacheListData>();
                newCache.Add(new CacheListData(strItem, callBack));

                loadCacheList.Add(strPack, newCache);
                
                //加载资源包strPack中的所有对象
                //异步加载资源，并在加载完成后调用回调函数
                LoadHelp.LoadObject(strPack, o =>
                {
                    //load所有对象
                    List<UnityEngine.Object> newObj = GetAllObject(o.LoadAll());
                  //  Debug.LogWarning(strPack);
                    allUnDic.Add(strPack, newObj);

                    LoadHelp.DeleteWWW(strPack);
                    //处理所有缓存的回调
                    List<CacheListData> cacheList = loadCacheList[strPack];//处理请求的队列

                    try
                    {
                        for (int i = 0; i < cacheList.Count; i++)
                        {
                            CacheListData cache = cacheList[i];

                            CallBackObject(strPack, cache.strItem, cache.callBack);
                        }
                    }

                    catch (System.Exception ex)
                    {
                        PlayerHandle.Error(strPack+ "资源包数据回调 出现异常消息:" + ex.Message + "\n" + ex.StackTrace);
                    }

                    cacheList.Clear();
                    loadCacheList.Remove(strPack);
                });
            }

        }

    }
    /// <summary>
    /// 添加注释：王庆国 2015/1/5 
    /// 找到未解压的文件中名字为strItem的对象findItem，并作为参数传入并调用callBack函数:callBack(findItem)
    /// </summary>
    /// <param name="strPack">资源包名字</param>
    /// <param name="strItem">预置名字</param>
    /// <param name="callBack">回调函数</param>
    private void CallBackObject(string strPack, string strItem, Action<UnityEngine.Object> callBack)//处理请求队列
    {
        List<UnityEngine.Object> packDic = allUnDic[strPack];
        UnityEngine.Object findItem = null;
        try
        {
            findItem = packDic.Find(search => { return search.name == strItem; });
        }
        catch 
        {
            PlayerHandle.Warning("资源包: " + strPack + " 不存在 prefab:" + strItem);
        }
        if (findItem == null)
        {
            PlayerHandle.Warning("资源包: " + strPack + " 不存在 prefab:" + strItem);
            callBack(null);
        }
        else
        {
            callBack(findItem);
        }
    }

    public List<UnityEngine.Object> GetAllObject(UnityEngine.Object[] objects)
    {
        List<UnityEngine.Object> newList = new List<UnityEngine.Object>();

        for (int i = 0; i < objects.Length; i++)
        {
            UnityEngine.Object obj = objects[i];
            newList.Add(obj);
        }

        return newList;
    }

    public void DeleteByName(string strPack)
    {
        if (!allUnDic.ContainsKey(strPack)) return;

        List<UnityEngine.Object> allList = allUnDic[strPack];

        for (int i = 0; i < allList.Count; i++)
        {
            GameObject.DestroyImmediate(allList[i],true);
        }

        allList.Clear();
        allUnDic.Remove(strPack);
    }

}
