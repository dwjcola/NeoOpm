//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XLua;
using Object = UnityEngine.Object;

namespace NeoOPM
{
    public abstract class UGuiForm : UIFormLogic
    {
        public const int DepthFactor = 100;
        private const float FadeTime = 0.1f;
        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;
        private List<Canvas> m_CachedCanvasContainer = new List<Canvas>();
        private Action<LuaTable> luaOnEnable;
        private Action<LuaTable> luaUpdate;
        private Action<LuaTable> luaOnDisable;
        private Action<LuaTable, object> luaOpen;
        private Action<LuaTable> luaOnDestroy;
        
        private Action<LuaTable> luaShowTween;
        private Action<LuaTable,Action> luaCloseTween;

        private Action<LuaTable> DestroyAllMonoItemLua;
        private TweenerCore<Vector3, Vector3, VectorOptions> currentTween;

        private Sequence currentQuenece;
        //private Action<LuaTable> luaTouchBlank;
        private LuaTable m_LuaClass;

        [HideInInspector]
        public bool isUIMonoItem = false;
        public string UIKey
        {
            get;
            set;
        }
        public int UITween
        {
            get;
            set;
        }
        public bool UIMask
        {
            get;
            set;
        }
        public LuaTable LuaClass => m_LuaClass;

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;

            GameObject go = new GameObject();
            go.AddComponent<Text>().font = mainFont;
            Destroy(go);
        }
        [HideInInspector]
        public List<string> keyList = new List<string>();
        [HideInInspector]
        public List<Object> valueList = new List<Object>();

        [HideInInspector]
        public List<string> strkeyList = new List<string>();

        [HideInInspector]
        public List<string> strvalueList = new List<string>();
        /// <summary>
        /// lua初始化
        /// </summary>
        /// <param name="luatable"></param>
        /// <param name="errorSignId"></param>
        public virtual void OnInit(LuaTable luatable, string errorSignId = "")
        {
            string key;
            int valueCount = valueList.Count;
            for ( int i = 0, len = keyList.Count; i < len; i++ )
            {
                key = keyList [ i ];
                if ( string.IsNullOrEmpty ( key ) ) continue;
                if ( i >= valueCount || valueList [ i ] == null )
                {
                    Log.Error( "【{0}】的UIMono中索引中的引用value丢失.或是key索引大于引用索引 ===>{1}" ,errorSignId, gameObject.name );
                    continue;
                }
                luatable.Set ( key, valueList [ i ] );
            }
            for ( int i = 0; i < strkeyList.Count && i < strvalueList.Count; i++ )
            {
                key = strkeyList [ i ];
                if ( string.IsNullOrEmpty ( key ) ) continue;
                luatable.Set ( key, strvalueList [ i ] );
            }
            luatable.Set ( "trans", this.transform );
            luatable.Set ( "view", this );
        }
        public void InitLua(string luaClassName,string luapath)
        {
            if (m_LuaClass == null)
            {
                var luaEnv = XluaManager.instance.LuaEnv;
                luaEnv.DoString($"require ('{UIKey}') ;");
                m_LuaClass = luaEnv.Global.Get<LuaTable>(luaClassName);
            }
            Debug.LogError(m_LuaClass);
            OnInit(m_LuaClass, luaClassName);
            luaOnEnable = m_LuaClass.GetInPath<Action<LuaTable>>("OnEnable");
            luaOpen = m_LuaClass.GetInPath<Action<LuaTable,object>>("Open");
            luaUpdate = m_LuaClass.GetInPath<Action<LuaTable>>("Update");
            luaOnDisable = m_LuaClass.GetInPath<Action<LuaTable>>("OnDisable");
            luaOnDestroy = m_LuaClass.GetInPath<Action<LuaTable>>("OnDestroy");
            luaShowTween = m_LuaClass.GetInPath<Action<LuaTable>>("ShowTween");
            luaCloseTween = m_LuaClass.GetInPath<Action<LuaTable,Action>>("CloseTween");
            DestroyAllMonoItemLua = m_LuaClass.GetInPath<Action<LuaTable>>("DestroyAllMonoItemLua");


            if (UIKey!=null)
            {
                if (UITween>=0)
                {
                    TweenToShow(UITween);
                }
            }
            luaShowTween?.Invoke(LuaClass);
        }

        public void LuaOpen(object param)
        {
            luaOpen?.Invoke(LuaClass, param);
        }

        private void OnEnable ( )
        {
            luaOnEnable?.Invoke(LuaClass);
        }
        
        private void OnDisable ( )
        {
            luaOnDisable?.Invoke(LuaClass);
        }
        private int Interval = 30;
        private int TimeCount = 1;
        private void LateUpdate ( )
        {
            if ( m_LuaClass != null )
            {
                if ( luaUpdate != null )
                {
                    TimeCount++;
                    if ( TimeCount%Interval==0 )
                    {
                        luaUpdate ( m_LuaClass );
                        TimeCount = 1;
                    }
               
                }
            }
        }
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
            if (LuaClass!=null)
            {
                DestroyAllMonoItemLua?.Invoke(LuaClass);
                if (luaOnDestroy != null)
                {
                    luaOnDestroy(LuaClass);
                    ClearLuaFunc();
                    LuaClass.Dispose();
                    m_LuaClass = null;
                }
            }
            DisposeUiMonoItems();
        }

        private void ClearLuaFunc()
        {
            luaOnEnable = null;
            luaOpen = null;
            luaUpdate = null;
            luaOnDisable = null;
            luaOnDestroy = null;
            luaShowTween = null;
            luaCloseTween = null;
            DestroyAllMonoItemLua = null;
        }
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            /*m_CanvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));*/
        }

        private void TweenToShow(int type)
        {
            if (type == 0)
            {
                transform.localScale = Vector3.one * 1.1f;
                currentTween?.Kill();
                currentTween = transform.DOScale(Vector3.one, FadeTime);
                currentTween.onComplete = () =>
                {
                    
                };
            }else if (type == 1)
            {
                transform.localScale = Vector3.one * 0.1f;
                currentQuenece?.Kill();
                currentQuenece = DOTween.Sequence();
                currentQuenece.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f));
                currentQuenece.AppendInterval(0.1f);
                currentQuenece.Append(transform.DOScale(Vector3.one, 0.1f));
                currentQuenece.SetAutoKill(true);
                currentQuenece.Play();
                currentQuenece.onComplete = () =>
                {
                    
                };
            }

        }
        protected override void OnClose(bool isShutdown, object userData)
        {
           
            if (UIKey!=null)
            {
                if (UITween == 0)
                {
                    transform.localScale = Vector3.one;
                    currentTween?.Pause();
                    currentTween = transform.DOScale(Vector3.one * 1.1f, FadeTime);
                    currentTween.onComplete = () =>
                    {
                        base.OnClose(isShutdown, userData);
                        if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                      
                    };
                }else if (UITween == 1)
                {
                    transform.localScale = Vector3.one;
                    currentQuenece?.Kill();
                    currentQuenece = DOTween.Sequence();
                    currentQuenece.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f));
                    currentQuenece.Append(transform.DOScale(Vector3.one * 0.1f, 0.1f));
                    currentQuenece.SetAutoKill(true);
                    currentQuenece.OnComplete(() =>
                    {
                        base.OnClose(isShutdown, userData);
                        if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                    });
                    currentQuenece.Play();
                }
                else if (UITween == -1 && luaCloseTween != null)
                {
                    luaCloseTween.Invoke(LuaClass, () =>
                     {
                         base.OnClose(isShutdown, userData);
                         if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                     });
                }
                else
                {
                    base.OnClose(isShutdown, userData);
                    if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                }
            }
            else
            {
                base.OnClose(isShutdown, userData);
                GameEntry.UI.CheckMask(UIForm.UIGroup);
            }
           
        }

   
        protected override void OnPause()
        {
            if (UIKey!=null)
            {
                if (UITween == 0)
                {
                    transform.localScale = Vector3.one;
                    currentTween?.Pause();
                    currentTween = transform.DOScale(Vector3.one * 1.1f, FadeTime);
                    currentTween.onComplete = () => { base.OnPause(); if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);};
                }else if (UITween == 1)
                {
                    transform.localScale = Vector3.one;
                    currentQuenece?.Kill();
                    currentQuenece = DOTween.Sequence();
                    currentQuenece.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f));
                    currentQuenece.Append(transform.DOScale(Vector3.one * 0.1f, 0.1f));
                    currentQuenece.SetAutoKill(true);
                    currentQuenece.OnComplete(() =>
                    {
                        base.OnPause();
                        if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                    });
                    currentQuenece.Play();
                }
                else if (UITween == -1 && luaCloseTween != null)
                {
                    luaCloseTween.Invoke(LuaClass, () =>
                    {

                        base.OnPause();
                        if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                    });
                }
                else
                {
                    base.OnPause();
                    if (UIMask)GameEntry.UI.CheckMask(UIForm.UIGroup);
                }
            }
            else
            {
                base.OnPause();
                GameEntry.UI.CheckMask(UIForm.UIGroup);
            }
            //luaCloseTween?.Invoke(LuaClass,null);
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (UIKey!=null)
            {
                if (UITween >= 0)
                {
                    TweenToShow(UITween);
                }
            }
            if (LuaClass!=null)
                luaShowTween?.Invoke(LuaClass);

        }

        protected override void OnCover()
        {
            base.OnCover();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            GetComponentsInChildren(true, m_CachedCanvasContainer);
            for (int i = 0; i < m_CachedCanvasContainer.Count; i++)
            {
                m_CachedCanvasContainer[i].sortingOrder += deltaDepth;
            }

            m_CachedCanvasContainer.Clear();
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this);
        }
        private void DisposeUiMonoItems()
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                var va = valueList[i];
                if(va is UIMonoItem)
                {
                    var item = va as UIMonoItem;
                    item.DisposeUiMonoItems();
                    item.DisposeLua();
                }
            }
        }
    }
}
