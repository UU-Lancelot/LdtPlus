using LdtPlus.MenuData;

namespace LdtPlus.Config;
public record ConfigData
{
    public ConfigData()
    {
        #warning TODO: load configuration
        Menu = new([
            new("first", [
                new MenuItemWithSubmenu("first.first", "testing A", [
                    new("first.first.first", [ new MenuCommand { Name = "fir", Description = "eee" } ]),
                    new("first.first.second", [ new MenuCommand { Name = "sec", Description = "ueou" } ]),
                ]),
            ]),
            new("second", [
                new MenuCommand { Name = "second.first",  Description = "ueouuaeuae" },
                new MenuCommand { Name = "second.second",  Description = "eeeuea.," },
            ]),
        ]);
    }

    public MenuRoot Menu { get; }
}