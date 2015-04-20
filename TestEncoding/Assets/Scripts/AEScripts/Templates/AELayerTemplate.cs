using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AELayerTemplate
{

    public int id;

    public int index;
    public int parent;

    public int width = 1;
    public int height = 1;

    public string name;

    public AELayerType type;
    public AELayerBlendingType blending;
    public AELayerBlendingType forcedBlending = AELayerBlendingType.NORMAL;



    public int inFrame;
    public int outFrame;

    public float inTime;
    public float outTime;


    public string source;

    public List<AEFrameTemplate> frames = new List<AEFrameTemplate>();

    public List<int> framesIndex = new List<int>();

    public void addFrame(AEFrameTemplate frame)
    {
        frames.Add(frame);
    }

    public void addFrameIndex(int nIndex)
    {
        framesIndex.Add(nIndex);
    }

    private static float fMinFloat = 0.9999f;
    private static float fMinFloatAdd = 0.001f;
    public void setInOutTime(float timeIn, float timeOut, AECompositionTemplate tpl)
    {
        inTime = timeIn;
        outTime = timeOut;

        double fIn = inTime / tpl.frameDuration;
        double fOut = outTime / tpl.frameDuration;
        if (fIn % 1 > fMinFloat)
            fIn += fMinFloatAdd;

        if (fOut % 1 > fMinFloat)
            fOut += fMinFloatAdd;
        //inFrame = Mathf.FloorToInt (fIn);
        //outFrame = Mathf.FloorToInt (fOut);

        inFrame = (int)(fIn); ;
        outFrame = (int)(fOut);

        if (outFrame + 1 == tpl.totalFrames)
        {
            outFrame = tpl.totalFrames;
        }
    }


    public AEFrameTemplate GetFrame(int index)
    {
        if (framesIndex.Count != 0)
        {
            if (index >= framesIndex.Count)
            {
                return null;
            }
            else
            {
                return frames[framesIndex[index]];
            }
        }
        else
        {
            if (index >= frames.Count)
            {
                return null;
            }
            else
            {
                return frames[index];
            }
        }
    }


    public int totalFrames
    {
        get
        {
            if (framesIndex.Count != 0)
            {
                return framesIndex.Count;
            }
            else
            {
                return frames.Count;
            }
        }
    }

    public int lastFrameIndex
    {
        get
        {
            return totalFrames - 1;
        }
    }


    public string sourceNoExt
    {
        get
        {
            return source.Substring(0, source.Length - 4);
        }
    }





}
