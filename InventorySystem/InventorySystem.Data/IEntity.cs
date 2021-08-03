namespace InventorySystem.Data
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
