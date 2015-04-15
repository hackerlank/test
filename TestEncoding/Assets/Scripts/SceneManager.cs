using System;
using System.Collections;
using UnityEngine;

public class SceneManager : SingletonForMono<SceneManager>
{
    private LoadSceneState _crrentSceneState;
    private string _NowSceneName;
    private string _PreSceneName;
    public ChangeSceneOver ChangeOver;
    public AsyncOperation mLoadingState;

    public bool ChangeScene(string vSceneName, ChangeSceneOver vChangeOver = null)
    {
        bool flag = false;
        if (this._crrentSceneState == LoadSceneState.NotLoading)
        {
            if (!this.IsCanChangeScene(vSceneName))
            {
                return flag;
            }
            this.mLoadingState = null;
            this.ChangeOver = vChangeOver;
            base.StartCoroutine(this.LoadScene(vSceneName, vChangeOver));
            flag = true;
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }
        return flag;
    }

    public bool GoBackToScene()
    {
        return this.ChangeScene(this._PreSceneName, null);
    }

    public void Initialize()
    {
        this._NowSceneName = string.Empty;
        this._PreSceneName = string.Empty;
    }

    private bool IsCanChangeScene(string vSceneName)
    {
        bool flag = false;
        if (string.IsNullOrEmpty(vSceneName))
        {
            return flag;
        }
        if (this._NowSceneName == vSceneName)
        {
            return flag;
        }
        return (flag = true);
    }

    IEnumerator LoadScene(string vSceneName, ChangeSceneOver vChangeOver)
    {
        mLoadingState = Application.LoadLevelAsync(vSceneName);

        if (null == mLoadingState)
        {
            yield break;
        }
        else
        {
            _PreSceneName = _NowSceneName;
            _NowSceneName = vSceneName;
        }

        _crrentSceneState = LoadSceneState.Loading;
        while (!mLoadingState.isDone)
        {
            Debug.Log("ad");
            Debug.Log(mLoadingState.progress);
            yield return 0;
        }
        Debug.Log(mLoadingState.progress);
        if (ChangeOver != null)
        {
            ChangeOver(vSceneName);
            ChangeOver = null;
        }
        PlayerSceneMusic(vSceneName);
        _crrentSceneState = LoadSceneState.NotLoading;
        mLoadingState = null;
    }

    public void PlayerSceneMusic(string vName)
    {
    }

    public string NowSceneName
    {
        get
        {
            return this._NowSceneName;
        }
    }

    public string PreSceneName
    {
        get
        {
            return this._PreSceneName;
        }
    }


    public delegate void ChangeSceneOver(string name);
}

