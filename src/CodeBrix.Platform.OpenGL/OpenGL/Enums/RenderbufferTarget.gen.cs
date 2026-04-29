// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "RenderbufferTarget")]
public enum RenderbufferTarget : int
{
    [NativeName("Name", "GL_RENDERBUFFER")]
    Renderbuffer = 0x8D41,
    [NativeName("Name", "GL_RENDERBUFFER_OES")]
    RenderbufferOes = 0x8D41,
}
