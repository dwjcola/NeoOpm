﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

namespace ProHA
{
// [ExecuteInEditMode]
    public class ListView : MonoBehaviour
    {
        public enum ESelectType
        {
            Single,
            Multiple
        }

        public enum EFlowType
        {
            Horizontal,
            Vertical,
        }

        class ItemInfo
        {
            public ListViewItem item;
            public bool isSelected;
        }

        [SerializeField] bool m_isVirtual; //是否为虚列表

        //[SerializeField] 暂时不支持点击
        ESelectType m_selectType; //选择类型
        [SerializeField]public EFlowType m_flowType; //滑动类型

        public RectOffset padding;
        //[NonSerialized]
        //public Corner m_StartCorner = Corner.UpperLeft;
        //[NonSerialized]
        //public TextAnchor m_ChildAlignment = TextAnchor.UpperCenter;
        public Vector2 m_itemSize; //item大小

        public Vector2 spacing { get { return m_itemSpace; } }
        [SerializeField] Vector2 m_itemSpace; //item间距

        public int constraintCount { get { return m_constraintCount; } }
        [SerializeField] int m_constraintCount; //固定的行数或列数，若为0则根据窗口大小自动计算行列数
        [SerializeField] private GameObject m_itemPrefab;

        public GameObject itemPrefab { get; set; }

        public bool isVirtual => m_isVirtual;
        private Action<LuaTable, LuaTable, object, int> m_luaUpdateBack;
        private LuaTable luaClass;
        private IList m_Datas;
        Action<LuaTable, LuaTable, object, int> luaUpdateBack;
        Func<LuaTable, LuaTable, object, int, PointerEventData, bool> luaOnDragBack;
        Action<LuaTable, LuaTable, object, int, PointerEventData> luaOnDragStartBack;
        Action<LuaTable, LuaTable, object, int, PointerEventData> luaOnDragEndBack;
        Action<int, ListViewItem> m_onItemRefresh; //用于刷新item UI
        Action<int, bool> m_onItemValueChanged; //item是否选中的状态发生改变时调用
        Action<ListViewItem> m_onItemClicked; //item被点击时调用
        public Action onSelectedItemCleared;

        int m_rowCount, m_columnCount; //上下滑动时，列数固定。左右滑动时，行数固定
        Vector2 m_initialSize; //listview窗口大小，用于计算行列数


        RectTransform m_rectTransform;
        ScrollRect m_scrollRect;
        GameObjectPool m_pool;

        List<ItemInfo> m_itemInfoList; //用做虚列表
        int m_itemRealCount; //用做虚列表，真实的item数量
        int m_startIndex, m_endIndex; //用做虚列表，视图中左上角item的下标，以及结束的下标

        bool m_isBoundsDirty;

        public int itemCount
        {
            get => m_isVirtual ? m_itemRealCount : m_itemList.Count;
            set
            {
                //ResetPosition();
                ClearAllSelectedItem();
                if (m_isVirtual)
                {
                    m_itemRealCount = value;
                    SetSize();
                    int oldCount = m_itemInfoList.Count;
                    if (m_itemRealCount > oldCount)
                    {
                        for (int i = oldCount; i < m_itemRealCount; i++)
                        {
                            ItemInfo info = new ItemInfo();
                            m_itemInfoList.Add(info);
                        }
                    }
                    //else
                    //{
                    //    for (int i = m_itemRealCount; i < oldCount; i++)
                    //    {
                    //        if (m_itemInfoList[i].item != null)
                    //        {
                    //            RemoveItem(m_itemInfoList[i].item);
                    //            m_itemInfoList[i].item = null;
                    //        }
                    //    }
                    //}
                }
                else
                {
                    int oldCount = m_itemList.Count;
                    if (value > oldCount)
                    {
                        for (int i = oldCount; i < value; i++)
                            AddItem();
                    }
                    else
                        RemoveItem(value, oldCount - 1);
                }

                ResetInfoList();
                Refresh();
            }
        }

        List<ListViewItem> m_itemList;

        List<ListViewItem> m_selectedItemList;
        // public List<ListViewItem> selectedItemList => m_selectedItemList;
        protected ListView()
        {
            if (padding == null)
            {
                padding = new RectOffset();
            }
        }
        void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
            m_rectTransform.pivot = Vector2.up;
            m_rectTransform.anchorMax = Vector2.up;
            m_rectTransform.anchorMin = Vector2.up;

            m_itemList = new List<ListViewItem>();
            m_selectedItemList = new List<ListViewItem>();
            m_itemInfoList = new List<ItemInfo>();
            m_scrollRect = m_rectTransform.GetComponentInParent<ScrollRect>();
            if (m_scrollRect == null)
                Debug.LogError("ListView can not find ScrollRect");
            m_scrollRect.onValueChanged.AddListener(OnScroll);
        }

        void Update()
        {
            if (m_isBoundsDirty)
                UpdateBounds();
        }

        public void Init(GameObject prefab, Action<int, ListViewItem> refresh, Action<int, bool> valueChanged,
            Action<ListViewItem> clicked)
        {
            if (prefab.GetComponent<ListViewItem>() == null)
            {
                Debug.LogError("ListView prefab dont have ListViewItem Component");
                return;
            }

            itemPrefab = prefab;

            if (m_pool != null)
                m_pool.Clear();
            m_pool = new GameObjectPool(m_rectTransform, itemPrefab);
            m_onItemRefresh = refresh;
            m_onItemValueChanged = valueChanged;
            m_onItemClicked = clicked;

            GetLayoutAttribute();
        }

        #region LUA相关

        public bool isInit = false;
        public void InitListLua(LuaTable lua, IList mDatas, string updateBack, string ondragStart = "",
            string ondragBack = "", string ondragEnd = "")
        {
            isInit = true;
            itemPrefab = m_itemPrefab;
            if (m_pool != null)
                m_pool.Clear();
            m_pool = new GameObjectPool(m_rectTransform, itemPrefab);

            luaClass = lua;
            luaUpdateBack = lua.Get<Action<LuaTable, LuaTable, object, int>>(updateBack);
            if (ondragStart != "")
            {
                luaOnDragStartBack = lua.Get<Action<LuaTable, LuaTable, object, int, PointerEventData>>(ondragStart);
            }

            if (ondragBack != "")
            {
                luaOnDragBack = lua.Get<Func<LuaTable, LuaTable, object, int, PointerEventData, bool>>(ondragBack);
            }

            if (ondragEnd != "")
            {
                luaOnDragEndBack = lua.Get<Action<LuaTable, LuaTable, object, int, PointerEventData>>(ondragEnd);
            }

            m_onItemRefresh = LuaRefresh;

            //m_onItemValueChanged = valueChanged;
            //m_onItemClicked = clicked;

            GetLayoutAttribute();
            UpdateData(mDatas);
        }

        public IList GetData()
        {
            return m_Datas;
        }

        public void RemoveOneItem(object item)
        {
            m_Datas.Remove(item);
            UpdateData(m_Datas);
        }
        public void ResetInfoList()
        {
            for (int i = 0; i < m_itemInfoList.Count; i++)
            {
                if (m_itemInfoList[i].item != null)
                {
                    RemoveItem(m_itemInfoList[i].item);
                    m_itemInfoList[i].item = null;
                }
            }
        }
        public void UpdateData(IList mDatas, bool isReposition = true)
        {
            m_Datas = mDatas;
            itemCount = m_Datas.Count;
            if (isReposition) ResetPosition();
            //for (int i = 0; i < m_Datas.Count; i++)
            //{
            //    Logger.Error("---0---" + ((LuaTable)m_Datas[i]).Get<int>("TaskId"));
            //}
        }
        public void UpdateDataLua(LuaTable lua,IList mDatas, bool isReposition = true)
        {
            luaClass = lua;
            UpdateData(mDatas, isReposition);
        }
        public void InsertNewData(int index,object item )
        {
            //for (int i = 0; i < m_Datas.Count; i++)
            //{
            //    Logger.Error("---0101---" + ((LuaTable)m_Datas[i]).Get<int>("TaskId"));
            //}
            for (int i=0;i<m_itemList.Count;i++)
            {
                m_itemList[i].ResetSizeDelta();
            }
            UpdateBounds();
            //for (int i = 0; i < m_Datas.Count; i++)
            //{
            //    Logger.Error("---1---" + ((LuaTable)m_Datas[i]).Get<int>("TaskId"));
            //}
            m_Datas.Insert(index, item);
           
            //for(int i=0;i< m_Datas.Count; i++)
            //{
            //    Logger.Error("---2---" + ((LuaTable)m_Datas[i]).Get<int>("TaskId"));
            //}

            itemCount = m_Datas.Count;
        }

        private void LuaRefresh(int index, ListViewItem item)
        {
            UIMonoItem itemCtrl = item.GetComponent<UIMonoItem>();
            LuaTable itemLua = itemCtrl.SelfInit();
            item.CurrentIndex = index;
            luaUpdateBack?.Invoke(luaClass, itemLua, m_Datas[index], index);
        }

        private bool LuaOnDrag(ListViewItem item, int index, PointerEventData e)
        {
            if (luaOnDragBack != null)
            {
                UIMonoItem itemCtrl = item.GetComponent<UIMonoItem>();
                LuaTable itemLua = itemCtrl.SelfInit();
                return luaOnDragBack(luaClass, itemLua, m_Datas[index], index, e);
            }

            return false;
        }

        private void LuaOnDragStart(ListViewItem item, int index, PointerEventData e)
        {
            if (luaOnDragStartBack != null)
            {
                UIMonoItem itemCtrl = item.GetComponent<UIMonoItem>();
                LuaTable itemLua = itemCtrl.SelfInit();
                luaOnDragStartBack(luaClass, itemLua, m_Datas[index], index, e);
            }
        }

        private void LuaOnDragEnd(ListViewItem item, int index, PointerEventData e)
        {
            if (luaOnDragEndBack != null)
            {
                UIMonoItem itemCtrl = item.GetComponent<UIMonoItem>();
                LuaTable itemLua = itemCtrl.SelfInit();
                luaOnDragEndBack(luaClass, itemLua, m_Datas[index], index, e);
            }
        }

        public void SetPosition(float p)
        {
            if (m_flowType == EFlowType.Horizontal)
            {
                m_scrollRect.horizontalNormalizedPosition = p;
            }
            else
            {
                m_scrollRect.verticalNormalizedPosition = p;
            }
        }

        #endregion

        public void Refresh()
        {
            if (m_isVirtual)
            {
                RenderVirtualItem(true);
            }
            else
            {
                for (int i = 0, count = m_itemList.Count; i < count; i++)
                {
                    ListViewItem item = m_itemList[i];
                    m_onItemRefresh?.Invoke(i, item);
                }
            }
        }

        public void ClearAllSelectedItem()
        {
            if (m_isVirtual)
            {
                for (int i = m_startIndex; i < itemCount; i++)
                    m_itemInfoList[i].isSelected = false;
            }
            else
            {
                for (int i = 0, count = m_selectedItemList.Count; i < count; i++)
                {
                    m_selectedItemList[i].isSelected = false;
                    // OnValueChanged(item); 由于原始数据可能已经变化了，所以使用onSelectedItemCleared代替
                }

                m_selectedItemList.Clear();
            }

            onSelectedItemCleared?.Invoke();
        }

        public ListViewItem AddItem()
        {
            if (itemPrefab == null) return null;

            GameObject go = m_pool.Get();
            go.transform.SetParent(m_rectTransform);
            go.transform.localScale = Vector3.one;
            ListViewItem item = go.GetOrAddComponent<ListViewItem>();
            item.sr = m_scrollRect;
            m_itemList.Add(item);
            item.Init(m_selectType, OnValueChanged, m_onItemClicked, LuaOnDragStart, LuaOnDrag, LuaOnDragEnd);
            go.SetActive(true);
            SetBoundsDirty();
            return item;
        }

        public void RemoveItem(ListViewItem item)
        {
            if (item.isSelected)
            {
                item.isSelected = false;
                OnValueChanged(item);
            }

            m_pool.Put(item.gameObject);
            m_itemList.Remove(item);
            SetBoundsDirty();
        }

        public void RemoveItem(int index)
        {
            if (index < 0 || index >= m_itemList.Count) return;
            RemoveItem(m_itemList[index]);
        }
        public void RemoveItemWithEff(int index,bool isReset=false)
        {
            ListViewItem item = m_itemList[index];
            RectTransform rect = item.GetComponent<RectTransform>();
            Tweener tweener = rect.DOSizeDelta(new Vector2(rect.sizeDelta.x, 0), 0.4f);
            
            CanvasGroup canvas = item.GetComponent<CanvasGroup>();
            item.AddTweener(tweener);
            item.AddTweener(canvas.DOFade(0, 0.4f));
            tweener.onComplete = () =>
            {
                //Logger.Error("remove---------------" + index + "____" +((LuaTable)m_Datas[index]).Get<int>("TaskId"));
                m_Datas.RemoveAt(index);
                RemoveItem(index);
                //m_isBoundsDirty = false;
            };
            
        }

        public void RemoveItem(int beginIndex, int endIndex)
        {
            if (beginIndex > endIndex) return;
            for (int i = beginIndex; i <= endIndex; i++)
                RemoveItem(beginIndex);
        }

        public void RemoveAllItem()
        {
            RemoveItem(0, itemCount - 1);
        }

        void SetBoundsDirty()
        {
            m_isBoundsDirty = true;
        }

        void UpdateBounds()
        {
            if (m_isVirtual)
                return;
            SetSize();
            for (int i = 0, count = itemCount; i < count; i++)
                m_itemList[i].transform.localPosition = CalculatePosition(i);

            m_isBoundsDirty = false;
        }

        /// <summary>
        /// 设定都是左上对齐的，原点在左上角
        /// </summary>
        /// <param name="idx"></param>
        public void Move2SelectItem(int idx)
        {
            if (idx < 0 || idx >= itemCount)
                return;
            var s_pos = CalculatePosition(idx);
            float s_MoveDis = m_flowType == EFlowType.Vertical ? Math.Abs(s_pos.y + m_itemSize.y/2)-padding.top :Math.Abs(s_pos.x - m_itemSize.x /2)-padding.left;
            float length = GetContentLength();
            float maxMoveDis = length - (m_flowType == EFlowType.Vertical ? m_initialSize.y : m_initialSize.x);
            m_rectTransform.localPosition = (m_flowType == EFlowType.Vertical ? Vector3.up : Vector3.left) * Math.Min(s_MoveDis, maxMoveDis);
        }
        void SetSize()
        {
            m_rectTransform.sizeDelta = m_flowType == EFlowType.Horizontal
                ? new Vector2(GetContentLength(), m_initialSize.y)
                : new Vector2(m_initialSize.x, GetContentLength());
        }

        void ResetPosition()
        {
            m_startIndex = 0;
            m_endIndex = 0;
            m_rectTransform.localPosition = Vector3.zero;
        }


        float GetContentLength()
        {
            if (itemCount == 0) return 0;
            //计算所有item需要的长度
            if (m_flowType == EFlowType.Horizontal)
            {
                int columnCount = Mathf.CeilToInt((float) itemCount / m_rowCount);
                return m_itemSize.x * columnCount + m_itemSpace.x * (columnCount - 1)+padding.horizontal;
            }
            else
            {
                int rowCount = Mathf.CeilToInt((float) itemCount / m_columnCount);
                return m_itemSize.y * rowCount + m_itemSpace.y * (rowCount - 1)+padding.vertical;
            }
        }

        void OnScroll(Vector2 position)
        {
            if (m_isVirtual)
                RenderVirtualItem(false);
        }

        void RenderVirtualItem(bool isForceRender)
        {
            ScrollAndRender(isForceRender);
        }

        //isForceRender:是否强制更新所有item
        void ScrollAndRender(bool isForceRender)
        {
            int oldStartIndex = m_startIndex, oldEndIndex = m_endIndex;
            if (m_flowType == EFlowType.Horizontal)
            {
                float currentX = m_rectTransform.localPosition.x; // <0
                currentX -= padding.left;
                m_startIndex = GetCurrentIndex(currentX);
                float endX = currentX - m_initialSize.x - m_itemSize.x - m_itemSpace.x;
                m_endIndex = GetCurrentIndex(endX);
            }
            else
            {
                float currentY = m_rectTransform.localPosition.y; // >0
                //上下滑动，根据listview的y值计算当前视图中第一个item的下标
                currentY -= padding.top;
                m_startIndex = GetCurrentIndex(currentY);
                //根据视图高度，item高度，间距的y，计算出结束行的下标
                float endY = currentY + m_initialSize.y + m_itemSize.y + m_itemSpace.y;
                m_endIndex = GetCurrentIndex(endY);
            }

            if (!isForceRender && oldStartIndex == m_startIndex && oldEndIndex == m_endIndex)
                return;

            //渲染当前视图内需要显示的item
            for (int i = m_startIndex; i < itemCount && i < m_endIndex; i++)
            {
                bool needRender = false; //是否需要刷新item ui
                ItemInfo info = m_itemInfoList[i];

                if (info.item == null)
                {
                    int j, jEnd;
                    if ((oldStartIndex < m_startIndex || oldEndIndex < m_endIndex))
                    {
                        //说明是往下或者往右滚动，即要从前面找复用的Item
                        j = 0;
                        jEnd = m_startIndex;
                    }
                    else
                    {
                        j = m_endIndex;
                        jEnd = itemCount;
                    }
                    if (jEnd <= 0) //这里 如果 jEnd<=0 则必定是由后返顶。m_startIndex为0,此时往前是找不到可复用的item的，故往后找
                    {
                        j = m_endIndex;
                        jEnd = itemCount;
                    }
                    for (; j < jEnd; j++)
                    {
                        if (m_itemInfoList[j].item != null)
                        {
                            info.item = m_itemInfoList[j].item;
                            m_itemInfoList[j].item = null;
                            needRender = true;
                            break;
                        }
                    }
                }

                //前后找不到的话，添加新的item
                if (info.item == null)
                {
                    info.item = AddItem();
                    needRender = true;
                }

                //更新位置，是否选中状态，以及数据
                if (isForceRender || needRender)
                {
                    info.item.transform.localPosition = CalculatePosition(i);
                    info.item.isSelected = info.isSelected;
                    m_onItemRefresh?.Invoke(i, info.item);
                }
            }
        }

        //根据listview的位置，计算该位置的行或列的第一个item的下标
        int GetCurrentIndex(float position)
        {
            position = Math.Abs(position);
            int moveAxis = (int)m_flowType;//移动轴向
            if (position < m_itemSize[moveAxis])
                return 0;
            position -= m_itemSize[moveAxis];
            return (Mathf.FloorToInt(position / (m_itemSize[moveAxis] + m_itemSpace[moveAxis])) + 1) * (moveAxis ==0? m_rowCount : m_columnCount);
            //if (m_flowType == EFlowType.Horizontal)
            //{
            //    position = -position;
            //    if (position < m_itemSize.x) return 0;

            //    position -= m_itemSize.x;
            //    return (Mathf.FloorToInt(position / (m_itemSize.x + m_itemSpace.x)) + 1) * m_rowCount;
            //}
            //else
            //{
            //    if (position < m_itemSize.y) return 0;
            //    position -= m_itemSize.y;
            //    return (Mathf.FloorToInt(position / (m_itemSize.y + m_itemSpace.y)) + 1) * m_columnCount;
            //}
        }

        public void SetVirtual()
        {
            if (!m_isVirtual)
            {
                int oldCount = itemCount;
                m_itemInfoList.Clear();
                RemoveAllItem();
                m_isVirtual = true;

                if (oldCount != 0)
                    itemCount = oldCount;
            }
        }

        void GetLayoutAttribute()
        {
            if(m_itemSize==Vector2.zero)
                m_itemSize = itemPrefab.GetComponent<RectTransform>().rect.size;
            m_initialSize = m_rectTransform.parent.GetComponent<RectTransform>().rect.size; //Viewport Size 

            //计算行或列
            if (m_flowType == EFlowType.Horizontal)
            {
                m_rowCount = m_constraintCount;
                if (m_rowCount <= 0)
                    m_rowCount = Mathf.FloorToInt((m_initialSize.y + m_itemSpace.y-padding.top) / (m_itemSize.y + m_itemSpace.y));
                if (m_rowCount == 0)
                    m_rowCount = 1;
            }
            else
            {
                m_columnCount = m_constraintCount;
                if (m_columnCount <= 0)
                    m_columnCount =
                        Mathf.FloorToInt((m_initialSize.x + m_itemSpace.x-padding.left) / (m_itemSize.x + m_itemSpace.x));
                if (m_columnCount == 0)
                    m_columnCount = 1;
            }
        }

        void OnValueChanged(ListViewItem item)
        {
            if (m_isVirtual)
            {
                if (item.isSelected)
                {
                    if (m_selectType == ESelectType.Single)
                    {
                        for (int i = 0; i < itemCount; i++)
                        {
                            //找到对应项，设置为选中
                            if (m_itemInfoList[i].item == item)
                            {
                                m_itemInfoList[i].isSelected = true;
                                m_onItemValueChanged?.Invoke(i, true);
                                continue;
                            }

                            //取消之前的选中状态
                            if (m_itemInfoList[i].isSelected)
                            {
                                m_itemInfoList[i].isSelected = false;
                                if (m_itemInfoList[i].item != null)
                                    m_itemInfoList[i].item.isSelected = false;
                                m_onItemValueChanged?.Invoke(i, false);
                            }
                        }
                    }
                    else
                    {
                        //找到对应项，设置为选中
                        for (int i = m_startIndex; i < itemCount; i++)
                        {
                            if (m_itemInfoList[i].item == item)
                            {
                                m_itemInfoList[i].isSelected = true;
                                m_onItemValueChanged?.Invoke(i, true);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    //找到对应项，设置为未选中
                    for (int i = m_startIndex; i < itemCount; i++)
                    {
                        if (m_itemInfoList[i].item == item)
                        {
                            m_itemInfoList[i].isSelected = false;
                            m_onItemValueChanged?.Invoke(i, false);
                            break;
                        }
                    }
                }
            }
            else
            {
                if (item.isSelected)
                {
                    if (m_selectType == ESelectType.Single)
                    {
                        if (m_selectedItemList.Count > 0)
                        {
                            //取消之前的选中状态
                            m_selectedItemList[0].isSelected = false;
                            int lastSelectedIndex = m_itemList.IndexOf(m_selectedItemList[0]);
                            m_onItemValueChanged?.Invoke(lastSelectedIndex, false);
                            m_selectedItemList.Clear();
                        }

                        m_selectedItemList.Add(item);
                    }
                    else
                        m_selectedItemList.Add(item);
                }
                else
                {
                    m_selectedItemList.Remove(item);
                }

                int index = m_itemList.IndexOf(item);
                m_onItemValueChanged?.Invoke(index, item.isSelected);
            }
        }

        public void Clear()
        {
            m_pool?.Clear();

            m_itemList?.Clear();
            m_selectedItemList?.Clear();
            m_itemInfoList?.Clear();
            isInit = false;
        }
        void OnDestroy()
        {
            // foreach (var item in m_itemList)
            // item.RemoveOnClickedHandle(OnValueChanged);
           Clear();
        }
        //根据item下标计算所在位置
        Vector2 CalculatePosition(int index)
        {
            int row, column;
            if (m_flowType == EFlowType.Horizontal)
            {
                row = index % m_rowCount;
                column = index / m_rowCount;
            }
            else
            {
                row = index / m_columnCount;
                column = index % m_columnCount;
            }

            float x = column * (m_itemSize.x + m_itemSpace.x) + m_itemSize.x/2+padding.left;
            float y = row * (m_itemSize.y + m_itemSpace.y) + m_itemSize.y/2 +padding.top;

            return new Vector2(x, -y);
        }

        [ContextMenu("ResetPosition")]
        public void ResetUIPosition()
        {
            UpdateData(GetData());
        }
    }
}