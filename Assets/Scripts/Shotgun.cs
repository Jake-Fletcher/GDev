using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public Transform spawnPoint;
    public float distance = 15;
    public GameObject muzzle;
    public GameObject impact;
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire"))
            Shoot();
    }

    private void Shoot()
    {
        RaycastHit hit;
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;
        RaycastHit hit4;
        RaycastHit hit5;
        RaycastHit hit6;
        RaycastHit hit7;

        GameObject muzzleInstance = Instantiate(muzzle, spawnPoint.position, spawnPoint.localRotation);
        muzzleInstance.transform.parent = spawnPoint;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance))
        {
            Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(-.2f, 0, 0), out hit1, distance))
        {
            Instantiate(impact, hit1.point, Quaternion.LookRotation(hit1.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(0, -.2f, 0), out hit2, distance))
        {
            Instantiate(impact, hit2.point, Quaternion.LookRotation(hit2.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(0, 0, -.2f), out hit3, distance))
        {
            Instantiate(impact, hit3.point, Quaternion.LookRotation(hit3.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(-.2f, -.2f, 0), out hit4, distance))
        {
            Instantiate(impact, hit4.point, Quaternion.LookRotation(hit4.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(0, -.2f, -.2f), out hit5, distance))
        {
            Instantiate(impact, hit5.point, Quaternion.LookRotation(hit5.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(-.2f, 0, -.2f), out hit6, distance))
        {
            Instantiate(impact, hit6.point, Quaternion.LookRotation(hit6.normal));
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward + new Vector3(0, 0, 0), out hit7, distance))
        {
            Instantiate(impact, hit7.point, Quaternion.LookRotation(hit7.normal));
        }
    }
}
