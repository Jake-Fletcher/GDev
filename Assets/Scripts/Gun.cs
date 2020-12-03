using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem bigMuzzleFlash;
    public GameObject impactEffect;
    public GameObject bigImpactEffect;
    public float impactForce = 50f;
    public float fireRate = 15f;

    private float nextTimeToFire = 0f;
/*    [SerializeField] AudioClip shotFired;
    AudioSource source;*/


/*    private void Start()
    {
        source = GetComponent<AudioSource>();
    }*/

    private void Update()
    {
        if (Input.GetButton("mouse 1") && Time.time >+ nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (Input.GetButton("mouse 2") && Time.time > +nextTimeToFire)
        {
            nextTimeToFire = Time.time + 10f / fireRate;
            BigShoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
/*        source.PlayOneShot(shotFired);*/
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }

    void BigShoot()
    {
        bigMuzzleFlash.Play();
        /*        source.PlayOneShot(shotFired);*/
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce*10);
            }

            GameObject impactGO = Instantiate(bigImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }
}
