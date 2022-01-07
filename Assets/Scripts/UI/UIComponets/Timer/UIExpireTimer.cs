using TMPro;
using UnityEngine;
namespace NeoOPM
{
// 到期时间计时器
    public class UIExpireTimer : UITimerBase
    {
        //----------------------------------------------------------------
        // 编辑器参数

        // 文本框
        public TextMeshProUGUI lable;

        public int ShowPlace = 2;
        // 显示的文本
        public string textExpired = "";
        public string textTimeDictionaryId = "";
        public string text_Day = "text_2";
        public string text_Hour = "text_1";
        public string text_Minute = "text_1";
        public string text_Seconds = "text_1";

        // 时间单位标识
        public int unit_Day = UNIT_DAY | UNIT_HOUR|UNIT_MINUTE|UNIT_SECOND;
        public int unit_Hour = UNIT_HOUR | UNIT_MINUTE|UNIT_SECOND;
        public int unit_Minute = UNIT_HOUR |UNIT_MINUTE | UNIT_SECOND;
        public int unit_Seconds = UNIT_HOUR |UNIT_MINUTE | UNIT_SECOND;

        //----------------------------------------------------------------
        // 成员变量
        //格式类型
        protected int m_textType = 0;
        //各个时间分量的值
        protected long[] m_timeValues = null;
        private bool changed;
        private string[] places = { "{0:D1}","{0:D2}","{0:D3}","{0:D4}"};
        
        //----------------------------------------------------------------
        // 公用函数
        public virtual void SetLabelText(string text)
        {
            if (!changed)
            {
                return;
            }

            if (!string.IsNullOrEmpty(textTimeDictionaryId) && !string.IsNullOrEmpty(text))
            {
                text = TableDic.GetDicValue(textTimeDictionaryId, text);
            }

            if (lable != null)
            {
                lable.text = text;
            }
        }
        //----------------------------------------------------------------
        // 内部函数
        protected override void UpdateDisplay()
        {
            long ticks = m_leftTime.Ticks;
            if (ticks <= 0)
            {
                changed = true;
                SetLabelText(textExpired);
            }
            else
            {
                switch (m_textType)
                {
                    case UNIT_DAY:
                        SetLabelText(FormatText(ticks, text_Day));
                        break;
                    case UNIT_HOUR:
                        SetLabelText(FormatText(ticks, text_Hour));
                        break;
                    case UNIT_MINUTE:
                        SetLabelText(FormatText(ticks, text_Minute));
                        break;
                    case UNIT_SECOND:
                        SetLabelText(FormatText(ticks, text_Seconds));
                        break;
                }
            }
        }

        protected override void UpdateDisplayUnit()
        {
            double leftSeconds = m_leftTime.TotalSeconds;
            if (leftSeconds > 86400)
            {
                m_textType = UNIT_DAY;
                SetDisplayUnit(unit_Day);
            }
            else if (leftSeconds > 3600)
            {
                m_textType = UNIT_HOUR;
                SetDisplayUnit(unit_Hour);
            }
            else if (leftSeconds > 60)
            {
                m_textType = UNIT_MINUTE;
                SetDisplayUnit(unit_Minute);
            }
            else
            {
                m_textType = UNIT_SECOND;
                SetDisplayUnit(unit_Seconds);
            }
        }

        protected virtual string FormatText(long ticks, string format)
        {
            if (m_timeValues == null)
            {
                m_timeValues = new long[UNIT_ARRAY.Length];
            }

            int valueCount = 0;
            long timeValue = 0;
            long unitTotalTicks = 0;
            long sum = 0;
            changed = false;
            for (int i = UNIT_ARRAY.Length - 1, imin = 0; i >= imin; --i)
            {
                if ((m_displayUnit & UNIT_ARRAY[i]) != 0)
                {
                    unitTotalTicks = UNIT_TOTAL_TICKS[i];
                    timeValue = (ticks - sum) / unitTotalTicks;
                    sum += timeValue * unitTotalTicks;
                    if (m_timeValues[valueCount] != timeValue)
                    {
                        m_timeValues[valueCount] = timeValue;
                        changed = true;
                    }

                    valueCount++;
                    if (timeValue == 0)
                    {
                        UpdateDisplayUnit();
                    }
                }
            }

            if (!changed)
            {
                return null;
            }

            string str = null;
            string p1 = string.Format(places[ShowPlace-1], m_timeValues[0]);
            string p2 = valueCount>=2?string.Format(places[ShowPlace-1], m_timeValues[1]):"";
            string p3 = valueCount>=3?string.Format(places[ShowPlace-1], m_timeValues[2]):"";
            string p4 = valueCount>=4?string.Format(places[ShowPlace-1], m_timeValues[3]):"";
            if (valueCount>=4)
            {
                p1 = m_timeValues[0].ToString();//天 不补零
            }
            switch (valueCount)
            {
                case 4:
                    str = TableDic.GetDicValue(format, p1, p2, p3, p4);
                    break;
                case 3:
                    str = TableDic.GetDicValue(format, p1, p2, p3);
                    break;
                case 2:
                    str = TableDic.GetDicValue(format, p1, p2);
                    break;
                case 1:
                    str = TableDic.GetDicValue(format, p1);
                    break;
            }

            return str;
        }
    }
}
