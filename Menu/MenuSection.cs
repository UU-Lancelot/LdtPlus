namespace LdtPlus.Menu;
public record MenuSection
(
    string Title,
    IEnumerable<IMenuItem> Submenu
);
