using LdtPlus.Config.RawData;
using LdtPlus.Interactive.MenuData;

namespace LdtPlus.Config;
public record ConfigData(
    string LdtPath,
    string LdtVersion,
    string LdtBookkitUrl,
    IEnumerable<MenuSection> Sections,
    List<MenuItemFavourite> Favourites,
    List<MenuItemRecent> Recent
)
{
    public MenuRoot Menu => new(Sections, Favourites, Recent);

    #region update
    public void AddRecent(string command)
    {
        Recent.RemoveAll(i => i.Name == command);
        Recent.Insert(0, new MenuItemRecent(command));

        if (Recent.Count > 100)
            Recent.RemoveAt(Recent.Count - 1);
    }

    public void AddFavourite(string name, string command)
    {
        Favourites.Add(new MenuItemFavourite(name, command));
    }

    public void RenameFavourite(string name, string newName)
    {
        int favouriteIndex = Favourites.FindIndex(f => f.Name == name);
        if (favouriteIndex < 0)
            return;

        Favourites[favouriteIndex] = new MenuItemFavourite(newName, Favourites[favouriteIndex].Command);
    }

    public void DeleteFavourite(string name)
    {
        int favouriteIndex = Favourites.FindIndex(f => f.Name == name);
        if (favouriteIndex < 0)
            return;

        Favourites.RemoveAt(favouriteIndex);
    }
    #endregion

    #region save
    public ConfigRaw ToRaw()
    {
        return new()
        {
            LdtPath = LdtPath,
            LdtVersion = LdtVersion,
            LdtBookkitUrl = LdtBookkitUrl,
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
            Sections = row is IMenuContainer container ? container.Sections.Select(Create).ToList() : [],
            Type = row switch 
            {
                MenuItemArea => ConfigRawMenuType.Area,
                MenuItemCommand => ConfigRawMenuType.Command,
                MenuItemArgumentSelect => ConfigRawMenuType.ArgumentSelect,
                MenuItemArgumentPath => ConfigRawMenuType.ArgumentPath,
                MenuItemArgumentFlag => ConfigRawMenuType.ArgumentFlag,
                MenuItemArgumentText => ConfigRawMenuType.ArgumentText,
                _ => null,
            },
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
        string ldtBookkitUrl = raw.LdtBookkitUrl ?? throw new MissingMemberException(nameof(ConfigRaw.LdtBookkitUrl));
        MenuSection[] sections = raw.Sections.Select(ValidateAndCreate).ToArray();
        List<MenuItemFavourite> favourites = raw.Favourites.Select(ValidateAndCreate).ToList();
        List<MenuItemRecent> recent = raw.History?.Recent.Select(ValidateAndCreate).ToList() ?? new();

        return new ConfigData(ldtPath, ldtVersion, ldtBookkitUrl, sections, favourites, recent);
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
        ConfigRawMenuType type = menu.GetMenuType();

        switch (type)
        {
            case ConfigRawMenuType.Area:
                return new MenuItemArea(
                    Name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                    Description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}"),
                    Sections: menu.Sections.Select(ValidateAndCreate).ToArray()
                );

            case ConfigRawMenuType.Command:
            default:
                return new MenuItemCommand(
                    Name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                    Description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}"),
                    ArgumentSections: menu.Sections.Select(ValidateAndCreate).ToArray()
                );

            case ConfigRawMenuType.ArgumentSelect:
                return new MenuItemArgumentSelect(
                    name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                    description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}"),
                    values: menu.Sections.SelectMany(s => s.Submenu).Select(v => v.Name).Where(v => v is not null).Cast<string>().ToArray()
                );

            case ConfigRawMenuType.ArgumentPath:
                return new MenuItemArgumentPath(
                    name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                    description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}")
                );

            case ConfigRawMenuType.ArgumentFlag:
                return new MenuItemArgumentFlag(
                    name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                    description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}")
                );
            
            case ConfigRawMenuType.ArgumentText:
                return new MenuItemArgumentText(
                    name: menu.Name ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Name)}"),
                    description: menu.Description ?? throw new MissingMemberException($"{nameof(ConfigRawSection.Submenu)}.{nameof(ConfigRawMenu.Description)}")
                );
        }
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