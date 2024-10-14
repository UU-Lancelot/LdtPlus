namespace LdtPlus;
public static class StringExtensions
{
    public static bool TryGetIndex(this string str, string value, out int startIndex, out int endIndex)
    {
        int index = str.IndexOf(value);

        if (index != -1)
        {
            startIndex = index;
            endIndex = index + value.Length;
            return true;
        }

        startIndex = 0;
        endIndex = 0;
        return false;
    }

    public static string Cut(this string str, int? startIndex = null, int? endIndex = null)
    {
        int start = startIndex ?? 0;
        int end = endIndex ?? str.Length;

        return str.Substring(start, end - start);
    }

    public static string[] SplitName(this string str)
    {
        return str.Split(',')
            .Select(s => s.Trim())
            .ToArray();
    }

    public static string SimplifyName(this string str)
    {
        return str.SplitName().First();
    }
}