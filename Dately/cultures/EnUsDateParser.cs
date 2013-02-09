using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RDumont.Dately.Cultures
{
    public class EnUsDateParser : DateParserBase, IDateParser
    {
        public string Culture
        {
            get { return "en-US"; }
        }

        public string Name
        {
            get { return "English (US)"; }
        }

        public DateTime Parse(string text)
        {
            text = text.Trim().ToLower();
            var match = Regex.Match(text, @"^(?<amount>\d+) (?<unit>\w+) ago$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var amount = -int.Parse(match.Groups["amount"].Value);
                var unit = match.Groups["unit"].Value;
                return ResultingTime(amount, unit);
            }

            match = Regex.Match(text, @"^(?<amount>\d+) (?<unit>\w+) from now$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var amount = int.Parse(match.Groups["amount"].Value);
                var unit = match.Groups["unit"].Value;
                return ResultingTime(amount, unit);
            }

            if (text == "now") return DateTime.Now;
            
            var namedDayResult = TryToUseNamedDays(text, new Dictionary<string,int>()
            {
                {"today", 0},
                {"tomorrow", 1},
                {"yesterday", -1}
            });

            if (namedDayResult.HasValue)
            {
                return namedDayResult.Value;
            }

            throw new FormatException("Unknown date format");
        }

        private DateTime ResultingTime(int amount, string unit)
        {
            if (unit == "day" || unit == "days")
                return DateTime.Now.AddDays(amount);
            if (unit == "hour" || unit == "hours")
                return DateTime.Now.AddHours(amount);
            if (unit == "week" || unit == "weeks")
                return DateTime.Now.AddDays(7*amount);
            if (unit == "minute" || unit == "minutes")
                return DateTime.Now.AddMinutes(amount);
            if (unit == "month" || unit == "months")
                return DateTime.Now.AddMonths(amount);
            if (unit == "second" || unit == "seconds")
                return DateTime.Now.AddSeconds(amount);
            if (unit == "year" || unit == "years")
                return DateTime.Now.AddYears(amount);

            throw new FormatException("Unknown date format");
        }

        public bool TryParse(string text, out DateTime result)
        {
            try
            {
                result = Parse(text);
                return true;
            }
            catch (FormatException)
            {
                result = default(DateTime);
                return false;
            }
        }
    }
}