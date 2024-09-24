namespace LdtPlus.Config;
public class ConfigIO
{
    public string? LdtPath { get; set; }
    public ConfigData Config { get; } = new();

    public bool TryLoadConfig()
    {
        string currentDir = Environment.CurrentDirectory;
        string? dllPath = Path.GetFullPath(System.Reflection.Assembly.GetEntryAssembly()?.Location ?? ".");
        string? exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
        string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return true;
    }

    public bool TryFindExecutable()
    {
        return true;
    }

    public void CreateConfig()
    {

    }

    public void AddRecent(string command)
    {
        Config.Recent.Insert(0, new MenuData.MenuItemRecent(command));

        if (Config.Recent.Count > 100)
            Config.Recent.RemoveAt(Config.Recent.Count - 1);
    }

    public void AddFavourite(string name, string command)
    {
        Config.Favourites.Add(new MenuData.MenuItemFavourite(name, command));
    }

    public void RenameFavourite(string name, string newName)
    {
        int favouriteIndex = Config.Favourites.FindIndex(f => f.Name == name);
        if (favouriteIndex >= 0)
            Config.Favourites[favouriteIndex] = new MenuData.MenuItemFavourite(newName, Config.Favourites[favouriteIndex].Command);
    }

    public void DeleteFavourite(string name)
    {
        int favouriteIndex = Config.Favourites.FindIndex(f => f.Name == name);
        if (favouriteIndex >= 0)
            Config.Favourites.RemoveAt(favouriteIndex);
    }
}