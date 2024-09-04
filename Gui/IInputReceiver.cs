namespace LdtPlus.Gui;
public interface IInputReceiver
{
    void HandleInput(ConsoleKeyInfo keyInfo, out bool passToNextReceiver);
}