using System.Text.RegularExpressions;
using LdtPlus.Interactive.MenuData;

namespace LdtPlus.Config;
public class ConfigBuilder
{
    public ConfigBuilder(string ldtPath, Executor executor)
    {
        _ldtPath = ldtPath;
        _executor = executor;
    }

    private readonly string _ldtPath;
    private readonly Executor _executor;
    private string? _ldtBookkitUrl;

    public ConfigData CreateConfig()
    {
        MenuSection[] sections = ParseCommand("");

        string ldtBookkitUrl = _ldtBookkitUrl?.Trim()
            ?? throw new MissingMemberException("uuBookKit: not found");

        string ldtVersion = _executor.RunAndGetOutput("version").Trim();

        return new ConfigData(_ldtPath, ldtVersion, ldtBookkitUrl, sections, new(), new());
    }

    private MenuSection[] ParseCommand(string command)
    {
        // run
        string output = _executor.RunAndGetOutput($"{command} --help");

        // split into sections
        Regex sectionRegex = new(@$"{Environment.NewLine} (.+):{Environment.NewLine} --+{Environment.NewLine}");
        (string title, string content)[] sections = sectionRegex.Split(output)
            .Skip(1) // skip header
            .Select((item, index) => (item, index))
            .GroupBy(pair => pair.index / 2) // group pairs
            .Select(g => (g.First().item, g.Last().item))
            .ToArray();

        // parse sections
        MenuSection[] sectionResults = sections
            .Select(pair => CreateFromSection(pair.title, pair.content, command))
            .Where(section => section is not null)
            .Select(section => section!)
            .ToArray();

        return sectionResults;
    }

    private MenuSection? CreateFromSection(string title, string content, string command)
    {
        if (title == "uuBookKit")
        {
            _ldtBookkitUrl ??= content;
            return null;
        }

        Regex rowRegex = new(@$"{Environment.NewLine}  ([^ ]+(, [^ ]+)*)  +");
        (string name, string description)[] contentRows = rowRegex.Split($"{Environment.NewLine}{content}")
            .Skip(1)
            .Where(r => !r.StartsWith(","))
            .Select((item, index) => (item, index))
            .GroupBy(pair => pair.index / 2) // group pairs
            .Select(g => (g.First().item, g.Last().item))
            .ToArray();
        IMenuRow[] rows = contentRows.Select(pair => CreateFromLine(pair.name, pair.description, command)).ToArray();

        MenuSection section = new(title, rows);
        return section;
    }

    private IMenuRow CreateFromLine(string name, string description, string command)
    {
        description = string.Join(" ", description.Split(Environment.NewLine).Select(l => l.Trim()));

        string singleName = name.Split(',')[0];
        string areaCommand = string.IsNullOrEmpty(command)
            ? singleName
            : $"{command} {singleName}";

        // area
        if (description.StartsWith("[area]"))
        {
            MenuSection[] submenu = ParseCommand(areaCommand);
            return new MenuItemArea(name, description, submenu);
        }

        // command
        if (description.StartsWith("[command]"))
        {
            MenuSection[] submenu = string.IsNullOrEmpty(command)
                ? [] // top level command -> no submenu
                : ParseCommand(areaCommand);
            return new MenuItemCommand(name, description, submenu);
        }

        // arguments
        if (TryGetEnum(ref description, out string[] values))
        {
            TryGetDefault(ref description, out string? defaultValue);

            return new MenuItemArgumentSelect(name, description, values, defaultValue);
        }

        if (description.Contains("path"))
        {
            return new MenuItemArgumentPath(name, description);
        }

        if (description.Contains("indicates"))
        {
            return new MenuItemArgumentFlag(name, description);
        }

        return new MenuItemArgumentText(name, description);
    }

    private bool TryGetEnum(ref string description, out string[] values)
    {
        if (!description.TryGetIndex("Valid values:", out int valuesIndex, out int valuesEndIndex))
        {
            values = Array.Empty<string>();
            return false;
        }

        values = description.Cut(valuesEndIndex)
            .Split(",")
            .Select(v => v.Trim(' ', '.'))
            .ToArray();
        description = description.Cut(endIndex: valuesIndex).Trim();
        return true;
    }

    private bool TryGetDefault(ref string description, out string? defaultValue)
    {
        defaultValue = null;

        if (!description.StartsWith("(Default: "))
            return false;

        if (!description.TryGetIndex(")", out int defautIndex, out int defaultEndIndex))
            return false;

        defaultValue = description.Cut("(Default: ".Length, defautIndex);
        description = description.Cut(defaultEndIndex).Trim();
        return true;
    }

    public static ConfigData CreateConfig(string ldtPath)
    {
        Executor executor = new(ldtPath);
        ConfigBuilder builder = new(ldtPath, executor);
        return builder.CreateConfig();
    }
}