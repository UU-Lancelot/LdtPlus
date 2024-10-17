using LdtPlus.Config;
using LdtPlus.Gui.Interfaces;

namespace LdtPlus.Gui;
public class Gui : IDisposable, IAsyncDisposable
{
    public Gui()
    {
        _guiBase = new GuiBase();
        _showedComponents = Array.Empty<IComponent>();

        Init();
        _guiBase.Start();
    }

    private GuiBase _guiBase;
    private IComponent[] _showedComponents;

    public Gui Init()
    {
        _guiBase.Update(new TitleComponent());

        if (_guiBase.IsRunning)
            _guiBase.Rerender();

        return this;
    }

    public Gui AddSubtitle(ConfigData config)
    {
        _guiBase.Update(new SubTitleComponent(config.LdtBookkitUrl));
        if (_guiBase.IsRunning)
            _guiBase.Rerender();

        return this;
    }

    public Gui Clear()
    {
        foreach (IComponent component in _showedComponents)
        {
            _guiBase.Remove(component);
        }

        _showedComponents = Array.Empty<IComponent>();
        _guiBase.Rerender();

        return this;
    }

    public IDisposable UseLoader(string text = "Loading...")
    {
        LoaderComponent loader = new(_guiBase, text);
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
            if (componentToRemove is IAsyncDisposable disposableComponent)
                disposableComponent.DisposeAsync().AsTask().Wait();

            _guiBase.Remove(componentToRemove);
        }

        // update/add other
        foreach (IComponent component in components)
        {
            _guiBase.Update(component);
        }
        _showedComponents = components.ToArray();
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