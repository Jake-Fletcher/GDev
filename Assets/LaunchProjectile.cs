using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchProjectile : MonoBehaviour
{

    public GameObject projectile;
    public GameObject target;
    float time;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.F))
        time += Time.deltaTime;
        if (time > 1.0f)
        {
            time = 0;
            GameObject t = (GameObject)Instantiate(projectile, transform.position, Quaternion.identity);
            Destroy(t, 3);
            transform.LookAt(target.transform);
            t.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
        }

    }
}
