// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

namespace CodeBrix.Platform.OpenGL.Core.Contexts; //was previously: Silk.NET.Core.Contexts;

public interface IGLContextSource
{
    IGLContext? GLContext { get; }
}
