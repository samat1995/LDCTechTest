using System.Collections.Generic;

namespace StringParsing
{
    public interface IStringParser
    {
        IEnumerable<string> ParseStrings(IEnumerable<string> input);
    }
}
