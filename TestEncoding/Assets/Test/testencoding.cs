using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;

using GBKEncoding;


public class testencoding : MonoBehaviour {

    //public static Encoding gbk = Encoding.GetEncoding(936);
    public static Encoding utf8 = Encoding.GetEncoding("utf-8");

	// Use this for initialization
	void Start () 
    {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 20, 100, 40), "test"))
        {
            //byte[] bytes = new byte[2] { 0xb9, 0xfe };
            //string result = utf8.GetString(bytes);
            ////Debug.Log(result);
            //OutLog.Log(result);

            byte[] result1 = GBKEncoder.ToBytes("深刻的减肥的快速发");

            string s2 = GBKEncoder.Read(result1);

            foreach (byte b in result1)
            {
                OutLog.Log(b + " ");
            }

            OutLog.Log(s2);


        }

    }
	// Update is called once per frame
	void Update () 
    {
	
	}
}
