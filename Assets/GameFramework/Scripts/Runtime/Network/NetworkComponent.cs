//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using GameFramework;
using GameFramework.Network;
using LitJson;
using Pomelo.DotNetClient;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 网络组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Network")]
    public sealed class NetworkComponent : GameFrameworkComponent
    {
        private INetworkManager m_NetworkManager = null;
        private EventComponent m_EventComponent = null;
        private float m_DelayReconnectTime = 1f;
        private float m_ReconnectTime = -1f;

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get
            {
                return m_NetworkManager.NetworkChannelCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_NetworkManager = GameFrameworkEntry.GetModule<INetworkManager>();
            if (m_NetworkManager == null)
            {
                Log.Fatal("Network manager is invalid.");
                return;
            }

            m_NetworkManager.NetworkConnected += OnNetworkConnected;
            m_NetworkManager.NetworkClosed += OnNetworkClosed;
            m_NetworkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
            m_NetworkManager.NetworkError += OnNetworkError;
            m_NetworkManager.NetworkCustomError += OnNetworkCustomError;
        }
        private void Update()
        {
            PomeloClient.Instance.Update(Time.deltaTime, Time.deltaTime);
            if (m_ReconnectTime>=0f)
            {
                m_ReconnectTime += Time.deltaTime;
                if (m_ReconnectTime>=m_DelayReconnectTime)
                {
                    m_ReconnectTime = -1f;
                    PomeloClient.Instance.disconnect();
                    PomeloClient.Instance.reconnect();
                    
                }
            }
        }
        public void StartReconnect()
        {
            m_ReconnectTime = 0f;
        }
        //const string host = "127.0.0.1";
        //const int port = 11303;
        public async void ConnectToServer(string host,int port,Action succ,Action initCB)
        {
            bool ret = await PomeloClient.Instance.initClient(host, port);
            if (ret == true)
            {
                //The user data is the handshake user params
                JsonData user = new JsonData();
                user["name"] = "slg";
                //Log.Error("PomeloClient Init Suc.connect ...");
                initCB?.Invoke();
                JsonData connRet = await PomeloClient.Instance.connect(user);
                if (connRet.Keys.Contains("error"))
                {
                    Log.Error("connect error!" + connRet["error"]);
                }
                else
                {
                    succ();  
                }
            }
            else
            {
                Log.Error("PomeloClient init error....");
            }
        }

        private void Start()
        {
            //InitClient();
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }
            PomeloClient pc = PomeloClient.Instance;
            pc.DelayAndReconnect = StartReconnect;
        }
        /*private void DelayAndReconnect(float delay,Action func)
        {
            try
            {
                StopAllCoroutines();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            StartCoroutine(DelayReconnect(delay,func));
        }*/
        /*private IEnumerator DelayReconnect(float delaytime,Action func)
        {
            yield return new WaitForSeconds(delaytime);
            func?.Invoke();
        }*/
/// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return m_NetworkManager.HasNetworkChannel(name);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return m_NetworkManager.GetNetworkChannel(name);
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            return m_NetworkManager.GetAllNetworkChannels();
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <param name="results">所有网络频道。</param>
        public void GetAllNetworkChannels(List<INetworkChannel> results)
        {
            m_NetworkManager.GetAllNetworkChannels(results);
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="serviceType">网络服务类型。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name, ServiceType serviceType, INetworkChannelHelper networkChannelHelper)
        {
            return m_NetworkManager.CreateNetworkChannel(name, serviceType, networkChannelHelper);
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            return m_NetworkManager.DestroyNetworkChannel(name);
        }

        private void OnNetworkConnected(object sender, GameFramework.Network.NetworkConnectedEventArgs e)
        {
            m_EventComponent.Fire(this, NetworkConnectedEventArgs.Create(e));
        }

        private void OnNetworkClosed(object sender, GameFramework.Network.NetworkClosedEventArgs e)
        {
            m_EventComponent.Fire(this, NetworkClosedEventArgs.Create(e));
        }

        private void OnNetworkMissHeartBeat(object sender, GameFramework.Network.NetworkMissHeartBeatEventArgs e)
        {
            m_EventComponent.Fire(this, NetworkMissHeartBeatEventArgs.Create(e));
        }

        private void OnNetworkError(object sender, GameFramework.Network.NetworkErrorEventArgs e)
        {
            m_EventComponent.Fire(this, NetworkErrorEventArgs.Create(e));
        }

        private void OnNetworkCustomError(object sender, GameFramework.Network.NetworkCustomErrorEventArgs e)
        {
            m_EventComponent.Fire(this, NetworkCustomErrorEventArgs.Create(e));
        }

    }
}
