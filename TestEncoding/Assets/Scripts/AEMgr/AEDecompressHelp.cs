using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Common;

/// <summary>
/// ս����ɫ���ݹ�����
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

    private Dictionary<string, UnityEngine.Object> allUnDic = new Dictionary<string, UnityEngine.Object>();//�������ݻ��������

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

    private Dictionary<string, List<CacheListData>> loadCacheList = new Dictionary<string, List<CacheListData>>();//��Դ����û��ѹ��������Ļ������

    public void LoadObject(string strName, Action<AESpriteScriptable> callBack)
    {
        strName += ".u";

        if (allUnDic.ContainsKey(strName))//��ѹ��Դ��û��������
        {
            CallBackObject(strName,callBack);
        }
        else
        {
            if (loadCacheList.ContainsKey(strName))//��������Ķ���
            {
                loadCacheList[strName].Add(new CacheListData(strName,callBack));
            }
            else//ֻ����һ��
            {
                List<CacheListData> newCache = new List<CacheListData>();

                newCache.Add(new CacheListData(strName,callBack));

                loadCacheList.Add(strName, newCache);

                LoadHelp.LoadObject(AssetAbsPath + strName, o =>
                {
                    UnityEngine.Object newObj = o.mainAsset;

                    allUnDic.Add(strName, newObj);

                    LoadHelp.DeleteWWW(AssetAbsPath + strName);

                    List<CacheListData> cacheList = loadCacheList[strName];//��������Ķ���

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
                        PlayerHandle.Error(strName + "��Դ�����ݻص� �����쳣��Ϣ:" + ex.Message + "\n" + ex.StackTrace);
                    }

                    cacheList.Clear();
                    loadCacheList.Remove(strName);
                });
            }

        }
    }

    private void CallBackObject(string strName, Action<AESpriteScriptable> callBack)//�����������
    {
        AESpriteScriptable ae = allUnDic[strName] as AESpriteScriptable;
        callBack(ae);
    }

}
