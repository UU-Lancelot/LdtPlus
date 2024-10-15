namespace LdtPlus.Interactive.MenuData;
public record MenuSection
(
    string Title,
    IEnumerable<IMenuRow> Submenu
);
