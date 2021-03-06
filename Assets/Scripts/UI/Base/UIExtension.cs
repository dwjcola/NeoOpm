//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.UI;
using System.Collections;
using GameFramework.Event;
using SLG;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XLua;
using TMPro;
using Object = UnityEngine.Object;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
using System;

namespace NeoOPM
{
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            canvasGroup.alpha = alpha;
        }

        public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while (time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue, value, time / duration);
                yield return new WaitForEndOfFrame();
            }

            slider.value = value;
        }
        public static bool HasUIForm(this UIComponent uiComponent, string uiKey)
        {
            string assetName = uiComponent.GetAssetNameByKey(uiKey);
            return uiComponent.HasUIForm(assetName);
        }
        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            uiComponent.CloseUIForm(uiForm.UIForm);
        }
        public static UIForm GetUIFormByKey(this UIComponent uiComponent, string uiKey)
        {
            string assetName = uiComponent.GetAssetNameByKey(uiKey);
            UIForm form = uiComponent.GetUIForm(assetName);
            return form;
        }
        public static string GetAssetNameByKey(this UIComponent uiComponent, string uiKey)
        {
            var panel = LC.GetUITable(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null;
            }
            string tAssetName = panel.AssetName;
            return  AssetUtility.GetUIFormAsset(tAssetName);
        }
        public static void CloseUI(this UIComponent uiComponent, string uiKey)
        {
            UIForm form = uiComponent.GetUIFormByKey(uiKey);
            if (form!=null)
            {
                uiComponent.CloseUIForm(form.Logic as UGuiForm);
            }
        }

        public static UIFormLogic GetUILogic(this UIComponent uiComponent, string uiKey)
        {
            UIForm form = uiComponent.GetUIFormByKey(uiKey);
            if (form!=null)
            {
                return form.Logic;
            }

            return null;
        }

        public static int? OpenUI(this UIComponent uiComponent, string uiKey, object userData = null)
        {
            var panel = LC.GetUITable(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null;
            }

            string tAssetName = panel.AssetName;
            bool tAllowMultiInstance = panel.AllowMultiInstance;
            string tUIGroupName = panel.UIGroupName;
            bool tPauseCoveredUIForm = panel.PauseCoveredUIForm;
            
            string tLuaName = panel.LuaName;
            string tLuaPath = panel.LuaPath;
            bool tMask = panel.Mask;
            int tween = panel.UITween;
            
            string assetName = AssetUtility.GetUIFormAsset(tAssetName);
            if (!tAllowMultiInstance)
            {
                if (uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if (uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }
            int serialId = uiComponent.OpenUIForm(assetName, tUIGroupName, Constant.AssetPriority.UIFormAsset, tPauseCoveredUIForm, userData);
            UIForm form;
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, (object sender, GameEventArgs e) =>
            {
                OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
                if (ne.UIForm.SerialId != serialId)
                {
                    return;
                }
                    
                form = (UIForm) ne.UIForm;
                UGuiForm ctrl = (UGuiForm) form.Logic;
                ctrl.UIKey = uiKey;
                ctrl.UIMask = tMask;
                ctrl.UITween = tween;
                if (!string.IsNullOrEmpty(tLuaName) && XluaManager.instance.HasLua(tLuaName, tLuaPath))
                {
                    ctrl.InitLua(tLuaName,tLuaPath);
                    ctrl.LuaOpen(userData);
                }
                LC.SendEvent("OpenUIFormSuccessEvent", ne.UIForm.SerialId);
                if (tMask)
                {
                    uiComponent.CheckMask(form.UIGroup);
                }
                
                
            });
            

            return serialId;
        }
        public static void CheckMask(this UIComponent uiComponent,IUIGroup group)
        {
            bool mask = false;
            bool res = false;
            UGuiGroupHelper help = group.Helper as UGuiGroupHelper;
            IUIForm[] forms = group.GetAllUIForms();
            for (int i = 0;i<forms.Length;i++)
            {
                UIForm form = (UIForm) forms[i];
                UGuiForm ctrl = (UGuiForm) form.Logic;
                if(ctrl.UIKey!=null)
                {
                    var panel = LC.GetUITable(ctrl.UIKey);
                    bool tMask = panel.Mask;
                    
                    if (tMask && !mask)
                    {
                        help.SetMask(ctrl.Depth - 1);
                        mask = true;
                    }
                    
                }
            }

            if (!mask)
            {
                help.SetMask(-1);
                LC.SendEvent("Show_Head_on_UI");
            }
            //if(!res)LC.SendEvent("Show_Res_on_UI",null);
        }
        public static LuaTable ShowPartUIForm(this UIComponent uiComponent, string uiKey, Transform parent,object userData = null)
        {
            var panel = LC.GetUITable(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null;
            }
            string tAssetName = panel.AssetName;
            string tLuaName = panel.LuaName;
            string tLuaPath = panel.LuaPath;
            LuaTable lua = null;
            string assetName = AssetUtility.GetUIFormAsset(tAssetName);
            GameObject go=LC.ResMgr.InstantiateAsset(assetName);
            if (go)
            {
                go.transform.SetParent(parent,false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                UIMonoItem item = go.GetComponent<UIMonoItem>();
                if (item)
                {
                    item.InitLua(tLuaName, tLuaPath);
                    item.Open(userData);
                    lua = item.Lua;
                }
            }
            return lua;
        }

        public static GameObject CreateItem(this UIComponent uiComponent,Transform parent,GameObject item)
        {
            GameObject go = Object.Instantiate<GameObject>(item, parent, false);
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);
            return go;
        }
        public static LuaTable CreateItem(this UIComponent uiComponent,GameObject item,Transform parent,LuaTable lua)
        {
            GameObject go = Object.Instantiate<GameObject>(item, parent, true);
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);
            UIMonoItem mono = go.GetComponent<UIMonoItem>();
            if (mono)
            {
                mono.OnInit(lua);
            }

            return lua;
        }
    }
}
