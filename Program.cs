using LdtPlus;
using LdtPlus.Config;
using LdtPlus.Gui;
using LdtPlus.Interactive;
using LdtPlus.Interactive.MenuResults;

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
        PathInput pathMenu = new(gui, currentDir, fileOnly: true, title: "Path to LDT");
        Result pathResult = pathMenu.GetCommand();
        if (pathResult is ResultQuit)
            return;
        if (pathResult is ResultSelectPath selectedPathResult)
            path = selectedPathResult.Path;

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
    
    gui.AddSubtitle(config);

    // menu
    Menu menu = new(gui, config.Menu);
    Result result;
    do
    {
        result = menu.GetCommand();

        // quit
        if (result is ResultQuit)
            return;
        
        // favourites
        else if (result is ResultAddFavourite addFavourite)
        {
            config.AddFavourite(addFavourite.Name, addFavourite.Command);
            configIO.SaveConfig(config);
            menu.RefreshMenu(config.Menu);
        }
        else if (result is ResultRenameFavourite renameFavourite)
        {
                config.RenameFavourite(renameFavourite.OldName, renameFavourite.NewName);
                configIO.SaveConfig(config);
                menu.RefreshMenu(config.Menu);
        }
        else if (result is ResultDeleteFavourite deleteFavourite)
        {
                config.DeleteFavourite(deleteFavourite.Name);
                configIO.SaveConfig(config);
                menu.RefreshMenu(config.Menu);
        }
    } while (result is not ResultRun);
    ResultRun run = (ResultRun)result;
    config.AddRecent(run.Command);
    configIO.SaveConfig(config);

    // run
    Executor executor = new(path);
    executor.Run(gui, run.Command);

    if (run.Command == "update")
    {
        using (gui.UseLoader("Updating config..."))
        {
            config = ConfigBuilder.CreateConfig(path);
            configIO.SaveConfig(config);
        }
    }
}
