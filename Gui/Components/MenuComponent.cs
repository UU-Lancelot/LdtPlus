using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.MenuData;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class MenuComponent : IComponent, IDisposable
{
    public MenuComponent(IComponentContainer parent, MenuPosition menu, string? key = null)
    {
        _parent = parent;
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

    private readonly IComponentContainer _parent;
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
            foreach (IMenuRow item in section.Submenu)
            {
                sectionGrid.AddRow([item.Name == _menu.ActiveSelection.SelectedKey ? ">" : " ", Markup.Escape(item.Name), Markup.Escape(item.Description)]);
            }
            _mainFrame.AddRow(sectionGrid);
        }
    }

    public void Dispose()
    {
        _parent.Remove(this);
    }
}