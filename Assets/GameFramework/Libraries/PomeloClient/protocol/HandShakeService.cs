using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using LitJson;

namespace Pomelo.DotNetClient
{
    public class HandShakeService
    {
        private Protocol protocol;
        private Action<JsonData> callback;

        public const string Version = "0.3.0";
        public const string Type = "unity-socket";


        public HandShakeService(Protocol protocol)
        {
            this.protocol = protocol;
        }

        public void request(JsonData user, Action<JsonData> callback)
        {
            //byte[] body = Encoding.UTF8.GetBytes(buildMsg(user).ToString());
            var body = MsgProtocol.encode(user);
            protocol.send(PackageType.PKG_HANDSHAKE, body);

            this.callback = callback;
        }

        internal void invokeCallback(JsonData data)
        {
            //Invoke the handshake callback
            if (callback != null) callback.Invoke(data);
        }

        public void ack()
        {
            protocol.send(PackageType.PKG_HANDSHAKE_ACK, new byte[0]);
        }

    }
}