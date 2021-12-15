using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class CSCallLua : MonoBehaviour
{
    
    private LuaTable mainLuaTable;
    private bool isInitLuaMain = false;

    Func<LuaTable, float, float, LuaTable> func_update;
    Func<LuaTable, float, float, LuaTable> func_uplateUpdate;
    public void SetMainLua(LuaTable luaTable)
    {
        if (luaTable != null)
        {
            mainLuaTable = luaTable;
          

            mainLuaTable.Get("update", out func_update);
            mainLuaTable.Get("lateUpdate", out func_uplateUpdate);

            isInitLuaMain = true;

        }
      
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isInitLuaMain && func_update != null)
        {
            func_update(mainLuaTable,Time.deltaTime,Time.timeScale);
        }
    }

    void LateUpdate()
    {
        //if (isInitLuaMain && func_uplateUpdate != null)
        //{
        //    func_uplateUpdate(mainLuaTable, Time.deltaTime, Time.timeScale);
        //}
    }
}
