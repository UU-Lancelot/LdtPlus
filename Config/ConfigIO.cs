using System.Diagnostics.CodeAnalysis;
using LdtPlus.Config.RawData;
using Tomlyn;

namespace LdtPlus.Config;
public class ConfigIO
{
    public ConfigData? TryLoadConfig()
    {
        try
        {
            // get config file
            string ldtPlusConfigPath = GetConfigPath();
            if (!File.Exists(ldtPlusConfigPath))
                return null;

            // parse config file
            string configString = File.ReadAllText(ldtPlusConfigPath);
            ConfigRaw rawConfig = Toml.ToModel<ConfigRaw>(configString);

            // create structure
            return ConfigData.FromRaw(rawConfig);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string? TryFindExecutable(ConfigData? data)
    {
        // config path
        if (data is not null && File.Exists(data.LdtPath))
            return data.LdtPath;

        // exe path
        string? exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName;
        string? exeDir = Path.GetDirectoryName(exePath);
        if (exeDir is not null && DirContainsLdt(exeDir, out string? ldtPath))
            return ldtPath;

        // current dir
        string currentDir = Environment.CurrentDirectory;
        if (DirContainsLdt(currentDir, out ldtPath))
            return ldtPath;

        // PATH env
        string[] paths = Environment.GetEnvironmentVariable("PATH")?.Split(';') ?? [];
        foreach (string path in paths)
        {
            if (DirContainsLdt(path, out ldtPath))
                return ldtPath;
        }

        return null;
    }

    public void SaveConfig(ConfigData data)
    {
        // get content
        ConfigRaw rawConfig = data.ToRaw();
        string configString = Toml.FromModel(rawConfig);

        // save
        string ldtPlusConfigPath = GetConfigPath();
        File.WriteAllText(ldtPlusConfigPath, configString);
    }

    private string GetConfigPath()
    {
        string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string ldtPlusConfigPath = Path.Combine(homeDir, ".ldt", "ldtplus.config.toml");
        return ldtPlusConfigPath;
    }

    private bool DirContainsLdt(string dir, [MaybeNullWhen(returnValue: false)] out string ldtPath)
    {
        string[] ldtFileNames = ["ldt.exe", "LancelotDeploymentTool.exe"];
        foreach (string ldtFileName in ldtFileNames)
        {
            ldtPath = Path.Combine(dir, ldtFileName);
            if (File.Exists(ldtPath))
                return true;
        }

        ldtPath = null;
        return false;
    }
}