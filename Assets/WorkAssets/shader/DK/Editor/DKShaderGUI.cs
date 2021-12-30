#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


internal class DKShaderGUI : ShaderGUI
{
    private static class Styles {
        public static GUIContent ScreenLight = new GUIContent("边缘光方向", "基于相机空间的边缘光，x y z 控制灯光朝向，w 控制灯光强度");
        public static GUIContent MainTex = new GUIContent("固有色", "固有色贴图");
        public static GUIContent LightTex = new GUIContent("亮色", "由美术指定的亮部颜色的贴图");
        public static GUIContent DarkTex = new GUIContent("暗色", "由美术指定的暗部颜色的贴图 拖动可提亮");
        public static GUIContent DarkSub = new GUIContent("压暗", "压暗暗部贴图颜色");
        public static GUIContent SpecularColor = new GUIContent("高光颜", "控制高光的颜色和内描AO高光中的B通道的遮罩配合使用 本项目中主要用于金属类物品的高光反射");
        public static GUIContent SpecularGloss = new GUIContent("光泽度", "控制高光的扩散范围，值越高高光越锐利");
        public static GUIContent SpecularSmooth = new GUIContent("柔和度", "控制高光的柔和度，值越高高光越柔和");
        public static GUIContent ScanLineColor = new GUIContent("流光", "流光的颜色");
        public static GUIContent ScanLineTex = new GUIContent("控制图", "R通道为流光的遮罩层 G通道为流光的样式线 B通道为反射属性的遮罩层（无反射可忽略） ");
        public static GUIContent ScanSpeed = new GUIContent("速度", "流光的速度");
        public static GUIContent InnerLineAoSpecMask = new GUIContent("控制图", "（R）通道为内描边信息 （G）通道为AO信息 （B）通道为高光遮罩层 和高光选项配合使用");
        public static GUIContent AoScale = new GUIContent("AO强度", "AO强度");
        public static GUIContent InnerLineAdd = new GUIContent("内描光照影响", "光照时 可以将内描边变淡多少 值越越淡");
        public static GUIContent LightRampTex = new GUIContent("控制图", "勾选时（R）有效，否则（R）无效，（G）通道一直有效，" +
            "（R）通道控制漫反射灯光明暗范围（颜色结果乘以了2即百分之五十灰为强度1显示固有色，白色为强度2 会显示亮色或提亮固有色） （G）通道控制高光范围");
        public static GUIContent ShadeSoft = new GUIContent("灯光柔和度", "禁用灯光控制图时有效，值越大灯光过渡效果越明显");
        public static GUIContent Shadow = new GUIContent("阴影", "启用阴影接受");
        public static GUIContent UseRamp = new GUIContent("灯光控制图", "启用灯光控制图");
        public static GUIContent UseSpecular = new GUIContent("高光", "启用高光");
        public static GUIContent UseReflect = new GUIContent("反射", "启用反射属性");
        public static GUIContent ReflectTex = new GUIContent("贴图", "反射用环境图 CUBE贴图");
        public static GUIContent OutLineCol = new GUIContent("颜色", "外描边颜色");
        public static GUIContent OutLineMinWidth = new GUIContent("最细", "线最粗到多少");
        public static GUIContent OutLineMaxWidth = new GUIContent("最粗", "线最细到多少");
        public static GUIContent MaxCameraDistance = new GUIContent("相机最远", "相机的最远距离");
        public static GUIContent MinCameraDistance = new GUIContent("相机最近", "相机的最近距离");
    }

    float LeftSpace = 20;
    float TopSpace = 5;

    MaterialProperty ScreenLight = null;
    MaterialProperty MainTex = null;
    MaterialProperty LightTex = null;
    MaterialProperty DarkTex = null;
    MaterialProperty DarkAdd = null;
    MaterialProperty DarkSub = null;
    MaterialProperty SpecularCol = null;
    MaterialProperty SpecularGloss = null;
    MaterialProperty SpecularSmooth = null;
    MaterialProperty ScanLineCol = null;
    MaterialProperty ScanTex = null;
    MaterialProperty ScanSpeed = null;
    MaterialProperty LineAoSpecPro = null;
    MaterialProperty AoScale = null;
    MaterialProperty LineAdd = null;
    MaterialProperty LindScale = null;
    MaterialProperty LightRampTex = null;
    MaterialProperty ShadeSoft = null;
    MaterialProperty bShadow = null;
    MaterialProperty bRamp = null;
    MaterialProperty bSpecular = null;
    MaterialProperty bReflect = null;
    MaterialProperty ReflectTex = null;
    MaterialProperty OutLineCol = null;
    MaterialProperty OutLineMin = null;
    MaterialProperty OutLineMax = null;
    MaterialProperty CameraMaxDis = null;
    MaterialProperty CameraMinDis = null;

    MaterialEditor m_MaterialEditor;
    bool m_FirstTimeApply = true;
    bool useReflect = true;
    bool useShadow = true;
    bool useAniso = true;

    bool foldMainTex = true;
    bool foldLight = true;
    bool foldInner = true;
    bool foldScan = true;
    bool foldOutline = true;

    GUIStyle LabelStyle = new GUIStyle();

    public void FindProperties(MaterialProperty[] props)
    {
        try
        {
            ScreenLight = FindProperty("_ScreenLight", props);
            MainTex = FindProperty("_MainTex", props);
            LightTex = FindProperty("_LightTex", props);
            DarkTex = FindProperty("_DarkTex", props);
            DarkAdd = FindProperty("_DarkAdd", props);
            DarkSub = FindProperty("_DarkSub", props);
            SpecularCol = FindProperty("_SpecularColor", props, true);
            SpecularGloss = FindProperty("_SpecularGloss", props);
            SpecularSmooth = FindProperty("_SpecularSmooth", props);
            ScanLineCol = FindProperty("_ScanLineColor", props);
            ScanTex = FindProperty("_ScanTex", props);
            ScanSpeed = FindProperty("_ScanLineSpeed", props);
            LineAoSpecPro = FindProperty("_SpecAndAOTexAndInnerLineColorTex", props);
            AoScale = FindProperty("_AoScale", props);
            LineAdd = FindProperty("_InnerLineMaxAdd", props);
            LindScale = FindProperty("_InnerLineScale", props);
            LightRampTex = FindProperty("_LightRampTex", props);
            ShadeSoft = FindProperty("_ShadeSoft", props);
            bRamp = FindProperty("_UseRampR", props);
            ReflectTex = FindProperty("_ReflectTex", props);
            OutLineCol = FindProperty("_OutlineColor", props);
            OutLineMin = FindProperty("_OutlineMinWidth", props);
            OutLineMax = FindProperty("_OutlineMaxWidth", props);
            CameraMaxDis = FindProperty("_MaxCameraDistance", props);
            CameraMinDis = FindProperty("_MinCameraDistance", props);
        }
        catch
        {
            Debug.Log("lost property");
        }
    }
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        
        FindProperties(props);
        m_MaterialEditor = materialEditor;
        Material material = materialEditor.target as Material;

        ShaderPropertiesGUI(material);
    }

    public override void OnClosed(Material material)
    {
        material.SetShaderPassEnabled("Always", true);
    }

    void ShaderPropertiesGUI(Material material)
    {
        EditorGUIUtility.labelWidth = 0f;
        EditorGUI.BeginChangeCheck();
        {
            foldMainTex= EditorGUILayout.Foldout(foldMainTex,"颜色图",true, LabelStyle);
            if (foldMainTex)
            {
                MainTexGui();
            }
            GUILayout.Space(TopSpace);
            foldLight = EditorGUILayout.Foldout(foldLight, "光照设置", true, LabelStyle);
            if (foldLight)
            {
                Light();
                
            }
            GUILayout.Space(TopSpace);
            foldInner = EditorGUILayout.Foldout(foldInner, "内描边（R）AO（G）高光遮罩（B）", true, LabelStyle);
            if (foldInner)
            {
                LineAoSpec();
            }
            if(ScanTex!=null)
            {
                GUILayout.Space(TopSpace);
                foldScan = EditorGUILayout.Foldout(foldScan, "流光及反射效果", true,LabelStyle);
                if (foldScan)
                {
                    ScanLine();
                }
                
            }
            GUILayout.Space(TopSpace);
            foldOutline = EditorGUILayout.Foldout(foldOutline, "外描边", true,LabelStyle);
            if (foldOutline)
            {
                OutLine();
            }
            
        }
        if (EditorGUI.EndChangeCheck())
        {
        }
        EditorGUILayout.Space();
    }

    void MainTexGui() {

        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.TexturePropertySingleLine(Styles.MainTex, MainTex);
        m_MaterialEditor.TexturePropertySingleLine(Styles.LightTex, LightTex);
        m_MaterialEditor.TexturePropertyTwoLines(Styles.DarkTex, DarkTex, DarkAdd, Styles.DarkSub, DarkSub);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

    }

    void OutLine()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.ColorProperty(OutLineCol, Styles.OutLineCol.text);
        m_MaterialEditor.RangeProperty(OutLineMin, "最细");
        m_MaterialEditor.RangeProperty(OutLineMax, "最粗");
        m_MaterialEditor.RangeProperty(CameraMinDis, "相机最小距离");
        m_MaterialEditor.RangeProperty(CameraMaxDis, "相机最大距离");
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void LineAoSpec()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.TexturePropertySingleLine(Styles.InnerLineAoSpecMask, LineAoSpecPro);
        m_MaterialEditor.RangeProperty(AoScale, "AO强度");
        m_MaterialEditor.RangeProperty(LineAdd, "内描边受光影响程度");
        m_MaterialEditor.RangeProperty(LindScale, "内描边强弱");
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void Light()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.TexturePropertySingleLine(Styles.LightRampTex, LightRampTex, bRamp);
        m_MaterialEditor.RangeProperty(ShadeSoft, "灯光柔和度");
        m_MaterialEditor.ColorProperty(SpecularCol,"高光颜色");
        m_MaterialEditor.RangeProperty(SpecularSmooth,"高光平滑");
        m_MaterialEditor.RangeProperty(SpecularGloss,"高光光泽");
        var lightInfo=ScreenLight.vectorValue;
        var dir = new Vector3(lightInfo.x, lightInfo.y, lightInfo.z);
        dir=EditorGUILayout.Vector3Field("边缘方向",dir);
        lightInfo.w = EditorGUILayout.Slider("边缘强度",lightInfo.w,0,2);
        lightInfo.x= dir.x;
        lightInfo.y = dir.y;
        lightInfo.z = dir.z;
        ScreenLight.vectorValue = lightInfo;
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void ScanLine()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.TexturePropertySingleLine(Styles.ScanLineTex, ScanTex, ScanLineCol);
        var vect = new Vector2(ScanSpeed.vectorValue.x, ScanSpeed.vectorValue.y);
        vect=EditorGUILayout.Vector2Field("流光速度",vect);
        var dirSpeed = new Vector4(vect.x, vect.y,0,0);
        ScanSpeed.vectorValue = dirSpeed;
        m_MaterialEditor.TexturePropertySingleLine(Styles.ReflectTex,ReflectTex);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void tmp()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
}
#endif