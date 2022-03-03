#region << 文 件 说 明 >>

/*----------------------------------------------------------------
// 文件名称：GameUtility
// 创 建 者：dongwj
// 创建时间：2022年02月17日 星期四 15:09
// 文件版本：V1.0.0
//===============================================================
// 功能描述：
//		工具方法
//
//----------------------------------------------------------------*/

#endregion

using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NeoOPM
{
    public class GameUtility
    {
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
                Object.Destroy(itemTemp.gameObject);
            }
        }
        public static Vector3 GetRayRaycastHitInfo(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("3DPlane")))
            {
                return hit.point;
            }
            return Vector3.zero;
        }
        public static Camera GetUICamera()
        {
            return GameEntry.UI.m_uiCamera;
        }
        public static Vector3 ScreenPointToWorldPointInRectangle(GameObject target, UnityEngine.EventSystems.PointerEventData eventData)
        {
            Vector3 pos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(target.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos))
            {
                return pos;
            }
            return pos;
        }
        public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Vector3 target)
        {
            Vector3 tarPos = target;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);

            Vector2 imgPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);
          
            return new Vector2(imgPos.x, imgPos.y);
        }


        public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Transform targetTR)
        {
            
            Vector3 tarPos = targetTR.position;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);

            Vector2 imgPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);

            return new Vector2(imgPos.x, imgPos.y);
        }
        public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Transform targetTR, float offsetX, float offsetY, float offsetZ)
        {
            Vector3 tarPos = targetTR.position + new Vector3(offsetX, offsetY, offsetZ);
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);

            Vector2 imgPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);

            return new Vector2(imgPos.x, imgPos.y);
        }
        public static Type GetType(string typeName)
        {
            // 先在当前Assembly找,再在UnityEngine里找.
            // 如果都找不到,再遍历所有Assembly
            var type = Assembly.GetExecutingAssembly().GetType(typeName) ?? typeof(ParticleSystem).Assembly.GetType(typeName);
            if (type != null) return type;
            type = GetUnityType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }

        private static Type GetUnityType(string typeName)
        {
            string namespaceStr = "UnityEngine";
            if (!typeName.Contains(namespaceStr))
                typeName = namespaceStr + "." + typeName;
            var assembly = Assembly.Load(namespaceStr);
            if (assembly == null)
                return null;
            return assembly.GetType(typeName);

        }

        public static Component GetOrAddComponent(GameObject target, string className)
        {
            Component com = target.GetComponent(className);
            if (com == null)
            {
                com = target.AddComponent(GetType(className));
            }
            return com;
        }

        public static Component GetOrAddComponent(GameObject target, string path, Type className)
        {
            Transform child = target.transform.Find(path);
            if (child == null)
            {
                return null;
            }
            Component com = child.GetComponent(className);
            if (com == null)
            {
                com = child.gameObject.AddComponent(className);
            }
            return com;
        }
        public static Component GetOrAddComponent(GameObject target, Type className)
        {
            Component com = target.GetComponent(className);
            if (com == null)
            {
                com = target.AddComponent(className);
            }
            return com;
        }

        public static SkinnedMeshRenderer[] GetSkinnedMeshRenderersInChildren(GameObject target)
        {
            SkinnedMeshRenderer[] com = target.GetComponentsInChildren<SkinnedMeshRenderer>();
            return com;
        }
    }
}