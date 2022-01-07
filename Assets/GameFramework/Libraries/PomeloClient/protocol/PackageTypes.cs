using System;

namespace Pomelo.DotNetClient
{
    public enum PackageType
    {
        /*PKG_HANDSHAKE = 1,
        PKG_HANDSHAKE_ACK = 2,*/
        PKG_HEARTBEAT = 0,
        //PKG_KICK = 1,
        PKG_DATA =1,
        PKG_LEN = 2
        

        //PKG_DATA_REQ        = 5 ,
        //PKG_DATA_NOTIFY     = 6 ,
        //PKG_DATA_RESPONSE   = 7 , 
        //PKG_DATA_PUSH       = 8 
 
 

    }
}