using System;
using System.Security;
using System.Xml;
//using UnityEditor;
using UnityEngine;
using System.Collections;
using Mono.Xml;
using System.IO;
public class TestMonoXml : MonoBehaviour
{
    [SerializeField] private string xmlFile = "test";
    public void OnGUI()
    {
        
        if (GUI.Button(new Rect(10, 10, 200, 100), "采用System.Xml保存XML文档"))
        {
            //Save(GamePath.Config + xmlFile);
        }
        if (GUI.Button(new Rect(10, 120, 200, 100), "采用Mono.Xml读取XML文档"))
        {
            //ReadBySecurityParser( GamePath.Config + xmlFile);
        }
        if (GUI.Button(new Rect(10, 220, 200, 100), "采用System.Xml读取XML文档"))
        {
            //ReadBySystemXml(GamePath.Config + xmlFile);
        }

        if (GUI.Button(new Rect(210, 10, 200, 100), "显示应用程序路径"))
        {
            NGUIDebug.Log("Application.dataPath=" + Application.dataPath);
            NGUIDebug.Log("Application.persistentDataPath=" + Application.persistentDataPath);
            NGUIDebug.Log("Application.streamingAssetsPath=" + Application.streamingAssetsPath);
            NGUIDebug.Log("Application.temporaryCachePath=" + Application.temporaryCachePath);
        }
        if (GUI.Button(new Rect(210, 120, 200, 100), "创建MonsterContainer.xml"))
        {
            var mc = new MonsterContainer();
            mc.Monsters = new Monster[2];
            var monster = new Monster();
            monster.Health = 100;
            monster.Name = "monster";
            mc.Monsters[0] = monster;

            monster = new Monster();
            monster.Health = 1000;
            monster.Name = "boss";
            mc.Monsters[1] = monster;

            mc.Save(Application.persistentDataPath + "/MonsterContainer.xml");
            NGUIDebug.Log("成功创建文件：" + Application.persistentDataPath + "/MonsterContainer.xml");
        }
        if (GUI.Button(new Rect(210, 220, 200, 100), "加载MonsterContainer.xml"))
        {
            MonsterContainer mc = MonsterContainer.Load(Application.persistentDataPath + "/MonsterContainer.xml");
            NGUIDebug.Log("成功读取文件：" + Application.persistentDataPath + "/MonsterContainer.xml");
            NGUIDebug.Log(mc.Des());
        }
        if (GUI.Button(new Rect(210, 320, 200, 100), "加载config下MonsterContainer.xml"))
        {
            try
            {
                //NGUIDebug.Log("开始读取文件：" + GamePath.Config.Replace("file://", "") + "MonsterContainer.xml");
                //MonsterContainer mc = MonsterContainer.Load(GamePath.Config.Replace("file://", "") + "MonsterContainer.xml");
                //NGUIDebug.Log("成功读取文件：" + GamePath.Config + "MonsterContainer.xml");
                //NGUIDebug.Log(mc.Des());
            }
            catch (Exception)
            {
                //NGUIDebug.Log("未能读取文件：" + GamePath.Config + "MonsterContainer.xml");
                throw;
            }
            
        }
        if (GUI.Button(new Rect(210, 420, 200, 100), "加载config下test.xml"))
        {
            //string file = GamePath.Config + xmlFile;
            //NGUIDebug.Log(file);
            
            //CoreLoadHelp.LoadText(this, file, s =>
            //{
            //    NGUIDebug.Log(s);
            //});
        }
    }

    void Save(string file)
    {
        string suffix = ".xml";
        if (!file.EndsWith(suffix))
        {
            file += suffix;
        }
        
        XmlParser.Instance.SaveXmlByDotNetXmlDoc(file);
        NGUIDebug.Log("Finish save file : " + file);
    }
    void ReadBySystemXml ( string file )
    {
        string suffix = ".xml";
        if (!file.EndsWith(suffix))
        {
            file += suffix;
        }
        if (!File.Exists(file))
        {
            CreateFile(file);
        }
        NGUIDebug.Log("Read file : " + file);
        XmlDocument doc = XmlParser.Instance.OpenByDotNetXmlDoc(file);
        XmlAttributeCollection xc = doc.SelectSingleNode("root").FirstChild.Attributes;
        XmlNodeList nodelist = doc.SelectSingleNode("root").ChildNodes;
        foreach (XmlNode node in nodelist)
        {
            string atts = "";
            foreach (XmlAttribute attribute in node.Attributes)
            {
                atts += attribute.Name + " = " + attribute.Value + "; ";
            }
            NGUIDebug.Log("node.name=" + node.Name + "; innerText = " + node.InnerText + "; Attributes.Count = " + node.Attributes.Count + "; Attributes is " + atts);
        }
    }

    void CreateFile ( string file )
    {
        FileStream fs = null;
        if (!File.Exists(file))
        {
            NGUIDebug.Log("找不到文件：" + file);
            
            if (!Directory.Exists(Path.GetDirectoryName(file)))
            {
                NGUIDebug.Log("不存在目录：" + Path.GetDirectoryName(file) + "，开始创建目录。");
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                NGUIDebug.Log("成功创建目录：" + Path.GetDirectoryName(file));
            }
            fs = File.Create(file);
            NGUIDebug.Log("成功创建文件：" + Path.GetFileName(file));
        }

        string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>"+ //"\n" +
"<root>" + //"\n" +
"  <table wave=\"1\" level=\"1\" name=\"John\" />" +// "\n" +
"  <table wave=\"2\" level=\"1\" name=\"Lucy\" />" + //"\n" +
"</root>";
        StreamWriter writer = new StreamWriter(fs);
        writer.Write(xml);
        NGUIDebug.Log("写入文件内容：" + xml);
        writer.Close();
        fs.Close();
    }
    void ReadBySecurityParser ( string file )
    {
        //if (!File.Exists(file))
        //{
        //    NGUIDebug.Log("找不到文件：" + file);
        //    CreateFile(file);
        //}
        CoreLoadHelp.LoadText(this, file, s =>
        {
            NGUIDebug.Log("Read file : " + file);
            NGUIDebug.Log("file conten : " + s);
            //SecurityParser sp = XmlParser.Instance.ParseBySecurityParser(file);
            SecurityParser sp = new SecurityParser();
            sp.LoadXml(s);
            SecurityElement se = sp.ToXml();
            //注意这里虽然可以添加新属性，新的元素，但是并不能保存到文件中。
            se.AddAttribute("test", "sin");
            SecurityElement ch = new SecurityElement("table", "sin");
            ch.AddAttribute("wave", "1");
            ch.AddAttribute("level", "2");
            ch.AddAttribute("name", "asin");

            se.AddChild(ch);

            foreach (SecurityElement child in se.Children)
            {
                if (child.Tag == "table")
                {
                    //获得节点属性
                    string wave = child.Attribute("wave");
                    string level = child.Attribute("level");
                    string name = child.Attribute("name");
                    NGUIDebug.Log("wave:" + wave + " level:" + level + " name:" + name);
                }
            }
        });
        
    }
}
