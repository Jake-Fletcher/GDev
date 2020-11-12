//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Core
{
    /// <summary>
    /// The Particle Destroyer waits until all the particles in the simulation are finished and then destroys the GameObject.
    /// </summary>
    public sealed class ParticleDestroyer : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
            InvokeRepeating(nameof(DestroyParticle), 0, 1);
        }

        private void DestroyParticle()
        {
            ParticleSystem particle = GetComponentInChildren<ParticleSystem>();

            if (!particle.IsAlive(true))
            {
                Destroy(gameObject);
            }
        }
    }
}
