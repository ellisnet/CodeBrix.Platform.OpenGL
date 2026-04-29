// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using CodeBrix.Platform.OpenGL.Core;
using CodeBrix.Platform.OpenGL.Core.Native;
using CodeBrix.Platform.OpenGL.Core.Attributes;
using CodeBrix.Platform.OpenGL.Core.Contexts;
using CodeBrix.Platform.OpenGL.Core.Loader;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL; //was previously: Silk.NET.OpenGL;

[NativeName("Name", "framebuffer")]
public unsafe partial struct Framebuffer
{
    public Framebuffer
    (
        uint? handle = null
    ) : this()
    {
        if (handle is not null)
        {
            Handle = handle.Value;
        }
    }


    [NativeName("Type", "")]
    [NativeName("Type.Name", "")]
    [NativeName("Name", "")]
    public uint Handle;
}
