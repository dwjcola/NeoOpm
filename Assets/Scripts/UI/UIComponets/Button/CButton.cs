
using DG.Tweening;
using GameFramework;
using GameFramework.AddressableResource;
using GameFramework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NeoOPM
{
    //[RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class CButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum ButtonType
        {
            squareButton = 0,
            hexagonButton = 1,
            placeholderButton1 = 2,
            placeholderButton2 = 3,
            placeholderButton3 = 4
        }

        public int ClickSoundId = 8;
        //private float[] scales = { 0.84f,0.9f,0.8f,0.8f,0.8f};
        private float[] scaleDurations = { 0.1f,0.1f,0.1f,0.1f,0.1f};
        public ButtonType m_ButtonType;
        public Material m_DisabledMat;
        public Sprite m_DisabledSp;
        private Button m_Button;
        private Image[] m_Images;
        private TextMeshProUGUI[] m_Texts;
        private bool m_Interactable;
        private Sprite[] m_DefaultSps;
        private Material[] m_DefaultMats;
        private Color[] m_DefaultColor;
        private bool[] m_DefaultGradient;
        private EventTriggerListener m_etl;

        public Material SpriteGreyMat = null;

        private Vector3 originalScale = Vector3.one;

        public bool Interactable
        {
            get => m_Interactable;
            set
            {
                m_Interactable = value;
                if (m_Button)
                {
                    m_Button.interactable = value;
                }
                if (m_Images != null)
                {
                    for (int i = 0; i < m_Images.Length; i++)
                    {
                        if (null != SpriteGreyMat)
                        {
                            m_Images[i].material = value ? null : SpriteGreyMat;
                        }
                        else
                        {
                            m_Images[i].sprite = value ? m_DefaultSps[i] : m_DisabledSp;
                        }
                    }
                }

                if (m_Texts!=null)
                {
                    for (int i = 0; i < m_Texts.Length; i++)
                    {
                        m_Texts[i].fontMaterial = value ? m_DefaultMats[i] : m_DisabledMat;
                        m_Texts[i].enableVertexGradient = value ?m_DefaultGradient[i]:false;
                        m_Texts[i].color = value?m_DefaultColor[i]:Color.white;
                    }
                }
                m_etl = GetComponent<EventTriggerListener>();
                if (m_etl!=null)
                {
                    m_etl.enabled = value;
                }
                
                //m_Images.material = value?null:m_DisabledMat;
            }
        }

        /// <summary>
        /// 区别在于
        /// 可以调用响应但是显示按钮灰色
        /// </summary>
        private bool m_InteractableOnlyShow;
        public bool InteractableOnlyShow
        {
            get => m_InteractableOnlyShow;
            set
            {
                m_InteractableOnlyShow = value;
                if (m_Images != null)
                {
                    for (int i = 0; i < m_Images.Length; i++)
                    {
                        if (null != SpriteGreyMat)
                        {
                            m_Images[i].material = value ? null : SpriteGreyMat;
                        }
                        else
                        {
                            m_Images[i].sprite = value ? m_DefaultSps[i] : m_DisabledSp;
                        }
                    }
                }

                if (m_Texts != null)
                {
                    for (int i = 0; i < m_Texts.Length; i++)
                    {
                        m_Texts[i].fontMaterial = value ? m_DefaultMats[i] : m_DisabledMat;
                        m_Texts[i].enableVertexGradient = value ? m_DefaultGradient[i] : false;
                        m_Texts[i].color = value ? m_DefaultColor[i] : Color.white;
                    }
                }
            }
        }

        private void Awake()
        {
            Init();
            m_Button = GetComponent<Button>();
            m_Images = GetComponentsInChildren<Image>();
            m_Texts = GetComponentsInChildren<TextMeshProUGUI>();
            originalScale = transform.localScale;
            if (m_Texts!=null)
            {
                m_DefaultMats = new Material[m_Texts.Length];
                m_DefaultGradient = new bool[m_Texts.Length];
                m_DefaultColor = new Color[m_Texts.Length];
                for (int i = 0; i < m_Texts.Length; i++)
                {
                    m_DefaultMats[i] = m_Texts[i].fontMaterial;
                    m_DefaultGradient[i] = m_Texts[i].enableVertexGradient;
                    m_DefaultColor[i] = m_Texts[i].color;
                }
            }
            if (m_Images!=null)
            {
                m_DefaultSps = new Sprite[m_Images.Length];
                for (int i = 0; i < m_Images.Length; i++)
                {
                    m_DefaultSps[i] = m_Images[i].sprite;
                }
            }
            
            /*if (m_DisabledMat!=null)
            {
                var buttonColors = m_Button.colors;
                buttonColors.disabledColor = Color.white;
            }*/
        }

        private async void Init()
        {
            IAddressableResourceManager ResMgr = GameFrameworkEntry.GetModule<IAddressableResourceManager>();
            m_DisabledSp = await ResMgr.GetAtlasSprite(AssetUtility.GetUIAtalsAsset("sys"), "btn_disable");
            m_DisabledMat = await ResMgr.GetMaterial("Assets/Resource_MS/Materials/UI/MSYH SDF-Outline grey 1.mat");
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(originalScale * GetScale(), GetDuration());
            /*//播放点击音乐
            if (ClickSoundId>0)
            {
                GameEntry.WWise.PlaySound(ClickSoundId);
            }*/
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(originalScale, GetDuration());
        }

        private float GetScale()
        {
            return 0.8f;
        }
        private float GetDuration()
        {
            return scaleDurations[(int) m_ButtonType];
        }
    }
    
}