//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEngine;

namespace FPSBuilder.UI
{
    public class Indicator : MonoBehaviour
    {
        public Vector3 TargetPosition { get; set; }
        public float Alpha { get; set; }

        public float GetAngleRelativeToTranform (Transform transform)
        {
            Vector3 direction = (TargetPosition - transform.position).normalized;
            return Mathf.Atan2(direction.x, direction.z) * -Mathf.Rad2Deg + transform.eulerAngles.y - 270;
        }
    }
}
