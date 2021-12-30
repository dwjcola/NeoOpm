using System;

namespace Pomelo.DotNetClient
{
    public class PackageProtocol
    {
        //0x75DD(2 Byte)  --Header(1 Byte)  --Length（4 Byte） --MsgId（2 Byte）  --requestId（2 Byte）
        public const int HEADER_LENGTH = 11;
        public const int HEADER_LENGTH_SC = 5;//返回包包头长度
        public const int MSG_HEAD = 4;//服务器用包头

        public static byte[] encodeHeartbeat()
        {
            return new byte[] { 1 };
        }
        /**
        *  format:
        * +------+------+------------+-------------+------------------+
        * | head(3b) | body length(4b) | serviceId(2b)| reqId(2b) | body |
        * +------+------+------------+-------------+------------------+
        **/
        public static byte[] encode(uint reqId, uint serviceId, byte[] msgBody)
        {
            int pkgBodyLen = 0;
            if (msgBody != null) pkgBodyLen += msgBody.Length;
            byte[] buf = new byte[HEADER_LENGTH];
            int offset = 0;
            buf[offset++] = ((byte)(MSG_HEAD >> 8));
            buf[offset++] = ((byte)MSG_HEAD);
            buf[offset++] = ((byte)0);//预留1byte
            offset += MsgProtocol.WriteInt32BE(buf, offset, pkgBodyLen);
            uint srId = (serviceId << 16) | reqId;
            offset += MsgProtocol.WriteInt32BE(buf, offset, (int)srId);
            return buf;
        }
        public static Package decode(byte[] buf)
        {
            PackageType type = PackageType.PKG_DATA;
            int serviceId = buf[5] << 8 | buf[6];
            if (serviceId < (int)PackageType.PKG_LEN-1)
            {
                type = (PackageType)serviceId;
            }

            byte[] body = new byte[buf.Length - HEADER_LENGTH_SC];

            for (int i = 0; i < body.Length; i++)
            {
                body[i] = buf[i + HEADER_LENGTH_SC];
            }

            return new Package(type, body);
        }


        public static Message decode(Package pkg)
        {
            //Decode head
            //Get flag
            var buffer = pkg.body;
            uint serviceId = MsgProtocol.ReadUShortBE(buffer, 0);
            MessageType type = MessageType.MSG_RESPONSE;
            if (serviceId >= (int)MessageType.MSG_PUSH_START)
            {
                type = MessageType.MSG_PUSH;
            }

            //Construct the message
            return new Message(type, serviceId, buffer, 2);

        }

    }
}
 