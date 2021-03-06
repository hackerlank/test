using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;

public class NGUI_Blood : NGUI_Base
{
    #region Property
    /// <summary>
    /// 血量
    /// </summary>
    public UISlider BloodSlider;
    /// <summary>
    /// 精力
    /// </summary>
    public UISlider SpriteSlider;
    /// <summary>
    /// 等级
    /// </summary>
    public UILabel LeveLabel;
    /// <summary>
    /// 玩家名字
    /// </summary>
    public UILabel PlayerNameLabel;
    #region signline
    /// <summary>
    /// 布局管理
    /// </summary>
    public UIGrid Grid;
    /// <summary>
    /// 标志线1：短线
    /// </summary>
    public GameObject line;
    /// <summary>
    /// 标志线2：长线
    /// </summary>
    public GameObject longline;
    #endregion //sign line
    /// <summary>
    /// 最大血量
    /// </summary>
    [SerializeField]
    private float max_blood = 1 ;
    /// <summary>
    /// 当前血量
    /// </summary>
    [SerializeField]
    private float current_blood;
    /// <summary>
    /// 最大精力
    /// </summary>
    [SerializeField]
    private float max_sprite;
    /// <summary>
    /// 当前精力
    /// </summary>
    [SerializeField]
    private float current_sprite;
    /// <summary>
    /// 多少血量，出现一个分割线
    /// </summary>
    [SerializeField]
    private float stepblood_perline = 100;
    /// <summary>
    /// 第几个line线，替换为长线
    /// </summary>
    [SerializeField]
    private uint longline_order;
    /// <summary>
    /// 玩家名字
    /// </summary>
    [SerializeField]
    private string playername;
    /// <summary>
    /// 玩家名字
    /// </summary>
    [SerializeField]
    private int playerlevel;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float MaxBlood
    {
        get { return max_blood; }
        set
        {
            //防止被0除
            if (value <= 1)
            {
                value = 1;
            }
            max_blood = value;
        }
    }

    /// <summary>
    /// 当前血量
    /// </summary>
    public float CurrentBlood
    {
        get { return current_blood; }
        set { current_blood = value; }
    }

    /// <summary>
    /// 最大精力
    /// </summary>
    public float MaxSprite
    {
        get { return max_sprite; }
        set
        {
            //防止被0除
            if (value <= 1)
            {
                value = 1;
            }
            max_sprite = value;
        }
    }

    /// <summary>
    /// 当前精力
    /// </summary>
    public float CurrentSprite
    {
        get { return current_sprite; }
        set { current_sprite = value; }
    }

    /// <summary>
    /// 多少血量，出现一个分割线
    /// </summary>
    public float StepbloodPerline
    {
        get { return stepblood_perline; }
        set { stepblood_perline = value; }
    }

    /// <summary>
    /// 第几个line线，替换为长线
    /// </summary>
    public uint LonglineOrder
    {
        get { return longline_order; }
        set { longline_order = value; }
    }

    /// <summary>
    /// 玩家名字
    /// </summary>
    public string Playername
    {
        get { return playername; }
        set
        {
            playername = value;
            PlayerNameLabel.text = playername;
        }
    }

    /// <summary>
    /// 玩家名字
    /// </summary>
    public int Playerlevel
    {
        get { return playerlevel; }
        set
        {
            playerlevel = value;
            LeveLabel.text = playerlevel.ToString();
        }
    }

    private List<GameObject> signLines = new List<GameObject>(); 
    #endregion //property
    // Use this for initialization
    //void Start () {
    //    Init();
    //}
	
	// Update is called once per frame
	void Update ()
	{
        BloodSlider.value = current_blood / max_blood;
	    SpriteSlider.value = current_sprite/max_sprite;
	    int numlines = (int)(current_blood/stepblood_perline);
	    if (signLines.Count < numlines)
	    {
	        
	    }
	    else
	    {
	        
	    }
	}

    public override void Init()
    {
        GameObject bloodGameObject = transform.FindChild("blood").gameObject;
        UISlider[] sliders = bloodGameObject.GetComponents<UISlider>();
        //这里通过名字来查找正确的控件对应关系，而不是通过顺序，因为顺序可能会改变。
        foreach (var uiSlider in sliders)
        {
            if (uiSlider.foregroundWidget.name.Contains("blood"))
            {
                BloodSlider = uiSlider;
            }
            else if (uiSlider.foregroundWidget.name.Contains("sprite"))
            {
                SpriteSlider = uiSlider;
            }
            else
            {
                Debug.LogError("NGUI_Blood.Init:滑动条前景控件命名不符合规范。请检查控件名字拼写是否正确。合法的名字是：blood, sprite。");
            }
        }
        Vector3 drawRegion = BloodSlider.foregroundWidget.drawRegion;
        float total = drawRegion.z - drawRegion.x;

        LeveLabel = transform.FindChild("level").gameObject.GetComponent<UILabel>();
        PlayerNameLabel = transform.FindChild("playername").gameObject.GetComponent<UILabel>();

        Grid = transform.FindChild("signline/grid").gameObject.GetComponent<UIGrid>();
        line = transform.FindChild("signline/line").gameObject;
        longline = transform.FindChild("signline/longline").gameObject;
        line.SetActive(false);
        longline.SetActive(false);
    }
}
