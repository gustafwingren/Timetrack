using System.CommandLine.Parsing;

namespace Timetrack.Helpers;

public static class ParseHelperMethods
{
    public static TimeSpan ParseTimeSpan(ArgumentResult result)
    {
        return TimeSpan.Parse(result.Tokens.Single().Value);
    }
}
