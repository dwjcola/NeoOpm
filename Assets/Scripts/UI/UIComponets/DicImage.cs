
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

namespace ProHA
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
         Debug.LogError(atlas+"---->"+image.sprite.name);
         LC.SetSprite(image,atlas,spriteName,IsNative);
      }
   }
}