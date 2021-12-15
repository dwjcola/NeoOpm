using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace ProHA
{
    [Serializable]
    public class DicRow
    {
         public string Id; // key
         public string Content; // 内容

    }
    [CreateAssetMenu(menuName = "DataTable/DicTable", fileName = "DicTable")]
    public class DicTable: DataTable
    {
        [SerializeField] 
        private DicRow[] rows = default;
        private readonly Dictionary<string, DicRow> _tables = new Dictionary<string, DicRow>();
        public Dictionary<string, DicRow> TableDic{
            get{
                return _tables;
            }
        }
        public DicRow GetRow(string id)
        {
            if (_tables.TryGetValue(id, out DicRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(DicRow);
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