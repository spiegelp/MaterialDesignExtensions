using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MaterialDesignExtensions.Controllers;
using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsTests
{
    [TestClass]
    public class FileFiltersTypeConverterTest
    {
        [TestMethod]
        public void TestConvertFromString()
        {
            FileFiltersTypeConverter converter = new FileFiltersTypeConverter();

            Assert.AreEqual(true, converter.CanConvertFrom(typeof(string)));
            Assert.AreEqual(true, converter.CanConvertFrom(typeof(IList<IFileFilter>)));

            IList<IFileFilter> filters = converter.ConvertFrom("Test|*.cs") as IList<IFileFilter>;

            Assert.IsNotNull(filters);
            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual("Test", filters[0].Label);
            Assert.AreEqual("*.cs", filters[0].Filters);

            filters = converter.ConvertFrom(filters) as IList<IFileFilter>;

            Assert.IsNotNull(filters);
            Assert.AreEqual(1, filters.Count);
            Assert.AreEqual("Test", filters[0].Label);
            Assert.AreEqual("*.cs", filters[0].Filters);

            filters = converter.ConvertFrom(null) as IList<IFileFilter>;

            Assert.IsNull(filters);
        }

        [TestMethod]
        public void TestConvertTo()
        {
            FileFiltersTypeConverter converter = new FileFiltersTypeConverter();

            Assert.AreEqual(true, converter.CanConvertTo(typeof(string)));
            Assert.AreEqual(true, converter.CanConvertTo(typeof(IList<IFileFilter>)));

            IList<IFileFilter> filters = new List<IFileFilter>()
            {
                FileFilter.Create("Test", "*.cs")
            };

            string filterStr = converter.ConvertTo(filters, typeof(string)) as string;

            Assert.IsNotNull(filterStr);
            Assert.AreEqual("Test|*.cs", filterStr);

            filterStr = converter.ConvertTo(filterStr, typeof(string)) as string;

            Assert.IsNotNull(filterStr);
            Assert.AreEqual("Test|*.cs", filterStr);

            filterStr = converter.ConvertFrom(null) as string;

            Assert.IsNull(filterStr);
        }
    }
}
