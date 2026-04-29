// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "PreserveModeATI")]
public enum PreserveModeATI : int
{
    [NativeName("Name", "GL_PRESERVE_ATI")]
    PreserveAti = 0x8762,
    [NativeName("Name", "GL_DISCARD_ATI")]
    DiscardAti = 0x8763,
}
