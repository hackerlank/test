using UnityEngine;
using System.Collections;

public class SingletonForMono<T> : MonoBehaviour where T : SingletonForMono<T>
{
    /// <summary>
    /// 避免同步
    /// </summary>
    //public static System.Object lockObj = new object();

    /// <summary>
    /// 单例的实例
    /// </summary>
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (applicationQuit)
            {
                _instance = null;

                return _instance;
            }

            if (_instance != null)
            {
                return _instance;
            }

            //lock (lockObj)
            //{
                

                _instance = FindObjectOfType(typeof(T)) as T;

                if (_instance == null)
                {
                    string instanceGoName = typeof(T).ToString() + @"(Singleton)";
                    _instance = new GameObject(instanceGoName).AddComponent<T>();
                    DontDestroyOnLoad(_instance);
                    _instance.CreateNewInstanceSuccessful();

                }

            //}
            return _instance;
        }
    }


    public virtual void CreateNewInstanceSuccessful()
    {

    }



    /// <summary>
    /// 判断该实例是否已存在
    /// </summary>
    public bool IsExistInstance
    {
        get
        {
            return _instance != null;
        }
    }

    /// <summary>
    /// 游戏是否退出
    /// </summary>
    private static bool applicationQuit = false;

    /// <summary>
    /// 游戏退出时调用
    /// </summary>
    void OnApplicationQuit()
    {
        applicationQuit = true;
    }

    public virtual void Destroy()
    {
        if (IsExistInstance)
        {
            Destroy(gameObject);
        }
    }


}
