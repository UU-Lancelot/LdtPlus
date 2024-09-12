namespace LdtPlus.Gui.Interfaces;
public interface IComponentContainer
{
    void Add(string key, IComponent component);
    void Update(string key, IComponent component);
    void Remove(string key);
}
