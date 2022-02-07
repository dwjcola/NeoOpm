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
    [RequireComponent(typeof(ScrollRect))]
    public class VScrollViewLoop : ScrollViewLoop
    {
        private float prevAnchoredPosition = 0;
        private float paddingDistance = 0;
        private int maxRow = 1;
        private int _restRowCount = 3;
        private Vector3 temp = Vector3.zero;
        private Vector3 temp1 = Vector3.zero;
        private List<ScrollViewBaseItem> resetTemplist = new List<ScrollViewBaseItem>();
        private List<ScrollViewBaseItem> lastItemsTempList = new List<ScrollViewBaseItem>();
        private Dictionary<int, float> cellHeights = new Dictionary<int, float>();

        // Use this for initialization
        public override  void Start()
        {
            base.Start();
            this.prevAnchoredPosition = this.contentRect.anchoredPosition.y;
            if (IsPreview)
            {
                isEnableFrameInit = false;
                ReLoad();
            }
            paddingDistance = grid.cellSize.y + grid.spacingY;
        }

        // Update is called once per frame
        void Update()
        {
            if (isEnableFrameInit && isFrameIniting)
            {
                return;
            }
            //下拉
            while (this.contentRect.anchoredPosition.y - this.prevAnchoredPosition > paddingDistance * 2)
            {
                //到达结尾元素
                if (curEndIndex >= totalCount - 1)
                {
                    return;
                }
               // Debug.Log("下拉出界");
                this.prevAnchoredPosition += paddingDistance;
                //在结尾刷新一行新数据 
                //如果不满一行
                int showItemCount = 0;
                if (totalCount - (curEndIndex + 1) < grid.column)
                {
                    //Debug.Log("刷新到最后一行");
                    showItemCount = totalCount - (curEndIndex + 1);
                }
                else
                {
                    showItemCount = grid.column;
                }
                //将顶层一行移动至最后一行 取出队列的顶部五个元素 再加入队列结尾
                //Vector3 position = Vector3.zero;
                float disY = (maxRow + 1 - 1) * grid.cellSize.y + (maxRow + 1 - 1) * grid.spacingY;
                for (int i = 0; i < grid.column; i++)
                {
                    ScrollViewBaseItem item = items.Dequeue();
                    //position = new Vector3(item.rect.anchoredPosition3D.x, item.rect.anchoredPosition3D.y - disY, 0);
                    temp.Set(item.rect.anchoredPosition3D.x, item.rect.anchoredPosition3D.y - disY, 0);
                    item.rect.anchoredPosition3D = temp;
                    curEndIndex++;
                    curBeginIndex++;
                    if (i < showItemCount)
                    {
                        item.gameObject.SetActive(true);                      
                        item.name = curEndIndex.ToString();
                        item.dataIndex = curEndIndex;
                        item.UpdateData(curEndIndex);
                    }
                    else
                    {
                        item.gameObject.SetActive(false);
                    }
                    items.Enqueue(item);
                }
            }

        //    Debug.LogErrorFormat("上拉出界##########{0}|{1}", this.contentRect.anchoredPosition.y, this.prevAnchoredPosition);
            while (this.contentRect.anchoredPosition.y - this.prevAnchoredPosition < 0)
            {
                //到达结尾元素
                if (curBeginIndex <= 0)
                {
                    return;
                }
                this.prevAnchoredPosition -= paddingDistance;


                List<ScrollViewBaseItem> lastItems = GetQuenLastItems();
               // Vector3 position = Vector3.zero;
                float disY = (maxRow + 1 - 1) * grid.cellSize.y + (maxRow + 1 - 1) * grid.spacingY;
                for (int i = grid.column - 1; i >= 0; i--)
                {
                    ScrollViewBaseItem item = lastItems[i];
                    item.gameObject.SetActiveVirtual(true);
                    //position = new Vector3(item.rect.anchoredPosition3D.x, item.rect.anchoredPosition3D.y + disY, 0);
                    temp1.Set(item.rect.anchoredPosition3D.x, item.rect.anchoredPosition3D.y + disY, 0);
                    item.rect.anchoredPosition3D = temp1;
                    curEndIndex--;
                    curBeginIndex--;
                    item.name = curBeginIndex.ToString();
                    item.dataIndex = curBeginIndex;
                    item.UpdateData(curBeginIndex);
                }
                ResetQuenItems();
            }
        }

        public override void ReLoad()
        {
            base.ReLoad();
            //回收
            RecoveryItems();
            if (totalCount <= 0) return;
            ResetListData();
            isFrameIniting = false;
            //计算可视范围内 需要生产的Item
            //除去padding内部填充距离
            //Debug.Log(viewportRect.rect);
            //列为固定
            float view_y = viewportRect.rect.height - grid.padding_top;
            int index = 0;
            while (true)
            {
                if (index > 100)
                {
                    break;
                }
                float y = grid.cellSize.y * 0.5f + grid.padding_top + (grid.cellSize.y + grid.spacingY) * index;
                if (y + grid.cellSize.y * 0.5f >= viewportRect.rect.height)
                {
                    break;
                }
                maxRow++;
                index++;
            }
            //Debug.Log("最大可视行数:"+maxRow);
            if (maxRow == 0)
            {
                Debug.LogError("计算错误！计算不出可视范围内最大能显示的行数");
            }
            // maxRow++;
            maxRow += _restRowCount;
            int preItemCount = maxRow * grid.column;
            if (preItemCount > totalCount)
            {
                preItemCount = totalCount;
            }
            if (isEnableFrameInit)
            {
                StopAllCoroutines();
                StartCoroutine(CreateItems(preItemCount));
            }
            else
            {

                for (int i = 0; i < preItemCount; i++)
                {
                    ScrollViewBaseItem item = GetNewItem();
                    item.gameObject.name = i.ToString();
                    item.gameObject.SetActiveVirtual(true);
                    Vector3 position = Vector3.zero;
                    position = new Vector3(grid.padding_left + grid.cellSize.x * 0.5f + (grid.cellSize.x + grid.spacingX) * (i % grid.column), -grid.cellSize.y * 0.5f - grid.padding_top - (grid.cellSize.y + grid.spacingY) * (i / grid.column), 0);
                    item.rect.anchoredPosition3D = position;
                    item.UpdateData(i);
                    items.Enqueue(item);
                }
                curEndIndex = preItemCount - 1;
                curBeginIndex = 0;
            }

            //计算需要占据的总行数的纵向滑动长度
            float totalLayoutSize = 0;
            if (totalCount % grid.column == 0)
            {
                //总单元格大小*行数+内边距顶部+内边距底部+纵向间隔数量*纵向间隔距离
                totalLayoutSize = (totalCount / grid.column) * grid.cellSize.y + grid.padding_top + grid.padding_bottom + (totalCount / grid.column - 1) * grid.spacingY;
            }
            else
            {
                int row = totalCount / grid.column + 1;
                totalLayoutSize = row * grid.cellSize.y + grid.padding_top + grid.padding_bottom + (row - 1) * grid.spacingY;
            }
            contentRect.sizeDelta = new Vector2(viewportRect.rect.width, totalLayoutSize);


        }
        /// <summary>
        /// 设置某个索引数据的单元格高度
        /// </summary>
        /// <param name="index"></param>
        /// <param name="height"></param>
        public void SetCellHeight(int index,float height)
        {
            if (cellHeights.ContainsKey(index))
            {
                cellHeights[index] = height;
            }
            else
            {
                cellHeights.Add(index,height);
            }
        }
        private IEnumerator CreateItems(int preItemCount)
        {
            isFrameIniting = true;
            for (int i = 0; i < preItemCount; i++)
            {
                ScrollViewBaseItem item = GetNewItem();
                item.gameObject.name = i.ToString();
                item.gameObject.SetActiveVirtual(true);
                Vector3 position = Vector3.zero;
                position = new Vector3(grid.padding_left + grid.cellSize.x * 0.5f + (grid.cellSize.x + grid.spacingX) * (i % grid.column), -grid.cellSize.y * 0.5f - grid.padding_top - (grid.cellSize.y + grid.spacingY) * (i / grid.column), 0);
                item.rect.anchoredPosition3D = position;
                item.UpdateData(i);
                items.Enqueue(item);
                if (i % frameInitCount == 0)
                {
                    yield return null;
                }

            }
            isFrameIniting = false;
            curEndIndex = preItemCount - 1;
            curBeginIndex = 0;
        }
        public void SetRestRowCount(int restRowCount)
        {

            if (restRowCount < 0 || restRowCount > 3)
            {
                Debug.LogError("restRowCount必须在0-3之间");
            }

            _restRowCount = restRowCount;
        }

        public override void ReloadTargetIndex(int targetIndex)
        {
            base.ReloadTargetIndex(targetIndex);
            //Debug.LogError("默认加载跳转到"+targetIndex);
            scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            ReLoad();
            Jump(targetIndex);
        }
        public override void Jump(int targetIndex)
        {
            base.Jump(targetIndex);
            //float moveTotalLength = 0;
            //计算需要占据的总行数的纵向滑动长度
           // float totalLayoutSize = 0;
            int totalrow = 0;
            if (totalCount % grid.column == 0)
            {
                //总单元格大小*行数+内边距顶部+内边距底部+纵向间隔数量*纵向间隔距离
               // totalLayoutSize = (totalCount / grid.column) * grid.cellSize.y + grid.padding_top + grid.padding_bottom + (totalCount / grid.column - 1) * grid.spacingY;
                totalrow = totalCount / grid.column;
            }
            else
            {
                totalrow = totalCount / grid.column + 1;
                //totalLayoutSize = totalrow * grid.cellSize.y + grid.padding_top + grid.padding_bottom + (totalrow - 1) * grid.spacingY;
            }
           // moveTotalLength = totalLayoutSize - viewportRect.rect.height;
            int rowIndex = 1; //计算索引在第几行 从1开始
            if ((targetIndex + 1) % grid.column == 0)
            {
                rowIndex = (targetIndex+1) / grid.column;
            }
            else
            {
                rowIndex = Mathf.CeilToInt((targetIndex + 1) *1f/ grid.column);
            }
            //float jumpPositonY = (targetIndex * 1f / (totalCount-1)) * moveTotalLength;  
            if (rowIndex > totalrow)
            {
                rowIndex = totalrow;
            }
            //float jumpPositonY = (rowIndex * 1f / totalrow ) * moveTotalLength;
            float jumpPositonY = (rowIndex - 1) * grid.cellSize.y + grid.padding_top - grid.spacingY + ((rowIndex - 1) * grid.spacingY);
            contentRect.anchoredPosition3D = new Vector2(contentRect.anchoredPosition.x,jumpPositonY);
            if (scrollRect.movementType != ScrollRect.MovementType.Elastic)
            {
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
            }

        }

        /// <summary>
        /// 重置队列
        /// </summary>
        private void ResetQuenItems()
        {
           // List<ScrollViewBaseItem> newItems = new List<ScrollViewBaseItem>();
            resetTemplist.Clear();
            resetTemplist.AddRange(GetQuenLastItems());
            var itor = items.GetEnumerator();
            while (itor.MoveNext())
            {
                resetTemplist.Add(itor.Current);
            }
            resetTemplist.RemoveRange(items.Count, grid.column);
            items.Clear();
            for (int i = 0; i < resetTemplist.Count; i++)
            {
                items.Enqueue(resetTemplist[i]);
            }

        }
        /// <summary>
        /// 获取队列最后一行元素 不打破队列结构
        /// </summary>
        /// <returns></returns>
        private List<ScrollViewBaseItem> GetQuenLastItems()
        {
            int count = items.Count;
            var itor = items.GetEnumerator();
            int index = 0;
            int getBegindIndex = items.Count - grid.column;
            // List<ScrollViewBaseItem> lastItems = new List<ScrollViewBaseItem>();
            lastItemsTempList.Clear();
            while (itor.MoveNext())
            {
                if (index >= getBegindIndex)
                {
                    lastItemsTempList.Add(itor.Current);
                }
                index++;
            }
            return lastItemsTempList;
        }
        /// <summary>
        /// 回收已经创建的item
        /// </summary>
        private void RecoveryItems()
        {
            if (items.Count == 0) return;
            var oldRestItems = new Queue<ScrollViewBaseItem>();
            copyQueueToAnotherQueueTail(recoveryItems, oldRestItems);
            recoveryItems.Clear();
            var tor = items.GetEnumerator();
            while (tor.MoveNext())
            {
                tor.Current.gameObject.SetActiveVirtual(false);
                recoveryItems.Enqueue(tor.Current);
            }
            items.Clear();
            copyQueueToAnotherQueueTail(oldRestItems, recoveryItems);
            oldRestItems.Clear();

        }

        private void copyQueueToAnotherQueueTail(Queue<ScrollViewBaseItem> source, Queue<ScrollViewBaseItem> target)
        {
            var torSource = source.GetEnumerator();
            while (torSource.MoveNext())
            {
                //torSource.Current.gameObject.SetActive(false);
                target.Enqueue(torSource.Current);
            }
        }
        /// <summary>
        /// 获取一个新的item
        /// </summary>
        /// <returns></returns>
        private ScrollViewBaseItem GetNewItem()
        {
            if (recoveryItems.Count == 0)
            {
                GameObject clone = GameObject.Instantiate(prefabItem.gameObject);
                clone.transform.SetParent(grid.transform);
                clone.transform.localScale = Vector3.one;
                clone.transform.localRotation = Quaternion.identity;
                ScrollViewBaseItem item = clone.GetComponent<ScrollViewBaseItem>();
                if (item is ScrollViewLuaItem)
                {
                    ScrollViewLuaItem luaItem = item as ScrollViewLuaItem;
                    ScrollViewLuaItem preLua = prefabItem.GetComponent<ScrollViewLuaItem>();
                    luaItem.RegisterInitFunction(preLua.initCallback);
                    luaItem.RegisterUpdataFunction(preLua.updataCallback);
                    luaItem.InitComponent ();
                }
                return item;
            }
            else
            {
                return recoveryItems.Dequeue();
            }
        }
        /// <summary>
        /// 重置列表
        /// </summary>
        private void ResetListData()
        {
            curEndIndex = 0;
            curBeginIndex = 0;
            maxRow = 1;
            contentRect.anchoredPosition3D = Vector3.zero;
            prevAnchoredPosition = contentRect.anchoredPosition.y;
            paddingDistance = grid.cellSize.y + grid.spacingY;
        }
        public override List<ScrollViewBaseItem> GetInViewUseDataAllItems()
        {
            List<ScrollViewBaseItem> rstItems = new List<ScrollViewBaseItem>();
            var itemtor =  items.GetEnumerator();
            while (itemtor.MoveNext())
            {
                if (itemtor.Current.dataIndex >= curBeginIndex && itemtor.Current.dataIndex <= curEndIndex)
                {
                    rstItems.Add(itemtor.Current);
                }
            }
            return rstItems;
        }

        //        #region Editor
        //#if UNITY_EDITOR
        //        [UnityEditor.MenuItem("GameObject/UI/VScrollViewLoop")]
        //        public static void CreateVertical()
        //        {
        //            GameObject loader = Resources.Load("VerticalScrollViewTemplate") as GameObject;
        //            if (loader.layer != UnityEditor.Selection.activeTransform.gameObject.layer)
        //            {
        //                if (UnityEditor.EditorUtility.DisplayDialog("错误提示", "资源模板中的layer层设置与选中的节点layer层不一致!请修改模板layer层级", "跳转到模板资源"))
        //                {
        //                    UnityEditor.Selection.activeObject = loader;
        //                }
        //                return;
        //            }

        //            GameObject template = GameObject.Instantiate(loader);
        //            template.name = "Vertical Scroll View";
        //            template.transform.SetParent(UnityEditor.Selection.activeTransform, false);
        //        }
        //#endif
        //        #endregion
    }
}
