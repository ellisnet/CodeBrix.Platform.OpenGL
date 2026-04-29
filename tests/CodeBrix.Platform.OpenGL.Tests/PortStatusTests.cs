using CodeBrix.Platform.OpenGL;
using SilverAssertions;
using Xunit;

namespace CodeBrix.Platform.OpenGL.Tests;

public class PortStatusTests
{
    [Fact]
    public void UpstreamVersion_reports_v2_23_0()
        => PortStatus.UpstreamVersion.Should().Be("Silk.NET v2.23.0");

    [Fact]
    public void UpstreamCommit_is_the_v2_23_0_tag_commit()
        => PortStatus.UpstreamCommit.Should().Be("94605142f7b7bd6e69c9201e8e721d245c69eb7e");

    [Fact]
    public void PortedSoFar_is_populated()
        => PortStatus.PortedSoFar.Should().NotBeNullOrWhiteSpace();
}
