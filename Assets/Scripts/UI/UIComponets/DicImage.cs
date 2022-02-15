
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace NeoOPM
{
   [RequireComponent(typeof(Image))]
   public class DicImage : MonoBehaviour
   {
      public bool IsNative = true;

      private void Awake ( )
      {
         ApplyValue ( );
      }
      [ContextMenu("Apply")]
      public void ApplyValue()
      {
         Image image = GetComponent<Image>();
         string spriteName = image.sprite.name;
         
         LuaTable lt = XluaManager.instance.GetLua("TAtlas");
         string atlas = lt.Get<string>(image.sprite.name);
         UIMonoPanel p = GetComponentInParent<UIMonoPanel>();
         if (p!=null)
         {
            LC.SetSprite(p,image,atlas,spriteName,IsNative);
         }
         
      }
   }
}