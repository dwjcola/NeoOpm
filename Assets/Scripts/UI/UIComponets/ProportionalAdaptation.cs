using ProHA;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
  [ExecuteAlways]
public class ProportionalAdaptation : MonoBehaviour
{
    [SerializeField]
    Vector2 baseScreenSize=new Vector2(1920,1080);
    RectTransform rect;
    // Start is called before the first frame update
    private Canvas canvas;
    Vector2 baseRect = Vector2.zero;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseRect = new Vector2(rect.rect.width, rect.rect.height);
        
    }
    void OnEnable()
    {
        
        canvas = GameEntry.UI?.InstanceRoot?.GetComponent<Canvas>();
        if (canvas == null)
        {
            CanvasScaler scaler = GetComponentInParent<CanvasScaler>();
            canvas = scaler?.GetComponent<Canvas>();
        }
        ShrinkRect();
    }
    //public void Update()
    //{
    //    ShrinkRect();
    //}
    // Update is called once per frame
   [ContextMenu("ShrinkRect")]
    public void ShrinkRect()
    {
        if (rect == null||baseScreenSize==Vector2.zero)
            return;
        //float baseRate = Shrink.x / Shrink.y;
        //float ScreenRate =(float) Screen.width / Screen.height;
        float xRate = Screen.width / baseScreenSize.x;
        float yRate = Screen.height / baseScreenSize.y;
        float scaler =canvas==null?1:canvas.scaleFactor;
        
       //Debug.LogError($"-{Screen.width}---{Screen.height}--{xRate}---{yRate}--{yRate * baseRect.x / scaler}--{yRate * baseRect.y / scaler}--{scaler}");
        rect.sizeDelta=new Vector2(Mathf.Max(xRate,yRate)* baseRect.x / scaler,Mathf.Max(xRate,yRate)* baseRect.y / scaler);
        rect.ForceUpdateRectTransforms();

    }

#if UNITY_EDITOR
    private void Update()
    {
            ShrinkRect();
    }
#endif
    
}
