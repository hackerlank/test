using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

namespace Common
{
    public enum AssetType
    {
        AssetBundle = 0,
        Texture,
        Audio,
        Text,
    }
    public class AssetRequest : IAssetRequest
    {
        public WWW Request = null;
        public DateTime LastModify;
        public string RealPath = string.Empty;
        public int UseCount = 0;
        public int UsedCount
        {
            get { return UseCount; }
            set { UseCount = value; }
        }
        public int RetryCount = 0;
        public bool WriteToLocal = false;
        public AssetRequestState State = AssetRequestState.None;
        public AssetRequestType RequestType = AssetRequestType.Default;
        private Stack<Action<AssetRequest>> callBackList = new Stack<Action<AssetRequest>>();
        private string path = "";
        public bool KeepWWW = false;
        public string Path
        {
            get { return path; }
        }
        public AssetRequest(string path)
        {
            RealPath = this.path = path;
        }
        private AssetType type = AssetType.AssetBundle;
        public AssetType Type
        {
            get { return type; }
            set { type = value; }
        }
        string text = null;
        public string Text
        {
            get 
            {
                if (text == null)
                {
                    text = Request.text;
                    Request.Dispose();
                }
                return text; 
            }
        }
        private Texture2D texture = null;
        public Texture Texture
        {
            get
            {
                if (RequestType == AssetRequestType.Default)
                {
                    //foreach (UnityEngine.Object o in Request.assetBundle.LoadAll())
                    //{
                    //    if (o.GetType() == typeof(Texture))
                    //    {
                    //        return o as Texture;
                    //    }
                    //}
                    //return Request.texture;
                    if (texture == null)
                        texture = Request.texture;
                    return texture;
                }
                else
                {
                    if (texture == null)
                        texture = Request.texture;
                    return texture;
                }
                //return null;
            }
        }
        private AudioClip audio;
        public AudioClip Audio
        {
            get
            {
                if (audio == null) audio = Request.audioClip;
                return audio;
            }
        }
        private UnityEngine.Object mainAssetObject;
        public UnityEngine.Object mainAsset
        {
            get
            {
                if (mainAssetObject == null)
                {
                    try
                    {
                        mainAssetObject = Request.assetBundle.mainAsset;
                    }
                    catch (Exception ex)
                    {
                        PlayerHandle.Error(this, "AssetRequest", string.Format("AssetRequest({0}) load mainAsset fail:({1})", path, ex.ToString()));
                    }
                }
                return mainAssetObject;
            }
        }

        private UnityEngine.Object[] objs;
        private UnityEngine.Object GetObjs(string strName)
        {
            UnityEngine.Object obj = null;
            for (int i = 0; i < objs.Length; i++)
            {
                obj = objs[i];
                if (obj.name == strName)
                {
                    break;
                }
                obj = null;
            }
            return obj;
        }

        private UnityEngine.Object GetObjs(string strName,Type type)
        {
            UnityEngine.Object obj = null;
            for (int i = 0; i < objs.Length; i++)
            {
                obj = objs[i];
                if (obj.name == strName&&obj.GetType()==type)
                {
                    break;
                }
                obj = null;
            }
            return obj;
        }


        public UnityEngine.Object[] LoadAll()
        {
            if (objs == null)
            {
                try
                {
                    objs = Request.assetBundle.LoadAll();
                }
                catch (Exception ex)
                {
                    PlayerHandle.Error(this, "AssetRequest", string.Format("AssetRequest({0}) load LoadAll fail:({1})", path, ex.ToString()));
                }
            }
            return objs;
        }
        public void AddTask(Action<AssetRequest> task)
        {
            //Debug.LogError("AddTask:  "+path + "   " + Time.realtimeSinceStartup);
            callBackList.Push(task);
            if (State == AssetRequestState.LoadComplete)
            {
                ClearTask();

                //PlayerHandle.InvokeHandle(delegate()
                //{
                //    ClearTask();
                
                //});
            }
            Update();
        }
        public void ClearTask()
        {
            operateAssetLoad();
            while (callBackList.Count > 0)
            {
                Action<AssetRequest> callBack = callBackList.Pop();
                if (State == AssetRequestState.LoadComplete) { callBack(this); }
                else
                {
                    PlayerHandle.Error(this, "ClearTask", string.Format("AssetRequest url:{0} fail, state:{1}", RealPath, State));
                    callBack(null);
                }
            }
        }
        public float Process
        {
            get
            {
                if (Request == null)
                {
                    return 0;
                }
                else
                {
                    if (State == AssetRequestState.Dispose || State == AssetRequestState.LoadFail || Request.isDone) return 1;
                    else return Request.progress;
                }
            }
        }
        public void GetAsset()
        {
            LoadHelp.RequestOperation.StartHandle(this);
//			Debug.LogError(this.path);
        }
        public void Update()
        {
            LoadHelp.RequestOperation.UpdateHandle(this);
        }
        public void Dispose()
        {
           // Debug.LogError("Delet-->" + this.path);
            operateAssetUnload();
            State = AssetRequestState.Dispose;
        }


        public void DisposeWWW()
        {
            /*
            if (State == AssetRequestState.LoadComplete && Request.isDone && RequestType == AssetRequestType.Default)
            {
                switch (Type)
                {
                    case AssetType.AssetBundle:
                        {
                            if (Request.assetBundle != null) Request.assetBundle.Unload(false);
                            break;
                        }
                    case AssetType.Texture:
                        {
                            if (texture != null)
                            {
                                GameObject.DestroyImmediate(texture, true);
                                texture = null;
                            }
                            break;
                        }
                    case AssetType.Audio:
                        {
                            if (audio != null)
                            {
                                GameObject.DestroyImmediate(audio, true);
                                audio = null;
                            }
                            break;
                        }
                }
            }
            mainAssetObject = null;
            State = AssetRequestState.Dispose;
            Request.Dispose();
            //Resources.UnloadUnusedAssets();
             * */
            operateAssetUnload();
        }

        private void operateAssetLoad()
        {
            if (Request == null) return;

            if (State == AssetRequestState.LoadComplete&&Request.isDone && RequestType == AssetRequestType.Default)
            {
                if (!KeepWWW)
                {
                    switch (Type)
                    {
                        case AssetType.AssetBundle:
                            {
                                try
                                {
                                    objs = Request.assetBundle.LoadAll();
                                }
                                catch (Exception ex)
                                {
                                    PlayerHandle.Error(this, "AssetRequest", string.Format("AssetRequest({0}) load LoadAll fail:({1})", path, ex.ToString()));
                                }
                                try
                                {
                                    mainAssetObject = Request.assetBundle.mainAsset;
                                }
                                catch (Exception ex)
                                {
                                    PlayerHandle.Error(this, "AssetRequest", string.Format("AssetRequest({0}) load LoadAll fail:({1})", path, ex.ToString()));
                                }
                                Request.assetBundle.Unload(false);
                                break;
                            }
                        case AssetType.Texture:
                            {
                                texture = Request.texture;
                                break;
                            }
                        case AssetType.Audio:
                            {
                                audio = Request.assetBundle.mainAsset as AudioClip;
                                Request.assetBundle.Unload(false);
                                break;
                            }
                    }
                    Request.Dispose();

                    Request = null;
                }
            }
        }
        private void operateAssetUnload()
        {

            if (State == AssetRequestState.LoadComplete&& RequestType == AssetRequestType.Default)
            {
                if (KeepWWW)
                {

                    if (Request == null) return;

                    switch (Type)
                    {
                        case AssetType.AssetBundle:
                            {
                                Request.assetBundle.Unload(true);
                                objs = null;
                                mainAssetObject = null;
                                break;
                            }
                        case AssetType.Texture:
                            {
                                GameObject.DestroyImmediate(texture, true);
                                texture = null;
                                break;
                            }
                        case AssetType.Audio:
                            {
                                GameObject.DestroyImmediate(audio, true);
                                audio = null;
                                break;
                            }
                    }
                    Request.Dispose();

                    Request = null;

                }
                else
                {
                    switch (Type)
                    {
                        case AssetType.AssetBundle:
                            {
                                GameObject.DestroyImmediate(mainAssetObject, true);
                                if (objs != null && objs.Length > 0)
                                {
                                    for (int i = 0; i < objs.Length; i++)
                                    {
                                        mainAssetObject = objs[i];
                                        GameObject.DestroyImmediate(mainAssetObject, true);
                                    }
                                }
                                break;
                            }
                        case AssetType.Texture:
                            {
                                GameObject.Destroy(texture,1);
                                texture = null;
                                break;
                            }
                        case AssetType.Audio:
                            {
                                GameObject.DestroyImmediate(audio, true);
                                audio = null;
                                break;
                            }
                    }
                }
            }
        }

        public bool Contains(string name)
        {
            return Request.assetBundle.Contains(name);
        }

        public UnityEngine.Object Load(string name)
        {
            if (!KeepWWW)
            {
                return GetObjs(name);
            }

            return Request.assetBundle.Load(name);
        }
        public UnityEngine.Object Load(string name, Type type)
        {
            if (!KeepWWW)
            {
                return GetObjs(name,type);
            }

            return Request.assetBundle.Load(name, type);
        }
        public AssetBundleRequest LoadAsyncState(string name, Type type)
        {

            return Request.assetBundle.LoadAsync(name, type);
        }
        /*
        public void LoadObjectFromAssetBundle(string name, Type type, Action<UnityEngine.Object> callBack)
        {
            //callBack(Request.assetBundle.Load(name, type));
            //Define.Mono.StartCoroutine(loadObjectFromAssetBundle(name, type, callBack));
            LoadHelp.AddObjectRequest(this, name, type, callBack);
        }
        private IEnumerator loadObjectFromAssetBundle(string name, Type type, Action<UnityEngine.Object> callBack)
        {
            yield return 0;
            //long t = DateTime.Now.Ticks;
            if (Request.assetBundle == null) Debug.LogError("loadObjectFromAssetBundle errror");
            AssetBundleRequest req = Request.assetBundle.LoadAsync(name, type);
            //Debug.Log(req.priority);
            //t = DateTime.Now.Ticks - t;
            //Debug.Log("loadObjectFromAssetBundle:" + t);
            while (!req.isDone)
            {
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForEndOfFrame();
            callBack(req.asset);
        }
         * */

        public byte[] Bytes
        {
            get 
            {
                return Request.bytes;
            }
        }
    }
    public enum AssetRequestState
    {
        None = 0,
        BeginCheckFile,
        EndCheckFile,
        BeginLoad,
        Loading,
        LoadComplete,
        LoadFail,
        Dispose
    }
    public enum AssetRequestType
    {
        Default = 0,
        OtherDomain
    }
    public class AssetRequestOperation
    {
        public AssetRequestOperation(AssetRequestOperationHandle start, AssetRequestOperationHandle update)
        {
            StartHandle = start;
            UpdateHandle = update;
        }
        public AssetRequestOperationHandle StartHandle;
        public AssetRequestOperationHandle UpdateHandle;
    }
    public delegate void AssetRequestOperationHandle(AssetRequest request);
    public interface IAssetRequest
    {
        float Process { get; }
        string Path { get; }
        byte[] Bytes { get; }
        void Dispose();
        AssetBundleRequest LoadAsyncState(string name, Type type);
        UnityEngine.Object Load(string name);
        UnityEngine.Object Load(string name, Type type);
        int UsedCount { get; set; }
        AssetType Type { get; set; }
        UnityEngine.Object mainAsset { get; }
        string Text { get;}
    }
}
