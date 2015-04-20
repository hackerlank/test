//#define CheckProperty//检查每个预设的图集引用
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Collections;
using Common;
using System.Linq;


namespace NGUI
{
    /// <summary>
    /// 添加注释：王庆国 2015/1/5
    /// 游戏中所有UI资源包信息管理器
    /// 管理资源包所引用贴图的引用信息，及贴图的加载
    /// </summary>
    public class NGUIAtlasHelp
    {
        private static NGUIAtlasHelp instance = null;
        public static NGUIAtlasHelp Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NGUIAtlasHelp();
                }
                return instance;
            }
        }



        private  string AssetAbsPath = @"NGUI/";

        #region 图片配置
        private NGUIConfig allNGUIConfig = null;
        public void LoadAssetNGUIData(Action callBack)
        {
            string strPack = "NGUIConfig.u";
            LoadHelp.LoadObject(AssetAbsPath + strPack, o =>
            {
                if (o == null)
                {
                    callBack();
                    return;
                }

                allNGUIConfig = o.mainAsset as NGUIConfig;

                callBack();
            });
        }
        #endregion
        public bool HasThisTet(string name) 
        {
            return allUIPackTex.ContainsKey(name);
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        /// <summary>
        /// 游戏中所有UI资源包引用的贴图信息字典
        /// [资源包名，资源包引用的图集信息]
        /// </summary>
        private Dictionary<string, NGUIAtlasData> allUIPackTex = new Dictionary<string, NGUIAtlasData>();
        /// <summary>
        /// 将资源包strPack、包内资源名strUI、贴图名strTex、贴图tex组成一个图集数据单位NGUIAtlasData_Item，并组成一个NGUIAtlasData_Item，添加到管理器allUIPackTex
        /// </summary>
        /// <param name="strPack">资源包名</param>
        /// <param name="strUI">资源名</param>
        /// <param name="strTex">图集名</param>
        /// <param name="tex">图集对象</param>
        /// <param name="itemAtlas">allUIPackTex中与strTex同名的NGUIAtlasData_Item</param>
        private void AddUIPackTex(string strPack,string strUI, string strTex, Texture2D tex,NGUIAtlasData_Item itemAtlas)//新加的资源包 图片 添加到管理器
        {
            if (allUIPackTex.ContainsKey(strPack))
            {
                NGUIAtlasData dat = allUIPackTex[strPack];
                if (!dat.CheckHasTex(strTex))
                {
                    if (itemAtlas == null)
                        dat.textureList.Add(new NGUIAtlasData_Item(strPack,strUI,strTex, tex));
                    else
                        dat.textureList.Add(itemAtlas);
                }
            }
            else
            {
                NGUIAtlasData newDat = new NGUIAtlasData(strPack);

                if (itemAtlas == null)
                    newDat.textureList.Add(new NGUIAtlasData_Item(strPack,strUI,strTex, tex));
                else
                    newDat.textureList.Add(itemAtlas);

                allUIPackTex.Add(strPack, newDat);
            }
        }


        private List<NGUIConfig_Property> GetPropertyByName(NGUIConfig config,string strName)//获取当前ui所调用的图片
        {
            if (config == null) return null;
#if CheckProperty
            String log = "Property  " + strName;
#endif
            for (int i = 0; i < config.UIConfig.Count; i++)
            {
                NGUIConfig_Data dat = config.UIConfig[i];
#if CheckProperty
                log += "\n" + dat.strName;
#endif
                if (dat.strName != strName) continue;

                return dat.property;
            }
 #if CheckProperty
            Debug.LogError(log);
#endif
            return null;
        }



        /// <summary>
        /// 检查资源包strPack内的资源strName是否需要加载新的图集，如果需要则加载新图集，并添加到管理器allUIPackTex
        /// </summary>
        /// <param name="strPack">资源包名</param>
        /// <param name="strName">资源包内的资源名</param>
        /// <param name="callBack">回调函数</param>
        public void  CheckNeedLoadAtlas(string strPack, string strName, Action callBack)//检查该资源包  新初始化的ui 所需要的图片
        {
            List<UnityEngine.Object> allDecompress = NGUIDecompressionHelp.Instance.GetPackObject(strPack);
#if CheckProperty
            string log = "<----" + strPack + "---" + strName + "-atlaslist--->";
#endif
            if (allDecompress == null || !allDecompress.Exists(finder => { return finder.name == strName; }))
            {
 #if CheckProperty
                Debug.LogError(log);
#endif
                callBack();
                return;
            }


            string strNameCfg = strPack.Replace("Icon/", "").Replace(".u", "") + "_config";

            NGUIConfig Nconfig = allDecompress.Find(finder => { return finder.name == strNameCfg; }) as NGUIConfig;
            List<NGUIConfig_Property> atlas = GetPropertyByName(Nconfig, strName);

            if (atlas == null || atlas.Count == 0)
            {
#if CheckProperty
                Debug.LogError(log + "configName:" + strNameCfg + "  Nconfig is null:" + (Nconfig == null) + "   atlas IS null:" + (atlas == null));
#endif
                callBack();
                return;
            }


            int nBackCount = 0;

            for (int i = 0; i < atlas.Count; i++)//循环ui所使用到的所有图集
            {
                NGUIConfig_Property property = atlas[i];

                Material mat = allDecompress.Find(finder => { return finder.name == property.strMaterialName && finder.GetType() == typeof(Material); }) as Material;
#if CheckProperty
                log += "\n" + property.strMaterialTex;
#endif
                CheckNeedLoadNewTexture(strPack, strName, property.strMaterialTex, delegate(NGUIAtlasData_Item tmpItem, Texture2D tex)
                {
                    AddUIPackTex(strPack, strName, property.strMaterialTex, tex, tmpItem);
                    if (tex == null) 
                    {
                        Debug.LogError(" strPack-->" + strPack + " strName-->" + strName);
                    }
                    mat.mainTexture = tex;
                    nBackCount++;

                    if (nBackCount == atlas.Count)
                    {
                        callBack();
                    }
                });
            }
#if CheckProperty
            Debug.LogError(log);
#endif
        }


        private delegate void AtlasDataCallBack(NGUIAtlasData_Item itm, Texture2D tex);
        /// <summary>
        /// 检查是否需要加载新的图集，如果需要，则加载。
        /// </summary>
        /// <param name="strPack">资源包名</param>
        /// <param name="strUI">资源包内的具体的资源名</param>
        /// <param name="strMaterialTex">资源的材质所使用的贴图名</param>
        /// <param name="callBack">回调函数</param>
       private void CheckNeedLoadNewTexture(string strPack,string strUI,string strMaterialTex,AtlasDataCallBack callBack)//检查是否需要加载新的图集
       {

           NGUIAtlasData_Item tmpItem = CheckHaveLoadSameTex(strPack,strUI, strMaterialTex);//所需要的图集是否在其它资源包里面重复使用

           if (tmpItem == null || tmpItem.tex==null)
            {
                LoadTexture(strMaterialTex, tex =>
                {
                    callBack(tmpItem,tex);
                });
            }
            else
            {
                callBack(tmpItem,tmpItem.tex);
            }
        }

        /// <summary>
       /// 检查资源包strPack里面的资源strUI所引用的贴图strName是否已经被加载，若果是则添加引用，并返回对应的NGUIAtlasData_Item，否则返回空
        /// </summary>
       /// <param name="strPack">资源包名</param>
       /// <param name="strUI">资源包内的具体的资源名</param>
       /// <param name="strName">资源的材质所使用的贴图名</param>
       /// <returns>返回allUIPackTex中与strName同名的NGUIAtlasData_Item</returns>
        private NGUIAtlasData_Item CheckHaveLoadSameTex(string strPack,string strUI,string strName)//不同的资源包引用了相同的图片
        {
            foreach (KeyValuePair<string, NGUIAtlasData> key in allUIPackTex)
            {
                List<NGUIAtlasData_Item> textureList = key.Value.textureList;

                for (int i = 0; i < textureList.Count; i++)
                {
                    NGUIAtlasData_Item itm = textureList[i];
                    if (itm.strAtlasName == strName)
                    {
                        itm.AddReferencePack(strPack,strUI);
                        return itm;
                    }
                }
            }


            return null;
        }


        #region 删除资源包所引用的图片
        public void DeleteByPack(string strPack)
        {
            return;

            if (!allUIPackTex.ContainsKey(strPack)) return;

            NGUIAtlasData dat = allUIPackTex[strPack];
            allUIPackTex.Remove(strPack);

            for (int i = 0; i < dat.textureList.Count; i++)
            {
                NGUIAtlasData_Item item = dat.textureList[i];

                if (item == null) continue;

                item.SubReferencePack(strPack);//判断引用关系是否为一

                if (item.GetReferenceCount == 0)
                {
                    LoadHelp.DeleteObject(AssetAbsPath + item.strAtlasName + ".u");
                    item = null;
                }
            }

            dat.textureList.Clear();
        }

        public void DeleteByPackWWW(string strPack)
        {

            if (!allUIPackTex.ContainsKey(strPack)) return;

            NGUIAtlasData dat = allUIPackTex[strPack];

            allUIPackTex.Remove(strPack);

            for (int i = 0; i < dat.textureList.Count; i++)
            {
                NGUIAtlasData_Item item = dat.textureList[i];

                if (item == null) continue;

                item.SubReferencePack(strPack);//判断引用关系是否为一

                if (item.GetReferenceCount == 0)
                {
                    LoadHelp.DeleteForceObject(AssetAbsPath + item.strAtlasName + ".u");
                    item = null;
                }
            }

            dat.textureList.Clear();

        }
        #endregion

        #region 删除某个ui使用的唯一ui
        public void DeleteByPack_UI(string strPack, string strUI)
        {
            if (!allUIPackTex.ContainsKey(strPack)) return;

            NGUIAtlasData dat = allUIPackTex[strPack];
            NGUIAtlasData_Item tmpItem = null;

            List<NGUIAtlasData_Item> strRemoveItem = new List<NGUIAtlasData_Item>();

            for (int i = 0; i < dat.textureList.Count; i++)
            {
                tmpItem = dat.textureList[i];

                if (tmpItem == null || !tmpItem.CheckHasUIRef(strPack, strUI)) continue;

                if (!tmpItem.CheckIsUnique) continue;

                strRemoveItem.Add(tmpItem);

            }


            for (int i = 0; i < strRemoveItem.Count; i++)
            {

                tmpItem = strRemoveItem[i];

                dat.textureList.Remove(tmpItem);

                //PlayerHandle.Error("DeleteByPack_UI: " + tmpItem.strAtlasName + ".u");

                LoadHelp.DeleteObject(AssetAbsPath + tmpItem.strAtlasName + ".u");
            }
        }

        public void DeleteByPackWWW_UI(string strPack, string strUI)
        {
            if (!allUIPackTex.ContainsKey(strPack)) return;

            NGUIAtlasData dat = allUIPackTex[strPack];
            NGUIAtlasData_Item tmpItem = null;

            List<NGUIAtlasData_Item> strRemoveItem = new List<NGUIAtlasData_Item>();

            for (int i = 0; i < dat.textureList.Count; i++)
            {
                tmpItem = dat.textureList[i];

                if (tmpItem == null || !tmpItem.CheckHasUIRef(strPack, strUI)) continue;

                if (!tmpItem.CheckIsUnique) continue;

                strRemoveItem.Add(tmpItem);

            }

            for (int i = 0; i < strRemoveItem.Count; i++)
            {

                tmpItem = strRemoveItem[i];

                dat.textureList.Remove(tmpItem);

                LoadHelp.DeleteForceObject(AssetAbsPath + tmpItem.strAtlasName + ".u");
            }
        }

        #endregion
        /// <summary>
        /// 加载贴图
        /// </summary>
        /// <param name="path">贴图的文件名</param>
        /// <param name="callBack">回调函数</param>
        private void LoadTexture(string path, Action<Texture2D> callBack)//加载图片
        {
            path += ".u";
            LoadHelp.LoadObject(AssetAbsPath + path, delegate(AssetRequest o)
            {
                callBack(o.mainAsset as Texture2D);
            });
        }



        public const string newMain = "newMain";
        public const string new_map_2 = "new_map_2";
        public void PreLoad()
        {
            PreLoadTextureByName(newMain, delegate { });
         //   PreLoadTextureByName(new_map_2, delegate { });
        }


        public void PreLoadTextureByName(string strName, Action callBack)
        {
            strName += ".u";
            LoadHelp.LoadObject(AssetAbsPath + strName, delegate(AssetRequest o)
            {
                callBack();
            });
        }

    }


    /// <summary>
    /// 资源包引用的图集信息
    /// </summary>
    public class NGUIAtlasData
    {
        /// <summary>
        /// 资源包所引用的图集资源引用信息列表
        /// </summary>
        public List<NGUIAtlasData_Item> textureList = new List<NGUIAtlasData_Item>();
        public bool CheckHasTex(string str)
        {
            for (int i = 0; i < textureList.Count; i++)
            {
                if (textureList[i].strAtlasName == str)
                    return true;
            }

            return false;
        }
       /// <summary>
       /// 资源包名
       /// </summary>
        public string strPack;
        public NGUIAtlasData(string str)
        {
            strPack=str;
        }

    }

    /// <summary>
    /// 图集资源引用信息
    /// 由贴图名strTex、贴图tex，引用该贴图的资源包strPack及具体引用该贴图的包内资源名strUI所组成一个图集数据单位
    /// 实际上可以认为是记录一个贴图的所有引用对象
    /// </summary>
    public class NGUIAtlasData_Item
    {
        /// <summary>
        /// <引用的资源包,  该资源包里面所有引用该图片的ui资源>
        /// </summary>
        private Dictionary<string, List<string>> referenceDic = new Dictionary<string, List<string>>();//引用的资源包  该资源包里面所有引用该图片的ui

        public int GetReferenceCount
        {
            get { return referenceDic.Count; }
        }

        public bool CheckIsUnique//是否该图片只被引用了一次
        {
            get
            {
                if (referenceDic.Count == 0) return false;
                if (referenceDic.Count != 1) return false;
                foreach (KeyValuePair<string, List<string>> keyValue in referenceDic)
                {
                    if (keyValue.Value.Count != 1) return false;
                }
                return true;
            }
        }


        public bool CheckHasUIRef(string strPack, string strUI)//检查该图片是否被ui引用
        {
            if (!referenceDic.ContainsKey(strPack)) return false;

            return referenceDic[strPack].Contains(strUI);
        }

        public void AddReferencePack(string str, string strUI)
        {
            List<string> tmp = new List<string>();

            if (referenceDic.ContainsKey(str))
            {
                tmp = referenceDic[str];

                //PlayerHandle.Error("AddReferencePack:    " + str + "    " + strAtlasName + "    " + strUI + "    " + tmp.Count);

                if (tmp.Contains(strUI)) { return; }

                tmp.Add(strUI);
                return;
            }


            tmp.Add(strUI);
            referenceDic.Add(str, tmp);

            //PlayerHandle.Error("AddReferencePack:    " + str + "    " + strAtlasName + "    " + strUI + "    " + tmp.Count);
        }

        public void SubReferencePack(string str)
        {
            if (!referenceDic.ContainsKey(str)) return;

            referenceDic.Remove(str);

            //PlayerHandle.Error("SubReferencePack:  " + str + "    " + strAtlasName + "    " + referenceList.Count + "   " + this.GetHashCode());
        }
        /// <summary>
        /// 图集名称
        /// </summary>
        public string strAtlasName;
        /// <summary>
        /// 图集对象
        /// </summary>
        public Texture2D tex;
        public NGUIAtlasData_Item(string strPack, string strUI, string strName, Texture2D t)
        {
            List<string> tmp = new List<string>();
            tmp.Add(strUI);
            referenceDic.Add(strPack, tmp);
            strAtlasName = strName;
            tex = t;

            //PlayerHandle.Error("NGUIAtlasData_Item:  " + strPack + "    " + strAtlasName + "    " + strUI + "      " + referenceDic.Count + "   " + this.GetHashCode());
        }
    }
}
