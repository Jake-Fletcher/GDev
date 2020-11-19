using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlNPC : MonoBehaviour
{
    bool screamed = false;
    [SerializeField] AudioClip spotted;
    AudioSource src;
    Animator anim;
    AnimatorStateInfo info;
    Ray ray;
    RaycastHit hit;
    int WPIndex = 1;
    void Start()
    {
        src = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        print("We did it boys: " + GameObject.Find("FPSController"));
    }

    void Update()
    {

        info = anim.GetCurrentAnimatorStateInfo(0);
        ray.origin = transform.position;
        ray.direction = transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        if (Physics.Raycast(ray.origin, ray.direction * 100, out hit))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                anim.SetBool("CanSeePlayer", true);
            }
        }



        if (info.IsName("Idle"))
        {
            //Debug.Log("In Idle");
            GetComponent<NavMeshAgent>().isStopped = false;
        }

        else if (info.IsName("FollowPlayer"))
        {
            if (!screamed) { 
                src.PlayOneShot(spotted);
                screamed = true;
            }


            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().destination = GameObject.Find("FPSController").transform.position;
        }
        else if (info.IsName("Patrol"))
        {
            //Debug.Log("Patrol");
            GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().destination = GameObject.Find("WP" + WPIndex).transform.position;
            if (Vector3.Distance(transform.position, GameObject.Find("WP" + WPIndex).transform.position) < 1)
            {
                WPIndex++;
                if (WPIndex > 3) WPIndex = 1;
            }
        }

    }
}
