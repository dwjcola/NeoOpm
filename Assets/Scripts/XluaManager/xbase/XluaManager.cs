using System;
using System.Collections.Generic;
using ProHA;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;
public class XluaManager : Singleton<XluaManager> {

    private const string luaRootPath = "LuaScripts";//lua 根目录
    private const string luaEditorPath = "Assets/Resource_MS/LuaScripts/";
    private const string luaRequirePath = "logic/requireLoad";
    private const string luaProxyPath = "proxy";
    private const string luaDataPath = "data";
    private  LuaEnv luaEnv; //all lua behaviour shared one luaenv only!
    /// <summary>
    /// lua 环境
    /// </summary>
    public LuaEnv LuaEnv { get

        {
            if (luaEnv == null)
                Init();
            return luaEnv;
        }
    }
//    private bool cachTxt=false;
    /// <summary>
    /// lua str map
    /// </summary>
    private Dictionary<string, string> luaMap = new Dictionary<string, string>();
    public Dictionary<string,string> LuaMap
    {
        get
        {
            return luaMap;
        }
    }

    private List<string> requireMap;
    

    public bool init = false;
    public bool LoadLuaDone = false;
    /// <summary>
    /// lua main 和uibase
    /// </summary>
    private LuaTable luaMain, luaUIBase;

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        if ( init==true )
            {
            return;
            }
        Logger.Message("xlua init");
        
        LoadLuaDone = true;
        InitEnv();
        InitLuaMain();
        //InitLuaUIBase();
        //InitProxy();
        init = true;
    }
    /// <summary>
    /// 初始化环境
    /// </summary>
    private void InitEnv()
    {
        
        luaEnv = new LuaEnv();
        requireMap = new List<string> ( );
        luaEnv.AddLoader ( ( ref string className ) => {
            if ( string.IsNullOrEmpty(className) )
            {
                return null;
            }

            if (className.EndsWith("Proxy"))
            {
                tempStr = GetLuaStringByName(className, luaProxyPath);
            }
            else if (className.EndsWith("Data"))
            {
                tempStr = GetLuaStringByName(className, luaDataPath);
            }
            else
            {
                var mPanelTable = TableTools.Tables.Panel.GetLineById(className);
                if (mPanelTable != null)
                {
                    tempStr = GetLuaStringByName(mPanelTable.LuaName, mPanelTable.LuaPath);
                }
                else
                {
                    tempStr = GetLuaStringByName(className, luaRequirePath);
                }
            }
            if (!string.IsNullOrEmpty(tempStr))
            {
                if (!requireMap.Contains(className))
                {
                    requireMap.Add(className);
                }
                return System.Text.Encoding.UTF8.GetBytes(tempStr);
            }
            else
            {
                Log.Warning("lua file{0}load failed!",className);
            }
            
            return null;

        } );

        luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
        //luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
        luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
        //luaEnv.AddBuildin("ffi", XLua.LuaDLL.Lua.LoadFFI);

    }
    /// <summary>
    /// 初始化main 和hotfix
    /// </summary>
    private void InitLuaMain()
    {
        DoLuaString("LUAMain","base");
        luaMain = luaEnv.Global.Get<LuaTable>("LUAMain");

#if HOTFIX_ENABLE
        Hotfix ( );
#endif
    }

    private void Hotfix ( )
        {
        //if ( ConfigManager.Instance.hotfixConfig!=null )
        //    {
        //    var config= ConfigManager.Instance.hotfixConfig["files"];
        //    var count=config.Count;
        //    for ( int i = 0; i < config.Count; i++ )
        //        {
        //        DoLuaString ( config [ i ].ToString ( ), "hotfix" );
        //        }
        //    }
    
       
        }
    ///// <summary>
    ///// 初始化uibase
    ///// </summary>
    //private void InitLuaUIBase()
    //{
    //    DoLuaString("LUAUIBase", "base");
    //    luaUIBase= luaEnv.Global.Get<LuaTable>("LUAUIBase");
    //    var baseinit = luaUIBase.Get<Action<LuaTable>>("init");
    //    if (baseinit!=null)
    //    {
    //        baseinit(luaUIBase);
    //    }

    //    //InitProxy();
    //}

    //private void InitProxy()
    //{
    //    DoLuaString("LUAInitProxy", "proxy");
    //}

    /// <summary>
    /// 销毁
    /// </summary>
    public void OnDestroy()
    {
        return;
//        if(luaMain!=null)
//        luaMain.Dispose();
//        luaMain = null;
//        if (luaUIBase != null)
//            luaUIBase.Dispose();
//        luaUIBase = null;
//        if (luaEnv != null)
//            luaEnv.Dispose();
//        luaEnv = null;
    }

    /// <summary>
    /// 执行luastr
    /// </summary>
    string tempStr=string.Empty;
    TextAsset tempText = null;
    /// <summary>
    /// 目前是预加载以lua文件名字绑定的内容，
    /// </summary>
    /// <param name="className"></param>
    /// <param name="path">这个path传了是为了编辑器模式下可以修改立刻生效，不走addressable</param>
    /// <returns></returns>
    public bool DoLuaString(string className,string path="")
    {
        if (string.IsNullOrEmpty(className))
        {
            return false;
        }
        tempStr = GetLuaStringByName(className, path);
        if (string.IsNullOrEmpty(tempStr))
        {
            return false;
        }
        luaEnv.DoString(tempStr, className);

        return true;       
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="className"></param>
    /// <param name="path">这个path传了是为了编辑器模式下可以修改立刻生效，不走addressable</param>
    /// <returns></returns>
    public object[] RetDoLuaString ( string className,string path="")
    {
        if ( string.IsNullOrEmpty ( className ) )
        {
            return null;
        }
        tempStr = GetLuaStringByName (className, path);
        if ( string.IsNullOrEmpty ( tempStr ) )
        {
            return null;
        }
        var res=luaEnv.DoString ( tempStr, className ) ;
        
        return res;
    }

    /// <summary>
    /// 查询lua文件是否存在
    /// </summary>
    /// <param name="className"></param>
    /// <param name="path">这个path传了是为了编辑器模式下可以修改立刻生效，不走addressable</param>
    /// <returns></returns>
    public bool HasLua(string className,string path="")
    {
        if (string.IsNullOrEmpty(className))
        {
            return false;
        }
        tempStr = GetLuaStringByName(className, path);
        return !string.IsNullOrEmpty(tempStr);               
    }

    /// <summary>
    /// 获取lua str
    /// </summary>
    /// <param name="className"></param>
    /// <param name="path">这个参数只是在编辑器模式下使用的，为的是可以在修改之后立刻生效</param>
    /// <returns></returns>
    private string GetLuaStringByName(string className,string path="")
    {     
        if (string.IsNullOrEmpty(className))
        {
            return className;
        }
#if UNITY_EDITOR //编辑器模式下走这个快速加载，修改可以立刻生效
        string pathTemp = string.IsNullOrEmpty(path) ? luaEditorPath : luaEditorPath + path + "/";
        tempText = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(pathTemp + className + ".txt");
        tempStr = tempText?.text;
        if (string.IsNullOrEmpty(tempStr))
        {
            tempStr = GetLuaStrInDic(className);//基本上走到这里的应该都是一些常驻的了吧
        }
#else //打包之后从这里
        tempStr = GetLuaStrInDic(className);
#endif
            if (string.IsNullOrEmpty(tempStr))
        {
            UnityGameFramework.Runtime.Log.Warning($"{className} did not find in luaMap");
        }
        return tempStr;
        
    }

    /// <summary>
    /// 字典中查找
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public string GetLuaStrInDic(string className)
    {
        if (!string.IsNullOrEmpty(className)&&luaMap.ContainsKey(className))
            return luaMap[className];
        return string.Empty;
    }
    private  float lastGCTime = 0;
    private const float GCInterval = 1;

    /// <summary>
    /// tick
    /// </summary>
    public void Tick()
    {
        if (Time.time - lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            lastGCTime = Time.time;
        }
    }

    public LuaTable GetLua(string key )
        {
        return luaEnv.Global.Get<LuaTable> ( key );
        }

    public void ClearLuaCache ( )
        {
        if ( luaMap!=null )
            {
            luaMap.Clear ( );
            }
        
       var func= luaMain?.Get<string, Action<LuaTable,List<string>>> ( "clearcache" );
        if ( func!=null )
        {
            func ( luaMain,requireMap );
        }
       
        }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }
    const string LuaGroupName = "LuaScripts";
    public void InitLuaMap(TextAsset text)
    {
        if (!luaMap.ContainsKey(text.name))
        {
            luaMap.Add(text.name, text.text);
        }
    }
}

public static class ArrayHelper
    {
    public static List<object> ToList ( this LuaTable table )
        {
        List<object> list = new List<object>();
       return ToList ( list, table );    
        }

    public static List<object> ToList ( List<object> list, LuaTable table )
        {
        if ( list == null || table == null )
            {
            return null;
            }
       var keys= table.GetKeys ( );
        foreach ( var key in keys )
            {
            object value=null;
            table.Get<object, object> ( key, out value );
            list.Add ( value );
            }
        return list;
        }

    public static object [ ] ToArray ( LuaTable table )
    {
        List<object> list = ToList(table);
        if ( list != null )
        {
            return list.ToArray ( );
        }
        return null;
    }

    }
