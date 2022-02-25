using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimatorC : MonoBehaviour
{
    public Animator animator;
    float normalizedTransitionDuration = 0.1f;
    string state = "";
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(transform.localRotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (state == "Attack")
            {
                animator.Play("Attack", 0,0);
            }
            else
             animator.CrossFade("Attack", normalizedTransitionDuration);
            state = "Attack";
        }
        if (Input.GetKeyDown(KeyCode.W))
        {

            if (state == "Ultra")
            {
                animator.Play("Ultra", 0, 0);
            }
            else
                animator.CrossFade("Ultra", normalizedTransitionDuration);
            state = "Ultra";
        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (state == "Light_Attk_1")
            {
                animator.Play("Light_Attk_1", 0, 0);
            }
            else
                animator.CrossFade("Light_Attk_1", normalizedTransitionDuration);
            state = "Light_Attk_1";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (state == "Dead_1")
            {
                animator.Play("Dead_1", 0, 0);
            }
            else
                animator.CrossFade("Dead_1", normalizedTransitionDuration);
            state = "Dead_1";
        }
    }

    private void OnDrawGizmos()
    {
        this.transform.DrawGizmoDisk(8,Color.black);

        this.transform.DrawGizmoDisk(5, Color.blue);
    }
}
