namespace LdtPlus.Menu;
public interface IMenuContainer
{
    IEnumerable<MenuSection> Sections { get; }
}
