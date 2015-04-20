using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MGUI;

namespace NGUI
{
    public abstract class NGUI_Base : MonoBehaviour
    {
        protected UIEventListener listener;
        public bool isDisposeByAnimation = false;//界面是否根据动画销毁
        public abstract void Init();
        public virtual void Dispose()
        {

        }

        public virtual void DisposeByAnimation()
        {

        }

        public GUIWindowState.ChangeState stateEvent;

        public virtual void SetState()
        {

        }

        public void OnEnable()
        {
#if wqg
            GuideManager.Instance.AddEnableWindows(name);
#endif

            OnEnableSelf();
        }

        public void OnDisable()
        {
#if wqg
            GuideManager.Instance.RemoveWindows(name);
#endif

            OnDisableSelf();
        }

        public virtual void OnEnableSelf()
        {

        }

        public virtual void OnDisableSelf()
        {

        }

        protected GameObject InstantiateItem(Transform root,GameObject item,Vector3 v)
        {
           // item.SetActive(true);
            GameObject ga = GameObject.Instantiate(item) as GameObject;
            ga.transform.parent = root;
            ga.transform.localPosition = v;
            ga.transform.localScale = item.transform.localScale;
            //item.SetActive(false);
            return ga;
        }

        protected GameObject InstantiateItem(Transform root, GameObject item)
        {
            //item.SetActive(true);
            GameObject ga = GameObject.Instantiate(item) as GameObject;
            ga.transform.parent = root;
            ga.transform.localPosition = item.transform.localPosition;
            ga.transform.localScale = item.transform.localScale;
            //item.SetActive(false);
            return ga;
        }

        protected void ClearAll(List<GameObject> objList)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                GameObject.Destroy(objList[i]);
            }

            objList.Clear();
        }
    }
}

