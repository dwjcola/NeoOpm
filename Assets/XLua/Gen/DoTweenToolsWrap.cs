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
    public class DoTweenToolsWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(DoTweenTools);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 0, 0);
			
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 35, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "getTweenTimeScale", _m_getTweenTimeScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "setTweenTimeScale", _m_setTweenTimeScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "pauseForTransform", _m_pauseForTransform_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "playForTransform", _m_playForTransform_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOSizeDelta", _m_DOSizeDelta_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenMoveTo", _m_DoTweenMoveTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenMoveToCall", _m_DoTweenMoveToCall_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOTweenLocalMoveTo", _m_DOTweenLocalMoveTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenRotateQuaternionTo", _m_DoTweenRotateQuaternionTo_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenMoveAndRoattion", _m_DoTweenMoveAndRoattion_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenColor", _m_DoTweenColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenImageColor", _m_DoTweenImageColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenTextColor", _m_DoTweenTextColor_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenTextColorAndCall", _m_DoTweenTextColorAndCall_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DoTweenImageColorAndCall", _m_DoTweenImageColorAndCall_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOWidth", _m_DOWidth_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOWidthAndCall", _m_DOWidthAndCall_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOFillAmount", _m_DOFillAmount_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOFillAmountAndCall", _m_DOFillAmountAndCall_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOFloatForLua", _m_DOFloatForLua_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOBrightnessForLua", _m_DOBrightnessForLua_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveX", _m_DOLocalMoveX_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveXOnComplete", _m_DOLocalMoveXOnComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMove", _m_DOLocalMove_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalMoveOnComplete", _m_DOLocalMoveOnComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScale", _m_DOScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOScaleOnComplete", _m_DOScaleOnComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IEnumeratorScale", _m_IEnumeratorScale_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalRotateOnComplete", _m_DOLocalRotateOnComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOLocalPathOnComplete", _m_DOLocalPathOnComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "DOPlay", _m_DOPlay_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TweenSettingsExtensions_OnComplete", _m_TweenSettingsExtensions_OnComplete_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "IsTweening", _m_IsTweening_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "CompleteTweenRightNow", _m_CompleteTweenRightNow_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new DoTweenTools();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to DoTweenTools constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_getTweenTimeScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    
                        var gen_ret = DoTweenTools.getTweenTimeScale(  );
                        LuaAPI.lua_pushnumber(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_setTweenTimeScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
            
            
                
                {
                    float _timeScale = (float)LuaAPI.lua_tonumber(L, 1);
                    
                    DoTweenTools.setTweenTimeScale( _timeScale );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_pauseForTransform_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                    DoTweenTools.pauseForTransform( _transform );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_playForTransform_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _transform = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                    DoTweenTools.playForTransform( _transform );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOSizeDelta_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 5&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)&& translator.Assignable<System.Action>(L, 5)) 
                {
                    UnityEngine.RectTransform _target = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 5);
                    
                    DoTweenTools.DOSizeDelta( _target, _to, _duration, _delay, _action );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 4&& translator.Assignable<UnityEngine.RectTransform>(L, 1)&& translator.Assignable<UnityEngine.Vector2>(L, 2)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 3)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 4)) 
                {
                    UnityEngine.RectTransform _target = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _to;translator.Get(L, 2, out _to);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    
                    DoTweenTools.DOSizeDelta( _target, _to, _duration, _delay );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DoTweenTools.DOSizeDelta!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenMoveTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _to;translator.Get(L, 3, out _to);
                    
                    DoTweenTools.DoTweenMoveTo( _target, _duration, _to );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenMoveToCall_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _to;translator.Get(L, 3, out _to);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    
                    DoTweenTools.DoTweenMoveToCall( _target, _duration, _to, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOTweenLocalMoveTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _to;translator.Get(L, 3, out _to);
                    System.Action _action = translator.GetDelegate<System.Action>(L, 4);
                    
                    DoTweenTools.DOTweenLocalMoveTo( _target, _duration, _to, _action );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenRotateQuaternionTo_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Quaternion _to;translator.Get(L, 3, out _to);
                    
                    DoTweenTools.DoTweenRotateQuaternionTo( _target, _duration, _to );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenMoveAndRoattion_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 2);
                    UnityEngine.Vector3 _pos;translator.Get(L, 3, out _pos);
                    UnityEngine.Quaternion _rotation;translator.Get(L, 4, out _rotation);
                    
                    DoTweenTools.DoTweenMoveAndRoattion( _target, _duration, _pos, _rotation );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Material _target = (UnityEngine.Material)translator.GetObject(L, 1, typeof(UnityEngine.Material));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    DoTweenTools.DoTweenColor( _target, _endValue, _duration );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenImageColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Image _target = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = DoTweenTools.DoTweenImageColor( _target, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenTextColor_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Text _target = (UnityEngine.UI.Text)translator.GetObject(L, 1, typeof(UnityEngine.UI.Text));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = DoTweenTools.DoTweenTextColor( _target, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenTextColorAndCall_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Text _target = (UnityEngine.UI.Text)translator.GetObject(L, 1, typeof(UnityEngine.UI.Text));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                        var gen_ret = DoTweenTools.DoTweenTextColorAndCall( _target, _endValue, _duration, _callback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DoTweenImageColorAndCall_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Image _target = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                        var gen_ret = DoTweenTools.DoTweenImageColorAndCall( _target, _endValue, _duration, _callback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOWidth_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _target = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endValue;translator.Get(L, 2, out _endValue);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = DoTweenTools.DOWidth( _target, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOWidthAndCall_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.RectTransform _target = (UnityEngine.RectTransform)translator.GetObject(L, 1, typeof(UnityEngine.RectTransform));
                    UnityEngine.Vector2 _endValue;translator.Get(L, 2, out _endValue);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                        var gen_ret = DoTweenTools.DOWidthAndCall( _target, _endValue, _duration, _callback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOFillAmount_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Image _target = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    
                        var gen_ret = DoTweenTools.DOFillAmount( _target, _endValue, _duration );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOFillAmountAndCall_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.UI.Image _target = (UnityEngine.UI.Image)translator.GetObject(L, 1, typeof(UnityEngine.UI.Image));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                        var gen_ret = DoTweenTools.DOFillAmountAndCall( _target, _endValue, _duration, _callback );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOFloatForLua_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    string _tag = LuaAPI.lua_tostring(L, 4);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 5);
                    
                    DoTweenTools.DOFloatForLua( _target, _endValue, _duration, _tag, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOBrightnessForLua_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.GameObject _target = (UnityEngine.GameObject)translator.GetObject(L, 1, typeof(UnityEngine.GameObject));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    string _tag = LuaAPI.lua_tostring(L, 4);
                    
                        var gen_ret = DoTweenTools.DOBrightnessForLua( _target, _endValue, _duration, _tag );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveX_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snap = LuaAPI.lua_toboolean(L, 4);
                    
                    DoTweenTools.DOLocalMoveX( _target, _endValue, _dur, _snap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveXOnComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    float _endValue = (float)LuaAPI.lua_tonumber(L, 2);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snap = LuaAPI.lua_toboolean(L, 4);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 5);
                    
                    DoTweenTools.DOLocalMoveXOnComplete( _target, _endValue, _dur, _snap, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMove_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snap = LuaAPI.lua_toboolean(L, 4);
                    
                    DoTweenTools.DOLocalMove( _target, _endValue, _dur, _snap );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalMoveOnComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    bool _snap = LuaAPI.lua_toboolean(L, 4);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 5);
                    
                    DoTweenTools.DOLocalMoveOnComplete( _target, _endValue, _dur, _snap, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    
                    DoTweenTools.DOScale( _target, _endValue, _dur );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOScaleOnComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    DoTweenTools.DOScaleOnComplete( _target, _endValue, _dur, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IEnumeratorScale_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _endValue;translator.Get(L, 2, out _endValue);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    float _delay = (float)LuaAPI.lua_tonumber(L, 4);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 5);
                    
                        var gen_ret = DoTweenTools.IEnumeratorScale( _target, _endValue, _dur, _delay, _callback );
                        translator.PushAny(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalRotateOnComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3 _roteValue;translator.Get(L, 2, out _roteValue);
                    float _dur = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    DoTweenTools.DOLocalRotateOnComplete( _target, _roteValue, _dur, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOLocalPathOnComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    UnityEngine.Vector3[] _path = (UnityEngine.Vector3[])translator.GetObject(L, 2, typeof(UnityEngine.Vector3[]));
                    float _duration = (float)LuaAPI.lua_tonumber(L, 3);
                    System.Action _callback = translator.GetDelegate<System.Action>(L, 4);
                    
                    DoTweenTools.DOLocalPathOnComplete( _target, _path, _duration, _callback );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_DOPlay_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    UnityEngine.Transform _target = (UnityEngine.Transform)translator.GetObject(L, 1, typeof(UnityEngine.Transform));
                    
                    DoTweenTools.DOPlay( _target );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TweenSettingsExtensions_OnComplete_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    DG.Tweening.Tweener _target = (DG.Tweening.Tweener)translator.GetObject(L, 1, typeof(DG.Tweening.Tweener));
                    System.Action _complete = translator.GetDelegate<System.Action>(L, 2);
                    
                    DoTweenTools.TweenSettingsExtensions_OnComplete( _target, _complete );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsTweening_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    object _targetOrId = translator.GetObject(L, 1, typeof(object));
                    
                        var gen_ret = DoTweenTools.IsTweening( _targetOrId );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CompleteTweenRightNow_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<object>(L, 1)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    object _targetOrId = translator.GetObject(L, 1, typeof(object));
                    bool _withCallbacks = LuaAPI.lua_toboolean(L, 2);
                    
                        var gen_ret = DoTweenTools.CompleteTweenRightNow( _targetOrId, _withCallbacks );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 1&& translator.Assignable<object>(L, 1)) 
                {
                    object _targetOrId = translator.GetObject(L, 1, typeof(object));
                    
                        var gen_ret = DoTweenTools.CompleteTweenRightNow( _targetOrId );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to DoTweenTools.CompleteTweenRightNow!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
