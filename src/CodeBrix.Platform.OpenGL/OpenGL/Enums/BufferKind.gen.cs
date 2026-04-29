// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "Buffer")]
public enum BufferKind : int
{
    [NativeName("Name", "GL_COLOR")]
    Color = 0x1800,
    [NativeName("Name", "GL_DEPTH")]
    Depth = 0x1801,
    [NativeName("Name", "GL_STENCIL")]
    Stencil = 0x1802,
}
