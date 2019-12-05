using AutoFixture;
using FakeItEasy;
using SFX.System.Infrastructure;
using SFX.System.Model;
using System;
using System.Security;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using static FakeItEasy.A;

namespace SFX.System.Test.Unit.Infrastructure
{
    [Trait("Category", "Unit")]
    public class EncryptionServiceTest
    {
        #region Members
        private readonly Fixture _fixture;
        private readonly Salt _salt;
        private readonly ITestOutputHelper _output;
        #endregion

        #region Test initialization
        public EncryptionServiceTest(ITestOutputHelper output)
        {
            _fixture = new Fixture();

            _salt = new Salt();
            _salt.SetStringValue("Salt is not a password");

            _output = output;
        }
        #endregion

        #region Type test
        [Fact]
        public void EncryptionService_implements_IEncryptionService()
        {
            Assert.True(typeof(IEncryptionService).IsAssignableFrom(typeof(EncryptionService)));
        }

        [Fact]
        public void EncryptionService_is_sealed()
        {
            Assert.True(typeof(EncryptionService).IsSealed);
        }
        #endregion

        #region Initialization test
        [Fact]
        public void Initializing_EncryptionService_with_null_SecureStringService_throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EncryptionService(null));
        }

        [Fact]
        public void Initializing_EncryptionService_sets_SecureStringService_property()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            Assert.Same(secureStringService, sut.SecureStringService);
        }
        #endregion

        #region EncryptString
        [Fact]
        public void Encrypt_null_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.Encrypt(null, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void Encrypt_invalid_salt_fails()
        {
            var message = "Hello world";
            var data = Encoding.Unicode.GetBytes(message);
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.Encrypt(data, new Salt());

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }
        #endregion

        #region Decrypt
        [Fact]
        public void Decrypt_null_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.Decrypt(null, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void Decrypt_empty_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.Decrypt(new byte[] { }, _salt);

            Assert.False(success);
            Assert.NotNull(error);
            Assert.Null(result);
        }

        [Fact]
        public void Decrypt_invalid_salt_fails()
        {
            var message = "Hello world";
            var secureStringService = new SecureStringService();
            var sut = new EncryptionService(secureStringService);
            var (_, _, str_) = sut.EncryptString(message, _salt);

            var (success, error, result) = sut.Decrypt(str_, new Salt());

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }
        #endregion

        #region EncryptString
        [Fact]
        public void EncryptString_null_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.EncryptString(null, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void EncryptString_invalid_salt_fails()
        {
            var message = "Hello world";
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.EncryptString(message, new Salt());

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }
        #endregion

        #region DecryptString
        [Fact]
        public void DecryptString_null_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.DecryptString(null, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void DecryptString_empty_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.DecryptString(new byte[] { }, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void DecryptString_invalid_salt_fails()
        {
            var message = "Hello world";
            var secureStringService = new SecureStringService();
            var sut = new EncryptionService(secureStringService);
            var (_, _, str_) = sut.EncryptString(message, _salt);

            var (success, error, result) = sut.DecryptString(str_, new Salt());

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }
        #endregion

        #region EncryptSecureString
        [Fact]
        public void EncryptSecureString_null_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.EncryptSecureString(null, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void EncryptSecureString_invalid_salt_fails()
        {
            var message = "Hello world";
            var secureStringService = new SecureStringService();
            var (_, _, str) = secureStringService.ToSecureString(message);
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.EncryptSecureString(str, new Salt());

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void EncryptSecureString_if_SecureStringService_fails_error_is_propagated()
        {
            var message = "Hello world";
            var _secureStringService = new SecureStringService();
            var (_, _, str) = _secureStringService.ToSecureString(message);
            var secureStringService = Fake<ISecureStringService>();
            var error_ = _fixture.Create<Exception>();
            CallTo(() => secureStringService.ToInsecureString(str))
                .Returns(new OperationResult<string>(error_, default));
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.EncryptSecureString(str, _salt);

            Assert.False(success);
            Assert.Same(error_, error);
            Assert.Null(result);
        }
        #endregion

        #region DecryptSecureString
        [Fact]
        public void DecryptSecureString_null_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.DecryptSecureString(null, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentNullException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void DecryptSecureString_empty_fails()
        {
            var secureStringService = Fake<ISecureStringService>();
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.DecryptSecureString(new byte[] { }, _salt);

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void DecryptSecureString_invalid_salt_fails()
        {
            var message = "Hello world";
            var secureStringService = new SecureStringService();
            var (_, _, str) = secureStringService.ToSecureString(message);
            var sut = new EncryptionService(secureStringService);
            var (_, _, str_) = sut.EncryptSecureString(str, _salt);

            var (success, error, result) = sut.DecryptSecureString(str_, new Salt());

            Assert.False(success);
            Assert.IsAssignableFrom<ArgumentException>(error);
            Assert.Null(result);
        }

        [Fact]
        public void DecryptSecureString_if_SecureStringService_fails_error_is_propagated()
        {
            var message = "Hello world";
            var _secureStringService = new SecureStringService();
            var (ok1, err1, str) = _secureStringService.ToSecureString(message);
            Assert.True(ok1);
            Assert.Null(err1);
            Assert.NotNull(str);
            var (ok2, err2, data) = (new EncryptionService(_secureStringService))
                .EncryptSecureString(str, _salt);
            _output.WriteLine("Result of encrypt. Ok: {0}. Error: {1}. Data: {2}",
                ok2, err2, data);
            Assert.True(ok2);
            Assert.Null(err2);
            Assert.NotNull(data);
            var secureStringService = Fake<ISecureStringService>();
            var error_ = _fixture.Create<Exception>();
            CallTo(() => secureStringService.ToSecureString(message))
                .Returns(new OperationResult<SecureString>(error_, default));
            var sut = new EncryptionService(secureStringService);

            var (success, error, result) = sut.DecryptSecureString(data, _salt);

            Assert.False(success);
            Assert.Same(error_, error);
            Assert.Null(result);
        }
        #endregion
    }
}
