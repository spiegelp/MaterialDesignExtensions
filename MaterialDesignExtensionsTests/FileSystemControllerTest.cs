using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensionsTests
{
    [TestClass]
    public class FileSystemControllerTest
    {
        private readonly string m_directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        [TestMethod]
        public void TestBuildFullFileNameForInCurrentDirectory()
        {
            FileSystemController fileSystemController = new FileSystemController()
            {
                ForceFileExtensionOfFileFilter = true,
                FileFilterToApply = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml")
            };

            fileSystemController.SelectDirectory(m_directory);

            string filename = fileSystemController.BuildFullFileNameForInCurrentDirectory("test.xaml");

            Assert.AreEqual($@"{m_directory}\test.xaml", filename);
        }

        [TestMethod]
        public void TestBuildFullFileNameForInCurrentDirectoryWithForcedFileExtension()
        {
            FileSystemController fileSystemController = new FileSystemController()
            {
                ForceFileExtensionOfFileFilter = true,
                FileFilterToApply = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml")
            };

            fileSystemController.SelectDirectory(m_directory);

            string filename = fileSystemController.BuildFullFileNameForInCurrentDirectory("test.txt");

            Assert.AreEqual($@"{m_directory}\test.txt.cs", filename);
        }

        [TestMethod]
        public void TestBuildFullFileNameForInCurrentDirectoryWithoutForcedFileExtension()
        {
            FileSystemController fileSystemController = new FileSystemController()
            {
                ForceFileExtensionOfFileFilter = false,
                FileFilterToApply = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml")
            };

            fileSystemController.SelectDirectory(m_directory);

            string filename = fileSystemController.BuildFullFileNameForInCurrentDirectory("test.txt");

            Assert.AreEqual($@"{m_directory}\test.txt", filename);
        }

        [TestMethod]
        public void TestSelectOrRemoveDirectoryForMultipleSelection()
        {
            string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string myPictures = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string myMusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            FileSystemController fileSystemController = new FileSystemController();

            Assert.IsFalse(fileSystemController.SelectedDirectories.Any());

            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myDocuments);
            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myPictures);
            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myMusic);

            Assert.IsTrue(fileSystemController.SelectedDirectories.Any(directory => directory.FullName == myDocuments));
            Assert.IsTrue(fileSystemController.SelectedDirectories.Any(directory => directory.FullName == myPictures));
            Assert.IsTrue(fileSystemController.SelectedDirectories.Any(directory => directory.FullName == myMusic));

            fileSystemController.SelectOrRemoveDirectoryForMultipleSelection(myMusic);

            Assert.IsTrue(fileSystemController.SelectedDirectories.Any(directory => directory.FullName == myDocuments));
            Assert.IsTrue(fileSystemController.SelectedDirectories.Any(directory => directory.FullName == myPictures));
            Assert.IsFalse(fileSystemController.SelectedDirectories.Any(directory => directory.FullName == myMusic));
        }

        [TestMethod]
        public void TestSelectOrRemoveFileForMultipleSelection()
        {
            string gitconfigFile = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\.gitconfig";

            FileSystemController fileSystemController = new FileSystemController();

            Assert.IsFalse(fileSystemController.SelectedFiles.Any());

            fileSystemController.SelectOrRemoveFileForMultipleSelection(gitconfigFile);

            Assert.IsTrue(fileSystemController.SelectedFiles.Any(file => file.FullName == gitconfigFile));

            fileSystemController.SelectOrRemoveFileForMultipleSelection(gitconfigFile);

            Assert.IsFalse(fileSystemController.SelectedFiles.Any(file => file.FullName == gitconfigFile));
        }
    }
}
