using System;
using RDumont.Dately.Tests.cultures;
using Shouldly;

namespace RDumont.Dately.Tests
{
    static internal class Helpers
    {
        public static void AssertDate(IDateParser parser, string text, DateTime expected, int precison = 2000)
        {
            // Act
            var result = parser.Parse(text);

            // Assert
            var max = expected.AddMilliseconds(precison);
            var min = expected.AddMilliseconds(-precison);

            result.ShouldBeGreaterThan(min);
            result.ShouldBeLessThan(max);
        }
    }
}