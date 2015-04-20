using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NGUIConfig : ScriptableObject
{
    public List<NGUIConfig_Data> UIConfig = new List<NGUIConfig_Data>();
}

[System.Serializable]
public class NGUIConfig_Data//每个界面属性
{
    public string strName;//界面的名字
    public List<NGUIConfig_Property> property = new List<NGUIConfig_Property>();
}

[System.Serializable]
public class NGUIConfig_Property
{
    public string strMaterialName;//对应的材质名字
    public string strMaterialTex;//对应材质的图片
}


