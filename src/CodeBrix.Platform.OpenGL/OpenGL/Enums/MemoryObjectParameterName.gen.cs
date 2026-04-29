// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "MemoryObjectParameterName")]
public enum MemoryObjectParameterName : int
{
    [NativeName("Name", "GL_DEDICATED_MEMORY_OBJECT_EXT")]
    DedicatedMemoryObjectExt = 0x9581,
    [NativeName("Name", "GL_PROTECTED_MEMORY_OBJECT_EXT")]
    ProtectedMemoryObjectExt = 0x959B,
}
