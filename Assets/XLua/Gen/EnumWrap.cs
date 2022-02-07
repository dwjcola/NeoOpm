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
    
    public class UnityEngineWrapModeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(UnityEngine.WrapMode), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(UnityEngine.WrapMode), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(UnityEngine.WrapMode), L, null, 7, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Once", UnityEngine.WrapMode.Once);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Loop", UnityEngine.WrapMode.Loop);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PingPong", UnityEngine.WrapMode.PingPong);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Default", UnityEngine.WrapMode.Default);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ClampForever", UnityEngine.WrapMode.ClampForever);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Clamp", UnityEngine.WrapMode.Clamp);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(UnityEngine.WrapMode), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushUnityEngineWrapMode(L, (UnityEngine.WrapMode)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Once"))
                {
                    translator.PushUnityEngineWrapMode(L, UnityEngine.WrapMode.Once);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Loop"))
                {
                    translator.PushUnityEngineWrapMode(L, UnityEngine.WrapMode.Loop);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "PingPong"))
                {
                    translator.PushUnityEngineWrapMode(L, UnityEngine.WrapMode.PingPong);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Default"))
                {
                    translator.PushUnityEngineWrapMode(L, UnityEngine.WrapMode.Default);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "ClampForever"))
                {
                    translator.PushUnityEngineWrapMode(L, UnityEngine.WrapMode.ClampForever);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Clamp"))
                {
                    translator.PushUnityEngineWrapMode(L, UnityEngine.WrapMode.Clamp);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for UnityEngine.WrapMode!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for UnityEngine.WrapMode! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningLoopTypeWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.LoopType), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.LoopType), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.LoopType), L, null, 4, 0, 0);

            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Restart", DG.Tweening.LoopType.Restart);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Yoyo", DG.Tweening.LoopType.Yoyo);
            
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "Incremental", DG.Tweening.LoopType.Incremental);
            

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.LoopType), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningLoopType(L, (DG.Tweening.LoopType)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

			    if (LuaAPI.xlua_is_eq_str(L, 1, "Restart"))
                {
                    translator.PushDGTweeningLoopType(L, DG.Tweening.LoopType.Restart);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Yoyo"))
                {
                    translator.PushDGTweeningLoopType(L, DG.Tweening.LoopType.Yoyo);
                }
				else if (LuaAPI.xlua_is_eq_str(L, 1, "Incremental"))
                {
                    translator.PushDGTweeningLoopType(L, DG.Tweening.LoopType.Incremental);
                }
				else
                {
                    return LuaAPI.luaL_error(L, "invalid string for DG.Tweening.LoopType!");
                }

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.LoopType! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
    public class DGTweeningEaseWrap
    {
		public static void __Register(RealStatePtr L)
        {
		    ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		    Utils.BeginObjectRegister(typeof(DG.Tweening.Ease), L, translator, 0, 0, 0, 0);
			Utils.EndObjectRegister(typeof(DG.Tweening.Ease), L, translator, null, null, null, null, null);
			
			Utils.BeginClassRegister(typeof(DG.Tweening.Ease), L, null, 39, 0, 0);

            Utils.RegisterEnumType(L, typeof(DG.Tweening.Ease));

			Utils.RegisterFunc(L, Utils.CLS_IDX, "__CastFrom", __CastFrom);
            
            Utils.EndClassRegister(typeof(DG.Tweening.Ease), L, translator);
        }
		
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CastFrom(RealStatePtr L)
		{
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			LuaTypes lua_type = LuaAPI.lua_type(L, 1);
            if (lua_type == LuaTypes.LUA_TNUMBER)
            {
                translator.PushDGTweeningEase(L, (DG.Tweening.Ease)LuaAPI.xlua_tointeger(L, 1));
            }
			
            else if(lua_type == LuaTypes.LUA_TSTRING)
            {

                try
				{
                    translator.TranslateToEnumToTop(L, typeof(DG.Tweening.Ease), 1);
				}
				catch (System.Exception e)
				{
					return LuaAPI.luaL_error(L, "cast to " + typeof(DG.Tweening.Ease) + " exception:" + e);
				}

            }
			
            else
            {
                return LuaAPI.luaL_error(L, "invalid lua type for DG.Tweening.Ease! Expect number or string, got + " + lua_type);
            }

            return 1;
		}
	}
    
}