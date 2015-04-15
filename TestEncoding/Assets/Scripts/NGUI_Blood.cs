using NGUI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NGUI_Blood : NGUI_Base
{
    public UISlider BloodSlider;
    [SerializeField]
    private float current_blood;
    [SerializeField]
    private float current_sprite;
    public UIGrid Grid;
    public UILabel LeveLabel;
    public GameObject line;
    public GameObject longline;
    [SerializeField]
    private uint longline_order;
    [SerializeField]
    private float max_blood = 1f;
    [SerializeField]
    private float max_sprite;
    [SerializeField]
    private int playerlevel;
    [SerializeField]
    private string playername;
    public UILabel PlayerNameLabel;
    private List<GameObject> signLines = new List<GameObject>();
    public UISlider SpriteSlider;
    [SerializeField]
    private float stepblood_perline = 100f;

    public override void Init()
    {
        foreach (UISlider slider in base.transform.FindChild("blood").gameObject.GetComponents<UISlider>())
        {
            if (slider.foregroundWidget.name.Contains("blood"))
            {
                this.BloodSlider = slider;
            }
            else if (slider.foregroundWidget.name.Contains("sprite"))
            {
                this.SpriteSlider = slider;
            }
            else
            {
                Debug.LogError("NGUI_Blood.Init:滑动条前景控件命名不符合规范。请检查控件名字拼写是否正确。合法的名字是：blood, sprite。");
            }
        }
        Vector3 drawRegion = (Vector3) this.BloodSlider.foregroundWidget.drawRegion;
        float num2 = drawRegion.z - drawRegion.x;
        this.LeveLabel = base.transform.FindChild("level").gameObject.GetComponent<UILabel>();
        this.PlayerNameLabel = base.transform.FindChild("playername").gameObject.GetComponent<UILabel>();
        this.Grid = base.transform.FindChild("signline/grid").gameObject.GetComponent<UIGrid>();
        this.line = base.transform.FindChild("signline/line").gameObject;
        this.longline = base.transform.FindChild("signline/longline").gameObject;
        this.line.SetActive(false);
        this.longline.SetActive(false);
    }

    private void Update()
    {
        this.BloodSlider.value = this.current_blood / this.max_blood;
        this.SpriteSlider.value = this.current_sprite / this.max_sprite;
        int num = (int) (this.current_blood / this.stepblood_perline);
        if (this.signLines.Count < num)
        {
        }
    }

    public float CurrentBlood
    {
        get
        {
            return this.current_blood;
        }
        set
        {
            this.current_blood = value;
        }
    }

    public float CurrentSprite
    {
        get
        {
            return this.current_sprite;
        }
        set
        {
            this.current_sprite = value;
        }
    }

    public uint LonglineOrder
    {
        get
        {
            return this.longline_order;
        }
        set
        {
            this.longline_order = value;
        }
    }

    public float MaxBlood
    {
        get
        {
            return this.max_blood;
        }
        set
        {
            if (value <= 1f)
            {
                value = 1f;
            }
            this.max_blood = value;
        }
    }

    public float MaxSprite
    {
        get
        {
            return this.max_sprite;
        }
        set
        {
            if (value <= 1f)
            {
                value = 1f;
            }
            this.max_sprite = value;
        }
    }

    public int Playerlevel
    {
        get
        {
            return this.playerlevel;
        }
        set
        {
            this.playerlevel = value;
            this.LeveLabel.text = this.playerlevel.ToString();
        }
    }

    public string Playername
    {
        get
        {
            return this.playername;
        }
        set
        {
            this.playername = value;
            this.PlayerNameLabel.text = this.playername;
        }
    }

    public float StepbloodPerline
    {
        get
        {
            return this.stepblood_perline;
        }
        set
        {
            this.stepblood_perline = value;
        }
    }
}

