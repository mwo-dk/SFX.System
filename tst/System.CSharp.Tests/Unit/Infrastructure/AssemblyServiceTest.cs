using SFX.System.Infrastructure;
using Xunit;

namespace SFX.System.Test.Unit.Infrastructure
{
    [Trait("Category", "Unit")]
    public class AssemblyServiceTest
    {
        #region Type test
        [Fact]
        public void AssemblyService_implements_IAssemblyService()
        {
            Assert.True(typeof(IAssemblyService).IsAssignableFrom(typeof(AssemblyService)));
        }

        [Fact]
        public void AssemblyService_is_sealed()
        {
            Assert.True(typeof(AssemblyService).IsSealed);
        }
        #endregion
    }
}
