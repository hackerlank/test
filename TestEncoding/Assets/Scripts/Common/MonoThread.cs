using UnityEngine;
using System.Collections;

public class MonoThread : SingletonForMono<MonoThread>
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnApplicationQuit()
    {
        Destroy(gameObject);
    }
}
