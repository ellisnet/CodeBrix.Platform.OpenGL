================================================================================
EXTRAS-README: CodeBrix.Platform.OpenGL
Samples, tools and other content in this repository that is not part of a NuGet package
================================================================================

This repository contains NO sample applications, demo projects, benchmark
projects or tool projects. It holds exactly two projects: the packable
library and its test project. There is no samples/ folder, no tools/ folder,
and no build or codegen scripts.

Note in particular that the code generator is NOT here. The pre-captured
generated method bodies are committed as ordinary source inside the library
(OpenGL/GL.gen.bodies.cs); the generator that produced them lives upstream.
See PROVENANCE AND VENDORED SOURCES in MAINTAINER-README.txt for what
refreshing that file involves.


TEST PROJECT
============

    tests/CodeBrix.Platform.OpenGL.Tests/

The only non-package content in the repository. It is not packed and is not
published; it exists to verify the library. Run it with:

    dotnet test CodeBrix.Platform.OpenGL.slnx

It is also the worked-example corpus for everything in the library that can
be exercised without a GPU. AGENT-README.txt points consumers at individual
test files on GitHub as the canonical usage examples:

    Core/TestSilkMarshal.cs   string and pointer marshalling in every
                              supported native string encoding, and string
                              arrays to and from native memory
    Maths/                    the vector, matrix, quaternion, plane and
                              Scalar<T> suites -- these are usable as
                              worked examples of the generic maths API
    OpenGL/GLSmokeTests.cs    GL type hierarchy and enum spec values
    PortStatusTests.cs        the run-time port-provenance strings

What it deliberately does NOT contain is a running OpenGL demo. No test
issues a real GL call, because every entry point needs a live context
current on the calling thread and the test host is headless. If you want to
see the bindings actually draw something, build a small consuming
application with a windowing library -- the AGENT-README's COMPLETE EXAMPLES
section carries a triangle program you can start from. Do not add such a
demo to this repository without a decision to take on a windowing
dependency.

There are no opt-in environment variables and no optional test-data sets in
this repository.
