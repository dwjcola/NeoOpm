using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using GameFramework.AddressableResource;
using UnityEngine;

using GameFramework.Event;
using GameFramework.Resource;
using LitJson;
using Pomelo.DotNetClient;
using SLG;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityGameFramework.Runtime;
using XLua;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ProHA
{
    public class ProcedureLogin : ProcedureBase
    {
        private bool m_LoginGame = false;
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        public void LoginGame()
        {
            m_LoginGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_LoginGame = false;
            GameEntry.UI.OpenUI("LoginForm", this);
            var updateForm=GameEntry.UI.GetUIForm(AssetUtility.GetUIFormAsset(CheckUpdateForm.UIAssetName));
            if (updateForm != null)
            {
                GameEntry.UI.CloseUIForm(updateForm.Logic as UGuiForm);
                GameEntry.UI.CheckMask(updateForm.UIGroup);
            }
        }
        public void ConnectServer(string ip,int port)
        {
            //ip = "127.0.0.1";
            Log.Warning("connect game server->{0}---{1}",ip,port);
            GameEntry.Network.ConnectToServer(ip,port, () =>
            {
                Log.Warning("connect game server succ->{0}---{1}",ip,port);
                //Log.Error("Connect_server_succ");
                LC.SendEvent("Connect_server_succ");
            }, () =>
            {
                PomeloClient pc = PomeloClient.Instance;
                Rpc.PushEvent.Init(pc);
                /*pc.ReconnectCallBack = ReConnectCallBack;
                pc.ServerTimeResetCB = ResetServerTime;*/
            });
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            LC.CloseUI("LoginForm");
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_LoginGame)
            {
                return;
            }


            ChangeState<ProcedureCity>(procedureOwner);

            /*procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, Constant.Scene.City);
            ChangeState<ProcedureChangeScene>(procedureOwner);*/


            /*procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, Constant.Scene.Battle);
            ChangeState<ProcedureChangeScene>(procedureOwner);*/
        }
    }
}