using System.Globalization;

namespace Domain.Utilities.Extensions;

public static class StringExtensions
{
    public static string ToKey(this string value)
    {
        return value.Trim().ToLower().Replace(' ', '_');
    }
    
    public static string FromKeyToValue(this string value)
    {
        var cultureInfo = CultureInfo.InvariantCulture;

        return cultureInfo.TextInfo.ToTitleCase(value.Replace('_', ' '));
    }
}