using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace NeoOPM
{
    public class CToggle : Toggle
    {
        private bool _hasGetUnSelect = false;
        private bool _hasGetSelect = false;
        private CanvasGroup _unSelectCG;
        public CanvasGroup UnSelectCG
        {
            get
            {
                if (!_hasGetUnSelect&&_unSelectCG==null&&targetGraphic!=null)
                {
                    _unSelectCG = targetGraphic.gameObject.GetComponent<CanvasGroup>();
                    _hasGetUnSelect = true;
                }

                return _unSelectCG;
            }
        }
        private CanvasGroup _selectCG;
        public CanvasGroup SelectCG
        {
            get
            {
                if (!_hasGetSelect&&_selectCG==null&&graphic!=null)
                {
                    _selectCG = graphic.gameObject.GetComponent<CanvasGroup>();
                    _hasGetSelect = true;
                }

                return _selectCG;
            }
        }
        public void SetVisibility(bool activeFlag)
        {
            if (activeFlag)
            {
                CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
                cg.alpha = 0f;
            }
            else
            {
                CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
                cg.alpha = 1f;
            }
          
        }
    }
}