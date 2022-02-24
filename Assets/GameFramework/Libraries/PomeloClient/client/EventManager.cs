using System;
using System.Collections.Generic;
using System.Text;
using LitJson;
using pb = global::Google.Protobuf;
namespace Pomelo.DotNetClient
{
    public class EventManager : IDisposable
    {
        private Dictionary<uint, List<IReceiveTransfer> > eventMap;
        private Dictionary<uint, IReceiveTransfer> callBackMap;
        public EventManager()
        {
            this.callBackMap = new Dictionary<uint, IReceiveTransfer>();
            this.eventMap = new Dictionary<uint, List<IReceiveTransfer>>();
        }

        //Adds callback to callBackMap by id.
        public void AddCallBack(uint id, IReceiveTransfer callback)
        {
            if (id > 0 && callback != null)
            {
                this.callBackMap.Add(id, callback);
            }
        }
        public void InvokeCallBack(uint id, Message msg)
        {
            IReceiveTransfer transfer;
            if (callBackMap.TryGetValue(id,out transfer))
            {
                transfer.doHandle(msg.body, msg.offset, (byte)msg.type,msg.errorCode);
            }
        }

        //Adds the event to eventMap by name.
        public void AddOnEvent(uint id, Action<JsonData> callback)
        {
            List< IReceiveTransfer> list = null;
            if (this.eventMap.TryGetValue(id, out list))
            {
                list.Add( new JsonReceiver(callback) );
            }
            else
            {
                list = new List<IReceiveTransfer>();
                list.Add(new JsonReceiver(callback));
                this.eventMap.Add(id, list);
            }
        }
        public void AddOnEvent<T>(uint id, Action<T> callback) where T:pb.IMessage,new()
        {
            List<IReceiveTransfer> list = null;
            if (this.eventMap.TryGetValue(id, out list))
            {
                list.Add(new ProtobufReceiver<T>(callback));
            }
            else
            {
                list = new List<IReceiveTransfer>();
                list.Add(new ProtobufReceiver<T>(callback));
                this.eventMap.Add(id, list);
            }
        }
        public void AddOnEvent(uint id, Action<uint,byte[]> callback)
        {
            List<IReceiveTransfer> list = null;
            if (this.eventMap.TryGetValue(id, out list))
            {
                list.Add(new LuaProtobufReceiver(callback));
            }
            else
            {
                list = new List<IReceiveTransfer>();
                list.Add(new LuaProtobufReceiver(callback));
                this.eventMap.Add(id, list);
            }
        }
        public void AddOnEvent(uint id, Action<byte[],int> callback)
        {
            List<IReceiveTransfer> list = null;
            if (this.eventMap.TryGetValue(id, out list))
            {
                list.Add(new RawReceiver(callback));
            }
            else
            {
                list = new List<IReceiveTransfer>();
                list.Add(new RawReceiver(callback));
                this.eventMap.Add(id, list);
            }
        }
        public void AddOnEvent(uint id, Action<uint,string> callback)
        {
            List<IReceiveTransfer> list = null;
            if (this.eventMap.TryGetValue(id, out list))
            {
                list.Add(new LuajsonReceiver(callback));
            }
            else
            {
                list = new List<IReceiveTransfer>();
                list.Add(new LuajsonReceiver(callback));
                this.eventMap.Add(id, list);
            }
        }

        public void RemoveEvent(uint id,Delegate action)
        {
            if (eventMap == null)
                return;
            List<IReceiveTransfer> list;
            if(this.eventMap.TryGetValue(id,out list))
            {
                int i = list.Count;
                while (--i > -1)
                {
                    if (list[i].Callback == action)
                    {
                        list.RemoveAt(i);
                    }
                }
            }
        }
        /// <summary>
        /// If the event exists,invoke the event when server return messge.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ///
        public void InvokeOnEvent(uint id, Message msg)
        {
            List<IReceiveTransfer> list;
            if (eventMap.TryGetValue(id,out list))
            {
                foreach (var action in list) 
                    action.doHandle(msg.body, msg.offset, (byte)msg.type,msg.errorCode);
            }
        }

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected void Dispose(bool disposing)
        {
            this.callBackMap.Clear();
            this.eventMap.Clear();
        }
    }
}