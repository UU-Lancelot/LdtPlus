using LdtPlus;
using LdtPlus.Config;
using LdtPlus.Gui;
using LdtPlus.Menu;

Console.ReadKey(true);
await using (Gui gui = new())
{
    ConfigIO config = new();
    Menu menu = new(gui, config.Config.Menu);

    // Get config
    bool isConfigLoaded;
    bool isExecutableFound;
    using (gui.UseLoader())
    {
        isConfigLoaded = config.TryLoadConfig();
        isExecutableFound = config.TryFindExecutable();
    }

    if (!isExecutableFound)
    {
        config.LdtPath = menu.GetPath();
    }

    if (!isConfigLoaded)
    {
        using (gui.UseLoader())
        {
            config.CreateConfig();
        }
    }

    // menu
    Command command;
    do
    {
        command = menu.GetCommand();
        if (command == Command.Exit)
            return;
    } while (command != Command.Run);

    // run
    Executor executor = new();
    executor.Run("TODO");
}
