using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class PackTextureEditor : AssetPostprocessor
{
    private static string _atlasPath = "Assets/Resource_MS/UI/UIAtlas";
    private static string _texturePath = "Assets/Resource_MS/UISprites";
    private static string _uiPath = "Assets/Resource_MS/UI";
    
    static void PackAtlasContents(string texturePath,string atlasName)
    {
        SpriteAtlas atlas = new SpriteAtlas();
        SpriteAtlasPackingSettings packSetting = new SpriteAtlasPackingSettings()
        {
            blockOffset = 1,
            enableRotation = false,
            enableTightPacking = false,
            padding = 2,
        };
        atlas.SetPackingSettings(packSetting);

        SpriteAtlasTextureSettings textureSetting = new SpriteAtlasTextureSettings()
        {
            readable = false,
            generateMipMaps = false,
            sRGB = true,
            filterMode = FilterMode.Bilinear,
        };
        atlas.SetTextureSettings(textureSetting);

        TextureImporterPlatformSettings platformSetting = new TextureImporterPlatformSettings()
        {
            maxTextureSize = 2048,
            format = TextureImporterFormat.Automatic,
            crunchedCompression = true,
            textureCompression = TextureImporterCompression.Compressed,
            compressionQuality = 50,
        };
        atlas.SetPlatformSettings(platformSetting);

        AssetDatabase.CreateAsset(atlas, $"{_atlasPath}/{atlasName}.spriteatlas");
        
        /*// 1、添加文件
        DirectoryInfo dir = new DirectoryInfo(texturePath);
        FileInfo[] files = dir.GetFiles("*.png");
        foreach (FileInfo file in files)
        {
            atlas.Add(new[] {AssetDatabase.LoadAssetAtPath<Sprite>($"{texturePath}/{file.Name}")});
        }*/

        // 2、添加文件夹
        Object obj = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Object));
        atlas.Add(new[] {obj});
        AssetDatabase.SaveAssets();
    }
    [MenuItem("Assets/PackSelectAtlas")]
    static void PackSelectAtlas()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        PackAtlasContents(path, Selection.activeObject.name);
    }
    [MenuItem("Assets/PackAllAtlas")]
    static void SetSelectAtlas2()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        DirectoryInfo dir = new DirectoryInfo(path);
        foreach (DirectoryInfo d in dir.GetDirectories())
        {
            PackAtlasContents($"{path}/{d.Name}", d.Name);
        }
    }
    [MenuItem("Tools/AutoPackAllAtlas")]
    static void aaa()
    {
        DirectoryInfo dir = new DirectoryInfo(_texturePath);
        foreach (DirectoryInfo d in dir.GetDirectories())
        {
            PackAtlasContents($"{_texturePath}/{d.Name}", d.Name);
        }

        SetAtlasFile();
    }
    /*public void OnPreprocessTexture()
    {
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.mipmapEnabled = false;
    }

    public static void SetTextureType()
    {
        Object[] textrues = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
        int progress = 0;
        float secs = textrues.Length;
        foreach (Texture2D texture in textrues)
        {
            EditorUtility.DisplayProgressBar("Set Texture Format...", texture.name, (float)(++progress / secs));
            string path = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImport = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImport.textureType == TextureImporterType.Sprite)
                continue;
            textureImport.textureType = TextureImporterType.Sprite;
            AssetDatabase.ImportAsset(path);
        }
        EditorUtility.ClearProgressBar();
    }*/

    #region Import Setting
    /// <summary>
    /// Android：不带透明通道压缩成ETC1，带透明通道压缩成ETC2，不被4整除fall back到RGBA32
    ///IOS: 不带透明通道压缩成RGB PVRTC，带透明通道压缩成RGBA PVRTC ，不是2的整数次幂fall back到RGBA32
    ///Texture图片拖入的时候，默认设置ToNerest，这样会自动保证Android平台下图片被4整除，IOS平台下图片是2的整除次幂。
    /// </summary>
    void OnPreprocessTexture()
        {
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer != null)
            {
                if (IsFirstImport(importer))
                {
                    if (assetPath.Contains(_uiPath))
                    {
                        importer.textureType = TextureImporterType.Sprite;
                    }
                    else
                    {
                        importer.textureType = TextureImporterType.Default;
                    }

                    importer.mipmapEnabled = false;
                    importer.isReadable = false;
                    TextureImporterPlatformSettings settings = importer.GetPlatformTextureSettings("iPhone");
                    bool isPowerOfTwo = IsPowerOfTwo(importer);
                    TextureImporterFormat defaultAlpha = isPowerOfTwo ? TextureImporterFormat.PVRTC_RGBA4 : TextureImporterFormat.ASTC_4x4;
                    TextureImporterFormat defaultNotAlpha = isPowerOfTwo ? TextureImporterFormat.PVRTC_RGB4 : TextureImporterFormat.ASTC_6x6;
                    settings.overridden = true;
                    settings.format = importer.DoesSourceTextureHaveAlpha() ? defaultAlpha : defaultNotAlpha;
                    importer.SetPlatformTextureSettings(settings);
     
                    settings = importer.GetPlatformTextureSettings("Android");
                    settings.overridden = true;
                    settings.allowsAlphaSplitting = false;
                    bool divisible4 = IsDivisibleOf4(importer);
                    defaultAlpha = divisible4 ? TextureImporterFormat.ETC2_RGBA8Crunched : TextureImporterFormat.ASTC_4x4;
                    defaultNotAlpha = divisible4 ? TextureImporterFormat.ETC_RGB4Crunched : TextureImporterFormat.ASTC_6x6;
                    settings.format = importer.DoesSourceTextureHaveAlpha() ? defaultAlpha : defaultNotAlpha;
                    importer.SetPlatformTextureSettings(settings);
                }
            }
        }
        //被4整除
        bool IsDivisibleOf4(TextureImporter importer)
        {
            (int width, int height) = GetTextureImporterSize(importer);
            return (width % 4 == 0 && height % 4 == 0);
        }
     
        //2的整数次幂
        bool IsPowerOfTwo(TextureImporter importer)
        {
            (int width, int height) = GetTextureImporterSize(importer);
            return (width == height) && (width > 0) && ((width & (width - 1)) == 0);
        }
     
        //贴图不存在、meta文件不存在、图片尺寸发生修改需要重新导入
        bool IsFirstImport(TextureImporter importer)
        {
            (int width, int height) = GetTextureImporterSize(importer);
            Texture tex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            bool hasMeta = File.Exists(AssetDatabase.GetAssetPathFromTextMetaFilePath(assetPath));
            return tex == null || !hasMeta || (tex.width != width && tex.height != height);
        }
     
        //获取导入图片的宽高
        (int, int) GetTextureImporterSize(TextureImporter importer)
        {
            if (importer != null)
            {
                object[] args = new object[2];
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);
                return ((int)args[0], (int)args[1]);
            }
            return (0, 0);
        }
        static bool forceImport = false;
        //放入UI目录得图强制更新
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string str in movedAssets)
            {
                if (str.Contains("UI保存路径"))
                {
                    forceImport = true;             //重新reimport并且强制 IsFirstImport
                    AssetDatabase.ImportAsset(str); // 如果是移动到指定UI目录下，需要重新Import
                }
            }
        }

    #endregion
    //生成图集配置文件
    [MenuItem("Tools/SetAtlasFile")]
    static void SetAtlasFile()
    {
        string dataPath = Application.dataPath + "/Resource_MS/LuaScripts/tableRead";
        string outfile = dataPath + "/TAtlas.txt";
        if (File.Exists(outfile))
        {
            List<string> lines = new List<string>();
            lines.Add("TAtlas=TblBase:new({ ");
            DirectoryInfo dir = new DirectoryInfo(_texturePath);
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                string atlasName = d.Name;
                FileInfo[] files = d.GetFiles();
                string fileName = "";
                string ext = "";
                foreach (var file in files)
                {
                    ext = Path.GetExtension(file.Name);
                    if (ext.Equals("meta"))
                    {
                        continue;
                    }
                    fileName = Path.GetFileNameWithoutExtension(file.Name);
                    
                    lines.Add("['"+fileName+"']='"+atlasName+"',");
                }
            }
            lines.Add("});");
            lines.Add("return TAtlas;");
            File.WriteAllLines(outfile,lines.ToArray());
            AssetDatabase.Refresh();
        }
    }
}
