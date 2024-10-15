using System.Diagnostics.CodeAnalysis;
using LdtPlus.Interactive.Tools;

namespace LdtPlus.Interactive;
public class Input
{
    public Input(Gui.Gui gui)
    {
        _gui = gui;
        _input = new();
        _result = string.Empty;
    }

    private readonly Gui.Gui _gui;
    private readonly InputHandler _input;
    private string _result;
    private bool? _isResultOk;

    public bool TryGetResult(string message, [MaybeNullWhen(returnValue: false)] out string result)
    {
        do
        {
            _gui.Show(b => b.ShowInput(message, _result));
            _input.WaitForInput(OnSelect, OnExit, onChar: OnChar, onBackspace: OnBackspace);
        }
        while (_isResultOk == null);

        result = _isResultOk.Value ? _result : null;
        return _isResultOk.Value;
    }

    private void OnChar(char c)
    {
        _result += c;
    }

    private void OnBackspace()
    {
        if (_result.Length > 0)
            _result = _result[..^1];
    }

    private void OnSelect()
    {
        _isResultOk = true;
    }

    private void OnExit()
    {
        _isResultOk = false;
    }

    public static bool TryGetResult(Gui.Gui gui, string message, [MaybeNullWhen(returnValue: false)] out string result)
    {
        Input input = new(gui);
        return input.TryGetResult(message, out result);
    }
}