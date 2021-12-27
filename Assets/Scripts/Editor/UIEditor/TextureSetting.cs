using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using Object = UnityEngine.Object;

/// <summary>
/// 批量图片资源导入设置
/// 使用说明： 选择需要批量设置的文件夹或贴图，
/// 单击Tools/Texture Import Settings，
/// 打开窗口后选择对应参数，
/// 点击Set Texture ImportSettings，
/// 稍等片刻，--批量设置成功。
/// 
/// by dwj
/// </summary>


public class TextureSetting : EditorWindow
{

    /// <summary>
    /// 临时存储int[]
    /// </summary>
    private int[] IntArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
    //AnisoLevel
    [SerializeField]
    private static int AnisoLevel = 1;

    //Filter Mode
    private static FilterMode mFilterMode = FilterMode.Point;
    //Wrap Mode
    private static TextureWrapMode mTextureWrapMode = TextureWrapMode.Clamp;
    //Texture Type
    private static TextureImporterType TextureType = TextureImporterType.Default;
    //Max Size
    private static int MaxSizeInt = 5;
    private string[] MaxSizeString = new string[] { "32", "64", "128", "256", "512", "1024", "2048", "4096" };
    //Format
    private static TextureImporterFormat ImporterFormat = TextureImporterFormat.AutomaticCompressed;

    private static TextureImporterMipFilter mTextureImporterMipFilter = TextureImporterMipFilter.BoxFilter;

    private static bool ReadAndWrite = false;

    private static bool grayscaleToAlpha = false;
    private static bool alphaIsTransparency = false;

    private static bool Mipmaps = false;

    private static bool InLinearSpace = false;

    private static bool BorderMapMaps = false;

    private static bool FadeoutMapMaps = false;

    private static bool OverrideOnAndroid = false;
    private static bool OverrideOnIOS = false;
    private static int IOSMaxSizeInt = 5;
    private static int AndroidMaxSizeInt = 5;

    private static TextureImporterFormat IOSImporterFormat = TextureImporterFormat.AutomaticCompressed;
    private static TextureImporterFormat AndroidImporterFormat = TextureImporterFormat.AutomaticCompressed;

    /// <summary>
    /// 创建、显示窗体
    /// </summary>
    [MenuItem("Tools/Texture Import Settings")]
    private static void Init()
    {
        TextureSetting window = (TextureSetting)EditorWindow.GetWindow(typeof(TextureSetting), true, "TextureImportSetting");
        window.Show();
    }

    //string Cur2DtexturePath = "";
    /// <summary>
    /// 显示窗体里面的内容
    /// </summary>
    private void OnGUI()
    {
        GUILayout.Label("please select folder or Texture");


        ReadAndWrite = GUILayout.Toggle(ReadAndWrite, "Read/Write");

        //Texture Type
        TextureType = (TextureImporterType)EditorGUILayout.EnumPopup("Texture Type", TextureType);
        grayscaleToAlpha = GUILayout.Toggle(grayscaleToAlpha, "Alpha from Grayscale");
        alphaIsTransparency = GUILayout.Toggle(alphaIsTransparency, "Alpha Is Transparency");

        //Wrap Mode
        mTextureWrapMode = (TextureWrapMode)EditorGUILayout.EnumPopup("Wrap Mode", mTextureWrapMode);

        Mipmaps = GUILayout.Toggle(Mipmaps, "Mipmaps");
        if (Mipmaps)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            InLinearSpace = GUILayout.Toggle(InLinearSpace, "In Linear Space");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            BorderMapMaps = GUILayout.Toggle(BorderMapMaps, "Border Map Maps");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            //Filter Mode
            mTextureImporterMipFilter = (TextureImporterMipFilter)EditorGUILayout.EnumPopup("Mip Map Filtering", mTextureImporterMipFilter);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            FadeoutMapMaps = GUILayout.Toggle(FadeoutMapMaps, "Fadeout Map Maps");
            GUILayout.EndHorizontal();
        }

        //Filter Mode
        mFilterMode = (FilterMode)EditorGUILayout.EnumPopup("Filter Mode", mFilterMode);

        //Max Size
        MaxSizeInt = EditorGUILayout.IntPopup("Max Size", MaxSizeInt, MaxSizeString, IntArray);
        //Format
        ImporterFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", ImporterFormat);
        //AnisoLevel
        GUILayout.BeginHorizontal();
        GUILayout.Label("Aniso Level  ");
        AnisoLevel = EditorGUILayout.IntSlider(AnisoLevel, 0, 9);
        GUILayout.EndHorizontal();

        OverrideOnAndroid = GUILayout.Toggle(OverrideOnAndroid, "Override for Android");
        if (OverrideOnAndroid)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            //Max Size
            AndroidMaxSizeInt = EditorGUILayout.IntPopup("Max Size", AndroidMaxSizeInt, MaxSizeString, IntArray);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            AndroidImporterFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", AndroidImporterFormat);
            GUILayout.EndHorizontal();

        }
        OverrideOnIOS = GUILayout.Toggle(OverrideOnIOS, "Override for IOS");
        if (OverrideOnIOS)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            //Max Size
            IOSMaxSizeInt = EditorGUILayout.IntPopup("Max Size", IOSMaxSizeInt, MaxSizeString, IntArray);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            //Format
            IOSImporterFormat = (TextureImporterFormat)EditorGUILayout.EnumPopup("Format", IOSImporterFormat);
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Set Texture ImportSettings"))
            LoopSetTexture();
    }

    //string SelectPath;

    /// <summary>
    /// 获取贴图设置
    /// </summary>
    public TextureImporter GetTextureSettings(string path)
    {
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        textureImporter.isReadable = ReadAndWrite;

        textureImporter.mipmapEnabled = Mipmaps;
        textureImporter.alphaIsTransparency = alphaIsTransparency;
        textureImporter.grayscaleToAlpha = grayscaleToAlpha;

        if (textureImporter.mipmapEnabled)
        {
            textureImporter.borderMipmap = BorderMapMaps;
            textureImporter.fadeout = FadeoutMapMaps;
            textureImporter.generateMipsInLinearSpace = InLinearSpace;
            textureImporter.mipmapFilter = mTextureImporterMipFilter;
        }
        //AnisoLevel
        textureImporter.anisoLevel = AnisoLevel;
        //Filter Mode
        textureImporter.filterMode = mFilterMode;

        //Wrap Mode
        textureImporter.wrapMode = mTextureWrapMode;

        //Texture Type
        textureImporter.textureType = TextureType;
        //Max Size 
        textureImporter.maxTextureSize = Convert.ToInt32(MaxSizeString[MaxSizeInt]);
        //Format
        textureImporter.textureFormat = ImporterFormat;


        if (OverrideOnAndroid)
        {
            int AndroidMaxSize = Convert.ToInt32(MaxSizeString[AndroidMaxSizeInt]);

            textureImporter.SetPlatformTextureSettings("Android", AndroidMaxSize, AndroidImporterFormat);
        }
        else
        {
            textureImporter.ClearPlatformTextureSettings("Android");
        }

        if (OverrideOnIOS)
        {
            textureImporter.SetPlatformTextureSettings("iPhone", Convert.ToInt32(MaxSizeString[IOSMaxSizeInt]), IOSImporterFormat);
        }
        else
        {
            textureImporter.ClearPlatformTextureSettings("iPhone");
        }

        return textureImporter;
    }


    /// <summary>
    /// 循环设置选择的贴图
    /// </summary>
    private void LoopSetTexture()
    {
        Object objSelected = Selection.activeObject;
        if (objSelected != null)
        {
            Object[] textures = GetSelectedTextures();
            Selection.objects = new Object[0];
            foreach (Texture2D texture in textures)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                SetTexture(path);

            }

        }
    }


    private void SetTexture(string path)
    {
        TextureImporter texImporter = GetTextureSettings(path);
        TextureImporterSettings tis = new TextureImporterSettings();
        texImporter.ReadTextureSettings(tis);
        texImporter.SetTextureSettings(tis);
        AssetDatabase.ImportAsset(path);
    }
    /// <summary>
    /// 获取选择的贴图
    /// </summary>
    /// <returns></returns>
    private Object[] GetSelectedTextures()
    {
        return Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
    }
}