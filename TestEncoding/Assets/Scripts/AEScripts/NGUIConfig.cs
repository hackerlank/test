using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NGUIConfig : ScriptableObject
{
    public List<NGUIConfig_Data> UIConfig = new List<NGUIConfig_Data>();
}

[System.Serializable]
public class NGUIConfig_Data//ÿ����������
{
    public string strName;//���������
    public List<NGUIConfig_Property> property = new List<NGUIConfig_Property>();
}

[System.Serializable]
public class NGUIConfig_Property
{
    public string strMaterialName;//��Ӧ�Ĳ�������
    public string strMaterialTex;//��Ӧ���ʵ�ͼƬ
}


