using UnityEngine;
using System.Collections;
using Common;
using System;

namespace NGUI
{      
    public class NGUI_MainManager
    {
        private static NGUI_MainManager instance = null;
        public static NGUI_MainManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NGUI_MainManager();
                }
                return instance;
            }
        }

        GameObject m_resRoot = null;
        GameObject m_resMain = null;
        public GameObject m_rootObj;
        private GameObject m_mainObj;
        private GameObject m_mainParent;
        private GameObject m_loginObj;
        public void Init(GameObject obj)
        {
            if (obj == null)
            {
                if (m_resRoot == null)
                {
                    m_resRoot = (GameObject)Resources.Load(NGUI_UI.NGUI_Root);
                    m_rootObj = (GameObject)GameObject.Instantiate(m_resRoot, m_resRoot.transform.position, m_resRoot.transform.rotation);
                    m_rootObj.name = NGUI_UI.NGUI_Root;
                    m_mainParent = m_rootObj.transform.FindChild("Anchor").gameObject;
                    InitMain();
                }
            
            }
            else
            {
                m_rootObj = obj;

                m_mainParent = m_rootObj.transform.FindChild("Anchor").gameObject;
                m_loginObj = m_mainParent.transform.FindChild("NGUI_Main(Clone)").gameObject;
            }
        }

        public void DestoryLogin()
        {
            if (m_loginObj == null)
                return;

            m_loginObj.transform.parent = null;
            GameObject.DestroyImmediate(m_loginObj);
            m_loginObj = null;

            Resources.UnloadUnusedAssets();

        }
        private void InitMain()
        {
            return;//by wangqingguo 2015/1/7
            //if(PlayerHandle.DebugHandle != null)
            //    PlayerHandle.Debug("加载菜单" + NGUI_UI.NGUI_BackGround);

            if (m_rootObj == null) return;

            m_resMain = (GameObject)Resources.Load(NGUI_UI.NGUI_BackGround);
            m_mainObj = (GameObject)GameObject.Instantiate(m_resMain, m_rootObj.transform.position, Quaternion.identity);
            if (LastReq == null)
            LoadNext(delegate
            {
                if (WaitTexture!=null)
                WaitTexture.mainTexture = LastReq.Texture;
            });
            else
                WaitTexture.mainTexture = LastReq.Texture;
            
            m_mainObj.layer = m_mainParent.layer;
            m_mainObj.transform.parent = m_mainParent.transform;
            m_mainObj.transform.localScale = Vector3.one;
            m_mainObj.transform.localPosition = new Vector3(0, 0, 5000.0f);
           
        }
        #region 游戏背景题图
        UITexture WaitTexture 
        {

            get 
            {
                if (m_mainObj == null) return null;

                return m_mainObj.transform.FindChild("AnCenter/Back/SpriteBack").GetComponent<UITexture>();
            }
        }
        private AssetRequest LastReq;
        bool OnRequsetNextTex = false;
        string NowTexName = "";
        string TextPath = "Card/";
        private void LoadTexture(string name,Action callback)
        {


            if (OnRequsetNextTex) return;
            OnRequsetNextTex = true;
            LoadHelp.LoadTexture(TextPath + name + ".jpg", delegate(AssetRequest req)
            {
               PlayerHandle.Debug("Get Texture--->" +name);
              if (LastReq != null)
                {
                    LastReq.Dispose();
                    LastReq = null;

                }
                LastReq = req;
                NowTexName = name;
                OnRequsetNextTex = false;
                if (callback != null)
                    callback();
              
            });

        }
        string[] TexNames = new string[] { "loading1", "loading2", "loading3", "loading4", "loading5", "loading6" };
        public void LoadNext(Action callback)
        {
            string TexName = TexNames[0];
            do
            {
                TexName = TexNames[UnityEngine.Random.Range(0, TexNames.Length)];
            }
            while (TexName == NowTexName);


            LoadTexture(TexName, callback);
        }
        #endregion

        public void DisAbleMain()
        {
#if wqg
            AppInterface.GUIModule.DisposeLogin();
#endif
            if (m_mainObj == null) return;

            m_mainObj.transform.parent = null;
            GameObject.DestroyImmediate(m_mainObj);
            LoadNext(null);
            m_resMain = null;
            m_mainObj = null;
            
        }


        public void ActiveMain()
        {
            PlayerHandle.Debug("NGUI_MainManager: ActiveMain");
            if (m_mainObj == null)
            {
                InitMain();
            }
        }


        public GameObject GetAnchor
        {
            get
            {
                return m_mainParent;
            }
        }

    }
}