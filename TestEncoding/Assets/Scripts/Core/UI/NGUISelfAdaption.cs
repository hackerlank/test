using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NGUI
{
    //设置ngui自适应方式
    public class NGUISelfAdaption
    {
        private static NGUISelfAdaption instance = null;
        public static NGUISelfAdaption Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NGUISelfAdaption();
                }
                return instance;
            }
        }


        private GameObject rootObj;
        public void SetGameObject(GameObject obj)
        {
            rootObj = obj;
            SetCameraUse(GetIsUseAnchor());
        }


        public void SetCameraUse(bool b)
        {
            if (b)//使用非拉伸的相机
            {
                rootObj.GetComponent<UIRoot>().enabled = true;
#if wqg
                UIAnchor.isUse = true;
                UIStretch.isUse = true;
#endif

                rootObj.transform.FindChild("Camera").gameObject.SetActive(true);
                rootObj.transform.FindChild("RootCamera").gameObject.SetActive(false);

            }
            else//使用拉伸相机
            {
                rootObj.GetComponent<UIRoot>().enabled = false;
                rootObj.transform.localScale = Vector3.one;
#if wqg
                UIAnchor.isUse = false;
                UIStretch.isUse = false;
#endif


                rootObj.transform.FindChild("Camera").gameObject.SetActive(false);
                rootObj.transform.FindChild("RootCamera").gameObject.SetActive(true);
            }
        }

        //判断是否使用对齐相机 true  使用非拉伸的相机   false 使用拉伸相机
        public bool GetIsUseAnchor()
        {
            //if (Application.platform == RuntimePlatform.IPhonePlayer)
            //    return true;

            return false;
        }
    }
}


