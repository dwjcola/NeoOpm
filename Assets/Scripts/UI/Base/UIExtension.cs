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

namespace ProHA
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
        /*public static bool HasUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            UIFormLine drUIForm = TableTools.Tables.UIForm.GetLineById(uiFormId);
            if (drUIForm == null)
            {
                return false;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(assetName);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(assetName);
        }
        */

        /*public static UGuiForm GetUIForm(this UIComponent uiComponent, int uiFormId, string uiGroupName = null)
        {
            UIFormLine drUIForm = TableTools.Tables.UIForm.GetLineById(uiFormId);
            if (drUIForm == null)
            {
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            UIForm uiForm = null;
            if (string.IsNullOrEmpty(uiGroupName))
            {
                uiForm = uiComponent.GetUIForm(assetName);
                if (uiForm == null)
                {
                    return null;
                }

                return (UGuiForm)uiForm.Logic;
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if (uiGroup == null)
            {
                return null;
            }

            uiForm = (UIForm)uiGroup.GetUIForm(assetName);
            if (uiForm == null)
            {
                return null;
            }

            return (UGuiForm)uiForm.Logic;
        }*/

        public static void CloseUIForm(this UIComponent uiComponent, UGuiForm uiForm)
        {
            /*UIForm[] childer = uiForm.GetChildForm();
            for (int i = childer.Length-1; i >=0; i--)
            {
                uiComponent.CloseUIForm(childer[i]);
            }*/
            uiComponent.GetResRoot(uiForm.UIForm.UIGroup,uiForm.UIKey);
            uiComponent.CloseUIForm(uiForm.UIForm);
            
            //uiComponent.CheckMask(uiForm.UIForm.UIGroup);
        }
        public static void CloseUI(this UIComponent uiComponent, string uiKey)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return;
            }

            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);
            UIForm form = uiComponent.GetUIForm(assetName);
            if (form!=null)
            {
                uiComponent.CloseUIForm(form.Logic as UGuiForm);
                //uiComponent.CheckMask(form.UIGroup);
            }
        }
        public static UIFormLogic GetUILogic(this UIComponent uiComponent, int serialId)
        {
            UIForm form = uiComponent.GetUIForm(serialId);
            if (form!=null)
            {
                return form.Logic;
            }

            return null;
        }
        public static UIFormLogic GetUILogic(this UIComponent uiComponent, string uiKey)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);
            UIForm form = uiComponent.GetUIForm(assetName);
            if (form!=null)
            {
                return form.Logic;
            }

            return null;
        }
        public static UIForm GetLuaUIForm(this UIComponent uiComponent, string uiKey)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);
            UIForm form = uiComponent.GetUIForm(assetName);
            if (form != null)
            {
                return form;
            }

            return null;
        }
        /*public static int? OpenUIForm(this UIComponent uiComponent, int uiFormId, object userData = null)
        {
            UIFormLine drUIForm = TableTools.Tables.UIForm.GetLineById(uiFormId);
            if (drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(drUIForm.AssetName);
            if (!drUIForm.AllowMultiInstance)
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

            return uiComponent.OpenUIForm(assetName, drUIForm.UIGroupName, Constant.AssetPriority.UIFormAsset, drUIForm.PauseCoveredUIForm, userData);
        }*/
        public static int? OpenUI(this UIComponent uiComponent, string uiKey, object userData = null)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null;
            }

            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);
            if (!panel.AllowMultiInstance)
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
            int serialId = uiComponent.OpenUIForm(assetName, panel.UIGroupName, Constant.AssetPriority.UIFormAsset, panel.PauseCoveredUIForm, userData);
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
                ctrl.UIKey = panel.Id;
                if (!string.IsNullOrEmpty(panel.LuaName) && XluaManager.instance.HasLua(panel.LuaName, panel.LuaPath))
                {
                    ctrl.InitLua(panel.LuaName,panel.LuaPath);
                    ctrl.LuaOpen(userData);
                }
                LC.SendEvent("OpenUIFormSuccessEvent", ne.UIForm.SerialId);
                if (panel.Mask)
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
                    PanelLine panel = TableTools.Tables.Panel.GetLineById(ctrl.UIKey);
                    
                    if (panel.Mask && !mask)
                    {
                        help.SetMask(ctrl.Depth - 1);
                        mask = true;
                        if (panel.ShowHead)
                        {
                            LC.SendEvent("Show_Head_on_UI",1);
                        }
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
        public static void GetResRoot(this UIComponent uiComponent,IUIGroup group,string uikey)
        {
            IUIForm[] forms = group.GetAllUIForms();
            for (int i = 0;i<forms.Length;i++)
            {
                UIForm form = (UIForm) forms[i];
                UGuiForm ctrl = (UGuiForm) form.Logic;
                if(ctrl.UIKey!=null && ctrl.UIKey != uikey)
                {
                    PanelLine panel = TableTools.Tables.Panel.GetLineById(ctrl.UIKey);
                    if (panel.ShowRes)
                    {
                        LC.SendEvent("Change_Res_Root",1);
                    }
                    if (panel.ShowRes)
                    {
                        LC.SendEvent("Change_Res_Root",form.transform);
                        return;
                    }
                    
                }
            }

            LC.SendEvent("Change_Res_Root",null);
            LC.SendEvent("Change_Res_Root");
        }
        public static LuaTable ShowPartUIForm(this UIComponent uiComponent, string uiKey, Transform parent,object userData = null)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return null ;
            }
            LuaTable lua = null;
            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);
            GameObject go=LC.ResMgr.InstantiateAsset(assetName);
            if (go)
            {
                go.transform.SetParent(parent,false);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                UIMonoItem item = go.GetComponent<UIMonoItem>();
                if (item)
                {
                    item.InitLua(panel.LuaName, panel.LuaPath);
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
        /// <summary>
        /// 对话形式文字动画
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Coroutine DoDialogueAni(this TextMeshProUGUI target, Action callBack =null)
        {
            IEnumerator RevealCharacters()
            {
                target.ForceMeshUpdate();
                TMP_TextInfo textInfo = target.textInfo;
                int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
                int visibleCount = 0;
                while (true)
                {
                    if (visibleCount > totalVisibleCharacters)
                    {
                        callBack?.Invoke();
                        break;
                    }
                    target.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?
                    visibleCount += 1;
                    yield return null;
                }
            }
            return target.StartCoroutine(RevealCharacters());
        }
        /// <summary>
        /// 立即完成对话文字动画
        /// </summary>
        /// <param name="target"></param>
        /// <param name="mCoroutine"></param>
        public static void FinishDialogueAni(this TextMeshProUGUI target, Coroutine mCoroutine)
        {
            target.StopCoroutine(mCoroutine);
            target.maxVisibleCharacters = target.textInfo.characterCount;
        }
		public static void PauseUIForm(this UIComponent uiComponent,string uiKey)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return;
            }

            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);

            UIForm form = uiComponent.GetUIForm(assetName);
            if (form != null)
            {
                form.OnPause();
                //uiComponent.CheckMask(form.UIGroup);
            }
        }
        public static void ResumeUIForm(this UIComponent uiComponent, string uiKey)
        {
            PanelLine panel = TableTools.Tables.Panel.GetLineById(uiKey);
            if (panel == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiKey);
                return;
            }

            string assetName = AssetUtility.GetUIFormAsset(panel.AssetName);

            UIForm form = uiComponent.GetUIForm(assetName);
            if (form != null)
            {
                form.OnResume();
            }
        }
    }
}
