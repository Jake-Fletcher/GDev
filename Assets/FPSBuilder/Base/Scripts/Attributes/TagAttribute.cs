//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public sealed class TagAttribute : PropertyAttribute
{
    public bool AllowUntagged = false;
}