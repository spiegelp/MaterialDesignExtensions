using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsTests.Controllers
{
    public class FileFilterTest
    {
        [Fact]
        public void TestConvertFileFilterToString()
        {
            IFileFilter fileFilter = FileFilter.Create("Test", "*.cs");
            string filterStr1 = FileFilterHelper.ConvertFileFilterToString(fileFilter);
            string filterStr2 = filterStr1.ToString();

            Assert.Equal("Test|*.cs", filterStr1);
            Assert.Equal("Test|*.cs", filterStr2);
        }

        [Fact]
        public void TestConvertFileFiltersToString()
        {
            IList<IFileFilter> fileFilters = new List<IFileFilter>()
            {
                FileFilter.Create("Test", "*.cs"),
                FileFilter.Create("Web", "*.html;*.js")
            };

            string filtersStr = FileFilterHelper.ConvertFileFiltersToString(fileFilters);

            Assert.Equal("Test|*.cs|Web|*.html;*.js", filtersStr);
        }

        [Fact]
        public void TestParseFileFilter()
        {
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs");

            Assert.Equal("Test", fileFilter.Label);
            Assert.Equal("*.cs", fileFilter.Filters);
        }

        [Fact]
        public void TestParseFileFilters()
        {
            IList<IFileFilter> fileFilters = FileFilterHelper.ParseFileFilters("Test|*.cs|Web|*.html;*.js");

            Assert.Equal("Test", fileFilters[0].Label);
            Assert.Equal("*.cs", fileFilters[0].Filters);

            Assert.Equal("Web", fileFilters[1].Label);
            Assert.Equal("*.html;*.js", fileFilters[1].Filters);
        }

        [Fact]
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

            Assert.True(fileFilter.IsMatch(filename1));
            Assert.True(fileFilter.IsMatch(filename2));
            Assert.False(fileFilter.IsMatch(filename3));
            Assert.False(fileFilter.IsMatch(filename4));
            Assert.True(fileFilter.IsMatch(filename5));
            Assert.True(fileFilter.IsMatch(filename6));
            Assert.True(fileFilter.IsMatch(filename7));
        }

        [Fact]
        public void TestRegularExpressionAny()
        {
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.*");
            string filename = "Test.cs";

            Assert.True(fileFilter.IsMatch(filename));
        }

        [Fact]
        public void TestGetFileExtensionsFromFilter()
        {
            IFileFilter fileFilter = FileFilterHelper.ParseFileFilter("Test", "*.cs;file.*;cs*.*;*.xaml;test;*.txt.bak");
            IEnumerable<string> fileExtensions = FileFilterHelper.GetFileExtensionsFromFilter(fileFilter);

            Assert.NotNull(fileExtensions);
            Assert.Equal(3, fileExtensions.Count());
            Assert.Contains(fileExtensions, fileExtension => fileExtension == "cs");
            Assert.Contains(fileExtensions, fileExtension => fileExtension == "xaml");
            Assert.Contains(fileExtensions, fileExtension => fileExtension == "bak");

            fileExtensions = FileFilterHelper.GetFileExtensionsFromFilter(null);

            Assert.True(fileExtensions == null || !fileExtensions.Any());
        }
    }
}
