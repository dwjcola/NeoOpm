using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameFramework;
using GameFramework.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace NeoOPM
{
    
    public static class UIUtility
    {
        public const int cameraW = 1920;
        public  const int cameraH = 1080;
        public static float GetMaxScale()
        {
            float scaleW = Screen.width*1f / cameraW;
            float scaleH = Screen.height*1f / cameraH;
            return Mathf.Max(1, Mathf.Max(scaleH, scaleW));
        }
        /// <summary>
        /// 销毁子控件
        /// </summary>
        /// <param name="trans"></param>
        public static void DestroyChildren(Transform trans)
        {
            if (trans == null)
            {
                return;
            }
            int count = trans.childCount;
            for (int i = count - 1; i >= 0; i--)
            {
                var itemTemp = trans.GetChild(i);
                itemTemp.transform.SetParent(null);
                GameObject.Destroy(itemTemp.gameObject);
            }
        }
//颜色值转换工具
        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color(r/255f,g/255f,b/255f,1);
        }
        public static Color32 HexToColor32(string hex)
        {
            return HexToColor(hex);
        }
        public static Vector3[] GetWorldCorners(RectTransform rt)
        {
            Vector3[] cs = new Vector3[4];
            if (rt != null)
            {
                rt.GetWorldCorners(cs);
            }
            return cs;
        }
        public static Vector2 UIWorldToScreenPoint(Vector3 world,Transform target = null)
        {
            Canvas root = GetRoot();
            Camera UICamera = GetUICamera();
            if (target!=null)
            {
                if (target.gameObject.layer == 11)
                {
                    Transform trans = Camera.main.transform.Find("3DUICamera");
                    Camera temp = null;
                    if (trans != null) 
                        temp = trans.GetComponent<Camera>();
                    UICamera = temp != null ? temp : UICamera;
                }
            }
            // 把世界坐标转成 屏幕坐标
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(UICamera, world);
            
            Vector2 localPoint;
            // 把屏幕坐标 转成 局部坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(root.GetComponent<RectTransform>(), screenPoint, GetUICamera(), out localPoint);
            return localPoint;
        }

        public static Vector2 GetWorldGoToUIPos(Vector3 pos)
        {
            Canvas root = GetRoot();
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(pos); 
            //Debug.LogError("屏幕坐标："+screenPoint);
            Vector2 localPoint;
            // 把屏幕坐标 转成 局部坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(root.GetComponent<RectTransform>(), screenPoint, GetUICamera(), out localPoint);
            //Debug.LogError("局部坐标："+localPoint);
            return localPoint;
        }
        public static Camera GetUICameraByTrans(Transform target)
        {
            Camera UICamera = GetUICamera();
            if (target.gameObject.layer == 11)
            {
                Transform trans = Camera.main.transform.Find("3DUICamera");
                Camera temp = null;
                if (trans != null) 
                    temp = trans.GetComponent<Camera>();
                UICamera = temp != null ? temp : UICamera;
            }

            return UICamera;
        }
        public static Vector2 WorldToScreenPosInRectangle(Vector3 worldPosition,RectTransform parent)
        {
            Camera UICamera = GetUICamera();
            Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);
            
            Vector2 position;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPosition,
                UICamera, out position))
            {
                return position;
            }
            return Vector2.zero;
        }
        public static Vector3 ScreenToWorldPosInRectangle(Vector3 pos, RectTransform parent)
        {
            Vector3 worldPosition;
            Camera UICamera = GetUICamera();
            RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, pos, UICamera, out worldPosition);
            return worldPosition;
        }

        public static Canvas GetRoot()
        {
            IUIGroup uiGroup = GameEntry.UI.GetUIGroup("Default");
            Transform rootT = ((UIGroupHelperBase) uiGroup.Helper).transform.parent;
            Canvas root = rootT.GetComponent<Canvas>();
            return root;
        }
        public static Vector2 GetResolution()
        {
            IUIGroup uiGroup = GameEntry.UI.GetUIGroup("Default");
            Transform rootT = ((UIGroupHelperBase) uiGroup.Helper).transform.parent;
            CanvasScaler rootScaler = rootT.GetComponent<CanvasScaler>();
            return rootScaler.referenceResolution;
        }
        public static Camera GetUICamera()
        {
            Canvas root = GetRoot();
            return root.worldCamera;
        }
        public static List<string> PaseStr(string s,string sub)
        {
            List<string> result = new List<string>();
            int start = 0;
            for (int i = 0; i < s.Length; i++)
            {
                int index = s.IndexOf(sub, start,StringComparison.Ordinal);
                if (index>-1)
                {
                    int r = index + 1;
                    string ss = s.Substring(r, 1);
                    if (!string.IsNullOrEmpty(ss))
                    {
                        result.Add(ss);
                    }

                    start = r;
                }
            }
            return result;
        }

        public static EventSystem currentEventSystem;
        public static void EnableEventSystem(bool b)
        {
            if (EventSystem.current)
            {
                currentEventSystem = EventSystem.current;
            }

            currentEventSystem.enabled = b;
        }
        public static void ResetUIWithInScreen(RectTransform target, Transform canvas)
        {
            Rect screenRect = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
            CanvasScaler canvasScaler = GameEntry.UI.InstanceRoot?.GetComponent<CanvasScaler>();
            if (canvasScaler != null)
            {
                float xScale = canvasScaler.referenceResolution.x / Screen.width;
                float yScale = canvasScaler.referenceResolution.y / Screen.height;
                screenRect = new Rect(screenRect.x * xScale, screenRect.y * yScale, screenRect.width * xScale, screenRect.height * yScale);
            }
            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(canvas, target);

            Vector2 delta = default(Vector2);
            if (bounds.center.x - bounds.extents.x < screenRect.x)//target超出area的左边框
            {
                delta.x += Mathf.Abs(bounds.center.x - bounds.extents.x - screenRect.x);
            }
            else if (bounds.center.x + bounds.extents.x > screenRect.width / 2)//target超出area的右边框
            {
                delta.x -= Mathf.Abs(bounds.center.x + bounds.extents.x - screenRect.width / 2);
            }
            if (bounds.center.y - bounds.extents.y < screenRect.y)//target超出area上边框
            {
                delta.y += Mathf.Abs(bounds.center.y - bounds.extents.y - screenRect.y);
            }
            else if (bounds.center.y + bounds.extents.y > screenRect.height / 2)//target超出area的下边框
            {
                delta.y -= Mathf.Abs(bounds.center.y + bounds.extents.y - screenRect.height / 2);
            }

            //加上偏移位置算出在屏幕内的坐标
            target.anchoredPosition += delta;

            // delta != default(Vector2);
        }
        public static Vector2 ScreenPointToLocalPointInRectangle(RectTransform rect,Vector3 screenPos,Camera camera)
        {
            Vector2 uguiPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, camera, out uguiPos)){
                return uguiPos;
            }
            return Vector2.zero;
        }

        public static bool IsInCity()
        {
            if (GameEntry.Procedure.CurrentProcedure is ProcedureCity)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }
   
}
