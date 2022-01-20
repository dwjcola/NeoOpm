#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class LCWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(LC);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 52, 2, 1);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "AddUIEvent", _m_AddUIEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddUIDataEvent", _m_AddUIDataEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddTabSelectEvent", _m_AddTabSelectEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Log", _m_Log_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Error", _m_Error_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Warning", _m_Warning_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CloseUI", _m_CloseUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetTable", _m_GetTable_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CallLuaFunc", _m_CallLuaFunc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUICamera", _m_GetUICamera_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "WorldPosToScreenLocalPos", _m_WorldPosToScreenLocalPos_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ChangeStateTo", _m_ChangeStateTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowOrHideUIGroup", _m_ShowOrHideUIGroup_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "OpenUI", _m_OpenUI_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ShowPartPanel", _m_ShowPartPanel_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUITable", _m_GetUITable_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateItem", _m_CreateItem_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreatePrefab", _m_CreatePrefab_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CreateGame", _m_CreateGame_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddCSEvent", _m_AddCSEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveCSEvent", _m_RemoveCSEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Subscribe", _m_Subscribe_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "Unsubscribe", _m_Unsubscribe_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddEvent", _m_AddEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SendEvent", _m_SendEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "RemoveEvent", _m_RemoveEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetImageSprite", _m_SetImageSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRawImageSprite", _m_SetRawImageSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSpriteRenderSprite", _m_SetSpriteRenderSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSingleSpriteByFolderPath", _m_SetSingleSpriteByFolderPath_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetSprite", _m_SetSprite_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetMaterial", _m_SetMaterial_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "SetRawImageMaterial", _m_SetRawImageMaterial_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearMaterial", _m_ClearMaterial_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetUILogicByKey", _m_GetUILogicByKey_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadSpine", _m_LoadSpine_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadSpineAnimation", _m_LoadSpineAnimation_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadBattleScene", _m_LoadBattleScene_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadAsset", _m_LoadAsset_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddButtonEvent", _m_AddButtonEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddInputEvent", _m_AddInputEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddInputEditEvent", _m_AddInputEditEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddDropDownEvent", _m_AddDropDownEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddSliderEvent", _m_AddSliderEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "AddToggleEvent", _m_AddToggleEvent_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadText", _m_LoadText_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "LoadGuide", _m_LoadGuide_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ClearAll", _m_ClearAll_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "ReconnectSucc", _m_ReconnectSucc_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetType", _m_GetType_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "GetOrAddComponent", _m_GetOrAddComponent_xlua_st_);
            
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ResMgr", _g_get_ResMgr);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "ISDEBUG", _g_get_ISDEBUG);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "ISDEBUG", _s_set_ISDEBUG);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new LC();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to LC constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUIEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _luaClass = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    string _key = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    System.Action<XLua.LuaTable, UnityEngine.GameObject> _callBack = translator.GetDelegate<System.Action<XLua.LuaTable, UnityEngine.GameObject>>(L, 4);
                    
                    LC.AddUIEvent( _luaClass, _key, _go, _callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddUIDataEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _luaClass = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    string _key = LuaAPI.lua_tostring(L, 2);
                    UnityEngine.GameObject _go = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    System.Action<XLua.LuaTable, UnityEngine.GameObject, XLua.LuaTable> _callBack = translator.GetDelegate<System.Action<XLua.LuaTable, UnityEngine.GameObject, XLua.LuaTable>>(L, 4);
                    XLua.LuaTable _data = (XLua.LuaTable)translator.GetObject(L, 5, typeof(XLua.LuaTable));
                    
                    LC.AddUIDataEvent( _luaClass, _key, _go, _callBack, _data );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTabSelectEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    NeoOPM.CToggleGroup _tab = (NeoOPM.CToggleGroup)translator.GetObject(L, 2, typeof(NeoOPM.CToggleGroup));
                    System.Action<XLua.LuaTable, int> _back = translator.GetDelegate<System.Action<XLua.LuaTable, int>>(L, 3);
                    
                    LC.AddTabSelectEvent( _lua, _tab, _back );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Log_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _message = translator.GetObject(L, 1, typeof(object));
                    
                    LC.Log( _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Error_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _error = translator.GetObject(L, 1, typeof(object));
                    
                    LC.Error( _error );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Warning_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _warning = translator.GetObject(L, 1, typeof(object));
                    
                    LC.Warning( _warning );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    
                    LC.CloseUI( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetTable_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 2)) 
                {
                    string _tableName = LuaAPI.lua_tostring(L, 1);
                    int _key = LuaAPI.xlua_tointeger(L, 2);
                    
                        var gen_ret = LC.GetTable( _tableName, _key );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _tableName = LuaAPI.lua_tostring(L, 1);
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = LC.GetTable( _tableName, _key );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.GetTable!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CallLuaFunc_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _fileName = LuaAPI.lua_tostring(L, 1);
                    string _funcName = LuaAPI.lua_tostring(L, 2);
                    
                    LC.CallLuaFunc( _fileName, _funcName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUICamera_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = LC.GetUICamera(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WorldPosToScreenLocalPos_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Camera>(L, 1)&& translator.Assignable<UnityEngine.Camera>(L, 2)&& translator.Assignable<UnityEngine.RectTransform>(L, 3)&& translator.Assignable<UnityEngine.Vector3>(L, 4)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    UnityEngine.RectTransform _rectangle = (UnityEngine.RectTransform)translator.GetObject(L, 3, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector3 _target;translator.Get(L, 4, out _target);
                    
                        var gen_ret = LC.WorldPosToScreenLocalPos( _camera, _uiCamera, _rectangle, _target );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.Camera>(L, 1)&& translator.Assignable<UnityEngine.Camera>(L, 2)&& translator.Assignable<UnityEngine.RectTransform>(L, 3)&& translator.Assignable<UnityEngine.Transform>(L, 4)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    UnityEngine.RectTransform _rectangle = (UnityEngine.RectTransform)translator.GetObject(L, 3, typeof(UnityEngine.RectTransform));
                    UnityEngine.Transform _targetTR = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    
                        var gen_ret = LC.WorldPosToScreenLocalPos( _camera, _uiCamera, _rectangle, _targetTR );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& translator.Assignable<UnityEngine.Camera>(L, 1)&& translator.Assignable<UnityEngine.Camera>(L, 2)&& translator.Assignable<UnityEngine.RectTransform>(L, 3)&& translator.Assignable<UnityEngine.Transform>(L, 4)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 5)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 6)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 7)) 
                {
                    UnityEngine.Camera _camera = (UnityEngine.Camera)translator.GetObject(L, 1, typeof(UnityEngine.Camera));
                    UnityEngine.Camera _uiCamera = (UnityEngine.Camera)translator.GetObject(L, 2, typeof(UnityEngine.Camera));
                    UnityEngine.RectTransform _rectangle = (UnityEngine.RectTransform)translator.GetObject(L, 3, typeof(UnityEngine.RectTransform));
                    UnityEngine.Transform _targetTR = (UnityEngine.Transform)translator.GetObject(L, 4, typeof(UnityEngine.Transform));
                    float _offsetX = (float)LuaAPI.lua_tonumber(L, 5);
                    float _offsetY = (float)LuaAPI.lua_tonumber(L, 6);
                    float _offsetZ = (float)LuaAPI.lua_tonumber(L, 7);
                    
                        var gen_ret = LC.WorldPosToScreenLocalPos( _camera, _uiCamera, _rectangle, _targetTR, _offsetX, _offsetY, _offsetZ );
                        translator.PushUnityEngineVector2(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.WorldPosToScreenLocalPos!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ChangeStateTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    System.Type _procedureType = (System.Type)translator.GetObject(L, 1, typeof(System.Type));
                    
                    LC.ChangeStateTo( _procedureType );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowOrHideUIGroup_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    string _groupName = LuaAPI.lua_tostring(L, 1);
                    bool _b = LuaAPI.lua_toboolean(L, 2);
                    
                    LC.ShowOrHideUIGroup( _groupName, _b );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenUI_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 2)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    object _para = translator.GetObject(L, 2, typeof(object));
                    
                        var gen_ret = LC.OpenUI( _key, _para );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LC.OpenUI( _key );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.OpenUI!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowPartPanel_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    object _para = translator.GetObject(L, 3, typeof(object));
                    
                        var gen_ret = LC.ShowPartPanel( _key, _parent, _para );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<UnityEngine.Transform>(L, 2)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                        var gen_ret = LC.ShowPartPanel( _key, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.ShowPartPanel!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUITable_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _uiKey = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LC.GetUITable( _uiKey );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateItem_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _item = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                        var gen_ret = LC.CreateItem( _item, _parent, _lua );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreatePrefab_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _prefab = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    
                        var gen_ret = LC.CreatePrefab( _prefab, _parent );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateGame_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    
                        var gen_ret = LC.CreateGame(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddCSEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _luaClass = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    int _key = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<XLua.LuaTable, object> _callBack = translator.GetDelegate<System.Action<XLua.LuaTable, object>>(L, 3);
                    
                    LC.AddCSEvent( _luaClass, _key, _callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveCSEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _luaClass = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    int _key = LuaAPI.xlua_tointeger(L, 2);
                    System.Action<XLua.LuaTable, object> _callBack = translator.GetDelegate<System.Action<XLua.LuaTable, object>>(L, 3);
                    
                    LC.RemoveCSEvent( _luaClass, _key, _callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Subscribe_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    System.EventHandler<GameFramework.Event.GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameFramework.Event.GameEventArgs>>(L, 2);
                    
                    LC.Subscribe( _key, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Unsubscribe_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    System.EventHandler<GameFramework.Event.GameEventArgs> _handler = translator.GetDelegate<System.EventHandler<GameFramework.Event.GameEventArgs>>(L, 2);
                    
                    LC.Unsubscribe( _key, _handler );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _luaClass = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    string _key = LuaAPI.lua_tostring(L, 2);
                    System.Action<XLua.LuaTable, object> _callBack = translator.GetDelegate<System.Action<XLua.LuaTable, object>>(L, 3);
                    
                    LC.AddEvent( _luaClass, _key, _callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 2)&& translator.Assignable<object>(L, 3)&& translator.Assignable<object>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    object _data = translator.GetObject(L, 2, typeof(object));
                    object _data2 = translator.GetObject(L, 3, typeof(object));
                    object _data3 = translator.GetObject(L, 4, typeof(object));
                    object _data4 = translator.GetObject(L, 5, typeof(object));
                    
                    LC.SendEvent( _key, _data, _data2, _data3, _data4 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 2)&& translator.Assignable<object>(L, 3)&& translator.Assignable<object>(L, 4)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    object _data = translator.GetObject(L, 2, typeof(object));
                    object _data2 = translator.GetObject(L, 3, typeof(object));
                    object _data3 = translator.GetObject(L, 4, typeof(object));
                    
                    LC.SendEvent( _key, _data, _data2, _data3 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    object _data = translator.GetObject(L, 2, typeof(object));
                    object _data2 = translator.GetObject(L, 3, typeof(object));
                    
                    LC.SendEvent( _key, _data, _data2 );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)&& translator.Assignable<object>(L, 2)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    object _data = translator.GetObject(L, 2, typeof(object));
                    
                    LC.SendEvent( _key, _data );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 1&& (LuaAPI.lua_isnil(L, 1) || LuaAPI.lua_type(L, 1) == LuaTypes.LUA_TSTRING)) 
                {
                    string _key = LuaAPI.lua_tostring(L, 1);
                    
                    LC.SendEvent( _key );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.SendEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _luaClass = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    string _key = LuaAPI.lua_tostring(L, 2);
                    System.Action<XLua.LuaTable, object> _callBack = translator.GetDelegate<System.Action<XLua.LuaTable, object>>(L, 3);
                    
                    LC.RemoveEvent( _luaClass, _key, _callBack );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetImageSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Image>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _atlas = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    bool _native = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.SetImageSprite( _sp, _atlas, _spname, _native );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Image>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _atlas = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    
                    LC.SetImageSprite( _sp, _atlas, _spname );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.SetImageSprite!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRawImageSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.RawImage>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityEngine.UI.RawImage _sp = (UnityEngine.UI.RawImage)translator.GetObject(L, 1, typeof(UnityEngine.UI.RawImage));
                    string _textureName = LuaAPI.lua_tostring(L, 2);
                    bool _native = LuaAPI.lua_toboolean(L, 3);
                    
                    LC.SetRawImageSprite( _sp, _textureName, _native );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.UI.RawImage>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.UI.RawImage _sp = (UnityEngine.UI.RawImage)translator.GetObject(L, 1, typeof(UnityEngine.UI.RawImage));
                    string _textureName = LuaAPI.lua_tostring(L, 2);
                    
                    LC.SetRawImageSprite( _sp, _textureName );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.SetRawImageSprite!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSpriteRenderSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.SpriteRenderer _spRender = (UnityEngine.SpriteRenderer)translator.GetObject(L, 1, typeof(UnityEngine.SpriteRenderer));
                    string _atlas = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    
                    LC.SetSpriteRenderSprite( _spRender, _atlas, _spname );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSingleSpriteByFolderPath_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Image>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _folderPath = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    bool _native = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.SetSingleSpriteByFolderPath( _sp, _folderPath, _spname, _native );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Image>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _folderPath = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    
                    LC.SetSingleSpriteByFolderPath( _sp, _folderPath, _spname );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.SetSingleSpriteByFolderPath!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetSprite_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Image>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _atlas = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    bool _native = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.SetSprite( _sp, _atlas, _spname, _native );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Image>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _atlas = LuaAPI.lua_tostring(L, 2);
                    string _spname = LuaAPI.lua_tostring(L, 3);
                    
                    LC.SetSprite( _sp, _atlas, _spname );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.SetSprite!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetMaterial_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                    LC.SetMaterial( _sp, _path );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetRawImageMaterial_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.RawImage _sp = (UnityEngine.UI.RawImage)translator.GetObject(L, 1, typeof(UnityEngine.UI.RawImage));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                    LC.SetRawImageMaterial( _sp, _path );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearMaterial_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Image _sp = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    
                    LC.ClearMaterial( _sp );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetUILogicByKey_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _uikey = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LC.GetUILogicByKey( _uikey );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSpine_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _spineName = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    System.Action<XLua.LuaTable, Spine.Unity.SkeletonGraphic> _callback = translator.GetDelegate<System.Action<XLua.LuaTable, Spine.Unity.SkeletonGraphic>>(L, 3);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    LC.LoadSpine( _spineName, _parent, _callback, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadSpineAnimation_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _spineName = LuaAPI.lua_tostring(L, 1);
                    UnityEngine.Transform _parent = (UnityEngine.Transform)translator.GetObject(L, 2, typeof(UnityEngine.Transform));
                    System.Action<XLua.LuaTable, Spine.Unity.SkeletonAnimation> _callback = translator.GetDelegate<System.Action<XLua.LuaTable, Spine.Unity.SkeletonAnimation>>(L, 3);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 4, typeof(XLua.LuaTable));
                    
                    LC.LoadSpineAnimation( _spineName, _parent, _callback, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadBattleScene_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _sceneName = LuaAPI.lua_tostring(L, 1);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 2, typeof(XLua.LuaTable));
                    System.Action<XLua.LuaTable, UnityEngine.GameObject> _action = translator.GetDelegate<System.Action<XLua.LuaTable, UnityEngine.GameObject>>(L, 3);
                    
                    LC.LoadBattleScene( _sceneName, _lua, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadAsset_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _assetPath = LuaAPI.lua_tostring(L, 1);
                    System.Action<UnityEngine.GameObject> _action = translator.GetDelegate<System.Action<UnityEngine.GameObject>>(L, 2);
                    
                    LC.LoadAsset( _assetPath, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddButtonEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Button>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, object>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.Button _btn = (UnityEngine.UI.Button)translator.GetObject(L, 1, typeof(UnityEngine.UI.Button));
                    System.Action<XLua.LuaTable, object> _action = translator.GetDelegate<System.Action<XLua.LuaTable, object>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddButtonEvent( _btn, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Button>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, object>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.UI.Button _btn = (UnityEngine.UI.Button)translator.GetObject(L, 1, typeof(UnityEngine.UI.Button));
                    System.Action<XLua.LuaTable, object> _action = translator.GetDelegate<System.Action<XLua.LuaTable, object>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddButtonEvent( _btn, _action, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.AddButtonEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddInputEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.InputField>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, string>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.InputField _input = (UnityEngine.UI.InputField)translator.GetObject(L, 1, typeof(UnityEngine.UI.InputField));
                    System.Action<XLua.LuaTable, string> _action = translator.GetDelegate<System.Action<XLua.LuaTable, string>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddInputEvent( _input, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.InputField>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, string>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.UI.InputField _input = (UnityEngine.UI.InputField)translator.GetObject(L, 1, typeof(UnityEngine.UI.InputField));
                    System.Action<XLua.LuaTable, string> _action = translator.GetDelegate<System.Action<XLua.LuaTable, string>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddInputEvent( _input, _action, _lua );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<TMPro.TMP_InputField>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, string>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    TMPro.TMP_InputField _input = (TMPro.TMP_InputField)translator.GetObject(L, 1, typeof(TMPro.TMP_InputField));
                    System.Action<XLua.LuaTable, string> _action = translator.GetDelegate<System.Action<XLua.LuaTable, string>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddInputEvent( _input, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<TMPro.TMP_InputField>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, string>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    TMPro.TMP_InputField _input = (TMPro.TMP_InputField)translator.GetObject(L, 1, typeof(TMPro.TMP_InputField));
                    System.Action<XLua.LuaTable, string> _action = translator.GetDelegate<System.Action<XLua.LuaTable, string>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddInputEvent( _input, _action, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.AddInputEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddInputEditEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<TMPro.TMP_InputField>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, string>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    TMPro.TMP_InputField _input = (TMPro.TMP_InputField)translator.GetObject(L, 1, typeof(TMPro.TMP_InputField));
                    System.Action<XLua.LuaTable, string> _action = translator.GetDelegate<System.Action<XLua.LuaTable, string>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddInputEditEvent( _input, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<TMPro.TMP_InputField>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, string>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    TMPro.TMP_InputField _input = (TMPro.TMP_InputField)translator.GetObject(L, 1, typeof(TMPro.TMP_InputField));
                    System.Action<XLua.LuaTable, string> _action = translator.GetDelegate<System.Action<XLua.LuaTable, string>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddInputEditEvent( _input, _action, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.AddInputEditEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddDropDownEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<TMPro.TMP_Dropdown>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, int>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    TMPro.TMP_Dropdown _dropdown = (TMPro.TMP_Dropdown)translator.GetObject(L, 1, typeof(TMPro.TMP_Dropdown));
                    System.Action<XLua.LuaTable, int> _action = translator.GetDelegate<System.Action<XLua.LuaTable, int>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddDropDownEvent( _dropdown, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<TMPro.TMP_Dropdown>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, int>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    TMPro.TMP_Dropdown _dropdown = (TMPro.TMP_Dropdown)translator.GetObject(L, 1, typeof(TMPro.TMP_Dropdown));
                    System.Action<XLua.LuaTable, int> _action = translator.GetDelegate<System.Action<XLua.LuaTable, int>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddDropDownEvent( _dropdown, _action, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.AddDropDownEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddSliderEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Slider>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, float>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.Slider _slider = (UnityEngine.UI.Slider)translator.GetObject(L, 1, typeof(UnityEngine.UI.Slider));
                    System.Action<XLua.LuaTable, float> _action = translator.GetDelegate<System.Action<XLua.LuaTable, float>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddSliderEvent( _slider, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Slider>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, float>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.UI.Slider _slider = (UnityEngine.UI.Slider)translator.GetObject(L, 1, typeof(UnityEngine.UI.Slider));
                    System.Action<XLua.LuaTable, float> _action = translator.GetDelegate<System.Action<XLua.LuaTable, float>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddSliderEvent( _slider, _action, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.AddSliderEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddToggleEvent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.UI.Toggle>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, bool>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.UI.Toggle _toggle = (UnityEngine.UI.Toggle)translator.GetObject(L, 1, typeof(UnityEngine.UI.Toggle));
                    System.Action<XLua.LuaTable, bool> _action = translator.GetDelegate<System.Action<XLua.LuaTable, bool>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    bool _isOverride = LuaAPI.lua_toboolean(L, 4);
                    
                    LC.AddToggleEvent( _toggle, _action, _lua, _isOverride );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.UI.Toggle>(L, 1)&& translator.Assignable<System.Action<XLua.LuaTable, bool>>(L, 2)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TTABLE)) 
                {
                    UnityEngine.UI.Toggle _toggle = (UnityEngine.UI.Toggle)translator.GetObject(L, 1, typeof(UnityEngine.UI.Toggle));
                    System.Action<XLua.LuaTable, bool> _action = translator.GetDelegate<System.Action<XLua.LuaTable, bool>>(L, 2);
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 3, typeof(XLua.LuaTable));
                    
                    LC.AddToggleEvent( _toggle, _action, _lua );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.AddToggleEvent!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadText_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _TextPath = LuaAPI.lua_tostring(L, 1);
                    System.Action<string> _action = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    LC.LoadText( _TextPath, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadGuide_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    XLua.LuaTable _lua = (XLua.LuaTable)translator.GetObject(L, 1, typeof(XLua.LuaTable));
                    System.Action<XLua.LuaTable, UnityEngine.GameObject, UnityEngine.GameObject> _action = translator.GetDelegate<System.Action<XLua.LuaTable, UnityEngine.GameObject, UnityEngine.GameObject>>(L, 2);
                    UnityEngine.GameObject _param = (UnityEngine.GameObject)translator.GetObject(L, 3, typeof(UnityEngine.GameObject));
                    
                    LC.LoadGuide( _lua, _action, _param );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAll_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LC.ClearAll(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReconnectSucc_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                    LC.ReconnectSucc(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetType_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _typeName = LuaAPI.lua_tostring(L, 1);
                    
                        var gen_ret = LC.GetType( _typeName );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetOrAddComponent_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _className = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = LC.GetOrAddComponent( _target, _className );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& translator.Assignable<System.Type>(L, 2)) 
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    System.Type _className = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        var gen_ret = LC.GetOrAddComponent( _target, _className );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityEngine.GameObject>(L, 1)&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    string _path = LuaAPI.lua_tostring(L, 2);
                    System.Type _className = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        var gen_ret = LC.GetOrAddComponent( _target, _path, _className );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to LC.GetOrAddComponent!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ResMgr(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.PushAny(L, LC.ResMgr);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ISDEBUG(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushboolean(L, LC.ISDEBUG);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ISDEBUG(RealStatePtr L)
        {
		    try {
                
			    LC.ISDEBUG = LuaAPI.lua_toboolean(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
