using LdtPlus.MenuData;

namespace LdtPlus.Config;
public class ConfigIO
{
    public string? LdtPath { get; set; }
    public ConfigData Config { get; } = new(
        [
            new("available areas of use:", [
                new MenuItemArea("build", "[area] management of installation packages preparation", [
                    new("available commands of 'build' area:", [
                        new MenuCommand { Name = "config", Description = "[command] launches a wizard to assist in creating the Builder.Config.toml file" },
                        new MenuCommand { Name = "package", Description = "[command] builds an installation package based on the provided Builder.Config.toml file" },
                    ]),
                ]),
                new MenuItemArea("configs", "[area] management and manipulation with Lancelot configuration files", [
                    new("available commands of 'configs' area:", [
                        new MenuCommand { Name = "apply-template", Description = "[command] applies the template configuration(s) to the base file (e.g. EnifConfig.xml)" },
                        new MenuCommand { Name = "app-params-trim-xpath", Description = "[command] creates default project application.params.toml file from a product file." },
                    ]),
                ]),
            ]),
            new("tool configuration and management:", [
                new MenuItemArea("auth, authentication", "[area] management of stored tokens and credentials", [
                    new("available commands of 'authentication' area:", [
                        new MenuCommand { Name = "get-token", Description = "[command] displays the token for the provided authentication provider or URI" },
                        new MenuCommand { Name = "store-credentials", Description = "[command] encrypts and stores the user-entered credentials (location: %USERPROFILE%\\.ldt\\.<provider>-credentials)" },
                    ]),
                ]),
                new MenuCommand { Name = "version",  Description = "[command] displays information about the LancelotDeploymentTool version" },
            ]),
        ],
        new(),
        new()
    );

    public bool TryLoadConfig()
    {
        #warning TODO: TryLoadConfig
        string currentDir = Environment.CurrentDirectory;
        string? dllPath = Path.GetFullPath(System.Reflection.Assembly.GetEntryAssembly()?.Location ?? ".");
        string? exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
        string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return true;
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