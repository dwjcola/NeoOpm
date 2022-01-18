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
    public class ScrollViewGrid : MonoBehaviour
    { 

        public enum ChildLayout
        {
            Horizontal,
            Vertical,
        }
        [HideInInspector][SerializeField]
        public float padding_left = 0;
        [HideInInspector][SerializeField]
        public float padding_right = 0;
        [HideInInspector][SerializeField]
        public float padding_top = 0;
        [HideInInspector][SerializeField]
        public float padding_bottom = 0;
        [HideInInspector][SerializeField]
        public float spacingX = 0;
        [HideInInspector][SerializeField]
        public float spacingY = 0;
        [HideInInspector][SerializeField]
        public Vector2 cellSize = new Vector2(100, 100);
        [HideInInspector][SerializeField]
        public ChildLayout childLayout = ChildLayout.Vertical;
        [HideInInspector][SerializeField]
        public int column = 1;
        [HideInInspector][SerializeField]
        public int row = 1;

        // Use this for initialization
        void Start()
        {
            RectTransform rect = GetComponent<RectTransform>();
        
            if (childLayout == ChildLayout.Horizontal)
            {
                rect.pivot = new Vector2(0f, 0.5f);
                rect.anchorMin = new Vector2(0f, 0.5f);
                rect.anchorMax = new Vector2(0f, 0.5f);
                rect.anchoredPosition3D = Vector3.zero;
            }
            else
            {
                rect.pivot = new Vector2(0.5f, 1);
                rect.anchorMin = new Vector2(0.5f, 1);
                rect.anchorMax = new Vector2(0.5f, 1);
                rect.anchoredPosition3D = Vector3.zero;
            }
        }
    }
}
