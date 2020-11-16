using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPC : MonoBehaviour
{
    Animator anim;
    AnimatorStateInfo info;
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        ray.origin = transform.position + Vector3.up;
        ray.direction = transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit))
        {
            if (hit.collider.gameObject.name == "FPS_Alt")
            {
                anim.SetBool("CanSeePlayer", true);
            }
        }


       /* if (Input.GetKeyDown(KeyCode.I))
        {
            anim.SetBool("CanSeePlayer", true);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetBool("CanSeePlayer", false);
        }*/

        float distanceBetweenPlayerAndNPC = Vector3.Distance(gameObject.transform.position, GameObject.Find("FPS_Alt").transform.position);
        if (distanceBetweenPlayerAndNPC < 5)
        {
            anim.SetBool("CanSeePlayer", true);
        }
        else
        {
            anim.SetBool("CanSeePlayer", false);
        }

        if (info.IsName("Idle"))
        {
            Debug.Log("In Idle");
            GetComponent<NavMeshAgent>().isStopped = false;
        }
        
        else if (info.IsName("FollowPlayer"))
        {
            Debug.Log("Follow Player state");
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().destination = GameObject.Find("FPS_Alt").transform.position;
        }


    }
}
