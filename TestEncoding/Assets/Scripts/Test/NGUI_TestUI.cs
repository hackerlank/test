using NGUI;
using UnityEngine;
using System.Collections;

public class NGUI_TestUI : NGUI_Base
{

	// Use this for initialization
	void Start () {
        Transform buttonTransform = transform.FindChild("Controls/Control - Colored Button");
        Debug.Log(buttonTransform.name);
        UIEventListener elListener = UIEventListener.Get(buttonTransform.gameObject);
        elListener.parameter = gameObject;
        elListener.onClick = go => NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_TestUI);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Init()
    {
        throw new System.NotImplementedException();
    }
}
