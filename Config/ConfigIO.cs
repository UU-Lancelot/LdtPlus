using LdtPlus.Config.RawData;
using LdtPlus.MenuData;
using Tomlyn;

namespace LdtPlus.Config;
public class ConfigIO
{
    public string? LdtPath { get; set; }
    public ConfigData Config => _config ?? throw new Exception("Config is not created yet");

    private ConfigData? _config;

    public bool TryLoadConfig()
    {
        try
        {
            // get config file
            string ldtPlusConfigPath = GetPath();
            if (!File.Exists(ldtPlusConfigPath))
                return false;

            // parse config file
            string configString = File.ReadAllText(ldtPlusConfigPath);
            ConfigRaw rawConfig = Toml.ToModel<ConfigRaw>(configString);

            // create structure
            _config = ConfigData.FromRaw(rawConfig);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool TryFindExecutable()
    {
#warning TODO: TryFindExecutable
        return true;
    }

    public void CreateConfig()
    {
#warning TODO: CreateConfig

    }

    public void AddRecent(string command)
    {
        Config.Recent.Insert(0, new MenuItemRecent(command));

        if (Config.Recent.Count > 100)
            Config.Recent.RemoveAt(Config.Recent.Count - 1);

        SaveConfig();
    }

    public void AddFavourite(string name, string command)
    {
        Config.Favourites.Add(new MenuItemFavourite(name, command));
        SaveConfig();
    }

    public void RenameFavourite(string name, string newName)
    {
        int favouriteIndex = Config.Favourites.FindIndex(f => f.Name == name);
        if (favouriteIndex < 0)
            return;

        Config.Favourites[favouriteIndex] = new MenuItemFavourite(newName, Config.Favourites[favouriteIndex].Command);
        SaveConfig();
    }

    public void DeleteFavourite(string name)
    {
        int favouriteIndex = Config.Favourites.FindIndex(f => f.Name == name);
        if (favouriteIndex < 0)
            return;

        Config.Favourites.RemoveAt(favouriteIndex);
        SaveConfig();
    }

    private void SaveConfig()
    {
        // get content
        ConfigRaw rawConfig = Config.ToRaw();
        string configString = Toml.FromModel(rawConfig);

        // save
        string ldtPlusConfigPath = GetPath();
        File.WriteAllText(ldtPlusConfigPath, configString);
    }

    private string GetPath()
    {
        string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string ldtPlusConfigPath = Path.Combine(homeDir, ".ldt", "ldtplus.config.toml");
        return ldtPlusConfigPath;
    }
}