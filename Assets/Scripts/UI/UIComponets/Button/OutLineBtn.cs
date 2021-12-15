
using DG.Tweening;
using GameFramework;
using GameFramework.AddressableResource;
using GameFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProHA
{
    [RequireComponent(typeof(Image))]
    public class OutLineBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Color OutlineColor = Color.white;
        public int OutlineWidth = 0;

        private Graphic graphic;
        /*private Material uiM;
        private Material lightM;
        private Image image;*/
        protected void Awake()
        {
            graphic = GetComponent<Graphic>();
            //image = GetComponent<Image>();
            /*uiM = image.material;
            var shader = Shader.Find("Custom_Shader/Outlight");
            lightM = new Material(shader);*/
        }
        protected void Start()
        {
            var shader = Shader.Find("Custom_Shader/Outlight");
            graphic.material = new Material(shader);

            var v1 = graphic.canvas.additionalShaderChannels;
            var v2 = AdditionalCanvasShaderChannels.TexCoord1;
            if ((v1 & v2) != v2)
            {
                graphic.canvas.additionalShaderChannels |= v2;
            }
            v2 = AdditionalCanvasShaderChannels.TexCoord2;
            if ((v1 & v2) != v2)
            {
                graphic.canvas.additionalShaderChannels |= v2;
            }

            Refresh();
        }
        protected void Refresh()
        {
            graphic.material.SetColor("_LightColor", this.OutlineColor);
            graphic.material.SetInt("_Size", this.OutlineWidth);
            graphic.SetVerticesDirty();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            OutlineWidth = 10;
            Refresh();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OutlineWidth = 0;
            Refresh();
        }
    }
    
}