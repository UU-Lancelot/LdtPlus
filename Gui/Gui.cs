using LdtPlus.Gui.Interfaces;

namespace LdtPlus.Gui;
public class Gui : IDisposable, IAsyncDisposable
{
    public Gui()
    {
        _guiBase = new GuiBase();
        _showedComponents = Array.Empty<IComponent>();

        _guiBase.Start();
        _guiBase.Add(new TitleComponent(_guiBase));
    }

    private GuiBase _guiBase;
    private IComponent[] _showedComponents;

    public IDisposable UseLoader()
    {
        LoaderComponent loader = new(_guiBase);
        _guiBase.Add(loader);
        _guiBase.Rerender();

        return loader;
    }

    public Gui Show(Action<GuiBatch> batch)
    {
        List<IComponent> components = new();
        GuiBatch guiBatch = new(_guiBase, components);
        batch(guiBatch);

        // remove unused components
        string[] keys = components.Select(c => c.Key).ToArray();
        foreach (IComponent componentToRemove in _showedComponents.Where(c => !keys.Contains(c.Key)))
        {
            _guiBase.Remove(componentToRemove);
        }

        // update/add other
        foreach (IComponent component in components)
        {
            _guiBase.Update(component);
        }
        _guiBase.Rerender();

        return this;
    }

    public void Dispose()
    {
        _guiBase.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _guiBase.DisposeAsync();
    }
}