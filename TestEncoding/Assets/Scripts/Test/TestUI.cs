using System;
using System.Collections.Generic;
using Common;
using NGUI;
using UnityEngine;
using System.Collections;

public class TestUI : MonoBehaviour {
    static public Action<String> sendError;
    void Start()
    {
        Debug.Log("start------------");
        PlayerHandle.ErrorHandle = UnityEngine.Debug.LogError;
        PlayerHandle.DebugHandle = UnityEngine.Debug.Log;
        PlayerHandle.WarningHandle = UnityEngine.Debug.LogWarning;
        //PlayerHandle.SetLogFilter(new string[] { "MonoHandle.cs", "PlatformAPI.cs" });
        PlayerHandle.ErrorHandle += delegate( object msg )
        {
            if (sendError != null)
                sendError(msg.ToString());
        };
        LoadHelp.Init(MonoThread.Instance);
        NGUI_MainManager.Instance.Init(GameObject.Find(NGUI_UI.NGUI_Root));
        NGUIManager.Instance.SetNguiRoot(NGUI_MainManager.Instance.m_rootObj);
    }
	// Use this for initialization
	void Add<T> () where T :  new()
	{
        PlayerHandle.Debug("add " + NGUI_UI.NGUI_TestUI);
#if Test
	    string url = "file:///" + Application.streamingAssetsPath + "/Icon/ui.u";
        StartCoroutine(LoadUI(url));
#elif Test2
        CoreLoadHelp.LoadBytes(this, "file:///E:/work/Rose2/Src/Client/branches/Beige/Assets/StreamingAssets/Icon/ui.u",
            bytes =>
            {
                string info = System.Text.Encoding.Default.GetString(bytes);
                Debug.LogWarning(info);
            }
            );
#endif
	    //UIManager.Instance.Add<T>(this.gameObject);
	    NGUIManager.Instance.AddByName<NGUI_TestUI>(NGUI_UI.NGUI_TestUI);
	}

    IEnumerator LoadUI(string url)
    {
        WWW req = new WWW(url);
        yield return req;
        if (req.error != null)
        {
            Debug.LogError(req.error);
        }
        else
        {
            Debug.Log(req.text);
        }
    }
    void Update()
    {
        LoadHelp.Update();
    }
	// Update is called once per frame
    private NGUI_Blood scriptBlood;
    public List<NGUI_Blood> scripts = new List<NGUI_Blood>();
    int i = 0;
    private int backg = 0;
	void OnGUI () {           
	    if (GUI.Button(new Rect(20, 20, 200, 100), "add test"))
	    {
	        Add<TestUI>();
	    }
        if (GUI.Button(new Rect(20, 120, 200, 100), "close ui"))
        {
            NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_TestUI);
        }
        if (GUI.Button(new Rect(20, 220, 200, 100), "show 12 blood"))
        {

            for (int j = 0; j < 12; j++)
            {
                
            
            NGUIManager.Instance.AddByName<NGUI_Blood>(NGUI_UI.NGUI_Blood, NGUIShowType.MULTI,  delegate(NGUI_Blood script)
            {
                scriptBlood = script;
                scripts.Add(script);
                script.Init();
                backg = (backg + 1)%3 + 1;
                script.BloodSlider.backgroundWidget.GetComponent<UISprite>().spriteName = "st000" + backg;
                script.BloodSlider.foregroundWidget.GetComponent<UISprite>().spriteName = "st0006";
                //script.BloodSlider.backgroundWidget.GetComponent<UISprite>().border.Set(10, 0, 55, 0);
                script.SpriteSlider.foregroundWidget.GetComponent<UISprite>().spriteName = "st0007";
                script.MaxBlood = 100;
                script.CurrentBlood = 80;
                script.MaxSprite = 100;
                script.CurrentSprite = 100;
                script.Playername = "test_gailun";
                script.Playerlevel = 29;
                
                UIFollowTarget followTarget = script.gameObject.AddComponent<UIFollowTarget>();
                i = (i + 1)%2;
                script.name = script.name + i;
                followTarget.target = GameObject.Find("Player" + i).transform;
                script.transform.localScale = new Vector3(3, 3, 3);//这里要做一个素材规范。
                Debug.LogError("实例化血条" + i);
            });
            }
	    }
        if (GUI.Button(new Rect(20, 320, 200, 50), "close blood"))
        {
            NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_Blood);
            scripts.Clear();
        }
        if (GUI.Button(new Rect(20, 370, 400, 50), "close blood"))
        {
            if (scripts.Count > 0)
            {
                NGUIManager.Instance.DeleteByNameAndScript(NGUI_UI.NGUI_Blood, scripts[0]);
                scripts.RemoveAt(0);
            }
        }
        if (GUI.Button(new Rect(20, 420, 200, 100), "加10滴血"))
        {
            scriptBlood.CurrentBlood += 10;
            if (scriptBlood.CurrentBlood > scriptBlood.MaxBlood)
            {
                scriptBlood.CurrentBlood = scriptBlood.MaxBlood;
            }
        }
        if (GUI.Button(new Rect(20, 520, 200, 100), "减10滴血"))
        {
            scriptBlood.CurrentBlood -= 10;
            if (scriptBlood.CurrentBlood < 0)
            {
                scriptBlood.CurrentBlood = 0;
            }
        }

	}
}
