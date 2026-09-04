# CodeBrix.Platform.OpenGL

A fully managed, cross-platform OpenGL bindings library for .NET 10. CodeBrix.Platform.OpenGL exposes the OpenGL Core-profile API to managed code through function-pointer P/Invoke dispatch, with the native-library resolution and the vector / matrix / scalar maths types that the OpenGL signatures need.

CodeBrix.Platform.OpenGL is provided as a .NET 10 library and associated `CodeBrix.Platform.OpenGL.MitLicenseForever` NuGet package.

CodeBrix.Platform.OpenGL supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## Installation

```
dotnet add package CodeBrix.Platform.OpenGL.MitLicenseForever
```

Note that the NuGet package ID and the namespace are different - there is no package named plain `CodeBrix.Platform.OpenGL`:

* NuGet package ID: `CodeBrix.Platform.OpenGL.MitLicenseForever`
* Assembly and primary namespace: `CodeBrix.Platform.OpenGL` - i.e. `using CodeBrix.Platform.OpenGL;`

XML documentation (IntelliSense) ships alongside the assembly.

The package pulls in the following automatically; no version pinning is needed in the consuming project:

* `Microsoft.DotNet.PlatformAbstractions`
* `Microsoft.Extensions.DependencyModel`

Both are used by the native-library resolver; the package has no other dependencies, and it does not bring a source generator into your build.

## CodeBrix.Platform.OpenGL supports:

* The full OpenGL Core-profile binding surface
* Function-pointer-based P/Invoke dispatch (`delegate* unmanaged[Stdcall|Cdecl]<...>`) for zero-allocation native calls
* Runtime calling-convention selection (Stdcall on Windows, Cdecl elsewhere)
* Cross-platform native-library resolution, so the same code loads `opengl32`, `libGL` or the platform equivalent
* Generic vector / matrix / scalar maths types used throughout the OpenGL signatures
* .NET 10-native `System.Half` support
* No build-time code generation: the P/Invoke method bodies ship as ordinary committed C# source, so nothing is generated while your project builds

## Sample Code

### Create a GL instance against an IGLContext

```csharp
using CodeBrix.Platform.OpenGL;
using CodeBrix.Platform.OpenGL.Core.Contexts;

IGLContext context = /* obtained from GLFW / SDL / your windowing layer */;
GL gl = GL.GetApi(context);
gl.ClearColor(0.2f, 0.3f, 0.4f, 1.0f);
gl.Clear(ClearBufferMask.ColorBufferBit);
```

`GL.GetApi` also accepts an `IGLContextSource`, an `INativeContext`, or a plain `Func<string, nint>` address loader, so it fits whatever windowing layer supplies the context.

## Documentation

The NuGet package includes `AGENT-README.txt`, a complete API reference and usage guide written for AI coding agents - point your agent at that file when it is writing code against this library.

Additional sample code and usage examples are available in the `CodeBrix.Platform.OpenGL.Tests` project:
https://github.com/ellisnet/CodeBrix.Platform.OpenGL/tree/main/tests/CodeBrix.Platform.OpenGL.Tests

## License

CodeBrix.Platform.OpenGL is licensed under the MIT License - see the
[LICENSE](https://github.com/ellisnet/CodeBrix.Platform.OpenGL/blob/main/LICENSE) file.

For licensing and provenance information about the open source code included in
this package, see [THIRD-PARTY-NOTICES.txt](https://github.com/ellisnet/CodeBrix.Platform.OpenGL/blob/main/THIRD-PARTY-NOTICES.txt).
