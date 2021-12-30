#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


internal class DKShaderGUIFight : ShaderGUI
{
    private static class Styles
    {
        public static GUIContent MainTex = new GUIContent("固有色", "固有色贴图，若有勾选框 勾选为使用二通道 不勾使用一通道");
        public static GUIContent Alpha = new GUIContent("透明度", "调整整体透明度");
        public static GUIContent DarkTex = new GUIContent("暗色", "由美术指定的暗部颜色的贴图 拖动可提亮");
        public static GUIContent FaceTex = new GUIContent("表情", "表情序列贴图");
        public static GUIContent SpecularColor = new GUIContent("高光颜", "控制高光的颜色和内描AO高光中的B通道的遮罩配合使用 本项目中主要用于金属类物品的高光反射");
        public static GUIContent SpecularGloss = new GUIContent("光泽度", "控制高光的扩散范围，值越高高光越锐利");
        public static GUIContent SpecularSmooth = new GUIContent("柔和度", "控制高光的柔和度，值越高高光越柔和");
        public static GUIContent InnerLineAoSpecMask = new GUIContent("控制图", "（R）通道为内描边信息 （G）通道为AO信息 （B）通道为高光遮罩层 和高光选项配合使用");
        public static GUIContent AoScale = new GUIContent("AO强度", "AO强度");
        public static GUIContent InnerLineAdd = new GUIContent("内描光照影响", "光照时 可以将内描边变淡多少 值越越淡");
        public static GUIContent InnerLineScal = new GUIContent("内描强度", "内描边的强度");
        public static GUIContent LightRampTex = new GUIContent("控制图", "勾选时（R）有效，否则（R）无效，（G）通道一直有效，" +
            "（R）通道控制漫反射灯光明暗范围（颜色结果乘以了2即百分之五十灰为强度1显示固有色，白色为强度2 会显示亮色或提亮固有色） （G）通道控制高光范围");
        public static GUIContent ShadeSoft = new GUIContent("灯光柔和度", "禁用灯光控制图时有效，值越大灯光过渡效果越明显");
        public static GUIContent DiffuseOffset = new GUIContent("漫反射偏移");
        public static GUIContent OutLineCol = new GUIContent("颜色", "外描边颜色");
        public static GUIContent OutLineMinWidth = new GUIContent("最细", "线最粗到多少");
        public static GUIContent OutLineMaxWidth = new GUIContent("最粗", "线最细到多少");
        public static GUIContent MaxCameraDistance = new GUIContent("相机最远", "相机的最远距离");
        public static GUIContent MinCameraDistance = new GUIContent("相机最近", "相机的最近距离");
        public static GUIContent IdMap = new GUIContent("ID贴图", "用于区分换色区域的ID图");
        public static GUIContent ColorMap = new GUIContent("换色图表", "用于换色查询用的颜色图库图片");
        public static GUIContent Index = new GUIContent("配色方案", "默认从0开始计数");
        public static GUIContent ColorOffset = new GUIContent("色表偏移", "偏移色表指针位置");
        public static GUIContent IdNum = new GUIContent("ID数量", "ID贴图有多少ID 很重要的信息 设置错误会造成颜色错乱");
        public static GUIContent ScanMask = new GUIContent("流光遮罩", "R通道流光遮罩，G通道流光线样式,颜色");
        public static GUIContent ScanColor = new GUIContent("流光色", "RGB控制颜色 Alpha控制强弱");
        public static GUIContent ScanSpeed = new GUIContent("流过速度", "xy 控制流光方向 大小控制速度");
        public static GUIContent RimColor = new GUIContent("边缘光色", "RGB控制边缘光颜色，alpha控制强度");
        public static GUIContent RimRange = new GUIContent("边缘光范围", "控制边缘光的范围");
        public static GUIContent RimOffset = new GUIContent("边缘光偏移", "交界线的位置偏移");
        public static GUIContent FresnelShadowRange = new GUIContent("边缘暗部范围", "边缘的暗部的范围");
        public static GUIContent RimTex = new GUIContent("边缘光", "边缘光图片");
        public static GUIContent RimTexColor = new GUIContent("边缘光颜色", "边缘光调整颜色");

        public static GUIContent MainColor = new GUIContent("人物颜色","开启边缘光时人物的颜色");
        public static GUIContent RimColor1 = new GUIContent("边缘光颜色","边缘光颜色调整");
        public static GUIContent RimRange1 = new GUIContent("边缘光范围","边缘光范围调整");
        public static GUIContent RimOffset1 = new GUIContent("边缘光偏移", "交界线的位置偏移");
        public static GUIContent RimSoft = new GUIContent("边缘光软硬度","边缘光软硬度");
        public static GUIContent RimToggle = new GUIContent("边缘光开关", "边缘光开关");
        public static GUIContent XRayColor = new GUIContent("XRayColor", "XRayColor");
    }

    float LeftSpace = 20;
    float TopSpace = 5;

    //textures
    MaterialProperty MainTex = null;
    MaterialProperty DarkTex = null;
    MaterialProperty DarkAdd = null;
    MaterialProperty FaceTex = null;
    MaterialProperty FaceOffset = null;
    //Alpha
    MaterialProperty Alpha = null;
    //IDmap
    MaterialProperty IdMap = null;
    MaterialProperty ColorMap = null;
    MaterialProperty Index = null;
    MaterialProperty ColorOffset = null;
    MaterialProperty IdNum = null;
    // MaterialProperty UseUv2 = null;
    //specular
    MaterialProperty SpecularCol = null;
    MaterialProperty SpecularGloss = null;
    MaterialProperty SpecularSmooth = null;
    //LineAoSpec
    MaterialProperty LineAoSpecPro = null;
    MaterialProperty AoScale = null;
    MaterialProperty LineAdd = null;
    MaterialProperty LindScale = null;
    //Light
    MaterialProperty DiffuseOffset = null;
    MaterialProperty ShadeSoft = null;
    //scanLine
    MaterialProperty ScanMask = null;
    MaterialProperty ScanSpeed = null;
    MaterialProperty ScanColor = null;
    //rimLight
    MaterialProperty RimColor = null;
    MaterialProperty RimRange = null;
    MaterialProperty RimOffset = null;
    //OutLine
    MaterialProperty OutLineCol = null;
    MaterialProperty OutLineMin = null;
    MaterialProperty OutLineMax = null;
    MaterialProperty CameraMaxDis = null;
    MaterialProperty CameraMinDis = null;
    //fresnelshadowRange
    MaterialProperty fresnelshadowRange = null;
    //RimTex
    MaterialProperty RimTex = null;
    MaterialProperty RimTexColor = null;
    //Rim1
    MaterialProperty MainColor = null;
    MaterialProperty RimColor1 = null;
    MaterialProperty RimRange1 = null;
    MaterialProperty RimOffset1 = null;
    MaterialProperty RimSoft = null;
    MaterialProperty RimToggle = null;
    MaterialProperty XRayColor = null;
    
    MaterialEditor m_MaterialEditor;

    bool m_FirstTimeApply = true;
    bool useReflect = true;
    bool useShadow = true;
    bool useAniso = true;

    bool foldRim = true;
    bool foldMainTex = true;
    bool foldLight = true;
    bool foldInner = true;
    bool foldOutline = true;
    bool foldChangeColor = true;
    bool foldScanLine = true;
    bool foldRimFresnel = true;
    bool foldRimLight1 = true;

    bool bHasRimLight = true;
    bool bHasFace = true;
    bool bHasChangeColor = true;
    bool bHasInnerLine = true;
    bool bHasOutLine = true;
    bool bHasAlpha = true;
    bool bHasScanLine = true;
    bool bHasFresnelShadow = true;
    bool bHasRimTex = true;
    bool bHasRimLight1 = true;

    float moveXDis = 0.25f;
    float moveYDis = 0.5f;


    String errWords = "";
    GUIStyle LabelStyle = new GUIStyle();

    public void FindProperties(MaterialProperty[] props)
    {
        MainTex = FindProperty("_MainTex", props);
        DarkTex = FindProperty("_SSSTex", props);

        try
        {
            FaceTex = FindProperty("_SecTex", props);
            bHasFace = true;
        }
        catch
        {
            bHasFace = false;
        }

        try
        {
            Alpha = FindProperty("_Alpha", props);
            bHasAlpha = true;
        }
        catch
        {
            bHasAlpha = false;
        }

        try
        {
            IdMap = FindProperty("_IdMap", props);
            ColorMap = FindProperty("_ColorMap", props);
            Index = FindProperty("_Index", props);
            ColorOffset = FindProperty("_ColorOffset", props);
            IdNum = FindProperty("_IdsNum", props);
            bHasChangeColor = true;
        }
        catch
        {
            bHasChangeColor = false;
        }
        DarkAdd = FindProperty("_darkAdd", props);
        SpecularCol = FindProperty("_SpecColor", props, true);
        SpecularGloss = FindProperty("_Shininess", props);
        SpecularSmooth = FindProperty("_SpecSmooth", props);

        LineAoSpecPro = FindProperty("_SpecAndAOTexAndInnerLineColorTex", props);
        AoScale = FindProperty("_aoScale", props);
        try
        {
            LineAdd = FindProperty("_innerLineMaxAdd", props);
            LindScale = FindProperty("_innerLineScale", props);
        }
        catch
        {
            bHasInnerLine = false;
        }
        ShadeSoft = FindProperty("_shadeSoft", props);
        DiffuseOffset = FindProperty("_diffuseOffset", props);

        try
        {
            OutLineCol = FindProperty("_OutlineColor", props);
            OutLineMin = FindProperty("_OutlineMinWidth", props);
            OutLineMax = FindProperty("_OutlineMaxWidth", props);
            CameraMaxDis = FindProperty("_MaxCameraDistance", props);
            CameraMinDis = FindProperty("_MinCameraDistance", props);
        }
        catch
        {
            bHasOutLine = false;
        }
        try
        {
            ScanMask = FindProperty("_ScanTex", props);
            ScanColor = FindProperty("_ScanLineColor", props);
            ScanSpeed = FindProperty("_ScanLineSpeed", props);
        }
        catch
        {
            bHasScanLine = false;
        }

        try
        {
            RimColor = FindProperty("_FresnelColor", props);
            RimRange = FindProperty("_FresnelRange", props);
            RimOffset = FindProperty("_FresnelOffset", props);        
        }
        catch
        {
            bHasRimLight = false;
        }

        try
        {
            fresnelshadowRange = FindProperty("_fresnelshadowRange", props);
        }
        catch
        {
            bHasFresnelShadow = false;
        }

        try
        {
            RimTex = FindProperty("_RimTex", props);
            RimTexColor = FindProperty("_RimColor", props);
        }
        catch
        {
            bHasRimTex = false;
        }

        try
        {
            MainColor = FindProperty("_MainColor",props);
            RimColor1 = FindProperty("_RimColor1", props);
            RimRange1 = FindProperty("_RimRange1", props);
            RimOffset1 = FindProperty("_RimOffset1", props);
            RimSoft = FindProperty("_RimSoft", props);
            RimToggle = FindProperty("_RimToggle", props);
        }
        catch {
            bHasRimLight1 = false;
        }

        try
        {
            XRayColor = FindProperty("_XRayColor", props);
        }
        catch
        {
        }

    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] props)
    {
        LabelStyle.fontSize = 13;
        LabelStyle.imagePosition = ImagePosition.ImageLeft;
        LabelStyle.richText = true;

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
        errWords = "";

        EditorGUIUtility.labelWidth = 0f;
        if (GUILayout.Button("重置所有参数"))
        {
            resetProps();
        }
        EditorGUI.BeginChangeCheck();

        {
            if (bHasChangeColor)
            {
                CheckChangeColor();
                DrawFold(ref foldChangeColor, "<color=#aaaaaa>换色属性</color>", resetChangeColor);
                if (foldChangeColor)
                {
                    DrawChangeColor();
                }
            }


            CheckMainTex();
            DrawFold(ref foldMainTex, "<color=#aaaaaa>颜色图</color>", resetMainTex);
            if (foldMainTex)
            {
                DrawMainTex();
            }

            GUILayout.Space(TopSpace);
            DrawFold(ref foldLight, "<color=#aaaaaa>光照设置</color>", resetLight);
            if (foldLight)
            {
                DrawLight();
            }


            CheckMaskTex();
            DrawFold(ref foldInner, "<color=#aaaaaa>内描边（R）AO（G）高光遮罩（B）</color>", resetAoLine);
            if (foldInner)
            {
                DrawLineAoSpec();
            }
            if (bHasScanLine)
            {
                GUILayout.Space(TopSpace);
                DrawFold(ref foldScanLine, "<color=#aaaaaa>流光遮罩（R）流光样式（G）</color>", resetScanLine);
                if (foldScanLine)
                {
                    DrawScanLine();
                }
            }
            if (bHasRimTex || bHasFresnelShadow)
            {
                GUILayout.Space(TopSpace);
                DrawFold(ref foldRimFresnel, "<color=#aaaaaa>边缘光与边缘暗部</color>", resetRimFresnel);
                if (foldRimFresnel)
                {
                    if (bHasRimTex)
                    {
                        DrawRimTex();
                    }
                    if (bHasFresnelShadow)
                    {
                        GUILayout.Space(TopSpace);
                        DrawFresnelShadowRange();
                    }
                }
            }
            if (bHasRimLight)
            {
                GUILayout.Space(TopSpace);
                DrawFold(ref foldRim, "<color=#aaaaaa>边缘光</color>", resetRim);
                if (foldRim)
                {
                    DrawRimRange();
                }
            }
            GUILayout.Space(TopSpace);
            DrawFold(ref foldOutline, "<color=#aaaaaa>外描边</color>", resetOutlLine);
            if (foldOutline)
            {
                DrawOutLine();
            }
            if (bHasRimLight1) {
                GUILayout.Space(TopSpace);
                DrawFold(ref foldRimLight1,"<color=#aaaaaa>边缘光</color>",resetRimLight1);
                if (foldRimLight1) {
                    DrawRimLight1();
                }
            }

            if (errWords != "")
            {
                GUILayout.Space(TopSpace);
                GUILayout.Label("<color=#00ffff>警告：</color>", LabelStyle);
                GUI.color = Color.yellow;
                GUILayout.TextArea(errWords);
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
        }
        EditorGUILayout.Space();
    }
    #region ChekcProps
    void CheckChangeColor()
    {
        if (IdMap.textureValue == null)
        {
            errWords += "未指定固ID贴图\n";
        }
        if (ColorMap.textureValue == null)
        {
            errWords += "未指定IdColor贴图\n";
        }
    }
    void CheckMainTex()
    {
        if (MainTex.textureValue == null)
        {
            errWords += "未指定固有色贴图\n";
        }
        if (DarkTex.textureValue == null)
        {
            errWords += "未指定暗色贴图\n";
        }
        if (bHasFace && DarkTex.textureValue == null)
        {
            errWords += "未指定表情贴图\n";
        }
    }
    void CheckMaskTex()
    {
        if (LineAoSpecPro.textureValue == null)
        {
            errWords += "未指定内描边AO高光遮罩贴图\n";
        }
    }

    #endregion
    delegate void callback();
    #region DrawElements
    void DrawChangeColor()
    {
        m_MaterialEditor.TexturePropertySingleLine(Styles.IdMap, IdMap);
        m_MaterialEditor.TexturePropertySingleLine(Styles.ColorMap, ColorMap);

        // index
        GUILayout.BeginHorizontal();
        var num = 0;
        GUILayout.Label(Styles.Index, GUILayout.Width(50));
        num = (int)GUILayout.HorizontalSlider(Index.floatValue, 0, 10000);
        num = EditorGUILayout.IntField(num, GUILayout.Width(30));
        Index.floatValue = num;
        GUILayout.EndHorizontal();
        // offset
        GUILayout.BeginHorizontal();
        num = 0;
        GUILayout.Label(Styles.ColorOffset, GUILayout.Width(50));
        num = (int)GUILayout.HorizontalSlider(ColorOffset.floatValue, 0, 255);
        num = EditorGUILayout.IntField(num, GUILayout.Width(30));
        ColorOffset.floatValue = num;
        GUILayout.EndHorizontal();
        // numbers
        GUILayout.BeginHorizontal();
        num = 0;
        GUILayout.Label(Styles.IdNum, GUILayout.Width(50));
        num = (int)GUILayout.HorizontalSlider(IdNum.floatValue, 1, 255);
        num = EditorGUILayout.IntField(num, GUILayout.Width(30));
        IdNum.floatValue = num;
        GUILayout.EndHorizontal();
    }
    void DrawMainTex()
    {

        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        if (bHasAlpha)
        {
            m_MaterialEditor.RangeProperty(Alpha, Styles.Alpha.text);
        }
        if (bHasFace)
        {
            GUILayout.BeginHorizontal(GUILayout.Height(60));

            GUILayout.BeginVertical();

            m_MaterialEditor.TexturePropertySingleLine(Styles.MainTex, MainTex);
            m_MaterialEditor.TexturePropertySingleLine(Styles.DarkTex, DarkTex);
            m_MaterialEditor.TexturePropertySingleLine(Styles.FaceTex, FaceTex);

            GUILayout.EndVertical();

            DrawChangeExpression();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            var hasMaskValue = true;
            var ts = FaceTex.textureScaleAndOffset;
            GUILayout.Label("X : " + (ts.z * 4 + 1), GUILayout.Width(50));
            GUILayout.Label("Y : " + (ts.w * -2 + 1), GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }
        else
        {
            m_MaterialEditor.TexturePropertySingleLine(Styles.MainTex, MainTex);
            m_MaterialEditor.TexturePropertySingleLine(Styles.DarkTex, DarkTex);
        }

        m_MaterialEditor.RangeProperty(DarkAdd, "暗部灯光增亮");

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    void DrawChangeExpression()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("▲", "up"), GUILayout.Width(40), GUILayout.Height(20)))
        {
            var ts = FaceTex.textureScaleAndOffset;
            ts.w += moveYDis;
            ts.w = Mathf.Clamp(ts.w, moveYDis - 1, 0);
            FaceTex.textureScaleAndOffset = ts;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("◀", "left"), GUILayout.Width(40), GUILayout.Height(20)))
        {
            var ts = FaceTex.textureScaleAndOffset;
            ts.z -= moveXDis;
            ts.z = Mathf.Clamp(ts.z, 0, 1 - moveXDis);
            FaceTex.textureScaleAndOffset = ts;
        }
        //if (GUILayout.Button(new GUIContent("●", "reset"), GUILayout.Width(40), GUILayout.Height(20)))
        //{
        //    var ts = Vector4.zero;
        //    ts.x = 1;
        //    ts.y = 1;
        //    FaceTex.textureScaleAndOffset = ts;
        //}
        if (GUILayout.Button(new GUIContent("▶", "right"), GUILayout.Width(40), GUILayout.Height(20)))
        {
            var ts = FaceTex.textureScaleAndOffset;
            ts.z += moveXDis;
            ts.z = Mathf.Clamp(ts.z, 0, 1 - moveXDis);
            FaceTex.textureScaleAndOffset = ts;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("▼", "down"), GUILayout.Width(40), GUILayout.Height(20)))
        {
            var ts = FaceTex.textureScaleAndOffset;
            ts.w -= moveYDis;
            ts.w = Mathf.Clamp(ts.w, moveYDis - 1, 0);
            FaceTex.textureScaleAndOffset = ts;
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }
    void DrawOutLine()
    {
        if (!bHasOutLine)
            return;
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.ColorProperty(OutLineCol, Styles.OutLineCol.text);
        var lineMin = OutLineMin.floatValue;
        var lineMax = OutLineMax.floatValue;

        GUILayout.BeginHorizontal();
        GUILayout.Label("描边粗细: " + lineMin, GUILayout.Width(82));
        GUILayout.Label("—  " + lineMax, GUILayout.Width(48));
        GUILayout.FlexibleSpace();
        EditorGUILayout.MinMaxSlider(ref lineMin, ref lineMax, 0.01f, 2);
        OutLineMin.floatValue = lineMin;
        OutLineMax.floatValue = lineMax;
        GUILayout.EndHorizontal();

        var minDis = CameraMinDis.floatValue;
        var maxDis = CameraMaxDis.floatValue;
        GUILayout.BeginHorizontal();
        GUILayout.Label("相机距离: " + minDis, GUILayout.Width(82));
        GUILayout.Label("—  " + maxDis, GUILayout.Width(48));
        GUILayout.FlexibleSpace();
        EditorGUILayout.MinMaxSlider(ref minDis, ref maxDis, 1, 200);
        CameraMinDis.floatValue = minDis;
        CameraMaxDis.floatValue = maxDis;
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    void DrawScanLine()
    {
        if (!bHasScanLine)
            return;
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.ColorProperty(ScanColor, Styles.ScanColor.text);
        m_MaterialEditor.TexturePropertySingleLine(Styles.ScanMask, ScanMask);
        var scandir = ScanSpeed.vectorValue;

        scandir.x = EditorGUILayout.Slider("水平方向", scandir.x, -1, 1);
        scandir.y = EditorGUILayout.Slider("垂直方向", scandir.y, -1, 1);
        scandir.z = EditorGUILayout.Slider("流动速度", scandir.z, 0, 100);
        Vector4 newDir = new Vector4(scandir.x, scandir.y, scandir.z, 0);
        ScanSpeed.vectorValue = newDir;

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    void DrawLineAoSpec()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();
        m_MaterialEditor.TexturePropertySingleLine(Styles.InnerLineAoSpecMask, LineAoSpecPro);
        m_MaterialEditor.RangeProperty(AoScale, Styles.AoScale.text);
        if (bHasInnerLine)
        {
            m_MaterialEditor.RangeProperty(LineAdd, Styles.InnerLineAdd.text);
            m_MaterialEditor.RangeProperty(LindScale, Styles.InnerLineScal.text);
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    void DrawLight()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(LeftSpace);
        GUILayout.BeginVertical();

        m_MaterialEditor.RangeProperty(ShadeSoft, Styles.ShadeSoft.text);
        m_MaterialEditor.RangeProperty(DiffuseOffset, Styles.DiffuseOffset.text);
        DrawSpecular();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    void DrawSpecular()
    {
        m_MaterialEditor.ColorProperty(SpecularCol, Styles.SpecularColor.text);
        m_MaterialEditor.RangeProperty(SpecularSmooth, Styles.SpecularSmooth.text);
        m_MaterialEditor.RangeProperty(SpecularGloss, Styles.SpecularGloss.text);
    }
    void DrawRimRange()
    {
        m_MaterialEditor.ColorProperty(RimColor, Styles.RimColor.text);
        m_MaterialEditor.RangeProperty(RimRange, Styles.RimRange.text);
        m_MaterialEditor.RangeProperty(RimOffset, Styles.RimOffset.text);
    }
    void DrawRimTex()
    {
        m_MaterialEditor.ColorProperty(RimTexColor, Styles.RimTexColor.text);
        m_MaterialEditor.TextureProperty(RimTex, Styles.RimTex.text);
    }
    void DrawFresnelShadowRange()
    {
        m_MaterialEditor.RangeProperty(fresnelshadowRange, Styles.FresnelShadowRange.text);
    }
    void DrawRimLight1() {
        m_MaterialEditor.ColorProperty(MainColor, Styles.MainColor.text);
        m_MaterialEditor.ColorProperty(RimColor1, Styles.RimColor1.text);
        m_MaterialEditor.RangeProperty(RimRange1,Styles.RimRange1.text);
        m_MaterialEditor.RangeProperty(RimSoft,Styles.RimSoft.text);
        m_MaterialEditor.RangeProperty(RimOffset1, Styles.RimOffset1.text);
        m_MaterialEditor.ShaderProperty(RimToggle, Styles.RimToggle.text);
        m_MaterialEditor.ShaderProperty(XRayColor, Styles.XRayColor.text);

    }

    void DrawFold(ref bool fold, string texts, callback reset)
    {
        GUILayout.BeginHorizontal();
        fold = EditorGUILayout.Foldout(fold, texts, true, LabelStyle);
        if (GUILayout.Button("重置", GUILayout.Width(40)))
        {
            reset();
        }
        GUILayout.EndHorizontal();
    }
    #endregion

    #region reset

    void resetProps()
    {
        resetMainTex();
        resetLight();
        resetAoLine();
        resetOutlLine();
        resetChangeColor();
        resetRim();
        resetScanLine();
        resetRimFresnel();
    }
    void resetChangeColor()
    {
        if (bHasChangeColor)
        {
            ColorOffset.floatValue = 0;
            Index.floatValue = 0;
        }
    }
    void resetMainTex()
    {
        DarkAdd.floatValue = 0.1f;
        if (bHasFace)
        {
            var ts = Vector4.zero;
            ts.x = 1;
            ts.y = 2;
            FaceTex.textureScaleAndOffset = ts;
        }
    }
    void resetLight()
    {
        ShadeSoft.floatValue = 0;
        SpecularCol.colorValue = Color.white;
        SpecularGloss.floatValue = 0.04f;
        SpecularSmooth.floatValue = 1;
        DiffuseOffset.floatValue = 0;
    }

    void resetScanLine()
    {
        if (!bHasScanLine)
            return;
        ScanColor.colorValue = Color.white;
        ScanSpeed.vectorValue = new Vector4(1, 1, 1, 0);

    }

    void resetRimFresnel()
    {
        if (!bHasRimTex && !bHasFresnelShadow)
            return;
        else
        {
            if (bHasRimTex)
            {
                RimTexColor.colorValue = new Color(1,1,1,0.2f);
                RimTex.textureValue = null;
            }
            if (bHasFresnelShadow)
            {
                fresnelshadowRange.floatValue = -1f;
            }
        }
    }

    void resetAoLine()
    {
        AoScale.floatValue = 2.5f;
        if (bHasInnerLine)
        {
            LineAdd.floatValue = 0;
            LindScale.floatValue = 0.5f;
        }
    }

    void resetOutlLine()
    {
        if (bHasOutLine)
        {
            OutLineCol.colorValue = Color.black;
            OutLineMin.floatValue = 0.2f;
            OutLineMax.floatValue = 0.4f;
            CameraMinDis.floatValue = 66.6f;
            CameraMaxDis.floatValue = 199;
        }
    }

    void resetRimLight1() {
        if (bHasRimLight1)
        {
            MainColor.colorValue = new Color(1,0.98f,0.9725f,1);
            RimColor1.colorValue = new Color (0.8235f,0.19215f,0,1);
            RimRange1.floatValue = 0.3f;
            RimSoft.floatValue = 0;
            RimOffset1.floatValue = -0.027f;
            RimToggle.floatValue = 0;
        }
    }

    void resetRim()
    {
        if (bHasRimLight)
        {
            RimColor.colorValue = Color.white;
            RimRange.floatValue = 8;
            RimOffset.floatValue = 0; 
        }
    }
    #endregion
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