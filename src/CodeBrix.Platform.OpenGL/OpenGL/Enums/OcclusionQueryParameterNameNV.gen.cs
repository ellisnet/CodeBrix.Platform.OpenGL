// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "OcclusionQueryParameterNameNV")]
public enum OcclusionQueryParameterNameNV : int
{
    [Obsolete("Deprecated in favour of \"NV\"")]
    [NativeName("Name", "GL_PIXEL_COUNT_NV")]
    PixelCountNV = 0x8866,
    [Obsolete("Deprecated in favour of \"AvailableNV\"")]
    [NativeName("Name", "GL_PIXEL_COUNT_AVAILABLE_NV")]
    PixelCountAvailableNV = 0x8867,
    [NativeName("Name", "GL_PIXEL_COUNT_NV")]
    NV = 0x8866,
    [NativeName("Name", "GL_PIXEL_COUNT_AVAILABLE_NV")]
    AvailableNV = 0x8867,
}
