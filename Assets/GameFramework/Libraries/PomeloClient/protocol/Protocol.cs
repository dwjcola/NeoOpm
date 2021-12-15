using System;
//using SimpleJson;
using LitJson;
using System.Text;
using pb = global::Google.Protobuf;
using System.IO;

namespace Pomelo.DotNetClient
{

    public class MsgProtocol
    {
        static public int WriteInt32BE(byte[] buffer, int offset, int value)
        {
            buffer[offset++] = ((byte)(value >> 24));
            buffer[offset++] = ((byte)(value >> 16));
            buffer[offset++] = ((byte)(value >> 8));
            buffer[offset++] = ((byte)value);
            return 4;
        }
        static public int WriteInt64BE(byte[] buffer, int offset, int value)
        {
            buffer[offset++] = ((byte)(value >> 56));
            buffer[offset++] = ((byte)(value >> 48));
            buffer[offset++] = ((byte)(value >> 40)); 
            buffer[offset++] = ((byte)(value >> 32)); 
            buffer[offset++] = ((byte)(value >> 24));
            buffer[offset++] = ((byte)(value >> 16));
            buffer[offset++] = ((byte)(value >> 8));
            buffer[offset++] = ((byte)value);

            return 8;
        }
        static public int WriteInt16BE(byte[] buffer, int offset, int value)
        {
            //buffer[offset++] = ((byte)(value >> 24));
            //buffer[offset++] = ((byte)(value >> 16));
            buffer[offset++] = ((byte)(value >> 8));
            buffer[offset++] = ((byte)value);
            return 2;
        }

        static public uint ReadUInt32BE(byte[] buffer, int offset )
        {
            uint value = (uint)(buffer[offset] << 24) | (uint)(buffer[offset+1] << 16) |
                (uint)(buffer[offset+2] << 8) | (uint)(buffer[offset+3] ) ;
            return value;
        }
        static public int ReadInt32BE(byte[] buffer, int offset)
        {
            uint value = (uint)(buffer[offset] << 24) | (uint)(buffer[offset + 1] << 16) |
                (uint)(buffer[offset + 2] << 8) | (uint)(buffer[offset + 3]);
            return (int)value;
        }

        static public int ReadInt64BE(byte[] buffer, int offset)
        {
            uint value =
                (uint)(buffer[offset++] << 56) |
                (uint)(buffer[offset++] << 48) |
                (uint)(buffer[offset++] << 40) |
                (uint)(buffer[offset++] << 32) |
                (uint)(buffer[offset++]   << 24) |
                (uint)(buffer[offset++] << 16) |
                (uint)(buffer[offset++] << 8) |
                (uint)(buffer[offset++]);

            return (int)value;
        }

        static public ushort ReadUShortBE(byte[] buffer, int offset)
        {
            ushort result = 0;
            result += (ushort)(buffer[offset] << 8);
            result += (ushort)(buffer[offset + 1]);
            return result;
        }

        static public byte[] encode(JsonData msg)
        {
            return Encoding.UTF8.GetBytes(msg.ToJson());
        }
        static public byte[] encode(pb.IMessage msg)
        {
            MemoryStream m = new MemoryStream();
            var cos = new pb.CodedOutputStream(m);
            msg.WriteTo(cos);
            cos.Flush();
            return m.ToArray();
        }
        static public T decode<T>(byte[] buffer, int offset) where T:pb.IMessage,new()
        {
            var cis = new pb.CodedInputStream(buffer, offset, buffer.Length - offset);
            T t = new T();
            t.MergeFrom(cis);
            return t;
        }

        static public JsonData decode(byte[] buffer, int offset)
        {
            JsonData msg = null;

            if (null != buffer)
            {
                var str = Encoding.UTF8.GetString(buffer, offset, buffer.Length - offset);
                msg = JsonMapper.ToObject(str);
            }
            return msg;
        }

    }

    public class Protocol
    {
        private MsgProtocol messageProtocol;
        private ProtocolState state;
        private Transporter transporter;
        private HandShakeService handshake;
        private HeartBeatService heartBeatService = null;
        private PomeloClient pc;

        public PomeloClient getPomeloClient()
        {
            return this.pc;
        }

        public Protocol(PomeloClient pc, System.Net.Sockets.Socket socket)
        {
            this.pc = pc;
            this.transporter = new Transporter(socket, this.processMessage);
            this.transporter.onDisconnect = onDisconnect;

            this.handshake = new HandShakeService(this);
            this.state = ProtocolState.start;
        }

        internal void start(JsonData user, Action<JsonData> callback)
        {
            this.transporter.start();
            this.handshake.request(user, callback);

            this.state = ProtocolState.handshaking;
        }

        //Send notify, do not need id



        internal void send(MessageType type, uint id, uint serviceId, JsonData msg)
        {

            if (this.state != ProtocolState.working) return;

            byte[] body = MsgProtocol.encode(msg);

            send(type, id, serviceId, body);
        }
        internal void send(MessageType type, uint id,uint serviceId, pb.IMessage msg)
        {
            if (this.state != ProtocolState.working) return;

            byte[] body = MsgProtocol.encode( msg);

            send(type, id,serviceId, body);
        }

        internal void send(MessageType type, uint reqId, uint serviceId, byte[] body)
        {
            if (this.state == ProtocolState.closed) return;

            byte[] head = PackageProtocol.encode(type, reqId, serviceId, body);

            transporter.send(head, body);
        }
        internal void send(MessageType type, uint id, uint serviceId)
        {
            if (this.state == ProtocolState.closed) return;

            byte[] head = PackageProtocol.encode(type, id, serviceId, null);

            transporter.send(head);
        }
        //Send request, user request id 
        //internal void send(string route, uint id, JsonData msg)
        //{
        //    if (this.state != ProtocolState.working) return;

        //    byte[] body = messageProtocol.encode(route, id, msg);

        //    send(PackageType.PKG_DATA, body);
        //}


        internal void send(PackageType type)
        {
            if (this.state == ProtocolState.closed) return;
            transporter.send(PackageProtocol.encode(type));
        }

        //Send system message, these message do not use messageProtocol
        //internal void send(PackageType type, JsonObject msg)
        //{
        //    //This method only used to send system package
        //    if (type == PackageType.PKG_DATA) return;

        //    byte[] body = Encoding.UTF8.GetBytes(msg.ToString());

        //    send(type, body);
        //}

        //Send message use the transporter
        internal void send(PackageType type, byte[] body)
        {
            if (this.state == ProtocolState.closed) return;

            byte[] head = PackageProtocol.encode(type, body);

            transporter.send(head, body);
        }

        //Invoke by Transporter, process the message

        //to do  not in main thread
        //static public bool IsDataPackage(PackageType type)
        //{
        //    return type == PackageType.PKG_DATA_NOTIFY || type == PackageType.PKG_DATA_PUSH 
        //        || type == PackageType.PKG_DATA_REQ || type == PackageType.PKG_DATA_RESPONSE;
        //} 
        internal void processMessage(byte[] bytes)
        {
            Package pkg = PackageProtocol.decode(bytes);

            //Ignore all the message except handshading at handshake stage
            if (pkg.type == PackageType.PKG_HANDSHAKE && this.state == ProtocolState.handshaking)
            {

                //Ignore all the message except handshading
                //JsonObject data = (JsonObject)SimpleJson.SimpleJson.DeserializeObject(Encoding.UTF8.GetString(pkg.body));

                JsonData data = MsgProtocol.decode(pkg.body, 0);
                bool flag = processHandshakeData(data);
                if (flag == false)
                {
                    pc.ReconnectCallBack?.Invoke(PomeloClient.CLIENT_EVENT_DISCONNECT);
                    return;
                }
                this.state = ProtocolState.working;
            }
            else if (pkg.type == PackageType.PKG_HEARTBEAT && this.state == ProtocolState.working)
            {
                this.heartBeatService.resetTimeout();
                if (!BitConverter.IsLittleEndian) {
                    Array.Reverse(pkg.body);
                }
                   
                ulong server_timestamp_ms = BitConverter.ToUInt64(pkg.body, 0);
                pc.ServerTimeResetCB?.Invoke((long)server_timestamp_ms);

                
            }
            else if (pkg.type == PackageType.PKG_DATA&& this.state == ProtocolState.working)
            {
                this.heartBeatService.resetTimeout();
                //ת�߳�
                pc.AddMsg(PackageProtocol.decode(pkg));
            }
            else if (pkg.type == PackageType.PKG_KICK)
            {
                this.getPomeloClient().disconnect();
                this.close();
                pc.ReconnectCallBack?.Invoke(PomeloClient.CLIENT_EVENT_DISCONNECT);
            }
        }

        private bool processHandshakeData(JsonData msg)
        {
            //Handshake error
            //if (!msg.ContainsKey("code") || !msg.ContainsKey("sys") || Convert.ToInt32(msg["code"]) != 200)
            //{
            //    throw new Exception("Handshake error! Please check your handshake config.");
            //}

            //Set compress data
            //JsonObject sys = (JsonObject)msg["sys"];

            //JsonObject dict = new JsonObject();
            //if (sys.ContainsKey("dict")) dict = (JsonObject)sys["dict"];

            //JsonObject protos = new JsonObject();
            //JsonObject serverProtos = new JsonObject();
            //JsonObject clientProtos = new JsonObject();

            //if (sys.ContainsKey("protos"))
            //{
            //    protos = (JsonObject)sys["protos"];
            //    serverProtos = (JsonObject)protos["server"];
            //    clientProtos = (JsonObject)protos["client"];
            //}

            //messageProtocol = new MessageProtocol(dict, serverProtos, clientProtos);

            //Init heartbeat service
            if (!msg.Keys.Contains("code") || (int)(msg["code"]) != 200)
            {
                string code = "";
                if (msg.Keys.Contains("code"))
                {
                    code = msg["code"].ToString();
                }
                handshake.invokeCallback(null);
                return false;
            }
            int interval = 10;
            if (msg.Keys.Contains("heartbeat")) interval = (int)(msg["heartbeat"]);

            heartBeatService = new HeartBeatService(interval, this);

            if (interval > 0)
            {
                heartBeatService.start();
            }

            //send ack and change protocol state
            handshake.ack();
            this.state = ProtocolState.working;

            //Invoke handshake callback
            JsonData user = new JsonData();
            if (msg.Keys.Contains("user")) user = (JsonData)msg["user"];
            handshake.invokeCallback(user);
            return true;
        }

        //The socket disconnect
        private void onDisconnect()
        {
            pc.StartReconnect(this);
        }

        internal void close()
        {
            transporter.close();

            if (heartBeatService != null) heartBeatService.stop();

            this.state = ProtocolState.closed;
        }
    }
}