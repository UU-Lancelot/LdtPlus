using LdtPlus;
using LdtPlus.Config;
using LdtPlus.Gui;
using LdtPlus.Menu;

// Console.ReadKey(true);
await using (Gui gui = new())
{
    ConfigIO configIO = new();

    // Get config
    ConfigData? config = null;
    string? path = null;
    using (gui.UseLoader())
    {
        config = configIO.TryLoadConfig();
        path = configIO.TryFindExecutable(config);
    }

    if (path is null)
    {
        string currentDir = Environment.CurrentDirectory;
        PathInput pathMenu = new(gui, currentDir, fileOnly: true);
        Command pathCommand = pathMenu.GetCommand(out string? pathParameter);
        switch (pathCommand)
        {
            case Command.Exit:
                return;
            case Command.Run:
                path = pathParameter;
                break;
        }

        // should not happen, just remove warning
        if (path is null)
            return;
    }

    if (config is null)
    {
        using (gui.UseLoader("Creating config..."))
        {
            config = ConfigBuilder.CreateConfig(path);
            configIO.SaveConfig(config);
        }
    }

    // menu
    Menu menu = new(gui, config.Menu);
    Command command;
    string? parameter;
    do
    {
        command = menu.GetCommand(out parameter);
        switch (command)
        {
            case Command.Exit:
                return;
            case Command.FavouriteAdd:
                if (parameter is null || !Input.TryGetResult(gui, "Favourite name", out string? name))
                    continue;

                config.AddFavourite(name, parameter);
                configIO.SaveConfig(config);
                menu.RefreshMenu(config.Menu);
                break;
            case Command.FavouriteRename:
                if (parameter is null || !Input.TryGetResult(gui, "New name", out string? newName))
                    continue;

                config.RenameFavourite(parameter, newName);
                configIO.SaveConfig(config);
                menu.RefreshMenu(config.Menu);
                break;
            case Command.FavouriteDelete:
                if (parameter is null)
                    continue;

                config.DeleteFavourite(parameter);
                configIO.SaveConfig(config);
                menu.RefreshMenu(config.Menu);
                break;
        }
    } while (command != Command.Run || parameter is null);
    config.AddRecent(parameter);
    configIO.SaveConfig(config);

    // run
    Executor executor = new(path);
    executor.Run(gui, parameter);

    if (parameter == "update")
    {
#warning TODO: Update config
    }
}
