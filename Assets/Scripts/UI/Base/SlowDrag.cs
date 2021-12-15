using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace ProHA
{

public class SlowDrag : MonoBehaviour
{
    [SerializeField]
    private ScrollRect scrollRect;
    public float moveRate = 1;
    private Vector2 startPos;
    private RectTransform thisRect;
    void Awake()
    {
        thisRect = GetComponent<RectTransform>();
        ResetScrollRect(scrollRect, thisRect.anchoredPosition);
    }
    public void ResetScrollRect(ScrollRect newScrollRect,Vector2 newStartPos)
    {
        if (newScrollRect == null)
            return;
        scrollRect = newScrollRect;
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener((pos) =>
        {
            thisRect.anchoredPosition = newStartPos + scrollRect.content.anchoredPosition * moveRate;
        });
      
    }
  
}
    
}