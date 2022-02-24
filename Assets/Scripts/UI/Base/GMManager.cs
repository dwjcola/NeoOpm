#if UNITY_EDITOR
using UnityEngine;
public class GMManager : MonoBehaviour {
    //记录上一次手机触摸位置
    private Vector2 _oldPostion1 = Vector2.zero;
    private Vector2 _oldPostion2 = Vector2.zero;
    
    private void Update()
    {
        //编辑器触发
        if (Input.GetKeyDown(KeyCode.F1))//打开
        {
            OpenGM();
        }
        else if (Input.GetKeyDown(KeyCode.F2))//关闭
        {
            CloseGM();
        }

        //移动端触发
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                _oldPostion1 = Input.GetTouch(0).position;
                _oldPostion2 = Input.GetTouch(1).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                if (Input.GetTouch(0).position.x - _oldPostion1.x > 20 && Input.GetTouch(1).position.x - _oldPostion2.x > 20)
                {
                    OpenGM();
                }
                else if (_oldPostion1.x - Input.GetTouch(0).position.x > 20 && _oldPostion2.x - Input.GetTouch(1).position.x > 20)
                {
                    CloseGM();
                }
            }
        }
    }
    public void OpenGM()
    {
        LC.OpenUI("GMCommandForm");
    }

    public void CloseGM()
    {
        LC.CloseUI("GMCommandForm");
    }
}
#endif