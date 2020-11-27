using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public ParticleSystem boom;
    bool runOnce;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <+ 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
            Explode();
    }

    private void Start()
    {
        runOnce = false;
    }

    void Explode()
    {
        if(runOnce == false)
        {
            Instantiate(boom, transform.position, transform.rotation);
            runOnce = true;
        }
    }
}
