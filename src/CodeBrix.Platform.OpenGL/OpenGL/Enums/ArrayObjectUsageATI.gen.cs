// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "ArrayObjectUsageATI")]
public enum ArrayObjectUsageATI : int
{
    [NativeName("Name", "GL_STATIC_ATI")]
    StaticAti = 0x8760,
    [NativeName("Name", "GL_DYNAMIC_ATI")]
    DynamicAti = 0x8761,
}
