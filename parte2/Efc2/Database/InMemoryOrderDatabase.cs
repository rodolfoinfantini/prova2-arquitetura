using Efc2.Models;

namespace Efc2.Database;

public sealed class InMemoryOrderDatabase(ILogger<InMemoryOrderDatabase> logger) : IDatabase<Order, int>
{
    private readonly Dictionary<int, Order> _orders = [];
    private int _nextId = 1;
    private readonly SemaphoreSlim _mutex = new(1);

    public IEnumerable<Order> GetAll() => _orders.Values;

    public Order? GetById(int id)
        => _orders.GetValueOrDefault(id);

    public void Save(Order entity)
    {
        if (!entity.Id.HasValue)
            throw new ArgumentNullException(nameof(entity));

        logger.LogInformation("Saved order with id {id}", entity.Id);
        _orders.Add(entity.Id.Value, entity);
    }

    public int GenerateId()
    {
        _mutex.Wait();

        try
        {
            return _nextId++;
        }
        finally
        {
            _mutex.Release();
        }
    }
}
