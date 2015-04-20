using System;
using System.Collections.Generic;
using Common;
using NGUI;
using UnityEngine;
using System.Collections;

public class TestDigit : MonoBehaviour {

    static public Action<String> sendError;
    void Start ()
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

    private NGUI_Digit scriptBlood;
    public List<NGUI_Digit> scripts = new List<NGUI_Digit>();
    int i = 0;
	// Update is called once per frame
    void Update ()
    {
        LoadHelp.Update();
    }
	void OnGUI () {
        if (GUI.Button(new Rect(20, 20, 200, 100), "show digit"))
        {

            NGUIManager.Instance.AddByName<NGUI_Digit>(NGUI_UI.NGUI_Digit, NGUIShowType.MULTI, delegate( NGUI_Digit script )
            {
                scriptBlood = script;
                script.Init();
                script.TextLabel.gameObject.SetActive(true);
                script.Text = "-100";
                
                scripts.Add(script);
                TweenPosition tweenPosition = script.TextLabel.GetComponent<TweenPosition>();
                tweenPosition.AddOnFinished(delegate
                {
                    //script.TextLabel.gameObject.SetActive(false);
                    NGUIManager.Instance.DeleteByNameAndScript(NGUI_UI.NGUI_Digit, script);//-----------
                });

            });
        }
        if (GUI.Button(new Rect(20, 120, 200, 50), "close digit"))
        {
            NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_Digit);
            scripts.Clear();
        }
        if (GUI.Button(new Rect(20, 170, 400, 50), "close digit"))
        {
            if (scripts.Count > 0)
            {
                NGUIManager.Instance.DeleteByNameAndScript(NGUI_UI.NGUI_Digit, scripts[0]);
                scripts.RemoveAt(0);
            }
        }
	}

}
