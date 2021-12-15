
using SLG;

namespace ProHA
{
    public static class MessageExtension
    {
        /*public static T MessageLua2C<T>( this T t,byte[] buffer) where T : Google.Protobuf.IMessage<T>, new()
        {
            var cis = new Google.Protobuf.CodedInputStream(buffer, 0, buffer.Length);
            t.MergeFrom(cis);
            return t;
        }*/
        /*local btArr = PomeloCLUA.pb.encode("EnterServerRsp",info)
        local message = CS.SLG.EnterServerRsp()
        message:MessageLua2C(btArr)*/
        public static EnterServerRsp MessageLua2C( this EnterServerRsp t,byte[] buffer)
        {
            var cis = new Google.Protobuf.CodedInputStream(buffer, 0, buffer.Length);
            t.MergeFrom(cis);
            return t;
        }
        public static ConfigTroopList MessageLua2C( this ConfigTroopList t,byte[] buffer)
        {
            var cis = new Google.Protobuf.CodedInputStream(buffer, 0, buffer.Length);
            t.MergeFrom(cis);
            return t;
        }
    }
    
}
