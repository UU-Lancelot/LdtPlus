using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
internal class StreamComponent : IComponent, IAsyncDisposable
{
    private const int BUFFER_SIZE = 256;
    public StreamComponent(IComponentContainer parent, Stream streamOutput, Stream streamError, string? key = null)
    {
        _parent = parent;
        _streamOutput = streamOutput;
        _streamError = streamError;
        _text = "";
        _cancellationTokenSource = new CancellationTokenSource();
        _positions = new Dictionary<Stream, int>();
        Key = key ?? "stream";

        _runTask = Run();
    }

    private readonly IComponentContainer _parent;
    private readonly Stream _streamOutput;
    private readonly Stream _streamError;
    private string _text;
    private CancellationTokenSource _cancellationTokenSource;
    private Task _runTask;
    private Dictionary<Stream, int> _positions;

    public string Key { get; }
    public IRenderable MainFrame => new Text(_text);

    private async Task Run()
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            await Task.Delay(100);

            TryReadAll(_streamOutput);
            TryReadAll(_streamError);
        }
    }

    private bool TryReadAll(Stream stream)
    {
        int startPosition = _positions.TryGetValue(stream, out int position) ? position : 0;
        int currentPosition = startPosition;
        byte[] buffer = new byte[BUFFER_SIZE];

        while (true)
        {
            int read = stream.Read(buffer, currentPosition, BUFFER_SIZE);
            currentPosition += read;

            if (read == 0)
                break;

            _text += System.Text.Encoding.UTF8.GetString(buffer, 0, read);
            _parent.Update(this);
            _parent.Rerender();
        }

        _positions[stream] = currentPosition;
        return currentPosition != startPosition;
    }

    public ValueTask DisposeAsync()
    {
        _cancellationTokenSource.Cancel();

        return new ValueTask(_runTask);
    }
}