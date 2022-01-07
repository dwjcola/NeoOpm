using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

namespace NeoOPM
{
    public enum PreloadState
    {
        None,
        Init,
        CheckDone,
        NoUpdate,
        HotUpdate,
        HotUpdateDone,
        PreLoading,
        Num
    }

    public class CheckUpdateForm : UGuiForm
    {
        public const string UIAssetName = "CheckUpdate/CheckUpdateForm";
        public Slider slider;
        public Image bgImage;
        public TextMeshProUGUI OtherDesc;
        public TextMeshProUGUI percentage;
        public GameObject dialogUI;
        public TextMeshProUGUI title;
        public TextMeshProUGUI message;
        public Button confirmBtn;
        public UnityAction onClickConfirmBtn;

        public PreloadState preloadState;
        public bool LoadingDone = false;
        public static CheckUpdateForm Instance;

        protected override void OnInit(object userData)
        {
            Instance = this;
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            slider.value = 0;
            OtherDesc.text = "";
            percentage.text = "";
            dialogUI.SetActive(false);
            title.text = "";
            message.text = "";
            confirmBtn.onClick.RemoveAllListeners();
            confirmBtn.onClick.AddListener(() =>
            {
                if (onClickConfirmBtn != null)
                {
                    onClickConfirmBtn?.Invoke();
                }

                dialogUI.SetActive(false);
            });
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (preloadState == PreloadState.Init)
            {
                //SimulateInitProgress();
            }
            else if (preloadState == PreloadState.HotUpdate)
            {
                //SimulateUpdateProgress();
            }
            else if (preloadState == PreloadState.PreLoading)
            {
                SimulateLoadingProgress();
            }
        }

        protected override void OnResume()
        {

            base.OnResume();
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
        //检查更新，结果表现在UI上

        public void ShowUpdateMessage(string _Title, string _Message, UnityAction action = null)
        {
            dialogUI.SetActive(true);
            title.text = _Title;
            message.text = _Message;
            onClickConfirmBtn = action;
        }

        public void UpdateSliderValue(float PercentComplete)
        {
            slider.value = PercentComplete;
            percentage.text = $"{string.Format("{0:f2}", PercentComplete * 100)}%";
        }

        public void ShowErrorMessage(string _Title, string _Message, UnityAction action = null)
        {
            ShowUpdateMessage(_Title, _Message, action);
        }

        public bool IsProgressDone()
        {
            return slider.value > 0.95;
        }

        private int CurrentTime = 0;

        int simulateLoadT = 400;

        private const int LoadingTime = 600;
        private const int InitTime = 900;
        private int playHitIndex = 0;

        private void SimulateInitProgress()
        {
            float value = 0.0f;
            int cacheCurrentTime = CurrentTime;
            cacheCurrentTime += GetInitStep();
            if (cacheCurrentTime <= InitTime)
            {
                value = (float) cacheCurrentTime / LoadingTime;
                if (value > 0.9f && !LoadingDone)
                {
                    return;
                }

                if (value > 0.99f)
                {
                    value = 0.99f;
                }

                UpdateSliderValue(value);
                CurrentTime = cacheCurrentTime;
            }
        }

        private void SimulateUpdateProgress()
        {
            float value = 0.0f;
            int cacheCurrentTime = CurrentTime;
            cacheCurrentTime += GetStep();
            if (cacheCurrentTime <= InitTime)
            {
                value = (float) cacheCurrentTime / LoadingTime;
                if (value > 0.9f && !LoadingDone)
                {
                    return;
                }

                if (value > 0.99f)
                {
                    value = 0.99f;
                }

                UpdateSliderValue(value);
                CurrentTime = cacheCurrentTime;
            }
        }

        private string FormatSize(ulong size)
        {
            float sizeM = (float) size / 1000000;
            return string.Format("{0:F}", sizeM);
        }

        private float GetUpdateStep()
        {
            if (!LoadingDone)
            {
                return 0.0015f;
            }
            else
            {
                return 0.01f;
            }
        }

        private void SimulateLoadingProgress()
        {
            float value = 0.0f;
            int cacheCurrentTime = CurrentTime;
            cacheCurrentTime += GetStep();
            if (cacheCurrentTime <= LoadingTime)
            {
                value = (float) cacheCurrentTime / LoadingTime;
                if (value > 0.9f && !LoadingDone)
                {
                    return;
                }

                if (value > 0.99f)
                {
                    value = 0.99f;
                }

                UpdateSliderValue(value);
                CurrentTime = cacheCurrentTime;
            }
        }

        private int GetStep()
        {
            if (LoadingDone)
            {
                return 10;
            }
            else
            {
                return 1;
            }
        }

        private int GetInitStep()
        {
            if (LoadingDone)
            {
                return 10;
            }
            else
            {
                return 1;
            }
        }

        public void ChangeLoadState()
        {
            CurrentTime = 0;
            LoadingDone = false;
        }
    }

    [Serializable]
    public class ProviderLoadRequestOptions
    {
        [SerializeField] bool m_IgnoreFailures = false;

        /// <summary>
        /// IgnoreFailures for provider load requests
        /// </summary>
        public bool IgnoreFailures
        {
            get { return m_IgnoreFailures; }
            set { m_IgnoreFailures = value; }
        }

        public ProviderLoadRequestOptions Copy()
        {
            return (ProviderLoadRequestOptions) this.MemberwiseClone();
        }

    }
}