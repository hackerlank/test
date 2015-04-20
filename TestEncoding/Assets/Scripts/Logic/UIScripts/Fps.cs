using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour
{

    // Use this for initialization
    public static int FPS = 0;
    public static float fpsDuration = 0;
    public static int fps = 0;
    void Start()
    {
        //Shader.DisableKeyword("SHAD1");
    }

    // Update is called once per frame
    void Update()
    {
        fpsDuration += Time.deltaTime;
        fps += 1;
        if (fpsDuration >= 1.0f)
        {
            FPS = fps;
            fps = 0;
            fpsDuration = 0;
        }
    }
    void OnGUI()
    {
        UnityEngine.GUI.Label(new Rect(Screen.width - 60, 0, 60, 20), "FPS " + FPS);
    }
}
