//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using FPSBuilder.Core.Surface;
using UnityEngine;

namespace FPSBuilder.Core.Player
{
    /// <summary>
    /// The Ladder trigger determines a volume and verifies that the character is within it, changing its state for climbing.
    /// </summary>
    public class LadderTrigger : MonoBehaviour
    {
        /// <summary>
        /// Surface identifier used to define the sound of climbing the ladder.
        /// </summary>
        private SurfaceIdentifier m_SurfaceIdentifier;

        private void Start()
        {
            m_SurfaceIdentifier = GetComponent<SurfaceIdentifier>();
        }

        /// <summary>
        /// Checks if the character is within the boundings of the ladder.
        /// </summary>
        /// <param name="c">The collider to be tested.</param>
        private void OnTriggerStay(Collider c)
        {
            FirstPersonCharacterController FPController = c.GetComponent<FirstPersonCharacterController>();
            Bounds ladderBounds = GetComponent<Collider>().bounds;

            if (FPController != null)
            {
                FPController.ClimbingLadder(true, ladderBounds.max, m_SurfaceIdentifier);
            }
        }

        /// <summary>
        /// Checks if the character has leave the boundings.
        /// </summary>
        /// <param name="c">The collider to be tested.</param>
        private void OnTriggerExit(Collider c)
        {
            FirstPersonCharacterController FPController = c.GetComponent<FirstPersonCharacterController>();

            if (FPController != null)
            {
                FPController.ClimbingLadder(false, Vector3.zero, m_SurfaceIdentifier);
            }
        }

        /// <summary>
        /// Draw the volume boundings.
        /// </summary>
        /// <param name="selected">Is this object selected?</param>
        private void DrawBounds(bool selected)
        {
            Vector3 boundsSize = GetComponent<Collider>().bounds.size;
            Color boundsColor = new Color(0.0f, 0.7f, 1f, selected ? 0.3f : 0.1f);

            Gizmos.color = boundsColor;
            Gizmos.DrawCube(transform.position, boundsSize);

            boundsColor.a = selected ? 0.5f : 0.2f;

            Gizmos.color = boundsColor;
            Gizmos.DrawWireCube(transform.position, boundsSize);
        }

        public void OnDrawGizmos()
        {
            DrawBounds(false);
        }

        public void OnDrawGizmosSelected()
        {
            DrawBounds(true);
        }
    }
}
