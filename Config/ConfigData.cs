using LdtPlus.MenuData;

namespace LdtPlus.Config;
public record ConfigData
{
    public ConfigData()
    {
        #warning TODO: load configuration
        Menu = new([
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
        ]);
    }

    public MenuRoot Menu { get; }
}