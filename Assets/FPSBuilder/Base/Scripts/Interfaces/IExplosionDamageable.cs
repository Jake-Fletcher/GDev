//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Interfaces
{
    public interface IExplosionDamageable : IDamageable
    {
        void ExplosionDamage(float damage, Vector3 targetPosition, Vector3 hitPosition);
    }
}
