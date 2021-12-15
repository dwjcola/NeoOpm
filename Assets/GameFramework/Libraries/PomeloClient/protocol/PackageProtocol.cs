using System;

namespace Pomelo.DotNetClient
{
    public class PackageProtocol
    {
        public const int HEADER_LENGTH = 4;

        public static byte[] encode(PackageType type)
        {
            return new byte[] { Convert.ToByte(type), 0, 0, 0 };
        }
        public static byte[] encode(PackageType type, byte[] pkgBody)
        {
            int length = HEADER_LENGTH;

            byte[] buf = new byte[length];

            int index = 0;

            buf[index++] = Convert.ToByte(type);
            buf[index++] = Convert.ToByte(pkgBody.Length >> 16 & 0xFF);
            buf[index++] = Convert.ToByte(pkgBody.Length >> 8 & 0xFF);
            buf[index++] = Convert.ToByte(pkgBody.Length & 0xFF);
            return buf;
        }
        //public static byte[] encode(PackageType type, byte[] body)
        //{
        //    int length = HEADER_LENGTH;

        //    if (body != null) length += body.Length;

        //    byte[] buf = new byte[length];

        //    int index = 0;

        //    buf[index++] = Convert.ToByte(type);
        //    buf[index++] = Convert.ToByte(body.Length >> 16 & 0xFF);
        //    buf[index++] = Convert.ToByte(body.Length >> 8 & 0xFF);
        //    buf[index++] = Convert.ToByte(body.Length & 0xFF);

        //    while (index < length)
        //    {
        //        buf[index] = body[index - HEADER_LENGTH];
        //        index++;
        //    }

        //    return buf;
        //}

        /**
        *  format:
        * +------+------------+-------------+------------------+
        * | type |body length |   body (msgHead + msgBody)     |
        * +------+------------+-------------+------------------+
        **/

        public static byte[] encode(MessageType type, uint reqId, uint serviceId, byte[] msgBody)
        {
            int length = HEADER_LENGTH;

            int pkgBodyLen = 0;
            if (msgBody != null) pkgBodyLen += msgBody.Length;

            int msgHeadLen = reqId > 0 ? 8 : 4;
            length += msgHeadLen;
            pkgBodyLen += msgHeadLen;

            byte[] buf = new byte[length];

            int index = 0;

            buf[index++] = Convert.ToByte(PackageType.PKG_DATA);
            buf[index++] = Convert.ToByte(pkgBodyLen >> 16 & 0xFF);
            buf[index++] = Convert.ToByte(pkgBodyLen >> 8 & 0xFF);
            buf[index++] = Convert.ToByte(pkgBodyLen & 0xFF);


            int offset = HEADER_LENGTH;
            serviceId = (serviceId & 0xFFF) | (uint)( (byte)type << 24 );

            offset += MsgProtocol.WriteInt32BE(buf, offset, (int)serviceId);

            if (reqId > 0)
            {
                offset += MsgProtocol.WriteInt32BE(buf, offset, (int)reqId);
            }
            return buf;
        }
        public static Package decode(byte[] buf)
        {
            PackageType type = (PackageType)buf[0];

            byte[] body = new byte[buf.Length - HEADER_LENGTH];

            for (int i = 0; i < body.Length; i++)
            {
                body[i] = buf[i + HEADER_LENGTH];
            }

            return new Package(type, body);
        }


        public static Message decode(Package pkg)
        {
            //Decode head
            //Get flag
            var buffer = pkg.body;
            //var type = pkg.type;
            //Set offset to 1, for the 1st byte will always be the flag

            //Get type from flag;

            var type =(MessageType)buffer[0];


            uint id = 0;

            int offset = 1;
            if (type == MessageType.MSG_RESPONSE )
            {
                id = MsgProtocol.ReadUInt32BE(buffer, offset);
                offset += 4;
            }
            else if (type == MessageType.MSG_PUSH )
            {
                id = MsgProtocol.ReadUShortBE(buffer, offset);
                offset += 2;
            }
            else
            {
                return null;
            }

            //Construct the message
            return new Message(type, id, buffer, offset);

        }

    }
}
 