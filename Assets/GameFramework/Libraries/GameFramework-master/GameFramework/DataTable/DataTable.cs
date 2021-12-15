using UnityEngine;

namespace GameFramework.DataTable
{
    public abstract class DataTable : ScriptableObject, ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
        }

        public abstract void OnAfterDeserialize();
    }
}