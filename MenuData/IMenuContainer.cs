namespace LdtPlus.MenuData;
public interface IMenuContainer
{
    IEnumerable<IMenuItem> Navigation { get; }
    IEnumerable<MenuSection> Sections { get; }
}
