//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace NeoOPM
{
    public static class AssetUtility
    {
        public const string ResourceRoot = "Assets/Resource_MS/";
      
        public static string GetFontAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/Fonts/{0}.ttf", assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/Scenes/{0}.unity", assetName);
        }
        
        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/UI/UIForms/{0}.prefab", assetName);
        }

 
        public static string GetUIAtalsAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/UI/UIAtlas/{0}.spriteatlas", assetName);
        }
        
        public static string GetSpineAsset(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/UI/UISpines/{0}.prefab", assetName);
        }

        public static string GetSpineAssetBattle(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/Prefabs/Heroes/{0}.prefab", assetName);
        }

        public static string GetBattleScene(string assetName)
        {
            return Utility.Text.Format("Assets/Resource_MS/Prefabs/HeroScenes/{0}.prefab", assetName);
        }
    }
}
