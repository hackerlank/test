using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

using Object = UnityEngine.Object;

public class TestEffect : MonoBehaviour
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public IEnumerator LoadEffectAssetbundle(string path, Vector3 pos)
    {
        WWW www = new WWW(path);
        yield return www;
        byte[] encryptedData = www.bytes;
        AssetBundleCreateRequest acr = AssetBundle.CreateFromMemory(encryptedData);
        yield return acr;
        AssetBundle bundle = acr.assetBundle;
        if (bundle != null)
        {
            GameObject obj = bundle.mainAsset as GameObject;
            if (obj != null)
            {
                GameObject obj2 = Object.Instantiate(obj) as GameObject;
                obj2.transform.position = pos;
                obj2.SetActive(true);
            }
            bundle.Unload(false);
        }
    }

    public IEnumerator LoadEffectFromFile(string path, Vector3 pos, Transform root = null)
    {
        byte[] encryptedData = File.ReadAllBytes(path);
        AssetBundleCreateRequest acr = AssetBundle.CreateFromMemory(encryptedData);
        yield return acr;
        AssetBundle bundle = acr.assetBundle;
        if (bundle != null)
        {
            GameObject obj = bundle.mainAsset as GameObject;
            if (obj != null)
            {
                GameObject obj2 = Object.Instantiate(obj) as GameObject;
                obj2.transform.localPosition = pos;
                if (root != null)
                    obj2.transform.parent = root;
                obj2.SetActive(true);
            }
            bundle.Unload(false);
        }
    }

    void OnGUI()
    {
        
        if (GUI.Button(new Rect(50, 100, 100, 20), "effect1"))
        {
            //this.StartCoroutine(this.LoadEffectAssetbundle("file://" + Application.streamingAssetsPath + "/asset/effect/LightningBeams.unity3d", Vector3.zero));
            this.StartCoroutine(this.LoadEffectFromFile("asset/effect/gailun_e01.unity3d", Vector3.zero));
        }

        if (GUI.Button(new Rect(50, 130, 100, 20), "effect2"))
        {
            //this.StartCoroutine(this.LoadEffectAssetbundle("file://" + Application.streamingAssetsPath + "/asset/effect/LightningBeams.unity3d", Vector3.zero));
            this.StartCoroutine(this.LoadEffectFromFile("asset/effect/jiuweiyaohu_001_01_r01_qiu.unity3d", Vector3.zero));
        }

        if (GUI.Button(new Rect(50, 160, 100, 20), "effect3"))
        {
            //this.StartCoroutine(this.LoadEffectAssetbundle("file://" + Application.streamingAssetsPath + "/asset/effect/LightningBeams.unity3d", Vector3.zero));
            this.StartCoroutine(this.LoadEffectFromFile("asset/effect/jiuweiyaohu_001_01_a01.unity3d", Vector3.zero));
        }
    }

}
