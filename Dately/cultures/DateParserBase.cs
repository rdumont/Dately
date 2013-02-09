using System;
using System.Collections.Generic;

namespace RDumont.Dately.Cultures
{
    public class DateParserBase
    {
        protected DateTime? TryToUseNamedDays(string text, Dictionary<string,int> namedDays)
        {
            if (namedDays.ContainsKey(text))                return DateTime.Today.AddDays(namedDays[text]);
            var splitBySpace = text.Split(new[] {' '}, 2);
            if (splitBySpace.Length >= 2 && namedDays.ContainsKey(splitBySpace[0]))
            {
                DateTime relativeOffset;

                if (DateTime.TryParse(splitBySpace[1], out relativeOffset))
                {
                    var baseDate = DateTime.Today.AddDays(namedDays[splitBySpace[0]]);

                    {
                        return new DateTime(
                            baseDate.Year, baseDate.Month, baseDate.Day,
                            relativeOffset.Hour, relativeOffset.Minute, relativeOffset.Second, relativeOffset.Millisecond);
                    }
                }
            }

            return null;
        }
    }
}