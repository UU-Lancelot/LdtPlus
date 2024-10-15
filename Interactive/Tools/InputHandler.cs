namespace LdtPlus.Interactive.Tools;
public class InputHandler
{
    public void WaitForInput(
        Action? onSelect = null,
        Action? onExit = null,
        Action<ConsoleKey>? onMove = null,
        Action<char>? onChar = null,
        Action? onBackspace = null)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        switch (keyInfo.Key)
        {
            case ConsoleKey.Enter:
            case ConsoleKey.Spacebar:
                onSelect?.Invoke();
                break;
            case ConsoleKey.Escape:
                onExit?.Invoke();
                break;
            case ConsoleKey.UpArrow:
            case ConsoleKey.DownArrow:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.RightArrow:
                onMove?.Invoke(keyInfo.Key);
                break;
            case ConsoleKey.Backspace:
                onBackspace?.Invoke();
                break;
            default:
                // ignore invisible characters
                if (char.IsControl(keyInfo.KeyChar) || char.IsWhiteSpace(keyInfo.KeyChar))
                    break;

                onChar?.Invoke(keyInfo.KeyChar);
                break;
        }
    }
}