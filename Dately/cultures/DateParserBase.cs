using System;
using System.Collections.Generic;

namespace RDumont.Dately.Cultures
{
    public class DateParserBase
    {
        protected DateTime? TryToUseNamedDays(string text, Dictionary<string,int> namedDays)
        {
            if (namedDays.ContainsKey(text))                return DateTime.Today.AddDays(namedDays[text]);
            foreach (var named in namedDays)
            {
                if (text.StartsWith(named.Key + " "))
                {
                    var rest = text.Substring(named.Key.Length + 1);

                    DateTime relativeOffset;

                    if (DateTime.TryParse(rest, out relativeOffset))
                    {
                        var baseDate = DateTime.Today.AddDays(namedDays[named.Key]);

                        {
                            return new DateTime(
                                baseDate.Year, baseDate.Month, baseDate.Day,
                                relativeOffset.Hour, relativeOffset.Minute, relativeOffset.Second, relativeOffset.Millisecond);
                        }
                    }
                }
            }

            return null;
        }
    }
}