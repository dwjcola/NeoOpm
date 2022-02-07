using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoTweenTools
{

    public static float getTweenTimeScale()
    {
        float timeScale = DOTween.timeScale;
        return timeScale;
    }

    public static void setTweenTimeScale(float timeScale)
    {
        DOTween.timeScale = timeScale;
    }

    public static void pauseForTransform(Transform transform)
    {
        ShortcutExtensions.DOPause(transform);
    }

    public static void playForTransform(Transform transform)
    {
        ShortcutExtensions.DOPlay(transform);
    }

    public static void DOSizeDelta(RectTransform target, Vector2 to, float duration, float delay, Action action = null)
    {
        Tweener _Tweener = target.DOSizeDelta(to, duration, false);
        _Tweener.SetDelay(delay);
        _Tweener.OnComplete(() =>
        {
            if (action != null)
            {
                action();
            }
        });
    }

    public static void DoTweenMoveTo(Transform target, float duration, Vector3 to)
    {
        target.DOMove(to, duration, false);
    }

    public static void DoTweenMoveToCall(Transform target, float duration, Vector3 to, Action action)
    {
        target.DOMove(to, duration, false).OnComplete(() =>
        {
            if (action != null)
            {
                action();
            }
        });
    }

    public static void DOTweenLocalMoveTo(Transform target, float duration, Vector3 to, Action action)
    {
        target.DOLocalMove(to, duration, false).OnComplete(() =>
        {
            if (action != null)
            {
                action();
            }
        });

    }

    public static void DoTweenRotateQuaternionTo(Transform target, float duration, Quaternion to)
    {
        target.DORotateQuaternion(to, duration);
    }

    public static void DoTweenMoveAndRoattion(Transform target, float duration, Vector3 pos, Quaternion rotation)
    {
        target.DOMove(pos, duration, false);
        target.DORotateQuaternion(rotation, duration);
    }

    public static void DoTweenColor(Material target, float endValue, float duration)
    {
        DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
    }

    public static Tweener DoTweenImageColor(UnityEngine.UI.Image target, float endValue, float duration)
    {
        return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
    }

    public static Tweener DoTweenTextColor(UnityEngine.UI.Text target, float endValue, float duration)
    {
        return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration);
    }

    public static Tweener DoTweenTextColorAndCall(UnityEngine.UI.Text target, float endValue, float duration, Action callback)
    {
        return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
            }
        });
    }

    public static Tweener DoTweenImageColorAndCall(UnityEngine.UI.Image target, float endValue, float duration, Action callback)
    {
        return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration).OnComplete(() =>
        {
            if (callback != null)
            {
                callback();
            }
        });
    }

    //public static Tweener DOAmplitudeGain(Cinemachine.CinemachineVirtualCamera target, float endValue, float duration)
    //{
    //    Tweener ter = DOTween.To(
    //        () => target.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain,
    //        (x) => target.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = x,
    //        endValue,
    //        duration
    //        );
    //    return ter;
    //}

    //public static Tweener DOFrequencyGain(Cinemachine.CinemachineVirtualCamera target, float endValue, float duration)
    //{
    //    Tweener ter = DOTween.To(
    //        () => target.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_FrequencyGain,
    //        (x) => target.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = x,
    //        endValue,
    //        duration
    //        );
    //    return ter;
    //}

    public static Tweener DOWidth(RectTransform target, Vector2 endValue, float duration)
    {
        Tweener tweener = DOTween.To(
            () => target.sizeDelta,
            (x) => target.sizeDelta = x,
            endValue,
            duration
            );
        return tweener;
    }

    public static Tweener DOWidthAndCall(RectTransform target, Vector2 endValue, float duration, Action callback)
    {
        Tweener tweener = DOTween.To(
            () => target.sizeDelta,
            (x) => target.sizeDelta = x,
            endValue,
            duration
            ).OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
        return tweener;
    }

    public static Tweener DOFillAmount(UnityEngine.UI.Image target, float endValue, float duration)
    {
        Tweener tweener = DOTween.To(
            () => target.fillAmount,
            (x) => target.fillAmount = x,
            endValue,
            duration
            );
        return tweener;
    }

    public static Tweener DOFillAmountAndCall(UnityEngine.UI.Image target, float endValue, float duration, Action callback)
    {
        Tweener tweener = DOTween.To(
            () => target.fillAmount,
            (x) => target.fillAmount = x,
            endValue,
            duration
            ).OnComplete(() =>
            {
                if (callback != null)
                {
                    callback();
                }
            });
        return tweener;
    }

    public static void DOFloatForLua(GameObject target, float endValue, float duration, string tag, Action callback)
    {
        Tweener tweener = null;
        RectTransform rectTrans = target.GetComponent<RectTransform>();
        switch (tag)
        {
            case "RectTransform_height":

                tweener = DOTween.To(
                    () => rectTrans.sizeDelta.y,
                    (y) => rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, y),
                    endValue,
                    duration
                    );

                break;
            case "Image_alpha":
                UnityEngine.UI.Image image = target.GetComponent<UnityEngine.UI.Image>();
                tweener = DOTween.To(
                    () =>
                    {
                        return image.color.a;
                    },
                    (x) =>
                    {
                        Color origin = image.color;
                        image.color = new Color(origin.r, origin.g, origin.b, x);
                    },
                    endValue,
                    duration
                    );
                break;
            case "Text_alpha":
                UnityEngine.UI.Text text = target.GetComponent<UnityEngine.UI.Text>();
                tweener = DOTween.To(
                    () =>
                    {
                        return text.color.a;
                    },
                    (x) =>
                    {
                        Color origin = text.color;
                        text.color = new Color(origin.r, origin.g, origin.b, x);
                    },
                    endValue,
                    duration
                    );
                break;
                //case "Image_Brightness":
                //break;
                //case "Image_Brightness":
        }
        TweenCallback call = delegate { if (callback != null) callback(); };
        TweenSettingsExtensions.OnComplete(tweener, call);
    }

    public static Tweener DOBrightnessForLua(GameObject target, float endValue, float duration, string tag)
    {

        Tweener tweener = null;
        DG.Tweening.Core.DOGetter<float> getter = null;
        DG.Tweening.Core.DOSetter<float> setter = null;
        switch (tag)
        {
            case "Image":
                getter = () =>
                {
                    UnityEngine.UI.Image image = target.GetComponent<UnityEngine.UI.Image>();
                    Color color = image.color;
                    float h, s, b;
                    Color.RGBToHSV(color, out h, out s, out b);
                    return b;

                };
                setter = (x) =>
                {
                    UnityEngine.UI.Image image = target.GetComponent<UnityEngine.UI.Image>();
                    Color color = image.color;
                    float h, s, b;
                    Color.RGBToHSV(color, out h, out s, out b);
                    image.color = Color.HSVToRGB(h, s, x);
                };
                break;
            case "RawImage":
                getter = () =>
                {
                    UnityEngine.UI.RawImage image = target.GetComponent<UnityEngine.UI.RawImage>();
                    Color color = image.color;
                    float h, s, b;
                    Color.RGBToHSV(color, out h, out s, out b);
                    return b;

                };
                setter = (x) =>
                {
                    UnityEngine.UI.RawImage image = target.GetComponent<UnityEngine.UI.RawImage>();
                    Color color = image.color;
                    float h, s, b;
                    Color.RGBToHSV(color, out h, out s, out b);
                    image.color = Color.HSVToRGB(h, s, x);
                };
                break;
        }
        if (getter != null && setter != null)
        {
            tweener = DOTween.To(getter, setter, endValue, duration);
        }

        return tweener;
    }
    public static void DOLocalMoveX(Transform target, float endValue, float dur, bool snap)
    {
        DG.Tweening.ShortcutExtensions.DOLocalMoveX(target, endValue, dur, snap);
    }
    public static void DOLocalMoveXOnComplete(Transform target, float endValue, float dur, bool snap, Action callback)
    {
        Tweener tweener = DG.Tweening.ShortcutExtensions.DOLocalMoveX(target, endValue, dur, snap);
        TweenCallback call = delegate { if (callback != null) callback(); };
        TweenSettingsExtensions.OnComplete(tweener, call);
    }
    public static void DOLocalMove(Transform target, Vector3 endValue, float dur, bool snap)
    {
        DG.Tweening.ShortcutExtensions.DOLocalMove(target, endValue, dur, snap);
    }
    public static void DOLocalMoveOnComplete(Transform target, Vector3 endValue, float dur, bool snap, Action callback)
    {
        Tweener tweener = DG.Tweening.ShortcutExtensions.DOLocalMove(target, endValue, dur, snap);
        TweenCallback call = delegate { if (callback != null) callback(); };
        TweenSettingsExtensions.OnComplete(tweener, call);
    }
    public static void DOScale(Transform target, Vector3 endValue, float dur)
    {
        DG.Tweening.ShortcutExtensions.DOScale(target, endValue, dur);
    }


    public static void DOScaleOnComplete(Transform target, Vector3 endValue, float dur, Action callback)
    {
        Tweener tweener = DG.Tweening.ShortcutExtensions.DOScale(target, endValue, dur);
        TweenCallback call = delegate { if (callback != null) callback(); };
        TweenSettingsExtensions.OnComplete(tweener, call);
    }

    //public static void DODelayScaleOnComplete(Transform target, Vector3 endValue, float dur, float delay, Action callback)
    //{
    //    FreeCoroutine.Instance.StartCoroutine(IEnumeratorScale(target, endValue, dur, delay, callback));
    //}
    public static IEnumerator IEnumeratorScale(Transform target, Vector3 endValue, float dur, float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        DOScaleOnComplete(target, endValue, dur, callback);

    }

    //public static void DOFade46(CanvasGroup canvasGroup, float endValue, float dur, Action complete)
    //{
    //    Tweener target = DG.Tweening.ShortcutExtensions46.DOFade(canvasGroup, endValue, dur);
    //    if (complete != null)
    //    {
    //        DG.Tweening.TweenSettingsExtensions.OnComplete(target, () =>
    //        {
    //            complete();
    //        });
    //    }
    //}

    public static void DOLocalRotateOnComplete(Transform target, Vector3 roteValue, float dur, Action callback)
    {
        Tweener tweener = DG.Tweening.ShortcutExtensions.DOLocalRotate(target, roteValue, dur);
        TweenCallback call = delegate { if (callback != null) callback(); };
        TweenSettingsExtensions.OnComplete(tweener, call);
    }

    public static void DOLocalPathOnComplete(Transform target, Vector3[] path, float duration, Action callback)
    {
        Tweener tweener = DG.Tweening.ShortcutExtensions.DOLocalPath(target, path, duration, PathType.CatmullRom);
        TweenCallback call = delegate { if (callback != null) callback(); };
        TweenSettingsExtensions.OnComplete(tweener, call);
    }

    public static void DOPlay(Transform target)
    {
        DG.Tweening.ShortcutExtensions.DOPlay(target);
    }
    public static void TweenSettingsExtensions_OnComplete(Tweener target, Action complete)
    {
        DG.Tweening.TweenSettingsExtensions.OnComplete(target, () =>
        {
            if (complete != null)
            {
                complete();
            }
        });
    }

    public static bool IsTweening(object targetOrId)
    {
        return DOTween.IsTweening(targetOrId);
    }

    public static int CompleteTweenRightNow(object targetOrId, bool withCallbacks = false)
    {
        return DOTween.Complete(targetOrId, withCallbacks);
    }




}
