using SFX.System.Infrastructure;
using Xunit;

namespace SFX.System.Test.Unit.Infrastructure
{
    [Trait("Category", "Unit")]
    public class FileSystemServiceTest
    {
        #region Type tests
        [Fact]
        public void FileSystemService_implements_IFileSystemService()
        {
            Assert.True(typeof(IFileSystemService).IsAssignableFrom(typeof(FileSystemService)));
        }

        [Fact]
        public void FileSystemService_is_sealed()
        {
            Assert.True(typeof(FileSystemService).IsSealed);
        }
        #endregion
    }
}
