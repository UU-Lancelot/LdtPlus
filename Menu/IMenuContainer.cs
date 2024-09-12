namespace LdtPlus.Menu;
public interface IMenuContainer
{
    IEnumerable<string> Navigation { get; }
    IEnumerable<MenuSection> Sections { get; }
}
