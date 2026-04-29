// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "MapTarget")]
public enum MapTarget : int
{
    [NativeName("Name", "GL_GEOMETRY_DEFORMATION_SGIX")]
    GeometryDeformationSgix = 0x8194,
    [NativeName("Name", "GL_TEXTURE_DEFORMATION_SGIX")]
    TextureDeformationSgix = 0x8195,
}
