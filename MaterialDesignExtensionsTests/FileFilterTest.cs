using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsTests
{
    [TestClass]
    public class FileFilterTest
    {
        [TestMethod]
        public void TestConvertFileFilterToString()
        {
            FileFilter fileFilter = new FileFilter() { Label = "Test", Filters = "*.cs" };
            string filterStr = FileFilterHelper.ConvertFileFilterToString(fileFilter);

            Assert.AreEqual("Test|*.cs", filterStr);
        }

        [TestMethod]
        public void TestConvertFileFiltersToString()
        {
            IList<FileFilter> fileFilters = new List<FileFilter>()
            {
                new FileFilter() { Label = "Test", Filters = "*.cs" },
                new FileFilter() { Label = "Web", Filters = "*.html;*.js" }
            };

            string filtersStr = FileFilterHelper.ConvertFileFiltersToString(fileFilters);

            Assert.AreEqual("Test|*.cs|Web|*.html;*.js", filtersStr);
        }

        [TestMethod]
        public void TestParseFileFilter()
        {
            FileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs");

            Assert.AreEqual("Test", fileFilter.Label);
            Assert.AreEqual("*.cs", fileFilter.Filters);
        }

        [TestMethod]
        public void TestParseFileFilters()
        {
            IList<FileFilter> fileFilters = FileFilterHelper.ParseFileFilters("Test|*.cs|Web|*.html;*.js");

            Assert.AreEqual("Test", fileFilters[0].Label);
            Assert.AreEqual("*.cs", fileFilters[0].Filters);

            Assert.AreEqual("Web", fileFilters[1].Label);
            Assert.AreEqual("*.html;*.js", fileFilters[1].Filters);
        }

        [TestMethod]
        public void TestRegularExpression()
        {
            FileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml;file.*;cs*.*");
            string filename1 = "Test.cs";
            string filename2 = "Test.xaml";
            string filename3 = "Test.html";
            string filename4 = "Test.css";
            string filename5 = "file.html";
            string filename6 = "csfile1_2.js";
            string filename7 = "cs.js";

            Assert.AreEqual(true, fileFilter.IsMatch(filename1));
            Assert.AreEqual(true, fileFilter.IsMatch(filename2));
            Assert.AreEqual(false, fileFilter.IsMatch(filename3));
            Assert.AreEqual(false, fileFilter.IsMatch(filename4));
            Assert.AreEqual(true, fileFilter.IsMatch(filename5));
            Assert.AreEqual(true, fileFilter.IsMatch(filename6));
            Assert.AreEqual(true, fileFilter.IsMatch(filename7));
        }

        [TestMethod]
        public void TestRegularExpressionAny()
        {
            FileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.*");
            string filename = "Test.cs";

            Assert.AreEqual(true, fileFilter.IsMatch(filename));
        }
    }
}
