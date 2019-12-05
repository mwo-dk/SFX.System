using System.Security;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Provides conversion between <see cref="string"/> and <see cref="SecureString"/>
    /// </summary>
    public interface ISecureStringService
    {
        /// <summary>
        /// Convers <paramref name="input"/> to a <see cref="SecureString"/>
        /// </summary>
        /// <param name="input">The <see cref="string"/> that needs to be converted</param>
        /// <returns><paramref name="input"/> as a <see cref="SecureString"/></returns>
        OperationResult<SecureString> ToSecureString(string input);

        /// <summary>
        /// Converts <paramref name="input"/> to a <see cref="string"/>
        /// </summary>
        /// <param name="input">The <see cref="SecureString"/> that needs to be converted</param>
        /// <returns><paramref name="input"/> as a <see cref="string"/></returns>
        OperationResult<string> ToInsecureString(SecureString input);
    }
}
