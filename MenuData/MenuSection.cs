namespace LdtPlus.MenuData;
public record MenuSection
(
    string Title,
    IEnumerable<IMenuItem> Submenu
);
