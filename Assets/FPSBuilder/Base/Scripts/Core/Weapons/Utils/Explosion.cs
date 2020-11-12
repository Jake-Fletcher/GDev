using FPSBuilder.Interfaces;
using UnityEngine;

namespace FPSBuilder.Core.Weapons
{
    public static class Explosion
    {
        private static RaycastHit hitInfo;

        /// <summary>
        /// Calculates the effects of the explosion as damage and force applied to nearby objects.
        /// </summary>
        /// <param name="radius">The blast radius.</param>
        /// <param name="force">How strong the objects will be pushed if within the blast radius.</param>
        /// <param name="damage">The damage dealt to objects if within the blast radius.</param>
        /// <param name="position">The world space position of the explosion.</param>
        /// <param name="ignoreCover">Should the explosion ignore objects within the radius that may block its effects?</param>
        public static void CalculateExplosionDamage(float radius, float force, float damage, Vector3 position, bool ignoreCover = false)
        {
            // Array of colliders near of the player.
            Collider[] m_HitColliders = new Collider[128];

            // Amount of objects near the explosion.
            int amount = Physics.OverlapSphereNonAlloc(position, radius, m_HitColliders);

            if (amount == 0)
                return;

            for (int i = 0; i < amount; i++)
            {
                Collider c = m_HitColliders[i];

                if (c.gameObject.isStatic)
                    continue;

                Vector3 pos = c.transform.position;
                Vector3 direction = (pos - position).normalized;
                float intensity = (radius - Vector3.Distance(position, pos)) / radius;

                IStunnable stunnableTarget =  c.GetComponent<IStunnable>();
                stunnableTarget?.DeafnessEffect(intensity);

                if (!TargetInSight(position, c, ignoreCover))
                    continue;

                IExplosionDamageable damageableTarget = c.GetComponent<IExplosionDamageable>();
                damageableTarget?.ExplosionDamage(intensity * damage, position, hitInfo.point);

                Rigidbody rigidBody = c.GetComponent<Rigidbody>();
                if (rigidBody && !c.CompareTag("Player"))
                {
                    if (!rigidBody.isKinematic)
                    {
                        // Apply force to all rigidBody hit by explosion (except the player).
                        rigidBody.AddForce(direction * (force * intensity), ForceMode.Impulse);
                    }
                }
            }
        }

        /// <summary>
        /// Cast 5 rays towards the object's bounding box to see if there is any object blocking the view.
        /// </summary>
        /// <param name="position">The explosion position.</param>
        /// <param name="target">The object collider.</param>
        /// <param name="ignoreCover">Should the explosion ignore objects within the radius that may block its effects?</param>
        /// <returns></returns>
        private static bool TargetInSight(Vector3 position, Collider target, bool ignoreCover = false)
        {
            if (ignoreCover)
                return true;

            // Object center.
            if (Physics.Linecast(position, target.bounds.center, out hitInfo) && hitInfo.collider == target)
                return true;

            // Object top.
            if (Physics.Linecast(position, target.bounds.center + Vector3.up * target.bounds.extents.y, out hitInfo) && hitInfo.collider == target)
                return true;

            // Object bottom.
            if (Physics.Linecast(position, target.bounds.center + Vector3.down * target.bounds.extents.y, out hitInfo) && hitInfo.collider == target)
                return true;

            // Object right.
            if (Physics.Linecast(position, target.bounds.center + Vector3.right * target.bounds.extents.x, out hitInfo) && hitInfo.collider == target)
                return true;

            // Object left.
            if (Physics.Linecast(position, target.bounds.center + Vector3.left * target.bounds.extents.x, out hitInfo) && hitInfo.collider == target)
                return true;

            return false;
        }
    }
}

