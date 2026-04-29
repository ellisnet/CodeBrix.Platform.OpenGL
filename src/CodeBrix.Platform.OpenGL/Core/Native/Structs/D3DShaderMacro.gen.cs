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

namespace CodeBrix.Platform.OpenGL.Core.Native; //was previously: Silk.NET.Core.Native;

[NativeName("Name", "_D3D_SHADER_MACRO")]
public unsafe partial struct D3DShaderMacro
{
    public D3DShaderMacro
    (
        byte* name = null,
        byte* definition = null
    ) : this()
    {
        if (name is not null)
        {
            Name = name;
        }

        if (definition is not null)
        {
            Definition = definition;
        }
    }


    [NativeName("Type", "LPCSTR")]
    [NativeName("Type.Name", "LPCSTR")]
    [NativeName("Name", "Name")]
    public byte* Name;

    [NativeName("Type", "LPCSTR")]
    [NativeName("Type.Name", "LPCSTR")]
    [NativeName("Name", "Definition")]
    public byte* Definition;
}
