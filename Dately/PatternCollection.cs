using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RDumont.Dately
{
    public class PatternCollection : IEnumerable<Regex>
    {
        private readonly List<Regex> _regexes = new List<Regex>();

        public void Add(string[] patterns)
        {
            foreach (var pattern in patterns)
                _regexes.Add(new Regex("^" + pattern + "$",
                    RegexOptions.IgnoreCase |
                        RegexOptions.Compiled));
        }

        public IEnumerator<Regex> GetEnumerator()
        {
            return _regexes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _regexes.GetEnumerator();
        }
    }
}