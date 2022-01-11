using UnityEngine;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace NeoOPM
{

public class ReminderItem : MonoBehaviour
{
    public TextMeshProUGUI contentLab;
    public DOTweenAnimation animation;
    //private const int MoveHeight = 250;

    //public static bool Running = false;
    void Awake()
    {
        
    }

    public void SetContent(string content)
    {
        if (contentLab != null)
        {
            contentLab.text = content;
        }

    }
    public Color Color
    {
        set
        {
            //TextMeshPro contentLab = GetComponent<TextMeshPro>();
            if (contentLab != null)
            {
                contentLab.color = value;
            }
        }

    }

    public virtual void SendOut()
    {
        //Running = true;
        //  UIReminder.UpQueue.Remove(this);
        animation.DORestart();
        gameObject.SetActive(true);

       // StartCoroutine(play());

        Invoke("tweenFinish", 3f);
    }
    public virtual void tweenFinish() 
    {
        //Running = false;
        //Destroy(this.gameObject);
        UIReminder.RecycleReminder(this);
    }
}
    
}