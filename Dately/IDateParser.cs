using System;

namespace RDumont.Dately
{
    public interface IDateParser
    {
        string Culture { get; }
        string Name { get; }

        DateTime Parse(string text);
        bool TryParse(string text, out DateTime result);
    }
}
