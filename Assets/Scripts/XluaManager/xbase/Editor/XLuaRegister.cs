/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
 */
#undef UNITY_EDITOR
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;
using Object = UnityEngine.Object;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class XLuaRegister
{

    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> luaCallCSharp = new List<Type>() {

        typeof (System.Object),
        typeof (LitJson.JsonData),
        typeof (UnityEngine.Object),
        typeof (Vector2),
        typeof (Vector3),
        typeof (Vector4),
        typeof (Quaternion),
        typeof (Color),
        typeof (Ray),
        typeof (Bounds),
        typeof (Ray2D),
        typeof (Time),
        typeof (GameObject),
        typeof (Component),
        typeof (Behaviour),
        typeof (Transform),
        typeof (Resources),
        typeof (TextAsset),
        typeof (Keyframe),
        typeof (AnimationCurve),
        typeof (AnimationClip),
        typeof (MonoBehaviour),
        typeof (ParticleSystem),
        typeof (SkinnedMeshRenderer),
        typeof (Renderer),
        typeof (UnityEngine.Networking.UnityWebRequest),

        typeof (System.Collections.Generic.List<int>),
        typeof (Texture2D),
        typeof (GameObject),
        typeof (Vector4),
        typeof (Vector3),
        typeof (Vector2),
        typeof (Color),
        typeof (Color),
        // typeof (StorylineActionEngine),
        typeof (Rect),
        typeof (Quaternion),

        typeof (Camera),

        typeof (WrapMode),
        typeof(UnityExtension),
        typeof(StringExtension),
        typeof(NeoOPM.UIExtension),
        typeof(NeoOPM.MessageExtension),
        typeof(DG.Tweening.ShortcutExtensions),
        typeof(DG.Tweening.DOTweenModuleUI),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.LoopType),
        typeof(DG.Tweening.TweenExtensions),
        typeof(DG.Tweening.Ease),
        typeof(DG.Tweening.Sequence),
        typeof(SLG.WsObjInfo),
        typeof(LC),
    };

    [ReflectionUse]
    public static List<Type> reflectuse = new List<Type>() {
        
        // typeof (SerializedMonoBehaviour),

        };

    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp
    {
        get
        {
            var ilua = (from type in Assembly.Load("Assembly-CSharp").GetTypes() where type.BaseType == typeof(ILuaData) select type).ToList();

            //System,unityengine,tables

            List<Type> luacs = new List<Type>();
            luacs.AddRange ( ilua );

            return luacs;
        }
    }

    [GCOptimizeAttribute]
    public static List<Type> LuaCallCSharpTables
    {
        get
        {

            return ( from type in Assembly.Load ( "Assembly-CSharp" ).GetTypes ( ) where type.Namespace == "Tables" && !type.IsClass select type ).ToList ( );
        }
    }

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
        typeof (Action),
        typeof (Action<LuaTable>),
        typeof (Action<LuaTable, object>),
        typeof (Action<LuaTable, object,object>),
        typeof (Action<LuaTable, bool>),
        typeof (Action<LuaTable, int>),
        
        typeof (   Action<LuaTable, float>),
        typeof (Action<LuaTable, string>),
        typeof (Action<LuaTable, string,string>),
        typeof (Action<LuaTable, string,int>),
        typeof (Action<LuaTable, LuaTable, Toggle>),
        typeof (Action<LuaTable, LuaTable, object, int>),
        typeof (Action<LuaTable, GameObject, LuaTable>),
        typeof (Action<LuaTable, GameObject, GameObject>),
        typeof (Action<LuaTable, GameObject>),
        typeof (Action<LuaTable, int, LuaTable>),
        typeof (Action<LuaTable, LuaTable>),
          typeof (Action<LuaTable, int, object>),

        typeof (Func<LuaTable, long>),
        typeof (Func<LuaTable, int>),
        typeof (Func<LuaTable, bool>),
        typeof (Func<LuaTable, int,int>),
        typeof (Func<LuaTable, int,bool>),
        typeof (Func<LuaTable, ulong,string>),
        typeof (Func<double, double, double>),
        typeof (Func<LuaTable, string, object>),
        typeof (Func<LuaTable, string, string,LuaTable>),
        typeof (Func<LuaTable, string, int,LuaTable>),
        typeof (Func<LuaTable, LuaTable, object, int,PointerEventData, bool>),
        typeof (Func<LuaTable, List<SLG.AttrInc>>),
        typeof (Action<LuaTable, LuaTable, object, int,PointerEventData>),
        typeof (Action<LuaTable, LuaTable, object, int,PointerEventData>),
        typeof (Action<string>),
        typeof (Action<double>),
        typeof (Action<int>),
        typeof (Action<long>),
        typeof (Action<LuaTable, GameObject, bool, Vector2>),
        typeof (UnityEngine.Events.UnityAction),
        typeof (System.Collections.IEnumerator),
        typeof (Action<DateTime>),
        typeof (Action<LuaTable, IList>),
        typeof( System.Action<> ),
           typeof( System.Action<Google.Protobuf.IMessage> ),
            typeof( System.Action<byte[]> ),
            typeof(Action<byte[],int>),
              typeof( Action<LuaTable,List<string>>),
        typeof(Action<LuaTable,float,LuaTable>),
        typeof(Action<LuaTable, GameObject, LuaTable,bool>),
        typeof(Delegate),
        typeof(UnityEngine.Events.UnityAction<float>),
        typeof(UnityEngine.Events.UnityAction<bool>),
        typeof (UnityEngine.Events.UnityAction<Vector2>),
        typeof ( Action<LuaTable, SkeletonAnimation> ),
        typeof (Func<LuaTable, float, float,LuaTable>),
    };
    //HOTFIX_ENABLE


    //[Hotfix]
    //public static List<Type> by_property
    //    {
    //    get
    //        {
    //        return ( from type in Assembly.Load ( "Assembly-CSharp" ).GetTypes ( ) where type.BaseType == typeof ( ProxyBase ) select type ).ToList ( );
    //        }
    //    }

    public static List<string> blacklistNameSpace = new List<string>() {
        "xARM",
        "PhotoshopFile",
        "Dynamic",
        "UnityEditor"

        };
    public static List<string> blacklistClassName = new List<string>() {
        "xARMGalleryWindow",
         "xARMProxy",
          "xARMConfig",
           "SkeletonGraphic",
            "SpecialObj",
            "Monster",
            "xARMManager"
        };
#if HOTFIX_USED
    [Hotfix]
    [LuaCallCSharp]
    [ReflectionUse]
    public static List<Type> by_property
        {
        get
            {
            if ( hotlist==null )
                {
                Debug.Log ( "get" );
                hotlist = ( from type in Assembly.Load ( "Assembly-CSharp" ).GetExportedTypes ( )
                           where (
                           isWhite ( type ) &&
                            ( type.Namespace == null || false
                            )
                            )
                           select type ).ToList ( );
              
                }
            
            return hotlist;

            }
        }

    public static List<Type> hotlist;

    static bool isWhite ( Type type )
        {
        return !blacklistClassName.Contains ( type.Name ) &&
              !type.ToString ( ).StartsWith ( "xARM" ) &&
               !type.ToString ( ).StartsWith ( "Test" ) &&
                !type.ToString ( ).StartsWith ( "Reporter" ) &&
                !type.ToString ( ).StartsWith ( "UnityEditor" ) &&
                 !type.ToString ( ).StartsWith ( "SpecialObj" ) &&
                 !type.ToString ( ).StartsWith ( "Monster" ) &&
                !type.ToString ( ).StartsWith ( "PhotoshopFile" ) &&
               !type.ToString ( ).StartsWith ( "Jenkins" ) &&
               !type.ToString ( ).StartsWith ( "PerformBuild" ) &&
                !type.ToString ( ).StartsWith ( "Utility" ) &&
                !type.ToString ( ).Contains ( "BuglyAgent" ) &&
               !type.ToString ( ).StartsWith ( "CameraPath" ) &&
               !type.ToString ( ).StartsWith ( "MyPath" ) &&
              !type.ToString ( ).StartsWith ( "XML" ) &&
            !type.ToString ( ).StartsWith ( "XLua" ) &&
              !type.ToString ( ).StartsWith ( "TipsData" ) &&
              !type.ToString ( ).StartsWith ( "JY_Group" ) &&
             !type.ToString ( ).StartsWith ( "XLua" ) &&
              !type.ToString ( ).StartsWith ( "CustomStructDrawer" ) &&
              !type.ToString ( ).StartsWith ( "ETA_Water" ) &&
               !type.ToString ( ).StartsWith ( "PrefabSign" ) &&
                !type.ToString ( ).StartsWith ( "SerializeDemo" ) &&
                 !type.ToString ( ).StartsWith ( "UIDrawCall" ) &&
                  !type.ToString ( ).StartsWith ( "UtilsNode" ) &&
                   !type.ToString ( ).StartsWith ( "UIDrawCall" ) &&
                    !type.ToString ( ).StartsWith ( "CellMgr" ) &&
                     !type.ToString ( ).StartsWith ( "UISlider" ) &&
                    !type.ToString ( ).StartsWith ( "effectParams" ) &&
                    !type.ToString ( ).StartsWith ( "LuaTool" ) &&
                    
               type.BaseType != typeof ( ScriptableObject );
      
        }



    static bool hasGenericParameter ( Type type )
        {
        if ( type.IsGenericTypeDefinition ) return true;
        if ( type.IsGenericParameter ) return true;
        if ( type.IsByRef || type.IsArray )
            {
            return hasGenericParameter ( type.GetElementType ( ) );
            }
        if ( type.IsGenericType )
            {
            foreach ( var typeArg in type.GetGenericArguments ( ) )
                {
                if ( hasGenericParameter ( typeArg ) )
                    {
                    return true;
                    }
                }
            }
        return false;
        }

    static bool typeHasEditorRef ( Type type )
        {
        if ( type.Namespace != null && ( type.Namespace == "UnityEditor" || type.Namespace.StartsWith ( "UnityEditor." ) ) )
            {
            return true;
            }
        if ( type.IsNested )
            {
            return typeHasEditorRef ( type.DeclaringType );
            }
        if ( type.IsByRef || type.IsArray )
            {
            return typeHasEditorRef ( type.GetElementType ( ) );
            }
        if ( type.IsGenericType )
            {
            foreach ( var typeArg in type.GetGenericArguments ( ) )
                {
                if ( typeHasEditorRef ( typeArg ) )
                    {
                    return true;
                    }
                }
            }
        return false;
        }

    static bool delegateHasEditorRef ( Type delegateType )
        {
        if ( typeHasEditorRef ( delegateType ) ) return true;
        var method = delegateType.GetMethod("Invoke");
        if ( method == null )
            {
            return false;
            }
        if ( typeHasEditorRef ( method.ReturnType ) ) return true;
        return method.GetParameters ( ).Any ( pinfo => typeHasEditorRef ( pinfo.ParameterType ) );
        }


     [CSharpCallLua]
    static IEnumerable<Type> AllDelegate
        {
        get
            {
            BindingFlags flag = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
            List<Type> allTypes = by_property;

            allTypes = allTypes.Distinct ( ).ToList ( );
            var allMethods = from type in allTypes
                             from method in type.GetMethods(flag)
                             select method;
            var returnTypes = from method in allMethods
                              select method.ReturnType;
            var paramTypes = allMethods.SelectMany(m => m.GetParameters()).Select(pinfo => pinfo.ParameterType.IsByRef ? pinfo.ParameterType.GetElementType() : pinfo.ParameterType);
            var fieldTypes = from type in allTypes
                             from field in type.GetFields(flag)
                             select field.FieldType;
            return ( returnTypes.Concat ( paramTypes ).Concat ( fieldTypes ) ).Where ( t => t.BaseType == typeof ( MulticastDelegate ) && !hasGenericParameter ( t ) && !delegateHasEditorRef ( t ) && isWhite ( t ) ).Distinct ( );
            }
        }



    [MenuItem ( "XLua/Disable hotfix" )]
    public static void Disablehotfix ( )
        {

        string currentSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        var newSymbol = currentSymbol.Replace("HOTFIX_ENABLE","");
        Debug.Log ( "Script define " + newSymbol );
        PlayerSettings.SetScriptingDefineSymbolsForGroup ( BuildTargetGroup.Android, newSymbol );

        }

    [MenuItem ( "XLua/Enable hotfix" )]
    public static void Enablehotfix ( )
        {

        string currentSymbol = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        if ( !currentSymbol.Contains( "HOTFIX_ENABLE" ) )
            {          
            var newSymbol = string.Format("{0};{1}", currentSymbol, "HOTFIX_ENABLE");
            Debug.Log ( "Script define " + newSymbol );
            PlayerSettings.SetScriptingDefineSymbolsForGroup ( BuildTargetGroup.Android, newSymbol );
            }


        }

    [MenuItem ( "XLua/WriteHotfixlist",false,1 )]
    public static void WriteList ( )
        {
        var list=by_property;
        List<string> strlist=new List<string>();
        for ( int i = 0; i < list.Count; i++ )
            {
            strlist.Add ( list [ i ].ToString ( ) );
            }
        strlist.Sort ( );
        using (System.IO.StreamWriter sw=new System.IO.StreamWriter("hotfixlist") )
            {
            sw.WriteLine ( list.Count );
            for ( int i = 0; i < strlist.Count; i++ )
                {
                Debug.Log ( strlist [ i ] );
                sw.WriteLine ( strlist [ i ] );
                }
            Debug.Log ( "finish" );
            }
     

        }

    [MenuItem ( "XLua/logNamespace", false, 1 )]
    public static void logNamespace ( )
        {
        var list=  ( from type in Assembly.Load ( "Assembly-CSharp" ).GetTypes ( ) select type.Namespace);
        list= list.Distinct<string> ( ).ToList<string> ( );
        foreach ( var item in list )
            {
            Debug.Log ( item );
            }
        }

    [MenuItem ( "XLua/CreateHotfixJson", false, 1 )]
    public static void CreateHotfixJson ( )
        {
        LitJson.JsonWriter jw=new LitJson.JsonWriter();
        jw.WriteObjectStart ( );
        jw.WritePropertyName ( "files" );
        jw.WriteArrayStart ( );
      var assets=  AssetDatabase.FindAssets ("",new string[] { "Assets/Resources_MS/LuaScripts/hotfix" } );
        for ( int i = 0; i < assets.Length; i++ )
            {
            jw.Write ( AssetDatabase.LoadAssetAtPath<TextAsset> ( AssetDatabase.GUIDToAssetPath ( assets [ i ] ) ).name );

            }
        jw.WriteArrayEnd ( );
        jw.WriteObjectEnd ( );
        var file=System.IO.Path.Combine( Application.dataPath,"Resources_MS/res/tables1/client/hotfix.json");
        Debug.Log ( file );
        if ( System.IO.File.Exists(file) )
            {
            System.IO.File.Delete ( file );
            }
        using ( System.IO.StreamWriter sw = new System.IO.StreamWriter ( file ) )
            {
            sw.Write ( jw.ToString ( ) );
            }
        }
#endif
    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>() {
        new List<string> () { "UnityEngine.WWW", "movie" },
#if UNITY_WEBGL
        new List<string> () { "UnityEngine.WWW", "threadPriority" },
#endif
        new List<string> () { "UnityEngine.Texture2D", "alphaIsTransparency" },
        new List<string> () { "UnityEngine.Security", "GetChainOfTrustValue" },
        new List<string> () { "UnityEngine.CanvasRenderer", "onRequestRebuild" },
        new List<string> () { "UnityEngine.Light", "areaSize" },
        new List<string> () { "UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup" },
#if !UNITY_WEBPLAYER
        new List<string> () { "UnityEngine.Application", "ExternalEval" },
#endif
        new List<string> () { "UIInput", "ProcessEvent","UnityEngine.Event" },
        new List<string> () { "UIWidget", "showHandles" },
        new List<string> () { "UIWidget", "showHandlesWithMoveTool" },
        new List<string> () { "UnityEngine.GameObject", "networkView" }, //4.6.2 not support
        new List<string> () { "UnityEngine.Component", "networkView" }, //4.6.2 not support
        new List<string> () { "System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections" },
        new List<string> () { "System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity" },
        new List<string> () { "System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections" },
        new List<string> () { "System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity" },
        new List<string> () { "System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity" },
        new List<string> () { "System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity" },
        new List<string> () { "UnityEngine.MonoBehaviour", "runInEditMode" },
         //new List<string> () { "UISlider", "TweenTo", "System.Single", "System.Single", "System.Boolean","System.Single" },
         // new List<string> () { "UISlider", "TweenTo", "System.Single", "System.Single", "System.Single" },
           new List<string> () { "CellMgr", "build"},
    };
}