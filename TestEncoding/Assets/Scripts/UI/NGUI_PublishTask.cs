using System.Collections.Generic;
using NGUI;
using UnityEngine;
using System.Collections;
using System;
using Net;

public class NGUI_PublishTask : NGUI_Base 
{
    private const int COUNTRY_TASK_COUNT = 3;
    private const int UNION_TASK_COUNT = 3;
    private List<GameObject> mCountryTaskObjects = new List<GameObject>();
    private List<GameObject> mUnionTaskObjects = new List<GameObject>();

    private bool isCountryTask = true;

    public enum enumCountryTask
    {
        CountryTask_YunBiao = 1,			/// 运镖
        CountryTask_GuoTan = 2,			/// 国探
        CountryTask_BanZhuan = 3,      /// 搬砖
     };

    public enum enumUnionTask
    {
        UnionTask_CaiJi = 1,			    /// 采集
        UnionTask_GuoJiaRenWu = 2,	/// 国家任务
        UnionTask_ShouJiYunTie = 3, /// 收集陨铁
    };
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void SetCountryTask(bool flag)
    {
        isCountryTask = flag;
    }

    public override void Init()
    {
        mCountryTaskObjects.Clear();
        for (int i=1; i<=COUNTRY_TASK_COUNT; ++i)
        {
            GameObject obj = transform.FindChild("PanelCountryTask/" + i.ToString()).gameObject;
            if (obj != null)
            {
                mCountryTaskObjects.Add(obj);
                UIEventListener.Get(obj).onClick = OnCountryTaskClick;
            }
        }


        mUnionTaskObjects.Clear();
        for (int i = 1; i <= UNION_TASK_COUNT; ++i)
        {
            GameObject obj = transform.FindChild("PanelUnionTask/" + i.ToString()).gameObject;
            if (obj != null)
            {
                mUnionTaskObjects.Add(obj);
                UIEventListener.Get(obj).onClick = OnUnionTaskClick;
            }
        }

        GameObject CloseObj = transform.FindChild("Close").gameObject;
        if (CloseObj != null)
            UIEventListener.Get(CloseObj).onClick = OnDlgClose;

        if (isCountryTask)
        {
            transform.FindChild("PanelUnionTask").gameObject.SetActive(false);
        }
        else
        {
            transform.FindChild("PanelCountryTask").gameObject.SetActive(false);
        }
    }

    public void OnCountryTaskClick(GameObject iterm)
    {
        if (iterm != null)
        {
            UInt32 index = 0;
            UInt32.TryParse(iterm.name, out index);

            if ((index > 0) && (index <= mCountryTaskObjects.Count))
            {
                //搬砖
                if (index == (UInt32)enumCountryTask.CountryTask_BanZhuan)
                {
                    SendTaskCmd(110424, "v336", 10);
                }
                //国探
                else if (index == (UInt32)enumCountryTask.CountryTask_GuoTan)
                {
                    SendTaskCmd(150016, "v227", 1);
                }
                //运镖
                else if (index == (UInt32)enumCountryTask.CountryTask_YunBiao)
                {
                    SendTaskCmd(150015, "v227", 1);
                }
            }
        }
    }

    public void OnUnionTaskClick(GameObject iterm)
    {
        if (iterm != null)
        {
            UInt32 index = 0;
            UInt32.TryParse(iterm.name, out index);

            if ((index > 0) && (index <= mUnionTaskObjects.Count))
            {
                //采集
                if (index == (UInt32)enumUnionTask.UnionTask_CaiJi)
                {
                    SendTaskCmd(160112, "v5224", 11);
                }
                //国家任务
                else if (index == (UInt32)enumUnionTask.UnionTask_GuoJiaRenWu)
                {
                    SendTaskCmd(150017, "v5287", 1);
                }
                //收集陨铁
                else if (index == (UInt32)enumUnionTask.UnionTask_ShouJiYunTie)
                {
                    SendTaskCmd(20717, "v5364", 11);
                }
            }
        }
    }

    public void SendTaskCmd(UInt32 id, string target, byte offset)
    {
        ///// 任务指令
        //const BYTE TASK_USERCMD = 23;
        //static const BYTE REQUEST_QUEST_PARA = 13;
        //struct stRequestQuestUserCmd : public stNullUserCmd
        //{
        //    stRequestQuestUserCmd()
        //    {
        //        byParam = REQUEST_QUEST_PARA;
        //    }
        //    DWORD id; //任务id
        //    char target[16]; //目标
        //    BYTE offset; //任务分支
        //};

        //UMessage message = new UMessage();
        //message.WriteHead(23, 13);
        //message.WriteUInt32(id);
        //message.WriteString(target, 16);
        //message.WriteByte(offset);
        //NetWorkModule.Instance.SendImmediate(message);
        //Debug.Log("@@发布任务,  id: " + id + " target: " + target + " offset: " + offset);

        UMessage message = new UMessage();
        stRequestQuestUserCmd cmd = new stRequestQuestUserCmd();
        cmd.id = id;
        cmd.target = target;
        cmd.offset = offset;
        if (message.WriteStruct<stRequestQuestUserCmd>(cmd))
            NetWorkModule.Instance.SendImmediate(message);
    }

     public void OnDlgClose(GameObject iterm)
     {
         Util.Log("Close");
         NGUIManager.Instance.DeleteByName(NGUI_UI.NGUI_PublishTask);
     }

}
