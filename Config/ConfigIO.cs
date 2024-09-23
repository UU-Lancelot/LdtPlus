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

    }

    public void AddFavourite(string name, string command)
    {

    }

    public void RenameFavourite(string name, string newName)
    {

    }

    public void DeleteFavourite(string name)
    {

    }
}