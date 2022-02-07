using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class GameObjectGizmosCircle : MonoBehaviour
{
    private bool isDraw = false;
    //����뾶
    private float volume_r = 0;
    //ռλ�뾶
    private float place_r = 0;
    //��λ�뾶
    private float dislocation_r = 0;
    //���뾶
    private float detection_r = 0;

    public void BeginDraw(float volume_r = 0, float place_r = 0, float dislocation_r = 0, float detection_r = 0)
    {
        this.volume_r = volume_r;
        this.place_r = place_r;
        this.dislocation_r = dislocation_r;
        this.detection_r = detection_r;
        isDraw = true;
    }

    public void StopDraw()
    {
        isDraw = false;
    }

    private void OnDrawGizmos()
    {
        if (isDraw)
        {
            transform.DrawGizmoDisk(volume_r,Color.red);
            transform.DrawGizmoDisk(place_r, Color.yellow);
            transform.DrawGizmoDisk(dislocation_r, Color.green);
            transform.DrawGizmoDisk(detection_r, Color.blue);
        }
    }
}
