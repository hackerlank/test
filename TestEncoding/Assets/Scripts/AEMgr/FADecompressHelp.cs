using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Common;

/// <summary>
/// ս����ɫ���ݹ�����
/// </summary>
public class FADecompressHelp 
{
    private static FADecompressHelp instance = null;
    public static FADecompressHelp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FADecompressHelp();
            }
            return instance;
        }
    }


    private string AssetAbsPath = @"Units_Data/";

    private Dictionary<string, UnityEngine.Object> allUnDic = new Dictionary<string, UnityEngine.Object>();//�������ݻ��������

    public class CacheListData
    {
        public IFrameAnimation frameAnimation;
        public Action callBack;
        public CacheListData(IFrameAnimation fa, Action call)
        {
            frameAnimation = fa;
            callBack = call;
        }
    }

    private Dictionary<string, List<CacheListData>> loadCacheList = new Dictionary<string, List<CacheListData>>();//��Դ����û��ѹ��������Ļ������

    public void LoadObject(IFrameAnimation frameAnimation, Action callBack)
    {
        string strName = frameAnimation.StrAnimationData + ".u";

        if (allUnDic.ContainsKey(strName))//��ѹ��Դ��û��������
        {
            CallBackObject(strName, frameAnimation, callBack);
        }
        else
        {
            if (loadCacheList.ContainsKey(strName))//��������Ķ���
            {
                loadCacheList[strName].Add(new CacheListData(frameAnimation, callBack));
            }
            else//ֻ����һ��
            {
                List<CacheListData> newCache = new List<CacheListData>();

                newCache.Add(new CacheListData(frameAnimation, callBack));

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

                            CallBackObject(cache.frameAnimation.StrAnimationData + ".u", cache.frameAnimation, cache.callBack);
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

    private void CallBackObject(string strName,IFrameAnimation frameAnimation, Action callBack)//�����������
    {
        frameAnimation.InitData(allUnDic[strName]);

        callBack();
    }


    public void PreLoad(string strName, Action<UnityEngine.Object> callBack)
    {
        strName += ".u";

        LoadHelp.LoadObject(AssetAbsPath + strName, o =>
        {
            UnityEngine.Object newObj = o.mainAsset;

            allUnDic.Add(strName, newObj);

            LoadHelp.DeleteWWW(AssetAbsPath + strName);

            callBack(newObj);
        });
    }


}
