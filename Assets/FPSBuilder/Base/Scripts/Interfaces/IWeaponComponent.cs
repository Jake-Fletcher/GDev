//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Interfaces
{
    public interface IWeaponComponent
    {
        void Draw();
        void Hide();
        void Hit(Vector3 position);
        void Interact();
        void Vault();
    }
}