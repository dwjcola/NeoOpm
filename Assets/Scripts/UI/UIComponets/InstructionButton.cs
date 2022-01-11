
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace NeoOPM
{
   public class InstructionButton : MonoBehaviour,IPointerClickHandler
   {
      public string titleDic = "";
      public string contentDic = "";

      private void Awake ( )
      {
      }

        public void OnPointerClick(PointerEventData eventData)
        {
            string[] param = { titleDic, contentDic };
            LC.OpenUI("InstructionForm", param);
        }
    }

    
}