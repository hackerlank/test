using UnityEngine;
using System.Collections;
using Net;

public class TestEncrypt : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    int num = 100;
    int num2 = 50;

    void OnGUI()
    {
        if (GUI.Button(new Rect((float) 400, (float) (15 + num2), (float) num, (float) num2), "TestRC5"))
		{
			UMessage testMsg = new UMessage();

			//大小消息号+时间戳 6个字节
			//testMsg.WriteHead(0x68, 120);

//			for (int i=0; i<8; ++i)
//			{
//				testMsg.WriteByte((byte)(i + 1));
//			}


			testMsg.WriteByte(60);
			testMsg.WriteByte(20);
			testMsg.WriteByte(215);
			testMsg.WriteByte(86);
			testMsg.WriteByte(201);
			testMsg.WriteByte(8);

			testMsg.Buffer[0] = 12;
			testMsg.Buffer[1] = 0;
			testMsg.Buffer[2] = 0;
			testMsg.Buffer[3] = 128;

			//for (int j=0; j<8; ++j)
			{
				Util.Log("Before CRC Encrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4]+ " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7]+ " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11]);
			}

			Util.Log("Before Length: " + testMsg.Length);

			testMsg.RC5Encrypt();

			Util.Log("After CRC Encrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4]+ " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7]+ " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);

			Util.Log("After Length: " + testMsg.Length);

            //testMsg.RC5Decrypt();
            byte[] Buffer = UMessage.rC5Decrypt(testMsg.Buffer);
            Util.Log("@After CRC DEncrypt: " + Buffer[0] + " " + Buffer[1] + " " + Buffer[2] + " " + Buffer[3] + " " + Buffer[4] + " " + Buffer[5] + " " + Buffer[6] + " " + Buffer[7]);// + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);

            //Util.Log("@After CRC DEncrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7] + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);

            //Util.Log("After Length: " + testMsg.Length);
		}

        if (GUI.Button(new Rect((float)500, (float)(15 + num2), (float)num, (float)num2), "Test Generate SubKey"))
        {
            UMessage testMsg = new UMessage();
            
            testMsg.WriteByte(60);
            testMsg.WriteByte(20);

            testMsg.WriteByte(60);
            testMsg.WriteByte(20);
            testMsg.WriteByte(215);
            testMsg.WriteByte(86);
            testMsg.WriteByte(201);
            testMsg.WriteByte(8);

            testMsg.Buffer[0] = 12;
            testMsg.Buffer[1] = 0;
            testMsg.Buffer[2] = 0;
            testMsg.Buffer[3] = 128;

            UMessage.generateSubKey();
        }

        if (GUI.Button(new Rect((float)600, (float)(15 + num2), (float)num, (float)num2), "TestDes"))
        {
            UMessage testMsg = new UMessage();

            //大小消息号+时间戳 6个字节
            //testMsg.WriteHead(0x68, 120);

            //			for (int i=0; i<8; ++i)
            //			{
            //				testMsg.WriteByte((byte)(i + 1));
            //			}


            //testMsg.WriteByte(2);
            //testMsg.WriteByte(3);
            //testMsg.WriteByte(0);
            //testMsg.WriteByte(0);
            //testMsg.WriteByte(0);
            //testMsg.WriteByte(0);

            //testMsg.WriteByte(255);
            //testMsg.WriteByte(255);
            //testMsg.WriteByte(255);
            //testMsg.WriteByte(255);

            //testMsg.WriteByte(95);
            //testMsg.WriteByte(159);
            //testMsg.WriteByte(235);
            //testMsg.WriteByte(73);

            //testMsg.WriteByte(4);
            //testMsg.WriteByte(0);
            //testMsg.WriteByte(0);
            //testMsg.WriteByte(0);

            //testMsg.Buffer[0] = 20;
            //testMsg.Buffer[1] = 0;
            //testMsg.Buffer[2] = 0;
            //testMsg.Buffer[3] = 128;

            testMsg.WriteByte(161);
            testMsg.WriteByte(3);
            testMsg.WriteByte(211);
            testMsg.WriteByte(206);

            testMsg.Buffer[0] = 12;
            testMsg.Buffer[1] = 0;
            testMsg.Buffer[2] = 0;
            testMsg.Buffer[3] = 128;

            //for (int j=0; j<8; ++j)
            {
                Util.Log("Before Des Encrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7]);// + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15] + " " + testMsg.Buffer[16] + " " + testMsg.Buffer[17] + " " + testMsg.Buffer[18] + " " + testMsg.Buffer[19] + " " + testMsg.Buffer[20] + " " + testMsg.Buffer[21]);  
            }

            Util.Log("Before Length: " + testMsg.Length);

            testMsg.DESEncrypt();

            Util.Log("After Des Encrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7]);// + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15] + " " + testMsg.Buffer[16] + " " + testMsg.Buffer[17] + " " + testMsg.Buffer[18] + " " + testMsg.Buffer[19] + " " + testMsg.Buffer[20] + " " + testMsg.Buffer[21] + " " + testMsg.Buffer[22] + " " + testMsg.Buffer[23]);  

            Util.Log("After Length: " + testMsg.Length);

            //testMsg.DESDencryptStep1();
            UMessage.DESEncryptHandle.encdec_des(testMsg.Buffer, 0, 8, false);

            Util.Log("After Des DEncrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7]);// + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15] + " " + testMsg.Buffer[16] + " " + testMsg.Buffer[17] + " " + testMsg.Buffer[18] + " " + testMsg.Buffer[19] + " " + testMsg.Buffer[20] + " " + testMsg.Buffer[21] + " " + testMsg.Buffer[22] + " " + testMsg.Buffer[23]);  

            Util.Log("After Length: " + testMsg.Length);

            //testMsg.RC5Decrypt();
            //byte[] Buffer = UMessage.rC5Decrypt(testMsg.Buffer);
            //Util.Log("@After CRC DEncrypt: " + Buffer[0] + " " + Buffer[1] + " " + Buffer[2] + " " + Buffer[3] + " " + Buffer[4] + " " + Buffer[5] + " " + Buffer[6] + " " + Buffer[7]);// + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);

            //Util.Log("@After CRC DEncrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7] + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);

            //Util.Log("After Length: " + testMsg.Length);
        }

        if (GUI.Button(new Rect((float)700, (float)(15 + num2), (float)num, (float)num2), "TestMergeVersionDes"))
        {
            UMessage testMsg = new UMessage();

            testMsg.WriteByte(3);
            testMsg.WriteByte(53);
            testMsg.WriteByte(0);
            testMsg.WriteByte(0);

            testMsg.WriteByte(0);
            testMsg.WriteByte(0);
            testMsg.WriteByte(1);
            testMsg.WriteByte(0);

            testMsg.WriteByte(0);
            testMsg.WriteByte(0);
            testMsg.WriteByte(0);
            testMsg.WriteByte(0);

            testMsg.Buffer[0] = 12;
            testMsg.Buffer[1] = 0;
            testMsg.Buffer[2] = 0;
            testMsg.Buffer[3] = 0;

            //for (int j=0; j<8; ++j)
            {
                Util.Log("Before Des Encrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7] + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);// + " " + testMsg.Buffer[16] + " " + testMsg.Buffer[17] + " " + testMsg.Buffer[18] + " " + testMsg.Buffer[19] + " " + testMsg.Buffer[20] + " " + testMsg.Buffer[21]);  
            }

            UMessage.DESEncryptHandle.encdec_des(testMsg.Buffer, 0, 8, false);

            UMessage.DESEncryptHandle.encdec_des(testMsg.Buffer, 8, 8, false);

            Util.Log("After Des DEncrypt: " + testMsg.Buffer[0] + " " + testMsg.Buffer[1] + " " + testMsg.Buffer[2] + " " + testMsg.Buffer[3] + " " + testMsg.Buffer[4] + " " + testMsg.Buffer[5] + " " + testMsg.Buffer[6] + " " + testMsg.Buffer[7] + " " + testMsg.Buffer[8] + " " + testMsg.Buffer[9] + " " + testMsg.Buffer[10] + " " + testMsg.Buffer[11] + " " + testMsg.Buffer[12] + " " + testMsg.Buffer[13] + " " + testMsg.Buffer[14] + " " + testMsg.Buffer[15]);// + " " + testMsg.Buffer[16] + " " + testMsg.Buffer[17] + " " + testMsg.Buffer[18] + " " + testMsg.Buffer[19] + " " + testMsg.Buffer[20] + " " + testMsg.Buffer[21] + " " + testMsg.Buffer[22] + " " + testMsg.Buffer[23]);  

            Util.Log("After Length: " + testMsg.Length);
        }

    }
}
