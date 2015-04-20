using NGUI;
using UnityEngine;
using System.Collections;

public class NGUI_Digit : NGUI_Base
{
    public UILabel TextLabel;
    //
    private string text;

    //void Start()
    //{
    //    Init();
    //}
    public string Text
    {
        get { return text; }
        set
        {
            text = value;
            TextLabel.text = Text;
        }
    }


    public override void Init()
    {
        TextLabel = transform.FindChild("TextLabel").GetComponent<UILabel>();
        TextLabel.text = Text;
        
    }
}
