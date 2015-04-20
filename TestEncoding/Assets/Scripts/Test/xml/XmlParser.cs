using System.Text;
using Mono.Xml;
using System.IO;
using System.Xml;

public class XmlParser : LSingleton<XmlParser>
{
    #region Mono.Xml Parser
    private SecurityParser _monoXmlParser;

    internal SecurityParser MonoXmlParser
    {
        get
        {
//            if (_monoXmlParser == null)
//            {
//                _monoXmlParser = new SecurityParser();
//            }
            return _monoXmlParser;
        }
        set { _monoXmlParser = value; }
    }


    /// <summary>
    /// 采用Mono.Xml.SecurityParser解析xml文件
    /// </summary>
    /// <param name="xmlfile">相对于工程目录的文件名，或绝对路径的文件名，含后缀名.xml</param>
    /// <returns></returns>
    internal SecurityParser ParseBySecurityParser ( string xmlfile )
    {
        var fs = new FileStream(xmlfile, FileMode.Open);
        
        StreamReader reader = new StreamReader(fs, Encoding.UTF8);
        if (MonoXmlParser == null)
        {
            MonoXmlParser = new SecurityParser();
        }
        MonoXmlParser.LoadXml(reader.ReadToEnd());
        //MonoXmlParser.LoadXml(Resources.Load(xmlfile).ToString());
        return MonoXmlParser;
    }

    internal void SaveXmlBySecurityParser(string xmlfile)
    {
        return;//SecurityParser不支持转换成字符串
        
        //Resources.Load和FileStream使用的相对路径不一样。为了对外部调用接口一致，这里需要处理一下
        FileStream fs = new FileStream( GamePath.Resources + xmlfile, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        
        sw.Write(MonoXmlParser.ToString());
        sw.Flush();
        sw.Close();
        fs.Close();
    }
    #endregion
    #region System.Xml

    private XmlDocument _dotNetXmlDoc;
    public XmlDocument DotNetXmlDoc
    {
        get
        {
//            if (_dotNetXmlDoc == null)
//            {
//                _dotNetXmlDoc = new XmlDocument();
//            }
            return _dotNetXmlDoc;
        }
        set { _dotNetXmlDoc = value; }
    }
    /// <summary>
    /// 采用System.Xml.XmlDocument解析文件
    /// </summary>
    /// <param name="xmlfile">相对于工程目录的文件名，或绝对路径的文件名，含后缀名.xml</param>
    /// <returns>XmlDocument</returns>
    public XmlDocument OpenByDotNetXmlDoc(string xmlfile)
    {
        if (DotNetXmlDoc == null)
        {
            DotNetXmlDoc = new XmlDocument();
        }
        DotNetXmlDoc.Load(xmlfile);
        return DotNetXmlDoc;
    }
    internal void SaveXmlByDotNetXmlDoc ( string xmlfile )
    {
        if (DotNetXmlDoc == null)
        {
            DotNetXmlDoc = new XmlDocument();//如果DotNetXmlDoc为空，则保存空文件
        }
        DotNetXmlDoc.Save(xmlfile);
    }
    #endregion
}
