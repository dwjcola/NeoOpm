using UnityEngine;

namespace ProHA
{
    public abstract class DataTable : ScriptableObject, ISerializationCallbackReceiver
    {
        public void OnBeforeSerialize()
        {
        }

        public abstract void OnAfterDeserialize();
    }
}