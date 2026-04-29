// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "TextureFilterSGIS")]
public enum TextureFilterSGIS : int
{
    [NativeName("Name", "GL_FILTER4_SGIS")]
    Filter4Sgis = 0x8146,
}
