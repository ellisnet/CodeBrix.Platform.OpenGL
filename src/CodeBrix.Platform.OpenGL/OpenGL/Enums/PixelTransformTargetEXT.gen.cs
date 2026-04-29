// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "PixelTransformTargetEXT")]
public enum PixelTransformTargetEXT : int
{
    [NativeName("Name", "GL_PIXEL_TRANSFORM_2D_EXT")]
    PixelTransform2DExt = 0x8330,
}
