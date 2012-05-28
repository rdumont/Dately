using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using RDumont.Dately.Cultures;
using Shouldly;

namespace RDumont.Dately.Tests
{
    [TestFixture]
    public class FactoryTests
    {
        [Test]
        public void Should_retrieve_en_us_parser()
        {
            // Arrange
            var factory = new DateParserFactory();

            // Act
            var result = factory.ForCulture("en-US");

            // Assert
            result.ShouldBeTypeOf<EnUsDateParser>();
        }

        [Test]
        public void Should_retrieve_pt_br_parser()
        {
            // Arrange
            var factory = new DateParserFactory();

            // Act
            var result = factory.ForCulture("pt-BR");

            // Assert
            result.ShouldBeTypeOf<PtBrDateParser>();
        }

        [Test]
        public void Should_not_find_unknown_culture()
        {
            // Arrange
            var factory = new DateParserFactory();

            // Act
            var exception = Should.Throw<KeyNotFoundException>(() =>
                factory.ForCulture("xxxxx"));

            // Assert
            exception.Message.ShouldBe("No parser found for this culture");
        }
    }
}
