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
    public class NeoOPMDataPoolComponentIBuildingPlaneDataBridge : LuaBase, NeoOPM.DataPoolComponent.IBuildingPlaneData
    {
	    public static LuaBase __Create(int reference, LuaEnv luaenv)
		{
		    return new NeoOPMDataPoolComponentIBuildingPlaneDataBridge(reference, luaenv);
		}
		
		public NeoOPMDataPoolComponentIBuildingPlaneDataBridge(int reference, LuaEnv luaenv) : base(reference, luaenv)
        {
        }
		
        
		void NeoOPM.DataPoolComponent.IBuildingPlaneData.ChangeOneBlockPos(int posx, int posy, int id)
		{
#if THREAD_SAFE || HOTFIX_ENABLE
            lock (luaEnv.luaEnvLock)
            {
#endif
				RealStatePtr L = luaEnv.L;
				int err_func = LuaAPI.load_error_func(L, luaEnv.errorFuncRef);
				
				
				LuaAPI.lua_getref(L, luaReference);
				LuaAPI.xlua_pushasciistring(L, "ChangeOneBlockPos");
				if (0 != LuaAPI.xlua_pgettable(L, -2))
				{
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				if(!LuaAPI.lua_isfunction(L, -1))
				{
					LuaAPI.xlua_pushasciistring(L, "no such function ChangeOneBlockPos");
					luaEnv.ThrowExceptionFromError(err_func - 1);
				}
				LuaAPI.lua_pushvalue(L, -2);
				LuaAPI.lua_remove(L, -3);
				LuaAPI.xlua_pushinteger(L, posx);
				LuaAPI.xlua_pushinteger(L, posy);
				LuaAPI.xlua_pushinteger(L, id);
				
				int __gen_error = LuaAPI.lua_pcall(L, 4, 0, err_func);
				if (__gen_error != 0)
					luaEnv.ThrowExceptionFromError(err_func - 1);
				
				
				
				LuaAPI.lua_settop(L, err_func - 1);
				
#if THREAD_SAFE || HOTFIX_ENABLE
            }
#endif
		}
        

        
        
        
		
		
	}
}
