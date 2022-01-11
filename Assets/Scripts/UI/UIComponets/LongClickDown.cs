using System;
using UnityEngine;
using GameFramework.UI;
using XLua;

namespace NeoOPM
{
    public class LongClickDown : MonoBehaviour {

        private bool isDown;
        private Action<LuaTable, object> callback;
        private Action<LuaTable, object> stopCB;
        private LuaTable lt;
        void Start () 
        {
            EventTriggerListener.Get(gameObject).onDown += OnClickDown;
            EventTriggerListener.Get(gameObject).onUp += OnClickUp;
        }
        void OnClickDown(GameObject go)
        {
            isDown = true;
        }

        void OnClickUp(GameObject go)
        {
            isDown = false;
            stopCB?.Invoke(lt,Time.deltaTime);
        }

        private void LateUpdate()
        {
            if (isDown)
            {
                callback?.Invoke(lt,Time.deltaTime);
            }
        }
        public void CallBack(LuaTable luaClass,Action<LuaTable,object> cb,Action<LuaTable,object> cb2)
        {
            lt = luaClass;
            callback = cb;
            stopCB = cb2;
        }


    }
}
