using System;
//using SimpleJson;
using LitJson;
using System.Text;
using pb = global::Google.Protobuf;
using System.IO;
using UnityEngine;

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

        static public ulong ReadInt64BE(byte[] buffer, int offset)
        {
            ulong a = buffer[offset++];
            a = a << 56;
            a = buffer[offset++];
            a = a << 48;
            a = buffer[offset++];
            a = a << 40;
            a = buffer[offset++];
            a = a << 32;
            a = buffer[offset++];
            a = a << 24;
            a = buffer[offset++];
            a = a << 16;
            a = buffer[offset++];
            a = a << 8;
            a = buffer[offset++];
            ulong value =
                (ulong)(buffer[offset++] << 56) |
                (ulong)(buffer[offset++] << 48) |
                (ulong)(buffer[offset++] << 40) |
                (ulong)(buffer[offset++] << 32) |
                (ulong)(buffer[offset++]   << 24) |
                (ulong)(buffer[offset++] << 16) |
                (ulong)(buffer[offset++] << 8) |
                (ulong)(buffer[offset++]);

            return (ulong)value;
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
        private HeartBeatService heartBeatService = null;
        private PomeloClient pc;
        private const int HEARTBEAT_INTERVAL = 10;

        public PomeloClient getPomeloClient()
        {
            return this.pc;
        }

        public Protocol(PomeloClient pc, System.Net.Sockets.Socket socket)
        {
            this.pc = pc;
            this.transporter = new Transporter(socket, this.processMessage);
            this.transporter.onDisconnect = onDisconnect;

            //this.state = ProtocolState.start;
            heartBeatService = new HeartBeatService(HEARTBEAT_INTERVAL, this);
            heartBeatService.start();
            this.state = ProtocolState.working;
        }

        internal void start()
        {
            this.transporter.start();
        }
        /*internal void send(MessageType type, uint id, uint serviceId, JsonData msg)
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
        }*/

        internal void send( uint reqId, uint serviceId, byte[] body)
        {
            if (this.state == ProtocolState.closed) return;

            byte[] head = PackageProtocol.encode( reqId, serviceId, body);

            transporter.send(head, body);
        }
        internal void send(uint id, uint serviceId)
        {
            if (this.state == ProtocolState.closed) return;

            byte[] head = PackageProtocol.encode( id, serviceId, null);

            transporter.send(head);
        }
        internal void sendHeartbeat()
        {
            if (this.state == ProtocolState.closed) return;
            transporter.send(PackageProtocol.encodeHeartbeat());
        }
        internal void processMessage(byte[] bytes)
        {
            Package pkg = PackageProtocol.decode(bytes);
            if (pkg.type == PackageType.PKG_HEARTBEAT && this.state == ProtocolState.working)
            {
                this.heartBeatService.resetTimeout();
                pc.Log("heartbeat!!!!!!!!!!!!");
                Array.Reverse(pkg.body);
                ulong server_timestamp_ms = BitConverter.ToUInt64(pkg.body, 0);
                pc.Log("-------------------"+server_timestamp_ms);
                pc.ServerTimeResetCB?.Invoke((long)server_timestamp_ms);
            }
            else if (pkg.type == PackageType.PKG_DATA&& this.state == ProtocolState.working)
            {     
                this.heartBeatService.resetTimeout();
                pc.AddMsg(PackageProtocol.decode(pkg));
            }
            else if (pkg.type == PackageType.PKG_KICK)
            {
                this.getPomeloClient().disconnect();
                this.close(); 
                //pc.ReconnectCallBack?.Invoke(PomeloClient.CLIENT_EVENT_DISCONNECT);
            }
        }


        //The socket disconnect
        private void onDisconnect()
        {
            //pc.StartReconnect(this);
        }

        internal void close()
        {
            transporter.close();

            if (heartBeatService != null) heartBeatService.stop();

            this.state = ProtocolState.closed;
        }
    }
}