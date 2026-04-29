using System;
using CodeBrix.Platform.OpenGL;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.OpenGL.Tests.OpenGL;

public class GLSmokeTests
{
    [Fact]
    public void GL_type_is_a_public_class()
        => typeof(GL).IsClass.Should().BeTrue();

    [Fact]
    public void GL_inherits_from_NativeAPI()
        => typeof(GL).BaseType.Should().Be(typeof(CodeBrix.Platform.OpenGL.Core.Native.NativeAPI));

    [Fact]
    public void GLEnum_type_is_a_public_enum()
        => typeof(GLEnum).IsEnum.Should().BeTrue();

    [Fact]
    public void GLEnum_has_ColorBufferBit()
        => Enum.IsDefined(typeof(GLEnum), GLEnum.ColorBufferBit).Should().BeTrue();

    [Fact]
    public void GLEnum_ColorBufferBit_value_matches_OpenGL_spec()
        => ((uint)GLEnum.ColorBufferBit).Should().Be(0x4000u);

    [Fact]
    public void PrimitiveType_Triangles_matches_OpenGL_spec()
        => ((uint)PrimitiveType.Triangles).Should().Be(0x0004u);

    [Fact]
    public void DrawElementsType_UnsignedInt_matches_OpenGL_spec()
        => ((uint)DrawElementsType.UnsignedInt).Should().Be(0x1405u);
}
