// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "ListParameterName")]
public enum ListParameterName : int
{
    [NativeName("Name", "GL_LIST_PRIORITY_SGIX")]
    ListPrioritySgix = 0x8182,
}
