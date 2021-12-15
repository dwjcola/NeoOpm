using System;
using LitJson;

namespace Pomelo.DotNetClient.Test
{
    public class ClientTest
    {
        public static PomeloClient pc = null;

        public static void loginTest(string host, int port)
        {
            pc = new PomeloClient();

            pc.NetWorkStateChangedEvent += (state) =>
            {
                Console.WriteLine(state);
            };


            //pc.initClient(host, port, () =>
            //{
            //    pc.connect(null, data =>
            //    {

            //        Console.WriteLine("on data back" + data.ToString());
            //        JsonData msg = new JsonData();
            //        msg["uid"] = 111;
            //        //pc.request("gate.gateHandler.queryEntry", msg, OnQuery);
            //        pc.request(100, msg, OnQuery);

            //    });
            //});
        }

        public static void OnQuery(JsonData result)
        {
            if (Convert.ToInt32(result["code"]) == 200)
            {
                pc.disconnect();

                string host = (string)result["host"];
                int port = Convert.ToInt32(result["port"]);
                pc = new PomeloClient();

                pc.NetWorkStateChangedEvent += (state) =>
                {
                    Console.WriteLine(state);
                };

                //pc.initClient(host, port, () =>
                //{
                //    pc.connect(null, (data) =>
                //    {
                //        JsonData userMessage = new JsonData();
                //        Console.WriteLine("on connect to connector!");

                //        //Login
                //        JsonData msg = new JsonData();
                //        msg["username"] = "test";
                //        msg["rid"] = "pomelo";

                //        //pc.request("connector.entryHandler.enter", msg, OnEnter);
                //        pc.request(200, msg, OnEnter);



                //    });
                //});
            }
        }

        public static void OnEnter(JsonData result)
        {
            Console.WriteLine("on login " + result.ToString());
        }

        public static void onDisconnect( )
        {
            Console.WriteLine("on sockect disconnected!");
        }

        public static void Run()
        {
            string host = "192.168.0.156";
            int port = 3014;

            loginTest(host, port);
        }
    }
}