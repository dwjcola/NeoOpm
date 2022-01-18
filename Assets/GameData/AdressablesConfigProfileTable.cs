using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
namespace NeoOPM
{
    [Serializable]
    public class AdressablesConfigProfileRow
    {
         public string ProfileName; // UI编号
         public string RemoteLoadPath; // 远端加载路径

    }
    [CreateAssetMenu(menuName = "DataTable/AdressablesConfigProfileTable", fileName = "AdressablesConfigProfileTable")]
    public class AdressablesConfigProfileTable: ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] 
        private AdressablesConfigProfileRow[] rows = default;
        private readonly Dictionary<string, AdressablesConfigProfileRow> _tables = new Dictionary<string, AdressablesConfigProfileRow>();
        public Dictionary<string, AdressablesConfigProfileRow> TableDic{
            get{
                return _tables;
            }
        }
        public AdressablesConfigProfileRow GetRow(string id)
        {
            if (_tables.TryGetValue(id, out AdressablesConfigProfileRow row))
            {
                return row;
            }
            else
            {
                Debug.LogError($"id {id} dont exist!");
                return default(AdressablesConfigProfileRow);
            }
        }
        public void OnBeforeSerialize()
        {
        }
        public void OnAfterDeserialize()
        {
            _tables.Clear();
            foreach (var v in rows)
            {
                _tables.Add(v.ProfileName, v);
            }
        }
    }
}