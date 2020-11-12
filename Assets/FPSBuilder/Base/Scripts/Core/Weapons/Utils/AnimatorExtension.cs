//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Core.Weapons
{
    public static class AnimatorExtension
    {
        /// <summary>
        /// Returns the Animation Clip with the corresponding name.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="name">The Animation Clip name.</param>
        /// <returns></returns>
        public static AnimationClip GetAnimationClip(this Animator animator, string name)
        {
            if (!animator)
                return null;

            for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (animator.runtimeAnimatorController.animationClips[i].name == name)
                    return animator.runtimeAnimatorController.animationClips[i];
            }
            return null;
        }
    }
}
