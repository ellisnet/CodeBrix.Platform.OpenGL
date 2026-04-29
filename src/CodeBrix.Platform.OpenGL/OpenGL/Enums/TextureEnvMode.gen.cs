// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "TextureEnvMode")]
public enum TextureEnvMode : int
{
    [NativeName("Name", "GL_BLEND")]
    Blend = 0xBE2,
    [NativeName("Name", "GL_REPLACE")]
    Replace = 0x1E01,
    [NativeName("Name", "GL_REPLACE_EXT")]
    ReplaceExt = 0x8062,
    [NativeName("Name", "GL_TEXTURE_ENV_BIAS_SGIX")]
    TextureEnvBiasSgix = 0x80BE,
}
