using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

namespace Common
{
    public class AssetObjectRequest
    {
        public IAssetRequest Request = null;
        public string Key = string.Empty;
        public string ObjectName;
        public Type ObjectType;
        public int UseCount;
        public AssetBundleRequest State;
        public void AddTask(Action<UnityEngine.Object> callBack)
        {
            callBackList.Push(callBack);
        }
        public void ClearTask()
        {
            while (callBackList.Count > 0)
            {
                Action<UnityEngine.Object> call = callBackList.Pop();
                call(State.asset);
            }
        }
        private Stack<Action<UnityEngine.Object>> callBackList = new Stack<Action<UnityEngine.Object>>();
    }
    public class AssetByteRequest
    {
        public IAssetRequest Request = null;
        public string Key = string.Empty;
        public int UseCount;
        public AssetBundleCreateRequest State;
        public void AddTask(Action<UnityEngine.Object> callBack)
        {
            callBackList.Push(callBack);
        }
        public void ClearTask()
        {
            while (callBackList.Count > 0)
            {
                Action<UnityEngine.Object> call = callBackList.Pop();
                call(State.assetBundle.mainAsset);
            }
        }
        private Stack<Action<UnityEngine.Object>> callBackList = new Stack<Action<UnityEngine.Object>>();
    }
}
