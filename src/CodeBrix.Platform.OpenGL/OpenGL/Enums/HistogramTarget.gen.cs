// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "HistogramTarget")]
public enum HistogramTarget : int
{
    [NativeName("Name", "GL_HISTOGRAM")]
    Histogram = 0x8024,
    [NativeName("Name", "GL_PROXY_HISTOGRAM")]
    ProxyHistogram = 0x8025,
}
