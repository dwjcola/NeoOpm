using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace GameFramework.DataTable
{
    [Serializable]
    public class PreLoadResRow
    {
         public string Res; // 资源路径
         public string fileType; // 类型
         public string AssemblyName; // 程序集名字
         public string Txt; // 文件后缀
         public int Scene; // 场景号
         public int Self; // 是否仅自己加载

    }
    [CreateAssetMenu(menuName = "DataTable/PreLoadResTable", fileName = "PreLoadResTable")]
    public class PreLoadResTable: DataTable
    {
        [SerializeField] 
        private PreLoadResRow[] rows = default;
        private readonly Dictionary<string, PreLoadResRow> _tables = new Dictionary<string, PreLoadResRow>();
        public Dictionary<string, PreLoadResRow> TableDic{
            get{
                return _tables;
            }
        }
        public PreLoadResRow GetRow(string id)
        {
            if (_tables.TryGetValue(id, out PreLoadResRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(PreLoadResRow);
            }
        }

        public override void OnAfterDeserialize()
        {
            _tables.Clear();
            foreach (var v in rows)
            {
                _tables.Add(v.Res, v);
            }
        }
    }
}