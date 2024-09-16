using LdtPlus.Gui.Interfaces;
using LdtPlus.Gui.Tools;
using LdtPlus.MenuData;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class MenuComponent : IComponent, IDisposable
{
    public MenuComponent(IComponentContainer parent, MenuPosition menu)
    {
        _parent = parent;
        _menu = menu;
        _mainFrame = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None,
        };
        _mainFrame.AddColumn(string.Empty);
        Key = "menu";

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
            foreach (string _ in _menu.NavigationFiltered)
            {
                navigation.AddColumn().AddColumn(); // select, nav
            }
            navigation.AddRow(_menu.NavigationFiltered.SelectMany<string, string>(n => [n == _menu.ActiveSelection.SelectedKey ? ">" : " ", n]).ToArray());
            _mainFrame.AddRow(navigation);
        }

        // add all sections
        foreach (MenuSection section in _menu.SectionsFiltered)
        {
            Text sectionTitle = new(section.Title);
            _mainFrame.AddRow(sectionTitle);

            Grid sectionGrid = new();
            sectionGrid.AddColumn().AddColumn().AddColumn(); // select, name, description
            foreach (IMenuItem item in section.Submenu)
            {
                sectionGrid.AddRow([item.Name == _menu.ActiveSelection.SelectedKey ? ">" : " ", item.Name, item.Description]);
            }
            _mainFrame.AddRow(sectionGrid);
        }

        _parent.Update(this);
    }

    public void Dispose()
    {
        _parent.Remove(this);
    }
}