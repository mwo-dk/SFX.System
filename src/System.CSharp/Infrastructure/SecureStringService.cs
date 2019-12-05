using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Provides conversion between <see cref="string"/> and <see cref="SecureString"/>
    /// </summary>
    public sealed class SecureStringService : ISecureStringService
    {
        /// <inheritdoc/>
        public OperationResult<SecureString> ToSecureString(string input)
        {
            if (input is null)
                return new OperationResult<SecureString>(new ArgumentNullException(nameof(input)), default);

            try
            {
                SecureString secure = new SecureString();
                foreach (char c in input)
                {
                    secure.AppendChar(c);
                }
                secure.MakeReadOnly();
                return new OperationResult<SecureString>(default, secure);
            }
            catch (Exception error)
            {
                return new OperationResult<SecureString>(error, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<string> ToInsecureString(SecureString input)
        {
            if (input is null)
                return new OperationResult<string>(new ArgumentNullException(nameof(input)), default);

            IntPtr ptr = default;
            string result = default;
            bool errorOccurred = false;
            Exception error = default;
            try
            {
                ptr = Marshal.SecureStringToBSTR(input);
                result = Marshal.PtrToStringBSTR(ptr);
            }
            catch (Exception exn)
            {
                errorOccurred = true;
                error = exn;
            }
            finally
            {
                if (ptr != default)
                    try
                    {
                        Marshal.ZeroFreeBSTR(ptr);
                    }
                    catch (Exception innerError)
                    {
                        errorOccurred = true;
                        error = innerError;
                    }
            }

            return new OperationResult<string>(error, errorOccurred ? default : result);
        }
    }
}
