// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "PathElementType")]
public enum PathElementType : int
{
    [NativeName("Name", "GL_UTF8_NV")]
    Utf8NV = 0x909A,
    [NativeName("Name", "GL_UTF16_NV")]
    Utf16NV = 0x909B,
}
