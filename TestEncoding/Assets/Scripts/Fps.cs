using System;
using UnityEngine;

public class Fps : MonoBehaviour
{
    public static int fps;
    public static int FPS;
    public static float fpsDuration;

    private void OnGUI()
    {
        GUI.Label(new Rect((float) (Screen.width - 60), 0f, 60f, 20f), "FPS " + FPS);
    }

    private void Start()
    {
    }

    private void Update()
    {
        fpsDuration += Time.deltaTime;
        fps++;
        if (fpsDuration >= 1f)
        {
            FPS = fps;
            fps = 0;
            fpsDuration = 0f;
        }
    }
}

