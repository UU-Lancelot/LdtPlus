namespace LdtPlus.MenuData;
public interface IMenuContainer
{
    IEnumerable<string> ItemOptions { get; }
    IEnumerable<string> Navigation { get; }
    IEnumerable<MenuSection> Sections { get; }
}
