// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "SpriteParameterNameSGIX")]
public enum SpriteParameterNameSGIX : int
{
    [NativeName("Name", "GL_SPRITE_MODE_SGIX")]
    SpriteModeSgix = 0x8149,
}
