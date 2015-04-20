using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    private IMonoHandle handle = null;
    void Start()
    {

//        Debug.Log(@"System.Text.Encoding.Default.GetBytes("").Length  " + System.Text.Encoding.Default.GetBytes("啊").Length);
//#if UNITY_STANDALONE_WIN && UNITY_EDITOR
//        //CoreLoadHelp.LoadApp(string.Concat("file:///", Application.dataPath, "/StreamingAssets/Data/GameCore.dat"), delegate(IMonoHandle _handle) { handle = _handle; handle.Start(this); handle.mWwise = WwiseAudioManager.GetInstance();handle.m_FAForStatic = new FrameAnimation();});
//        //handle = new MonoHandle();handle.Start(this);handle.mWwise = WwiseAudioManager.GetInstance();handle.mAndroidCommand = AndroidCommandMgr.Single;handle.m_FAForStatic = new FrameAnimation();
//#elif UNITY_IPHONE && UNITY_EDITOR
//        handle = new MonoHandle(); handle.Start(this);handle.mWwise = WwiseAudioManager.GetInstance(); handle.m_FAForStatic = new FrameAnimation();
//#elif UNITY_ANDROID && UNITY_EDITOR
//        //handle = new MonoHandle(); 
//        //handle.Start(this);
//        //handle.mWwise = WwiseAudioManager.GetInstance();
//        //handle.m_FAForStatic = new FrameAnimation();
//        CoreLoadHelp.LoadApp(string.Concat("file:///", Application.dataPath, "/StreamingAssets/Data/GameCore.dat"), delegate(IMonoHandle _handle) { handle = _handle; handle.Start(this);  
//        //handle.mWwise = WwiseAudioManager.GetInstance();
//        //handle.m_FAForStatic = new FrameAnimation();
//        });
//#elif UNITY_STANDALONE_WIN && !UNITY_EDITOR
//        handle = new MonoHandle();handle.Start(this);handle.mWwise = WwiseAudioManager.GetInstance(); handle.mAndroidCommand = AndroidCommandMgr.Single;handle.m_FAForStatic = new FrameAnimation();
//        //CoreLoadHelp.LoadApp(string.Concat("file:///", Application.dataPath, "/StreamingAssets/Data/GameCore.dat"), delegate(IMonoHandle _handle) { handle = _handle; handle.Start(this); handle.mWwise = WwiseAudioManager.GetInstance(); handle.m_FAForStatic = new FrameAnimation();});
//#elif UNITY_IPHONE && !UNITY_EDITOR
//        handle = new MonoHandle(); handle.Start(this);handle.mWwise = WwiseAudioManager.GetInstance(); handle.m_FAForStatic = new FrameAnimation(); handle.m_FAForStatic = new FrameAnimation();
//#elif UNITY_ANDROID && !UNITY_EDITOR
//        string path = string.Concat(Application.persistentDataPath, "/Data/GameCore.dat");
//        if (System.IO.File.Exists(path))
//        {
//            path = string.Concat("file:///", Application.persistentDataPath, "/Data/GameCore.dat");
//        }
//        else
//        {
//            path = string.Concat("jar:file://", Application.dataPath, "!/assets/Data/GameCore.dat");
//        }
//        CoreLoadHelp.LoadApp(path, delegate(IMonoHandle _handle) { handle = _handle;  handle.mWwise = WwiseAudioManager.GetInstance(); handle.m_FAForStatic = new FrameAnimation(); handle.mAndroidCommand = AndroidCommandMgr.Single; handle.Start(this); });
//#endif

    }

    void Update()
    {
        if (handle != null) handle.Update();
        //CameraEffect.Instance.Update();
    }
    void LateUpdate()
    {
        if (handle != null) handle.LateUpdate();
    }

    void FixedUpdate()
    {
        if (handle != null) handle.FixedUpdate();
    }

    void OnGUI()
    {
        if (handle != null) handle.OnGUI();
    }
    void OnApplicationQuit()
    {
        if (handle != null) handle.OnApplicationQuit();

        #if UNITY_STANDALONE_WIN&&!UNITY_EDITOR
              System.Diagnostics.Process.GetCurrentProcess().Kill();
        #endif
    }
    void OnApplicationFocus(bool f)
    {
        if (handle != null) handle.OnApplicationFocus(f);
    }
    public void OnApplicationPause(bool f)
    {
        if (handle != null) handle.OnApplicationPause(f);
    }
    public void RegisterNotifications(string para)
    {
        if (handle != null) handle.RegisterNotifications(para);
    }
    public void Login(string para)
    {
        if (handle != null) handle.Login(para);
    }
    public void PayResult(string para)
    {
        if (handle != null) handle.PayResult(para);
    }
    public void OnPause(string para)
    {
        if (handle != null) handle.OnPause(para);
    }
    public void OutputLog(string para)
    {
        //PlayerHandle.Debug(para);
    }
    public void SaveRecharge(string para)
    {
        if (handle != null) handle.SaveRecharge(para);
    }
 
}
