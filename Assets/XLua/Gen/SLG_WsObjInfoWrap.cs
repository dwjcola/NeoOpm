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
    public class SLGWsObjInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(SLG.WsObjInfo);
			Utils.BeginObjectRegister(type, L, translator, 0, 7, 25, 24);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Clone", _m_Clone);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Equals", _m_Equals);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHashCode", _m_GetHashCode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ToString", _m_ToString);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "WriteTo", _m_WriteTo);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CalculateSize", _m_CalculateSize);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MergeFrom", _m_MergeFrom);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "EntityId", _g_get_EntityId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Type", _g_get_Type);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Level", _g_get_Level);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PosX", _g_get_PosX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PosY", _g_get_PosY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnionId", _g_get_UnionId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnionName", _g_get_UnionName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "UnionBadge", _g_get_UnionBadge);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayerId", _g_get_PlayerId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayerName", _g_get_PlayerName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "State", _g_get_State);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ResId", _g_get_ResId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartPosX", _g_get_StartPosX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "StartPosY", _g_get_StartPosY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EndPosX", _g_get_EndPosX);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "EndPosY", _g_get_EndPosY);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Movespeed", _g_get_Movespeed);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "BattleId", _g_get_BattleId);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TransferGate1", _g_get_TransferGate1);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TransferGate2", _g_get_TransferGate2);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "TimeMs", _g_get_TimeMs);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PlayerBuildings", _g_get_PlayerBuildings);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "Path", _g_get_Path);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PathIndex", _g_get_PathIndex);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "SummonerId", _g_get_SummonerId);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "EntityId", _s_set_EntityId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Type", _s_set_Type);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Level", _s_set_Level);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PosX", _s_set_PosX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PosY", _s_set_PosY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnionId", _s_set_UnionId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnionName", _s_set_UnionName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "UnionBadge", _s_set_UnionBadge);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PlayerId", _s_set_PlayerId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PlayerName", _s_set_PlayerName);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "State", _s_set_State);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "ResId", _s_set_ResId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StartPosX", _s_set_StartPosX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "StartPosY", _s_set_StartPosY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EndPosX", _s_set_EndPosX);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "EndPosY", _s_set_EndPosY);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "Movespeed", _s_set_Movespeed);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "BattleId", _s_set_BattleId);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TransferGate1", _s_set_TransferGate1);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TransferGate2", _s_set_TransferGate2);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "TimeMs", _s_set_TimeMs);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PlayerBuildings", _s_set_PlayerBuildings);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "PathIndex", _s_set_PathIndex);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "SummonerId", _s_set_SummonerId);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 26, 2, 0);
			
			
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EntityIdFieldNumber", SLG.WsObjInfo.EntityIdFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TypeFieldNumber", SLG.WsObjInfo.TypeFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "LevelFieldNumber", SLG.WsObjInfo.LevelFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PosXFieldNumber", SLG.WsObjInfo.PosXFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PosYFieldNumber", SLG.WsObjInfo.PosYFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnionIdFieldNumber", SLG.WsObjInfo.UnionIdFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnionNameFieldNumber", SLG.WsObjInfo.UnionNameFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "UnionBadgeFieldNumber", SLG.WsObjInfo.UnionBadgeFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PlayerIdFieldNumber", SLG.WsObjInfo.PlayerIdFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PlayerNameFieldNumber", SLG.WsObjInfo.PlayerNameFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StateFieldNumber", SLG.WsObjInfo.StateFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "ResIdFieldNumber", SLG.WsObjInfo.ResIdFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StartPosXFieldNumber", SLG.WsObjInfo.StartPosXFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "StartPosYFieldNumber", SLG.WsObjInfo.StartPosYFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EndPosXFieldNumber", SLG.WsObjInfo.EndPosXFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "EndPosYFieldNumber", SLG.WsObjInfo.EndPosYFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "MovespeedFieldNumber", SLG.WsObjInfo.MovespeedFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "BattleIdFieldNumber", SLG.WsObjInfo.BattleIdFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TransferGate1FieldNumber", SLG.WsObjInfo.TransferGate1FieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TransferGate2FieldNumber", SLG.WsObjInfo.TransferGate2FieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "TimeMsFieldNumber", SLG.WsObjInfo.TimeMsFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PlayerBuildingsFieldNumber", SLG.WsObjInfo.PlayerBuildingsFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PathFieldNumber", SLG.WsObjInfo.PathFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "PathIndexFieldNumber", SLG.WsObjInfo.PathIndexFieldNumber);
            Utils.RegisterObject(L, translator, Utils.CLS_IDX, "SummonerIdFieldNumber", SLG.WsObjInfo.SummonerIdFieldNumber);
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Parser", _g_get_Parser);
            Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "Descriptor", _g_get_Descriptor);
            
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new SLG.WsObjInfo();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 2 && translator.Assignable<SLG.WsObjInfo>(L, 2))
				{
					SLG.WsObjInfo _other = (SLG.WsObjInfo)translator.GetObject(L, 2, typeof(SLG.WsObjInfo));
					
					var gen_ret = new SLG.WsObjInfo(_other);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to SLG.WsObjInfo constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Clone(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.Clone(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Equals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<object>(L, 2)) 
                {
                    object _other = translator.GetObject(L, 2, typeof(object));
                    
                        var gen_ret = gen_to_be_invoked.Equals( _other );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 2&& translator.Assignable<SLG.WsObjInfo>(L, 2)) 
                {
                    SLG.WsObjInfo _other = (SLG.WsObjInfo)translator.GetObject(L, 2, typeof(SLG.WsObjInfo));
                    
                        var gen_ret = gen_to_be_invoked.Equals( _other );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to SLG.WsObjInfo.Equals!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHashCode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.GetHashCode(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ToString(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ToString(  );
                        LuaAPI.lua_pushstring(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_WriteTo(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    Google.Protobuf.CodedOutputStream _output = (Google.Protobuf.CodedOutputStream)translator.GetObject(L, 2, typeof(Google.Protobuf.CodedOutputStream));
                    
                    gen_to_be_invoked.WriteTo( _output );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CalculateSize(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.CalculateSize(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MergeFrom(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<SLG.WsObjInfo>(L, 2)) 
                {
                    SLG.WsObjInfo _other = (SLG.WsObjInfo)translator.GetObject(L, 2, typeof(SLG.WsObjInfo));
                    
                    gen_to_be_invoked.MergeFrom( _other );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 2&& translator.Assignable<Google.Protobuf.CodedInputStream>(L, 2)) 
                {
                    Google.Protobuf.CodedInputStream _input = (Google.Protobuf.CodedInputStream)translator.GetObject(L, 2, typeof(Google.Protobuf.CodedInputStream));
                    
                    gen_to_be_invoked.MergeFrom( _input );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to SLG.WsObjInfo.MergeFrom!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Parser(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, SLG.WsObjInfo.Parser);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Descriptor(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			    translator.Push(L, SLG.WsObjInfo.Descriptor);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EntityId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.EntityId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Type(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushuint(L, gen_to_be_invoked.Type);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Level(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.Level);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PosX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.PosY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnionId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.UnionId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnionName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.UnionName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_UnionBadge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.UnionBadge);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.PlayerId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.PlayerName);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_State(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.State);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ResId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.ResId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartPosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.StartPosX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_StartPosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.StartPosY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EndPosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.EndPosX);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_EndPosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.EndPosY);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Movespeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushnumber(L, gen_to_be_invoked.Movespeed);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_BattleId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.BattleId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TransferGate1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.TransferGate1);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TransferGate2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.TransferGate2);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_TimeMs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.TimeMs);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PlayerBuildings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.PlayerBuildings);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_Path(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.Path);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PathIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.PathIndex);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_SummonerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushuint64(L, gen_to_be_invoked.SummonerId);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EntityId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EntityId = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Type(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Type = LuaAPI.xlua_touint(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Level(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Level = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PosX = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PosY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnionId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UnionId = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnionName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UnionName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_UnionBadge(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.UnionBadge = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PlayerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PlayerId = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PlayerName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PlayerName = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_State(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.State = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_ResId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.ResId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StartPosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StartPosX = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_StartPosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.StartPosY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EndPosX(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EndPosX = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_EndPosY(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.EndPosY = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_Movespeed(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.Movespeed = (float)LuaAPI.lua_tonumber(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_BattleId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.BattleId = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TransferGate1(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TransferGate1 = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TransferGate2(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TransferGate2 = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_TimeMs(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.TimeMs = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PlayerBuildings(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PlayerBuildings = (SLG.BuildingsInfo)translator.GetObject(L, 2, typeof(SLG.BuildingsInfo));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PathIndex(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.PathIndex = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_SummonerId(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                SLG.WsObjInfo gen_to_be_invoked = (SLG.WsObjInfo)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.SummonerId = LuaAPI.lua_touint64(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
