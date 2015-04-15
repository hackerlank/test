namespace NGUI
{
    using MGUI;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class NGUI_Base : MonoBehaviour
    {
        public bool isDisposeByAnimation;
        protected UIEventListener listener;
        public GUIWindowState.ChangeState stateEvent;

        protected NGUI_Base()
        {
        }

        protected void ClearAll(List<GameObject> objList)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                UnityEngine.Object.Destroy(objList[i]);
            }
            objList.Clear();
        }

        public virtual void Dispose()
        {
        }

        public virtual void DisposeByAnimation()
        {
        }

        public abstract void Init();
        protected GameObject InstantiateItem(Transform root, GameObject item)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(item) as GameObject;
            obj2.transform.parent = root;
            obj2.transform.localPosition = item.transform.localPosition;
            obj2.transform.localScale = item.transform.localScale;
            return obj2;
        }

        protected GameObject InstantiateItem(Transform root, GameObject item, Vector3 v)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate(item) as GameObject;
            obj2.transform.parent = root;
            obj2.transform.localPosition = v;
            obj2.transform.localScale = item.transform.localScale;
            return obj2;
        }

        public void OnDisable()
        {
            this.OnDisableSelf();
        }

        public virtual void OnDisableSelf()
        {
        }

        public void OnEnable()
        {
            this.OnEnableSelf();
        }

        public virtual void OnEnableSelf()
        {
        }

        public virtual void SetState()
        {
        }
    }
}

