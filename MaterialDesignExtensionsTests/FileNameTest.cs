using Microsoft.VisualStudio.TestTools.UnitTesting;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensionsTests
{
    [TestClass]
    public class FileNameTest
    {
        [TestMethod]
        public void TestCheckFileName()
        {
            Assert.AreEqual(true, FileNameHelper.CheckFileName("myDirectory"));
            Assert.AreEqual(true, FileNameHelper.CheckFileName("myTextFile.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName(""));
            Assert.AreEqual(false, FileNameHelper.CheckFileName(null));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_<_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_>_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_:_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_\"_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_/_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_\\_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_|_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_?_.txt"));
            Assert.AreEqual(false, FileNameHelper.CheckFileName("t_*_.txt"));
        }
    }
}
