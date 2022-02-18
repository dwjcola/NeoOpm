using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using GameFramework;
using GameFramework.AddressableResource;
using GameFramework.Event;
using GameFramework.Procedure;
using XLua;
using GameFramework.UI;
using Pomelo.DotNetClient;
using NeoOPM;
using Spine.Unity;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = NeoOPM.GameEntry;
using Image = UnityEngine.UI.Image;
using Object = UnityEngine.Object;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class ReferenceManager
{

    private static IAddressableResourceManager _resMgr;
    public static IAddressableResourceManager ResMgr => _resMgr ??= GameFrameworkEntry.GetModule<IAddressableResourceManager>();

    
    private static Dictionary<GameObject,List<SpriteAtlas>> CatchAtlas = new Dictionary<GameObject, List<SpriteAtlas>>();
    private static Dictionary<GameObject,List<Sprite>> CatchTextrues = new Dictionary<GameObject, List<Sprite>>();
    private static Dictionary<GameObject,List<Material>> CatchMats = new Dictionary<GameObject, List<Material>>();
    private static void CatchAtlasHandle(GameObject target,SpriteAtlas atlas)
    {
        List<SpriteAtlas> list;
        if (CatchAtlas.TryGetValue(target,out list))
        {
            list.Add(atlas);
        }
        else
        {
            list = new List<SpriteAtlas>();
            list.Add(atlas);
            CatchAtlas.Add(target,list);
        }
    }

    public static void RealeasAtlas(GameObject target)
    {
        List<SpriteAtlas> list;
        if (CatchAtlas.TryGetValue(target,out list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                while (list.Count > 0)
                {
                    ResMgr.UnloadAsset(list[0]);
                    list.RemoveAt(0);
                }
                
            }
        }
    }
    private static void CatchTextruesHandle(GameObject target,Sprite sp)
    {
        List<Sprite> list;
        if (CatchTextrues.TryGetValue(target,out list))
        {
            list.Add(sp);
        }
        else
        {
            list = new List<Sprite>();
            list.Add(sp);
            CatchTextrues.Add(target,list);
        }
    }
    public static void RealeasTextrues(GameObject target)
    {
        List<Sprite> list;
        if (CatchTextrues.TryGetValue(target,out list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                while (list.Count > 0)
                {
                    ResMgr.UnloadAsset(list[0]);
                    list.RemoveAt(0);
                }
                
            }
        }
    }
    private static void CatchMatsHandle(GameObject target,Material mat)
    {
        List<Material> list;
        if (CatchMats.TryGetValue(target,out list))
        {
            list.Add(mat);
        }
        else
        {
            list = new List<Material>();
            list.Add(mat);
            CatchMats.Add(target,list);
        }
    }
    public static void RealeasMats(GameObject target)
    {
        List<Material> list;
        if (CatchMats.TryGetValue(target,out list))
        {
            for (int i = 0; i < list.Count; i++)
            {
                while (list.Count > 0)
                {
                    ResMgr.UnloadAsset(list[0]);
                    list.RemoveAt(0);
                }
                
            }
        }
    }
    /// <summary>
    /// 设置Image的sprite，并且缓存图集ab信息跟UI绑定，当UI被销毁时同时释放atlas的引用计数
    /// </summary>
    /// <param name="target"></param>
    /// <param name="sp"></param>
    /// <param name="atlasName"></param>
    /// <param name="spname"></param>
    /// <param name="native"></param>
    public static async void SetSprite(UGuiForm target,Image sp, string atlasName, string spname, bool native = false)
    {
        atlasName = AssetUtility.GetUIAtalsAsset(atlasName);
        Task<SpriteAtlas> task = ResMgr.GetAtlas(atlasName);
        var atlas = await task;
        var sprite = atlas.GetSprite(spname);
        if (target.IsDestroy() || sp == null)
        {
            RealeasAtlas(target.gameObject);
            return;
        }
        CatchAtlasHandle(target.gameObject, task.Result);
        sp.sprite = sprite;
        if (native)
        {
            sp.SetNativeSize();
        }
    }

    /// <summary>
    /// 与SetSprite的区别在于这个不是找的SpriteAtlas后缀的图集，而是.png等
    /// </summary>
    /// <param name="target"></param>
    /// <param name="sp"></param>
    /// <param name="atlasName"></param>
    /// <param name="spname"></param>
    public static async void SetImageSprite(UGuiForm target,Image image, string atlasName, string spname, bool native = false)
    {
        atlasName = AssetUtility.GetUITextrueAsset(atlasName);
        Task<Sprite> task = ResMgr.GetSprite(atlasName,spname);
        var sp = await task;
        var sprite = sp;
        if (target.IsDestroy() || sp == null)
        {
            RealeasTextrues(target.gameObject);
            return;
        }
        CatchTextruesHandle(target.gameObject, task.Result);
        image.sprite = sprite;
        if (native)
        {
            image.SetNativeSize();
        }
    }

    public static async void SetMaterial(UGuiForm target,Image sp, string path)
    {
        Task<Material> task = ResMgr.GetMaterial(path);
        var mat = await task;
        if (target.IsDestroy() || sp == null)
        {
            RealeasMats(target.gameObject);
            return;
        }
        CatchMatsHandle(target.gameObject, task.Result);
        sp.material = mat;
    }

   
}