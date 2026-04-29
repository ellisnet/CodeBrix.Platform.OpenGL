// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "ConvolutionBorderModeEXT")]
public enum ConvolutionBorderModeEXT : int
{
    [NativeName("Name", "GL_REDUCE")]
    Reduce = 0x8016,
    [NativeName("Name", "GL_REDUCE_EXT")]
    ReduceExt = 0x8016,
}
