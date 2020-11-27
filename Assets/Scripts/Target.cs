using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public ParticleSystem boom;

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

    void Explode()
    {

        Instantiate(boom, transform.position, transform.rotation);

    }
}
