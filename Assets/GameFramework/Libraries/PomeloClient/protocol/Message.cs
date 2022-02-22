using System;

namespace Pomelo.DotNetClient
{
    public class Message
    {
        //public MessageType type;
        public MessageType type;
        //public string route;
        public uint id;
        public uint errorCode;
        public byte[] body;
        //public int bodyLen;
        public int offset;
        //public JsonObject data;



        public Message(MessageType type,uint id,uint errorCode,byte[]body ,int offset )
        {
            this.type = type;
            this.id = id;
            this.errorCode = errorCode;
            this.body = body;
            this.offset = offset;
        }
    }
}