# CodeBrix.Platform.OpenGL

A fully managed, cross-platform OpenGL bindings library for .NET 10. CodeBrix.Platform.OpenGL is a port of the [Silk.NET.OpenGL](https://github.com/dotnet/Silk.NET) v2.23.0 NuGet package and its immediate dependencies (Silk.NET.Core and Silk.NET.Maths), adapted to target `net10.0` exclusively and to build without the Silk.NET.SilkTouch Roslyn source generator — the P/Invoke method bodies that SilkTouch emits at build time in the upstream project are pre-captured and committed into this repository as static source.

CodeBrix.Platform.OpenGL is provided as a .NET 10 library and associated `CodeBrix.Platform.OpenGL.MitLicenseForever` NuGet package.

CodeBrix.Platform.OpenGL supports applications and assemblies that target Microsoft .NET version 10.0 and later.
Microsoft .NET version 10.0 is a Long-Term Supported (LTS) version of .NET, and was released on Nov 11, 2025; and will be actively supported by Microsoft until Nov 14, 2028.
Please update your C#/.NET code and projects to the latest LTS version of Microsoft .NET.

## CodeBrix.Platform.OpenGL supports:

* The full OpenGL Core-profile surface emitted by the Silk.NET.OpenGL v2.23.0 bindings (~3,680 entry points)
* Function-pointer-based P/Invoke dispatch (`delegate* unmanaged[Stdcall|Cdecl]<...>`) for zero-allocation native calls
* Runtime calling-convention selection (Stdcall on Windows, Cdecl elsewhere)
* Cross-platform native-library resolution via the ported Silk.NET.Core loader
* Porting of the Silk.NET.Maths vector / matrix / scalar types used throughout OpenGL signatures
* .NET 10-native `System.Half` (no `Ultz.Bcl.Half` polyfill dependency)

## Port status

This is a work-in-progress port. See [`AGENT-README.txt`](./AGENT-README.txt) for the current state and [`THIRD-PARTY-NOTICES.txt`](./THIRD-PARTY-NOTICES.txt) for upstream attribution.

## Sample Code

### Create a GL instance against an IGLContext (once the port stabilizes)

```csharp
using CodeBrix.Platform.OpenGL;
using CodeBrix.Platform.OpenGL.Core.Contexts;

IGLContext context = /* obtained from GLFW / SDL / your windowing layer */;
GL gl = GL.GetApi(context);
gl.ClearColor(0.2f, 0.3f, 0.4f, 1.0f);
gl.Clear((uint)ClearBufferMask.ColorBufferBit);
```

## License

The project is licensed under the MIT License. see: https://en.wikipedia.org/wiki/MIT_License
