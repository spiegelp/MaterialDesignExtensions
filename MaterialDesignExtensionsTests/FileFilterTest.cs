using System.Collections.Generic;
using System.Linq;

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
            IFileFilter fileFilter = FileFilter.Create("Test", "*.cs");
            string filterStr1 = FileFilterHelper.ConvertFileFilterToString(fileFilter);
            string filterStr2 = filterStr1.ToString();

            Assert.AreEqual("Test|*.cs", filterStr1);
            Assert.AreEqual("Test|*.cs", filterStr2);
        }

        [TestMethod]
        public void TestConvertFileFiltersToString()
        {
            IList<IFileFilter> fileFilters = new List<IFileFilter>()
            {
                FileFilter.Create("Test", "*.cs"),
                FileFilter.Create("Web", "*.html;*.js")
            };

            string filtersStr = FileFilterHelper.ConvertFileFiltersToString(fileFilters);

            Assert.AreEqual("Test|*.cs|Web|*.html;*.js", filtersStr);
        }

        [TestMethod]
        public void TestParseFileFilter()
        {
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs");

            Assert.AreEqual("Test", fileFilter.Label);
            Assert.AreEqual("*.cs", fileFilter.Filters);
        }

        [TestMethod]
        public void TestParseFileFilters()
        {
            IList<IFileFilter> fileFilters = FileFilterHelper.ParseFileFilters("Test|*.cs|Web|*.html;*.js");

            Assert.AreEqual("Test", fileFilters[0].Label);
            Assert.AreEqual("*.cs", fileFilters[0].Filters);

            Assert.AreEqual("Web", fileFilters[1].Label);
            Assert.AreEqual("*.html;*.js", fileFilters[1].Filters);
        }

        [TestMethod]
        public void TestRegularExpression()
        {
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs;*.xaml;file.*;cs*.*");
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
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.*");
            string filename = "Test.cs";

            Assert.AreEqual(true, fileFilter.IsMatch(filename));
        }

        [TestMethod]
        public void TestGetFileExtensionsFromFilter()
        {
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs;file.*;cs*.*;*.xaml;test;*.txt.bak");
            IEnumerable<string> fileExtensions = FileFilterHelper.GetFileExtensionsFromFilter(fileFilter);

            Assert.IsNotNull(fileExtensions);
            Assert.AreEqual(3, fileExtensions.Count());
            Assert.IsTrue(fileExtensions.Any(fileExtension => fileExtension == "cs"));
            Assert.IsTrue(fileExtensions.Any(fileExtension => fileExtension == "xaml"));
            Assert.IsTrue(fileExtensions.Any(fileExtension => fileExtension == "bak"));

            fileExtensions = FileFilterHelper.GetFileExtensionsFromFilter(null);

            Assert.IsTrue(fileExtensions == null || !fileExtensions.Any());
        }
    }
}
