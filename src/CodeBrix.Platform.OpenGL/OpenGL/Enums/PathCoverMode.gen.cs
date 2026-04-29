// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "PathCoverMode")]
public enum PathCoverMode : int
{
    [NativeName("Name", "GL_PATH_FILL_COVER_MODE_NV")]
    PathFillCoverModeNV = 0x9082,
    [NativeName("Name", "GL_CONVEX_HULL_NV")]
    ConvexHullNV = 0x908B,
    [NativeName("Name", "GL_BOUNDING_BOX_NV")]
    BoundingBoxNV = 0x908D,
}
