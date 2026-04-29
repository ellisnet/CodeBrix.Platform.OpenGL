// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "ParameterRangeEXT")]
public enum ParameterRangeEXT : int
{
    [NativeName("Name", "GL_NORMALIZED_RANGE_EXT")]
    NormalizedRangeExt = 0x87E0,
    [NativeName("Name", "GL_FULL_RANGE_EXT")]
    FullRangeExt = 0x87E1,
}
