//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Interfaces
{
    public interface IWeapon : IEquipable
    {
        int Identifier
        {
            get;
        }

        GameObject Viewmodel
        {
            get;
        }

        bool CanSwitch
        {
            get;
        }

        bool CanUseItems
        {
            get;
        }

        bool CanVault
        {
            get;
        }

        float HideAnimationLength
        {
            get;
        }

        float InteractAnimationLength
        {
            get;
        }

        float InteractDelay
        {
            get;
        }

        void Interact();
    }
}