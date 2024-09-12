namespace LdtPlus.Gui.Interfaces;
public interface IInputReceiver
{
    void HandleInput(ConsoleKeyInfo keyInfo, out bool passToNextReceiver);
}