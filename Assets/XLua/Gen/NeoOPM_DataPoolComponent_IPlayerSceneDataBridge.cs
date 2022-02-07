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
using System;


namespace XLua.CSObjectWrap
{
    public class NeoOPMDataPoolComponentIPlayerSceneDataBridge : LuaBase, NeoOPM.DataPoolComponent.IPlayerSceneData
    {
	    public static LuaBase __Create(int reference, LuaEnv luaenv)
		{
		    return new NeoOPMDataPoolComponentIPlayerSceneDataBridge(reference, luaenv);
		}
		
		public NeoOPMDataPoolComponentIPlayerSceneDataBridge(int reference, LuaEnv luaenv) : base(reference, luaenv)
        {
        }
		
        
		SLG.WsTroopBasicInfo NeoOPM.DataPoolComponent.IPlayerSceneData.GetSelfTroopDataById(ulong entityId)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
				RealStatePtr L = luaEnv.L;
				int err_func = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				ObjectTranslator translator = luaEnv.translator;
				
				LuaAPI.lua_getref(L, luaReference);
				LuaAPI.xlua_pushasciistring(L, "GetSelfTroopDataById");
				if (0 != LuaAPI.xlua_pgettable(L, -2))
				{
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				if(!LuaAPI.lua_isfunction(L, -1))
				{
					LuaAPI.xlua_pushasciistring(L, "no such function GetSelfTroopDataById");
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				LuaAPI.lua_pushvalue(L, -2);
				LuaAPI.lua_remove(L, -3);
				LuaAPI.lua_pushuint64(L, entityId);
				
				int __gen_error = LuaAPI.lua_pcall(L, 2, 1, err_func);
				if (__gen_error != 0)
					luaEnv.ThrowExceptionFromError(err_func - 1);
				
				
				SLG.WsTroopBasicInfo __gen_ret = (SLG.WsTroopBasicInfo)translator.GetObject(L, err_func + 1, typeof(SLG.WsTroopBasicInfo));
				LuaAPI.lua_settop(L, err_func - 1);
				return  __gen_ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		SLG.WsObjInfo NeoOPM.DataPoolComponent.IPlayerSceneData.GetCastleInfo()
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
				RealStatePtr L = luaEnv.L;
				int err_func = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				ObjectTranslator translator = luaEnv.translator;
				
				LuaAPI.lua_getref(L, luaReference);
				LuaAPI.xlua_pushasciistring(L, "GetCastleInfo");
				if (0 != LuaAPI.xlua_pgettable(L, -2))
				{
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				if(!LuaAPI.lua_isfunction(L, -1))
				{
					LuaAPI.xlua_pushasciistring(L, "no such function GetCastleInfo");
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				LuaAPI.lua_pushvalue(L, -2);
				LuaAPI.lua_remove(L, -3);
				
				int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
				if (__gen_error != 0)
					luaEnv.ThrowExceptionFromError(err_func - 1);
				
				
				SLG.WsObjInfo __gen_ret = (SLG.WsObjInfo)translator.GetObject(L, err_func + 1, typeof(SLG.WsObjInfo));
				LuaAPI.lua_settop(L, err_func - 1);
				return  __gen_ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		System.Nullable<bool> NeoOPM.DataPoolComponent.IPlayerSceneData.IsSelfTroopInBattle(ulong entityId)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
				RealStatePtr L = luaEnv.L;
				int err_func = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				ObjectTranslator translator = luaEnv.translator;
				
				LuaAPI.lua_getref(L, luaReference);
				LuaAPI.xlua_pushasciistring(L, "IsSelfTroopInBattle");
				if (0 != LuaAPI.xlua_pgettable(L, -2))
				{
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				if(!LuaAPI.lua_isfunction(L, -1))
				{
					LuaAPI.xlua_pushasciistring(L, "no such function IsSelfTroopInBattle");
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				LuaAPI.lua_pushvalue(L, -2);
				LuaAPI.lua_remove(L, -3);
				LuaAPI.lua_pushuint64(L, entityId);
				
				int __gen_error = LuaAPI.lua_pcall(L, 2, 1, err_func);
				if (__gen_error != 0)
					luaEnv.ThrowExceptionFromError(err_func - 1);
				
				
				System.Nullable<bool> __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
				LuaAPI.lua_settop(L, err_func - 1);
				return  __gen_ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		System.Nullable<bool> NeoOPM.DataPoolComponent.IPlayerSceneData.IsSelfCityInBattle()
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
				RealStatePtr L = luaEnv.L;
				int err_func = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				ObjectTranslator translator = luaEnv.translator;
				
				LuaAPI.lua_getref(L, luaReference);
				LuaAPI.xlua_pushasciistring(L, "IsSelfCityInBattle");
				if (0 != LuaAPI.xlua_pgettable(L, -2))
				{
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				if(!LuaAPI.lua_isfunction(L, -1))
				{
					LuaAPI.xlua_pushasciistring(L, "no such function IsSelfCityInBattle");
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				LuaAPI.lua_pushvalue(L, -2);
				LuaAPI.lua_remove(L, -3);
				
				int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
				if (__gen_error != 0)
					luaEnv.ThrowExceptionFromError(err_func - 1);
				
				
				System.Nullable<bool> __gen_ret;translator.Get(L, err_func + 1, out __gen_ret);
				LuaAPI.lua_settop(L, err_func - 1);
				return  __gen_ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        
		ulong NeoOPM.DataPoolComponent.IPlayerSceneData.GetCastleEntityId()
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
				RealStatePtr L = luaEnv.L;
				int err_func = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				
				
				LuaAPI.lua_getref(L, luaReference);
				LuaAPI.xlua_pushasciistring(L, "GetCastleEntityId");
				if (0 != LuaAPI.xlua_pgettable(L, -2))
				{
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				if(!LuaAPI.lua_isfunction(L, -1))
				{
					LuaAPI.xlua_pushasciistring(L, "no such function GetCastleEntityId");
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				LuaAPI.lua_pushvalue(L, -2);
				LuaAPI.lua_remove(L, -3);
				
				int __gen_error = LuaAPI.lua_pcall(L, 1, 1, err_func);
				if (__gen_error != 0)
					luaEnv.ThrowExceptionFromError(err_func - 1);
				
				
				ulong __gen_ret = LuaAPI.lua_touint64(L, err_func + 1);
				LuaAPI.lua_settop(L, err_func - 1);
				return  __gen_ret;
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        

        
        
        
		
		
	}
}
