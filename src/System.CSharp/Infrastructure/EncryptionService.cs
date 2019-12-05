using SFX.System.Model;
using System;
using System.Security;
using System.Text;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Implementation of <see cref="IEncryptionService"/>
    /// </summary>
    public sealed class EncryptionService : IEncryptionService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="secureStringService"><see cref="ISecureStringService"/> utilized for handling <see cref="SecureString"/>s</param>
        public EncryptionService(ISecureStringService secureStringService) =>
            SecureStringService = secureStringService ?? throw new ArgumentNullException(nameof(secureStringService));

        /// <summary>
        /// The <see cref="ISecureStringService"/> utlized to secure strings
        /// </summary>
        internal ISecureStringService SecureStringService { get; }

        /// <inheritdoc/>
        public OperationResult<byte[]> Encrypt(byte[] input, Salt salt)
        {
            if (input is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(input)), default);
            if (salt is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(salt)), default);
            if (!salt.IsValid())
                return new OperationResult<byte[]>(new ArgumentException("Salt is not valid"), default);

            try
            {
                byte[] encryptedData = global::System.Security.Cryptography.ProtectedData.Protect(
                    input, salt.Value,
                    global::System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return new OperationResult<byte[]>(default, encryptedData);
            }
            catch (Exception exn)
            {
                return new OperationResult<byte[]>(exn, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<byte[]> EncryptString(string input, Salt salt)
        {
            if (input is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(input)), default);
            if (salt is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(salt)), default);
            if (!salt.IsValid())
                return new OperationResult<byte[]>(new ArgumentException("Salt is not valid"), default);

            try
            {
                byte[] encryptedData = global::System.Security.Cryptography.ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(input), salt.Value,
                    global::System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return new OperationResult<byte[]>(default, encryptedData);
            }
            catch (Exception exn)
            {
                return new OperationResult<byte[]>(exn, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<byte[]> EncryptSecureString(SecureString input, Salt salt)
        {
            if (input is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(input)), default);
            if (salt is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(salt)), default);
            if (!salt.IsValid())
                return new OperationResult<byte[]>(new ArgumentException("Salt is not valid"), default);

            var (success, error, str) =
                SecureStringService.ToInsecureString(input);
            if (!success)
                return new OperationResult<byte[]>(error, default);

            try
            {
                byte[] encryptedData = global::System.Security.Cryptography.ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(str), salt.Value,
                    global::System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return new OperationResult<byte[]>(default, encryptedData);
            }
            catch (Exception exn)
            {
                return new OperationResult<byte[]>(exn, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<byte[]> Decrypt(byte[] encryptedData, Salt salt)
        {
            if (encryptedData is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(encryptedData)), default);
            if (salt is null)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(salt)), default);
            if (!salt.IsValid())
                return new OperationResult<byte[]>(new ArgumentException("Salt is not valid"), default);

            try
            {
                byte[] decryptedData =
                    global::System.Security.Cryptography.ProtectedData.Unprotect(
                        encryptedData,
                        salt.Value,
                        global::System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return new OperationResult<byte[]>(default, decryptedData);
            }
            catch (Exception exn)
            {
                return new OperationResult<byte[]>(exn, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<string> DecryptString(byte[] encryptedData, Salt salt)
        {
            if (encryptedData is null)
                return new OperationResult<string>(new ArgumentNullException(nameof(encryptedData)), default);
            if (encryptedData.Length == 0)
                return new OperationResult<string>(new ArgumentException("Unable to decrypt empty data"), default);
            if (salt is null)
                return new OperationResult<string>(new ArgumentNullException(nameof(salt)), default);
            if (!salt.IsValid())
                return new OperationResult<string>(new ArgumentException("Salt is not valid"), default);

            try
            {
                byte[] decryptedData =
                    global::System.Security.Cryptography.ProtectedData.Unprotect(
                        encryptedData,
                        salt.Value,
                        global::System.Security.Cryptography.DataProtectionScope.CurrentUser);
                var result = Encoding.Unicode.GetString(decryptedData);
                return new OperationResult<string>(default, result);
            }
            catch (Exception exn)
            {
                return new OperationResult<string>(exn, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<SecureString> DecryptSecureString(byte[] encryptedData, Salt salt)
        {
            if (encryptedData is null)
                return new OperationResult<SecureString>(new ArgumentNullException(nameof(encryptedData)), default);
            if (encryptedData.Length == 0)
                return new OperationResult<SecureString>(new ArgumentException("Unable to decrypt empty data"), default);
            if (salt is null)
                return new OperationResult<SecureString>(new ArgumentNullException(nameof(salt)), default);
            if (!salt.IsValid())
                return new OperationResult<SecureString>(new ArgumentException("Salt is not valid"), default);

            try
            {
                byte[] decryptedData =
                    global::System.Security.Cryptography.ProtectedData.Unprotect(
                        encryptedData,
                        salt.Value,
                        global::System.Security.Cryptography.DataProtectionScope.CurrentUser);
                var (success, error, result) =
                    SecureStringService.ToSecureString(Encoding.Unicode.GetString(decryptedData));
                if (!success)
                    return new OperationResult<SecureString>(error, default);
                return new OperationResult<SecureString>(default, result);
            }
            catch (Exception exn)
            {
                return new OperationResult<SecureString>(exn, default);
            }
        }
    }
}
