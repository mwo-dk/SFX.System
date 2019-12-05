using SFX.System.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SFX.System.Infrastructure
{
    /// <summary>
    /// Implements <see cref="IFileSystemService"/>
    /// </summary>
    public sealed class FileSystemService : IFileSystemService
    {
        /// <inheritdoc/>
        public (bool Success, string Error, bool Result) FolderExists(FolderPath folderPath)
        {
            try
            {
                return (true, default, Directory.Exists(folderPath.Value));
            }
            catch (Exception exn)
            {
                return (false, exn.Message, false);
            }
        }


        /// <inheritdoc/>
        public (bool Success, string Error) CreateFolder(FolderPath folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath.Value);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error) ClearFolder(FolderPath folderPath)
        {
            try
            {
                foreach (var folder in Directory.GetDirectories(folderPath.Value))
                    Directory.Delete(folder, true);
                foreach (var file in Directory.GetFiles(folderPath.Value))
                    File.Delete(file);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error, bool Result) FileExists(FilePath filePath)
        {
            try
            {
                return (true, default, File.Exists(filePath.Value));
            }
            catch (Exception exn)
            {
                return (false, exn.Message, false);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error, IEnumerable<FilePath> Result) GetFiles(FolderPath folderPath)
        {
            try
            {
                var result = Directory.GetFiles(folderPath.Value)
                    .Select(filePath => new FilePath { Value = filePath });
                return (true, default, result);
            }
            catch (Exception exn)
            {
                return (false, exn.Message, default);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error) CreateFile(FilePath filePath, byte[] content)
        {
            if (content is null || content.Length == 0)
                return (false, "No content provided");

            try
            {
                using (var stream = File.Create(filePath.Value))
                    stream.Write(content, 0, content.Length);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error) CreateFile(FilePath filePath, string content)
        {
            if (string.IsNullOrEmpty(content))
                return (false, "No content provided");

            try
            {
                using (var stream = File.CreateText(filePath.Value))
                    stream.Write(content);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Error)> CreateFileAsync(FilePath filePath, byte[] content)
        {
            if (content is null || content.Length == 0)
                return (false, "No content provided");

            try
            {
                using (var stream = File.Create(filePath.Value))
                    await stream.WriteAsync(content, 0, content.Length).ConfigureAwait(false);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Error)> CreateFileAsync(FilePath filePath, string content)
        {
            if (string.IsNullOrEmpty(content))
                return (false, "No content provided");

            try
            {
                using (var stream = File.CreateText(filePath.Value))
                    await stream.WriteAsync(content).ConfigureAwait(false);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error) DeleteFile(FilePath filePath)
        {
            try
            {
                if (!File.Exists(filePath.Value))
                    return (false, $"File \"{filePath}\" not found");
                File.Delete(filePath.Value);
                return (true, default);
            }
            catch (Exception exn)
            {
                return (false, exn.Message);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error, byte[] Result) ReadFileBinaryContent(FilePath filePath)
        {
            var (success, error, result) = FileExists(filePath);
            if (!success)
                return (false, error, default);
            if (!result)
                return (false, $"File \"{filePath}\" not found", default);

            try
            {
                var info = new FileInfo(filePath.Value);
                var size = info.Length;
                var data = new byte[size];
                var count = 0L;
                var bufferSize = Int32.MaxValue < size ? (1 << 20) : ((int)size);
                byte[] buffer = new byte[bufferSize];

                using (var file = File.OpenRead(filePath.Value))
                {
                    while (count < size)
                    {
                        var n = file.Read(buffer, 0, bufferSize);
                        Array.Copy(buffer, 0, data, count, n);
                        count += n;
                    }

                    return (true, default, data);
                }
            }
            catch (Exception exn)
            {
                return (false, exn.Message, default);
            }
        }

        /// <inheritdoc/>
        public (bool Success, string Error, string Result) ReadFileStringContent(FilePath filePath)
        {
            var (success, error, result) = FileExists(filePath);
            if (!success)
                return (false, error, default);
            if (!result)
                return (false, $"File \"{filePath}\" not found", default);

            try
            {
                using (var file = File.OpenRead(filePath.Value))
                {
                    using (var streamReader = new StreamReader(file))
                    {
                        return (true, default, streamReader.ReadToEnd());
                    }
                }
            }
            catch (Exception exn)
            {
                return (false, exn.Message, default);
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Error, byte[] Result)> ReadFileBinaryContentAsync(FilePath filePath)
        {
            var (success, error, result) = FileExists(filePath);
            if (!success)
                return (false, error, default);
            if (!result)
                return (false, $"File \"{filePath}\" not found", default);

            try
            {
                var info = new FileInfo(filePath.Value);
                var size = info.Length;
                var data = new byte[size];
                var count = 0L;
                var bufferSize = Int32.MaxValue < size ? (1 << 20) : ((int)size);
                byte[] buffer = new byte[bufferSize];

                using (var file = File.OpenRead(filePath.Value))
                {
                    while (count < size)
                    {
                        var n = await file.ReadAsync(buffer, 0, bufferSize)
                            .ConfigureAwait(false);
                        Array.Copy(buffer, 0, data, count, n);
                        count += n;
                    }

                    return (true, default, data);
                }
            }
            catch (Exception exn)
            {
                return (false, exn.Message, default);
            }
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Error, string Result)> ReadFileStringContentAsync(FilePath filePath)
        {
            var (success, error, result) = FileExists(filePath);
            if (!success)
                return (false, error, default);
            if (!result)
                return (false, $"File \"{filePath}\" not found", default);

            try
            {
                using (var file = File.OpenRead(filePath.Value))
                {
                    using (var streamReader = new StreamReader(file))
                    {
                        return (true, default, await streamReader.ReadToEndAsync()
                            .ConfigureAwait(false));
                    }
                }
            }
            catch (Exception exn)
            {
                return (false, exn.Message, default);
            }
        }
    }
}
