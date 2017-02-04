using System;
using Model.Helpers;
using Xunit;

namespace UnitTests.Fixtures
{
    public class FormatHelperFixtures
    {
        [Fact]
        public void DateFormatValidation()
        {
            TestDateFormat(new DateTime(2017, 1, 1), "01.01.2017");
            TestDateFormat(new DateTime(2017, 12, 1), "01.12.2017");
            TestDateFormat(new DateTime(2017, 1, 31), "31.01.2017");
            TestDateFormat(new DateTime(2017, 12, 12), "12.12.2017");
        }

        [Fact]
        public void DateTimeFormatValidation()
        {
            TestDateTimeFormat(new DateTime(2017, 1, 1, 7, 50, 0), "01.01.2017 07:50");
            TestDateTimeFormat(new DateTime(2017, 12, 1, 19, 50, 0), "01.12.2017 19:50");
            TestDateTimeFormat(new DateTime(2017, 1, 31, 23, 59, 59), "31.01.2017 23:59");
            TestDateTimeFormat(new DateTime(2017, 12, 12, 0, 1, 0), "12.12.2017 00:01");
        }

        private void TestDateFormat(DateTime date, string excepted)
        {
            var data = FormatHelper.DateFormat(date);
            Assert.Equal(data, excepted);
        }

        private void TestDateTimeFormat(DateTime datetime, string excepted)
        {
            var data = FormatHelper.DateTimeFormat(datetime);
            Assert.Equal(data, excepted);
        }
    }
}
