// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "DepthStencilTextureMode")]
public enum DepthStencilTextureMode : int
{
    [NativeName("Name", "GL_STENCIL_INDEX")]
    StencilIndex = 0x1901,
    [NativeName("Name", "GL_DEPTH_COMPONENT")]
    DepthComponent = 0x1902,
}
