namespace LdtPlus.MenuData;
public record MenuSection
(
    string Title,
    IEnumerable<IMenuRow> Submenu
);
