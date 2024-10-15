using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive.MenuData;
public record MenuItemArgumentSelect : IMenuRow, IMenuContainer
{
    public MenuItemArgumentSelect(string name, string description, IEnumerable<string> values, string? defaultValue = null)
    {
        Name = name;
        Description = description;
        DefaultValue = defaultValue ?? values.FirstOrDefault() ?? throw new Exception("No values for select argument");
        Sections = Enumerable.Repeat(new MenuSection("Values", values.Select(v => new MenuItemArgumentSelectValue(v, this))), 1);
    }

    public string Name { get; }
    public string Description { get; }
    public string DefaultValue { get; }
    public IEnumerable<IMenuItem> Navigation => [new MenuNavBack()];
    public IEnumerable<MenuSection> Sections { get; }
    public IEnumerable<IMenuItem> ItemOptions => [];

    public void OnSelect(Gui.Gui gui, MenuPosition position, Action<Command, string> setCommand)
    {
        position.EnterSelected();
    }
}