using UnityEngine;
using System.Collections;
using Net;
using System.Text;
public class TestStruct : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    int num = 100;
    int num2 = 50;

    void OnGUI()
    {
        if (GUI.Button(new Rect((float)300, (float)(15 + num2), (float)num, (float)num2), "Test Struct"))
        {
            UMessage testMsg = new UMessage();

            //定义消息结构体
            TestMSG2 m;

            //初始化
            //m = new TestMSG2("吴明生");
            //m.cmd_type = 001;
            //m.srcID = 002;
            //m.dstID = 003;

            ////转换
            //byte[] message = Converter.StructToBytes(m);

            //Util.Log("length: " + message.Length);
            
            //TestMSG2 n = (TestMSG2)Converter.BytesToStruct(message, m.GetType());

            //Util.Log("转成bytes: " + Encoding.ASCII.GetString(message));

            //Util.Log("2转后 username: " + n.username);

            stChannelChatUserCmd cmd = new stChannelChatUserCmd();
            //cmd.pstrName = "老怪";
            //cmd.pstrChat = "哈";
            byte[] bytes = Converter.StructToBytes(cmd);

            stChannelChatUserCmd cmd2 = (stChannelChatUserCmd)Converter.BytesToStruct(bytes, cmd.GetType());

            //Util.Log("pstrName: " + cmd2.pstrName);
            //Util.Log("pstrChat: " + cmd.pstrChat);

            
        }
    }



}
