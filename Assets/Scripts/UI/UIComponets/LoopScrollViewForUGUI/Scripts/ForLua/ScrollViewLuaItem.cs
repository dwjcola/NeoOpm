using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// explain：循环列表组件 UGUI版
/// author：王云飞
/// email:1172906928@qq.com
/// 
/// lua专用
/// </summary>
/// 
namespace lufeigame
{
    public class ScrollViewLuaItem : ScrollViewBaseItem
    {   
        public delegate void CallbackVoid(ScrollViewLuaItem item,int index);
        public delegate void CallbackVoidOneParam(ScrollViewLuaItem item);

        public CallbackVoid updataCallback;
        public CallbackVoidOneParam initCallback;
        public override void Awake()
        {
            base.Awake();
            //InitComponent();
        }
        public override void UpdateData(int index)
        {
            base.UpdateData(index);
            //Debug.LogError(index);
            if (updataCallback != null)
            {
                updataCallback(this, index);
            }
        }
		public void InitComponent()
        {
            if (initCallback != null)
            {
                initCallback(this);
            }
        }
        /// <summary>
        /// 注册item 初始化lua方法
        /// </summary>
        /// <param name="luaFunction"></param>
        public void RegisterInitFunction(CallbackVoidOneParam luaFunction)
        {
           // LuaVM.Instance.AddRefUnloader(this);
            initCallback = luaFunction;
        }
        /// <summary>
        /// 注册item 更新数据lua方法
        /// </summary>
        /// <param name="luaFunction"></param>
        public void RegisterUpdataFunction(CallbackVoid luaFunction)
        {
            //LuaVM.Instance.AddRefUnloader(this);
            updataCallback = luaFunction;
        }

        private void OnDestroy()
        {
            UnloadFunction();
        }
        public void UnloadFunction()
        {
            updataCallback = null;
            initCallback = null;
        }

        public void Dispose()
        {
            UnloadFunction();
        }
    }
}
