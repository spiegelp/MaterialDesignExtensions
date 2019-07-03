using System;
using System.Globalization;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MaterialDesignExtensions.Converters;

namespace MaterialDesignExtensionsTests
{
    [TestClass]
    public class DateTimeAgoConverterTest
    {
        private void SetThreadCulture(string cultureString = "de-at")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureString);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureString);
        }

        [TestMethod]
        public void TestArgumentOfSameDay()
        {
            SetThreadCulture();

            DateTime dateTime = DateTime.Now;

            string result = new DateTimeAgoConverter().Convert(dateTime, typeof(string), null, Thread.CurrentThread.CurrentCulture) as string;

            Assert.AreEqual(result, dateTime.ToShortTimeString());
        }

        [TestMethod]
        public void TestArgumentOfYesterday()
        {
            SetThreadCulture();

            DateTime dateTime = DateTime.Now.AddDays(-1);

            string result = new DateTimeAgoConverter().Convert(dateTime, typeof(string), null, Thread.CurrentThread.CurrentCulture) as string;

            Assert.AreEqual(result, dateTime.ToShortDateString());
        }

        [TestMethod]
        public void TestArgumentNotOfTypeDateTime()
        {
            SetThreadCulture();

            string result = new DateTimeAgoConverter().Convert(4, typeof(string), null, Thread.CurrentThread.CurrentCulture) as string;
            
            Assert.AreEqual(result, string.Empty);
        }
    }
}
