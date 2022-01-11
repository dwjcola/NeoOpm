
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace NeoOPM
{
   public class CrossGo : MonoBehaviour,
      IPointerClickHandler,
      IPointerDownHandler,
      IPointerUpHandler,
      IBeginDragHandler,
      IDragHandler,
      IEndDragHandler
   {
      private LuaTable luaClass;
      private Action<LuaTable> luaClickCallBack;
      private Action<LuaTable> luaDownCallBack;
      private Action<LuaTable> luaDragCallBack;
      private RectTransform _target;

      private Rect rect;

        private void Awake()
        {
            Vector3[] corners = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(corners);
            float width = Math.Abs(Vector2.Distance(corners[0], corners[3]));
            float height = Math.Abs(Vector2.Distance(corners[0], corners[1]));
            rect = new Rect(corners[0], new Vector2(width, height));
        }

      public void SetClickCallBackWithTarget(LuaTable lua, Action<LuaTable> callback,RectTransform target)
      {
         luaClass = lua;
         luaClickCallBack = callback;
         _target = target;
      }
      public void SetClickCallBack(LuaTable lua, Action<LuaTable> callback)
      {
         luaClass = lua;
         luaClickCallBack = callback;
      }
        public void ClearClickCallBack()
        {
            luaClickCallBack = null;
        }
        public void SetDownCallBack(LuaTable lua, Action<LuaTable> callback)
      {
         luaClass = lua;
         luaDownCallBack = callback;
      }
      public void SetDragCallBack(LuaTable lua, Action<LuaTable> callback)
      {
         luaClass = lua;
         luaDragCallBack = callback;
      }
      private void ExecuteAll(PointerEventData eventData, int flag)
      {
         List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventData, results);
         foreach (RaycastResult result in results)
         {
            if (result.gameObject != gameObject)
            {
               GameObject find = null;
               eventData.pointerCurrentRaycast = result;
               switch (flag)
               {
                  case 1:
                     find = ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);
                     break;
                  case 2:
                     find = ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, ExecuteEvents.pointerDownHandler);
                     break;
                  case 3:
                     find = ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, ExecuteEvents.pointerUpHandler);
                     break;
                  case 4:
                     find = ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, ExecuteEvents.beginDragHandler);
                     break;
                  case 5:
                     find = ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, ExecuteEvents.dragHandler);
                     break;
                  case 6:
                     find = ExecuteEvents.ExecuteHierarchy(result.gameObject, eventData, ExecuteEvents.endDragHandler);
                     break;
               }

               if (find != null)
               {
                  break;
               }

               
            }
         }
      }

      public void OnPointerClick(PointerEventData eventData)
      {
         if (_target != null)
         {
            if (RectTransformUtility.RectangleContainsScreenPoint(_target,eventData.position,UIUtility.GetUICameraByTrans(_target)))
            {
               if (!isDraging) luaClickCallBack?.Invoke(luaClass);
               ExecuteAll(eventData, 1);
            }
         }
         else
         {
            if (!isDraging) luaClickCallBack?.Invoke(luaClass);
            ExecuteAll(eventData, 1);
         }
            
      }

      public void OnPointerDown(PointerEventData eventData)
      {
         if (_target != null)
         {
            if (RectTransformUtility.RectangleContainsScreenPoint(_target,eventData.position,UIUtility.GetUICameraByTrans(_target)))
            {
               luaDownCallBack?.Invoke(luaClass);
               ExecuteAll(eventData, 2);
            }
         }
         else
         {
            luaDownCallBack?.Invoke(luaClass);
            ExecuteAll(eventData, 2);
         }
         
      }

      public void OnPointerUp(PointerEventData eventData)
      {
         if (_target != null)
         {
            if (RectTransformUtility.RectangleContainsScreenPoint(_target,eventData.position,UIUtility.GetUICameraByTrans(_target)))
            {
               ExecuteAll(eventData, 3);
            }
         }
         else
         {
            ExecuteAll(eventData, 3);
         }
         
      }

      private bool isDraging = false;

      public void OnBeginDrag(PointerEventData eventData)
      {
         if (_target == null)
         {
            isDraging = true;
            ExecuteAll(eventData, 4);
         }
      }

      public void OnDrag(PointerEventData eventData)
      {
         if (_target == null)
         {
            luaDragCallBack?.Invoke(luaClass);
            ExecuteAll(eventData, 5);
         }
      }

      public void OnEndDrag(PointerEventData eventData)
      {
         if (_target==null)
         {
            isDraging = false;
            ExecuteAll(eventData, 6);
         }
         
      }

   }
}