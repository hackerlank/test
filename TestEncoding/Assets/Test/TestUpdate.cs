using UnityEngine;
using System.Collections;

public class TestUpdate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnGUI()
    {
        int num = 100;
        int num2 = 50;
        if (GUI.Button(new Rect((float) (Screen.width - num), 20f, (float) num, (float) num2), "检测并下载新版本"))
        {
            if (true)
            {
                Application.OpenURL("http://ld.ztgame.com/");
            }
        }
    }
}
