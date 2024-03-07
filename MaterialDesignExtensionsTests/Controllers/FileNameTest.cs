using System;
using System.Collections.Generic;

using Xunit;

using MaterialDesignExtensions.Controllers;

namespace MaterialDesignExtensionsTests.Controllers
{
    public class FileNameTest
    {
        [Fact]
        public void TestCheckFileName()
        {
            Assert.True(FileNameHelper.CheckFileName("myDirectory"));
            Assert.True(FileNameHelper.CheckFileName("myTextFile.txt"));
            Assert.False(FileNameHelper.CheckFileName(""));
            Assert.False(FileNameHelper.CheckFileName(null));
            Assert.False(FileNameHelper.CheckFileName("t_<_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_>_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_:_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_\"_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_/_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_\\_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_|_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_?_.txt"));
            Assert.False(FileNameHelper.CheckFileName("t_*_.txt"));
        }
    }
}
