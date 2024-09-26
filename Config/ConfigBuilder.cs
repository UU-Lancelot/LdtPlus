using System.Text.RegularExpressions;
using LdtPlus.MenuData;

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
        string output = _executor.RunAndGetOutput(command);

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
        if (description.StartsWith("[area]"))
        {
            string areaCommand = string.IsNullOrEmpty(command)
                ? name.Split(',')[0]
                : $"{command} {name.Split(',')[0]}";
            MenuSection[] submenu = ParseCommand(areaCommand);
            return new MenuItemArea(name, description, submenu);
        }

        if (description.StartsWith("[command]"))
            return new MenuItemCommand(name, description);

        throw new NotSupportedException($"Unsupported line type: {description}");
    }

    public static ConfigData CreateConfig(string ldtPath)
    {
        Executor executor = new(ldtPath);
        ConfigBuilder builder = new(ldtPath, executor);
        return builder.CreateConfig();
    }
}