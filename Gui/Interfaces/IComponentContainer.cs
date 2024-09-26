namespace LdtPlus.Gui.Interfaces;
public interface IComponentContainer
{
    void Add(IComponent component);
    void Update(IComponent component);
    void Remove(IComponent component);
    void Rerender();
}
