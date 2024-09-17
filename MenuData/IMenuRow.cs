namespace LdtPlus.MenuData;
public interface IMenuRow : IMenuItem
{
    string Description { get; }
    IEnumerable<string> ItemOptions { get; }
}
