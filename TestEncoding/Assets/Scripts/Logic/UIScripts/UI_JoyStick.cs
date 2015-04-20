using UnityEngine;
using System.Collections;

public class UI_JoyStick : MonoBehaviour {

    public GameObject JoyStick;
    private UIButton JsRealBtn;
    private Transform JsFake;
    Vector3 JsPos;

    public Camera uiCamera;

    public float JsDragRange = 75f;

    public MoveDirction InputDir = MoveDirction.USER_DIR_WRONG;

	// Use this for initialization
	void Start () {
        JoyStick = transform.FindChild("Camera/JoyStick").gameObject;
        JsFake = JoyStick.transform.FindChild("JsFake");
        JsRealBtn = JoyStick.transform.FindChild("JsReal").GetComponent<UIButton>();

        UIEventListener.Get(JsRealBtn.gameObject).onDrag = JsBtnDrag;
        UIEventListener.Get(JsRealBtn.gameObject).onPress = JsBtnPress;

        JsPos = JoyStick.transform.position;

        if (uiCamera == null)
            uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
	}

    void JsBtnDrag(GameObject button, Vector2 delta)
    {
//        Vector3 pos;
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//        pos = Input.mousePosition;
//#else 
//        if (Input.touchCount <= 0)
//        {
//            return;
//        }
//        pos = Input.GetTouch(0).position;
//# endif
//        if (uiCamera != null)
//        {
//            // Since the screen can be of different than expected size, we want to convert
//            // mouse coordinates to view space, then convert that to world position.
//            pos.x = Mathf.Clamp01(pos.x / Screen.width);
//            pos.y = Mathf.Clamp01(pos.y / Screen.height);
//            JsRealBtn.transform.position = uiCamera.ViewportToWorldPoint(pos);


//            // For pixel-perfect results
//            if (uiCamera.isOrthoGraphic)
//            {
//                Vector3 lp = JsRealBtn.transform.localPosition;
//                lp.x = Mathf.Round(lp.x);
//                lp.y = Mathf.Round(lp.y);
//                JsRealBtn.transform.localPosition = lp;
//            }
//        }

        if (Vector3.Distance(JsPos, JsRealBtn.transform.position) <= JsDragRange)
        {
            JsFake.position = JsRealBtn.transform.position;
        }
        else
        {
            Vector3 dVector = (JsRealBtn.transform.position - JsPos).normalized * JsDragRange;
            JsFake.position = JsPos + dVector;
        }
        if (Vector3.Magnitude(JsRealBtn.transform.position - JsPos) > 0.03f)
        {
            ProcessInput((JsRealBtn.transform.position - JsPos).normalized);
        }
    }

    void JsBtnPress(GameObject button, bool state)
    {
        if (state == false)
        {
            JsRealBtn.transform.localPosition = JsFake.localPosition;

            TweenPosition.Begin(JsRealBtn.transform.gameObject, 0.05f, JsPos);
            TweenPosition.Begin(JsFake.transform.gameObject, 0.05f, JsPos);
            //JsRealBtn.transform.localPosition = Vector3.zero;
            //JsFake.transform.localPosition = Vector3.zero;
            InputDir = MoveDirction.USER_DIR_WRONG;
        }
    }

    void Update()
    {
        if (GameManager.Instance.MainPlayer == null)
        {
            return;
        }
        if (InputDir != MoveDirction.USER_DIR_WRONG)
        {
            if (GameManager.Instance.MainPlayer.trackState)
            {
                GameManager.Instance.MainPlayer.EndTrackTarget();
            }

            GameManager.Instance.MainPlayer.MoveTo(InputDir);
        }
    }

    /// <summary>
    /// 计算摇杆对应的方向
    /// </summary>
    /// <param name="root"></param>
    /// <param name="pos"></param>
    public void ProcessInput(Vector3 dir)
    {
        float degree = Vector3.Angle(Vector3.up, dir);

        if (degree < 22.5f)
        {
            InputDir = MoveDirction.USER_DIR_UP;
        }
        else if (degree >= 22.5f && degree < 67.5f)
        {
            if (dir.x > 0)
            {
                InputDir = MoveDirction.USER_DIR_UPRIGHT;
            }
            else
            {
                InputDir = MoveDirction.USER_DIR_LEFTUP;
            }
        }
        else if (degree >= 67.5f && degree < 112.5f)
        {
            if (dir.x > 0)
            {
                InputDir = MoveDirction.USER_DIR_RIGHT;
            }
            else
            {
                InputDir = MoveDirction.USER_DIR_LEFT;
            }
        }
        else if (degree >= 112.5f && degree < 157.5f)
        {
            if (dir.x > 0)
            {
                InputDir = MoveDirction.USER_DIR_RIGHTDOWN;
            }
            else
            {
                InputDir = MoveDirction.USER_DIR_DOWNLEFT;
            }
        }
        else if (degree >= 157.5f)
        {
            InputDir = MoveDirction.USER_DIR_DOWN;
        }
    }
}
