using System;
using System.Timers;

namespace Pomelo.DotNetClient
{
    public class HeartBeatService
    {
        int interval;
        public int timeout;
        Timer timer;
        DateTime lastTime;

        Protocol protocol;

        public HeartBeatService(int interval, Protocol protocol)
        {
            this.interval = interval * 1000;
            this.protocol = protocol;
        }

        internal void resetTimeout()
        {
            this.timeout = 0;
            lastTime = DateTime.Now;
        }

        public void sendHeartBeat(object source, ElapsedEventArgs e)
        {
            TimeSpan span = DateTime.Now - lastTime;
            timeout = (int)span.TotalMilliseconds;

            //check timeout
            if (timeout > interval * 2)
            {
                this.resetTimeout();
                //PomeloClient.Instance.StartReconnect(protocol);
                return;
            }

            sendHeartBeat();

        }

        private void sendHeartBeat()
        {
            //Send heart beat
            protocol.sendHeartbeat();
            //Debug.LogError(string.Format("============================3============================send heart beat{0}", DateTime.Now));
        }
        public void start()
        {
            if (interval < 1000) return;
            sendHeartBeat();
            //start hearbeat
            this.timer = new Timer();
            timer.Interval = interval;
            timer.Elapsed += new ElapsedEventHandler(sendHeartBeat);
            timer.Enabled = true;

            //Set timeout
            timeout = 0;
            lastTime = DateTime.Now;
        }

        public void stop()
        {
            if (this.timer != null)
            {
                this.timer.Enabled = false;
                this.timer.Dispose();
            }
        }
    }
}