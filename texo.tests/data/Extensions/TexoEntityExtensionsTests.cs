using System.Diagnostics.CodeAnalysis;
using texo.data.Extensions;
using texo.domain.Entities;
using Xunit;

namespace texo.tests.data
{
    [ExcludeFromCodeCoverage]
    public class TexoEntityExtensionsTests
    {
        [Fact]
        public void TestGetFieldNames()
        {
            var movie = new Movie();
            var fields = movie.GetFieldNames();
            Assert.Equal(new[] { "id", "release_year", "title", "studios", "producers", "winner" }, fields);
        }

        [Fact]
        public void TestGetFieldNamesNoChange()
        {
            var movie = new Movie();
            var fields = movie.GetFieldNames(noSnakeCase: true);
            Assert.Equal(new[] { "Id", "ReleaseYear", "Title", "Studios", "Producers", "Winner" }, fields);
        }

        [Theory]
        [InlineData("ReleaseYear", "release_year")]
        [InlineData("Studios", "studios")]
        public void TestSnakeCase(string text, string expected)
        {
            Assert.Equal(expected, text.GetAsSnakeCase());
        }
    }
}