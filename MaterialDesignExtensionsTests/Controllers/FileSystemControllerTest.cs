using System;
using System.Linq;

using Xunit;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensionsTests.Controllers
{
    [Trait("Category", "SkipCI")]
    public class FileSystemControllerTest
    {
        private readonly string m_directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        [Fact]
        public void TestBuildFullFileNameForInCurrentDirectory()
        {
            FileSystemController fileSystemController = new FileSystemController()
            {
                ForceFileExtensionOfFileFilter = true,
                FileFilterToApply = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml")
            };

            fileSystemController.SelectDirectory(m_directory);

            string filename = fileSystemController.BuildFullFileNameForInCurrentDirectory("test.xaml");

            Assert.Equal($@"{m_directory}\test.xaml", filename);
        }

        [Fact]
        public void TestBuildFullFileNameForInCurrentDirectoryWithForcedFileExtension()
        {
            FileSystemController fileSystemController = new FileSystemController()
            {
                ForceFileExtensionOfFileFilter = true,
                FileFilterToApply = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml")
            };

            fileSystemController.SelectDirectory(m_directory);

            string filename = fileSystemController.BuildFullFileNameForInCurrentDirectory("test.txt");

            Assert.Equal($@"{m_directory}\test.txt.cs", filename);
        }

        [Fact]
        public void TestBuildFullFileNameForInCurrentDirectoryWithoutForcedFileExtension()
        {
            FileSystemController fileSystemController = new FileSystemController()
            {
                ForceFileExtensionOfFileFilter = false,
                FileFilterToApply = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml")
            };

            fileSystemController.SelectDirectory(m_directory);

            string filename = fileSystemController.BuildFullFileNameForInCurrentDirectory("test.txt");

            Assert.Equal($@"{m_directory}\test.txt", filename);
        }

        [Fact]
        public void TestSelectOrRemoveDirectoryForMultipleSelection()
        {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string myMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            FileSystemController fileSystemController = new FileSystemController();

            Assert.False(fileSystemController.SelectedDirectories.Any());

            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myDocuments);
            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myPictures);
            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myMusic);

            Assert.Contains(fileSystemController.SelectedDirectories, directory => directory.FullName == myDocuments);
            Assert.Contains(fileSystemController.SelectedDirectories, directory => directory.FullName == myPictures);
            Assert.Contains(fileSystemController.SelectedDirectories, directory => directory.FullName == myMusic);

            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myMusic);

            Assert.Contains(fileSystemController.SelectedDirectories, directory => directory.FullName == myDocuments);
            Assert.Contains(fileSystemController.SelectedDirectories, directory => directory.FullName == myPictures);
            Assert.DoesNotContain(fileSystemController.SelectedDirectories, directory => directory.FullName == myMusic);
        }

        [Fact]
        public void TestSelectOrRemoveFileForMultipleSelection()
        {
            string gitconfigFile = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\.gitconfig";

            FileSystemController fileSystemController = new FileSystemController();

            Assert.False(fileSystemController.SelectedFiles.Any());

            fileSystemController.SelectOrRemoveFileForMultipleSelection(gitconfigFile);

            Assert.Contains(fileSystemController.SelectedFiles, file => file.FullName == gitconfigFile);

            fileSystemController.SelectOrRemoveFileForMultipleSelection(gitconfigFile);

            Assert.DoesNotContain(fileSystemController.SelectedFiles, file => file.FullName == gitconfigFile);
        }
    }
}
