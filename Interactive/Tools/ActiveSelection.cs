namespace LdtPlus.Interactive.Tools;
public class ActiveSelection
{
    public ActiveSelection(string[][] options)
    {
        _options = options;
        _selectedRow = 0;
        _selectedColumn = 0;
        SelectedKey = string.Empty;
        UpdateKey();
    }

    private string[][] _options;
    private int _selectedRow;
    private int _selectedColumn;

    public string SelectedKey { get; private set; }

    public void UpdateOptions(string[][] options)
    {
        _options = options;

        if (TryFindKey(SelectedKey, out _selectedRow, out _selectedColumn))
            return;

        _selectedRow = 0;
        _selectedColumn = 0;
        UpdateKey();
    }

    public void ResetPosition()
    {
        _selectedRow = 0;
        _selectedColumn = 0;
        UpdateKey();
    }

    public void MoveUp()
    {
        _selectedRow = _selectedRow != 0
            ? _selectedRow - 1
            : _options.Length - 1;
        _selectedColumn = Math.Min(_selectedColumn, _options[_selectedRow].Length - 1);
        UpdateKey();
    }
    public void MoveDown()
    {
        _selectedRow = _selectedRow != _options.Length - 1
            ? _selectedRow + 1
            : 0;
        _selectedColumn = Math.Min(_selectedColumn, _options[_selectedRow].Length - 1);
        UpdateKey();
    }
    public void MoveLeft()
    {
        _selectedColumn = _selectedColumn != 0
            ? _selectedColumn - 1
            : _options[_selectedRow].Length - 1;
        UpdateKey();
    }
    public void MoveRight()
    {
        _selectedColumn = _selectedColumn != _options[_selectedRow].Length - 1
            ? _selectedColumn + 1
            : 0;
        UpdateKey();
    }

    private bool TryFindKey(string key, out int row, out int column)
    {
        for (int i = 0; i < _options.Length; i++)
        {
            for (int j = 0; j < _options[i].Length; j++)
            {
                if (_options[i][j] == key)
                {
                    row = i;
                    column = j;
                    return true;
                }
            }
        }

        row = -1;
        column = -1;
        return false;
    }

    private void UpdateKey()
    {
        if (!_options.Any())
        {
            SelectedKey = string.Empty;
            return;
        }

        SelectedKey = _options[_selectedRow][_selectedColumn];
    }
}