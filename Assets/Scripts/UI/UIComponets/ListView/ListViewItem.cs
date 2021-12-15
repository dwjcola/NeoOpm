using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

namespace ProHA
{
    public class ListViewItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public int CurrentIndex { get; set; }
        public int id { get; private set; }
        private Func<ListViewItem, int, PointerEventData, bool> m_onDrag;
        private Action<ListViewItem, int, PointerEventData> m_OnDragStart;
        private Action<ListViewItem, int, PointerEventData> m_OnDragEnd;
        private RectTransform m_rectTransform;
        [NonSerialized]
        public ScrollRect sr;
        Vector2 m_originSize;
        List<Tweener> m_tweenerList = new List<Tweener>();


        void Awake()
        {
            id = GetInstanceID();
     
            m_rectTransform = GetComponent<RectTransform>();
            m_rectTransform.anchorMin = Vector2.up;
            m_rectTransform.anchorMax = Vector2.up;
            m_rectTransform.pivot = new Vector2(0.5f, 0.5f);
            m_originSize = m_rectTransform.sizeDelta;
        }

        public void OnBeginDrag(PointerEventData data)
        {
            m_OnDragStart?.Invoke(this, CurrentIndex, data);
            sr.OnBeginDrag(data);
        }
        public void OnDrag(PointerEventData data)
        {
            bool flag = false;
            if (m_onDrag != null)
            {
                flag = m_onDrag(this, CurrentIndex, data);
            }

            if (!flag)
            {
                sr.OnDrag(data);
            }
            else
            {
                sr.OnEndDrag(data);
            }
        }

        public void OnEndDrag(PointerEventData data)
        {
            m_OnDragEnd?.Invoke(this, CurrentIndex, data);
            sr.OnEndDrag(data);
        }

        public void Init( 
            Action<ListViewItem, int, PointerEventData> luaOnDragStart,
            Func<ListViewItem, int, PointerEventData, bool> luaOnDrag,
            Action<ListViewItem, int, PointerEventData> luaOnDragEnd)
        {
            m_onDrag = luaOnDrag;
            m_OnDragStart = luaOnDragStart;
            m_OnDragEnd = luaOnDragEnd;
        }
        public void DragCallBack(
            Action<ListViewItem, int, PointerEventData> luaOnDragStart,
            Func<ListViewItem, int, PointerEventData, bool> luaOnDrag,
            Action<ListViewItem, int, PointerEventData> luaOnDragEnd)
        {
            m_onDrag = luaOnDrag;
            m_OnDragStart = luaOnDragStart;
            m_OnDragEnd = luaOnDragEnd;
        }
        public void ResetSizeDelta()
        {
            foreach (var tw in this.m_tweenerList)
            {
                tw.Complete();
            }
            if (m_tweenerList.Count>0)
            {
                m_rectTransform.sizeDelta = m_originSize;
                CanvasGroup ca = GetComponent<CanvasGroup>();
                ca.alpha = 1;
                this.m_tweenerList.Clear();
            }
           
        }
        public void AddTweener(Tweener tw)
        {
            m_tweenerList.Add(tw);
        }
    }
}