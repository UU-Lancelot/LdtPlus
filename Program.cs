using LdtPlus;
using LdtPlus.Config;
using LdtPlus.Gui;
using LdtPlus.Menu;

await using (Gui gui = new())
{
    ConfigIO config = new();

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
        // config.LdtPath = menu.GetPath();
    }

    if (!isConfigLoaded)
    {
        using (gui.UseLoader())
        {
            config.CreateConfig();
        }
    }

    // menu
    Menu menu = new(gui, config.Config.Menu);
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
                menu.RefreshMenu(config.Config.Menu);
                break;
            case Command.FavouriteRename:
                if (parameter is null || !Input.TryGetResult(gui, "New name", out string? newName))
                    continue;

                config.RenameFavourite(parameter, newName);
                menu.RefreshMenu(config.Config.Menu);
                break;
            case Command.FavouriteDelete:
                if (parameter is null)
                    continue;

                config.DeleteFavourite(parameter);
                menu.RefreshMenu(config.Config.Menu);
                break;
        }
    } while (command != Command.Run || parameter is null);
    config.AddRecent(parameter);

    // run
    Executor executor = new();
    executor.Run("TODO");
}
