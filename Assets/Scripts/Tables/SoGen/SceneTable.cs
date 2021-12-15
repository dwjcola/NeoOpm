using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace ProHA
{
    [Serializable]
    public class SceneRow
    {
         public int Id; // 场景编号
         public string AssetName; // 资源名称
         public int BackgroundMusicId; // 背景音乐编号

    }
    [CreateAssetMenu(menuName = "DataTable/SceneTable", fileName = "SceneTable")]
    public class SceneTable: DataTable
    {
        [SerializeField] 
        private SceneRow[] rows = default;
        private readonly Dictionary<int, SceneRow> _tables = new Dictionary<int, SceneRow>();
        public Dictionary<int, SceneRow> TableDic{
            get{
                return _tables;
            }
        }
        public SceneRow GetRow(int id)
        {
            if (_tables.TryGetValue(id, out SceneRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(SceneRow);
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