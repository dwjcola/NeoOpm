using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// explain：循环列表组件 UGUI版
/// author：王云飞
/// email:1172906928@qq.com
/// </summary>
namespace lufeigame
{
    [CustomEditor(typeof(lufeigame.ScrollViewGrid))]
    public class ScrollViewGridInspector : Editor
    {

        private ScrollViewGrid myTarget;
        private ScrollRect parentScrollRect;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (myTarget == null)
            {
                myTarget = target as ScrollViewGrid;
            }
            if (parentScrollRect == null)
            {
                parentScrollRect = myTarget.transform.GetComponentInParent<ScrollRect>();
            }
            if (parentScrollRect == null)
            {
                EditorGUILayout.HelpBox("未检测到ScrollRect组件", MessageType.Warning);
            }
            myTarget.padding_left = EditorGUILayout.FloatField("Padding-Left(左边距)", myTarget.padding_left);
            myTarget.padding_right = EditorGUILayout.FloatField("Padding-Right(右边距)", myTarget.padding_right);
            myTarget.padding_top = EditorGUILayout.FloatField("Padding-Top(顶边距)", myTarget.padding_top);
            myTarget.padding_bottom = EditorGUILayout.FloatField("Padding-Bottom(底边距)", myTarget.padding_bottom);
            myTarget.spacingX = EditorGUILayout.FloatField("Spacing X(横向间距)", myTarget.spacingX);
            myTarget.spacingY = EditorGUILayout.FloatField("Spacing Y(纵向间距)", myTarget.spacingY);
            myTarget.cellSize = EditorGUILayout.Vector2Field("Cell Size(元素大小)", myTarget.cellSize);
            myTarget.childLayout = (ScrollViewGrid.ChildLayout)EditorGUILayout.EnumPopup("Child Layout(元素布局)", myTarget.childLayout);
            if (myTarget.childLayout == ScrollViewGrid.ChildLayout.Horizontal)
            {
                myTarget.row = EditorGUILayout.IntField("Row(固定行数)", myTarget.row);
             
            }
            else
            {
                myTarget.column = EditorGUILayout.IntField("Column(固定列数)", myTarget.column);
            }
            EditorUtility.SetDirty(myTarget);
        }
    }
}
