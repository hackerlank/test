namespace MGUI
{
    using NGUI;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class GUIWindowState
    {
        private static ChangeState changeStateEvent;
        private static Dictionary<MethodInfo, ChangeState> changeStateEventDic = new Dictionary<MethodInfo, ChangeState>();
        public static EWindowState LastState;
        public static EWindowState WindowState = EWindowState.Login;

        public static void ClearChangeState()
        {
            changeStateEvent = null;
            changeStateEventDic.Clear();
        }

        private static void CommonState(EWindowState changeToState)
        {
            //PlayerHandle.Debug(string.Concat(new object[] { "SetState ", changeToState, "   ", LastState }));
            if (WindowState != EWindowState.ViewHeroDetail)
            {
                LastState = WindowState;
            }
            WindowState = changeToState;
        }

        public static void ExcuteChangeStateEvent()
        {
            if (changeStateEvent != null)
            {
                changeStateEvent();
            }
        }

        public static void LoadNGUI(EWindowState changeToState, bool bIsForbiddenAuto)
        {
        }

        public static void RemoveStateEvent(MethodInfo mInfo)
        {
            if (changeStateEventDic.ContainsKey(mInfo))
            {
                changeStateEvent = (ChangeState) Delegate.Remove(changeStateEvent, changeStateEventDic[mInfo]);
                changeStateEventDic.Remove(mInfo);
            }
        }

        public static void SetState(EWindowState changeToState)
        {
            CommonState(changeToState);
            if (ChangeStateEvent != null)
            {
                ChangeStateEvent();
            }
        }

        public static void SetStateAutoDelete(EWindowState changeToState)
        {
            CommonState(changeToState);
            //NGUIManager.Instance.AutoDeleteUI();
            if (ChangeStateEvent != null)
            {
                ChangeStateEvent();
            }
        }

        public static void SetStateAutoDeleteOpen(EWindowState changeToState)
        {
            CommonState(changeToState);
            //NGUIManager.Instance.AutoDeleteUI();
            LoadNGUI(changeToState, false);
        }

        public static void SetStateAutoDeleteOpen(EWindowState changeToState, System.Action callBack)
        {
            CommonState(changeToState);
            //NGUIManager.Instance.AutoDeleteUI();
        }

        public static void SetStateAutoDeleteOpen(EWindowState changeToState, bool bIsForbiddenAuto)
        {
            CommonState(changeToState);
            LoadNGUI(changeToState, bIsForbiddenAuto);
        }

        public static void SetStateAutoOpen(EWindowState changeToState)
        {
            CommonState(changeToState);
            LoadNGUI(changeToState, false);
        }

        public static ChangeState ChangeStateEvent
        {
            get
            {
                return changeStateEvent;
            }
            set
            {
                changeStateEventDic[value.Method] = value;
                changeStateEvent = (ChangeState) Delegate.Combine(changeStateEvent, value);
            }
        }

        public static ChangeState RealStateEvent
        {
            get
            {
                return changeStateEvent;
            }
            set
            {
                changeStateEvent = value;
            }
        }

        public delegate void ChangeState();
    }
}

