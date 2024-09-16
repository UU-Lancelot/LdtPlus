using LdtPlus.Gui.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;

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

    private void StartLive(LiveDisplayContext ctx)
    {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
            ctx.Refresh();

            WaitFor(_rerender.Task, _cancellationTokenSource.Token);
            _rerender = new TaskCompletionSource();
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
    public void Add(string key, IRenderable renderable)
    {
        _keys.Add(key);
        _mainFrame.AddRow(renderable);
    }
    public void Add(IComponent item)
    {
        Add(item.Key, item.MainFrame);
    }

    public void Update(string key, IRenderable renderable)
    {
        int keyIndex = _keys.IndexOf(key);
        if (keyIndex == -1)
        {
            Add(key, renderable);
            return;
        }

        _mainFrame.Rows.Update(keyIndex, 0, renderable);
    }
    public void Update(IComponent item)
    {
        Update(item.Key, item.MainFrame);
    }

    public void Remove(string key)
    {
        int keyIndex = _keys.IndexOf(key);
        if (keyIndex == -1)
        {
            // warning
            return;
        }

        _mainFrame.Rows.RemoveAt(keyIndex);
        _keys.RemoveAt(keyIndex);
    }
    public void Remove(IComponent item)
    {
        Remove(item.Key);
    }

    public void Rerender()
    {
        if (_showTableTask is null)
            throw new InvalidOperationException("GuiVisible is not started");

        if (!_rerender.Task.IsCompleted)
            _rerender.SetResult();
    }
    #endregion

    #region Tools
    private void WaitFor(Task task, CancellationToken cancellationToken)
    {
        try
        {
            task.Wait(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
    }
    #endregion

    public static GuiBase StartNew()
    {
        return new GuiBase()
            .Start();
    }
}