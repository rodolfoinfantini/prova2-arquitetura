namespace Efc2.Database;

public interface IDatabase<TEntity, TId>
{
    TEntity? GetById(TId id);
    IEnumerable<TEntity> GetAll();
    void Save(TEntity entity);
    int GenerateId();
}
