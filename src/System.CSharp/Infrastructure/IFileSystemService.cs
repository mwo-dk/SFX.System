using SFX.System.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Interface describing access to the local file system
    /// </summary>
    public interface IFileSystemService
    {
        /// <summary>
        /// Determines whether a given folder exists.
        /// </summary>
        /// <param name="folderPath">The <see cref="FolderPath"/> to the given folder</param>
        /// <returns>True if the folder exists. Else false</returns>
        (bool Success, string Error, bool Result) FolderExists(FolderPath folderPath);

        /// <summary>
        /// Creates a folder denoted by the given <paramref name="folderPath"/>
        /// </summary>
        /// <param name="folderPath">The <see cref="FolderPath"/> to the folder to be created</param>
        (bool Success, string Error) CreateFolder(FolderPath folderPath);

        /// <summary>
        /// Clears folder denoted by the given <paramref name="folderPath"/>
        /// </summary>
        /// <param name="folderPath">The <see cref="FolderPath"/> to the folder to be cleared</param>
        (bool Success, string Error) ClearFolder(FolderPath folderPath);

        /// <summary>
        /// Determines whether a given file exists.
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> to the given file</param>
        /// <returns>True if the file exists. Else false</returns>
        (bool Success, string Error, bool Result) FileExists(FilePath filePath);

        /// <summary>
        /// Gets a list of files in the given <paramref name="folderPath"/>
        /// </summary>
        /// <param name="folderPath">The <see cref="FolderPath"/> to the given folder</param>
        /// <returns>A sequence of <see cref="FilePath"/>s to the files in <paramref name="folderPath"/></returns>
        (bool Success, string Error, IEnumerable<FilePath> Result) GetFiles(FolderPath folderPath);

        /// <summary>
        /// Creates a file denoted by the given <paramref name="filePath"/> and writes the content <paramref name="content"/> to it
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> to the file to be created</param>
        /// <param name="content">The content to be written to the new file</param>
        (bool Success, string Error) CreateFile(FilePath filePath, byte[] content);

        /// <summary>
        /// Creates a file denoted by the given <paramref name="filePath"/> and writes the content <paramref name="content"/> to it
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> to the file to be created</param>
        /// <param name="content">The content to be written to the new file</param>
        (bool Success, string Error) CreateFile(FilePath filePath, string content);

        /// <summary>
        /// Creates a file denoted by the given <paramref name="filePath"/> and writes the content <paramref name="content"/> to it
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> to the file to be created</param>
        /// <param name="content">The content to be written to the new file</param>
        Task<(bool Success, string Error)> CreateFileAsync(FilePath filePath, byte[] content);

        /// <summary>
        /// Creates a file denoted by the given <paramref name="filePath"/> and writes the content <paramref name="content"/> to it
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> to the file to be created</param>
        /// <param name="content">The content to be written to the new file</param>
        Task<(bool Success, string Error)> CreateFileAsync(FilePath filePath, string content);

        /// <summary>
        ///  Creates a file denoted by the given <paramref name="filePath"/> 
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> to the file to be deleted</param>
        (bool Success, string Error) DeleteFile(FilePath filePath);

        /// <summary>
        /// Reads the content of the file denoted by <paramref name="filePath"/>
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> of the file whose binary content is to be read</param>
        /// <returns>The binary content of the file denoted by <paramref name="filePath"/></returns>
        (bool Success, string Error, byte[] Result) ReadFileBinaryContent(FilePath filePath);

        /// <summary>
        /// Reads the content of the file denoted by <paramref name="filePath"/>
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> of the file whose <see cref="string"/> content is to be read</param>
        /// <returns>The <see cref="string"/> content of the file denoted by <paramref name="filePath"/></returns>
        (bool Success, string Error, string Result) ReadFileStringContent(FilePath filePath);

        /// <summary>
        /// Reads the content of the file denoted by <paramref name="filePath"/>
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> of the file whose binary content is to be read</param>
        /// <returns>The binary content of the file denoted by <paramref name="filePath"/></returns>
        Task<(bool Success, string Error, byte[] Result)> ReadFileBinaryContentAsync(FilePath filePath);

        /// <summary>
        /// Reads the content of the file denoted by <paramref name="filePath"/>
        /// </summary>
        /// <param name="filePath">The <see cref="FilePath"/> of the file whose <see cref="string"/> content is to be read</param>
        /// <returns>The <see cref="string"/> content of the file denoted by <paramref name="filePath"/></returns>
        Task<(bool Success, string Error, string Result)> ReadFileStringContentAsync(FilePath filePath);
    }
}
