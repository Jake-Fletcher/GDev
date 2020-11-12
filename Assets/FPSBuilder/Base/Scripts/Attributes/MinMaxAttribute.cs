//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public sealed class MinMaxAttribute : PropertyAttribute
{
    public readonly float min;
    public readonly float max;

    public MinMaxAttribute(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}