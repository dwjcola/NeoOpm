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
    public class HScrollViewLoop : ScrollViewLoop
    {
        private float prevAnchoredPosition = 0;
        private float paddingDistance = 0;
        private int maxCloum = 1;
        private int jumpTargetIndex = -1;

        // Use this for initialization
       public override void Start()
        {
            base.Start();
            this.prevAnchoredPosition = this.contentRect.anchoredPosition.x;
            if (IsPreview)
            {
                ReLoad();
            }
            paddingDistance = grid.cellSize.x + grid.spacingX;
        }
        // Update is called once per frame
        void Update()
        {
            if (isEnableFrameInit && isFrameIniting)
            {
                return;
            }

            //下拉
            while (this.contentRect.anchoredPosition.x - this.prevAnchoredPosition <- paddingDistance * 2)
            {
                //到达结尾元素
                if (curEndIndex >= totalCount - 1)
                {
                    return;
                }
                //Debug.Log("左滑出界");
                this.prevAnchoredPosition -= paddingDistance;
                //在结尾刷新一行新数据 
                //如果不满一行
                int showItemCount = 0;
                if (totalCount - (curEndIndex + 1) < grid.row)
                {
                    //Debug.Log("刷新到最后一行");
                    showItemCount = totalCount - (curEndIndex + 1);
                }
                else
                {
                    showItemCount = grid.row;
                }
                //将顶层一行移动至最后一行 取出队列的顶部五个元素 再加入队列结尾
                Vector3 position = Vector3.zero;
                float disX = (maxCloum + 1 - 1) * grid.cellSize.x + (maxCloum + 1 - 1) * grid.spacingX;
                for (int i = 0; i < grid.row; i++)
                {
                    ScrollViewBaseItem item = items.Dequeue();
                    position = new Vector3(item.rect.anchoredPosition3D.x + disX, item.rect.anchoredPosition3D.y, 0);
                    item.rect.anchoredPosition3D = position;
                    curEndIndex++;
                    curBeginIndex++;
                    if (i < showItemCount)
                    {
                        item.name = curEndIndex.ToString();
                        item.gameObject.SetActiveVirtual(true);
                        item.UpdateData(curEndIndex);
                    }
                    else
                    {
                        item.gameObject.SetActiveVirtual(false);
                    }
                    items.Enqueue(item);
                }
            }

            while (this.contentRect.anchoredPosition.x - this.prevAnchoredPosition > -paddingDistance*0.5f)
            {
               /// 到达结尾元素
                if (curBeginIndex <= 0)
                {
                    return;
                }
              //  Debug.Log("右滑出界");
                this.prevAnchoredPosition += paddingDistance;
                List<ScrollViewBaseItem> lastItems = GetQuenLastItems();
                Vector3 position = Vector3.zero;
                float disX = (maxCloum + 1 - 1) * grid.cellSize.x + (maxCloum + 1 - 1) * grid.spacingX;
                for (int i = grid.row - 1; i >= 0; i--)
                {
                    ScrollViewBaseItem item = lastItems[i];
                    item.gameObject.SetActive(true);
                    position = new Vector3(item.rect.anchoredPosition3D.x - disX, item.rect.anchoredPosition3D.y, 0);
                    item.rect.anchoredPosition3D = position;
                    curEndIndex--;
                    curBeginIndex--;
                    item.name = curBeginIndex.ToString();
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
            //计算可视范围内 需要生产的Item
            //除去padding内部填充距离
            //Debug.Log(viewportRect.rect);
            //列为固定
            int index = 0;
            while (true)
            {
                if (index > 100)
                {
                    break;
                }
                float x = grid.cellSize.x * 0.5f + grid.padding_left + (grid.cellSize.x + grid.spacingX) * index;
                if (x + grid.cellSize.x * 0.5f >= viewportRect.rect.width)
                {
                    break;
                }
                maxCloum++;
                index++;
            }
            //Debug.Log("最大可视行数:"+maxRow);
            if (maxCloum == 0)
            {
                Debug.LogError("计算错误！计算不出可视范围内最大能显示的行数");
            }
            // maxRow++;
            maxCloum += 3;
            int preItemCount = maxCloum * grid.row;
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
                    item.gameObject.SetActive(true);
                    Vector3 position = Vector3.zero;
                    //position = new Vector3(grid.padding_left + grid.cellSize.x * 0.5f + (grid.cellSize.x + grid.spacingX) * (i % grid.column), -grid.cellSize.y * 0.5f - grid.padding_top - (grid.cellSize.y + grid.spacingY) * (i / grid.column), 0);
                    position = new Vector3(grid.cellSize.x * 0.5f + grid.padding_left + (grid.cellSize.x + grid.spacingX) * (i / grid.row), -grid.padding_top - grid.cellSize.y * 0.5f - (grid.cellSize.y + grid.spacingY) * (i % grid.row), 0);
                    item.rect.anchoredPosition3D = position;
                    item.UpdateData(i);
                    items.Enqueue(item);
                }
                curEndIndex = preItemCount - 1;
                curBeginIndex = 0;
            }
       
            //计算需要占据的总行数的纵向滑动长度
            float totalLayoutSize = 0;
            if (totalCount % grid.row == 0)
            {
                //总单元格大小*行数+内边距顶部+内边距底部+纵向间隔数量*纵向间隔距离
                totalLayoutSize = (totalCount / grid.row) * grid.cellSize.x + grid.padding_left + grid.padding_right + (totalCount / grid.row - 1) * grid.spacingX;
            }
            else
            {
                int colum = totalCount / grid.row + 1;
                totalLayoutSize = colum * grid.cellSize.x + grid.padding_left + grid.padding_right + (colum - 1) * grid.spacingX;
            }
            contentRect.sizeDelta = new Vector2(totalLayoutSize,viewportRect.rect.height);


        }
        private IEnumerator CreateItems(int preItemCount)
        {
            for (int i = 0; i < preItemCount; i++)
            {
                ScrollViewBaseItem item = GetNewItem();
                item.gameObject.name = i.ToString();
                item.gameObject.SetActive(true);
                Vector3 position = Vector3.zero;
                //position = new Vector3(grid.padding_left + grid.cellSize.x * 0.5f + (grid.cellSize.x + grid.spacingX) * (i % grid.column), -grid.cellSize.y * 0.5f - grid.padding_top - (grid.cellSize.y + grid.spacingY) * (i / grid.column), 0);
                position = new Vector3(grid.cellSize.x * 0.5f + grid.padding_left + (grid.cellSize.x + grid.spacingX) * (i / grid.row), -grid.padding_top - grid.cellSize.y * 0.5f - (grid.cellSize.y + grid.spacingY) * (i % grid.row), 0);
                item.rect.anchoredPosition3D = position;               
                item.UpdateData(i);
                items.Enqueue(item);
                if (i % frameInitCount == 0)
                {
                    yield return null;
                }
            }
            curEndIndex = preItemCount - 1;
            curBeginIndex = 0;
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
           // float moveTotalLength = 0;
            //计算需要占据的总行数的纵向滑动长度
            //float totalLayoutSize = 0;
            int totalcol = 0;
            if (totalCount % grid.row == 0)
            {
                //总单元格大小*行数+内边距顶部+内边距底部+纵向间隔数量*纵向间隔距离
               // totalLayoutSize = (totalCount / grid.row) * grid.cellSize.x + grid.padding_left + grid.padding_right + (totalCount / grid.row - 1) * grid.spacingX;
                totalcol = totalCount / grid.row;
            }
            else
            {
               // totalLayoutSize = totalcol * grid.cellSize.x + grid.padding_left + grid.padding_right + (totalcol - 1) * grid.spacingX;
            }
            //moveTotalLength = totalLayoutSize - viewportRect.rect.width;
            int colIndex = 1; //计算索引在第几列 从1开始
            if ((targetIndex + 1) % grid.row == 0)
            {
                colIndex = (targetIndex + 1) / grid.row;
            }
            else
            {
                colIndex = Mathf.CeilToInt((targetIndex + 1) *1f/ grid.row);
            }
            //float jumpPositonY = (targetIndex * 1f / (totalCount-1)) * moveTotalLength;  
            if (colIndex > totalcol)
            {
                colIndex = totalcol;
            }
            //Debug.LogError("可移动总长度：" + moveTotalLength);
            //jumpTargetIndex = targetIndex;
            //float jumpPositonX = -(targetIndex * 1f / (totalCount - 1)) * moveTotalLength;
            //去除左边距
            //float removePading = colIndex * grid.spacingX;
            //Debug.LogError(colIndex);

            float jumpPositonX = -(colIndex - 1) * grid.cellSize.x - grid.padding_left + grid.spacingX-((colIndex-1)*grid.spacingX);
           // float jumpPositonX = -(colIndex * 1f / totalcol) * totalLayoutSize - grid.padding_left;
            //Debug.LogError(jumpPositonX);
            contentRect.anchoredPosition3D = new Vector2(jumpPositonX, contentRect.anchoredPosition.y);
            if (scrollRect.movementType != ScrollRect.MovementType.Elastic)
            {
                scrollRect.movementType = ScrollRect.MovementType.Elastic;
            }

        }

        public void FirstLineJump(float targetIndex)
        {
            float jumpPositonX = (Mathf.CeilToInt(targetIndex / grid.row) - 1) * (grid.spacingX + grid.cellSize.x);
            float min = 0;
            float max = 0;
            if (totalCount % grid.column == 0)
            {
                //总单元格大小*行数+内边距顶部+内边距底部+纵向间隔数量*纵向间隔距离
                max = (totalCount / grid.row) * grid.cellSize.x + grid.padding_left + grid.padding_right + (totalCount / grid.row - 1) * grid.spacingX;
            }
            else
            {
                int colum = totalCount / grid.row + 1;
                max = colum * grid.cellSize.x + grid.padding_left + grid.padding_right + (colum - 1) * grid.spacingX;
            }
            max = Mathf.Max(max - viewportRect.rect.width, 0);
            jumpPositonX = Mathf.Max(min, jumpPositonX);
            jumpPositonX = Mathf.Min(max, jumpPositonX);
            contentRect.anchoredPosition3D = new Vector2(jumpPositonX, contentRect.anchoredPosition.y);
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
            List<ScrollViewBaseItem> newItems = new List<ScrollViewBaseItem>();
            newItems.AddRange(GetQuenLastItems());
            var itor = items.GetEnumerator();
            while (itor.MoveNext())
            {
                newItems.Add(itor.Current);
            }
            newItems.RemoveRange(items.Count, grid.row);
            items.Clear();
            for (int i = 0; i < newItems.Count; i++)
            {
                items.Enqueue(newItems[i]);
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
            int getBegindIndex = items.Count - grid.row;
            List<ScrollViewBaseItem> lastItems = new List<ScrollViewBaseItem>();
            while (itor.MoveNext())
            {
                if (index >= getBegindIndex)
                {
                    lastItems.Add(itor.Current);
                }
                index++;
            }
            return lastItems;
        }
        /// <summary>
        /// 回收已经创建的item
        /// </summary>
        public void RecoveryItems()
        {
            if (items.Count == 0) return;
            recoveryItems.Clear();
            var tor = items.GetEnumerator();
            while (tor.MoveNext())
            {
                tor.Current.gameObject.SetActiveVirtual(false);
                recoveryItems.Enqueue(tor.Current);
            }
            items.Clear();

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
            maxCloum = 1;
            contentRect.anchoredPosition3D = Vector3.zero;
            prevAnchoredPosition = contentRect.anchoredPosition.x;
            paddingDistance = grid.cellSize.x + grid.spacingX;
        }

        public override List<ScrollViewBaseItem> GetInViewUseDataAllItems()
        {
            List<ScrollViewBaseItem> rstItems = new List<ScrollViewBaseItem>();
            var itemtor = items.GetEnumerator();
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
        //        [UnityEditor.MenuItem("GameObject/UI/HScrollViewLoop")]
        //        public static void CreateVertical()
        //        {
        //            GameObject loader = Resources.Load("HorizontalScrollViewTemplate") as GameObject;
        //            if (loader.layer != UnityEditor.Selection.activeTransform.gameObject.layer)
        //            {
        //                if (UnityEditor.EditorUtility.DisplayDialog("错误提示", "资源模板中的layer层设置与选中的节点layer层不一致!请修改模板layer层级", "跳转到模板资源"))
        //                {
        //                    UnityEditor.Selection.activeObject = loader;
        //                }
        //                return;
        //            }
        //            GameObject template = GameObject.Instantiate(loader);
        //            template.name = "Horizontal Scroll View";
        //            template.transform.SetParent(UnityEditor.Selection.activeTransform, false);
        //        }
        //#endif
        //        #endregion

    }
}
