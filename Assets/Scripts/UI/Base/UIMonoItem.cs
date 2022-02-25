using UnityEngine;
using System;
using XLua;
/**********************************************
* @版本号：V1.00
**********************************************/
namespace NeoOPM
{
    [AddComponentMenu("UIMonoItem")]
    public class UIMonoItem : UGuiForm
    {
        private LuaTable lua;
        private int m_interval = 30;
        private int m_timeCount = 1;
        public bool ReleaseRes { get; set; }

        public LuaTable Lua
        {
            get { return lua; }
        }
        private Action<LuaTable> luaOnEnable;
        private Action<LuaTable> luaOnDisable;
        private Action<LuaTable> luaOnDestroy;
        private Action<LuaTable> luaUpdate;
        private Action<LuaTable, object> luaOpen;

        public override void OnInit(LuaTable luatable, string errorSignId = "")
        {
            isUIMonoItem = true;
            if (this.lua != null)
            {
                DisposeLua();
            }
            base.OnInit(luatable,errorSignId);
            this.lua = luatable;
            luaOnEnable = lua.Get<Action<LuaTable>>("OnEnable");
            luaOnDisable = lua.Get<Action<LuaTable>>("OnDisable");
            luaOnDestroy = lua.Get<Action<LuaTable>>("OnDestroy");
            luaUpdate = lua.Get<Action<LuaTable>>("Update");
            luaOpen = lua.Get<Action<LuaTable,object>>("Open");
            Action<LuaTable> luaInit = lua.Get<Action<LuaTable>>("Init");
            luaInit?.Invoke(luatable);
        }

        public LuaTable SelfInit()
        {
            OnInit(XluaManager.instance.LuaEnv.NewTable());
            return lua;
        }
        public void Open(object param)
        {
            if (lua != null)
            {
                luaOpen?.Invoke(lua,param);
            }
        }

        private void OnEnable()
        {
            if (lua != null)
            {
                if (luaOnEnable != null)
                {
                    luaOnEnable(lua);
                }
            }
        }

        private void OnDisable()
        {
            if (lua != null)
            {
                if (luaOnDisable != null)
                {
                    luaOnDisable(lua);
                }
            }
        }

        private void OnDestroy()
        {
            DisposeLua();
            if (ReleaseRes)
            {
                //ResourceManager.Destroy(gameObject, true);
            }

        }
        public void DisposeLua()
        {
            if (lua != null)
            {
                if (XluaManager.instance.LuaEnv != null)
                {
                    if (luaOnDestroy != null)
                    {
                        luaOnDestroy(lua);
                    }

                    lua.Dispose();
                    lua = null;
                }
            }
        }

        private void Update()
        {
            if (lua != null)
            {
                if (luaUpdate != null)
                {
                    m_timeCount++;
                    if (m_timeCount % m_interval == 0)
                    {
                        luaUpdate(lua);
                        m_timeCount = 1;
                    }

                }
            }
        }
        //[ContextMenu("CopyToMonoPanel")]
        //public void CopyToMonoPanel()
        //{
        //    UIMonoPanel item = GetComponent<UIMonoPanel>();
        //    if (item == null)
        //    {

        //        item = gameObject.AddComponent<UIMonoPanel>();
        //    }

        //    string key;
        //    int valueCount = valueList.Count;
        //    for (int i = 0, len = keyList.Count; i < len; i++)
        //    {
        //        key = keyList[i];
        //        if (string.IsNullOrEmpty(key)) continue;
        //        if (i >= valueCount || valueList[i] == null)
        //        {
        //            continue;
        //        }

        //        item.keyList.Add(key);
        //        item.valueList.Add(valueList[i]);
        //    }

        //    for (int i = 0; i < strkeyList.Count && i < strvalueList.Count; i++)
        //    {
        //        key = strkeyList[i];
        //        if (string.IsNullOrEmpty(key)) continue;
        //        item.strkeyList.Add(key);
        //        item.strvalueList.Add(strvalueList[i]);
        //    }
        //}
    }
}
