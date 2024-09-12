using LdtPlus.Gui.Interfaces;

namespace LdtPlus.Gui.Tools;
public class InputHandler
{
    public InputHandler()
    {
        _receivers = new();
    }

    private readonly List<IInputReceiver> _receivers;
    public void Register(IInputReceiver receiver)
    {
        _receivers.Add(receiver);
    }

    public void Unregister(IInputReceiver receiver)
    {
        _receivers.Remove(receiver);
    }

    public void WaitForInput()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        foreach (IInputReceiver receiver in _receivers.Reverse<IInputReceiver>())
        {
            receiver.HandleInput(keyInfo, out bool passToNextReceiver);
            if (!passToNextReceiver)
                break;
        }
    }
}