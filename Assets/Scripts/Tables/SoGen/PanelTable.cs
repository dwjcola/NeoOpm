using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace ProHA
{
    [Serializable]
    public class PanelRow
    {
         public string Id; // UI编号
         public string AssetName; // 资源名称
         public string UIGroupName; // 界面组名称
         public bool AllowMultiInstance; // 是否允许多个界面实例
         public bool PauseCoveredUIForm; // 是否暂停被其覆盖的界面
         public string LuaPath; // LUA路径
         public string LuaName; // lua名字
         public bool Mask; // 是否遮挡

    }
    [CreateAssetMenu(menuName = "DataTable/PanelTable", fileName = "PanelTable")]
    public class PanelTable: DataTable
    {
        [SerializeField] 
        private PanelRow[] rows = default;
        private readonly Dictionary<string, PanelRow> _tables = new Dictionary<string, PanelRow>();
        public Dictionary<string, PanelRow> TableDic{
            get{
                return _tables;
            }
        }
        public PanelRow GetRow(string id)
        {
            if (_tables.TryGetValue(id, out PanelRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(PanelRow);
            }
        }

        public override void OnAfterDeserialize()
        {
            _tables.Clear();
            foreach (var v in rows)
            {
                _tables.Add(v.Id, v);
            }
        }
    }
}