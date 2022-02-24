using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEditorInternal;
using System.Runtime.CompilerServices;

public class RendererLayerEditor
{
    public static void Register()
    {
        //UnityEditor.MeshRendererEditor
        // MeshRendererEditor
        Type type = typeof(AssetDatabase).Assembly.GetType("UnityEditor.MeshRendererEditor");
        MethodInfo method = type.GetMethod("OnInspectorGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        type = typeof(RendererLayerEditor);
        MethodInfo methodReplacement = type.GetMethod("SubRendererOnInspectorGUI", BindingFlags.Static | BindingFlags.NonPublic);
        MethodInfo methodProxy = type.GetMethod("SubRendererOnInspectorGUIProxy", BindingFlags.Static | BindingFlags.NonPublic);
        MethodHook hooker = new MethodHook(method, methodReplacement, methodProxy);
        hooker.Install();

        // SkinnedMeshRendererEditor
        type = typeof(AssetDatabase).Assembly.GetType("UnityEditor.SkinnedMeshRendererEditor");
        method = type.GetMethod("OnInspectorGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        type = typeof(RendererLayerEditor);
        methodReplacement = type.GetMethod("SubRendererOnInspectorGUI", BindingFlags.Static | BindingFlags.NonPublic);
        methodProxy = type.GetMethod("SubRendererOnInspectorGUIProxyE", BindingFlags.Static | BindingFlags.NonPublic);
        hooker = new MethodHook(method, methodReplacement, methodProxy);
        hooker.Install();
    }

    static void SubRendererOnInspectorGUI(Editor editor)
    {
        //Debug.Log(editor.target);
        if (editor.target == null)
        {
            return;
        }
        

        var renderer = editor.target as Renderer;

        FieldInfo info = editor.GetType().BaseType.GetField("m_SortingOrder", BindingFlags.NonPublic | BindingFlags.Instance);
        SerializedProperty sp = info.GetValue(editor) as SerializedProperty;
        FieldInfo info1 = editor.GetType().BaseType.GetField("m_SortingLayerID", BindingFlags.NonPublic | BindingFlags.Instance);
        SerializedProperty sp1 = info1.GetValue(editor) as SerializedProperty;

        var options = RendererLayerEditor.GetSortingLayerNames();
        var picks = new int[options.Length];
        //Debug.Log($"renderer.sortingLayerName:{renderer.sortingLayerName},options.len:{options.Length}");
        var name = renderer.sortingLayerName;
        var choice = -1;
        for (int i = 0; i < options.Length; i++)
        {
            picks[i] = i;
            if (name == options[i]) choice = i;
        }
        if(!sp1.hasMultipleDifferentValues)
        {
            choice = EditorGUILayout.IntPopup("Sorting Layer", choice, options, picks);

            string oldLayerName = renderer.sortingLayerName;
            string newLayerName = options[choice];
            renderer.sortingLayerName = newLayerName;
            ///多重编辑
            if(oldLayerName!=newLayerName)
            {
                foreach (var item in editor.targets)
                {
                    var renderer1 = item as Renderer;
                    renderer1.sortingLayerName = newLayerName;
                    EditorUtility.SetDirty(renderer1);
                }
            }
        }

        if(!sp.hasMultipleDifferentValues&& !sp1.hasMultipleDifferentValues)
        {
            int oldOrder = renderer.sortingOrder;
            int newOrder = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);
            ///多重编辑
            if (oldOrder != newOrder)
            {
                foreach (var item in editor.targets)
                {
                    var renderer1 = item as Renderer;
                    renderer1.sortingOrder = newOrder;
                    EditorUtility.SetDirty(renderer1);
                }
            }
        }


        if (editor.target is MeshRenderer)
        {
            SubRendererOnInspectorGUIProxy(editor);
        }
        else
        {
            SubRendererOnInspectorGUIProxyE(editor);
        }




    }
    [MethodImpl(MethodImplOptions.NoOptimization)]
    static void SubRendererOnInspectorGUIProxy(Editor editor)
    {
        // 随便乱写点东西以占据空间
        for (int i = 0; i < 100; i++)
        {
            UnityEngine.Debug.Log("something");
        }
        UnityEngine.Debug.Log(Application.targetFrameRate);
    }
    [MethodImpl(MethodImplOptions.NoOptimization)]
    static void SubRendererOnInspectorGUIProxyE(Editor editor)
    {
        // 随便乱写点东西以占据空间
        for (int i = 0; i < 100; i++)
        {
            UnityEngine.Debug.Log("something");
        }
        UnityEngine.Debug.Log(Application.targetFrameRate);
    }
    public static string[] GetSortingLayerNames()
    {
        //SortingLayer.layers
        Type internalEditorUtilityType = typeof(InternalEditorUtility);
        PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
        return (string[])sortingLayersProperty.GetValue(null, new object[0]);
    }
}
