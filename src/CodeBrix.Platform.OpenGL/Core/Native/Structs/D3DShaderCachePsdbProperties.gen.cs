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

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
[NativeName("Name", "D3D_SHADER_CACHE_PSDB_PROPERTIES")]
public unsafe partial struct D3DShaderCachePsdbProperties
{
    public D3DShaderCachePsdbProperties
    (
        char* pAdapterFamily = null,
        char* pPsdbPath = null
    ) : this()
    {
        if (pAdapterFamily is not null)
        {
            PAdapterFamily = pAdapterFamily;
        }

        if (pPsdbPath is not null)
        {
            PPsdbPath = pPsdbPath;
        }
    }


    [NativeName("Type", "const wchar_t *")]
    [NativeName("Type.Name", "const wchar_t *")]
    [NativeName("Name", "pAdapterFamily")]
    public char* PAdapterFamily;

    [NativeName("Type", "const wchar_t *")]
    [NativeName("Type.Name", "const wchar_t *")]
    [NativeName("Name", "pPsdbPath")]
    public char* PPsdbPath;
}
