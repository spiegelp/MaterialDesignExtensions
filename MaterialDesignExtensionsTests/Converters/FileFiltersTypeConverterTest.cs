using System;
using System.Collections.Generic;

using Xunit;

using MaterialDesignExtensions.Converters;
using MaterialDesignExtensions.Model;

namespace MaterialDesignExtensionsTests.Converters
{
    public class FileFiltersTypeConverterTest
    {
        [Fact]
        public void TestConvertFromString()
        {
            FileFiltersTypeConverter converter = new FileFiltersTypeConverter();

            Assert.True(converter.CanConvertFrom(typeof(string)));
            Assert.True(converter.CanConvertFrom(typeof(IList<IFileFilter>)));

            IList<IFileFilter> filters = converter.ConvertFrom("Test|*.cs") as IList<IFileFilter>;

            Assert.NotNull(filters);
            Assert.Equal(1, filters.Count);
            Assert.Equal("Test", filters[0].Label);
            Assert.Equal("*.cs", filters[0].Filters);

            filters = converter.ConvertFrom(filters) as IList<IFileFilter>;

            Assert.NotNull(filters);
            Assert.Equal(1, filters.Count);
            Assert.Equal("Test", filters[0].Label);
            Assert.Equal("*.cs", filters[0].Filters);

            filters = converter.ConvertFrom(null) as IList<IFileFilter>;

            Assert.Null(filters);
        }

        [Fact]
        public void TestConvertTo()
        {
            FileFiltersTypeConverter converter = new FileFiltersTypeConverter();

            Assert.True(converter.CanConvertTo(typeof(string)));
            Assert.True(converter.CanConvertTo(typeof(IList<IFileFilter>)));

            IList<IFileFilter> filters = new List<IFileFilter>()
            {
                FileFilter.Create("Test", "*.cs")
            };

            string filterStr = converter.ConvertTo(filters, typeof(string)) as string;

            Assert.NotNull(filterStr);
            Assert.Equal("Test|*.cs", filterStr);

            filterStr = converter.ConvertTo(filterStr, typeof(string)) as string;

            Assert.NotNull(filterStr);
            Assert.Equal("Test|*.cs", filterStr);

            filterStr = converter.ConvertFrom(null) as string;

            Assert.Null(filterStr);
        }
    }
}
