using Spectre.Console;
using Spectre.Console.Rendering;

namespace LdtPlus.Gui;
public class ConsoleMain : IDisposable, IAsyncDisposable
{
    public ConsoleMain()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _mainFrame = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None,
        };
        _mainFrame.AddColumn(new TableColumn(string.Empty));
        _keys = [];
    }

    private bool _rerender;
    private Table _mainFrame;
    private List<string> _keys;
    private Task? _showTableTask;
    private CancellationTokenSource _cancellationTokenSource;

    #region Lifecycle
    public ConsoleMain Start()
    {
        _showTableTask = Task.Run(() => AnsiConsole.Live(_mainFrame).Start(StartLive));

        return this;
    }

    private void StartLive(LiveDisplayContext ctx)
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            Thread.Sleep(50);

            if (_rerender)
            {
                _rerender = false;
                ctx.Refresh();
            }
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
    }

    public ValueTask DisposeAsync()
    {
        _cancellationTokenSource.Cancel();

        if (_showTableTask is not null)
            return new ValueTask(_showTableTask);

        return ValueTask.CompletedTask;
    }
    #endregion

    #region Rendering
    public void Add(string key, IRenderable item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        _keys.Add(key);
        _mainFrame.AddRow(item);
    }

    public void Update(string key, IRenderable item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        int keyIndex = _keys.IndexOf(key);
        if (keyIndex == -1)
        {
            // warning
            return;
        }

        _mainFrame.Rows.Update(keyIndex, 0, item);
    }

    public void Remove(string key)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        int keyIndex = _keys.IndexOf(key);
        if (keyIndex == -1)
        {
            // warning
            return;
        }

        _mainFrame.Rows.RemoveAt(keyIndex);
        _keys.RemoveAt(keyIndex);
    }

    public void Clear()
    {
        _mainFrame.Rows.Clear();
    }

    public void Show()
    {
        _rerender = true;
    }
    #endregion

    public static ConsoleMain StartNew()
    {
        return new ConsoleMain()
            .Start();
    }
}