using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNPC : MonoBehaviour
{
    Animator anim;
    AnimatorStateInfo info;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
       if(Input.GetKeyDown(KeyCode.I))
        {
            anim.SetBool("CanSeePlayer", true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetBool("CanSeePlayer", false);
        }

        if (info.IsName("Idle"))
        {
            Debug.Log("In Idle");
        }
        
        else if (info.IsName("FollowPlayer"))
        {
            Debug.Log("Follow Player state");
        }


    }
}
