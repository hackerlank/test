using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Monster
{
    [XmlAttribute("name")] public string Name;
    public int Health;

    public string Des()
    {
        return ToString() + ".Name = " + Name + "; Health = " + Health;
    }
}

[XmlRoot("MonsterCollection")]
public class MonsterContainer
{
    [XmlArray("Monsters"), XmlArrayItem("Monster")]
    public Monster[] Monsters;
    /// <summary>
    /// 加载xml文件
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public static MonsterContainer Load(string file)
    {

#if UNITY_EDITOR
        var serializer = new XmlSerializer(typeof(MonsterContainer));
        using (var fs = new FileStream(file, FileMode.Open))
        {
            return serializer.Deserialize(fs) as MonsterContainer;
        }
#else
        return null;
#endif
    }
    /// <summary>
    /// 保存xml文件
    /// </summary>
    /// <param name="file"></param>
    public void Save(string file)
    {
        var serializer = new XmlSerializer(typeof(MonsterContainer));
        using (var fs = new FileStream(file, FileMode.Create))
        {
            serializer.Serialize(fs, this);
        }
    }

    public string Des ()
    {
        string des = ToString() + " is ";
        foreach (var monster in Monsters)
        {
            des += monster.Des() + ";";
        }
        return des;
    }
}