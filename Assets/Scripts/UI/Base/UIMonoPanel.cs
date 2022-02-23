using UnityEngine;
using XLua;

namespace NeoOPM
{
    // custom editor UIViewEditor

    [AddComponentMenu("UIMonoPanel")]
    public class UIMonoPanel : UGuiForm
    {
        [ContextMenu("CopyToMonoItem")]
        public void CopyToMonoPanel()
        {
            UIMonoItem item = GetComponent<UIMonoItem>();
            if (item == null)
            {
                return;
            }

            string key;
            int valueCount = valueList.Count;
            for (int i = 0, len = keyList.Count; i < len; i++)
            {
                key = keyList[i];
                if (string.IsNullOrEmpty(key)) continue;
                if (i >= valueCount || valueList[i] == null)
                {
                    continue;
                }

                item.keyList.Add(key);
                item.valueList.Add(valueList[i]);
            }

            for (int i = 0; i < strkeyList.Count && i < strvalueList.Count; i++)
            {
                key = strkeyList[i];
                if (string.IsNullOrEmpty(key)) continue;
                item.strkeyList.Add(key);
                item.strvalueList.Add(strvalueList[i]);
            }
        }

    }
}