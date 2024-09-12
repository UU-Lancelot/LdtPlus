using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
public class MenuComponent : IComponent, IComponentContainer
{
    public MenuComponent(IComponentContainer parent)
    {
        _parent = parent;
        _key = Guid.NewGuid().ToString();
        _keys = new();
        _components = new();
        _empty = new Text(string.Empty);
    }

    private readonly IComponentContainer _parent;
    private string _key;
    private List<string> _keys;
    private List<IComponent> _components;
    private IRenderable _empty;

    public IRenderable MainFrame => _components.LastOrDefault()?.MainFrame ?? _empty;

    public void Rerender()
    {
        _parent.Update(_key, this);
    }

    public void Add(string key, IComponent component)
    {
        _keys.Add(key);
        _components.Add(component);

        _parent.Update(_key, this);
    }

    public void Update(string key, IComponent component)
    {
        int index = _keys.IndexOf(key);

        // no key -> add
        if (index == -1)
        {
            Add(key, component);
            return;
        }

        _components[index] = component;
        if (index == _components.Count - 1)
            _parent.Update(_key, this);
    }

    public void Remove(string key)
    {
        int index = _keys.IndexOf(key);

        // no key -> nothing to remove
        if (index == -1)
            return;

        _keys.RemoveAt(index);
        _components.RemoveAt(index);
        if (index == _components.Count)
            _parent.Update(_key, this);
    }
}