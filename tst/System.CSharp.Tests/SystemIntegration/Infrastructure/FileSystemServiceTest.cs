using AutoFixture;
using SFX.System.Infrastructure;
using SFX.System.Model;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace SFX.System.Test.SystemIntegration.Infrastructure
{
    [Trait("Category", "SystemIntegration")]
    public class FileSystemServiceTests : IDisposable
    {
        #region Members
        private readonly Fixture _fixture;
        private static readonly string WorkingFolder = Path.Combine(Directory.GetCurrentDirectory(), "WorkingFolder");
        
        private static FolderPath TestFolder { get; } = new FolderPath { Value = Path.Combine(Directory.GetCurrentDirectory(), @"WorkingFolder\TestFolder") };
        #endregion

        #region Test initialization
        public FileSystemServiceTests()
        {
            _fixture = new Fixture().Customize(new SupportMutableValueTypesCustomization()) as Fixture;

            CreateWorkingFolder();
        }
        #endregion

        #region Folder existence tests
        [Fact]
        public void If_test_folder_is_not_created_FolderExists_returns_false()
        {
            var sut = new FileSystemService();

            var (success, error, result) = sut.FolderExists(TestFolder);

            Assert.True(success);
            Assert.Null(error);
            Assert.False(result);
        }

        [Fact]
        public void If_test_folder_is_created_FolderExists_returns_false()
        {
            var sut = new FileSystemService();
            CreateFolder(TestFolder.Value);

            var (success, error, result) = sut.FolderExists(TestFolder);

            Assert.True(success);
            Assert.Null(error);
            Assert.True(result);
        }
        #endregion

        #region Create folder tests
        [Fact]
        public void CreateFolder_creates_new_empty_folder()
        {
            var folderName = _fixture.Create<string>();
            var folderPath = new FolderPath { Value = Path.Combine(WorkingFolder, folderName) };
            var sut = new FileSystemService();

            sut.CreateFolder(folderPath);

            Assert.Contains(GetFolders(), folder => folder == folderPath.Value);
        }
        #endregion

        #region Clear folder tests
        [Fact]
        public void ClearFolder_removes_existing_folders()
        {
            var folderNames = _fixture.CreateMany<string>().ToArray();
            var folderPaths =
                folderNames.Select(folderName => new FolderPath { Value = Path.Combine(WorkingFolder, folderName) })
                    .ToArray();
            var sut = new FileSystemService();
            foreach (var folderPath in folderPaths)
                CreateFolder(folderPath.Value);

            sut.ClearFolder(new FolderPath { Value = WorkingFolder });

            Assert.True(!GetFolders().Any());
        }

        [Fact]
        public void ClearFolder_removes_existing_files()
        {
            var fileNames = _fixture.CreateMany<string>().ToArray();
            var filePaths =
                fileNames.Select(fileName => new FolderPath { Value = Path.Combine(WorkingFolder, fileName) })
                    .ToArray();
            var sut = new FileSystemService();
            foreach (var filePath in filePaths)
                CreateFile(filePath.Value, _fixture.Create<string>());

            sut.ClearFolder(new FolderPath { Value = WorkingFolder });

            Assert.True(!GetFolders().Any());
        }
        #endregion

        #region File existence tests
        [Fact]
        public void If_file_is_not_created_FileExists_return_false()
        {
            var sut = new FileSystemService();

            var (success, error, result) = sut.FileExists(new FilePath { Value = Path.Combine(WorkingFolder, _fixture.Create<string>()) });

            Assert.True(success);
            Assert.Null(error);
            Assert.False(result);
        }

        [Fact]
        public void If_file_is_created_FileExists_return_true()
        {
            var fileName = Path.Combine(WorkingFolder, _fixture.Create<string>());
            var sut = new FileSystemService();
            CreateFile(fileName, _fixture.Create<string>());

            var (success, error, result) = sut.FileExists(new FilePath { Value = fileName });

            Assert.True(success);
            Assert.Null(error);
            Assert.True(result);
        }
        #endregion

        #region File listing tests
        [Fact]
        public void GetFiles_returns_exact_list_of_files_in_folder()
        {
            var fileNames = _fixture.CreateMany<string>().ToArray();
            var filePaths =
                fileNames.Select(fileName => new FilePath { Value = Path.Combine(WorkingFolder, fileName) })
                    .ToArray();
            var sut = new FileSystemService();
            foreach (var filePath in filePaths)
                CreateFile(filePath.Value, _fixture.Create<string>());

            var (success, error, files_) = sut.GetFiles(new FolderPath { Value = WorkingFolder });

            Assert.True(success);
            Assert.Null(error);
            Assert.NotNull(files_);

            var files = files_.ToArray();

            Assert.Equal(fileNames.Length, files.Length);
            foreach (var file in files)
                Assert.Contains(filePaths, filePath => file.Equals(filePath));
        }
        #endregion

        #region Create file tests
        [Fact]
        public void CreateFile_creates_a_file_with_expected_content()
        {
            var fileName = Path.Combine(WorkingFolder, _fixture.Create<string>());
            var fileContent = _fixture.Create<string>();
            var sut = new FileSystemService();

            sut.CreateFile(new FilePath { Value = fileName }, fileContent);

            var content = ReadFile(fileName);

            Assert.Equal(fileContent, content);
        }
        #endregion

        #region Delete file tests
        [Fact]
        public void DeleteFile_works()
        {
            var fileName = Path.Combine(WorkingFolder, _fixture.Create<string>());
            var fileContent = _fixture.Create<string>();
            var sut = new FileSystemService();

            sut.CreateFile(new FilePath { Value = fileName }, fileContent);
            sut.DeleteFile(new FilePath { Value = fileName });

            Assert.False(sut.FileExists(new FilePath { Value = fileName }).Result);
        }
        #endregion

        #region Read file tests
        [Fact]
        public void ReadFile_returns_expected_content()
        {
            var fileName = Path.Combine(WorkingFolder, _fixture.Create<string>());
            var fileContent = _fixture.Create<string>();
            var sut = new FileSystemService();
            CreateFile(fileName, fileContent);

            sut.CreateFile(new FilePath { Value = fileName }, fileContent);

            var (success, error, content) = sut.ReadFileStringContent(new FilePath { Value = fileName });

            Assert.True(success);
            Assert.Null(error);
            Assert.Equal(fileContent, content);
        }
        #endregion

        #region Utility

        private static void CreateWorkingFolder()
        {
            CreateFolder(WorkingFolder);
        }
        private static void RemoveFilesAndFolders()
        {
            foreach (var folder in GetFolders())
                RemoveFolder(folder);
            foreach (var file in GetFiles())
                RemoveFile(file);
        }
        private static void RemoveWorkingFolder()
        {
            RemoveFolder(WorkingFolder);
        }

        private static string[] GetFolders() => Directory.GetDirectories(WorkingFolder);
        private static void CreateFolder(string path) => Directory.CreateDirectory(path);
        private static void RemoveFolder(string path) => Directory.Delete(path);
        private static string[] GetFiles() => Directory.GetFiles(WorkingFolder);

        private static string ReadFile(string path)
        {
            using (var file = File.OpenRead(path))
            {
                using (var streamReader = new StreamReader(file))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        private static StreamWriter CreateFile(string path) => File.CreateText(path);

        private static void CreateFile(string path, string content)
        {
            using (var streamWriter = CreateFile(path))
            {
                streamWriter.Write(content);
            }
        }
        private static void RemoveFile(string path) => File.Delete(path);

        #endregion

        public void Dispose()
        {
            RemoveFilesAndFolders();
            RemoveWorkingFolder();
        }
    }
}
