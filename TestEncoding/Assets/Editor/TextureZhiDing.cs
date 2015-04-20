using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class TextureZhiDing
{
    [MenuItem("Textures/Change Tga Textures for test map")]
    static void Excute() 
    {
        Texture2D[] textures = GetAllObjects<Texture2D>("Assets/Artworks/StaticModels/tga");

        foreach (var texture in textures)
        {
            string name = texture.name;

            string path = @"Assets/Artworks/StaticModels/Materials/" + name + ".mat";

            Material mat = AssetDatabase.LoadAssetAtPath(path,typeof(Material)) as Material;

            mat.mainTexture = texture;

            EditorUtility.SetDirty(mat);
        }

        AssetDatabase.Refresh();

    }

    /// <summary>
    /// 获取同一路径下的所有相同类型的实例，包含子目录
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="path">路径,相对路径</param>
    /// <returns></returns>
    public static T[] GetAllObjects<T>(string path) where T : Object
    {
        //深度便利指定的文件夹下的所有文件，包括其子文件夹
        string[] fileNames = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        int length = fileNames.Length;

        if (length == 0)
        {
            return default(T[]);
        }

        List<T> list = new List<T>();

        for (int i = 0; i < length; i++)
        {
            if (fileNames[i].IndexOf(@".meta") != -1)
            {
                continue;
            }

            int index = fileNames[i].IndexOf(@"Assets");
            string fileReletivePath = fileNames[i].Substring(index);
            Object obj = AssetDatabase.LoadAssetAtPath(fileReletivePath, typeof(T));
            if (obj != null)
            {
                if (obj is T)
                {
                    //Debug.Log("name : " + obj.name);
                    T asset = obj as T;
                    list.Add(asset);
                }
            }
        }
        return list.ToArray();

    }
}
