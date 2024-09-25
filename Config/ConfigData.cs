using LdtPlus.Config.RawData;
using LdtPlus.MenuData;

namespace LdtPlus.Config;
public record ConfigData(
    string LdtPath,
    string LdtVersion,
    IEnumerable<MenuSection> Sections,
    List<MenuItemFavourite> Favourites,
    List<MenuItemRecent> Recent
)
{
    public MenuRoot Menu => new(Sections, Favourites, Recent);

    #region save
    public ConfigRaw ToRaw()
    {
        return new()
        {
            LdtPath = LdtPath,
            LdtVersion = LdtVersion,
            Sections = Sections.Select(Create).ToList(),
            Favourites = Favourites.Select(Create).ToList(),
            History = Create(Recent),
        };
    }

    private ConfigRawSection Create(MenuSection section)
    {
        return new()
        {
            Title = section.Title,
            Submenu = section.Submenu.Select(Create).ToList(),
        };
    }
    private ConfigRawMenu Create(IMenuRow row)
    {
        return new()
        {
            Name = row.Name,
            Description = row.Description,
            Sections = row is MenuItemArea area ? area.Sections.Select(Create).ToList() : new(),
        };
    }
    private ConfigRawFavourite Create(MenuItemFavourite favourite)
    {
        return new()
        {
            Name = favourite.Name,
            Command = favourite.Command,
        };
    }
    private ConfigRawHistory Create(IEnumerable<MenuItemRecent> recent)
    {
        return new()
        {
            Recent = recent.Select(r => r.Name).ToList(),
        };
    }
    #endregion

    #region load
    public static ConfigData FromRaw(ConfigRaw raw)
    {
        string ldtPath = raw.LdtPath ?? throw new MissingMemberException(nameof(ConfigRaw.LdtPath));
        string ldtVersion = raw.LdtVersion ?? throw new MissingMemberException(nameof(ConfigRaw.LdtVersion));
        MenuSection[] sections = raw.Sections.Select(ValidateAndCreate).ToArray();
        List<MenuItemFavourite> favourites = raw.Favourites.Select(ValidateAndCreate).ToList();
        List<MenuItemRecent> recent = raw.History?.Recent.Select(ValidateAndCreate).ToList() ?? new();

        return new ConfigData(ldtPath, ldtVersion, sections, favourites, recent);
    }

    private static MenuSection ValidateAndCreate(ConfigRawSection section)
    {
        return new MenuSection
        (
            Title: section.Title ?? throw new MissingMemberException($"{nameof(ConfigRaw.Sections)}.{nameof(ConfigRawSection.Title)}"),
            Submenu: section.Submenu.Select(ValidateAndCreate).ToArray()
        );
    }
    private static IMenuRow ValidateAndCreate(ConfigRawMenu menu)
    {
        // is area
        if (menu.Sections.Any())
        {
            return new MenuItemArea(
                Name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                Description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}"),
                Sections: menu.Sections.Select(ValidateAndCreate).ToArray()
            );
        }

        // is command
        return new MenuItemCommand(
            Name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
            Description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}")
        );
    }
    private static MenuItemFavourite ValidateAndCreate(ConfigRawFavourite favourite)
    {
        return new(
            Name: favourite.Name ?? throw new MissingMemberException($"{nameof(ConfigRaw.Favourites)}.{nameof(ConfigRawFavourite.Name)}"),
            Command: favourite.Command ?? throw new MissingMemberException($"{nameof(ConfigRaw.Favourites)}.{nameof(ConfigRawFavourite.Command)}")
        );
    }
    private static MenuItemRecent ValidateAndCreate(string recent)
    {
        return new(
            Name: recent
        );
    }
    #endregion
}