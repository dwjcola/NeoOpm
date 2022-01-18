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
    public class ScrollViewBaseItem : MonoBehaviour
    {

        public RectTransform rect;
        public int dataIndex;
        public virtual void Awake()
        {
            rect = GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0,1);
            rect.anchorMax = new Vector2(0,1);
        }

        public virtual void UpdateData(int index)
        {
            dataIndex = index;
        }
    }
}
