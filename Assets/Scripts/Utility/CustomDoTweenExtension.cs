using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DG.Tweening.Core;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using DG.Tweening.Core.Enums;



namespace NeoOPM
{
    public static class CustomDoTweenExtension
    {
        // 原DOShakePosition貌似只能在初始位置的基础上shake，导致与其他tween效果无法叠加比如相机ZoomIn
        // 这个函数是在最新位置上做相对震动，因此可以和ZoomIn搭配使用
        public static Tweener DORelativeShakePosition(
            this Transform target,
            float duration,
            Vector3 strength,
            int vibrato = 10,
            float randomness = 90f,
            bool snapping = false,
            bool fadeOut = true)
        {
            if ((double) duration > 0.0)
            {
                Vector3 startPos = Vector3.zero;
                return DOTween.Shake((DOGetter<Vector3>) (() => startPos = target.localPosition), 
                    (DOSetter<Vector3>) (x =>
                        target.localPosition += (x - startPos)
                    ), 
                    duration, strength, vibrato, randomness, fadeOut).SetTarget<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>((object) target).SetSpecialStartupMode<TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>>(SpecialStartupMode.SetShake).SetOptions(snapping);
            }
            if (Debugger.logPriority > 0)
                Debug.LogWarning((object) "DOShakePosition: duration can't be 0, returning NULL without creating a tween");
            return (Tweener) null;
        }
        
        
    }
    
}

