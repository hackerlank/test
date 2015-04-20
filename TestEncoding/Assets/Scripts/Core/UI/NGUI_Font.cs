using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
namespace NGUI
{
    public enum PackageFontType
    {
        None,
        COMON,
        LOGIN,
        MAP,
        MENU,
        FIGHT,
        SECRET,
        SECRETMAP
    }
    public class NGUI_FontSys
    {
        public PackageFontType _packageType;
        public UILabel m_simKaiN_30_1;
        public UILabel m_simKaiI_30_0;
        public UILabel m_simKaiB_30_0;
        public UILabel m_simKaiN_24_1;
        public UILabel m_simKai_24_4;
        public UILabel m_simKaiN_36_5;
        private Font m_simKaiFont = null;
        private Transform root;
        private NGUIAssetItem m_nguiFont;
        private string _strPackageName;
        private void InitLabelInfo()
        {

            SetFontByName("lbSimKai");
            SetFontByName("lbSimKai_B");
            SetFontByName("simkai_B_30_0");
            SetFontByName("lbSimKai_BI");
            SetFontByName("lbSimKai_M");
            SetFontByName("simkai_skillName");
            SetFontByName("simkai_N_24_5");
            SetFontByName("simkai_N_30_5");
            m_simKaiN_24_1 = SetFontByName("simkai_N_24_1");
            m_simKaiN_30_1 = SetFontByName("simkai_N_30_1");
            SetFontByName("simkai_N_18_1");
            SetFontByName("simkai_I_24_1");
            m_simKaiB_30_0 = SetFontByName("simkai_I_30_0");
            m_simKaiN_36_5 = SetFontByName("simkai_N_36_5");
            SetFontByName("simkai_N_18_5");
            SetFontByName("simkai_N_36_1");
            m_simKai_24_4 = SetFontByName("simkai_N_24_4");
            SetFontByName("simkai_N_48_1");
            SetFontByName("simkai_I_100_1");
        }
        private UILabel SetFontByName(string strName)
        {
            Transform t = root.FindChild(strName);
            if (t == null)
            {
                return null;
            }

            UILabel lab = t.GetComponent<UILabel>();
            if (lab == null)
            {
                return null;
            }
            lab.font.dynamicFont = m_simKaiFont;

            return lab;
        }

        public void Init(PackageFontType type, string strPackageName, Action CallBack, Font simKaiFont)
        {
            m_simKaiFont = simKaiFont;
            _packageType = type;
            _strPackageName = strPackageName;
            string strPath = NGUI_Font.Instance.GetFontPackageName(type);
            NGUIManager.Instance.AddByName( _strPackageName,strPath, NGUIShowType.SELF, delegate(NGUIAssetItem obj)
            {
                m_nguiFont = obj;
                root = m_nguiFont.gameObject.transform;

                InitLabelInfo();
                CallBack();
            });
        }

        public void Dispose()
        {
            string strPath = NGUI_Font.Instance.GetFontPackageName(_packageType);
            NGUIManager.Instance.DeleteByName(_strPackageName);
            m_nguiFont = null;
        }
    }

    public class NGUI_Font
    {
        public string GetFontPackageName(PackageFontType fontType)
        {
            string name = "";
            switch (fontType)
            {
                case PackageFontType.LOGIN:
                    return NGUIAssetHelp.AssetLoginPath;
                case PackageFontType.MENU:
                    return NGUIAssetHelp.AssetAbsPath;
                case PackageFontType.FIGHT:
                    return NGUIAssetHelp.AssetFightPath;
                case PackageFontType.MAP:
                    return NGUIAssetHelp.AssetNewMapPath;
                case PackageFontType.SECRET:
                    return NGUIAssetHelp.AssetSecretUIPath;
                case PackageFontType.SECRETMAP:
                    return NGUIAssetHelp.AssetSecretMapPath;
                case PackageFontType.COMON:
                    return NGUIAssetHelp.AssetCommonPath;
            }
            return name;
        }
        public PackageFontType GetFontPackageType(string strPath)
        {
            switch (strPath)
            {
                case NGUIAssetHelp.AssetLoginPath:
                    return PackageFontType.LOGIN;

                case NGUIAssetHelp.AssetAbsPath:
                    return PackageFontType.MENU;

                case NGUIAssetHelp.AssetFightPath:
                    return PackageFontType.FIGHT;

                case NGUIAssetHelp.AssetNewMapPath:
                    return PackageFontType.MAP;

                case NGUIAssetHelp.AssetSecretUIPath:
                    return PackageFontType.SECRET;
                    
                case NGUIAssetHelp.AssetSecretMapPath:
                    return PackageFontType.SECRETMAP;

                case NGUIAssetHelp.AssetCommonPath:
                    return PackageFontType.COMON;

                default:
                    return PackageFontType.None;
            }
        }
        private static NGUI_Font instance;

        public static NGUI_Font Instance
        {
            get{
                if (instance == null)
                    instance = new NGUI_Font();
                return instance;
            }

        }
        private Font m_simKaiFont = null;
        private void InitFont()
        {
            m_simKaiFont = m_nguiFontLoad.gameObject.transform.FindChild("LB_Font").GetComponent<UILabel>().font.dynamicFont;
        }

        public Font GetSimKaiFont()
        {
            return m_simKaiFont;
        }
        private NGUIAssetItem m_nguiFontLoad;
        public void Init(PackageFontType type, string fontPackageName, Action CallBack)
        {
            NGUIManager.Instance.AddByName(NGUI_UI.NGUI_FontLoad, NGUIAssetHelp.AssetCommonPath, NGUIShowType.SELF, delegate(NGUIAssetItem res)
            {
                m_nguiFontLoad = res;
                InitFont();
                AddFont(type, fontPackageName, CallBack);
            });
        }

        private void AddFont(PackageFontType type, string fontPackageName, Action CallBack)
        {
            if (_lstFontPackage.ContainsKey(type))
            {
                CallBack();
                return;
            }
            NGUI_FontSys _fontSys = new NGUI_FontSys();
            _lstFontPackage.Add(type, _fontSys);
            _fontSys.Init(type, fontPackageName, CallBack, m_simKaiFont);
        }

        public void Dispose(PackageFontType type)
        {
            if (!_lstFontPackage.ContainsKey(type))
                return;
            _lstFontPackage[type].Dispose();
            _lstFontPackage.Remove(type);
        }
        public void RemoveAllWithoutCommon()
        {
            NGUI_FontSys _commonPack = null;
            foreach (KeyValuePair<PackageFontType, NGUI_FontSys> key in _lstFontPackage)
            {
                if (key.Key == PackageFontType.COMON)
                {
                    _commonPack = key.Value;
                    continue;
                }
                key.Value.Dispose();
            }
            _lstFontPackage.Clear();
            if (_commonPack != null)
                _lstFontPackage.Add(PackageFontType.COMON, _commonPack);
        }
        public Dictionary<PackageFontType, NGUI_FontSys> _lstFontPackage = new Dictionary<PackageFontType, NGUI_FontSys>();
    }
}