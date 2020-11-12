//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Interfaces
{
    public interface IDamageable
    {
        bool IsAlive
        {
            get;
        }

        void Damage(float damage);

        void Damage(float damage, Vector3 targetPosition, Vector3 hitPosition);
    }
}