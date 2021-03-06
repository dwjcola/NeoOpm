using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectGizmosCircleLine : MonoBehaviour
{
    private bool isDraw = false;
    //?????뾶
    private float volume_r = 0;
    //ռλ?뾶 ??̬??
    private float place_r = 0;
    //??λ?뾶
    private float dislocation_r = 0;
    //?????뾶
    private float detection_r = 0;
    // ?????뾶
    private float attack_r = 0;
    // ռλ?뾶 ??????
    private float place_r_base = 0;
    public void BeginDraw(float volume_r = 0, float place_r = 0, float dislocation_r = 0, float detection_r = 0,float attack_r = 0,float place_r_base = 0)
    {
        this.volume_r = volume_r;
        this.place_r = place_r;
        this.dislocation_r = dislocation_r;
        this.detection_r = detection_r;
        this.attack_r = attack_r;
        this.place_r_base = place_r_base;
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
            transform.DrawGizmoCircleLine(attack_r, Color.red);
            transform.DrawGizmoCircleLine(volume_r, Color.white);
            transform.DrawGizmoCircleLine(place_r, Color.yellow);
            transform.DrawGizmoCircleLine(dislocation_r, Color.green);
            transform.DrawGizmoCircleLine(detection_r, Color.blue);
            transform.DrawGizmoCircleLine(place_r_base, Color.gray);
        }
    }
}

