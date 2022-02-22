using System;
using LitJson;
using System.Text;
using pb= global::Google.Protobuf;
using System.IO;

namespace Pomelo.DotNetClient
{
    public enum Code : byte
    {
        Null = 0,
        Json = 1,
        Protobuf = 2,
        Protobufc = 3,
        SfxProtobuf = 13,
        Misc = 10
    }
    #region sender
    public interface ISendTransfer
    {
        Code Type
        {
            get;
        }
        byte[] msgBody
        {
            get;
        }
    }

    public class JsonSender : ISendTransfer
    {
        private JsonData msg;
        public JsonSender(JsonData msg)
        {
            this.msg = msg;
        }

        public Code Type => Code.Json;
        public byte[] msgBody=>Encoding.UTF8.GetBytes(msg.ToString());
    }
    public class ProtobufSender<T> : ISendTransfer where T : pb.IMessage
    {
        private T msg;
        private Code type;
        public ProtobufSender(T msg, Code type)
        {
            this.msg = msg;
            this.type = type;
        }
        public Code Type => type;
        public byte[] msgBody
        {
            get
            {
                MemoryStream m = new MemoryStream();
                var cos = new pb.CodedOutputStream(m);
                msg.WriteTo(cos);
                cos.Flush();
                return m.ToArray();
            }
        }
    }
    public class LuaProtobufSender : ISendTransfer
    {
        private string msgName;
        private string data;
        private Code type;
        public LuaProtobufSender(string msgName, string bytes, Code type)
        {
            this.msgName = msgName;
            this.type = type;
            data = bytes;
        }
        public Code Type
        {
            get
            {
                return type;
            }
        }
        public byte[] msgBody
        {
            get
            {
                MemoryStream m = new MemoryStream();  
                var cos = new pb.CodedOutputStream(m);
                pb.ByteString msg = pb.ByteString.CopyFromUtf8(data);

                cos.Flush();
                msg.WriteTo(m);
                return m.ToArray();
            }
        }
    }
    #endregion
    #region receiver
    public interface IReceiveTransfer
    {
        Delegate Callback
        {
            get;
        }
        void doHandle(byte[] data, int offset, byte bodyType,uint errorCode);

    }
    public class RawReceiver : IReceiveTransfer
    {
        private Action<byte[],int > callback;
        public Delegate Callback
        {
            get { return callback; }
        }
        public RawReceiver(Action<byte[], int> callback)
        {
            this.callback = callback;
        }
        public void doHandle(byte[] buffer, int offset, byte bodyType,uint errorCode)
        {
            callback(buffer, offset);
        }
    }
    public class JsonReceiver : IReceiveTransfer
    {
        private Action<JsonData> callback;
        public Delegate Callback
        {
            get { return callback; }
        }
        public JsonReceiver(Action<JsonData> callback)
        {
            this.callback = callback;
        }
        public void doHandle(byte[] buffer, int offset, byte bodyType,uint errorCode)
        {
            JsonData msg = null;

            if (null != buffer)
            {
                var str = Encoding.UTF8.GetString(buffer, offset, buffer.Length - offset);
                msg = JsonMapper.ToObject(str );
            }
            callback(msg);
        }
    }
    public class ProtobufReceiver<T> : IReceiveTransfer where T : pb.IMessage, new()
    {
        private Action<T> callback;
        public Delegate Callback => callback;
        public ProtobufReceiver(Action<T> callback_)
        {
            callback = callback_;
        }
        public void doHandle(byte[] buffer, int offset, byte bodyType,uint errorCode)
        {
            var cis = new pb.CodedInputStream(buffer, offset, buffer.Length - offset);
            T t = new T();
            t.MergeFrom(cis);
            callback(t);
        }
        public void doCB(object data)
        {
            callback((T)data);
        }
    }
    public class LuajsonReceiver : IReceiveTransfer
    {
        private Action<uint,string> callback;
        public Delegate Callback
        {
            get { return callback; }
        }
        public LuajsonReceiver(Action<uint,string> callback_)
        {
            callback = callback_;
        }
        public void doHandle(byte[] buffer, int offset, byte bodyType,uint errorCode)
        {
            if (errorCode > 0) //有错误则不进行解析
            {
                callback?.Invoke(errorCode,"");
            }
            else
            {
                string str = "";
                if (null != buffer)
                {
                    str = Encoding.UTF8.GetString(buffer, offset, buffer.Length - offset);
                
                }
                callback?.Invoke(errorCode,str);
            }
            
        }
        public void doCB(object data)
        {
            callback(0,data.ToString());
        }
    }
    public class LuaProtobufReceiver : IReceiveTransfer
    {
        private Action<uint,byte[]> callback;
        public Delegate Callback
        {
            get { return callback; }
        }
        public LuaProtobufReceiver(Action<uint,byte[]> callback_)
        {
            callback = callback_;
        }
        public void doHandle(byte[] buffer, int offset, byte bodyType,uint errorCode)
        {
            if (errorCode > 0)//有错误则不进行解析
            {
                callback?.Invoke(errorCode,null);
            }
            else
            {
                MemoryStream m = new MemoryStream(buffer, offset, buffer.Length - offset);
                callback?.Invoke(errorCode,m.ToArray());
            }
            
        }
        public void doCB(object data)
        {
            callback(0,(byte[])data);
        }
    }

    #endregion
}