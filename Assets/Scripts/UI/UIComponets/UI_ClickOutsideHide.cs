using UnityEngine;
using System;
namespace ProHA
{

public class UI_ClickOutsideHide : MonoBehaviour
{
    private Rect m_rect;
    private Camera m_uguiCamera;
    private RectTransform m_canvas;
    private bool m_canCheck = false;
    private float width = 0;
    private float height = 0;
    private RectTransform rectTrans;
    [SerializeField]
    string UIName;
    private void Awake()
    {
        this.rectTrans = this.GetComponent<RectTransform>();
    }
    RectTransform UICanvas
    {
        get
        {
            if (m_canvas == null)
            {
                m_canvas = UIUtility.GetRoot().GetComponent<RectTransform>();
            }
            return m_canvas;
        }
    }

    void OnEnable()
    {
        m_canCheck = true;
    }

    void OnDisable()
    {
        m_canCheck = false;
    }

    public void OnResize()
    {
        Vector3[] corners = new Vector3[4];
        this.rectTrans.GetWorldCorners(corners);
        float width = Math.Abs(Vector2.Distance(corners[0], corners[3]));
        float height = Math.Abs(Vector2.Distance(corners[0], corners[1]));
        m_rect = new Rect(corners[0], new Vector2(width, height));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_canCheck)
        {
            return;
        }

        if (this.width != this.rectTrans.rect.width || this.height != this.rectTrans.rect.height)
        {
            this.OnResize();
            this.width = this.rectTrans.rect.width;
            this.height = this.rectTrans.rect.height;
        }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Vector3 curPos = TransToWorldPos(Input.mousePosition);
            if (!m_rect.Contains(curPos))
            {
                //this.gameObject.SetActive(false);
                GameEntry.UI.CloseUI(UIName);
            }
        }
    }

    public static Vector2 TransToWorldPos(Vector3 mousePosition)
    {
        RectTransform rectTransform = UIUtility.GetRoot().GetComponent<RectTransform>();
        return UIUtility.ScreenToWorldPosInRectangle(mousePosition, rectTransform);
    }
}
    
}