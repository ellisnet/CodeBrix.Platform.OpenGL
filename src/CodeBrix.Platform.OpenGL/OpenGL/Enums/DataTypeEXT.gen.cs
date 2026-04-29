// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "DataTypeEXT")]
public enum DataTypeEXT : int
{
    [NativeName("Name", "GL_SCALAR_EXT")]
    ScalarExt = 0x87BE,
    [NativeName("Name", "GL_VECTOR_EXT")]
    VectorExt = 0x87BF,
    [NativeName("Name", "GL_MATRIX_EXT")]
    MatrixExt = 0x87C0,
}
