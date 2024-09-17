namespace LdtPlus.MenuData;
public interface IMenuContainer
{
    IEnumerable<IMenuNav> Navigation { get; }
    IEnumerable<MenuSection> Sections { get; }
}
