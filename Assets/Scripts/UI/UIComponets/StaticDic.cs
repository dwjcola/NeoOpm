
using TMPro;
using UnityEngine;

namespace ProHA
{
   public class StaticDic : MonoBehaviour
   {
      public string value = "";

      private void Awake ( )
      {
         ApplyValue ( );
      }
      [ContextMenu("Apply")]
      public void ApplyValue()
      {
         TMP_Text lab = GetComponent<TMP_Text>();
         if (value.Length > 0 && lab != null)
         {
            lab.text = TableDic.GetDicValue(value);
         }
      }
   }
}