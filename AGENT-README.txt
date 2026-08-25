================================================================================
AGENT-README: CodeBrix.Platform.OpenGL
A Guide for AI Coding Agents — CONSUMING the CodeBrix.Platform.OpenGL.MitLicenseForever NuGet package
================================================================================

OVERVIEW
========
CodeBrix.Platform.OpenGL is a fully managed, cross-platform OpenGL binding
for .NET 10 or later. It exposes the complete OpenGL core-profile surface
-- roughly 700 distinct native GL functions -- as about 1,590 methods on a
single class, GL, plus about 2,080 Span/array/out convenience overloads on
the GLOverloads static extension class, which is in scope from the same
"using". Every call dispatches through an unmanaged function pointer that is
resolved lazily on first use from the GL context YOU provide. Members that
live on GLOverloads rather than on GL itself are marked "[GLOverloads]"
throughout the reference below; the call syntax is identical either way.
No Roslyn source generator runs in your build; the generated method bodies
ship as ordinary compiled code inside the package.

The package also carries two supporting areas that the bindings depend on
and that you will use directly:

  - CodeBrix.Platform.OpenGL.Core.*   native-library loading, string and
                                      pointer marshalling, GL-context
                                      abstractions, native window handles
  - CodeBrix.Platform.OpenGL.Maths    generic vector / matrix / quaternion /
                                      shape structs (Vector3D<float>,
                                      Matrix4X4<float>, ...) plus converters
                                      to and from System.Numerics

Provenance: this is a port of Silk.NET.OpenGL v2.23.0 together with its
Silk.NET.Core and Silk.NET.Maths dependencies. Namespaces were renamed
(Silk.NET.OpenGL -> CodeBrix.Platform.OpenGL, Silk.NET.Core ->
CodeBrix.Platform.OpenGL.Core, Silk.NET.Maths -> CodeBrix.Platform.OpenGL.Maths).
Do NOT add "using Silk.NET.*" lines and do NOT reference any Silk.NET NuGet
package alongside this one; the upstream namespaces do not exist here.

What you must bring: a window and a current OpenGL context created by some
other library (SDL, GLFW, a platform windowing layer, EGL, ...). This package
never creates windows or contexts; it needs either an IGLContext object or a
"get proc address" delegate from that layer.

INSTALLATION
============
PackageId:  CodeBrix.Platform.OpenGL.MitLicenseForever

    dotnet add package CodeBrix.Platform.OpenGL.MitLicenseForever

License:            MIT (the upstream Silk.NET code is also MIT)
Target framework:   .NET 10 or later (net10.0 assembly; no other TFMs)
NuGet dependencies: Microsoft.DotNet.PlatformAbstractions,
                    Microsoft.Extensions.DependencyModel (both pulled in
                    automatically; used by the native-library path resolver)
Native requirement: an OpenGL implementation on the machine (libGL.so.1 on
                    Linux, the OpenGL framework on macOS, opengl32.dll plus
                    the vendor ICD on Windows) and a current GL context.

Project settings you will want:

    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>

Pointer-taking overloads (void*, byte*, float*) and the DrawElements
"offset into a bound element buffer" idiom require an unsafe context. The
out / ref readonly / Span<T> / string overloads do not, so a purely safe
consumer is possible but awkward for index-buffer drawing.

Package/namespace mismatch: the PackageId ends in .MitLicenseForever; the
namespaces do NOT (they start with CodeBrix.Platform.OpenGL).

KEY NAMESPACES / USINGS
=======================
    using CodeBrix.Platform.OpenGL;                 // GL, GLOverloads, all
                                                    // 344 enums, the 15 GL
                                                    // handle structs,
                                                    // DebugProc, PortStatus,
                                                    // ContextSourceExtensions
    using CodeBrix.Platform.OpenGL.Core;            // Bool32, Bool8, Version32,
                                                    // Version64, RawImage,
                                                    // PfnVoidFunction,
                                                    // PlatformException,
                                                    // BreakneckLock
    using CodeBrix.Platform.OpenGL.Core.Contexts;   // INativeContext,
                                                    // IGLContext,
                                                    // IGLContextSource,
                                                    // LamdaNativeContext,
                                                    // DefaultNativeContext,
                                                    // MultiNativeContext,
                                                    // INativeWindow, ...
    using CodeBrix.Platform.OpenGL.Core.Loader;     // UnmanagedLibrary,
                                                    // LibraryLoader,
                                                    // PathResolver,
                                                    // DefaultPathResolver,
                                                    // SearchPathContainer,
                                                    // SymbolLoadingException
    using CodeBrix.Platform.OpenGL.Core.Native;     // SilkMarshal,
                                                    // GlobalMemory, ComPtr<T>,
                                                    // NativeAPI,
                                                    // NativeExtension<T>,
                                                    // NativeStringEncoding
    using CodeBrix.Platform.OpenGL.Core.Attributes; // NativeName, Count, Flow,
                                                    // Extension, Inject,
                                                    // UnmanagedType attributes
    using CodeBrix.Platform.OpenGL.Maths;           // Vector2D/3D/4D<T>,
                                                    // Matrix4X4<T>, ...

There are exactly these seven namespaces. "Enums" and "Structs" are source
folders only: every OpenGL enum (BufferTargetARB, ShaderType, ...) and every
handle struct (Buffer, Shader, ...) lives directly in CodeBrix.Platform.OpenGL.
Name clashes to expect with a using for CodeBrix.Platform.OpenGL:
System.Buffer (use CodeBrix.Platform.OpenGL.Buffer explicitly),
System.Threading.Tasks.Task-free but System.Drawing.Rectangle vs
CodeBrix.Platform.OpenGL.Maths.Rectangle<T>, and System.Numerics.Quaternion
vs Maths.Quaternion<T> when both namespaces are imported.

CORE API REFERENCE
==================
Public surface: about 490 public types - 344 enums, 15 GL handle structs,
the GL class plus the GLOverloads extension class, 58 Maths types, and
roughly 70 Core types (of which about 30 are Windows-only COM/D3D interop
carried over from upstream Core and irrelevant to OpenGL work). The sections
below cover every feature area and the types you will actually touch.

1. CREATING A GL INSTANCE
-------------------------
    public unsafe partial class GL : NativeAPI                // NativeAPI :
                                                              // NativeApiContainer
                                                              // : IDisposable
    public GL(INativeContext ctx)
    public static GL GetApi(IGLContextSource contextSource)   // throws
                              // InvalidOperationException if .GLContext is null
    public static GL GetApi(IGLContext ctx)
    public static GL GetApi(Func<string, nint> getProcAddress) // wraps in
                                                               // LamdaNativeContext
    public static GL GetApi(INativeContext ctx)
    public static INativeContext CreateDefaultContext(string[] n)
                              // tries each library name via
                              // DefaultNativeContext.TryCreate; throws
                              // System.IO.FileNotFoundException if none load
    public static GL CreateOpenGL(this IGLContextSource src)  // extension in
                                                              // ContextSourceExtensions
    public INativeContext Context { get; }                    // from NativeAPI
    public void Dispose()          // disposes the INativeContext AND the
                                   // function-pointer table
    public void PurgeEntryPoints() // forget every resolved pointer; they are
                                   // re-resolved on next call (use after
                                   // switching to a different context)
    public IVTable CurrentVTable { get; }
    public bool TryGetExtension<T>(out T ext) where T : NativeExtension<GL>
    public override bool IsExtensionPresent(string extension)
                                   // enumerates GL_NUM_EXTENSIONS /
                                   // glGetStringi once and caches; "GL_"
                                   // prefix optional

The context abstraction (CodeBrix.Platform.OpenGL.Core.Contexts):

    public interface INativeContext : IDisposable
    {
        nint GetProcAddress(string proc, int? slot = default);
        bool TryGetProcAddress(string proc, out nint addr, int? slot = default);
    }
    public interface IGLContext : INativeContext, IDisposable
    {
        nint Handle { get; }
        IGLContextSource? Source { get; }
        bool IsCurrent { get; }
        void SwapInterval(int interval);
        void SwapBuffers();
        void MakeCurrent();
        void Clear();
    }
    public interface IGLContextSource { IGLContext? GLContext { get; } }

    public struct LamdaNativeContext : INativeContext
    {
        public delegate bool TryLoader(string proc, out nint pfn);
        public LamdaNativeContext(Func<string, nint> getProcAddress);
        public LamdaNativeContext(TryLoader getProcAddress);
        // GetProcAddress throws SymbolLoadingException when the delegate
        // returns 0; TryGetProcAddress returns false instead.
    }
    public class DefaultNativeContext : INativeContext
    {
        public static bool TryCreate(string name, out DefaultNativeContext context);
        public DefaultNativeContext(UnmanagedLibrary library);
        public DefaultNativeContext(string name);
        public DefaultNativeContext(string[] names);
        public DefaultNativeContext(string name, LibraryLoader loader);
        public DefaultNativeContext(string[] names, LibraryLoader loader);
        public DefaultNativeContext(string name, LibraryLoader loader, PathResolver pathResolver);
        public DefaultNativeContext(string[] names, LibraryLoader loader, PathResolver pathResolver);
        public UnmanagedLibrary Library { get; }
    }
    public class MultiNativeContext : INativeContext
    {
        public INativeContext?[] Contexts { get; set; }
        public MultiNativeContext(params INativeContext?[] contexts);
        // TryGetProcAddress asks each context in order; first non-zero wins.
    }

Which factory to use:

  - You have a windowing library that hands out a proc-address function
    (SDL_GL_GetProcAddress, glfwGetProcAddress, glXGetProcAddress,
    eglGetProcAddress, wglGetProcAddress): use
    GL.GetApi(Func<string, nint>). This is the recommended route; the
    windowing layer already knows the driver and the current context.
  - You have an object implementing IGLContext / IGLContextSource: use
    GL.GetApi(IGLContext) or GL.GetApi(IGLContextSource) / CreateOpenGL().
  - You want the bindings to dlopen the GL library themselves:
    DefaultNativeContext(string) / GL.CreateDefaultContext(string[]). This
    resolves entry points ONLY by exported-symbol lookup on the library
    handle (NativeLibrary.TryGetExport under the hood); it never calls
    glXGetProcAddress / wglGetProcAddress / eglGetProcAddress. On Windows,
    opengl32.dll exports only OpenGL 1.1, so CreateShader, GenBuffers, etc.
    throw SymbolLoadingException through this route. Combine with
    MultiNativeContext when you need both (see example 2 below).

Library names per OS (you pass them; the library does not choose for you):

    Linux    "libGL.so.1"        (DefaultPathResolver also tries "libGL.so",
                                  i.e. the name with trailing version parts
                                  removed)
    macOS    "/System/Library/Frameworks/OpenGL.framework/OpenGL"
             (a plain "libGL.dylib" is also tried with version suffixes
              stripped, but the framework path is what ships on macOS)
    Windows  "opengl32.dll"

How a plain name is resolved: UnmanagedLibrary(name) uses
LibraryLoader.GetPlatformDefaultLoader() (a System.Runtime.InteropServices.
NativeLibrary-based loader) and PathResolver.Default, a DefaultPathResolver
whose Resolvers list expands the name into candidates in this order:
PassthroughResolver (the name itself), LinuxVersioningResolver ("libX.so.1.2"
-> "libX.so.1" -> "libX.so"), MacVersioningResolver, BaseDirectoryResolver
(AppContext.BaseDirectory), MainModuleDirectoryResolver, RuntimesFolderResolver
(runtimes/<rid>/native), NativePackageResolver (from the app's .deps.json),
SilkDirectoryResolver (beside this assembly). The first candidate that loads
wins.

How entry points are resolved: every GL method reads a cached nint for its
"glXxx" symbol; on first use it calls INativeContext.GetProcAddress("glXxx")
and caches the result for the life of the GL instance. A missing symbol
surfaces as SymbolLoadingException at the FIRST CALL of that method, not at
construction. Calling convention is chosen at runtime: Stdcall on Windows
(SilkMarshal.IsWinapiStdcall == true), Cdecl elsewhere.

2. ENTRY-POINT SHAPES: HOW TO READ THE OVERLOADS
------------------------------------------------
Every OpenGL function appears as a family of overloads on GL (instance
methods) plus, for Span-taking forms, static extension methods in
GLOverloads (in scope with "using CodeBrix.Platform.OpenGL;"). The pattern:

  - Typed enum vs GLEnum: each enum parameter has a twin overload taking the
    catch-all GLEnum, e.g. BindBuffer(BufferTargetARB, uint) and
    BindBuffer(GLEnum, uint). Prefer the typed enum; use GLEnum for values
    the typed enum lacks. Mask parameters additionally accept a raw uint,
    e.g. Clear(uint) and Clear(ClearBufferMask).
  - Pointer vs managed: a native "const T*" parameter appears as T* (unsafe),
    as "ref readonly T" (call with "in x"), and - where the count is
    inferable - as ReadOnlySpan<T> (drops the count parameter); a native
    "T*" output appears as T*, "out T" and Span<T>. Generic T0 : unmanaged
    versions exist for void* parameters (BufferData<T0>, TexImage2D<T0>,
    DrawElements<T0>, ReadPixels<T0>).
  - Gen/Delete singletons: GenBuffer() returns one uint; GenBuffers(uint n)
    returns the first of n; GenBuffers(Span<uint>) fills a span;
    DeleteBuffer(uint); DeleteBuffers(ReadOnlySpan<uint>). The same shape
    exists for Textures, VertexArrays, Framebuffers, Renderbuffers, Samplers,
    Queries, ProgramPipelines, TransformFeedbacks.
  - Handle-struct variants: wherever a uint handle array is accepted, an
    overload with the matching struct exists (GenBuffers(Span<Buffer>),
    DeleteTextures(ReadOnlySpan<Texture>)).
  - String helpers: parameters that are C strings accept string
    (GetUniformLocation(uint, string), ObjectLabel(..., string)), and several
    "get a string back" calls have string-returning conveniences
    (GetStringS, GetShaderInfoLog(uint) : string).

Signature notation below: instance methods on GL unless marked
"[GLOverloads]" (static extension, same call syntax). Attributes such as
[Flow] and [Count] are omitted.

3. BUFFERS AND VERTEX ARRAYS
----------------------------
    uint GenBuffer()
    void GenBuffers(uint n, out uint buffers)
    void GenBuffers(Span<uint> buffers)
    uint GenBuffers(uint n)
    void BindBuffer(BufferTargetARB target, uint buffer)
    void BufferData(BufferTargetARB target, nuint size, void* data, BufferUsageARB usage)
    void BufferData<T0>(BufferTargetARB target, nuint size, ref readonly T0 data, BufferUsageARB usage) where T0 : unmanaged
    void BufferData<T0>(BufferTargetARB target, ReadOnlySpan<T0> data, BufferUsageARB usage) where T0 : unmanaged
                                            // size = data.Length * sizeof(T0)
    void BufferSubData(BufferTargetARB target, nint offset, nuint size, void* data)
    void BufferSubData<T0>(BufferTargetARB target, nint offset, ReadOnlySpan<T0> data) where T0 : unmanaged
    void BufferStorage<T0>(BufferStorageTarget target, nuint size, ReadOnlySpan<T0> data, BufferStorageMask flags) [GLOverloads]
    void* MapBufferRange(BufferTargetARB target, nint offset, nuint length, MapBufferAccessMask access)
    bool  UnmapBuffer(BufferTargetARB target)
    void BindBufferBase(BufferTargetARB target, uint index, uint buffer)
    void DeleteBuffer(uint buffers)
    void DeleteBuffers(ReadOnlySpan<uint> buffers)

    uint GenVertexArray()
    void GenVertexArrays(Span<uint> arrays)
    void BindVertexArray(uint array)
    void EnableVertexAttribArray(uint index)
    void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, uint stride, nint pointer)
    void VertexAttribPointer(uint index, int size, VertexAttribPointerType type, bool normalized, uint stride, void* pointer)
                                            // "pointer" is the byte offset
                                            // into the bound array buffer;
                                            // the nint overload needs no
                                            // unsafe context
    void VertexAttribDivisor(uint index, uint divisor)
    void DeleteVertexArray(uint arrays)
    void DeleteVertexArrays(ReadOnlySpan<uint> arrays)

4. SHADERS AND PROGRAMS
-----------------------
    uint CreateShader(ShaderType type)
    void ShaderSource(uint shader, string @string)                 // one source
    void ShaderSource(uint shader, uint count, string[] @stringSa, int* length)
    void CompileShader(uint shader)
    int  GetShader(uint shader, ShaderParameterName pname)         // returns the value
    void GetShader(uint shader, ShaderParameterName pname, out int @params)
    string GetShaderInfoLog(uint shader)
    void GetShaderInfoLog(uint shader, out string info)
    void DeleteShader(uint shader)

    uint CreateProgram()
    void AttachShader(uint program, uint shader)
    void LinkProgram(uint program)
    int  GetProgram(uint program, ProgramPropertyARB pname)
    void GetProgram(uint program, ProgramPropertyARB pname, out int @params)
    string GetProgramInfoLog(uint program)
    void UseProgram(uint program)
    void DeleteProgram(uint program)
    int  GetUniformLocation(uint program, string name)
    int  GetAttribLocation(uint program, string name)
    string GetActiveAttrib(uint program, uint index, out int size, out AttributeType type)
    string GetActiveUniform(uint program, uint uniformIndex, out int size, out UniformType type)

Compile/link status idiom: GetShader(s, ShaderParameterName.CompileStatus)
returns 0 on failure; GetProgram(p, ProgramPropertyARB.LinkStatus) likewise.

5. UNIFORMS
-----------
    void Uniform1(int location, int v0)
    void Uniform1(int location, float v0)
    void Uniform1(int location, uint v0)
    void Uniform1(int location, double x)
    void Uniform1(int location, uint count, ReadOnlySpan<float> value) [GLOverloads]  // also int/uint/double
    void Uniform2(int location, Vector2 vector)              // System.Numerics
    void Uniform3(int location, Vector3 vector)
    void Uniform4(int location, Vector4 vector)
    void Uniform4(int location, Quaternion quaternion)       // System.Numerics
    void Uniform3(int location, uint count, ReadOnlySpan<float> value) [GLOverloads]
    void UniformMatrix4(int location, uint count, bool transpose, float* value)
    void UniformMatrix4(int location, uint count, bool transpose, ref readonly float value)
    void UniformMatrix4(int location, bool transpose, ReadOnlySpan<float> value)
    void ProgramUniform2/3/4(uint program, int location, Vector2/3/4 vector)

There is no Uniform overload taking Maths.Vector3D<T> or Matrix4X4<T>;
convert with .ToSystem() (SystemNumericsExtensions) or pass the first
component by "in" (see example 4).

6. TEXTURES AND SAMPLERS
------------------------
    uint GenTexture()
    void GenTextures(Span<uint> textures)
    void ActiveTexture(TextureUnit texture)
    void BindTexture(TextureTarget target, uint texture)
    void TexImage2D(TextureTarget target, int level, InternalFormat internalformat, uint width, uint height, int border, PixelFormat format, PixelType type, void* pixels)
    void TexImage2D<T0>(TextureTarget target, int level, InternalFormat internalformat, uint width, uint height, int border, PixelFormat format, PixelType type, ReadOnlySpan<T0> pixels) [GLOverloads]
    void TexSubImage2D<T0>(TextureTarget target, int level, int xoffset, int yoffset, uint width, uint height, PixelFormat format, PixelType type, ReadOnlySpan<T0> pixels) [GLOverloads]
    void TexStorage2D(TextureTarget target, uint levels, SizedInternalFormat internalformat, uint width, uint height)
    void TexParameter(TextureTarget target, TextureParameterName pname, int param)
    void TexParameter(TextureTarget target, TextureParameterName pname, float param)
    void GenerateMipmap(TextureTarget target)
    void GetTexImage<T0>(TextureTarget target, int level, PixelFormat format, PixelType type, Span<T0> pixels) [GLOverloads]
    void PixelStore(PixelStoreParameter pname, int param)
    void DeleteTexture(uint textures)
    void DeleteTextures(ReadOnlySpan<uint> textures)

TexImage2D's internalformat also has an "int" overload for raw GL constants.
TexParameter takes int/float, so cast enum values:
(int)TextureMinFilter.Linear, (int)TextureWrapMode.ClampToEdge.

7. FRAMEBUFFERS AND RENDERBUFFERS
---------------------------------
    uint GenFramebuffer()
    void BindFramebuffer(FramebufferTarget target, uint framebuffer)
    void FramebufferTexture2D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget textarget, uint texture, int level)
    uint GenRenderbuffer()
    void BindRenderbuffer(RenderbufferTarget target, uint renderbuffer)
    void RenderbufferStorage(RenderbufferTarget target, InternalFormat internalformat, uint width, uint height)
    void FramebufferRenderbuffer(FramebufferTarget target, FramebufferAttachment attachment, RenderbufferTarget renderbuffertarget, uint renderbuffer)
    GLEnum CheckFramebufferStatus(FramebufferTarget target)   // compare with
                                                              // GLEnum.FramebufferComplete
    void DrawBuffers(ReadOnlySpan<DrawBufferMode> bufs)
    void BlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, ClearBufferMask mask, BlitFramebufferFilter filter)
    void ReadPixels(int x, int y, uint width, uint height, PixelFormat format, PixelType type, void* pixels)
    void ReadPixels<T0>(int x, int y, uint width, uint height, PixelFormat format, PixelType type, Span<T0> pixels) [GLOverloads]

8. STATE, DRAWING, QUERIES, SYNC, COMPUTE
-----------------------------------------
    void ClearColor(float red, float green, float blue, float alpha)
    void ClearColor(System.Drawing.Color color)               // divides by 255
    void ClearColor<T>(Vector4D<T> color)                     // ALSO divides by
                                                              // 255 - pass 0..255
    void Clear(ClearBufferMask mask)
    void Viewport(int x, int y, uint width, uint height)
    void Viewport(Vector2D<int> size)
    void Viewport(Vector2D<int> location, Vector2D<int> size)
    void Scissor(int x, int y, uint width, uint height)
    void Enable(EnableCap cap)  / void Disable(EnableCap cap)
    void DepthFunc(DepthFunction func)
    void CullFace(TriangleFace mode)
    void BlendFunc(BlendingFactor sfactor, BlendingFactor dfactor)
    void PolygonMode(TriangleFace face, PolygonMode mode)
    void DrawArrays(PrimitiveType mode, int first, uint count)
    void DrawArraysInstanced(PrimitiveType mode, int first, uint count, uint instancecount)
    void DrawElements(PrimitiveType mode, uint count, DrawElementsType type, void* indices)
    void DrawElements<T0>(PrimitiveType mode, uint count, DrawElementsType type, ref readonly T0 indices) where T0 : unmanaged
    void DrawElements<T0>(PrimitiveType mode, uint count, DrawElementsType type, ReadOnlySpan<T0> indices) [GLOverloads]
    void DrawElementsInstanced(PrimitiveType mode, uint count, DrawElementsType type, void* indices, uint instancecount)
    GLEnum GetError()                                          // compare with
                                                               // (GLEnum)ErrorCode.X
    int   GetInteger(GetPName pname)  / void GetInteger(GetPName pname, out int data)
    float GetFloat(GetPName pname)    / bool GetBoolean(GetPName pname)
    void  GetFloat(GLEnum pname, out Matrix4x4 matrix)          // System.Numerics
    string GetStringS(StringName name)                          // Vendor, Renderer,
                                                                // Version, ...
    string GetStringS(StringName name, uint index)              // Extensions, i
    byte* GetString(StringName name)
    nint  FenceSync(SyncCondition condition, SyncBehaviorFlags flags)
    GLEnum ClientWaitSync(nint sync, SyncObjectMask flags, ulong timeout)
    void  DeleteSync(nint sync)
    void  DispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z)
    void  MemoryBarrier(MemoryBarrierMask barriers)
    void  ObjectLabel(ObjectIdentifier identifier, uint name, uint length, string label)

DrawElements with an element buffer bound: the "indices" argument is a byte
OFFSET, so pass (void*)0 (or (void*)byteOffset) in an unsafe block. Do NOT
use the ref readonly / ReadOnlySpan forms for that case - they pass a
managed address, which is only correct when NO element buffer is bound
(client-side indices, compatibility contexts only).

9. DEBUG OUTPUT
---------------
    [UnmanagedFunctionPointer(CallingConvention.Winapi)]
    public delegate void DebugProc(GLEnum source, GLEnum type, int id, GLEnum severity, int length, nint message, nint userParam);

    void DebugMessageCallback(DebugProc callback, void* userParam)
    void DebugMessageCallback<T0>(DebugProc callback, ref readonly T0 userParam) where T0 : unmanaged
    void DebugMessageControl(DebugSource source, DebugType type, DebugSeverity severity, uint count, ref readonly uint ids, bool enabled)

Lifetime: the callback parameter is marked [PinObject(PinMode.UntilNextCall)]
- the binding keeps the delegate alive only until the NEXT GL call. The
driver keeps calling it for the life of the context, so YOU must hold a
strong reference to the DebugProc delegate (a static field is the usual
choice) or the GC will collect it and the next debug message crashes the
process. Read the message with
SilkMarshal.PtrToString(message, NativeStringEncoding.UTF8) (length bytes).
Enable with Enable(EnableCap.DebugOutput) and, for callbacks on the calling
thread, Enable(EnableCap.DebugOutputSynchronous). Enum member names carry
their prefix: DebugSeverity.DebugSeverityHigh / DebugSeverityNotification,
DebugType.DebugTypeError, DebugSource.DebugSourceApi, and each has DontCare.

10. GL HANDLE STRUCTS
---------------------
Fifteen thin wrappers, all shaped like:

    public unsafe partial struct Buffer { public Buffer(uint? handle = null); public uint Handle; }

    Buffer, DisplayList, Framebuffer, PerfQueryHandle, PerfQueryId, Program,
    ProgramPipeline, Query, Renderbuffer, Sampler, Shader, Sync, Texture,
    TransformFeedback, VertexArray

They exist for the Span<Buffer>/ReadOnlySpan<Texture> overload variants and
for type-safe storage in your own code; there are NO implicit conversions to
uint, so pass .Handle to the plain-uint entry points. Most calls take uint
directly; the structs are optional.

11. ENUMS
---------
344 enums, all in namespace CodeBrix.Platform.OpenGL, all ": int" with GL
hex values, each member tagged [NativeName("Name", "GL_...")]; mask enums
carry [Flags]. Naming: the GL_ prefix is dropped and the remainder is
PascalCased (GL_ARRAY_BUFFER -> ArrayBuffer, GL_COLOR_BUFFER_BIT ->
ColorBufferBit). Enums from the OpenGL registry keep their registry names
including ARB/EXT suffixes even though they are core (BufferTargetARB,
BufferUsageARB, ProgramPropertyARB, BlendEquationModeEXT). GLEnum is the
union enum with 1,300+ members covering every constant; every typed-enum
parameter has a GLEnum overload, and GLEnum values are returned by GetError
and CheckFramebufferStatus.

Most-used enums and representative members:

    BufferTargetARB        ArrayBuffer, ElementArrayBuffer, UniformBuffer,
                           ShaderStorageBuffer, PixelPackBuffer, ...
    BufferUsageARB         StaticDraw, DynamicDraw, StreamDraw, ...
    ShaderType             VertexShader, FragmentShader, GeometryShader,
                           ComputeShader, TessControlShader, TessEvaluationShader
    PrimitiveType          Triangles, TriangleStrip, Lines, Points, ...
    DrawElementsType       UnsignedByte, UnsignedShort, UnsignedInt
    VertexAttribPointerType Float, Int, UnsignedByte, HalfFloat, ...
    ClearBufferMask        ColorBufferBit, DepthBufferBit, StencilBufferBit
    EnableCap              DepthTest, CullFace, Blend, ScissorTest,
                           DebugOutput, DebugOutputSynchronous, ...
    TextureTarget          Texture2D, TextureCubeMap, Texture2DArray, ...
    TextureUnit            Texture0 .. Texture31
    TextureParameterName   TextureMinFilter, TextureMagFilter, TextureWrapS,
                           TextureWrapT, ...
    TextureMinFilter / TextureMagFilter / TextureWrapMode
                           Linear, Nearest, LinearMipmapLinear / ClampToEdge,
                           Repeat, ...
    InternalFormat         Rgba, Rgba8, Rgb, DepthComponent24, ...
    SizedInternalFormat    Rgba8, Rgba16f, R8, Depth24Stencil8, ...
    PixelFormat            Rgba, Rgb, Red, Bgra, DepthComponent, ...
    PixelType              UnsignedByte, Float, HalfFloat, ...
    ShaderParameterName    CompileStatus, InfoLogLength, ShaderSourceLength
    ProgramPropertyARB     LinkStatus, InfoLogLength, ActiveUniforms, ...
    FramebufferTarget / FramebufferAttachment / FramebufferStatus
                           Framebuffer, ReadFramebuffer / ColorAttachment0,
                           DepthAttachment / FramebufferComplete
    DepthFunction, BlendingFactor, TriangleFace, PolygonMode, StringName,
    GetPName, ErrorCode, MapBufferAccessMask, MemoryBarrierMask,
    DebugSource / DebugType / DebugSeverity, UniformType, AttributeType

12. MATHS (CodeBrix.Platform.OpenGL.Maths)
------------------------------------------
Generic value types over T : unmanaged, IFormattable, IEquatable<T>,
IComparable<T> (use float for GL work; Half, double, int, long, ... also
work). Each generic struct has a same-named NON-generic static companion
class holding the operations, except Quaternion whose statics live on the
struct itself.

    public struct Vector2D<T> { public T X, Y; ... }
    public struct Vector3D<T> { public T X, Y, Z; ... }
    public struct Vector4D<T> { public T X, Y, Z, W; ... }
        ctors: Vector3D(T value), Vector3D(T x, T y, T z),
               Vector3D(Vector2D<T> value, T z);
               Vector4D(Vector3D<T> value, T w)
        static Zero, One, UnitX, UnitY, UnitZ (UnitW)
        T this[int i]; T Length { get; }; T LengthSquared { get; }
        operators + - * / (vector-vector, vector-scalar, unary -), == !=
        explicit casts to Vector3D<float>/<double>/<Half>/<int>/... and to
        System.Numerics.Vector2/3/4
        Vector3D<TOther> As<TOther>()
        void CopyTo(T[] array) / CopyTo(T[] array, int index)

    public static class Vector3D    // same set on Vector2D / Vector4D
        Abs, Add, Clamp, Cross (3D only), Distance, DistanceSquared, Divide,
        Dot, Lerp, Max, Min, Multiply (incl. vector x Matrix3X3/3X4/3X2),
        Negate, Normalize, Reflect, SquareRoot, Subtract,
        Transform(Vector3D<T> position, Matrix4X4<T> matrix),
        Transform(Vector3D<T> value, Quaternion<T> rotation),
        TransformNormal(Vector3D<T> normal, Matrix4X4<T> matrix)
        e.g. public static Vector3D<T> Normalize<T>(Vector3D<T> value)
             public static T Dot<T>(Vector3D<T> vector1, Vector3D<T> vector2)

    public struct Matrix4X4<T> : IEquatable<Matrix4X4<T>>
        public Vector4D<T> Row1, Row2, Row3, Row4;      // fields
        Vector4D<T> Column1..Column4 { get; set; }
        T M11 .. M44 { get; set; }
        Vector4D<T> this[int x]; T this[int x, int y]
        ctors: (Vector4D<T> row1, row2, row3, row4), (16 scalars m11..m44),
               and from Matrix3X2/3X3/3X4/4X3/4X2/2X4
        static Matrix4X4<T> Identity; bool IsIdentity; T GetDeterminant()
        operators + - * (matrix*matrix, matrix*scalar, Vector4D*matrix)
        Matrix4X4<TOther> As<TOther>()
    public static class Matrix4X4
        CreatePerspectiveFieldOfView<T>(T fieldOfView, T aspectRatio, T nearPlaneDistance, T farPlaneDistance)
        CreatePerspective<T>(T width, T height, T near, T far)
        CreatePerspectiveOffCenter<T>(T left, T right, T bottom, T top, T near, T far)
        CreateOrthographic<T>(T width, T height, T zNearPlane, T zFarPlane)
        CreateOrthographicOffCenter<T>(T left, T right, T bottom, T top, T zNear, T zFar)
        CreateLookAt<T>(Vector3D<T> cameraPosition, Vector3D<T> cameraTarget, Vector3D<T> cameraUpVector)
        CreateTranslation<T>(Vector3D<T> position) / (T x, T y, T z)
        CreateScale<T>(T scale) / (Vector3D<T> scales) / (T x, T y, T z) [+ centerPoint overloads]
        CreateRotationX/Y/Z<T>(T radians) [+ centerPoint overloads]
        CreateFromAxisAngle<T>(Vector3D<T> axis, T angle)
        CreateFromQuaternion<T>(Quaternion<T> quaternion)
        CreateFromYawPitchRoll<T>(T yaw, T pitch, T roll)
        CreateWorld, CreateBillboard, CreateConstrainedBillboard,
        CreateReflection, CreateShadow
        bool Invert<T>(Matrix4X4<T> matrix, out Matrix4X4<T> result)
        Matrix4X4<T> Transpose<T>(Matrix4X4<T> matrix)
        Multiply, Add, Subtract, Negate, Lerp, Transform(m, Quaternion<T>)
        bool Decompose<T>(Matrix4X4<T> matrix, out Vector3D<T> scale, out Quaternion<T> rotation, out Vector3D<T> translation)
    Row-major storage, row-vector convention (v * M), the same as
    System.Numerics.Matrix4x4. Upload to GLSL with transpose = false and
    multiply in GLSL as gl_Position = vec4(pos,1) * model * view * proj, or
    keep column-vector GLSL and pass transpose = true.

    Other matrix shapes: Matrix2X2<T>, Matrix2X3<T>, Matrix2X4<T>,
    Matrix3X2<T>, Matrix3X3<T>, Matrix3X4<T>, Matrix4X2<T>, Matrix4X3<T>,
    Matrix5X4<T>, each with a static companion (Matrix3X3: CreateRotationX/
    Y/Z, CreateScale, CreateFromQuaternion, CreateFromAxisAngle,
    CreateFromYawPitchRoll, Invert, Transpose, Multiply, ...; Matrix3X2:
    CreateRotation, CreateScale, CreateSkew, CreateTranslation, Invert, ...).

    public struct Quaternion<T> { public T X, Y, Z, W; ... }
        ctors: (T x, T y, T z, T w), (Vector3D<T> vectorPart, T scalarPart)
        static Identity; bool IsIdentity; T Length(); T LengthSquared()
        static CreateFromAxisAngle(Vector3D<T> axis, T angle)
        static CreateFromYawPitchRoll(T yaw, T pitch, T roll)
        static CreateFromRotationMatrix(Matrix4X4<T> m) / (Matrix3X3<T> m)
        static Normalize, Conjugate, Inverse, Dot, Lerp, Slerp,
               Concatenate, Multiply, Add, Subtract, Divide, Negate
        operators + - * / ; explicit cast to System.Numerics.Quaternion

    Shapes:
    public struct Box2D<T>      { Vector2D<T> Min, Max; Center; Size;
                                  Contains(point|box); GetDistanceToNearestEdge;
                                  GetTranslated; GetScaled; GetInflated; As }
    public struct Box3D<T>      { Vector3D<T> Min, Max; same members in 3D }
    public struct Rectangle<T>  { Vector2D<T> Origin, Size; Center; Max;
                                  HalfSize; Contains; GetDistanceToNearestEdge;
                                  GetTranslated; GetScaled; GetInflated }
        public static class Rectangle { Rectangle<T> FromLTRB<T>(T left, T top, T right, T bottom) }
    public struct Cube<T>       { Vector3D<T> Origin, Size; Center; Max; HalfSize; ... }
    public struct Circle<T>     { Vector2D<T> Center; T Radius; Diameter;
                                  SquaredRadius; Circumference; Contains; ... }
    public struct Sphere<T>     { Vector3D<T> Center; T Radius; Diameter; ... }
    public struct Plane<T>      { Vector3D<T> Normal; T Distance }
        public static class Plane { CreateFromVertices, Dot, DotCoordinate,
                                    DotNormal, Normalize, Transform }
    public struct Ray2D<T> / Ray3D<T> { Origin; Direction; GetPoint(T distance) }

    public static partial class Scalar          // generic scalar math
        TTo As<TFrom, TTo>(TFrom val)           // numeric conversion between
                                                // any two scalar types
        bool IsHardwareAccelerated { get; }
        Add, Subtract, Multiply, Divide, Negate, Equal, NotEqual,
        GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual,
        IsNaN, IsFinite, IsInfinity, IsNegative, IsNormal, IsSubnormal,
        Abs, Sqrt, Cbrt, Pow, Exp, Log, Log10, Sin, Cos, Tan, Asin, Acos,
        Atan, Atan2, Sinh, Cosh, Tanh, Asinh, Acosh, Atanh, Floor, Ceiling,
        Round, Truncate, Sign, Min, Max, IEEERemainder, Reciprocal,
        DegreesToRadians<T>(T degrees), RadiansToDegrees<T>(T radians)
        e.g. public static T Sqrt<T>(T x)
    public static partial class Scalar<T>       // per-type constants
        Epsilon, MaxValue, MinValue, NaN, NegativeInfinity, PositiveInfinity,
        Zero, One, Two, MinusOne, MinusTwo, E, Pi, PiOver2, Tau,
        DegreesPerRadian, RadiansPerDegree

    public static class SystemNumericsExtensions
        ToSystem(this Vector2D<float>)  : System.Numerics.Vector2
        ToSystem(this Vector3D<float>)  : Vector3
        ToSystem(this Vector4D<float>)  : Vector4
        ToSystem(this Matrix4X4<float>) : Matrix4x4
        ToSystem(this Matrix3X2<float>) : Matrix3x2
        ToSystem(this Quaternion<float>): Quaternion
        ToSystem(this Plane<float>)     : Plane
        ToGeneric(this Vector2/Vector3/Vector4/Matrix4x4/Matrix3x2/Quaternion/Plane)
                                        : the <float> generic equivalents
    Only the <float> instantiations convert; for others go through As<float>().

13. CORE.NATIVE HELPERS
-----------------------
    public static class SilkMarshal
        nint   Allocate(int length)              // unmanaged bytes
        bool   Free(nint ptr)
        nint   StringToPtr(string? input, NativeStringEncoding encoding = NativeStringEncoding.Ansi)
        string? PtrToString(nint input, NativeStringEncoding encoding = NativeStringEncoding.Ansi)
        void   FreeString(nint ptr, NativeStringEncoding encoding = NativeStringEncoding.Ansi)
        nuint  StringLength(nint ptr, NativeStringEncoding encoding)  // and Span forms
        GlobalMemory StringToMemory(string? input, NativeStringEncoding encoding = ...)
        string MemoryToString(GlobalMemory input, NativeStringEncoding e = ...)
        GlobalMemory StringArrayToMemory(...) / nint StringArrayToPtr(...)
        string[] PtrToStringArray(...) / string[] MemoryToStringArray(...)
        int    GetMaxSizeOf(string? input, NativeStringEncoding encoding = ...)
        int    StringIntoSpan(...)
        nint   DelegateToPtr(Delegate d) / DelegateToPtr<TDelegate>(...)
        T      PtrToDelegate<T>(nint p) where T : Delegate
        delegate* unmanaged[Cdecl]<void> DelegateToCdecl(...)   // + Stdcall,
                                                               // Fastcall, Thiscall
        ref T  NullRef<T>()
        Guid   GuidOf<T>()  / Guid* GuidPtrOf<T>()
        void   ThrowHResult(int hResult)
        static readonly bool IsWinapiStdcall     // true on Windows
    public enum NativeStringEncoding { BStr, LPStr, LPTStr, LPUTF8Str, LPWStr, WinString, Ansi = LPStr, Auto = LPTStr, Uni = LPWStr, UTF8 = LPUTF8Str }
        GL strings are UTF-8/ASCII: use NativeStringEncoding.UTF8.

    public sealed class GlobalMemory : IDisposable   // owned unmanaged block
        static GlobalMemory Allocate(int length)
        int Length; nint Handle; Span<byte> AsSpan(); Span<T> AsSpan<T>();
        ref T AsRef<T>(int index = 0); T* AsPtr<T>(int index = 0);
        implicit operator Span<byte> / void* / nint; ref byte this[int]
    public unsafe struct ComPtr<T> : IDisposable    // COM smart pointer
        (Windows interop only; T* Handle, Get(), GetAddressOf(), Release(),
        Detach(), Dispose())
    public abstract class NativeApiContainer : IDisposable   // base of GL
        GcUtility GcUtility; IVTable CurrentVTable; PurgeEntryPoints()
    public abstract class NativeAPI : NativeApiContainer     // + Context,
                                                             // IsExtensionPresent
    public abstract class NativeExtension<T> : NativeApiContainer where T : NativeAPI
        (base class for extension bindings; this package ships none)
    public class GcUtility  { Pin(object, int slot); PinUntilNextCall(object, int slot); Unpin(object, int? slot = null) }
    public struct HResult   { int Value; IsSuccess; IsFailure; IsError }
    Windows-only COM/D3D interop types carried from upstream Core:
    IUnknown, IInspectable, ID3D10Blob, ID3DInclude, ID3DDestructionNotifier,
    ID3DShaderCache*, D3DCommon, D3DShaderMacro, the *VtblExtensions
    classes, WinString, Luid, ScHandle, SecurityAttributes, Timespec,
    TagPoint/TagRect/TagSize/TagPaletteEntry, VkHandle,
    VkNonDispatchableHandle. Ignore them for OpenGL work.

14. CORE.LOADER
---------------
    public class UnmanagedLibrary : IDisposable
        UnmanagedLibrary(string name) / (string[] names)
        UnmanagedLibrary(string name, LibraryLoader loader) / (string[] names, LibraryLoader loader)
        UnmanagedLibrary(string name, LibraryLoader loader, PathResolver pathResolver)  // + string[] form
        static bool TryCreate(string name, LibraryLoader loader, PathResolver pathResolver, out UnmanagedLibrary library)
        nint Handle { get; }
        nint LoadFunction(string name)                 // throws if missing
        bool TryLoadFunction(string name, out nint pfn)
        T    LoadFunction<T>(string name)              // as delegate
        bool TryLoadFunction<T>(string name, out T? pfn) where T : Delegate
    public abstract class LibraryLoader
        static LibraryLoader GetPlatformDefaultLoader()
        nint LoadNativeLibrary(string name) / (string[] names) / (name, PathResolver) / (names, PathResolver)
        bool TryLoadNativeLibrary(... , out nint result)   // same four shapes
        nint LoadFunctionPointer(nint handle, string functionName)
        bool TryLoadFunctionPointer(nint handle, string functionName, out nint pfn)
        void FreeNativeLibrary(nint handle)
        void RegisterDependencies(params string[] names)
    public abstract class PathResolver
        static PathResolver Default { get; }               // a DefaultPathResolver
        abstract IEnumerable<string> EnumeratePossibleLibraryLoadTargets(string name)
    public class DefaultPathResolver : PathResolver
        List<Func<string, IEnumerable<string>>> Resolvers { get; set; }
        static readonly Func<string, IEnumerable<string>> PassthroughResolver,
            LinuxVersioningResolver, MacVersioningResolver, BaseDirectoryResolver,
            MainModuleDirectoryResolver, RuntimesFolderResolver,
            NativePackageResolver, SilkDirectoryResolver
    public abstract class SearchPathContainer     // per-OS name table base
        static UnderlyingPlatform Platform { get; set; }   // auto-detected;
                                                           // settable
        abstract string[] Windows64, Windows86, Linux, MacOS { get; }
        virtual  string[] Android (=> Linux), IOS (=> MacOS)
        string[] GetLibraryNames()
    public enum UnderlyingPlatform { Unknown, Windows64, Windows86, Linux, Android, MacOS, IOS }
    public class SymbolLoadingException : Exception
        SymbolLoadingException(string symbol)                // "Native symbol
                                                             // not found (Symbol: x)"
        SymbolLoadingException(string symbol, string msg)

15. OTHER CORE TYPES
--------------------
CodeBrix.Platform.OpenGL.Core:
    Bool32 / Bool8        4-byte / 1-byte native bools with implicit
                          conversions to and from bool and uint/byte
    Version32 / Version64 packed Major.Minor.Patch versions (Version32(uint
                          major, uint minor, uint patch); implicit to uint
                          and System.Version)
    RawImage              (int width, int height, Memory<byte> rgbaPixels);
                          Width, Height, Pixels - RGBA8 pixel carrier
    PfnVoidFunction       wraps delegate* unmanaged[Cdecl]<void>; implicit
                          conversions from Delegate and to nint
    PlatformException     thrown by loaders on unsupported platforms
    BreakneckLock         spin lock struct (Create(); Enter(ref bool taken);
                          TryEnter(...); Exit())
CodeBrix.Platform.OpenGL.Core.Contexts (window-handle plumbing for
windowing layers that implement it; nothing here creates windows):
    INativeWindow         NativeWindowFlags Kind; X11 (Display, Window)?;
                          Wayland (Display, Surface)?; Win32 (Hwnd, HDC,
                          HInstance)?; Cocoa nint?; EGL; Glfw; Sdl; Android;
                          UIKit; WinRT; Vivante; DXHandle
    INativeWindowSource   { INativeWindow? Native { get; } }
    IVkSurface / IVkSurfaceSource   Vulkan surface plumbing (unused by GL)
    NativeWindowFlags     [Flags] enum (Glfw, Sdl, Cocoa, UIKit, Wayland,
                          WinRT, Android, Vivante, EGL, ...)
CodeBrix.Platform.OpenGL.Core.Attributes:
    NativeNameAttribute, CountAttribute, FlowAttribute, ExtensionAttribute,
    InjectAttribute, UnmanagedTypeAttribute - metadata on the generated
    bindings; read-only for consumers (ExtensionAttribute(string name) is
    what TryGetExtension<T> looks for on your own extension classes).

16. PORT PROVENANCE MARKER
--------------------------
    public static class PortStatus
        static string UpstreamVersion { get; }   // "Silk.NET v2.23.0"
        static string UpstreamCommit  { get; }   // upstream git sha
        static string PortedSoFar     { get; }   // human-readable summary

COMPLETE EXAMPLES
=================
All examples assume "using CodeBrix.Platform.OpenGL;" and a window whose GL
context is current on the calling thread. The windowing layer is
represented by a delegate "getProcAddress" of type Func<string, nint>
(e.g. name => SDL_GL_GetProcAddress(name), or glfwGetProcAddress).

Example 1 - a triangle (context from a GetProcAddress delegate, VAO/VBO/EBO,
shader, draw). Compiles with AllowUnsafeBlocks=true.

    using System;
    using CodeBrix.Platform.OpenGL;

    public sealed class Triangle : IDisposable
    {
        private readonly GL _gl;
        private readonly uint _vao, _vbo, _ebo, _program;

        private const string VertexSrc = @"#version 330 core
            layout(location = 0) in vec3 aPos;
            layout(location = 1) in vec3 aColor;
            out vec3 vColor;
            void main() { vColor = aColor; gl_Position = vec4(aPos, 1.0); }";

        private const string FragmentSrc = @"#version 330 core
            in vec3 vColor;
            out vec4 FragColor;
            void main() { FragColor = vec4(vColor, 1.0); }";

        public Triangle(Func<string, nint> getProcAddress)
        {
            _gl = GL.GetApi(getProcAddress);

            float[] vertices =
            {
                //   x      y     z      r     g     b
                -0.5f, -0.5f, 0f,    1f, 0f, 0f,
                 0.5f, -0.5f, 0f,    0f, 1f, 0f,
                 0.0f,  0.5f, 0f,    0f, 0f, 1f,
            };
            uint[] indices = { 0, 1, 2 };

            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
            _gl.BufferData<float>(BufferTargetARB.ArrayBuffer,
                                  new ReadOnlySpan<float>(vertices),
                                  BufferUsageARB.StaticDraw);

            _ebo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
            _gl.BufferData<uint>(BufferTargetARB.ElementArrayBuffer,
                                 new ReadOnlySpan<uint>(indices),
                                 BufferUsageARB.StaticDraw);

            uint stride = 6 * sizeof(float);
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, (nint)0);
            _gl.EnableVertexAttribArray(0);
            _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, stride, (nint)(3 * sizeof(float)));
            _gl.EnableVertexAttribArray(1);

            uint vs = CompileShader(ShaderType.VertexShader, VertexSrc);
            uint fs = CompileShader(ShaderType.FragmentShader, FragmentSrc);
            _program = _gl.CreateProgram();
            _gl.AttachShader(_program, vs);
            _gl.AttachShader(_program, fs);
            _gl.LinkProgram(_program);
            if (_gl.GetProgram(_program, ProgramPropertyARB.LinkStatus) == 0)
            {
                throw new InvalidOperationException("Link failed: " + _gl.GetProgramInfoLog(_program));
            }
            _gl.DeleteShader(vs);
            _gl.DeleteShader(fs);

            _gl.BindVertexArray(0);
        }

        private uint CompileShader(ShaderType type, string source)
        {
            uint shader = _gl.CreateShader(type);
            _gl.ShaderSource(shader, source);
            _gl.CompileShader(shader);
            if (_gl.GetShader(shader, ShaderParameterName.CompileStatus) == 0)
            {
                throw new InvalidOperationException(type + " compile failed: " + _gl.GetShaderInfoLog(shader));
            }
            return shader;
        }

        public unsafe void Render(int width, int height)
        {
            _gl.Viewport(0, 0, (uint)width, (uint)height);
            _gl.ClearColor(0.1f, 0.1f, 0.15f, 1f);
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _gl.UseProgram(_program);
            _gl.BindVertexArray(_vao);
            _gl.DrawElements(PrimitiveType.Triangles, 3, DrawElementsType.UnsignedInt, (void*)0);
            _gl.BindVertexArray(0);
            // then: your windowing layer's SwapBuffers()
        }

        public void Dispose()
        {
            _gl.DeleteProgram(_program);
            _gl.DeleteBuffer(_ebo);
            _gl.DeleteBuffer(_vbo);
            _gl.DeleteVertexArray(_vao);
            _gl.Dispose();
        }
    }

Example 2 - letting the bindings open the GL library, with a windowing-layer
fallback (Windows needs both: wglGetProcAddress returns 0 for GL 1.1
functions, opengl32.dll exports only those).

    using CodeBrix.Platform.OpenGL;
    using CodeBrix.Platform.OpenGL.Core.Contexts;

    static GL CreateGl(Func<string, nint> wglOrGlxGetProcAddress)
    {
        string[] names = OperatingSystem.IsWindows() ? new[] { "opengl32.dll" }
                       : OperatingSystem.IsMacOS()   ? new[] { "/System/Library/Frameworks/OpenGL.framework/OpenGL" }
                       :                               new[] { "libGL.so.1", "libGL.so" };

        INativeContext exported = GL.CreateDefaultContext(names);   // FileNotFoundException if none load
        var ctx = new MultiNativeContext(new LamdaNativeContext(wglOrGlxGetProcAddress), exported);
        return GL.GetApi(ctx);
    }

Example 3 - debug output with a callback that survives garbage collection.

    using System;
    using CodeBrix.Platform.OpenGL;
    using CodeBrix.Platform.OpenGL.Core.Native;

    static class GlDebug
    {
        // Static field: the driver holds the native pointer for the life of
        // the context; the binding only pins it until the next GL call.
        private static readonly DebugProc Callback = OnMessage;

        public static unsafe void Enable(GL gl)
        {
            gl.Enable(EnableCap.DebugOutput);
            gl.Enable(EnableCap.DebugOutputSynchronous);
            gl.DebugMessageCallback(Callback, null);
            uint ids = 0;   // count == 0 -> applies to all ids
            gl.DebugMessageControl(DebugSource.DontCare, DebugType.DontCare,
                                   DebugSeverity.DebugSeverityNotification, 0, in ids, false);
        }

        private static void OnMessage(GLEnum source, GLEnum type, int id, GLEnum severity,
                                      int length, nint message, nint userParam)
        {
            string text = SilkMarshal.PtrToString(message, NativeStringEncoding.UTF8) ?? string.Empty;
            Console.WriteLine($"GL {type} [{severity}] {id}: {text}");
        }
    }

Example 4 - camera matrices with Maths and uploading them as uniforms.

    using CodeBrix.Platform.OpenGL;
    using CodeBrix.Platform.OpenGL.Maths;

    static void UploadMvp(GL gl, uint program, float aspect, float seconds)
    {
        Matrix4X4<float> model = Matrix4X4.CreateRotationY(seconds);
        Matrix4X4<float> view = Matrix4X4.CreateLookAt(
            new Vector3D<float>(0f, 1f, 3f), Vector3D<float>.Zero, Vector3D<float>.UnitY);
        Matrix4X4<float> proj = Matrix4X4.CreatePerspectiveFieldOfView(
            Scalar.DegreesToRadians(60f), aspect, 0.1f, 100f);

        Matrix4X4<float> mvp = model * view * proj;   // row-vector convention

        int loc = gl.GetUniformLocation(program, "uMvp");
        gl.UniformMatrix4(loc, 1, false, in mvp.Row1.X);   // 16 contiguous floats

        Vector3D<float> lightDir = Vector3D.Normalize(new Vector3D<float>(1f, 2f, 0.5f));
        gl.Uniform3(gl.GetUniformLocation(program, "uLightDir"), lightDir.ToSystem());
    }

    // GLSL for transpose = false with this convention:
    //     gl_Position = vec4(aPos, 1.0) * uMvp;
    // If your shader multiplies uMvp * vec4(aPos, 1.0), pass transpose = true
    // (or upload Matrix4X4.Transpose(mvp)).

Example 5 - RGBA8 texture upload from a byte array and a sampler setup.

    static uint CreateTexture(GL gl, uint width, uint height, byte[] rgba)
    {
        uint tex = gl.GenTexture();
        gl.ActiveTexture(TextureUnit.Texture0);
        gl.BindTexture(TextureTarget.Texture2D, tex);
        gl.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
        gl.TexImage2D<byte>(TextureTarget.Texture2D, 0, InternalFormat.Rgba8,
                            width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte,
                            new ReadOnlySpan<byte>(rgba));
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        gl.GenerateMipmap(TextureTarget.Texture2D);
        return tex;
    }

Example 6 - reading back the framebuffer and checking for errors.

    static byte[] ReadBackRgba(GL gl, int width, int height)
    {
        var pixels = new byte[width * height * 4];
        gl.PixelStore(PixelStoreParameter.PackAlignment, 1);
        gl.ReadPixels<byte>(0, 0, (uint)width, (uint)height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.AsSpan());
        GLEnum err = gl.GetError();
        if (err != GLEnum.NoError) throw new InvalidOperationException("GL error " + err);
        return pixels;   // bottom row first (OpenGL origin is bottom-left)
    }

    static void PrintDriverInfo(GL gl)
    {
        Console.WriteLine(gl.GetStringS(StringName.Vendor));
        Console.WriteLine(gl.GetStringS(StringName.Renderer));
        Console.WriteLine(gl.GetStringS(StringName.Version));
        Console.WriteLine(gl.GetStringS(StringName.ShadingLanguageVersion));
        Console.WriteLine("Max texture size: " + gl.GetInteger(GetPName.MaxTextureSize));
        Console.WriteLine("Has GL_ARB_debug_output: " + gl.IsExtensionPresent("ARB_debug_output"));
    }

MINIMUM VIABLE PROJECT
======================
A console app that owns no window cannot render; this skeleton shows the
package wiring with the windowing layer left as a placeholder delegate.

    <!-- GlApp.csproj -->
    <Project Sdk="Microsoft.NET.Sdk">
      <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net10.0</TargetFramework>
        <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
      </PropertyGroup>
      <ItemGroup>
        <PackageReference Include="CodeBrix.Platform.OpenGL.MitLicenseForever" Version="*" />
        <!-- plus your windowing package (SDL / GLFW / platform layer) -->
      </ItemGroup>
    </Project>

    // Program.cs
    using System;
    using CodeBrix.Platform.OpenGL;

    // 1. create a window + GL context with your windowing library and make
    //    it current on this thread
    // 2. obtain its proc-address function:
    Func<string, nint> getProcAddress = name => throw new NotImplementedException("wire to SDL_GL_GetProcAddress / glfwGetProcAddress");

    using var triangle = new Triangle(getProcAddress);   // from Example 1
    // 3. per frame:
    //    triangle.Render(width, height);  then swap buffers via the windowing library

PERFORMANCE TIPS
================
- Entry points are resolved once per GL instance and then called through a
  cached unmanaged function pointer; there is no per-call delegate or
  marshalling cost for blittable arguments. Keep ONE GL instance per
  context for the life of the context; do not call GL.GetApi per frame.
- Prefer the Span<T>/ReadOnlySpan<T> and "in" overloads over arrays of
  handles or manual pinning; they pin only for the duration of the call.
- String-taking overloads (ShaderSource(uint, string),
  GetUniformLocation(uint, string), ObjectLabel) allocate and encode on
  every call. Cache uniform locations at link time instead of looking them
  up per frame.
- The string-returning conveniences (GetShaderInfoLog(uint),
  GetProgramInfoLog(uint), GetActiveUniform(...)) query GL twice (length,
  then text); use them at load time only.
- IsExtensionPresent enumerates all extensions on its first call and
  caches the list; TryGetExtension<T> uses reflection (Activator) - do both
  at start-up.
- Maths types are plain structs with generic arithmetic dispatched through
  Scalar<T>; float instantiations are the fast path. For hot inner loops on
  float data consider converting to System.Numerics with ToSystem() and
  back with ToGeneric().
- GetError is a full driver round-trip; call it in debug builds only and
  use the DebugMessageCallback route for diagnostics.
- Avoid ClearColor<T>(Vector4D<T>) and ClearColor(Color) in hot paths if
  the 0..255 scaling surprises you; the four-float ClearColor is direct.

COMMON PITFALLS TO AVOID
========================
- No current context, no calls. Every GL method executes on the calling
  thread against whatever context is current there. Create the GL instance
  and issue all calls on the thread that owns the context (typically the UI
  or render thread). Calling from a Task or another thread yields
  GL_INVALID_OPERATION at best and a crash at worst.
- Errors surface late. A wrong or missing entry point throws
  SymbolLoadingException at the first CALL of that method, not when GL is
  created; a GL.GetApi that "worked" says nothing about which functions
  exist. Check GetStringS(StringName.Version) early.
- DefaultNativeContext / CreateDefaultContext resolve exported symbols
  only. On Windows that is OpenGL 1.1; anything newer needs the windowing
  layer's GetProcAddress (use GL.GetApi(Func<string, nint>) or a
  MultiNativeContext as in Example 2).
- Unsafe is required for pointer overloads and for the DrawElements offset
  idiom ((void*)0). Set <AllowUnsafeBlocks>true</AllowUnsafeBlocks>.
- Span vs pointer overloads mean different things for DrawElements:
  ReadOnlySpan<T0>/"in" pass a MANAGED address (client-side index array),
  void* passes an OFFSET into the bound element buffer. Using the span form
  with an element buffer bound reads garbage or faults.
- BufferData<T0>(target, ReadOnlySpan<T0>, usage) computes the size for
  you; the (target, nuint size, ref readonly T0, usage) form does NOT - size
  is in bytes, and forgetting sizeof(T) uploads a truncated buffer.
- DebugProc lifetime: keep the delegate in a static or long-lived field.
  The binding pins it only until the next GL call; the driver calls it
  forever. A collected delegate means a native call into freed memory.
- ClearColor<T>(Vector4D<T>) and BlendColor<T>(Vector4D<T>) divide each
  component by 255, so Vector4D<float>(0.2f, ...) clears to almost black.
  Use ClearColor(float, float, float, float) for 0..1 values.
- GenBuffers(uint n) returns only the FIRST handle of n; use the Span form
  to get all of them. The single-handle helpers are GenBuffer(),
  GenTexture(), GenVertexArray(), GenFramebuffer(), GenRenderbuffer().
- Enum names keep registry suffixes and prefixes: BufferTargetARB,
  BufferUsageARB, ProgramPropertyARB, DebugSeverity.DebugSeverityHigh,
  DebugType.DebugTypeError, DebugSource.DebugSourceApi. There is no
  BufferTarget or BufferUsage enum.
- GetError and CheckFramebufferStatus return GLEnum, not ErrorCode /
  FramebufferStatus. Compare with GLEnum.NoError / GLEnum.FramebufferComplete
  or cast.
- The namespace "using CodeBrix.Platform.OpenGL;" brings in a struct named
  Buffer; qualify System.Buffer if you use both. Maths.Rectangle<T> and
  Maths.Quaternion<T> collide with System.Drawing / System.Numerics only
  by simple name; the GL.Uniform overloads take the System.Numerics types.
- Matrix layout: Maths matrices are row-major with the row-vector (v * M)
  convention like System.Numerics. Passing transpose = false to
  UniformMatrix4 requires GLSL code that multiplies vec * mat, or pass
  transpose = true / Transpose() for the conventional mat * vec shaders.
- TexParameter takes int/float, not the filter/wrap enums; cast
  (int)TextureMinFilter.Linear.
- No GL context in unit tests: on a headless CI host there is no way to
  make a context, so tests of code that calls GL must inject a fake around
  your own abstraction. The types and enums themselves can be tested
  freely (they never touch native code until a method is called).
- Do not mix with Silk.NET packages: the types are namespace-renamed copies
  and are not interchangeable with Silk.NET.OpenGL.GL or Silk.NET.Maths.

WHAT THIS PACKAGE DOES NOT DO
=============================
- It does not create windows or OpenGL contexts, swap buffers, handle
  input or run a message loop. IGLContext/IGLContextSource/INativeWindow
  are interfaces for a windowing layer to IMPLEMENT; no implementation
  ships here.
- It ships no extension bindings: the Silk.NET.OpenGL.Extensions.* families
  (ARB, EXT, KHR, AMD, INTEL, MESA, NV, ...) are out of scope.
  TryGetExtension<T> only finds classes you write yourself.
- No OpenGL ES (Silk.NET.OpenGLES), no OpenGL Legacy/compatibility-profile
  binding (Silk.NET.OpenGL.Legacy), and no WGL/GLX/EGL bindings
  (Silk.NET.WGL etc.) - the core profile only.
- No image decoding, font rendering, scene graph, or shader tooling; it is
  a raw binding.
- No SIMD-accelerated Maths: the hand-rolled intrinsic paths from upstream
  are not enabled (Scalar.IsHardwareAccelerated reports the BCL state).
- No source generator and no public-API-diff discipline: the bindings are
  fixed at the ported core-profile surface.
- Not usable on a headless host for real GL calls: there is no software
  rasterizer or offscreen context provider inside the package.

WORKING EXAMPLES ON GITHUB
==========================
The repository test project exercises the parts that need no GL context:

    https://github.com/ellisnet/CodeBrix.Platform.OpenGL/tree/main/tests/CodeBrix.Platform.OpenGL.Tests

    Core/TestSilkMarshal.cs      SilkMarshal.StringToPtr / PtrToString /
                                 StringLength with every NativeStringEncoding,
                                 string arrays to and from native memory
    Maths/Vector2Tests.cs        Vector2D<float> construction, Dot, Distance,
    Maths/Vector3Tests.cs        Cross, Normalize, Lerp, Transform, casts
    Maths/Vector4Tests.cs
    Maths/Matrix4x4Tests.cs      Identity, determinant, Invert (translation,
                                 rotation, scale, projection, affine),
                                 CreateLookAt, CreatePerspective*, Decompose
    Maths/QuaternionTests.cs     Dot, Length, Lerp, Slerp, CreateFrom* and
                                 conversions
    Maths/PlaneTests.cs          Plane construction, Normalize, Dot variants
    Maths/ScalarTests.cs,        Scalar generic arithmetic incl. BigInteger,
    Maths/ExpTests.cs,           Exp / Log / integer Pow accuracy
    Maths/LogTests.cs,
    Maths/PowIntTests.cs,
    Maths/Scalar.Bitwise.cs      And/Or/Xor/Not/shift/rotate on Scalar
    OpenGL/GLSmokeTests.cs       GL type hierarchy and enum spec values
                                 (GLEnum.ColorBufferBit == 0x4000,
                                 PrimitiveType.Triangles == 4, ...)
    PortStatusTests.cs           PortStatus strings

No test issues real GL calls; there is no headless context on the test
host. The Triangle / debug / texture examples above are the canonical
consumer patterns.

QUICK REFERENCE CARD
====================
    // create
    GL gl = GL.GetApi(getProcAddress);            // Func<string, nint>
    GL gl = GL.GetApi(iglContext);                // IGLContext
    GL gl = contextSource.CreateOpenGL();         // IGLContextSource
    INativeContext c = GL.CreateDefaultContext(new[] { "libGL.so.1" });
    gl.Dispose();                                 // also disposes the context

    // buffers / VAO
    uint vbo = gl.GenBuffer();  gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
    gl.BufferData<float>(BufferTargetARB.ArrayBuffer, span, BufferUsageARB.StaticDraw);
    uint vao = gl.GenVertexArray();  gl.BindVertexArray(vao);
    gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, (nint)0);
    gl.EnableVertexAttribArray(0);

    // shaders
    uint s = gl.CreateShader(ShaderType.VertexShader);
    gl.ShaderSource(s, src);  gl.CompileShader(s);
    if (gl.GetShader(s, ShaderParameterName.CompileStatus) == 0) log = gl.GetShaderInfoLog(s);
    uint p = gl.CreateProgram();  gl.AttachShader(p, s);  gl.LinkProgram(p);
    if (gl.GetProgram(p, ProgramPropertyARB.LinkStatus) == 0) log = gl.GetProgramInfoLog(p);
    gl.UseProgram(p);  int loc = gl.GetUniformLocation(p, "uMvp");
    gl.UniformMatrix4(loc, 1, false, in mvp.Row1.X);
    gl.Uniform3(loc, vec3d.ToSystem());

    // textures
    uint t = gl.GenTexture();  gl.BindTexture(TextureTarget.Texture2D, t);
    gl.TexImage2D<byte>(TextureTarget.Texture2D, 0, InternalFormat.Rgba8, w, h, 0,
                        PixelFormat.Rgba, PixelType.UnsignedByte, bytes);
    gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

    // frame
    gl.Viewport(0, 0, w, h);
    gl.ClearColor(r, g, b, a);  gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    gl.DrawArrays(PrimitiveType.Triangles, 0, count);
    unsafe { gl.DrawElements(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, (void*)0); }

    // diagnostics
    GLEnum err = gl.GetError();                      // GLEnum.NoError
    string ver = gl.GetStringS(StringName.Version);
    gl.Enable(EnableCap.DebugOutput);  gl.DebugMessageCallback(staticDebugProc, null);

    // maths
    Matrix4X4<float> m = Matrix4X4.CreatePerspectiveFieldOfView(fov, aspect, near, far);
    Matrix4X4<float> v = Matrix4X4.CreateLookAt(eye, target, Vector3D<float>.UnitY);
    Vector3D<float> n = Vector3D.Normalize(v3);   float d = Vector3D.Dot(a, b);
    Quaternion<float> q = Quaternion<float>.CreateFromAxisAngle(axis, angle);
    Matrix4X4.Invert(m, out Matrix4X4<float> inv);   var sys = m.ToSystem();

    // native strings
    nint p = SilkMarshal.StringToPtr(s, NativeStringEncoding.UTF8);  SilkMarshal.FreeString(p, NativeStringEncoding.UTF8);
    string s = SilkMarshal.PtrToString(p, NativeStringEncoding.UTF8);

    // rules
    context current on the calling thread | one GL per context | keep DebugProc alive
    void* = offset for DrawElements, Span = client memory | ClearColor<T>(Vector4D) is 0..255
    AllowUnsafeBlocks for pointer overloads | no GL calls in headless tests

================================================================================
END OF AGENT-README
================================================================================
