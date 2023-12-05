namespace EmployeeEntities.Data.Interface;

public interface IDataStoreClient<TEntity> where TEntity : class
{
    Task<Result<TEntity>> GetBtId(string id);
    Task<Result<TEntity>> Create(TEntity data);
    Task<Result<TEntity>> Update(TEntity data);
    Task<Result<TEntity>> DeleteById(string id);
}
