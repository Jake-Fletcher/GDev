//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public sealed class RequiredAttribute : PropertyAttribute
{
    public readonly string message = "This field is required";
    public readonly bool overrideMessage;

    public RequiredAttribute()
    {
        overrideMessage = false;
    }

    public RequiredAttribute(string message)
    {
        overrideMessage = true;
        this.message = message;
    }
}
