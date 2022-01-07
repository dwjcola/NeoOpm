using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace NeoOPM
{
    public class CToggleGroup : ToggleGroup
    {
        public delegate void TabSelectEvent(int index);

        public TabSelectEvent tabSelect;
        public bool SelectDefault = true;
        public bool DoubleHit = false;
        public float Scale = 1f;
        private bool init;
        private int CurrentIndex = -1;
        private List<Toggle> m_CatchList = new List<Toggle>();
        //protected override void Start()
        //{
        //    base.Start();
        //    AddEvent();
        //}

        public void AddEvent()
        {
            if (init)
            {
                ReSetList();
                return;
            }

            m_CatchList.Clear();
            for (int i = 0; i < m_Toggles.Count; i++)
            {
                m_CatchList.Add(m_Toggles[i]);
                int index = i;
                m_Toggles[i].onValueChanged.AddListener((b) =>
                {
                    if (index >= m_Toggles.Count)
                    {
                        return;
                    }
                    PlayEffect(index, b);
                    if (!DoubleHit && CurrentIndex == index && b)
                    {
                        return;
                    }

                    CurrentIndex = index;
                    ToggleEvent(b, index);
                });
                if (m_Toggles[i].isOn && SelectDefault)
                {
                    ToggleEvent(true, i);
                }
            }

            init = true;
        }

        private void ReSetList()
        {
            if (m_CatchList.Count > 0)
            {
                m_Toggles.Clear();
                for (int i = 0; i < m_CatchList.Count; i++)
                {
                    m_Toggles.Add(m_CatchList[i]);
                }
            }
        }

        public void SelectTab(int index)
        {
            //Debug.LogError(index+"-------"+m_Toggles.Count);
            if (index < m_Toggles.Count && index >= 0)
            {
                m_Toggles[index].isOn = true;
                CurrentIndex = index;
                PlayEffect(index, true);
            }
        }

        public void ResetSelectStatus()
        {
            bool lastB = allowSwitchOff;
            allowSwitchOff = true;
            for (int i = 0; i < m_CatchList.Count; i++)
            {
                m_CatchList[i].isOn = false;
            }
            allowSwitchOff = lastB;
            CurrentIndex = -1;
        }
        /// <summary>
        /// 将当前选中的tab 重置
        /// </summary>
        /// <param name="index"></param>
        public void ResetSelectedTab(int index, bool isOn = false)
        {
            m_Toggles[index].isOn = isOn;
            //CurrentIndex = -1;
            //m_Toggles[index].transform.localScale = Vector3.one;
            //PlayEffect(index, false);
        }

        public void ShowOrHideTab(int index, bool state)
        {
            ReSetList();
            m_Toggles[index].gameObject.SetActive(state);
            ReSetList();
        }


        private void ToggleEvent(bool b, int index)
        {
            if (b)
            {
                if (tabSelect != null && index >= 0)
                {
                    tabSelect(index);
                }
            }
            else if (DoubleHit)//
            {
                if (tabSelect != null && index >= 0)
                {
                    tabSelect(index);
                }
            }
            m_Toggles[index].transform.localScale = b ? new Vector3(Scale, Scale, 1f) : Vector3.one;

        }

        public void PlayEffect(int index, bool b)
        {
            CToggle t = m_Toggles[index] as CToggle;
            if (t != null)
            {
                t.StopAllCoroutines();
                if (t.UnSelectCG)
                {
                    t.StartCoroutine(t.UnSelectCG.FadeToAlpha(b ? 0f : 1f, t.toggleTransition == Toggle.ToggleTransition.None ? 0f : 0.1f));
                }
                if (t.SelectCG != null)
                {
                    t.StartCoroutine(t.SelectCG.FadeToAlpha(b ? 1f : 0f, t.toggleTransition == Toggle.ToggleTransition.None ? 0f : 0.1f));
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopAllCoroutines();
        }

        public int CurSelectIndex()
        {
            if ((CurrentIndex < 0 || CurrentIndex > m_CatchList.Count) && m_CatchList.Count > 0)
            {
                CurrentIndex = 0;
                SelectTab(0);
            }
            if (m_CatchList.Count <= 0)
            {
                CurrentIndex = -1;
            }
            return CurrentIndex;
        }

        public string CurSelectName()
        {
            if ((CurrentIndex < 0 || CurrentIndex > m_CatchList.Count) && m_CatchList.Count > 0)
            {
                CurrentIndex = 0;
            }
            if (m_CatchList.Count <= 0)
            {
                return "";
            }
            return m_CatchList[CurrentIndex].gameObject.name;
        }
        public void ClearAll()
        {
            init = false;
            if (m_CatchList != null)
            {
                for (int i = 0; i < m_CatchList.Count; i++)
                {
                    UnregisterToggle(m_CatchList[i]);
                }
            }
            m_CatchList.Clear();
        }

        public int GetIsOnIndex()
        {
            for (int i = 0; i < m_CatchList.Count; i++)
            {
                if (m_CatchList[i].isOn)
                {
                    return i;
                }
            }

            return 0;
        }

        public int GetIsActiveMinIndex()
        {
            for (int i = 0; i < m_CatchList.Count; i++)
            {
                if (m_CatchList[i].isOn == false && m_CatchList[i].IsActive())
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetCurrentIndex()
        {
            return CurrentIndex;
        }
    }
}