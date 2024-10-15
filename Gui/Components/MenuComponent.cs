using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.MenuData;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class MenuComponent : IComponent
{
    public MenuComponent(MenuPosition menu, string? key = null)
    {
        _menu = menu;
        _mainFrame = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None,
        };
        _mainFrame.AddColumn(string.Empty);
        Key = key ?? "menu";

        UpdateVisible();
    }

    private readonly MenuPosition _menu;
    private Table _mainFrame;

    public IRenderable MainFrame => _mainFrame;
    public string Key { get; }

    private void UpdateVisible()
    {
        // clear previous
        _mainFrame.Rows.Clear();

        // add navigation
        if (_menu.NavigationFiltered.Any())
        {
            Grid navigation = new();
            foreach (IMenuItem _ in _menu.NavigationFiltered)
            {
                navigation.AddColumn().AddColumn(); // select, nav
            }
            navigation.AddRow(_menu.NavigationFiltered.SelectMany<IMenuItem, string>(n => [n.Name == _menu.ActiveSelection.SelectedKey ? ">" : " ", n.Name]).ToArray());
            _mainFrame.AddRow(navigation);
        }

        // add all sections
        foreach (MenuSection section in _menu.SectionsFiltered)
        {
            _mainFrame.AddEmptyRow();
            _mainFrame.AddRow(section.Title);
            _mainFrame.AddRow(new Rule());

            Grid sectionGrid = new();
            sectionGrid.AddColumn().AddColumn().AddColumn(); // select, name, description
            foreach (IMenuItem _ in _menu.CurrentMenu.ItemOptions)
            {
                sectionGrid.AddColumn().AddColumn(); // select, name
            }
            foreach (IMenuRow item in section.Submenu)
            {
                List<string> row = new List<string>
                {
                    item.Name == _menu.ActiveSelection.SelectedKey ? ">" : " ",
                    Markup.Escape(item.Name),
                    Markup.Escape(item.Description),
                };
                foreach (IMenuItem option in _menu.CurrentMenu.ItemOptions)
                {
                    row.Add($"{item.Name}~{option.Name}" == _menu.ActiveSelection.SelectedKey ? ">" : " ");
                    row.Add(option.Name);
                }
                sectionGrid.AddRow(row.ToArray());
            }
            _mainFrame.AddRow(sectionGrid);
        }
    }
}