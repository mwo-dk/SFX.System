using System;

namespace SFX.System.Infrastructure
{
    public sealed class Base64Service : IBase64Service
    {
        /// <inheritdoc/>
        public OperationResult<string> ToBase64String(byte[] data)
        {
            if (data is null)
                return new OperationResult<string>(new ArgumentNullException(nameof(data)), default);

            try
            {
                var result = Convert.ToBase64String(data);
                return new OperationResult<string>(default, result);
            }
            catch (Exception error)
            {
                return new OperationResult<string>(error, default);
            }
        }

        /// <inheritdoc/>
        public OperationResult<byte[]> FromBase64String(string data)
        {
            if (data is null || data.Length == 0)
                return new OperationResult<byte[]>(new ArgumentNullException(nameof(data)), default);

            try
            {
                var result = Convert.FromBase64String(data);
                return new OperationResult<byte[]>(default, result);
            }
            catch (Exception error)
            {
                return new OperationResult<byte[]>(error, default);
            }
        }
    }
}
