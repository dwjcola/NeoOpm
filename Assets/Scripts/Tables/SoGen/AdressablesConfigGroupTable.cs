using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace ProHA
{
    [Serializable]
    public class AdressablesConfigGroupRow
    {
         public string Path; // 资源路径
         public string GroupName; // 组名称
         public string Address; // addressable简称,不是路径
         public string LabelNames; // 标签 。。。
         public bool isLocal; // 是否本地包
         public bool StaticContent; // 是否是静态包
         public int BundleMode; // bundle模式,分开打还是打进一个bundle

    }
    [CreateAssetMenu(menuName = "DataTable/AdressablesConfigGroupTable", fileName = "AdressablesConfigGroupTable")]
    public class AdressablesConfigGroupTable: DataTable
    {
        [SerializeField] 
        private AdressablesConfigGroupRow[] rows = default;
        private readonly Dictionary<string, AdressablesConfigGroupRow> _tables = new Dictionary<string, AdressablesConfigGroupRow>();
        public Dictionary<string, AdressablesConfigGroupRow> TableDic{
            get{
                return _tables;
            }
        }
        public AdressablesConfigGroupRow GetRow(string id)
        {
            if (_tables.TryGetValue(id, out AdressablesConfigGroupRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(AdressablesConfigGroupRow);
            }
        }

        public override void OnAfterDeserialize()
        {
            _tables.Clear();
            foreach (var v in rows)
            {
                _tables.Add(v.Path, v);
            }
        }
    }
}