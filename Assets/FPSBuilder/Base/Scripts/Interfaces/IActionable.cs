//=========== Copyright (c) GameBuilders, All rights reserved. ================//

namespace FPSBuilder.Interfaces
{
    public interface IActionable
    {
        bool RequiresAnimation
        {
            get;
        }

        void Interact();

        string Message();
    }
}
