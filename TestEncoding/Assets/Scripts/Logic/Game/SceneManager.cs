using UnityEngine;
using System.Collections;

public class SceneManager : SingletonForMono<SceneManager>
{
    private string _NowSceneName;
    private string _PreSceneName;
    public UnityEngine.AsyncOperation mLoadingState = null;
    private LoadSceneState _crrentSceneState = LoadSceneState.NotLoading;

    public delegate void ChangeSceneOver(string name);
    public ChangeSceneOver ChangeOver;

    public void Initialize() 
    {
        _NowSceneName = "";
        _PreSceneName = "";
    }

    public string NowSceneName 
    {
        get { return _NowSceneName; }
    }

    public string PreSceneName 
    {
        get { return _PreSceneName; }
    }

    private bool IsCanChangeScene(string vSceneName) 
    {
        bool vResult = false;
        if (string.IsNullOrEmpty(vSceneName))
            return vResult;
        if (_NowSceneName == vSceneName)
            return vResult;
        return vResult = true;
    }

    public bool ChangeScene(string vSceneName, ChangeSceneOver  vChangeOver = null) 
    {
        bool vResult = false;
        if (_crrentSceneState != LoadSceneState.NotLoading) 
        {
            return vResult;
        }

        if (!IsCanChangeScene(vSceneName)) 
        {
            return vResult;
        }  
        mLoadingState = null;
        ChangeOver = vChangeOver;
        StartCoroutine(LoadScene(vSceneName,vChangeOver));
        vResult = true;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        return vResult;
       
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
        while(!mLoadingState.isDone)
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

    public bool GoBackToScene() 
    {
        return ChangeScene(_PreSceneName);
    }

    public void PlayerSceneMusic(string  vName) 
    {
        
    }

}

public enum LoadSceneState
{
    NotLoading = 0,
    Loading,
    LoadingFinish,
}
