using System;
using SFX.System.Infrastructure;
using Xunit;

namespace SFX.System.Test.Unit.Infrastructure
{
    [Trait("Category", "Unit")]
    public class SecureStringServiceTest
    {
        #region Type test
        [Fact]
        public void SecureStringService_implements_ISecureStringService()
        {
            Assert.True(typeof(ISecureStringService).IsAssignableFrom(typeof(SecureStringService)));
        }

        [Fact]
        public void SecureStringService_is_sealed()
        {
            Assert.True(typeof(SecureStringService).IsSealed);
        }
        #endregion

        #region ToSecureString
        [Fact]
        public void ToSecureString_null_works()
        {
            var sut = new SecureStringService();

            var (success, error, result) = sut.ToSecureString(null);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void ToSecureString_works()
        {
            var sut = new SecureStringService();

            var (success, error, result) = sut.ToSecureString("Hello world");

            Assert.True(success);
            Assert.Null(error);
            Assert.NotNull(result);
        }
        #endregion

        #region ToInsecureString
        [Fact]
        public void ToInsecureString_null_works()
        {
            var sut = new SecureStringService();

            var (success, error, result) = sut.ToInsecureString(null);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void ToInsecureString_works()
        {
            var sut = new SecureStringService();

            var (_, _, secureString) = sut.ToSecureString("Hello world");
            var (success, error, result) = sut.ToInsecureString(secureString);

            Assert.True(success);
            Assert.Null(error);
            Assert.NotNull(result);
            Assert.Equal("Hello world", result);
        }
        #endregion
    }
}
