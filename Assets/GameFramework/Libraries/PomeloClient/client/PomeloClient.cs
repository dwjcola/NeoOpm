using LitJson;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using pb = global::Google.Protobuf;

namespace Pomelo.DotNetClient
{
    /// <summary>
    /// network state enum
    /// </summary>
    public enum NetWorkState
    {
        [Description("initial state")]
        CLOSED,

        [Description("connecting server")]
        CONNECTING,

        [Description("server connected")]
        CONNECTED,

        [Description("disconnected with server")]
        DISCONNECTED,

        [Description("connect timeout")]
        TIMEOUT,

        [Description("netwrok error")]
        ERROR
    }

    public class PomeloClient : IDisposable
    {
        public const string CLIENT_EVENT_SHUTDOWN = "shutdownconnect";
        public const string CLIENT_EVENT_DISCONNECT = "disconnect";
        public const string CLIENT_EVENT_RECONNECT = "reconnect";
        public const string CLIENT_EVENT_CONNECTED = "connected";
        /// <summary>
        /// netwrok changed event
        /// </summary>
        public event Action<NetWorkState> NetWorkStateChangedEvent;

        ConcurrentQueue<Message> msgQueue = new ConcurrentQueue<Message>();


        private NetWorkState netWorkState = NetWorkState.CLOSED;   //current network state

        private EventManager eventManager;
        private Socket socket;
        private Protocol protocol;
        private bool disposed = false;
        private uint reqId = 1;

        private ManualResetEvent timeoutEvent = new ManualResetEvent(false);
        private int timeoutMSec = 8000;    //connect timeout count in millisecond

        private static PomeloClient instance;
        public static PomeloClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PomeloClient();
                }
                return instance;
            }
        }

        public Action<long> ServerTimeResetCB;
        public void AddMsg(Message msg)
        {
            msgQueue.Enqueue(msg);
        }
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if( netWorkState != NetWorkState.CONNECTED)
            {
                return;
            }
            //work queue
            while (msgQueue.Count > 0)
            {
                Message msg;
                if( msgQueue.TryDequeue(out msg))
                {
                    //do 
                    if( msg.type == MessageType.MSG_RESPONSE)
                    {
                        eventManager.InvokeCallBack(msg.id,msg);
                    }else if(msg.type == MessageType.MSG_PUSH)
                    {
                        eventManager.InvokeOnEvent(msg.id, msg);
                    }
                    else
                    {
                        //error

                    }
                }
            }

        }
        /// <summary>
        /// initialize pomelo client
        /// </summary>
        /// <param name="host">server name or server ip (www.xxx.com/127.0.0.1/::1/localhost etc.)</param>
        /// <param name="port">server port</param>
        /// <param name="callback">socket successfully connected callback(in network thread)</param>
        public  Task<bool> initClient(string host, int port )
        {
            /*connectIP = host;
            connectPort = port;*/
            TaskCompletionSource<bool> taskCS = new TaskCompletionSource<bool>();
            timeoutEvent.Reset();
            eventManager = new EventManager();
            NetWorkChanged(NetWorkState.CONNECTING);
            IPAddress ipAddress = null;
            try
            {
                ipAddress = IPAddress.Parse(host);    
            }
            catch (Exception e)
            {
                NetWorkChanged(NetWorkState.ERROR);
                taskCS.SetResult(false);
                return taskCS.Task;
            }
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ie = new IPEndPoint(ipAddress, port);

            socket.BeginConnect(ie, new AsyncCallback((result) =>
            {
                try
                {
                    this.socket.EndConnect(result);
                    this.protocol = new Protocol(this, this.socket);
                    protocol.start();
                    NetWorkChanged(NetWorkState.CONNECTED);
                    taskCS.SetResult(true);
                }
                catch (SocketException e)
                {
                    if (netWorkState != NetWorkState.TIMEOUT)
                    {
                        NetWorkChanged(NetWorkState.ERROR);
                    }
                    Dispose();
                    taskCS.SetResult(false);
                }
                finally
                {
                    timeoutEvent.Set();

                }
            }), this.socket);

            if (timeoutEvent.WaitOne(timeoutMSec, false))
            {
                if (netWorkState != NetWorkState.CONNECTED && netWorkState != NetWorkState.ERROR)
                {
                    NetWorkChanged(NetWorkState.TIMEOUT);
                    Dispose();
                    taskCS.SetResult(false);
                }
            }
            return taskCS.Task;
        }

        /// <summary>
        /// 网络状态变化
        /// </summary>
        /// <param name="state"></param>
        private void NetWorkChanged(NetWorkState state)
        {
            netWorkState = state;

            if (NetWorkStateChangedEvent != null)
            {
                NetWorkStateChangedEvent(state);
            }
        }

        /// <summary>
        /// 无参
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="action"></param>
        public void request(uint serviceId, Action<byte[], int> action)
        {
            this.eventManager.AddCallBack(reqId, new RawReceiver(action));
            protocol.send(reqId, serviceId);
            reqId++;
        }
        /// <summary>
        /// byte[]
        /// </summary>
        /// <param name="serviceId"></param>
        /// <param name="msgData"></param>
        /// <param name="action"></param>
        public void request(uint serviceId, byte[] msgData, Action<byte[], int> action)
        {
            this.eventManager.AddCallBack(reqId, new RawReceiver(action));
            protocol.send(reqId, serviceId, msgData);
            reqId++;
        }

        /*public void request<R>(uint serviceId, pb.IMessage msg,Action<R> action) where R:pb.IMessage,new()
        {
            this.eventManager.AddCallBack(reqId, new ProtobufReceiver<R>(action));
            protocol.send(MessageType.MSG_REQUEST, reqId, serviceId, msg);
            reqId++;
        }
        public void request<R>(uint serviceId, JsonData msg,Action<R> action) where R:pb.IMessage,new()
        {
            this.eventManager.AddCallBack(reqId, new ProtobufReceiver<R>(action));
            protocol.send(MessageType.MSG_REQUEST, reqId, serviceId, msg);
            reqId++;
        }
        public void request<R>(uint serviceId, Action<R> action) where R : pb.IMessage, new()
        {
            this.eventManager.AddCallBack(reqId, new ProtobufReceiver<R>(action));
            protocol.send(reqId, serviceId );
            reqId++;
        }
        public void request(uint serviceId, JsonData msg, Action<JsonData> action)
        {
            this.eventManager.AddCallBack(reqId, new JsonReceiver(action));
            protocol.send(MessageType.MSG_REQUEST, reqId, serviceId, msg);
            reqId++;
        }*/
        public void LuaPBRequest(uint serviceId, byte[] msg, Action<byte[]> action)
        {
            this.eventManager.AddCallBack(reqId, new LuaProtobufReceiver(action));
            protocol.send(reqId, serviceId, msg);
            reqId++;
        }
        public void LuaJsonRequest(uint serviceid,string msg,Action<string> action)
        {
            this.eventManager.AddCallBack(reqId, new LuajsonReceiver(action));
            protocol.send(reqId, serviceid, Encoding.UTF8.GetBytes(msg));
            reqId++;

        }

        /*public void notify(uint serviceId, pb.IMessage msg)        
        {
            protocol.send(MessageType.MSG_NOTIFY,0, serviceId, msg);
        }
        public void notify(uint serviceId, JsonData msg)        
        {
            protocol.send(MessageType.MSG_NOTIFY, 0, serviceId, msg);
        }
        public void notify(uint serviceId )
        {
            protocol.send(MessageType.MSG_NOTIFY, 0, serviceId);
        }*/
        /// <summary>
        /// 这里和string参数的名字区分开是因为lua传到c# ,byte[]也是string，其他地方一样的方式
        /// </summary>
        /// <param name="serviceid"></param>
        /// <param name="msg"></param>
        /*public void LuaPBNotify(uint serviceid, byte[] msg)
        {
            protocol.send(MessageType.MSG_NOTIFY, 0, serviceid, msg);
        }
        public void notify(uint serviceId, string msg)
        {
            protocol.send(MessageType.MSG_NOTIFY,0, serviceId, Encoding.UTF8.GetBytes(msg));
        }*/

        public void on(uint id,Action<string> action)
        {
            eventManager.AddOnEvent(id, action);
        }
        public void LuaPBOn(uint id, Action<byte[]> action)
        {
            eventManager.AddOnEvent(id, action);
        }
        public void on(uint id, Action<JsonData> action)
        {
            eventManager.AddOnEvent(id, action);
        }
        public void on<T>(uint id, Action<T> action) where T:pb.IMessage,new()
        {
            eventManager.AddOnEvent(id, action);
        }
        public void on(uint id, Action<byte[],int> action)
        {
            eventManager.AddOnEvent(id, action);
        }
        public void RemoveEvent(uint id,Delegate action)
        {
            eventManager.RemoveEvent(id, action);
        }
        public void RemoveEvent(uint id, Action<string> action)
        {
            eventManager.RemoveEvent(id, action);
        }
        public void RemoveLuaPBEvent(uint id, Action<byte[]> action)
        {
            eventManager.RemoveEvent(id, action);
        }
        public void disconnect()
        {
            Dispose();
            //ReconnectCallBack?.Invoke(CLIENT_EVENT_SHUTDOWN);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                // free managed resources
                if (this.protocol != null)
                {
                    this.protocol.close();
                }

                if (this.eventManager != null)
                {
                    this.eventManager.Dispose();
                }

                try
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    this.socket = null;
                }
                catch (Exception)
                {
                    //todo : 有待确定这里是否会出现异常，这里是参考之前官方github上pull request。emptyMsg
                }

            }
            //释放非托管内存

            this.disposed = true;
        }
        public bool isConnect
        {
            get
            {
                return protocol != null && socket != null && this.netWorkState == NetWorkState.CONNECTED;
            }
        }
        /*/// <summary>
        /// 重连用
        /// </summary>
        /// <param name="old"></param>
        public void StartReconnect(Protocol old)
        {
            if(old!=null) old.close();
            if (old!= null && protocol != null && old != protocol)
            {                
                return;
            }

            reconnectCount = 0;
            disconnect();
            reconnect();
        }
        private int reconnectCount = 0;
        private int maxReconnectCount = 10;
        private string connectIP;
        private int connectPort;
        public Action<string> ReconnectCallBack;
        public Action DelayAndReconnect;
        
        public async void reconnect()
        {
            //Debug.LogWarning("断线重连第"+reconnectCount+"次");
            if (reconnectCount==0)
            {
                ReconnectCallBack?.Invoke(CLIENT_EVENT_RECONNECT);
            }
            reconnectCount++;
            bool ret = await initClient(connectIP, connectPort);
            if (ret == true)
            {
                //The user data is the handshake user params
                JsonData user = new JsonData();
                user["name"] = "slg";
                JsonData connRet = await PomeloClient.Instance.connect(user);
                if (connRet.Keys.Contains("error"))
                {
                    ConnectFail2Reconnect();
                }
                else
                {
                    reconnectCount = 0;
                    ReconnectCallBack?.Invoke(CLIENT_EVENT_CONNECTED);
                }
            }
            else
            {
                ConnectFail2Reconnect();
            }
        }
        private void ConnectFail2Reconnect()
        {
            //Debug.LogWarning("断线重连失败，准备重试");
            if (reconnectCount < maxReconnectCount)
            {
                DelayAndReconnect?.Invoke();
            }
            else
            {
                ReconnectCallBack?.Invoke(CLIENT_EVENT_DISCONNECT);
                netWorkState = NetWorkState.ERROR;
            }
        }*/
        public void Log(string msg)
        {
            Debug.LogError(msg);
        }
    }
}