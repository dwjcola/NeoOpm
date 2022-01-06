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
        public async void GetServerInfo(LuaTable data)
        {
            string acc = data["acc"].ToString();
            JsonData res = new JsonData();
            res["acc_id"] = acc;
            res["ip"] = "10.1.10.50";
            res["port"] = 9039;
            JsonData list = new JsonData();
            JsonData temp = new JsonData();
            temp["status"] = 1;
            temp["id"] = 10001;
            temp["sname"] = "只手遮天";
            temp["ip"] = "10.1.10.50";
            temp["port"] = 9039;
            list.Add(temp);
            temp["status"] = 2;
            temp["id"] = 10002;
            temp["sname"] = "猛龙过江";
            temp["ip"] = "10.1.10.50";
            temp["port"] = 9039;
            list.Add(temp);
            temp["status"] = 3;
            temp["id"] = 10003;
            temp["sname"] = "龙争虎斗";
            temp["ip"] = "10.1.10.50";
            temp["port"] = 9039;
            list.Add(temp);
            res["svr_list"] = list;
            
            LC.SendEvent("Get_server_list_back",res.toLuaTable());
            /*JsonData jsData = new JsonData();
            foreach (var key in data.GetKeys())
            {
                jsData[key.ToString()] = data[key].ToString();
            }
            string url = "";
            GameEntry.UI.StartCoroutine(DownLoadGameServerList(jsData,url));*/
        }
        private IEnumerator DownLoadGameServerList(JsonData jsData,string uri)
        {
            Log.Warning(uri);
            string postData = JsonMapper.ToJson(jsData);
            Log.Warning("拉取服务器列表-》{0}",postData);
            UnityWebRequest request = new UnityWebRequest(uri, "POST");
            byte[] data = (byte[]) null;
            if (!string.IsNullOrEmpty(postData))
                data = Encoding.UTF8.GetBytes(postData);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
            request.uploadHandler.contentType = "application/json";
            request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            
            if (request.error == null)
            {
                string result = System.Text.Encoding.UTF8.GetString(request.downloadHandler.data);
                Log.Warning( "拉取服务器列表返回--->" + result );
                JsonData res = JsonMapper.ToObject(result);
                JsonData serverList = res["svr_list"];
                LC.SendEvent("Get_server_list_back",res.toLuaTable(),res["svr_list"]);
                /*accid = res["acc_id"].ToString();
                token = res["token"].ToString();*/
            }
            else
            {
                //当center服务器取数据失败时重新拉取
            }

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
                pc.ServerTimeResetCB = ResetServerTime;
            });
        }
        public void ResetServerTime(long st)
        {
            ServerTime.Init(st);
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