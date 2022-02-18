#if UNITY_EDITOR
#define ENABLE_LOG
#endif
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

public class LC
{
#if DEBUG || UNITY_EDITOR
    public static bool ISDEBUG = true;
#else
    public static bool ISDEBUG = false;
#endif
    private static IAddressableResourceManager _resMgr;
    public static IAddressableResourceManager ResMgr => _resMgr ??= GameFrameworkEntry.GetModule<IAddressableResourceManager>();


    /// <summary>
    /// lua添加ui事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="go"></param>
    /// <param name="luaClass"></param>
    /// <param name="callBack"></param>
    public static void AddUIEvent(LuaTable luaClass, string key, GameObject go, Action<LuaTable, GameObject> callBack)
    {
        switch (key)
        {
            case "onclick":
                EventTriggerListener.Get(go).onClick = (g) => { callBack(luaClass, g); };
                break;
            case "onDown":
                EventTriggerListener.Get(go).onDown = (g) => { callBack(luaClass, g); };
                break;
            case "onDoubleClick":
                EventTriggerListener.Get(go).onDoubleClick = (g) => { callBack(luaClass, g); };
                break;
            case "onUp":
                EventTriggerListener.Get(go).onUp = (g) => { callBack(luaClass, g); };
                break;
            case "onPress":
                EventTriggerListener.Get(go).onPress = (g) => { callBack(luaClass, g); };
                break;
            case "onExit":
                EventTriggerListener.Get(go).onExit = (g) => { callBack(luaClass, g); };
                break;
            case "OnDragBegin":
                EventTriggerListener.Get(go).OnDragBegin = (g) => { callBack(luaClass, g); };
                break;
            case "OnDragFuc":
                EventTriggerListener.Get(go).OnDragFuc = (g) => { callBack(luaClass, g); };
                break;
            case "OnDragEnd":
                EventTriggerListener.Get(go).OnDragEnd = (g) => { callBack(luaClass, g); };
                break;
            default:
                break;
        }

    }
    public static void RemoveUIEvent(GameObject go)
    {
        EventTriggerListener.Get(go).onClick = null;
        EventTriggerListener.Get(go).onDown = null;
        EventTriggerListener.Get(go).onDoubleClick = null;
        EventTriggerListener.Get(go).onUp = null;
        EventTriggerListener.Get(go).onPress  = null;
        EventTriggerListener.Get(go).onExit  = null;
        EventTriggerListener.Get(go).OnDragBegin = null;
        EventTriggerListener.Get(go).OnDragFuc = null;
        EventTriggerListener.Get(go).OnDragEnd = null;
        EventTriggerListener.Get(go).onBeginDragOPM = null;
        EventTriggerListener.Get(go).onDragOPM = null;
        EventTriggerListener.Get(go).onEndDragOPM = null;
    }
    public static void AddUIEvent_PointData(string key, GameObject go, Action<GameObject, PointerEventData> callBack)
    {
        switch (key)
        {
            case "onBeginDragOPM":
                EventTriggerListener.Get(go).onBeginDragOPM = (g,eventData) => { callBack(g, eventData); };
                break;
            case "onDragOPM":
                EventTriggerListener.Get(go).onDragOPM = (g, eventData) => { callBack( g, eventData); };
                break;
            case "onEndDragOPM":
                EventTriggerListener.Get(go).onEndDragOPM = (g, eventData) => { callBack( g, eventData); };
                break;
            default:
                break;
        }

    }

    public static void AddUIDataEvent(LuaTable luaClass, string key, GameObject go, Action<LuaTable, GameObject, LuaTable> callBack, LuaTable data)
    {
        switch (key)
        {
            case "onclick":
                EventTriggerListener.Get(go).onClick = (g) => { callBack(luaClass, g, data); };
                break;
            case "onDown":
                EventTriggerListener.Get(go).onDown = (g) => { callBack(luaClass, g, data); };
                break;
            case "onDoubleClick":
                EventTriggerListener.Get(go).onDoubleClick = (g) => { callBack(luaClass, g, data); };
                break;
            case "onUp":
                EventTriggerListener.Get(go).onUp = (g) => { callBack(luaClass, g, data); };
                break;
            case "onPress":
                EventTriggerListener.Get(go).onPress = (g) => { callBack(luaClass, g, data); };
                break;
            default:
                break;
        }

    }
    /// <summary>
    /// 监听tab的选择事件
    /// </summary>
    /// <param name="lua"></param>
    /// <param name="tab"></param>
    /// <param name="back"></param>
    public static void AddTabSelectEvent(LuaTable lua, CToggleGroup tab, Action<LuaTable, int> back)
    {
        tab.tabSelect = (index) =>
        {
            back(lua, index);
        };
        tab.AddEvent();
    }
    /// <summary>
    /// log信息
    /// </summary>
    /// <param name="message"></param>
    public static void Log(object message)
    {
        UnityGameFramework.Runtime.Log.Info(message);
    }
    /// <summary>
    /// error
    /// </summary>
    /// <param name="error"></param>
    public static void Error(object error)
    {
        UnityGameFramework.Runtime.Log.Error(error);
    }
    public static void Warning(object warning)
    {
        UnityGameFramework.Runtime.Log.Warning(warning);
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="key"></param>
    public static void CloseUI(string key)
    {
        var panel = GetUITable(key);
        if (panel == null)
        {
            return;
        }
        string tAssetName = panel.AssetName;
        string assetName = AssetUtility.GetUIFormAsset(tAssetName);
        if (GameEntry.UI.IsLoadingUIForm(assetName))
        {
            return;//如果ui正在打开停止close
        }
        GameEntry.UI.CloseUI(key);
        
    }
    //读取xlua表
    public static LuaTable GetTable(string tableName,string key)
    {
        LuaTable t = XluaManager.instance.GetLua("ConfigReader");
        Func<LuaTable, string, string,LuaTable> func;
        t.Get("getDataByNameAndId",out func);
        LuaTable lt = func(t, tableName, key);
        return lt;
    }
    public static LuaTable GetTable(string tableName,int key)
    {
        LuaTable t = XluaManager.instance.GetLua("ConfigReader");
        Func<LuaTable, string, int,LuaTable> func;
        t.Get("getDataByNameAndId",out func);
        LuaTable lt = func(t, tableName, key);
        return lt;
    }

    public static void CallLuaFunc(string fileName, string funcName)
    {
        LuaTable t = XluaManager.instance.GetLua(fileName);
        Func<LuaTable, LuaTable> func;
        t.Get(funcName, out func);
        func(t);
        t.Dispose();
    }

    #region 移动到了GameUtility.cs中

    public static Vector3 GetRayRaycastHitInfo(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, LayerMask.GetMask("3DPlane")))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
    public static Camera GetUICamera()
    {
        return GameEntry.UI.m_uiCamera;
    }
    public static Vector3 ScreenPointToWorldPointInRectangle(GameObject target, UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector3 pos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(target.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos))
        {
            return pos;
        }
        return pos;
    }
    public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Vector3 target)
    {
        Vector3 tarPos = target;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);

        Vector2 imgPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);
      
        return new Vector2(imgPos.x, imgPos.y);
    }


    public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Transform targetTR)
    {
        
        Vector3 tarPos = targetTR.position;
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);

        Vector2 imgPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);

        return new Vector2(imgPos.x, imgPos.y);
    }
    public static Vector2 WorldPosToScreenLocalPos(UnityEngine.Camera camera, UnityEngine.Camera uiCamera, RectTransform rectangle, Transform targetTR, float offsetX, float offsetY, float offsetZ)
    {
        Vector3 tarPos = targetTR.position + new Vector3(offsetX, offsetY, offsetZ);
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, tarPos);

        Vector2 imgPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectangle, screenPos, uiCamera, out imgPos);

        return new Vector2(imgPos.x, imgPos.y);
    }
    public static Type GetType(string typeName)
    {
        // 先在当前Assembly找,再在UnityEngine里找.
        // 如果都找不到,再遍历所有Assembly
        var type = Assembly.GetExecutingAssembly().GetType(typeName) ?? typeof(ParticleSystem).Assembly.GetType(typeName);
        if (type != null) return type;
        type = GetUnityType(typeName);
        if (type != null) return type;
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = a.GetType(typeName);
            if (type != null)
                return type;
        }
        return null;
    }

    private static Type GetUnityType(string typeName)
    {
        string namespaceStr = "UnityEngine";
        if (!typeName.Contains(namespaceStr))
            typeName = namespaceStr + "." + typeName;
        var assembly = Assembly.Load(namespaceStr);
        if (assembly == null)
            return null;
        return assembly.GetType(typeName);

    }

    public static Component GetOrAddComponent(GameObject target, string className)
    {
        Component com = target.GetComponent(className);
        if (com == null)
        {
            com = target.AddComponent(GetType(className));
        }
        return com;
    }

    public static Component GetOrAddComponent(GameObject target, string path, Type className)
    {
        Transform child = target.transform.Find(path);
        if (child == null)
        {
            return null;
        }
        Component com = child.GetComponent(className);
        if (com == null)
        {
            com = child.gameObject.AddComponent(className);
        }
        return com;
    }
    public static Component GetOrAddComponent(GameObject target, Type className)
    {
        Component com = target.GetComponent(className);
        if (com == null)
        {
            com = target.AddComponent(className);
        }
        return com;
    }
    public static GameObject CreateGame()
    {
        GameObject obj = new GameObject();
        obj.SetActive(true);
        return obj;
    }
    #endregion
    

    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="key"></param>
    /// <param name="para"></param>
	public static int? OpenUI(string key, object para = null)
    {
        return GameEntry.UI.OpenUI(key, para);
    }
    public static LuaTable ShowPartPanel(string key, Transform parent, object para = null)
    {
        return GameEntry.UI.ShowPartUIForm(key, parent, para);
    }
 
    public static DataPoolComponent.ITableValue GetUITable(string uiKey)
    {
        DataPoolComponent.ITableValue tv = GameEntry.DataPool.GetPanelValueByKey(uiKey);
        return tv;
    }
    public static LuaTable CreateItem(GameObject item, Transform parent, LuaTable lua)
    {
        GameObject go = Object.Instantiate<GameObject>(item, parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
        UIMonoItem mono = go.GetComponent<UIMonoItem>();
        if (mono)
        {
            mono.OnInit(lua);
        }

        return lua;
    }


    #region c#和lua互通的事件

    public static void Subscribe(string key, EventHandler<GameEventArgs> handler)
    {
        GameEntry.Event.Subscribe(key.GetHashCode(), handler);
    }
    public static void Unsubscribe(string key, EventHandler<GameEventArgs> handler)
    {
        GameEntry.Event.Unsubscribe(key.GetHashCode(), handler);
    }
    public static void AddEvent(LuaTable luaClass, string key, Action<LuaTable, object> callBack)
    {
        AddEvent(luaClass, key.GetHashCode(), callBack);
    }
    private static void AddEvent(LuaTable luaClass, int key, Action<LuaTable, object> callBack)
    {
        EventHandler<GameEventArgs> handler = (send, e) =>
        {
            LUAEventArgs lea = (LUAEventArgs)e;
            callBack(luaClass, lea);
        };
        string luaId = "";
        luaClass.Get<string, string>("luaId", out luaId);
        string hkey = luaId + callBack.GetHashCode() + key.ToString();
        if (!handlers.ContainsKey(hkey))
        {
            handlers.Add(hkey, handler);
        }
        else
        {
            handlers[hkey] = handler;
        }
        GameEntry.Event.Subscribe(key, handler);
    }
    public static void AddCSEvent(LuaTable luaClass, int key, Action<LuaTable, object> callBack)
    {
        EventHandler<GameEventArgs> handler = (send, e) =>
        {
            callBack(luaClass, e);
        };
        string luaId = "";
        luaClass.Get<string, string>("luaId", out luaId);
        string hkey = luaId + callBack.GetHashCode() + key.ToString();
        if (!handlers.ContainsKey(hkey))
        {
            handlers.Add(hkey, handler);
        }
        else
        {
            handlers[hkey] = handler;
        }

        GameEntry.Event.Subscribe(key, handler);
    }
    public static void RemoveEvent(LuaTable luaClass, string key, Action<LuaTable, object> callBack)
    {
        RemoveEvent(luaClass, key.GetHashCode(), callBack);
    }
    private static void RemoveEvent(LuaTable luaClass, int key, Action<LuaTable, object> callBack)
    {
        EventHandler<GameEventArgs> mh;
        string luaId = "";
        luaClass.Get<string, string>("luaId", out luaId);
        var hkey = luaId + callBack.GetHashCode() + key.ToString();
        if (handlers.TryGetValue(hkey, out mh))
        {
            GameEntry.Event.Unsubscribe(key, mh);
            handlers.Remove(hkey);
            mh = null;
        }
    }
    public static void RemoveCSEvent(LuaTable luaClass, int key, Action<LuaTable, object> callBack)
    {
        EventHandler<GameEventArgs> mh;
        string luaId = "";
        luaClass.Get<string, string>("luaId", out luaId);
        var hkey = luaId + callBack.GetHashCode() + key.ToString();
        if (handlers.TryGetValue(hkey, out mh))
        {
            GameEntry.Event.Unsubscribe(key, mh);
            handlers.Remove(hkey);
            mh = null;
        }
    }
    
    public static void SendEvent(string key, object data = null, object data2 = null, object data3 = null, object data4 = null)
    {
        SendEvent(key.GetHashCode(), data, data2, data3, data4);
    }
    private static Dictionary<string, EventHandler<GameEventArgs>> handlers = new Dictionary<string, EventHandler<GameEventArgs>>();


    private static EventComponent m_EventComponent = null;
    private static object EventSender = new object();
    private static void SendEvent(int key, object data = null, object data2 = null, object data3 = null, object data4 = null)
    {
        if (m_EventComponent == null)
        {
            m_EventComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<EventComponent>();
        }
        m_EventComponent.Fire(EventSender, LUAEventArgs.Create(key, data, data2, data3, data4));
    }


    #endregion
    
    public static void RealeasUICatch(GameObject target)
    {
        ReferenceManager.RealeasAtlas(target);
        ReferenceManager.RealeasTextrues(target);
        ReferenceManager.RealeasMats(target);
    }
    public static async void SetSprite(UGuiForm target,Image sp, string atlasName, string spname, bool native = false)
    {
        ReferenceManager.SetSprite(target,sp,atlasName,spname,native);
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
        ReferenceManager.SetImageSprite(target,image,atlasName,spname,native);
    }
    public static void SetMaterial(UGuiForm target,Image sp, string path)
    {
        ReferenceManager.SetMaterial(target,sp,path);
    }

    /*public static async void LoadSpine(string spineName, Transform parent, Action<LuaTable, SkeletonGraphic> callback, LuaTable lua)
    {
        string path = AssetUtility.GetSpineAsset(spineName); ;
        IAddressableResourceManager resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
        GameObject go = await resMgr.LoadAssetAsync<GameObject>(path).Task;
        GameObject skeleGo = Object.Instantiate(go, parent, false);
        SkeletonGraphic sa = skeleGo.GetComponent<SkeletonGraphic>();
        callback?.Invoke(lua, sa);
    }

    public static async void LoadSpineAnimation(string spineName, Transform parent, Action<LuaTable, SkeletonAnimation> callback, LuaTable lua)
    {
        string path = AssetUtility.GetSpineAssetBattle(spineName); ;
        IAddressableResourceManager resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
        GameObject go = await resMgr.LoadAssetAsync<GameObject>(path).Task;
        GameObject skeleGo = Object.Instantiate(go, parent, false);
        SkeletonAnimation sa = skeleGo.GetComponent<SkeletonAnimation>();      
        callback?.Invoke(lua, sa);
    }*/
    public static async void LoadBattleScene(string sceneName, LuaTable lua, Action<LuaTable, GameObject> action)
    {
        IAddressableResourceManager resMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
        GameObject per = await resMgr.LoadAssetAsync<GameObject>(AssetUtility.GetBattleScene(sceneName)).Task;
        GameObject go = Object.Instantiate(per);
        action?.Invoke(lua, go);
    }
    public static AsyncOperationHandle<GameObject> LoadAsset(string assetPath, Action<GameObject> action)
    {
        var handle = ResMgr.LoadAssetAsync<GameObject>(assetPath);
        handle.Completed+= (per) =>
        {
            if (action != null)
            {
                GameObject go = Object.Instantiate(per.Result);
                action.Invoke(go);
            }
        };
        return handle;
    }
   
    public static void ClearAll()
    {
        GameEntry.UI.RemoveAllUI();
        PomeloClient.Instance.Dispose();
        if (XluaManager.instance != null)
        {
            XluaManager.instance.ClearLuaCache();
            XluaManager.instance.init = false;
        }

        GameEntry.Event.UnsubscribeAll();

        handlers?.Clear();
    }
    public static void ReconnectSucc()
    {
        Rpc.PushEvent.Init(PomeloClient.Instance);

        XluaManager.instance.LuaEnv.DoString(@"
        if PomeloCLUA ~= nil then 
            PomeloCLUA:ConnectServerInit() 
        else
            PomeloCLUA=require('PomeloCLUA')
            PomeloCLUA:Init()
        end");
    }

   
}

#region LUAEventArgs

public class LUAEventArgs : GameEventArgs
{
    /// <summary>
    /// 打开界面成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(LUAEventArgs).GetHashCode();

    /// <summary>
    /// 初始化打开界面成功事件的新实例。
    /// </summary>
    public LUAEventArgs()
    {
        EventKey = 0;
        Param1 = null;
        Param2 = null;
        Param3 = null;
        Param4 = null;
    }

    /// <summary>
    /// 获取打开界面成功事件编号。
    /// </summary>
    public override int Id
    {
        get
        {
            return EventKey == 0 ? EventId : EventKey;
        }
    }

    public int EventKey
    {
        get;
        private set;
    }
#region 参数

    /// <summary>
    /// 用户数据1
    /// </summary>
    public object Param1
    {
        get;
        private set;
    }
    /// <summary>
    /// 用户数据2
    /// </summary>
    public object Param2
    {
        get;
        private set;
    }
    /// <summary>
    /// 用户数据3
    /// </summary>
    public object Param3
    {
        get;
        private set;
    }
    /// <summary>
    /// 用户数据4
    /// </summary>
    public object Param4
    {
        get;
        private set;
    }

#endregion

    /// <summary>
    /// 创建打开界面成功事件。
    /// </summary>
    /// <param name="e">内部事件。</param>
    /// <returns>创建的打开界面成功事件。</returns>
    public static LUAEventArgs Create(int key, object param1 = null, object param2 = null, object param3 = null, object param4 = null)
    {
        LUAEventArgs lea = ReferencePool.Acquire<LUAEventArgs>();
        lea.EventKey = key;
        lea.Param1 = param1;
        lea.Param2 = param2;
        lea.Param3 = param3;
        lea.Param4 = param4;
        return lea;
    }

    /// <summary>
    /// 清理打开界面成功事件。
    /// </summary>
    public override void Clear()
    {
        EventKey = 0;
        Param1 = null;
        Param2 = null;
        Param3 = null;
        Param4 = null;
    }
}

#endregion
