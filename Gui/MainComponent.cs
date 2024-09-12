using LdtPlus.Gui.Interfaces;
using Spectre.Console;

namespace LdtPlus.Gui;
public class MainComponent : IComponentContainer, IDisposable, IAsyncDisposable
{
    public MainComponent()
    {
        _mainFrame = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None,
        };
        _mainFrame.AddColumn(new TableColumn(string.Empty));
        _keys = [];
        _cancellationTokenSource = new CancellationTokenSource();
        _rerender = new TaskCompletionSource();
    }

    private Table _mainFrame;
    private List<string> _keys;
    private CancellationTokenSource _cancellationTokenSource;
    private TaskCompletionSource _rerender;
    private Task? _showTableTask;

    #region Lifecycle
    public MainComponent Start()
    {
        _showTableTask = Task.Run(() => AnsiConsole.Live(_mainFrame).Start(StartLive));

        return this;
    }

    private async void StartLive(LiveDisplayContext ctx)
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            await _rerender.Task;
            _rerender = new TaskCompletionSource();
            ctx.Refresh();
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
    public void Add(string key, IComponent item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        _keys.Add(key);
        _mainFrame.AddRow(item.MainFrame);
    }

    public void Update(string key, IComponent item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        int keyIndex = _keys.IndexOf(key);
        if (keyIndex == -1)
        {
            Add(key, item);
            return;
        }

        _mainFrame.Rows.Update(keyIndex, 0, item.MainFrame);
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

    public void Rerender()
    {
        _rerender.SetResult();
    }
    #endregion

    public static MainComponent StartNew()
    {
        return new MainComponent()
            .Start();
    }
}