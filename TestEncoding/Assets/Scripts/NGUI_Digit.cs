using NGUI;
using System;

public class NGUI_Digit : NGUI_Base
{
    private string text;
    public UILabel TextLabel;

    public override void Init()
    {
        this.TextLabel = base.transform.FindChild("TextLabel").GetComponent<UILabel>();
        this.TextLabel.text = this.Text;
    }

    public string Text
    {
        get
        {
            return this.text;
        }
        set
        {
            this.text = value;
            this.TextLabel.text = this.Text;
        }
    }
}

