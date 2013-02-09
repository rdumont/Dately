using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RDumont.Dately.Cultures
{
    public class PtBrDateParser : DateParserBase, IDateParser
    {
        public string Culture
        {
            get { return "pt-BR"; }
        }

        public string Name
        {
            get { return "Português Brasileiro"; }
        }

        protected override DateTime DoLanguageSpecificParse(string text)
        {
            text = text.Trim().ToLower();
            var match = Regex.Match(text, @"^(?<amount>\d+) (?<unit>\w+) atrás$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var amount = -int.Parse(match.Groups["amount"].Value);
                var unit = match.Groups["unit"].Value;
                return ResultingTime(amount, unit);
            }

            match = Regex.Match(text, @"^daqui a (?<amount>\d+) (?<unit>\w+)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var amount = int.Parse(match.Groups["amount"].Value);
                var unit = match.Groups["unit"].Value;
                return ResultingTime(amount, unit);
            }

            if (text == "agora") return DateTime.Now;

            var namedDayResult = TryToUseNamedDays(text, new Dictionary<string, int>()
            {
                {"hoje", 0},
                {"amanhã", 1},
                {"ontem", -1},
                {"depois de amanhã", 2},
                {"anteontem",-2}
            });

            if (namedDayResult.HasValue)
            {
                return namedDayResult.Value;
            }

            throw new FormatException("Unknown date format");
        }

        private DateTime ResultingTime(int amount, string unit)
        {
            if (unit == "dia" || unit == "dias")
                return DateTime.Now.AddDays(amount);
            if (unit == "hora" || unit == "horas")
                return DateTime.Now.AddHours(amount);
            if (unit == "semana" || unit == "semanas")
                return DateTime.Now.AddDays(7*amount);
            if (unit == "minuto" || unit == "minutos")
                return DateTime.Now.AddMinutes(amount);
            if (unit == "mês" || unit == "meses")
                return DateTime.Now.AddMonths(amount);
            if (unit == "segundo" || unit == "segundos")
                return DateTime.Now.AddSeconds(amount);
            if (unit == "ano" || unit == "anos")
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