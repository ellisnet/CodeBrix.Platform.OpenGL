// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using CodeBrix.Platform.OpenGL.Core.Attributes;

#pragma warning disable 1591

namespace CodeBrix.Platform.OpenGL.Core.Native; //was previously: Silk.NET.Core.Native;

[NativeName("Name", "TrustLevel")]
public enum TrustLevel : int
{
    [NativeName("Name", "BaseTrust")]
    BaseTrust = 0x0,
    [NativeName("Name", "PartialTrust")]
    PartialTrust = 0x1,
    [NativeName("Name", "FullTrust")]
    FullTrust = 0x2,
}
