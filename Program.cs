using LdtPlus;
using LdtPlus.Config;
using LdtPlus.Gui;
using LdtPlus.Menu;

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

    path ??= "aa"; // menu.GetPath();

    if (config is null)
    {
        using (gui.UseLoader())
        {
            Executor executorForConfig = new(path);
            config = configIO.CreateConfig(executorForConfig);
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
    executor.Run("TODO");
}
