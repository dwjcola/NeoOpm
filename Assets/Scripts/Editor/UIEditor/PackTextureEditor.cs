using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

public class PackTextureEditor : AssetPostprocessor
{
    private static string _atlasPath = "Assets/Resource_MS/UI/UIAtlas";
    private static string _texturePath = "Assets/Resource_MS/UISprites";

    
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
    public void OnPreprocessTexture()
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
    }
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
