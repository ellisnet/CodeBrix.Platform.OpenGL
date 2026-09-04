================================================================================
MAINTAINER-README: CodeBrix.Platform.OpenGL
Notes for people and agents MAINTAINING this repository — not for package consumers
================================================================================

If you are CONSUMING the NuGet package, read AGENT-README.txt instead. This
file is about the repository itself: how it is laid out, how it builds, how
its tests run, how it is packed and published, and where its source came
from.


PURPOSE AND SCOPE
=================

This repository produces exactly ONE NuGet package from ONE project.

    Package id      CodeBrix.Platform.OpenGL.MitLicenseForever
    Assembly        CodeBrix.Platform.OpenGL.dll
    Project         src/CodeBrix.Platform.OpenGL/CodeBrix.Platform.OpenGL.csproj
    Consumer doc    AGENT-README.txt (repo root) -- covers this one package
    License         MIT (PackageLicenseExpression), LICENSE at the repo root

The library is a fully managed, cross-platform OpenGL binding for .NET 10:
the complete OpenGL core-profile entry-point surface on a single GL class,
dispatched through lazily resolved unmanaged function pointers, plus the
native-loading / marshalling layer (Core) and the generic vector and matrix
maths types (Maths) that the bindings depend on. Three upstream libraries
are merged into this one assembly and re-namespaced.

The defining constraint of the port: it builds WITHOUT the upstream Roslyn
source generator. The P/Invoke method bodies the generator emits at build
time upstream are pre-captured and committed here as ordinary static source.

There are no other packable projects, no sample apps and no tool projects in
this repository. See EXTRAS-README.txt.

Out of scope for this fork -- deliberately not ported, do not add them
without a decision to widen the scope:

    the OpenGL extension bindings (ARB, EXT, KHR, AMD, INTEL, MESA, NV, ...)
    OpenGL ES
    the legacy / compatibility-profile OpenGL bindings
    the Windows WGL bindings


REPOSITORY LAYOUT
=================

Root files
----------
    CodeBrix.Platform.OpenGL.slnx  solution; carries a "Solution Items"
                                   folder (.gitignore, AGENT-README.txt,
                                   EXTRAS-README.txt, global.json,
                                   icon-codebrix-128.png, LICENSE,
                                   MAINTAINER-README.txt, README-INDEX.txt,
                                   README.md, THIRD-PARTY-NOTICES.txt) and a
                                   "Tests" folder holding the test project
    AGENT-README.txt               consumer documentation; PACKED into the
                                   nupkg (see PACKAGING AND PUBLISHING)
    MAINTAINER-README.txt          this file
    EXTRAS-README.txt              non-package content inventory
    README-INDEX.txt               map of the README files here
    README.md                      human-facing overview; packed as the
                                   nuspec readme
    LICENSE                        MIT
    THIRD-PARTY-NOTICES.txt        upstream attribution and the list of
                                   ported files; packed
    icon-codebrix-128.png          package icon; packed
    global.json                    selects the Microsoft.Testing.Platform
                                   test runner; pins no SDK version

Source layout (src/CodeBrix.Platform.OpenGL/) -- the three upstream
libraries are kept as three top-level folders and three namespaces:

    OpenGL/       ns CodeBrix.Platform.OpenGL       (was the upstream OpenGL
                  binding library) the GL class, the GLOverloads extension
                  class, every OpenGL enum, the GL handle structs, DebugProc,
                  PortStatus, ContextSourceExtensions.
      Enums/      source folder only -- the enums live directly in the
                  CodeBrix.Platform.OpenGL namespace, NOT in a ".Enums"
                  namespace.
      Structs/    source folder only -- likewise, the handle structs live
                  directly in CodeBrix.Platform.OpenGL.
    Core/         ns CodeBrix.Platform.OpenGL.Core  (was the upstream core
                  library) with sub-namespaces Core.Contexts, Core.Loader,
                  Core.Native, Core.Attributes, and a Core/Miscellaneous
                  folder.
    Maths/        ns CodeBrix.Platform.OpenGL.Maths (was the upstream maths
                  library) generic vector / matrix / quaternion / shape
                  structs, the Scalar<T> generic-arithmetic layer (with a
                  Scalar.Bitwise/ sub-folder), and the System.Numerics
                  converters.

    InternalsVisibleTo.cs grants InternalsVisibleTo to
                          "CodeBrix.Platform.OpenGL.Tests".

Roughly 512 source files. Four of them are machine-generated and enormous;
know them before you go editing:

    OpenGL/GL.gen.bodies.cs    ~151,600 lines. The pre-captured generated
                               method bodies plus the private sealed
                               GeneratedVTable class. This is the file that
                               replaces the upstream build-time source
                               generator. Do not hand-edit; regenerate.
    OpenGL/GL.gen.cs           ~23,000 lines. The partial-method
                               declarations for every entry point, and the
                               GL(INativeContext) constructor.
    OpenGL/GLOverloads.gen.cs  ~12,500 lines. The span / array / out
                               convenience overloads.
    OpenGL/Enums/GLEnum.gen.cs ~2,800 lines. The catch-all numeric enum.

Test layout (tests/CodeBrix.Platform.OpenGL.Tests/):

    PortStatusTests.cs    the provenance marker
    Core/                 TestSilkMarshal.cs -- the string and pointer
                          marshalling helpers
    Maths/                the ported upstream maths suite: Vector2/3/4,
                          Matrix4x4, Quaternion, Plane, Scalar, Scalar
                          bitwise, Exp / Log / integer-Pow accuracy, plus a
                          MathHelper.cs helper
    OpenGL/               GLSmokeTests.cs -- type hierarchy and enum spec
                          values, no GL calls


BUILDING
========

    dotnet restore CodeBrix.Platform.OpenGL.slnx
    dotnet build   CodeBrix.Platform.OpenGL.slnx

The library builds cleanly on net10.0 in both Debug and Release, with no
source generator in the build. Expect the build to be slow relative to the
repository's apparent size: GL.gen.bodies.cs alone is over 150,000 lines.

Project facts recorded in
src/CodeBrix.Platform.OpenGL/CodeBrix.Platform.OpenGL.csproj:

  * TargetFramework net10.0 only. No multi-targeting.
  * Nullable reference types are ENABLED, as a per-repo exception to the
    CodeBrix family convention of NRT-off. The upstream maths project
    enables NRT and the upstream code depends on "?" annotations
    throughout, so preserving upstream fidelity requires NRT-on here. New,
    non-ported CodeBrix code added to this repository should still avoid "?"
    annotations where it can.
  * AllowUnsafeBlocks is ON -- required by the pointer-taking OpenGL entry
    points.
  * GenerateDocumentationFile is TRUE, with NoWarn 1591 (missing XML doc)
    for this repository only, by explicit per-repo exception. The ported
    upstream surface -- about 3,680 entry points plus the Core and Maths
    types -- is largely undocumented upstream, and retrofitting doc comments
    on every ported member is incompatible with fidelity. No other warning
    is suppressed project-wide; do not add one.
  * DefineConstants adds BTEC_INTRINSICS, MATHF and POH. These are carried
    over from the upstream maths project's constants for its old target
    framework, and gate code inside the Maths area only. They do not affect
    Core or the GL bindings.
  * SSE and AdvSIMD are deliberately NOT defined -- read this before you
    "re-enable hardware acceleration". Defining them activates the upstream
    hand-rolled intrinsics dispatch, which was observed to return WRONG
    RESULTS on net10.0 Release builds for the Log and Exp operations
    (Log(1) came back as negative infinity instead of 0; Exp(1) as 1.17
    instead of 2.718). With them off, Maths takes the scalar path, which is
    correct and covered by the ExpTests / LogTests suites. The JIT still
    emits SIMD for hot paths through the standard Vector<T> and
    System.Runtime.Intrinsics APIs, so vector and matrix work is still
    hardware-accelerated via the BCL.
  * A .nupkg is produced on every build (GeneratePackageOnBuild is true), so
    an ordinary `dotnet build` also packs.

Runtime NuGet dependencies, both carried over from the upstream core
library and used by the native-library path resolver:

    Microsoft.DotNet.PlatformAbstractions
    Microsoft.Extensions.DependencyModel

Upstream dependencies deliberately DROPPED, and why: the BCL half-precision,
System.Memory, System.Runtime.CompilerServices.Unsafe, System.Numerics.Vectors
and hash-code compatibility packages, because net10.0 provides all of them
natively; and the public-API analyzer package, because this fork does not
maintain public-API-diff discipline. Do not reinstate them.


TESTING
=======

    dotnet test CodeBrix.Platform.OpenGL.slnx

THE TEST RUNNER IS Microsoft.Testing.Platform, selected by global.json at the
repository root. That file does NOT pin an SDK version, so the newest
installed .NET 10 SDK is still used; it exists solely to select the runner:

    { "test": { "runner": "Microsoft.Testing.Platform" } }

Because the setting lives in global.json rather than in the test csproj, it
applies to every `dotnet test` run anywhere in the repository. Keep the file
committed -- without it `dotnet test` silently falls back to the older VSTest
bridge.

Test project: tests/CodeBrix.Platform.OpenGL.Tests/, targeting net10.0 with
AllowUnsafeBlocks ON. Framework is xUnit v3 plus SilverAssertions, with no
coverage collector. There is no xunit.runner.json and no special runner
configuration; collections run in parallel by default. There are no opt-in
environment variables and no special prep.

What the suite does and does not cover
--------------------------------------
The upstream core and maths test projects were ported and migrated to xUnit
v3; they are the bulk of the suite and they are real coverage. Upstream
shipped NO test project for the OpenGL binding itself, so GL coverage is a
handful of CodeBrix smoke tests over the type hierarchy and the enum spec
values.

NO TEST ISSUES A REAL GL CALL. Every entry point needs a live OpenGL context
current on the calling thread, and the test host is headless. Do not "fix"
this by adding tests that construct a GL over a stub context and call
through it -- a missing symbol surfaces as a load failure at the first call
of that method, and a fabricated context proves nothing about the binding.
Verification of actual rendering belongs in a consuming application with a
window, not here.

Because the Maths intrinsics defines are off (see BUILDING), the
Exp / Log / Pow accuracy suites are the guard on that decision. If someone
turns SSE or AdvSIMD on, those suites are what will fail -- treat a failure
there as "the define came back", not as a tolerance to loosen.


PACKAGING AND PUBLISHING
========================

There is no pack script and no CI workflow. Packing happens as a side effect
of building, because the library csproj sets GeneratePackageOnBuild=true;
`dotnet pack` on the same project produces the same nupkg.

What ships in the nupkg, beyond the assembly (all four are declared as
<None ... Pack="true" PackagePath=""> in the library csproj, pulled from the
repository root two levels up):

    icon-codebrix-128.png     PackageIcon
    README.md                 PackageReadmeFile
    AGENT-README.txt          the consumer guide -- this is the ONLY
                              README-family file that ships; keep it
                              accurate, because agents read it out of the
                              extracted package
    THIRD-PARTY-NOTICES.txt   upstream attribution

MAINTAINER-README.txt, EXTRAS-README.txt and README-INDEX.txt are
repository-only and are NOT packed.

The package ships no native binaries and declares no runtime identifiers.
The consumer supplies the OpenGL implementation and the context.

Versioning
----------
Date-stamped and auto-incrementing, computed in the csproj from
System.DateTime.UtcNow, in the form 1.<x>.<y>.<z>:

    1   major       pinned to 1 for this library
    x   minor       whole years since $(_VersionBaseYear) (2026 => 0)
    y   build       UTC day of year, 1-based (Jan 1 = 1)
    z   revision    UTC minute of day, 0..1439

Version, AssemblyVersion and FileVersion all take that value. Consequences
worth remembering:

  * Every build produces a NEW version, and with GeneratePackageOnBuild that
    means a fresh .nupkg on every build.
  * Two builds within the SAME UTC minute produce the SAME version. Do not
    publish two packages from within one minute.
  * This is not SemVer: minor encodes the year and major is pinned, so
    major/minor say nothing about API compatibility.
  * To re-baseline the minor number, change _VersionBaseYear in the csproj.

Publishing is manual: build, then push the produced .nupkg to nuget.org.
Tag the repository to match the published package version.


PROVENANCE AND VENDORED SOURCES
===============================

CodeBrix.Platform.OpenGL is a port of Silk.NET.OpenGL v2.23.0 together with
its Silk.NET.Core and Silk.NET.Maths dependencies (upstream:
https://github.com/dotnet/Silk.NET, MIT, tag v2.23.0, commit
94605142f7b7bd6e69c9201e8e721d245c69eb7e). Namespaces were renamed
Silk.NET.OpenGL -> CodeBrix.Platform.OpenGL, Silk.NET.Core ->
CodeBrix.Platform.OpenGL.Core, Silk.NET.Maths ->
CodeBrix.Platform.OpenGL.Maths. THIRD-PARTY-NOTICES.txt is the authoritative
attribution record and carries the full list of ported files.

Every ported .cs file carries a "//was previously: <upstream namespace>;"
comment on its namespace line, so the upstream origin of any file is
trivially recoverable, and upstream top-of-file copyright headers are
preserved verbatim.

The upstream version and commit are also exposed at run time, which is how a
consumer or a test can assert what this build was ported from:

    public static class PortStatus            // ns CodeBrix.Platform.OpenGL
        static string UpstreamVersion { get; }
        static string UpstreamCommit  { get; }
        static string PortedComponents { get; }

Keep those three strings in step with THIRD-PARTY-NOTICES.txt whenever the
port is refreshed; PortStatusTests.cs asserts on them.

The source-generator decision
-----------------------------
Upstream generates the P/Invoke method bodies with a Roslyn source generator
at build time. This fork captures that output once and commits it as
OpenGL/GL.gen.bodies.cs, so consumers and this repository build with no
generator in the pipeline. That is the whole point of the fork; do not
reintroduce a build-time generator dependency.

Consequence for maintenance: refreshing to a newer upstream version is not a
merge, it is a re-capture. You need the upstream tree, its generator, and a
build that emits the bodies, and then the emitted output is re-namespaced
and committed in place of the current .gen files. The hand-written files
(GL.cs, ContextSourceExtensions.cs, PortStatus.cs, InternalsVisibleTo.cs)
are the ones you merge by hand.

Maintaining the vendored source
-------------------------------
Treat OpenGL/, Core/ and Maths/ as vendored upstream code. Prefer changes a
future re-port can still be applied over. Never hand-edit the four .gen
files. When you must diverge, comment at the divergence rather than silently
reformatting, and keep the "//was previously:" namespace comments and the
upstream copyright headers intact.


CODING CONVENTIONS
==================

CodeBrix family conventions, as they apply in this repository:

  * Target framework net10.0 only. No multi-targeting.
  * Nullable reference types are ON here, by per-repo exception (see
    BUILDING). This is the opposite of the family default, and it is
    load-bearing for upstream fidelity. New non-ported code should still
    avoid "?" annotations where practical.
  * Global usings are OFF. Every file declares its own usings.
  * File-scoped namespaces only. No block-scoped "namespace X { ... }".
  * InternalsVisibleTo is granted only to CodeBrix.Platform.OpenGL.Tests.
  * Unsafe blocks are ENABLED for this library, as the OpenGL pointer APIs
    require.
  * XML doc generation is ON with CS1591 suppressed for this repository
    only, by per-repo exception. Do not suppress any other warning
    project-wide.
  * Folder names are not namespace names here. OpenGL/Enums/ and
    OpenGL/Structs/ are organisational folders whose types live directly in
    the CodeBrix.Platform.OpenGL namespace. Keep it that way -- the consumer
    documentation states it as a rule.

Test conventions:

  * xUnit v3 plus SilverAssertions.
  * Ported upstream tests preserve upstream style (PascalCase method names,
    raw Assert.* calls) for fidelity. Do not mass-rewrite them.
  * NEW tests written on top of the port follow the CodeBrix conventions:
    <Member>_snake_case names with SilverAssertions fluent assertions.
  * Any call inside a test that accepts a CancellationToken passes
    TestContext.Current.CancellationToken (xUnit1051).


NOTES
=====

  * Nothing in this repository requires a GPU, a display or network access
    to build or test. The suite runs on a headless Linux host.
  * The AI-agent pointer files at the repository root and under .cursor/,
    .github/, .junie/ are stubs that route an agent to AGENT-README.txt.
    They are maintained centrally across the CodeBrix family -- do not
    hand-edit them here.
  * The public surface is large (roughly 490 public types, and about 3,680
    GL entry points). When you add or rename hand-written public API, update
    AGENT-README.txt in the same change; it is what ships to consumers
    inside the package.
