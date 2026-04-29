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

[StructLayout(LayoutKind.Explicit)]
[NativeName("Name", "D3D_VERSION_NUMBER")]
public unsafe partial struct D3DVersionNumber
{
    public D3DVersionNumber
    (
        ulong? version = null
    ) : this()
    {
        if (version is not null)
        {
            Version = version.Value;
        }
    }


    [FieldOffset(0)]
    [NativeName("Type", "UINT64")]
    [NativeName("Type.Name", "UINT64")]
    [NativeName("Name", "Version")]
    public ulong Version;
    [FieldOffset(0)]
    [NativeName("Type", "UINT16[4]")]
    [NativeName("Type.Name", "UINT16[4]")]
    [NativeName("Name", "VersionParts")]
    public fixed ushort VersionParts[4];
}
