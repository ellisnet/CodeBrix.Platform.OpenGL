// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "TriangleListSUN")]
public enum TriangleListSUN : int
{
    [NativeName("Name", "GL_RESTART_SUN")]
    RestartSun = 0x1,
    [NativeName("Name", "GL_REPLACE_MIDDLE_SUN")]
    ReplaceMiddleSun = 0x2,
    [NativeName("Name", "GL_REPLACE_OLDEST_SUN")]
    ReplaceOldestSun = 0x3,
}
