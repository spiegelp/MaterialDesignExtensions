using System;

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
    }
}
