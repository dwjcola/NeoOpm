using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameFramework.UI
{
    public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);
        public delegate void PointerEventDataDelegate(GameObject go, PointerEventData value);
        public VoidDelegate onClick
        
        ;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidDelegate onPress;
        public VoidDelegate onDoubleClick;
        public VoidDelegate OnDragBegin;
        public VoidDelegate OnDragFuc;
        public VoidDelegate OnDragEnd;

        public PointerEventDataDelegate onDragOPM;
        public PointerEventDataDelegate onBeginDragOPM;
        public PointerEventDataDelegate onEndDragOPM;


        private bool m_Interactable = true;

        private bool m_isPointDown = false;
        private float m_CurrDownTime = 0f;
        private int m_ClickCount = 0;
        private bool m_isPress = false;
        //上一次触发点击事件的时间
        private static float m_lastClickTime = 0f;
        private const float m_twoClickTime = 0.5f;

        private ScrollRect _scrollrect;

        public bool Interactable { get => m_Interactable; set => m_Interactable = value; }
        private void Start()
        {
            ScrollRect scrollrect = GetComponentInParent<ScrollRect>();
            if (scrollrect != null)
            {
                Transform rect = scrollrect.content;
                if (rect != null)
                {
                    if (transform.IsChildOf(rect.parent))
                    {
                        _scrollrect = scrollrect;
                    }
                }
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (_scrollrect != null)
            {
                _scrollrect.OnBeginDrag(eventData);
            }
            OnDragBegin?.Invoke(gameObject);
        }
        private bool m_isDragging = false;
        public override void OnDrag(PointerEventData eventData)
        {
            m_isDragging = true;
            if (_scrollrect != null)
            {
                _scrollrect.OnDrag(eventData);
            }

            OnDragFuc?.Invoke(gameObject);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            m_isDragging = false;
            if (_scrollrect != null)
            {
                _scrollrect.OnEndDrag(eventData);
            }
            OnDragEnd?.Invoke(gameObject);
        }


        public void OnDragOPM(PointerEventData eventData)
        {
            if (m_Interactable && onDragOPM != null) onDragOPM(gameObject, eventData);
        }
        public void OnBeginDragOPM(PointerEventData eventData)
        {
            if (m_Interactable && onBeginDragOPM != null) onBeginDragOPM(gameObject, eventData);
        }
        public void OnEndDragOPM(PointerEventData eventData)
        {
            if (m_Interactable && onEndDragOPM != null) onEndDragOPM(gameObject, eventData);
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (_scrollrect != null)
            {
                _scrollrect.OnInitializePotentialDrag(eventData);
            }
        }
        public override void OnScroll(PointerEventData eventData)
        {
            if (_scrollrect != null)
            {
                _scrollrect.OnScroll(eventData);
            }
        }

        private const float PRESS_TIME = 0.5f;
        private const float DOUBLE_CLICK_TIME = 0.2f;

      

        private void Update()
        {
            if (m_isPointDown)
            {
                if (Time.unscaledTime - m_CurrDownTime >= PRESS_TIME)
                {
                    m_isPress = true;
                    m_CurrDownTime = 0;
                    m_isPointDown = false;
                    onPress?.Invoke(gameObject);
                }
            }

            if (m_ClickCount > 0)
            {
                if (Time.unscaledTime - m_CurrDownTime >= DOUBLE_CLICK_TIME)
                {
                    m_ClickCount = 0;
                }

                if (m_ClickCount >= 2)
                {
                    onDoubleClick?.Invoke(gameObject);
                    m_ClickCount = 0;
                }
            }
        }

        static public EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (m_isDragging)
            {
                m_isDragging = false;
                return;
            }
            if (m_ClickCount < 2)
            {
                if (Time.unscaledTime - m_lastClickTime >= m_twoClickTime)
                {
                    onClick?.Invoke(gameObject);
                    m_lastClickTime = Time.unscaledTime;
                }
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            m_isPointDown = true;
            m_isPress = false;
            m_CurrDownTime = Time.unscaledTime;

            if (onDown != null) onDown(gameObject);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            m_isPointDown = false;
            if (!m_isPress)
            {
                m_ClickCount++;
            }
            if (onUp != null) onUp(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }
        public void UnregisterAllEvents()
        {
            onClick = null;
            onDown = null;
            onEnter = null;
            onExit = null;
            onUp = null;
            onSelect = null;
            onUpdateSelect = null;
            onPress = null;
            onDoubleClick = null;
            OnDragBegin = null;
            OnDragFuc = null;
            OnDragEnd = null;

            if (_scrollrect)
            {

                if (_scrollrect.onValueChanged != null)
                {
                    _scrollrect.onValueChanged.RemoveAllListeners();
                }
            }
            _scrollrect = null;
        }
        public void UnloadFunction()
        {
            UnregisterAllEvents();
        }

        private void OnDestroy()
        {
            UnloadFunction();
        }

    }
}