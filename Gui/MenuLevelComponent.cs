using LdtPlus.Gui.Interfaces;
using LdtPlus.Menu;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
public class MenuLevelComponent : IComponent, IDisposable
{
    public MenuLevelComponent(IComponentContainer parent, IMenuContainer menu)
    {
        _parent = parent;
        _menu = menu;
        _key = Guid.NewGuid().ToString();
        _mainFrame = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None,
        };
        _mainFrame.AddColumn(string.Empty);
        _filter = string.Empty;
    }

    private readonly IComponentContainer _parent;
    private readonly IMenuContainer _menu;
    private string _key;
    private Table _mainFrame;
    private string? _selectedKey;
    private string _filter;
    
    private IEnumerable<MenuSection> SectionsFiltered => _menu.Sections
        .Select(s => new MenuSection(s.Title, s.Submenu.Where(m => m.Name.StartsWith(_filter, ignoreCase: true, null))))
        .Where(s => s.Submenu.Any());
    private IEnumerable<string> NavigationFiltered => _menu.Navigation.Where(n => n.StartsWith(_filter, ignoreCase: true, null));

    public IRenderable MainFrame => _mainFrame;

    public void Rerender()
    {
        // clear previous
        _mainFrame.Rows.Clear();

        // add navigation
        if (NavigationFiltered.Any())
        {
            Grid navigation = new();
            foreach (string _ in NavigationFiltered)
            {
                navigation.AddColumn().AddColumn(); // select, nav
            }
            navigation.AddRow(NavigationFiltered.SelectMany<string, string>(n => [n == _selectedKey ? ">" : " ", n]).ToArray());
            _mainFrame.AddRow(navigation);
        }

        // add all sections
        foreach (MenuSection section in SectionsFiltered)
        {
            Text sectionTitle = new(section.Title);
            _mainFrame.AddRow(sectionTitle);

            Grid sectionGrid = new();
            sectionGrid.AddColumn().AddColumn().AddColumn(); // select, name, description
            foreach (IMenuItem item in section.Submenu)
            {
                sectionGrid.AddRow([item.Name == _selectedKey ? ">" : " ", item.Name, item.Description]);
            }
            _mainFrame.AddRow(sectionGrid);
        }

        _parent.Update(_key, this);
    }

    public void SetSelectedKey(string key)
    {
        _selectedKey = key;
        Rerender();
    }

    public void SetFilter(string filter)
    {
        _filter = filter;
        Rerender();
    }

    public void Dispose()
    {
        _parent.Remove(_key);
    }
}