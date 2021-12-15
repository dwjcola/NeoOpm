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
        //[SerializeField] 暂时不公开选中操作
        GameObject m_selectedGameObject;
        public int CurrentIndex { get; set; }
        public int id { get; private set; }
        public ListView.ESelectType selectType { get; private set; }
        Action<ListViewItem> m_onValueChanged;
        Action<ListViewItem> m_onClicked; //适用于只在Item被单击时做操作的情况
        private Func<ListViewItem, int, PointerEventData, bool> m_onDrag;
        private Action<ListViewItem, int, PointerEventData> m_OnDragStart;
        private Action<ListViewItem, int, PointerEventData> m_OnDragEnd;
        public RectTransform m_rectTransform;
        Button m_button;
        public ScrollRect sr;

        bool m_isSelected;
        Vector2 m_originSize;
        List<Tweener> m_tweenerList = new List<Tweener>();
        public bool isSelected
        {
            get => m_isSelected;
            set
            {
                if (m_isSelected != value)
                {
                    m_isSelected = value;
                    UpdateSelectedUI();
                }
            }
        }

        void Awake()
        {
            id = GetInstanceID();
            m_button = GetComponent<Button>();
            isSelected = false;
            if (m_button)
            {
                m_button.onClick.AddListener(OnClicked);
            }
            

            m_rectTransform = GetComponent<RectTransform>();
            m_rectTransform.anchorMin = Vector2.up;
            m_rectTransform.anchorMax = Vector2.up;
            m_rectTransform.pivot = new Vector2(0.5f, 0.5f);
            //sr = GetComponentInParent<ScrollRect>();
            m_originSize = m_rectTransform.sizeDelta;
        }

        public void OnBeginDrag(PointerEventData data)
        {
            //if (m_OnDragStart!=null)
            //{
            /*Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectTransform, data.position, data.enterEventCamera, out pos);

            oldPos = pos;*/
            m_OnDragStart?.Invoke(this, CurrentIndex, data);
            //}

            sr.OnBeginDrag(data);

        }

        //private Vector3 oldPos;
        //private GameObject go;
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

            /*Vector3 pos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectTransform, data.position, data.enterEventCamera, out pos);
            if (Mathf.Abs(pos.y-oldPos.y)>m_rectTransform.rect.height*0.5f)
            {
                Debug.LogError("22222222222");
                if (go==null)
                {
                    go = GameObject.Instantiate(this.gameObject);
                    go.transform.parent = m_rectTransform.parent;
                }
                go.GetComponent<RectTransform>().position = pos;
                sr.OnEndDrag(data);
            }
            else
            {
                sr.OnDrag(data);
            }*/
        }

        public void OnEndDrag(PointerEventData data)
        {
            m_OnDragEnd?.Invoke(this, CurrentIndex, data);
            sr.OnEndDrag(data);
        }

        public void Init(ListView.ESelectType type, Action<ListViewItem> onValueChanged, Action<ListViewItem> onClicked,
            Action<ListViewItem, int, PointerEventData> luaOnDragStart,
            Func<ListViewItem, int, PointerEventData, bool> luaOnDrag,
            Action<ListViewItem, int, PointerEventData> luaOnDragEnd)
        {
            selectType = type;
            m_onValueChanged = onValueChanged;
            m_onClicked = onClicked;
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


        void OnClicked()
        {
            bool isValueChange = false;
            if (selectType == ListView.ESelectType.Single)
            {
                if (!isSelected)
                    isValueChange = true;
                isSelected = true;
            }
            else
            {
                isValueChange = true;
                isSelected = !isSelected;
            }

            if (isValueChange)
                m_onValueChanged?.Invoke(this);
            m_onClicked?.Invoke(this);
        }

        void ClearSelected()
        {
            isSelected = false;
        }

        protected virtual void UpdateSelectedUI()
        {
            if (m_selectedGameObject != null)
            {
                m_selectedGameObject.SetActive(isSelected);
            }
        }

        public void ResetSizeDelta()
        {
           
            foreach (var tw in this.m_tweenerList)
            {
                tw.Complete();
                //tw.Kill();
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