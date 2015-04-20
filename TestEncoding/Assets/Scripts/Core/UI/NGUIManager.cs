using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MGUI;

namespace NGUI
{
    public class NGUIManager
    {
        public NGUI_Bag nguiBag = null;

        private static NGUIManager instance = null;
        public static NGUIManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NGUIManager();
                    // instance.SetNguiRoot();
                }
                return instance;
            }
        }

        //设置ngui根节点
        private GameObject nguiRoot;

        public void SetNguiRoot ( GameObject obj )
        {
            nguiRoot = obj.transform.FindChild("Anchor").gameObject;
#if wangqingguo
            NGUISelfAdaption.Instance.SetGameObject(obj);
#endif
        }

        public void DisappearNGUI ()
        {
            nguiRoot.SetActive(false);
        }

        public void ShowNGUI ()
        {
            nguiRoot.SetActive(true);
        }

        public GameObject NguiRoot
        {
            get
            {
                return nguiRoot;
            }
        }
        /// <summary>
        /// 界面资源的名字、信息字典
        /// [界面资源名，界面资源信息]
        /// </summary>
        private Dictionary<string, NGUIAssetItem> nguiAssetItemDic = new Dictionary<string, NGUIAssetItem>();
        /// <summary>
        /// 根据界面资源名，获取界面资源信息
        /// </summary>
        /// <param name="strName">界面资源名</param>
        /// <returns></returns>
        public NGUIAssetItem GetNGUIAssetItemByName ( string strName )
        {
            if (nguiAssetItemDic.ContainsKey(strName))
                return nguiAssetItemDic[strName];
            return null;
        }

        //初始化一个ngui  T表示要附加到对象的逻辑代码
        public void AddByName<T> ( string strName, Action<NGUIAssetItem> callBack ) where T : NGUI_Base
        {
            Add<T>(strName, callBack, NGUIShowType.ONLYONE, false);
        }

        //初始化一个ngui  T表示要附加到对象的逻辑代码
        public void AddByName<T> ( string strName ) where T : NGUI_Base
        {
            Add<T>(strName, null, NGUIShowType.ONLYONE, false);
        }

        //1.2.3
        //初始化一个ngui  T表示要附加到对象的逻辑代码
        public void AddByName<T> ( string strFontName, string strPath, string strName ) where T : NGUI_Base
        {
            NGUI_Font.Instance.Init(NGUI_Font.Instance.GetFontPackageType(strPath), strFontName, delegate
           {
               Add<T>(strName, strPath, null, NGUIShowType.ONLYONE, false);
           });
        }


        //初始化一个ngui  T表示要附加到对象的逻辑代码 返回执行T
        public void AddByName<T> ( string strName, Action<T> callBack ) where T : NGUI_Base
        {
            AddByName<T>(strName, callBack, false);
        }

        //初始化一个ngui  T表示要附加到对象的逻辑代码 返回执行T
        /// <summary>
        /// 加载1.2.1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFontName"></param>
        /// <param name="strPath"></param>
        /// <param name="strName"></param>
        /// <param name="callBack"></param>
        public void AddByName<T> ( string strFontName, string strPath, string strName, Action<T> callBack ) where T : NGUI_Base
        {

            AddByName<T>(strFontName, strPath, strName, callBack, false);
        }

        public void AddByName<T> ( string strName, Action<T> callBack, bool bIsForbiddenAuto ) where T : NGUI_Base
        {
            Add<T>(strName, nguiAssetItem =>
            {
                if (nguiAssetItem.gameObject == null)
                {
                    PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                    callBack(null);
                }
                else
                {
                    callBack(nguiAssetItem.gameObject.GetComponent<T>());
                }

            }, NGUIShowType.ONLYONE, bIsForbiddenAuto);
        }

        /// <summary>
        /// 1.2.2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFontName"></param>
        /// <param name="strPath"></param>
        /// <param name="strName"></param>
        /// <param name="callBack"></param>
        /// <param name="bIsForbiddenAuto"></param>
        public void AddByName<T> ( string strFontName, string strPath, string strName, Action<T> callBack, bool bIsForbiddenAuto ) where T : NGUI_Base
        {
            NGUI_Font.Instance.Init(NGUI_Font.Instance.GetFontPackageType(strPath), strFontName, delegate
            {
                Add<T>(strName, strPath, nguiAssetItem =>
                {
                    if (nguiAssetItem.gameObject == null)
                    {
                        PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                        callBack(null);
                    }
                    else
                    {
                        callBack(nguiAssetItem.gameObject.GetComponent<T>());
                    }

                }, NGUIShowType.ONLYONE, bIsForbiddenAuto);
            });
        }

        //初始化一个ngui  T表示要附加到对象的逻辑代码 返回执行T  
        public void AddByName<T> ( string strName, Action<T> callBack, Action<NGUIAssetItem> callBackItem ) where T : NGUI_Base
        {
            Add<T>(strName, nguiAssetItem =>
            {
                if (nguiAssetItem.gameObject == null)
                {
                    PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                    callBack(null);
                    callBackItem(null);
                }
                else
                {
                    callBack(nguiAssetItem.gameObject.GetComponent<T>());
                    callBackItem(nguiAssetItem);
                }

            }, NGUIShowType.ONLYONE, false);
        }

        //初始化一个ngui  T表示要附加到对象的逻辑代码
        public void AddByName<T> ( string strName, NGUIShowType nguiShowType ) where T : NGUI_Base
        {
            Add<T>(strName, null, nguiShowType, false);
        }

        //1.1.1
        //初始化一个ngui  T表示要附加到对象的逻辑代码 返回执行T
        public void AddByName<T> ( string strName, NGUIShowType nguiShowType, Action<T> callBack ) where T : NGUI_Base
        {
            Add<T>(strName, nguiAssetItem =>
            {
                if (nguiAssetItem.gameObject == null)
                {
                    PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                    callBack(null);
                }
                else
                {
                    callBack(nguiAssetItem.gameObject.GetComponent<T>());
                }

            }, nguiShowType, false);

        }

        /// <summary>
        /// 初始化一个ngui  T表示要附加到对象的逻辑代码 返回执行T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strFontName">字体名称</param>
        /// <param name="strPath">资源路径</param>
        /// <param name="strName">NGUI名称</param>
        /// <param name="nguiShowType"></param>
        /// <param name="callBack"></param>
        public void AddByName<T> ( string strFontName, string strPath, string strName, NGUIShowType nguiShowType, Action<T> callBack ) where T : NGUI_Base
        {
            NGUI_Font.Instance.Init(NGUI_Font.Instance.GetFontPackageType(strPath), strFontName, delegate
            {
                Add<T>(strName, strPath, nguiAssetItem =>
                {
                    if (nguiAssetItem.gameObject == null)
                    {
                        PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                        callBack(null);
                    }
                    else
                    {
                        callBack(nguiAssetItem.gameObject.GetComponent<T>());
                    }

                }, nguiShowType, false);
            });

        }

        //XJ 实例化UI首先加载字体再加载UI
        public void AddByName<T> ( string strFontName, string strName, string strPath, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack ) where T : NGUI_Base
        {
            NGUI_Font.Instance.Init(NGUI_Font.Instance.GetFontPackageType(strPath), strFontName, delegate
            {
                Add<T>(strName, strPath, callBack, nguiShowType, false);
            });
        }

        public void AddByName<T> ( string strName, NGUIShowType nguiShowType, Action<T> callBack, bool bIsForbiddenAuto ) where T : NGUI_Base
        {
            Add<T>(strName, nguiAssetItem =>
            {
                if (nguiAssetItem.gameObject == null)
                {
                    PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                    callBack(null);
                }
                else
                {
                    callBack(nguiAssetItem.gameObject.GetComponent<T>());
                }

            }, nguiShowType, bIsForbiddenAuto);

        }

        public void AddByName<T> ( string strFontName, string strPath, string strName, NGUIShowType nguiShowType, Action<T> callBack, bool bIsForbiddenAuto ) where T : NGUI_Base
        {
            NGUI_Font.Instance.Init(NGUI_Font.Instance.GetFontPackageType(strPath), strFontName, delegate
            {
                Add<T>(strName, strPath, nguiAssetItem =>
                {
                    if (nguiAssetItem.gameObject == null)
                    {
                        PlayerHandle.Error(this, "AddByName", "资源:" + nguiAssetItem.AssetName + "逻辑顺序出现问题,出现了删除并调用的操作");
                        callBack(null);
                    }
                    else
                    {
                        callBack(nguiAssetItem.gameObject.GetComponent<T>());
                    }

                }, nguiShowType, bIsForbiddenAuto);
            });

        }
        //初始化一个ngui  T表示要附加到对象的逻辑代码
        public void AddByName<T> ( string strName, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack ) where T : NGUI_Base
        {
            Add<T>(strName, callBack, nguiShowType, false);
        }

        public void AddByNameGame<T> ( string strName, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack ) where T : NGUI_Base
        {
            Add<T>(strName, callBack, nguiShowType, false);
        }

        public void AddByNameGame<T> ( string strName, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack, bool isAuto ) where T : NGUI_Base
        {
            Add<T>(strName, callBack, nguiShowType, isAuto);
        }

        public void AddByName ( string strName, string strPath, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack )
        {
            Add(strName, strPath, callBack, nguiShowType, false);
        }

        public void AddByName ( string strName, string strPath, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack, bool bIsForbiddenAuto )
        {
            Add(strName, strPath, callBack, nguiShowType, bIsForbiddenAuto);
        }

        public void AddByName<T> ( string strName, string strPath, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack ) where T : NGUI_Base
        {
            Add<T>(strName, strPath, callBack, nguiShowType, false);
        }



        private string strNone = "";
        //1.1.2
        /// <summary>
        /// 加载UI资源strName，然后实例化一个UI资源，并将逻辑脚本T挂到资源对象上。(初始化一个ngui  T表示要附加到对象的逻辑代码)。加载完成后，执行回调函数。
        /// 创建NGUIAssetItem，加载并实例化UI资源，将UI资源添加到UI资源的根节点，添加逻辑脚本，并添加到字典进行管理
        /// </summary>
        /// <typeparam name="T">界面逻辑脚本</typeparam>
        /// <param name="strName">界面资源名</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="nguiShowType">界面显示方式</param>
        /// <param name="bIsForbiddenAuto">是否禁止 自动删除 自动不显示界面</param>
        private void Add<T> ( string strName, Action<NGUIAssetItem> callBack, NGUIShowType nguiShowType, bool bIsForbiddenAuto ) where T : NGUI_Base
        {
            if (nguiAssetItemDic.ContainsKey(strName))
            {
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    NGUIAssetItem value = nguiAssetItemDic[strName];
                    if (value.prefab == null)
                    {
                        PlayerHandle.Error(this, "Add", "加载不到资源:" + value.AssetName + "添加回调函数");
                        value.CallbackAction += delegate(NGUIAssetItem item)
                        {
                            value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                            value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                            value.gameObject.AddComponent<T>();

                            SaveNewObjectProperty(value);
                            value.gameObject.transform.parent = nguiRoot.transform;
                            SetNewObjectProperty(callBack, value);
                            value.GameObjects.Add(value.gameObject);//如果需要支持多个，则添加到对象列表
                        };//有可能是资源尚未加载完，先放到保存到回调中，以备后续加载。
                        value.CallbackAction += delegate( NGUIAssetItem a ) { Debug.Log("=============first call" + a.nguiAssetType); };
                        return;
                    }

                    value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                    value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                    value.gameObject.AddComponent<T>();

                    SaveNewObjectProperty(value);
                    value.gameObject.transform.parent = nguiRoot.transform;
                    SetNewObjectProperty(callBack, value);
                    value.GameObjects.Add(value.gameObject);//如果需要支持多个，则添加到对象列表。
                    return; ;
                }

                if (callBack != null)
                {

                    NGUIAssetItem assetItem = nguiAssetItemDic[strName];

                    CallBackLoadedItem<T>(strName, assetItem);

                    if (!assetItem.bIsForbiddenAuto)
                    {
                        MonoThread.Instance.StartCoroutine(YieldTimeDelay(0.1f, delegate
                        {
                            GUIWindowState.ExcuteChangeStateEvent();
                            AutoDeleteUIExceptName(strName);
                            callBack(assetItem);
                        }));
                    }
                    else
                    {
                        callBack(assetItem);
                    }

                }

                return;
            }
            Debug.Log("开始加载资源：" + strName);
            //创建NGUIAssetItem，加载并实例化UI资源，将UI资源添加到UI资源的根节点，添加逻辑脚本，并添加到字典进行管理
            NGUIAssetHelp.LoadAsset(NewAssetItem(strName, nguiShowType, bIsForbiddenAuto), value =>
            {
                Debug.Log("已经加载资源：" + strName + " ;" + (value.prefab == null));
                if (value.prefab == null)
                {
                    Debug.LogError("加载不到资源：" + strName + " ;" + (value.prefab == null));
                    PlayerHandle.Error(this, "Add", "加载不到资源:" + value.AssetName);
                    return;
                }

                if (CheckObjectHasRemoved(callBack, value))
                {
                    Debug.LogError("已经删除资源：" + strName);
                    return;
                }
               
                Debug.Log("已经实例化资源：" + strName);
                value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                value.gameObject.AddComponent<T>();
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    value.GameObjects.Add(value.gameObject);//如果需要支持多个，则添加到对象列表。
                }
                SaveNewObjectProperty(value);
                value.gameObject.transform.parent = nguiRoot.transform;
                SetNewObjectProperty(callBack, value);
                //看看有没有延迟的回调续要调用
                if (value.CallbackAction != null)
                {
                    value.CallbackAction(value);
                    value.CallbackAction = null;//加载完成后，调用所有回调，之后清空。
                }
            });
            
                
        }

        #region 回调的模式初始化
        private void CallBackLoadedItem<T> ( string strName, NGUIAssetItem assetItem ) where T : NGUI_Base
        {
            if (assetItem.gameObject != null)
            {
                T t = assetItem.gameObject.GetComponent<T>();
                t.OnEnable();
                t.enabled = true;

                assetItem.gameObject.SetActive(true);
                //ResetPosition(strName);
            }
            else
                PlayerHandle.Error("Add: " + strName + " 为空!对象已被删除");
        }

        #endregion





        public IEnumerator YieldTimeDelay ( float tTime, Action callBack )
        {
            //yield return new WaitForSeconds(tTime);
            yield return new WaitForEndOfFrame();
            callBack();
        }

        //3.1.1
        private void Add<T> ( string strName, string assetPath, Action<NGUIAssetItem> callBack, NGUIShowType nguiShowType, bool bIsForbiddenAuto ) where T : NGUI_Base
        {
            if (nguiAssetItemDic.ContainsKey(strName))
            {
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    NGUIAssetItem value = nguiAssetItemDic[strName];
                    if (value.prefab == null)
                    {
                        PlayerHandle.Error(this, "Add", "加载不到资源:" + assetPath + "    " + value.AssetName);
                        return;
                    }

                    value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                    value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                    value.gameObject.AddComponent<T>();
                    SaveNewObjectProperty(value);
                    value.gameObject.transform.parent = nguiRoot.transform;
                    SetNewObjectProperty(callBack, value);
                    value.GameObjects.Add(value.gameObject);
                    return;
                }
                if (callBack != null)
                {
                    NGUIAssetItem assetItem = nguiAssetItemDic[strName];
                    CallBackLoadedItem<T>(strName, assetItem);
                    if (!assetItem.bIsForbiddenAuto)
                    {
                        MonoThread.Instance.StartCoroutine(YieldTimeDelay(0.1f, delegate
                        {
                            GUIWindowState.ExcuteChangeStateEvent();
                            AutoDeleteUIExceptName(strName);
                            callBack(assetItem);
                        }));

                    }
                    else
                    {
                        callBack(assetItem);
                    }
                }

                return;
            }

            NGUIAssetHelp.LoadAsset(NewAssetItem(strName, nguiShowType, bIsForbiddenAuto), assetPath, value =>
            {

                if (value.prefab == null)
                {
                    PlayerHandle.Error(this, "Add", "加载不到资源:" + assetPath + "    " + value.AssetName);
                    return;
                }

                if (CheckObjectHasRemoved(callBack, value))
                    return;

                value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                value.gameObject.AddComponent<T>();
                SaveNewObjectProperty(value);
                value.gameObject.transform.parent = nguiRoot.transform;
                SetNewObjectProperty(callBack, value);
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    value.GameObjects.Add(value.gameObject);
                }

            });
        }

        private void Add ( string strName, string strPath, Action<NGUIAssetItem> callBack, NGUIShowType nguiShowType, bool bIsForbiddenAuto )
        {
            if (nguiAssetItemDic.ContainsKey(strName))
            {
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    NGUIAssetItem value = nguiAssetItemDic[strName];
                    if (value.prefab == null)
                    {
                        PlayerHandle.Error(this, "Add", "加载不到资源:" + strPath + "    " + value.AssetName);
                        return;
                    }

                    value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                    value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                    SaveNewObjectProperty(value);
                    value.gameObject.transform.parent = nguiRoot.transform;
                    SetNewObjectProperty(callBack, value);
                    value.GameObjects.Add(value.gameObject);
                    return;
                }
                if (callBack != null)
                {
                    NGUIAssetItem assetItem = nguiAssetItemDic[strName];
                    assetItem.gameObject.SetActive(true);
                    //ResetPosition(strName);

                    callBack(assetItem);
                }

                return;
            }

            NGUIAssetHelp.LoadAsset(NewAssetItem(strName, nguiShowType, bIsForbiddenAuto), strPath, value =>
            {
                if (value.prefab == null)
                {
                    PlayerHandle.Error(this, "Add", "加载不到资源:" + strPath + "     " + value.AssetName);
                    return;
                }

                if (CheckObjectHasRemoved(callBack, value))
                    return;

                value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                SaveNewObjectProperty(value);
                value.gameObject.transform.parent = nguiRoot.transform;
                SetNewObjectProperty(callBack, value);
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    value.GameObjects.Add(value.gameObject);
                }
            });
        }



        //加载3D UI
        public void Add3DUIByName<T> ( string strName, string strPath, NGUIShowType nguiShowType, Action<NGUIAssetItem> callBack ) where T : NGUI_Base
        {
            if (nguiAssetItemDic.ContainsKey(strName))
            {

                if (nguiShowType == NGUIShowType.MULTI)
                {
                    NGUIAssetItem value = nguiAssetItemDic[strName];
                    if (value.prefab == null)
                    {
                        PlayerHandle.Error("加载不到资源:" + value.AssetName);
                        return;
                    }

                    value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                    value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                    value.gameObject.AddComponent<T>();

                    SaveNewObjectProperty(value);
                    SetNewObjectProperty(callBack, value);
                    value.GameObjects.Add(value.gameObject);
                    return;
                }
                if (callBack != null)
                {
                    NGUIAssetItem assetItem = nguiAssetItemDic[strName];
                    CallBackLoadedItem<T>(strName, assetItem);
                    callBack(assetItem);
                }
                return;
            }

            NGUIAssetHelp.LoadAsset(NewAssetItem(strName, nguiShowType, false), strPath, value =>
            {
                if (value.prefab == null)
                {
                    PlayerHandle.Error("加载不到资源:" + value.AssetName);
                    return;
                }

                if (CheckObjectHasRemoved(callBack, value))
                    return;

                value.gameObject = GameObject.Instantiate(value.prefab) as GameObject;
                value.gameObject.name = value.gameObject.name.Substring(0, value.gameObject.name.Length - 7);
                value.gameObject.AddComponent<T>();

                SaveNewObjectProperty(value);
                SetNewObjectProperty(callBack, value);
                if (nguiShowType == NGUIShowType.MULTI)
                {
                    value.GameObjects.Add(value.gameObject);
                }
            });
        }
        /// <summary>
        /// 创建一个新的NGUIAssetItem，初始化部分数据(另一部分数据需要加载完成后初始化)，并添加到字典中进行管理
        /// </summary>
        /// <param name="strName">UI资源名</param>
        /// <param name="nguiShowType">界面显示类型</param>
        /// <param name="bIsForbiddenAuto">是否禁止 自动删除 自动不显示界面</param>
        /// <returns></returns>
        private NGUIAssetItem NewAssetItem ( string strName, NGUIShowType nguiShowType, bool bIsForbiddenAuto )
        {
            NGUIAssetItem item = new NGUIAssetItem();
            item.AssetName = strName;
            item.bIsForbiddenAuto = bIsForbiddenAuto;

            item.nguiShowType = nguiShowType;
            nguiAssetItemDic.Add(strName, item);

            item.fStartTime_Instantiate = Time.realtimeSinceStartup;
            return item;
        }

        private Vector3 vP, vS, vR;
        private void SaveNewObjectProperty ( NGUIAssetItem value )
        {
            vP = value.gameObject.transform.localPosition;
            vS = value.gameObject.transform.localScale;
            vR = value.gameObject.transform.localEulerAngles;
        }

        private void SetNewObjectProperty ( Action<NGUIAssetItem> callBack, NGUIAssetItem value )
        {
            value.gameObject.transform.localPosition = vP;
            value.gameObject.transform.localScale = vS;
            value.gameObject.transform.localEulerAngles = vR;
            //PlayerHandle.Debug(this, "SetNewObjectProperty", " 实例化NGUI界面:" + value.AssetName + ",  总消耗时间:" + value.GetInstantiate(Time.realtimeSinceStartup));

            if (!value.bIsForbiddenAuto)
            {
                GUIWindowState.ExcuteChangeStateEvent();
                AutoDeleteUIByName();
            }

            if (callBack != null)
            {
                callBack(value);
                callBack = null;
            }
        }

        private void CheckIsFinishedLoadAll ()
        {

        }


        private bool CheckObjectHasRemoved ( Action<NGUIAssetItem> callBack, NGUIAssetItem item )//玩家快速切换界面  还没初始化完成 界面就删了 
        {
            if (!item.isRemoved)
                return false;

            DisposeAllItemData(item);
            callBack = null;
            return true;
        }

        //界面的唯一性 自动删除界面
        public void AutoDeleteUI ()
        {
            foreach (KeyValuePair<string, NGUIAssetItem> key in nguiAssetItemDic)
            {
                switch (key.Value.nguiShowType)
                {
                    case NGUIShowType.ONLYONE:
                        autoDeletelist.Add(key.Key);
                        break;
                    case NGUIShowType.ACTIVE_FALSE:
                        autoActivelist.Add(key.Key);
                        //key.Value.gameObject.SetActive(false);
                        break;
                }
            }
        }

        private List<string> autoDeletelist = new List<string>();
        private List<string> autoActivelist = new List<string>();
        public void AutoDeleteUIByName ()
        {
            AutoDeleteUIExceptName(strNone);
        }

        public void AutoDeleteUIByNameYield ()
        {
            MonoThread.Instance.StartCoroutine(AutoDeleteUIByNameYieldEnd());
        }


        private IEnumerator AutoDeleteUIByNameYieldEnd ()
        {
            yield return new WaitForEndOfFrame();
            AutoDeleteUIExceptName(strNone);
        }

        public void AutoDeleteUIExceptName ( string strExceptName )
        {
            for (int i = 0; i < autoDeletelist.Count; i++)
            {
                DeleteByName_ForceUI(autoDeletelist[i]);
            }

            //Debug.LogError(strExceptName);

            for (int i = 0; i < autoActivelist.Count; i++)
            {
                string strName = autoActivelist[i];
                if (strName == strExceptName) continue;
                if (!nguiAssetItemDic.ContainsKey(strName)) continue;

                DisActiveByName(strName);
            }

            autoDeletelist.Clear();
            autoActivelist.Clear();
        }
        /// <summary>
        /// 多个实例的处理，删除其中一个实例，如果所有实例都删除，则删除NGUIAssetItem
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="script"></param>
        public void DeleteByNameAndScript ( string strName, NGUI_Base script )
        {
            //Debug.LogError("DeleteByName"+strName);
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;

            NGUIAssetItem itm = nguiAssetItemDic[strName];
            if (itm.gameObject == null)
            {
                itm.prefab = null;
                return;
            }


            bool bIsDestroy = true;
            GameObject gameObj = itm.GameObjects.Find(go => go == script.gameObject);
            try
            {
                NGUI_Base baseScript = script;
                if (baseScript != null)
                {
                    baseScript.Dispose();//自定义的界面数据删除

                    if (baseScript.isDisposeByAnimation)//自定义界面根据动画删除 
                        baseScript.DisposeByAnimation();

                    bIsDestroy = !baseScript.isDisposeByAnimation;//上面顺序执行对了 才会执行这行 异常直接抛出 管理器还能删除该异常界面

                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError(itm.AssetName);
                PlayerHandle.Error("NGUIManager", "DisposeAllItemData", "删除界面 自定义 Dispose  DisposeByAnimation 出现异常消息:" + ex.Message + "\n" + ex.StackTrace);
                throw;
            }

            if (bIsDestroy)
            {
                gameObj.SetActive(false);
                GameObject.Destroy(gameObj);
                itm.GameObjects.Remove(gameObj);
                if (itm.GameObjects.Count <= 0)
                {
                    itm.isRemoved = true;
                    nguiAssetItemDic.Remove(strName);
                    itm.prefab = null;
                }
                else
                {
                    itm.gameObject = itm.GameObjects[itm.GameObjects.Count - 1];//gameObject指向最后创建的对象
                }
            }
        }
        public void DeleteByName ( string strName )
        {
            //Debug.LogError("DeleteByName"+strName);
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;

            NGUIAssetItem itm = nguiAssetItemDic[strName];
            itm.isRemoved = true;
            nguiAssetItemDic.Remove(strName);

            DisposeAllItemData(itm);
        }

        public void DeleteByName_ForceUI ( string strName )//强制该ui唯一使用的删除图片
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;

            NGUIAssetItem itm = nguiAssetItemDic[strName];
            itm.isRemoved = true;
            nguiAssetItemDic.Remove(strName);

            DisposeAllItemData(itm);

            NGUIAtlasHelp.Instance.DeleteByPack_UI(itm.strPack, itm.AssetName);
        }

        public void DeleteByName_ForceUIWWW ( string strName )//强制该ui唯一使用的删除图片 并删除预解压文件
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;

            NGUIAssetItem itm = nguiAssetItemDic[strName];
            itm.isRemoved = true;
            nguiAssetItemDic.Remove(strName);

            DisposeAllItemData(itm);

            NGUIAtlasHelp.Instance.DeleteByPackWWW_UI(itm.strPack, itm.AssetName);
        }

        public void DisActiveByName ( string strName )
        {
            //PlayerHandle.Error(this, "DisActiveByName",strName);

            if (!nguiAssetItemDic.ContainsKey(strName))
                return;

            NGUIAssetItem itm = nguiAssetItemDic[strName];

            //Guide.Instance.RemoveWindows(itm.gameObject.name);

            itm.gameObject.SetActive(false);
            //添加支持多个实例
            foreach (var go in itm.GameObjects)
            {
                go.SetActive(false);
            }
            //ResetFarPosition(strName);
        }

        public GameObject GetDlgRootByName(string strName)
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return null;

            return nguiAssetItemDic[strName].gameObject;
        }

        public T GetDlgByName<T>(string strName) where T : Component
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return null;

            return nguiAssetItemDic[strName].gameObject.GetComponent<T>();
        }

        private static void DisposeAllItemDataGameObjectList(NGUIAssetItem itm)
        {
            
        }
        private static void DisposeAllItemData ( NGUIAssetItem itm )
        {
            if (itm.gameObject == null)
            {
                itm.prefab = null;
                return;
            }


            bool bIsDestroy = true;

            try
            {
                if (itm.GameObjects.Count == 0)
                {
                    NGUI_Base baseScript = itm.gameObject.GetComponent<NGUI_Base>();
                    if (baseScript != null)
                    {
                        baseScript.Dispose(); //自定义的界面数据删除

                        if (baseScript.isDisposeByAnimation) //自定义界面根据动画删除 
                            baseScript.DisposeByAnimation();

                        bIsDestroy = !baseScript.isDisposeByAnimation; //上面顺序执行对了 才会执行这行 异常直接抛出 管理器还能删除该异常界面

                    }
                }
                else
                {
                    //添加支持多个实例
                    foreach (var go in itm.GameObjects)
                    {
                            NGUI_Base baseScript =go.GetComponent<NGUI_Base>();
                            if (baseScript != null)
                            {
                                baseScript.Dispose();//自定义的界面数据删除

                                if (baseScript.isDisposeByAnimation)//自定义界面根据动画删除 
                                    baseScript.DisposeByAnimation();

                                bIsDestroy = !baseScript.isDisposeByAnimation;//上面顺序执行对了 才会执行这行 异常直接抛出 管理器还能删除该异常界面
                                if (bIsDestroy)
                                {
                                    go.SetActive(false);
                                    GameObject.Destroy(go);
                                }
                            }
                    }
                }

            }
            catch (System.Exception ex)
            {
                Debug.LogError(itm.AssetName);
                PlayerHandle.Error("NGUIManager", "DisposeAllItemData", "删除界面 自定义 Dispose  DisposeByAnimation 出现异常消息:" + ex.Message + "\n" + ex.StackTrace);
            }


            if (bIsDestroy)
            {
                itm.gameObject.SetActive(false);
                GameObject.Destroy(itm.gameObject);
            }

            itm.prefab = null;
        }

        public void DeleteAllWindowBesideMenu ()
        {
            //return;
            int nDeleteCount = nguiAssetItemDic.Count;

            while (nDeleteCount > 0)
            {
                nDeleteCount = nguiAssetItemDic.Count;

                foreach (KeyValuePair<string, NGUIAssetItem> item in nguiAssetItemDic)
                {


                    nDeleteCount--;
                    if (item.Key != "NGUI_Menu")
                        if (item.Value.nguiAssetType == NGUIAssetType.INSCENE)
                        {
                            DeleteByName(item.Key);
                            break;
                        }

                }
            }


        }

        public void DeleteAllWindow ()
        {
            //return;
            int nDeleteCount = nguiAssetItemDic.Count;

            while (nDeleteCount > 0)
            {
                nDeleteCount = nguiAssetItemDic.Count;

                foreach (KeyValuePair<string, NGUIAssetItem> item in nguiAssetItemDic)
                {

                    PlayerHandle.Debug("------------------>" + item.Key);
                    nDeleteCount--;
                    if (item.Value.nguiAssetType == NGUIAssetType.INSCENE)
                    {
                        DeleteByName(item.Key);
                        break;
                    }

                }
            }


        }

        public void PrefabDeleteByLoadHelp ()
        {
            foreach (string s in nguiAssetItemDic.Keys)
            {
                if (nguiAssetItemDic[s].prefab == null)
                {
                    PlayerHandle.Warning(this, "PrefabDeleteByLoadHelp", "LoadHelp删除了" + s + "的assetbundle , 但是ui管理器里面还存在相应的界面,逻辑调用错误！");
                }
            }
        }

        public void ActiveByName ( string strName )
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;
            NGUIAssetItem itm = nguiAssetItemDic[strName];
            if (itm.gameObject != null)
                itm.gameObject.SetActive(true);
            ////添加支持多个实例
            foreach ( var go  in itm.GameObjects)
            {
                if (go != null)
                {
                    go.SetActive(true);
                }
            }
            //ResetPosition(strName);
        }

        public bool IsContainByName ( string strName )
        {
            if (nguiAssetItemDic.ContainsKey(strName))
                return true;
            return false;
        }


        public bool IsContainByNameHaveLoaded ( string strName )
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return false;

            NGUIAssetItem item = nguiAssetItemDic[strName];
            if (item.prefab == null)
                return false;
            return true;
        }


        #region 位置设置

        public static Vector3 vNGUIDefault = new Vector3(-100.3241f, 0, 0);
#if false  //这里的函数没有用到，注释掉
        private Vector3 vFarPosition = new Vector3(0, 2000, 0);
        public void ResetFarPosition ( string strName )
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;
            NGUIAssetItem itm = nguiAssetItemDic[strName];
            if (itm.gameObject == null) return;

            if (itm.vOriginPos == vNGUIDefault)
                itm.vOriginPos = itm.gameObject.transform.localPosition;

            itm.gameObject.transform.localPosition = vFarPosition;
        }

        public void ResetPosition ( string strName )
        {
            if (!nguiAssetItemDic.ContainsKey(strName))
                return;

            NGUIAssetItem itm = nguiAssetItemDic[strName];
            if (itm.gameObject == null) return;

            if (itm.vOriginPos == vNGUIDefault) return;

            itm.gameObject.transform.localPosition = itm.vOriginPos;
        }
#endif
        #endregion
    }
}


