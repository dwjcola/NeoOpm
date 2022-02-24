///
///用来读取和存放LUA端的客户端缓存数据
/// 

using System;
using System.Collections.Generic;
using SLG;
using UnityEngine;
using UnityGameFramework.Runtime;
using XLua;

namespace NeoOPM
{
    public class DataPoolComponent : GameFrameworkComponent
    {
        [CSharpCallLua]
        public interface ITableValue
        {
            string Id { get; set; }
            string AssetName { get; set; }
            string UIGroupName { get; set; }
            bool AllowMultiInstance { get; set; }
            bool PauseCoveredUIForm { get; set; }
            string LuaPath { get; set; }
            string LuaName { get; set; }
            bool Mask { get; set; }
            int UITween { get; set; }
        }
        private Dictionary<string, ITableValue> m_Dic;
        public Dictionary<string, ITableValue> Dic
        {
            get
            {
                if (m_Dic == null)
                {
                    LuaEnv luaEnv = XluaManager.instance.LuaEnv;
                    m_Dic = luaEnv.Global.GetInPath<Dictionary<string,ITableValue>>("TPanel");
                }
                return m_Dic;
            }
        }
        public ITableValue GetPanelValueByKey(string key)
        {
            ITableValue tv;
            if (Dic.TryGetValue(key,out tv))
            {
                return tv;
            }

            return null;
        }
    }
}