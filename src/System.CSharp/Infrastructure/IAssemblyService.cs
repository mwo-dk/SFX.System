using SFX.ROP.CSharp;
using SFX.System.Model;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Describes access to information of a given file
    /// </summary>
    public interface IAssemblyService
    {
        /// <summary>
        /// Returns the full path to a running exe file
        /// </summary>
        /// <returns>The path to a running exe</returns>
        Result<FilePath> GetExeFilePath();
    }
}
