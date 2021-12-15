//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework.UI;

namespace ProHA
{
    public class LoginForm : UGuiForm
    {
        [SerializeField]
        private GameObject m_LoginButton = null;

        private ProcedureLogin m_ProcedureLogin = null;


#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureLogin = (ProcedureLogin)userData;
            if (m_ProcedureLogin == null)
            {
                Log.Warning("ProcedureMenu is invalid when open CityForm.");
                return;
            }

            EventTriggerListener.Get(m_LoginButton).onClick = OnLoginButtonClick;
        }

        private async void OnLoginButtonClick(GameObject go)
        {
            Debug.Log("DoSomeThings");
            //eg.
            //var userInfo = new SLG.UserLoginInfo();
            //userInfo.Account = "xyz";
            //var ret = await Rpc.gfProxy_Handler.Login(userInfo);
            //Log.Debug($"recv:{ret.Code}");

            m_ProcedureLogin.LoginGame();
        }
        private void Update()
        {
            Pomelo.DotNetClient.PomeloClient.Instance.Update(Time.deltaTime, Time.deltaTime);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            m_ProcedureLogin = null;

            base.OnClose(isShutdown, userData);
        }
    }
}
