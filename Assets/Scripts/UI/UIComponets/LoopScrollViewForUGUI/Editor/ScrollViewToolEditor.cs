using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScrollViewToolEditor :Editor {

    [UnityEditor.MenuItem("GameObject/UI/HScrollViewLoop",false,1)]
    public static void CreateHertical()
    {
        GameObject loader = Resources.Load("HorizontalScrollViewTemplate") as GameObject;
        if (loader.layer != UnityEditor.Selection.activeTransform.gameObject.layer)
        {
            if (UnityEditor.EditorUtility.DisplayDialog("错误提示", "资源模板中的layer层设置与选中的节点layer层不一致!请修改模板layer层级", "跳转到模板资源"))
            {
                UnityEditor.Selection.activeObject = loader;
            }
            return;
        }
        GameObject template = GameObject.Instantiate(loader);
        template.name = "Horizontal Scroll View";
        template.transform.SetParent(UnityEditor.Selection.activeTransform, false);
    }

    [UnityEditor.MenuItem("GameObject/UI/VScrollViewLoop",false,2)]
    public static void CreateVertical()
    {
        GameObject loader = Resources.Load("VerticalScrollViewTemplate") as GameObject;
        if (loader.layer != UnityEditor.Selection.activeTransform.gameObject.layer)
        {
            if (UnityEditor.EditorUtility.DisplayDialog("错误提示", "资源模板中的layer层设置与选中的节点layer层不一致!请修改模板layer层级", "跳转到模板资源"))
            {
                UnityEditor.Selection.activeObject = loader;
            }
            return;
        }

        GameObject template = GameObject.Instantiate(loader);
        template.name = "Vertical Scroll View";
        template.transform.SetParent(UnityEditor.Selection.activeTransform, false);
    }
}
