using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorC : MonoBehaviour
{
    public Animator animator;
    float normalizedTransitionDuration = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(transform.localRotation);
      
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    animator.CrossFade("Run", normalizedTransitionDuration);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    animator.CrossFade("Hard_Attk_1", normalizedTransitionDuration);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    animator.CrossFade("Light_Attk_1", normalizedTransitionDuration);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    animator.CrossFade("Dead_1", normalizedTransitionDuration);
        //}
    }

    private void OnDrawGizmos()
    {
        this.transform.DrawGizmoDisk(8,Color.black);

        this.transform.DrawGizmoDisk(5, Color.blue);
    }
}
