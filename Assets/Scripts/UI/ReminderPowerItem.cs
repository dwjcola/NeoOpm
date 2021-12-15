using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

namespace ProHA
{

public class ReminderPowerItem : ReminderItem
{
    public Image bg;
    public Image arrow;
    Action endEvent;
    Vector3 endValueV3;
    public override void SendOut()
    {
        Recycle();
        animation.DORestartById("ScaleShow");
        gameObject.SetActive(true);
        //bg.transform.DOScaleX(1, 0.2f).onComplete = () => {
        //    bg.DOColor(new Color(1, 1, 1, 0), 0.1f).SetDelay(1f).onComplete = () => {
        //        bg.transform.DOMove(endValueV3,0.2f).onComplete=()=> { tweenFinish(); };
        //        Debug.LogError(endValueV3);
        //    };
        //};
    }
    public void SetTargetPos(Vector3 target)
    {
        endValueV3 = target;
           var tweens = animation.GetComponents<DOTweenAnimation>();
        var t = Array.Find(tweens, (f) => { return f.id == "MoveTarget"; });
        if (t == null)
            return;
        t.tween.Rewind();
        t.tween.Kill();
        t.endValueV3 = target;
        t.useTargetAsV3 = false;
        t.targetType = DOTweenAnimation.TargetType.Transform;
        if (t.isValid)
            t.CreateTween();
    }
    public void SetEndEvent(Action _event)
    {
        endEvent = _event;
    }
    public void Recycle()
    {
        if (bg != null)
        {
            bg.color = Color.white;
            bg.transform.localPosition = Vector3.zero;
        }
    }
    public override void tweenFinish()
    {
        base.tweenFinish();
        endEvent?.Invoke();
    }
}
    
}