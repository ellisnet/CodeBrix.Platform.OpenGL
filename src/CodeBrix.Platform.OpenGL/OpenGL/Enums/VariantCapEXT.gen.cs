// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "VariantCapEXT")]
public enum VariantCapEXT : int
{
    [NativeName("Name", "GL_VARIANT_ARRAY_EXT")]
    VariantArrayExt = 0x87E8,
}
