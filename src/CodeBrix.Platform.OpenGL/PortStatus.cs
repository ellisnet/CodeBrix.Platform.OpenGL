namespace CodeBrix.Platform.OpenGL;

/// <summary>
/// Marker class recording the provenance of the port from Silk.NET.OpenGL
/// v2.23.0 to CodeBrix.Platform.OpenGL. It gives the library package a
/// discoverable public surface, and ships as the port-provenance record.
/// </summary>
public static class PortStatus
{
    /// <summary>
    /// Gets the Silk.NET upstream version that this library was forked from.
    /// </summary>
    public static string UpstreamVersion => "Silk.NET v2.23.0";

    /// <summary>
    /// Gets the upstream Silk.NET git commit hash that this library was forked from.
    /// </summary>
    public static string UpstreamCommit => "94605142f7b7bd6e69c9201e8e721d245c69eb7e";

    /// <summary>
    /// Gets a human-readable short description of which portions of the upstream
    /// source were ported. Kept in step with THIRD-PARTY-NOTICES.txt by hand
    /// whenever the port is refreshed to a newer upstream version.
    /// </summary>
    public static string PortedComponents =>
        "Silk.NET.Core (125 files), Silk.NET.Maths (58 files), and Silk.NET.OpenGL " +
        "(327 source files + one ~151k-line SilkTouch-captured generated-bodies file) " +
        "all ported. Silk.NET.Core.Tests and Silk.NET.Maths.Tests ported to xUnit v3. " +
        "Silk.NET.OpenGL had no upstream xUnit project; OpenGL coverage is via " +
        "smoke tests that exercise types/enums without a live GL context.";
}
