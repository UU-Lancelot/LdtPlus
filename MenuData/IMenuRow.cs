namespace LdtPlus.MenuData;
public interface IMenuRow : IMenuItem
{
    string Description { get; }
    IEnumerable<IMenuItem> ItemOptions { get; }
}
