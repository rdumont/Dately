using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RDumont.Dately
{
    public abstract class AbstractDateParser : IDateParser
    {
        public string Culture { get; protected set; }
        public string Name { get; protected set; }

        protected PatternCollection Now { get; private set; }
        protected PatternCollection Today { get; private set; }
        protected PatternCollection Yesterday { get; private set; }
        protected PatternCollection Tomorrow { get; private set; }

        protected PatternCollection YearsAgo { get; private set; }
        protected PatternCollection MonthsAgo { get; private set; }
        protected PatternCollection WeeksAgo { get; private set; }
        protected PatternCollection DaysAgo { get; private set; }
        protected PatternCollection HoursAgo { get; private set; }
        protected PatternCollection MinutesAgo { get; private set; }
        protected PatternCollection SecondsAgo { get; private set; }

        protected PatternCollection YearsFromNow { get; private set; }
        protected PatternCollection MonthsFromNow { get; private set; }
        protected PatternCollection WeeksFromNow { get; private set; }
        protected PatternCollection DaysFromNow { get; private set; }
        protected PatternCollection HoursFromNow { get; private set; }
        protected PatternCollection MinutesFromNow { get; private set; }
        protected PatternCollection SecondsFromNow { get; private set; }

        protected List<Func<string, DateTime?>> Custom { get; private set; }

        protected abstract DateType GetDateType(string text);

        protected AbstractDateParser(string culture, string name)
        {
            Culture = culture;
            Name = name;

            Now = new PatternCollection();
            Today = new PatternCollection();
            Yesterday = new PatternCollection();
            Tomorrow = new PatternCollection();

            YearsAgo = new PatternCollection();
            MonthsAgo = new PatternCollection();
            WeeksAgo = new PatternCollection();
            DaysAgo = new PatternCollection();
            HoursAgo = new PatternCollection();
            MinutesAgo = new PatternCollection();
            SecondsAgo = new PatternCollection();

            YearsFromNow = new PatternCollection();
            MonthsFromNow = new PatternCollection();
            WeeksFromNow = new PatternCollection();
            DaysFromNow = new PatternCollection();
            HoursFromNow = new PatternCollection();
            MinutesFromNow = new PatternCollection();
            SecondsFromNow = new PatternCollection();

            Custom = new List<Func<string, DateTime?>>();
        }

        public DateTime Parse(string text)
        {
            var type = GetDateType(text);

            switch (type)
            {
                case DateType.SomethingAgo:
                    return ParseAgo(text);
                case DateType.SomethingFromNow:
                    return ParseFromNow(text);
                case DateType.Special:
                    return ParseSpecial(text);
                case DateType.Custom:
                    return ParseCustom(text);
                default:
                    throw new ArgumentOutOfRangeException();
            }
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

        private DateTime? TryParseCollection(IEnumerable<Regex> patternCollection, string text, Func<Match, DateTime> parseResult)
        {
            foreach (var pattern in patternCollection)
            {
                var match = pattern.Match(text);
                if (match.Success) return parseResult(match);
            }

            return null;
        }

        private DateTime ParseAgo(string text)
        {
            var result = TryParseCollection(YearsAgo, text,
                    match => DateTime.Now.AddYears(int.Parse(match.Groups["years"].Value)))
                ?? TryParseCollection(MonthsAgo, text,
                    match => DateTime.Now.AddMonths(int.Parse(match.Groups["months"].Value)))
                ?? TryParseCollection(WeeksAgo, text,
                    match => DateTime.Now.AddDays(7 * int.Parse(match.Groups["weeks"].Value)))
                ?? TryParseCollection(DaysAgo, text,
                    match => DateTime.Now.AddDays(int.Parse(match.Groups["days"].Value)))
                ?? TryParseCollection(HoursAgo, text,
                    match => DateTime.Now.AddHours(int.Parse(match.Groups["hours"].Value)))
                ?? TryParseCollection(MinutesAgo, text,
                    match => DateTime.Now.AddHours(int.Parse(match.Groups["minutes"].Value)))
                ?? TryParseCollection(SecondsAgo, text,
                    match => DateTime.Now.AddSeconds(int.Parse(match.Groups["seconds"].Value)));

            if (result != null) return result.Value;
            throw new FormatException("Unexpected DateTime format");
        }

        private DateTime ParseFromNow(string text)
        {
            var result = TryParseCollection(YearsFromNow, text,
                    match => DateTime.Now.AddYears(int.Parse(match.Groups["years"].Value)))
                ?? TryParseCollection(MonthsFromNow, text,
                    match => DateTime.Now.AddMonths(int.Parse(match.Groups["months"].Value)))
                ?? TryParseCollection(WeeksFromNow, text,
                    match => DateTime.Now.AddDays(7 * int.Parse(match.Groups["weeks"].Value)))
                ?? TryParseCollection(DaysFromNow, text,
                    match => DateTime.Now.AddDays(int.Parse(match.Groups["days"].Value)))
                ?? TryParseCollection(HoursFromNow, text,
                    match => DateTime.Now.AddHours(int.Parse(match.Groups["hours"].Value)))
                ?? TryParseCollection(MinutesFromNow, text,
                    match => DateTime.Now.AddHours(int.Parse(match.Groups["minutes"].Value)))
                ?? TryParseCollection(SecondsFromNow, text,
                    match => DateTime.Now.AddSeconds(int.Parse(match.Groups["seconds"].Value)));

            if (result != null) return result.Value;
            throw new FormatException("Unexpected DateTime format");
        }

        private DateTime ParseSpecial(string text)
        {
            var result = TryParseCollection(Now, text, match => DateTime.Today)
                ?? TryParseCollection(Today, text, match => DateTime.Today)
                    ?? TryParseCollection(Yesterday, text, match => DateTime.Today)
                        ?? TryParseCollection(Tomorrow, text, match => DateTime.Today);

            if(result != null) return result.Value;
            throw new FormatException("Unexpected DateTime format");
        }

        private DateTime ParseCustom(string text)
        {
            foreach (var method in Custom)
            {
                var result = method(text);
                if(result != null) return result.Value;
            }

            throw new FormatException("Unexpected DateTime format");
        }
    }
}