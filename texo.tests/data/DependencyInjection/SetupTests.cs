using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using texo.data.DependencyInjection;


namespace texo.tests.data.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public class SetupTests
    {
        [Fact]
        public void TestSetup()
        {
            var services = new ServiceCollection();
            services.AddDataServices();
            Assert.Equal(9, services.Count);
        }
    }
}