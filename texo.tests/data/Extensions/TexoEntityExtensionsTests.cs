using System.Diagnostics.CodeAnalysis;
using texo.data.Extensions;
using texo.domain.Entities;
using Xunit;

namespace texo.tests.data.Extensions
{
    [ExcludeFromCodeCoverage]
    public class TexoEntityExtensionsTests
    {
        [Fact]
        public void TestGetFieldNames()
        {
            var movie = new Movie();
            var fields = movie.GetFieldNames();
            Assert.Equal(new[] { "id", "release", "title", "studios", "producers", "winner" }, fields);
        }

        [Fact]
        public void TestGetFieldNamesNoChange()
        {
            var movie = new Movie();
            var fields = movie.GetFieldNames(noSnakeCase: true);
            Assert.Equal(new[] { "Id", "Release", "Title", "Studios", "Producers", "Winner" }, fields);
        }

        [Theory]
        [InlineData("Release", "release")]
        [InlineData("Studios", "studios")]
        public void TestSnakeCase(string text, string expected)
        {
            Assert.Equal(expected, text.GetAsSnakeCase());
        }
    }
}