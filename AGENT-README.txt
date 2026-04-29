================================================================================
AGENT-README: CodeBrix.Platform.OpenGL
A Comprehensive Guide for AI Coding Agents
================================================================================

OVERVIEW
--------
CodeBrix.Platform.OpenGL is a fully managed, cross-platform OpenGL bindings
library for .NET 10. It is a port of the Silk.NET.OpenGL v2.23.0 NuGet
package (and its immediate dependencies, Silk.NET.Core and Silk.NET.Maths),
adapted to target net10.0 exclusively and to operate without the
Silk.NET.SilkTouch Roslyn source generator at build time. The P/Invoke
method bodies that SilkTouch emits at build time in the upstream project
are pre-captured and committed into this repository as static source.

This repository is licensed under MIT. Silk.NET is licensed under MIT.
See THIRD-PARTY-NOTICES.txt for upstream attribution and the full list of
ported files.

PORT STATUS
-----------
Silk.NET.Core, Silk.NET.Maths, and Silk.NET.OpenGL (including the
SilkTouch-captured method-body file) are all ported and the library
builds cleanly on net10.0 in both Debug and Release. Upstream
Silk.NET.Core.Tests and Silk.NET.Maths.Tests have been ported and
migrated to xUnit v3. Silk.NET.OpenGL has no upstream xUnit project —
coverage comes from a handful of CodeBrix smoke tests that exercise
types, enum values, and type-hierarchy without a live GL context (real
GL calls require a GL context and are out of scope for the test suite
on a headless host).

Out of scope for this v1 fork:
  - Silk.NET.OpenGL.Extensions.* (ARB, EXT, KHR, AMD, INTEL, MESA, NV, ...)
  - Silk.NET.OpenGLES
  - Silk.NET.OpenGL.Legacy
  - Silk.NET.WGL

INSTALLATION
------------
NuGet package name:  CodeBrix.Platform.OpenGL.MitLicenseForever

    dotnet add package CodeBrix.Platform.OpenGL.MitLicenseForever

Target framework:    net10.0 (and later)
Root namespace:      CodeBrix.Platform.OpenGL
                     (NuGet PackageId uses the .MitLicenseForever suffix;
                     the namespace does NOT include the suffix.)

KEY NAMESPACES
--------------
The port preserves the three upstream sub-libraries as sub-namespaces:

    using CodeBrix.Platform.OpenGL;           // main GL class + top-level types
    using CodeBrix.Platform.OpenGL.Core;      // was Silk.NET.Core
    using CodeBrix.Platform.OpenGL.Maths;     // was Silk.NET.Maths

Further sub-namespaces (Core.Native, Core.Loader, Core.Contexts, Core.Attributes,
OpenGL.Enums, OpenGL.Structs, etc.) mirror the upstream layout 1:1.

CORE API REFERENCE
------------------
The public API shape is identical to Silk.NET.OpenGL v2.23.0 at the
method-signature level, with only namespace renames (`Silk.NET.OpenGL.*`
-> `CodeBrix.Platform.OpenGL.*`, `Silk.NET.Core.*` -> `CodeBrix.Platform.OpenGL.Core.*`,
`Silk.NET.Maths.*` -> `CodeBrix.Platform.OpenGL.Maths.*`). Consult the
upstream Silk.NET.OpenGL documentation at
https://dotnet.github.io/Silk.NET/ for method-level reference — every
`GL.<MethodName>` entry point there exists on this library's `GL` class
with the same signature.

Main entry-point class:

    CodeBrix.Platform.OpenGL.GL
        -- Inherits from CodeBrix.Platform.OpenGL.Core.Native.NativeAPI.
        -- Constructor: `public GL(INativeContext ctx)`.
        -- A `public partial` method entry point exists for every OpenGL
           core-profile entry point. Bodies are in GL.gen.bodies.cs.
        -- Typical setup:
               IGLContext windowingContext = /* your windowing library */;
               var ctx = new DefaultNativeContext("opengl32");  // or libGL.so
               var gl = new GL(ctx);
               gl.ClearColor(0.2f, 0.3f, 0.4f, 1.0f);

Key types:

    CodeBrix.Platform.OpenGL.GLEnum            -- generic numeric OpenGL enum
    CodeBrix.Platform.OpenGL.PrimitiveType     -- GL_TRIANGLES, GL_LINES, ...
    CodeBrix.Platform.OpenGL.DrawElementsType  -- GL_UNSIGNED_INT, ...
    CodeBrix.Platform.OpenGL.ClearBufferMask   -- GL_COLOR_BUFFER_BIT, ...
    CodeBrix.Platform.OpenGL.Core.Contexts.INativeContext
    CodeBrix.Platform.OpenGL.Core.Native.NativeAPI
    CodeBrix.Platform.OpenGL.Core.Native.SilkMarshal

Port-provenance marker (available at runtime):

    CodeBrix.Platform.OpenGL.PortStatus.UpstreamVersion  -- "Silk.NET v2.23.0"
    CodeBrix.Platform.OpenGL.PortStatus.UpstreamCommit   -- upstream git sha
    CodeBrix.Platform.OpenGL.PortStatus.PortedSoFar      -- human-readable status

CODING CONVENTIONS (CodeBrix family)
-------------------------------------
- Target framework: net10.0 only. No multi-targeting.
- Nullable reference types: OFF (project default). Do NOT use `?` on
  reference types or the null-forgiveness `!` operator. Value-type
  `Nullable<T>` (int?, DateOnly?, enum?) is fine.
- Global usings: OFF. Every file declares its own usings.
- File-scoped namespaces only. No `namespace X { ... }` block-scoped form.
- InternalsVisibleTo: granted only to CodeBrix.Platform.OpenGL.Tests.
- Unsafe blocks: ENABLED for this library (required by OpenGL pointer APIs).
- XML doc generation: ENABLED with CS1591 suppressed for this repo (a
  per-repo exception granted because the ported Silk.NET surface is
  substantial and largely undocumented upstream). Do NOT suppress other
  warnings project-wide.
- Testing: xUnit v3 + SilverAssertions. Ported upstream tests preserve
  upstream style (PascalCase method names, raw Assert.* calls) for
  fidelity — new tests added on top of the port follow the CodeBrix
  conventions in `MemberName_snake_case` form with SilverAssertions fluent
  assertions.

ARCHITECTURE
------------
Top-level directory layout under src/CodeBrix.Platform.OpenGL/:

    OpenGL/       — was Silk.NET.OpenGL
    Core/         — was Silk.NET.Core
    Maths/        — was Silk.NET.Maths

Every ported .cs file carries a `//was previously:` comment on its
namespace line so agents can trace back to upstream. Top-of-file copyright
headers from upstream are preserved verbatim.

The single SilkTouch-generated file (containing the captured method
bodies and the GeneratedVTable class) is committed into
`OpenGL/GL.gen.bodies.cs`.

TESTING
-------
Test project: tests/CodeBrix.Platform.OpenGL.Tests

    dotnet test CodeBrix.Platform.OpenGL.slnx

Upstream tests are ported from Silk.NET.Core.Tests and Silk.NET.Maths.Tests.
Upstream did not ship a Silk.NET.OpenGL.Tests project; integration tests
for actual OpenGL calls require a GL context (not available on
headless Linux CI) and are marked `[Fact(Skip = "requires GL context")]`
where present.

================================================================================
END OF AGENT-README
================================================================================
