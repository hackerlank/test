using System;
using UnityEngine;

public class SingletonForMono<T> : MonoBehaviour where T: SingletonForMono<T>
{
    private static T _instance;
    private static bool applicationQuit;

    public virtual void CreateNewInstanceSuccessful()
    {
    }

    public virtual void Destroy()
    {
        if (this.IsExistInstance)
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SingletonForMono<T>.applicationQuit = true;
    }

    public static T Instance
    {
        get
        {
            if (SingletonForMono<T>.applicationQuit)
            {
                SingletonForMono<T>._instance = null;
                return SingletonForMono<T>._instance;
            }
            if (SingletonForMono<T>._instance == null)
            {
                SingletonForMono<T>._instance = UnityEngine.Object.FindObjectOfType(typeof(T)) as T;
                if (SingletonForMono<T>._instance == null)
                {
                    SingletonForMono<T>._instance = new GameObject(typeof(T).ToString() + "(Singleton)").AddComponent<T>();
                    UnityEngine.Object.DontDestroyOnLoad(SingletonForMono<T>._instance);
                    SingletonForMono<T>._instance.CreateNewInstanceSuccessful();
                }
            }
            return SingletonForMono<T>._instance;
        }
    }

    public bool IsExistInstance
    {
        get
        {
            return (SingletonForMono<T>._instance != null);
        }
    }
}

