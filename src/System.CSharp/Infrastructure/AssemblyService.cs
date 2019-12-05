using SFX.System.Model;
using System;
using System.IO;
using System.Reflection;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Implementation of <see cref="IAssemblyService"/>
    /// </summary>
    public sealed class AssemblyService : IAssemblyService
    {
        /// <inheritdoc/>
        public OperationResult<FilePath> GetExeFilePath()
        {
            try
            {
                var entry = Assembly.GetEntryAssembly();
                var codebase = entry.CodeBase;
                var path = new Uri(codebase).AbsolutePath;
                var cleanPath = Uri.UnescapeDataString(path);
                return new OperationResult<FilePath>(default, new FilePath { Value = Path.GetFullPath(cleanPath) });
            }
            catch (Exception error)
            {
                return new OperationResult<FilePath>(error, default);
            }
        }
    }
}
