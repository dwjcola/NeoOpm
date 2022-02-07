using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragEventSyn : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public ScrollRect parentScrollRect;

    void Start()
    {
        if (parentScrollRect == null)
        {
            if (transform.parent)
            {
                parentScrollRect = transform.parent.GetComponentInParent<ScrollRect>();
            }
            if (parentScrollRect == null)
            {
                Debug.Log("请手动关联父物体");
            }
        }        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (parentScrollRect)
        {
            parentScrollRect.OnEndDrag(eventData);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentScrollRect)
        {
            parentScrollRect.OnBeginDrag(eventData);
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (parentScrollRect)
        {
            parentScrollRect.OnDrag(eventData);
        }

    }
}
