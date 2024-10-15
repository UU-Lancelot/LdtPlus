using System.Diagnostics;
using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class ProcessComponent : IComponent
{
    internal ProcessComponent(IComponentContainer parent, Process process, string? key = null)
    {
        _parent = parent;
        _process = process;

        Key = key ?? "process";
        _text = "";

        Init();
    }

    private readonly IComponentContainer _parent;
    private readonly Process _process;
    private string _text;

    public string Key { get; }
    public IRenderable MainFrame => new Text(_text);

    public void Init()
    {
        _process.OutputDataReceived += OnOutput;
        _process.ErrorDataReceived += OnError;
    }

    public void OnOutput(object sender, DataReceivedEventArgs e)
    {
        _text += e.Data;
        _parent.Update(this);
        _parent.Rerender();
    }

    public void OnError(object sender, DataReceivedEventArgs e)
    {
        _text += e.Data;
        _parent.Update(this);
        _parent.Rerender();
    }
}