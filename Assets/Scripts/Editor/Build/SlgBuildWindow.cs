using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Rendering;

public class SlgBuildWindow : EditorWindow
{
    string[] arch = new string[] { };
   public static void Init()
    {
        SlgBuildWindow window = GetWindow<SlgBuildWindow>();
        window.Show();
    }
    bool OpenGLES3, Vulkan = false;
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        JenkinsAdapter._isDebug = GUILayout.Toggle(JenkinsAdapter._isDebug, "是否开启debug模式");
        if (JenkinsAdapter._isDebug)
        {
            JenkinsAdapter.buildOptions =(BuildOptions) EditorGUILayout.EnumFlagsField("选择打包的附加操作(DEBUG模式、连接profiler等)", JenkinsAdapter.buildOptions);
        }
        else
        {
            JenkinsAdapter.buildOptions = BuildOptions.None;
        }
        JenkinsAdapter._il2cpp= GUILayout.Toggle(JenkinsAdapter._il2cpp, "是否使用IL2Cpp打包");
        JenkinsAdapter._mutelog = GUILayout.Toggle(JenkinsAdapter._mutelog, "是否开启Log");
        JenkinsAdapter._opengm = GUILayout.Toggle(JenkinsAdapter._opengm, "是否开启GM（暂时无用）");
        JenkinsAdapter._rebuildAB = GUILayout.Toggle(JenkinsAdapter._rebuildAB, "是否重新打bundle");
        JenkinsAdapter._updateAb = GUILayout.Toggle(JenkinsAdapter._updateAb, "是否打更新bundle");
        JenkinsAdapter._architecture = EditorGUILayout.EnumPopup("选择打包平台架构", (AndroidArchitecture)Enum.Parse(typeof(AndroidArchitecture), JenkinsAdapter._architecture)).ToString();
        JenkinsAdapter._autoGraphic=GUILayout.Toggle(JenkinsAdapter._autoGraphic, "自动选择渲染API");
        JenkinsAdapter.buildTarget =(BuildTarget) EditorGUILayout.EnumPopup("选择打包平台",JenkinsAdapter.buildTarget);
        if (!JenkinsAdapter._autoGraphic)
        {
           if (OpenGLES3 = GUILayout.Toggle(OpenGLES3, "OpenGLES3"))
            {
                if (!JenkinsAdapter._graphicAPIs.Contains(GraphicsDeviceType.OpenGLES3))
                {
                    JenkinsAdapter._graphicAPIs.Add(GraphicsDeviceType.OpenGLES3);
                }
            }
            else
            {
                if (JenkinsAdapter._graphicAPIs.Contains(GraphicsDeviceType.OpenGLES3))
                {
                    JenkinsAdapter._graphicAPIs.Remove(GraphicsDeviceType.OpenGLES3);
                }
            }
            if (Vulkan = GUILayout.Toggle(Vulkan, "Vulkan"))
            {
                if (!JenkinsAdapter._graphicAPIs.Contains(GraphicsDeviceType.Vulkan))
                {
                    JenkinsAdapter._graphicAPIs.Add(GraphicsDeviceType.Vulkan);
                }
            }
            else
            {
                if (JenkinsAdapter._graphicAPIs.Contains(GraphicsDeviceType.Vulkan))
                {
                    JenkinsAdapter._graphicAPIs.Remove(GraphicsDeviceType.Vulkan);
                }
            }        }
        if (GUILayout.Button("开始打包"))
        {
            JenkinsAdapter.BuildApp();
        }
        EditorGUILayout.EndVertical();

    }
}
