using LdtPlus.Gui.Interfaces;
using Spectre.Console;

namespace LdtPlus.Gui;
internal class GuiBase : IComponentContainer, IDisposable, IAsyncDisposable
{
    public GuiBase()
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
    public GuiBase Start()
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

            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            ctx.Refresh();
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        Rerender();
    }

    public ValueTask DisposeAsync()
    {
        _cancellationTokenSource.Cancel();
        Rerender();

        if (_showTableTask is not null)
            return new ValueTask(_showTableTask);

        return ValueTask.CompletedTask;
    }
    #endregion

    #region Rendering
    public void Add(IComponent item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiBase is not started");

        _keys.Add(item.Key);
        _mainFrame.AddRow(item.MainFrame);
    }

    public void Update(IComponent item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        int keyIndex = _keys.IndexOf(item.Key);
        if (keyIndex == -1)
        {
            Add(item);
            return;
        }

        _mainFrame.Rows.Update(keyIndex, 0, item.MainFrame);
    }

    public void Remove(IComponent item)
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        int keyIndex = _keys.IndexOf(item.Key);
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
        if (!_rerender.Task.IsCompleted)
            _rerender.SetResult();
    }
    #endregion

    public static GuiBase StartNew()
    {
        return new GuiBase()
            .Start();
    }
}