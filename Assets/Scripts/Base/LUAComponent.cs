///设计目的：之后若不能更新c#,而又想添加组件时，可以挂载此脚本，添加目标lua文件名字，即可在Open之后执行lua函数
///生命周期在c# 控制
///开发期新增组件可以选择继承此脚本或者自己写
///添加字段、数组、属性等如monoitem

using XLua;
using System;

namespace ProHA
{

    public class LUAComponent : UIMonoItem
    {
        public string LuaName ;
        Action<LuaTable, object> actionOpen;
        public void Awake()
        {
            var dostr = $@"
local itemLua=require ('{LuaName}') ;
local result=itemLua:new();
result.f_name='{LuaName}';
return result;
";
            LuaTable ll = XluaManager.instance.LuaEnv.DoString(dostr)[0] as LuaTable;
            if (ll == null)
            {
                GameFramework.GameFrameworkLog.Warning($@"{this.GetType().Name} init lua failed,GameObject={this.gameObject.name}");
                ll = XluaManager.instance.LuaEnv.NewTable();
            }

            OnInit(ll, LuaName);
            actionOpen = ll.Get<Action<LuaTable, object>>("Open");
            actionOpen?.Invoke(ll,null);
        }
    }
    
}