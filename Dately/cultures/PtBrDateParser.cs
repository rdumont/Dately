using System;
using System.Text.RegularExpressions;

namespace RDumont.Dately.Cultures
{
    public class PtBrDateParser : IDateParser
    {
        public string Culture
        {
            get { return "pt-BR"; }
        }

        public string Name
        {
            get { return "Português Brasileiro"; }
        }

        public DateTime Parse(string text)
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

            if (text == "hoje") return DateTime.Today;
            if (text == "agora") return DateTime.Now;
            if (text == "amanhã") return DateTime.Today.AddDays(1);
            if (text == "ontem") return DateTime.Today.AddDays(-1);
            if (text == "depois de amanhã") return DateTime.Today.AddDays(2);
            if (text == "anteontem") return DateTime.Today.AddDays(-2);

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