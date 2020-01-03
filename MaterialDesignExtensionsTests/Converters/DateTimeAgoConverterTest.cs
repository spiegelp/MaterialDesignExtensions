using System;
using System.Globalization;
using System.Threading;

using Xunit;

using MaterialDesignExtensions.Converters;

namespace MaterialDesignExtensionsTests.Converters
{
    public class DateTimeAgoConverterTest
    {
        private void SetThreadCulture(string cultureString = "de-at")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureString);
        }

        [Fact]
        public void TestArgumentOfSameDay()
        {
            SetThreadCulture();

            DateTime dateTime = DateTime.Now;

            string result = new DateTimeAgoConverter().Convert(dateTime, typeof(string), null, Thread.CurrentThread.CurrentCulture) as string;

            Assert.Equal(result, dateTime.ToShortTimeString());
        }

        [Fact]
        public void TestArgumentOfYesterday()
        {
            SetThreadCulture();

            DateTime dateTime = DateTime.Now.AddDays(-1);

            string result = new DateTimeAgoConverter().Convert(dateTime, typeof(string), null, Thread.CurrentThread.CurrentCulture) as string;

            Assert.Equal(result, dateTime.ToShortDateString());
        }

        [Fact]
        public void TestArgumentNotOfTypeDateTime()
        {
            SetThreadCulture();

            string result = new DateTimeAgoConverter().Convert(4, typeof(string), null, Thread.CurrentThread.CurrentCulture) as string;
            
            Assert.Equal(result, string.Empty);
        }
    }
}
