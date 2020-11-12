//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.Core
{
    public static class MathfUtilities
    {
        /// <summary>
        /// Returns a random point within the space defined by the lower and upper bounds.
        /// </summary>
        /// <param name="min">Lower bounds.</param>
        /// <param name="max">Upper bounds.</param>
        public static Vector3 RandomInsideBounds(Vector3 min, Vector3 max)
        {
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        /// <summary>
        /// 	Returns the midpoint between two vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3 Midpoint(Vector3 a, Vector3 b)
        {
            return new Vector3((a.x + b.x) / 2, (a.y + b.y) / 2, (a.z + b.z) / 2);
        }

        /// <summary>
        /// Clamps the given Quaternion's X-axis between the given minimum float and maximum float values.
        /// Returns the given value if it is within the min and max range.
        /// </summary>
        /// <param name="q">Target Quaternion.</param>
        /// <param name="minimum">Minimum float value.</param>
        /// <param name="maximum">Maximum float value.</param>
        /// <returns></returns>
        public static Quaternion ClampRotationAroundXAxis(Quaternion q, float minimum, float maximum)
        {
            q.x /= q.w;
            q.y = 0;
            q.z = 0;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, minimum, maximum);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}

