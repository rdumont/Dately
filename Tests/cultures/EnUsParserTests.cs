using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RDumont.Dately.Cultures;

namespace RDumont.Dately.Tests.cultures
{
    [TestFixture]
    public class EnUsParserTests
    {
        private static IDateParser GetParser()
        {
            return new EnUsDateParser();
        }

        #region Special

        [Test]
        public void Now()
        {
            Helpers.AssertDate(GetParser(), "now", DateTime.Now);
        }

        [Test]
        public void Today()
        {
            Helpers.AssertDate(GetParser(), "today", DateTime.Today);
        }

        [Test]
        public void Yesterday()
        {
            Helpers.AssertDate(GetParser(), "yesterday", DateTime.Today.AddDays(-1));
        }

        [Test]
        public void Tomorrow()
        {
            Helpers.AssertDate(GetParser(), "tomorrow", DateTime.Today.AddDays(1));
        }

        [Test]
        public void ShouldUseDateTimeForCompoundSupport()
        {
            var tomorrow = DateTime.Today.AddDays(1);

            var expectedTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 13, 2, 3, DateTimeKind.Local);

            Helpers.AssertDate(GetParser(), "tomorrow 01:02:03 PM", expectedTime);
        }

        #endregion

        #region Hours

        [Test]
        public void One_hour_from_now()
        {
            Helpers.AssertDate(GetParser(), "1 hour from now", DateTime.Now.AddHours(1));
        }

        [TestCase(2)]
        [TestCase(12)]
        [TestCase(30)]
        public void X_hours_from_now(int horas)
        {
            Helpers.AssertDate(GetParser(), horas + " hours from now", DateTime.Now.AddHours(horas));
        }

        [Test]
        public void One_hour_ago()
        {
            Helpers.AssertDate(GetParser(), "1 hour ago", DateTime.Now.AddHours(-1));
        }

        [TestCase(2)]
        [TestCase(14)]
        [TestCase(33)]
        public void X_hours_ago(int horas)
        {
            Helpers.AssertDate(GetParser(), horas + " hours ago", DateTime.Now.AddHours(-horas));
        }

        #endregion

        #region Days

        [TestCase(2)]
        [TestCase(8)]
        [TestCase(35)]
        public void X_days_from_now(int dias)
        {
            Helpers.AssertDate(GetParser(), dias + " days from now", DateTime.Now.AddDays(dias));
        }

        [TestCase(2)]
        [TestCase(8)]
        [TestCase(35)]
        public void X_days_ago(int dias)
        {
            Helpers.AssertDate(GetParser(), dias + " days ago", DateTime.Now.AddDays(-dias));
        }

        #endregion
    }

}
