using Xunit;
using CodeBrix.Platform.OpenGL.Maths;

namespace CodeBrix.Platform.OpenGL.Tests.Maths;

public class ScalarAbsTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(-5, 5)]
    [InlineData(-100, 100)]
    [InlineData(-127, 127)]
    [InlineData(42, 42)]
    public void abs_of_sbyte_is_the_magnitude(int value, int expected)
        => Assert.Equal((sbyte) expected, Scalar.Abs((sbyte) value));

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(-100, 100)]
    [InlineData(-32767, 32767)]
    [InlineData(1234, 1234)]
    public void abs_of_short_is_the_magnitude(int value, int expected)
        => Assert.Equal((short) expected, Scalar.Abs((short) value));

    [Theory]
    [InlineData(0, 0)]
    [InlineData(-1, 1)]
    [InlineData(-100, 100)]
    [InlineData(-123456789, 123456789)]
    [InlineData(int.MaxValue, int.MaxValue)]
    [InlineData(-int.MaxValue, int.MaxValue)]
    public void abs_of_int_is_the_magnitude(int value, int expected)
        => Assert.Equal(expected, Scalar.Abs(value));

    [Theory]
    [InlineData(0L, 0L)]
    [InlineData(-1L, 1L)]
    [InlineData(-100L, 100L)]
    [InlineData(-1234567890123L, 1234567890123L)]
    [InlineData(long.MaxValue, long.MaxValue)]
    [InlineData(-long.MaxValue, long.MaxValue)]
    public void abs_of_long_is_the_magnitude(long value, long expected)
        => Assert.Equal(expected, Scalar.Abs(value));

    [Fact]
    public void is_hardware_accelerated_is_false_because_no_member_has_a_hand_written_simd_path()
        => Assert.False(Scalar.IsHardwareAccelerated);
}
