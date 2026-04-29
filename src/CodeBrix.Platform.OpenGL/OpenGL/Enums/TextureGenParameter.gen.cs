// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "TextureGenParameter")]
public enum TextureGenParameter : int
{
    [NativeName("Name", "GL_TEXTURE_GEN_MODE_OES")]
    TextureGenModeOes = 0x2500,
    [NativeName("Name", "GL_EYE_POINT_SGIS")]
    EyePointSgis = 0x81F4,
    [NativeName("Name", "GL_OBJECT_POINT_SGIS")]
    ObjectPointSgis = 0x81F5,
    [NativeName("Name", "GL_EYE_LINE_SGIS")]
    EyeLineSgis = 0x81F6,
    [NativeName("Name", "GL_OBJECT_LINE_SGIS")]
    ObjectLineSgis = 0x81F7,
}
