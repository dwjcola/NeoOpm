using UnityEngine;
using System;
namespace NeoOPM
{
// 界面用的计时器基类
    public abstract class UITimerBase : MonoBehaviour
    {
        //----------------------------------------------------------------
        // 常量
        public const int UNIT_SECOND = 1;
        public const int UNIT_MINUTE = 1 << 1;
        public const int UNIT_HOUR = 1 << 2;
        public const int UNIT_DAY = 1 << 3;
        public const int UNIT_ALL = UNIT_SECOND | UNIT_MINUTE | UNIT_HOUR | UNIT_DAY;
        public readonly static int[] UNIT_ARRAY = new int[] {UNIT_SECOND, UNIT_MINUTE, UNIT_HOUR, UNIT_DAY};
        public readonly static long[] UNIT_TOTAL_TICKS = new long[] {10000000, 600000000, 36000000000, 864000000000};
        protected const float SLOW_UPDATE_INTERVAL = 60f;
        protected const float FAST_UPDATE_INTERVAL = 0.3f;

        //----------------------------------------------------------------
        // 编辑器参数

        // 是否使用服务器时间
        public bool useServerTime = true;

        // 倒计时结束时发送的事件
        public string eventName = "";

        // 事件触发延迟（单位：秒）
        public float triggerDelay = 0;

        //----------------------------------------------------------------
        // 成员变量

        protected DateTime m_startTime = DateTime.MinValue; // 开始时间
        protected DateTime m_targetTime = DateTime.MinValue; // 结束时间
        protected DateTime m_finalTargetTime = DateTime.MinValue; // 实际结束时间
        protected TimeSpan m_leftTime = new TimeSpan(); // 剩余时间
        protected long m_totalTicks = 0; // 总Tick数
        protected float m_elapse = 0f; // 当前间隔内流逝时间
        protected float m_updateInterval = 1f; // 更新间隔
        protected bool m_timerStarted = false; // 是否已经开始计时
        protected int m_displayUnit = UNIT_ALL; // 显示的单位
        protected bool IsRealStart = false;
        //----------------------------------------------------------------
        // 公用函数

        /// <summary>
        /// 开始倒计时（如果觉得调用其他函数麻烦的话，调这个就可以了）
        /// </summary>
        /// <param name="milliseconds"></param>
        public virtual void StartCountdown(long milliseconds)
        {
            if (milliseconds <= 0)
            {
                return;
            }

            StopTimer(false);
            SetStartTimeNow();
            SetCountdown(milliseconds);
        }

        public virtual void StartCountdown(DateTime targetTime)
        {
            long milliseconds = (long) (targetTime - GetCurrentTime()).TotalMilliseconds;
            StartCountdown(milliseconds);
        }

        public virtual void StartCountdownT(long targetTime)
        {
            long milliseconds = (long) targetTime - ServerTime.CurrentTime();
            StartCountdown(milliseconds);
        }

        /// <summary>
        /// 设置开始时间
        /// </summary>
        /// <param name="startTime"></param>
        public virtual void SetStartTime(DateTime startTime)
        {
            m_startTime = startTime;
        }

        /// <summary>
        /// 设置开始时间为当前时间
        /// </summary>
        public virtual void SetStartTimeNow()
        {
            SetStartTime(GetCurrentTime());
        }

        /// <summary>
        /// 获取开始时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetStartTime()
        {
            return m_startTime;
        }

        /// <summary>
        /// 设置目标时间（结束时间）
        /// </summary>
        /// <param name="targetTime"></param>
        public virtual void SetTargetTime(DateTime targetTime)
        {
            DateTime currentTime = GetCurrentTime();
            m_targetTime = targetTime;
            m_timerStarted = true;
            enabled = m_targetTime > currentTime;
            if (enabled)
            {
                try
                {
                    m_finalTargetTime = (m_targetTime != DateTime.MaxValue && triggerDelay > 0)
                        ? m_targetTime.AddSeconds(triggerDelay)
                        : m_targetTime;
                }
                catch
                {
                    Logger.Error("[UITimerBase] Invalid target time: ");
                    m_finalTargetTime = m_targetTime;
                }

                m_leftTime = m_finalTargetTime - currentTime;
                m_elapse = float.MaxValue;
                IsRealStart = false;
                UpdateDisplayUnit();
                UpdateInterval();
                UpdateDisplay();
            }
            else
            {
                m_leftTime = m_targetTime - currentTime;
                StopTimer(true, false, true);
            }
        }

        /// <summary>
        /// 获取目标时间
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetTargetTime()
        {
            return m_targetTime;
        }

        /// <summary>
        /// 设置倒计时时间（开始时间+倒计时为结束时间）
        /// </summary>
        /// <param name="milliseconds"></param>
        public virtual void SetCountdown(long milliseconds)
        {
            SetTargetTime(new DateTime(GetCurrentTime().Ticks + milliseconds * 10000));
        }

        public virtual DateTime GetCurrentTime()
        {
            return useServerTime ? ServerTime.GetNowTime : DateTime.Now;
        }

        /// <summary>
        /// 获取剩余时间，单位：毫秒
        /// </summary>
        /// <returns></returns>
        public virtual long GetLeftTime()
        {
            DateTime currentTime = GetCurrentTime();
            return m_targetTime > currentTime ? (long) ((m_targetTime - currentTime).TotalMilliseconds) : 0;
        }

        /// <summary>
        /// 获取int型的剩余时间，lua用，注意时间太长可能截断
        /// </summary>
        /// <returns></returns>
        public virtual int GetLeftTimeInt()
        {
            return (int) GetLeftTime();
        }

        /// <summary>
        /// 设置显示的单位，关系到更新频率
        /// </summary>
        /// <param name="displayUnit"></param>
        public virtual void SetDisplayUnit(int displayUnit)
        {
            m_displayUnit = displayUnit;
        }

        /// <summary>
        /// 重置计时器，即停止计时器
        /// </summary>
        /// <param name="sendEvent"></param>
        public virtual void StopTimer(bool sendEvent = true, bool resetTime = false, bool updateDisplay = false)
        {
            enabled = false;
            if (resetTime)
            {
                SetStartTimeNow();
                m_finalTargetTime = m_targetTime = m_startTime;
                m_elapse = 0;
            }

            if (updateDisplay)
            {
                UpdateDisplay();
            }

            if (m_timerStarted)
            {
                m_timerStarted = false;
                if (sendEvent)
                {
                    SendEvent();
                }
            }
        }

        //----------------------------------------------------------------
        // MonoBehaviour

        protected virtual void OnEnable()
        {
            if (m_timerStarted)
            {
                SetTargetTime(m_targetTime);
            }
        }

        protected virtual void OnApplicationFocus(bool focus)
        {
            if (m_timerStarted)
            {
                SetTargetTime(m_targetTime);
            }
        }

        
        protected virtual void Update()
        {
            float temp = IsRealStart ? m_updateInterval : FAST_UPDATE_INTERVAL;
            /*string s = "------------------------------------------------------" + Time.unscaledDeltaTime;
            s = Time.unscaledDeltaTime > 0.02 ? s : Time.unscaledDeltaTime.ToString();
            Debug.Log(s);
            if (m_elapse>60)
            {
                Debug.LogError(m_elapse);
            }*/
            if (m_elapse < temp)
            {
                m_elapse += Time.unscaledDeltaTime;
                return;
            }

            DateTime currentTime = GetCurrentTime();
            m_leftTime = m_finalTargetTime - currentTime;
            m_elapse = 0f;
            UpdateInterval();
            UpdateDisplay();
            if (m_leftTime.Ticks <= 0)
            {
                StopTimer(true);
            }

            if (!IsRealStart)
            {
                
                if (m_leftTime.Ticks%UNIT_TOTAL_TICKS[1] >= 59*UNIT_TOTAL_TICKS[0])
                {
                    IsRealStart = true;
                }
            }
        }

        //----------------------------------------------------------------
        // 子类重载

        protected abstract void UpdateDisplay();

        //----------------------------------------------------------------
        // 内部函数

        protected virtual void UpdateDisplayUnit()
        {
            m_displayUnit = UNIT_ALL;
        }

        protected virtual void UpdateInterval()
        {
            if ((m_displayUnit & UNIT_SECOND) != 0)
            {
                m_updateInterval = FAST_UPDATE_INTERVAL;
            }
            else
            {
                m_updateInterval = SLOW_UPDATE_INTERVAL;
            }
        }


        protected void SendEvent()
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                LC.SendEvent(eventName);
            }
        }
    }
}
