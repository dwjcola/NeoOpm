
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

namespace ProHA
{
   
    public class ListScrollStep : MonoBehaviour,IBeginDragHandler,IEndDragHandler
    {
        public ScrollRect scrollRect;
        private bool isDraging = false;
        private int childrenCount = 0;
        private float distance = 0;
        public float duration = 0.3f;
        private Action<LuaTable, object> callBack;
        private LuaTable lua;
        private void Start()
        {
            //scrollRect = GetComponent<ScrollRect>();
            UpdateDate();
        }

        public void UpdateDate()
        {
            childrenCount = scrollRect.content.childCount;
            distance = childrenCount <= 1?0f:1f / (childrenCount-1);
        }
        public int GetChildrenCount()
        {
            return childrenCount;
        }
        public void Init(LuaTable l,Action<LuaTable, object> cb)
        {
            lua = l;
            callBack = cb;
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            isDraging = false;
            Vector2 pos = scrollRect.normalizedPosition;
            int index = distance == 0 ?0:(int)Math.Round(pos.x / distance);
            MoveToByIndex(index);
        }

        public void MoveToByIndex(int index,bool tween=true)
        {
            index = Math.Min(index, childrenCount-1);
            index = Math.Max(index, 0);
            float targetPos = index * distance;
            targetPos = Math.Min(targetPos, 1f);
            targetPos = Math.Max(targetPos, 0f);
            if (tween)
            {
                Tweener tweener = DOTween.To(() => scrollRect.horizontalNormalizedPosition, x => scrollRect.horizontalNormalizedPosition = x,
                    targetPos, duration);
                
                tweener.OnComplete(() =>
                {
                    callBack?.Invoke(lua,index);
                });
            }
            else
            {
                scrollRect.horizontalNormalizedPosition = targetPos;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDraging = true;
            
        }
    }
}