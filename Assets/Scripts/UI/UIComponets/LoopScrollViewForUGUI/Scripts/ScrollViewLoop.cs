using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// explain：循环列表组件 UGUI版
/// author：王云飞
/// email:1172906928@qq.com
/// </summary>
namespace lufeigame
{
    public  class ScrollViewLoop : MonoBehaviour
    {
        public bool IsPreview = false;
        public RectTransform prefabItem;
        protected ScrollViewGrid grid;
        public int totalCount = 99;
        protected RectTransform viewportRect;
        protected RectTransform contentRect;

        protected Queue<ScrollViewBaseItem> items = new Queue<ScrollViewBaseItem>();
        protected Queue<ScrollViewBaseItem> recoveryItems = new Queue<ScrollViewBaseItem>();
        [HideInInspector]
        public ScrollRect scrollRect;
        protected int frameInitCount = 1;
        protected bool isEnableFrameInit = true;

        /// <summary>
        /// 是否正在分帧实例化中
        /// </summary>
        protected bool isFrameIniting = false;
        protected bool isInited = false;

        protected int curEndIndex = 0;
        protected int curBeginIndex = 0;

        public void UpdateCurDatas()
        {
            foreach (var item in items)
            {
                if (item.gameObject.activeSelf)
                {
                    if (item.dataIndex>= curBeginIndex && item.dataIndex <= curEndIndex)
                    {
                        item.UpdateData(item.dataIndex);
                    }
                }
         
            }
        }



        public virtual void ReLoad()
        {
            isFrameIniting = false;
            isInited = false;
        }
        /// <summary>
        /// 默认跳转到
        /// </summary>
        /// <param name="index"></param>
        public virtual void ReloadTargetIndex(int targetIndex)
        {
            isFrameIniting = false;
            isInited = false;
        }
        public virtual void OnScrollViewChange()
        {

        }
        public virtual void SetCount(int count)
        {
            if (IsPreview)
            {
                Debug.LogError("预览状态下 禁止动态设置数量，请取消掉预览。 ");
            }
            totalCount = count;
        }
        public virtual void SetItemPrefab(RectTransform itemPrefab)
        {
            prefabItem = itemPrefab;
        }
        public virtual void SetPadding(float paddingLeft, float paddingRight, float paddingTop, float paddingBottom)
        {
            grid.padding_left = paddingLeft;
            grid.padding_right = paddingRight;
            grid.padding_top = paddingTop;
            grid.padding_bottom = paddingBottom;
        }
        public virtual void SetCellSieze(Vector2 size)
        {
            grid.cellSize = size;
        }
        public virtual void SetSpacing(Vector2 spacing)
        {
            grid.spacingX = spacing.x;
            grid.spacingY = spacing.y;
        }
        public virtual void SetColumn(int column)
        {
            grid.column = column;
        }
        public virtual void SetRow(int row)
        {
            grid.row = row;
        }
        public virtual void Jump(int index)
        {
  
        }

        private void Awake()
        {
            var scrollRect = this.GetComponent<ScrollRect>();
            viewportRect = scrollRect.viewport;
            contentRect = scrollRect.content;
            grid = transform.GetComponentInChildren<ScrollViewGrid>();
            if (grid == null)
            {
                Debug.LogError("未找到组件ScrollViewGrid");
            }
            this.scrollRect = scrollRect;
            scrollRect.onValueChanged.AddListener((v) => {
                OnScrollViewChange();
            });
        }
        public virtual  void Start()
        {
            if (prefabItem != null)
            {
                prefabItem.gameObject.SetActive(false);
            }
        }
        private void OnDestroy()
        {
            if(scrollRect.onValueChanged != null)
            { 
                scrollRect.onValueChanged.RemoveAllListeners();
                scrollRect.onValueChanged = null;
            }
        }
        /// <summary>
        /// 清除所有Item 建议在切换显示不同Item时再调用。
        /// 不建议 非切换item时调用清除操作。会把缓存中的对象清除掉，增加下次实例化性能消耗。
        /// </summary>
        public virtual void DestroyAllItems()
        {
            if (items.Count > 0)
            {
               var tor =  items.GetEnumerator();
                while (tor.MoveNext())
                {
                    GameObject.Destroy(tor.Current.gameObject);
                }
                items.Clear();
            }

            if (recoveryItems.Count > 0)
            {
                var tor = recoveryItems.GetEnumerator();
                while (tor.MoveNext())
                {
                    GameObject.Destroy(tor.Current.gameObject);
                }
                recoveryItems.Clear();
            }
        }
        /// <summary>
        /// 获取视野内 所有有效数据的item。
        /// 如果是多生成的备用item，没用于赋值，则不算其中。
        /// </summary>
        public virtual List<ScrollViewBaseItem> GetInViewUseDataAllItems()
        {
            return null;
        }
        /// <summary>
        /// 开启分帧实例化后 每帧实例化item数量 默认是1帧一个
        /// </summary>
        /// <param name="frameInitCount"></param>
        public void SetFrameInitItemCount(int frameInitCount)
        {
            this.frameInitCount = frameInitCount;
        }
        /// <summary>
        /// 是否开启分帧实例化 默认开启
        /// </summary>
        /// <param name="isEnableFrameInit"></param>
        public void SetIsEnableFrameInit(bool isEnableFrameInit)
        {
            this.isEnableFrameInit = isEnableFrameInit;
        }
    }
}
