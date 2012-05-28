using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RDumont.Dately.Cultures;

namespace RDumont.Dately
{
    public interface IDateParserFactory
    {
        IDateParser ForCulture(string cultureCode);
    }

    public class DateParserFactory : IDateParserFactory
    {
        private static readonly IDictionary<string, Type> _cultureParsers;

        static DateParserFactory()
        {
            _cultureParsers = new SortedDictionary<string, Type>
                {
                    {"en-US", typeof (EnUsDateParser)},
                    {"pt-BR", typeof (PtBrDateParser)}
                };
        }

        public IDateParser ForCulture(string cultureCode)
        {
            try
            {
                var type = _cultureParsers[cultureCode];
                return (IDateParser)type.GetConstructor(new Type[] { }).Invoke(new object[] { });
            }
            catch (KeyNotFoundException e)
            {
                throw new KeyNotFoundException("No parser found for this culture", e);
            }
        }
    }
}
