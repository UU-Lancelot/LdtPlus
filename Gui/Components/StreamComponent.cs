using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class StreamComponent : IComponent, IDisposable
{
    public StreamComponent(IComponentContainer parent, Stream stream, string? key = null)
    {
        _parent = parent;
        _stream = stream;
        _text = "";
        _cancellationTokenSource = new CancellationTokenSource();
        Key = key ?? "stream";

        _runTask = Run();
    }

    private readonly IComponentContainer _parent;
    private readonly Stream _stream;
    private string _text;
    private CancellationTokenSource _cancellationTokenSource;
    private Task _runTask;

    public string Key { get; }
    public IRenderable MainFrame => new Text(_text);

    private async Task Run()
    {
        int bufferSize = 256;
        int currentPosition = 0;
        byte[] buffer = new byte[bufferSize];
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            int read = _stream.Read(buffer, currentPosition, bufferSize);
            currentPosition += read;

            if (read > 0)
            {
                _text += System.Text.Encoding.UTF8.GetString(buffer, 0, read);
                _parent.Update(this);
                _parent.Rerender();
                continue;
            }

            await Task.Delay(100);
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }
}