// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "PrecisionType")]
public enum PrecisionType : int
{
    [NativeName("Name", "GL_LOW_FLOAT")]
    LowFloat = 0x8DF0,
    [NativeName("Name", "GL_MEDIUM_FLOAT")]
    MediumFloat = 0x8DF1,
    [NativeName("Name", "GL_HIGH_FLOAT")]
    HighFloat = 0x8DF2,
    [NativeName("Name", "GL_LOW_INT")]
    LowInt = 0x8DF3,
    [NativeName("Name", "GL_MEDIUM_INT")]
    MediumInt = 0x8DF4,
    [NativeName("Name", "GL_HIGH_INT")]
    HighInt = 0x8DF5,
}
