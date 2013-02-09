using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RDumont.Dately.Cultures;

namespace RDumont.Dately.Tests.cultures
{
    [TestFixture]
    public class PtBrParserTests
    {
        private static IDateParser GetParser()
        {
            return new PtBrDateParser();
        }

        [Test]
        public void Agora()
        {
            Helpers.AssertDate(GetParser(), "agora", DateTime.Now);
        }

        [Test]
        public void Hoje()
        {
            Helpers.AssertDate(GetParser(), "hoje", DateTime.Today);
        }

        [Test]
        public void Daqui_a_1_hora()
        {
            Helpers.AssertDate(GetParser(), "daqui a 1 hora", DateTime.Now.AddHours(1));
        }

        [TestCase(2)]
        [TestCase(12)]
        [TestCase(30)]
        public void Daqui_a_x_horas(int horas)
        {
            Helpers.AssertDate(GetParser(), "daqui a " + horas + " horas", DateTime.Now.AddHours(horas));
        }

        [Test]
        public void Ha_1_hora()
        {
            Helpers.AssertDate(GetParser(), "1 hora atrás", DateTime.Now.AddHours(-1));
        }

        [TestCase(2)]
        [TestCase(14)]
        [TestCase(33)]
        public void Ha_x_horas(int horas)
        {
            Helpers.AssertDate(GetParser(), horas + " horas atrás", DateTime.Now.AddHours(-horas));
        }

        [Test]
        public void Ontem()
        {
            Helpers.AssertDate(GetParser(), "ontem", DateTime.Today.AddDays(-1));
        }

        [Test]
        public void Anteontem()
        {
            Helpers.AssertDate(GetParser(), "anteontem", DateTime.Today.AddDays(-2));
        }

        [Test]
        public void Amanha()
        {
            Helpers.AssertDate(GetParser(), "amanhã", DateTime.Today.AddDays(1));
        }

        [Test]
        public void Depois_de_amanha()
        {
            Helpers.AssertDate(GetParser(), "depois de amanhã", DateTime.Today.AddDays(2));
        }

        [TestCase(2)]
        [TestCase(8)]
        [TestCase(35)]
        public void Daqui_a_x_dias(int dias)
        {
            Helpers.AssertDate(GetParser(), "daqui a " + dias + " dias", DateTime.Now.AddDays(dias));
        }

        [TestCase(2)]
        [TestCase(8)]
        [TestCase(35)]
        public void Ha_x_dias(int dias)
        {
            Helpers.AssertDate(GetParser(), dias + " dias atrás", DateTime.Now.AddDays(-dias));
        }

        [Test]
        public void ShouldUseSystemDateTimeForCompoundSupport()
        {
            var dayAfterTomorrow = DateTime.Today.AddDays(2);

            var expectedTime = new DateTime(dayAfterTomorrow.Year, dayAfterTomorrow.Month, dayAfterTomorrow.Day, 13, 2, 3, DateTimeKind.Local);

            Helpers.AssertDate(GetParser(), "depois de amanhã 01:02:03 PM", expectedTime);
        }

    }

}
