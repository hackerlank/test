using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Common;

///���ע�ͣ������ 2015/1/5
/// ����ѹ����Դ������������ɺ󣬵��ûص��������ص�����������Զ���Դ���н�ѹ��
/// ���ص���Դ��ѹ����Դ��������allUnDic�ֵ��У��ȴ���ѹ����
///=================================================  
/// <summary>
/// ngui��ѹassetbundle�ļ�������
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
    /// ���ע�ͣ������ 2015/1/5 
    /// ���ص�ѹ����Դ���ȴ���ѹ��
    /// <key,value>=<��Դ��������Դ�������еļ��ص���Դ����>
    /// </summary>
    private Dictionary<string, List<UnityEngine.Object>> allUnDic = new Dictionary<string, List<UnityEngine.Object>>();
    /// <summary>
    /// ��ȡָ����Դ��strPack��������Դ(��Ϊѹ����Դ����Ҫ��ѹ)
    /// </summary>
    /// <param name="strPack">��Դ����</param>
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
        /// ���ע�ͣ������ 2015/1/5 
        /// ��Դ���а�������Դ����
        /// </summary>
        public string strItem;
        /// <summary>
        /// ���ע�ͣ������ 2015/1/5 
        /// ��������Դ����Ҫ���õĻص�����
        /// </summary>
        public Action<UnityEngine.Object> callBack;
        public CacheListData(string str, Action<UnityEngine.Object> call)
        {
            strItem = str;
            callBack = call;
        }
    }
    /// <summary>
    /// ���ע�ͣ������ 2015/1/5 
    /// <key, value>=<��Դ������ ���������б�>
    /// </summary>
    private Dictionary<string, List<CacheListData>> loadCacheList = new Dictionary<string, List<CacheListData>>();//��Դ����û��ѹ��������Ļ������
    
     
    /// <summary>
    /// ���ע�ͣ������ 2015/1/5 
    /// ����strPack��Դ���е����ж��󣬲��ڼ�����ɺ���ûص�����
    /// </summary>
    /// <param name="strPack">��Դ����</param>
    /// <param name="strItem">��Դ���е���Դ��</param>
    /// <param name="callBack">�ص�����</param>
    public void LoadObject(string strPack, string strItem, Action<UnityEngine.Object> callBack)
    {
       // Debug.LogError("LoadObject  -->" + strPack + "---" + strItem + "---" + allUnDic.ContainsKey(strPack));
        if (allUnDic.ContainsKey(strPack))//��ѹ��Դ��û��������
        {
            CallBackObject(strPack, strItem, callBack);
        }
        else
        {
            if (loadCacheList.ContainsKey(strPack))//��������Ķ���
            {
                loadCacheList[strPack].Add(new CacheListData(strItem, callBack));
            }
            else//ֻ����һ��
            {
                //����Ҫ���صĶ���ͻص�����
                List<CacheListData> newCache = new List<CacheListData>();
                newCache.Add(new CacheListData(strItem, callBack));

                loadCacheList.Add(strPack, newCache);
                
                //������Դ��strPack�е����ж���
                //�첽������Դ�����ڼ�����ɺ���ûص�����
                LoadHelp.LoadObject(strPack, o =>
                {
                    //load���ж���
                    List<UnityEngine.Object> newObj = GetAllObject(o.LoadAll());
                  //  Debug.LogWarning(strPack);
                    allUnDic.Add(strPack, newObj);

                    LoadHelp.DeleteWWW(strPack);
                    //�������л���Ļص�
                    List<CacheListData> cacheList = loadCacheList[strPack];//��������Ķ���

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
                        PlayerHandle.Error(strPack+ "��Դ�����ݻص� �����쳣��Ϣ:" + ex.Message + "\n" + ex.StackTrace);
                    }

                    cacheList.Clear();
                    loadCacheList.Remove(strPack);
                });
            }

        }

    }
    /// <summary>
    /// ���ע�ͣ������ 2015/1/5 
    /// �ҵ�δ��ѹ���ļ�������ΪstrItem�Ķ���findItem������Ϊ�������벢����callBack����:callBack(findItem)
    /// </summary>
    /// <param name="strPack">��Դ������</param>
    /// <param name="strItem">Ԥ������</param>
    /// <param name="callBack">�ص�����</param>
    private void CallBackObject(string strPack, string strItem, Action<UnityEngine.Object> callBack)//�����������
    {
        List<UnityEngine.Object> packDic = allUnDic[strPack];
        UnityEngine.Object findItem = null;
        try
        {
            findItem = packDic.Find(search => { return search.name == strItem; });
        }
        catch 
        {
            PlayerHandle.Warning("��Դ��: " + strPack + " ������ prefab:" + strItem);
        }
        if (findItem == null)
        {
            PlayerHandle.Warning("��Դ��: " + strPack + " ������ prefab:" + strItem);
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
