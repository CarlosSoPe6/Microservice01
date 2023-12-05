using EmployeeEntities.Data.Interface;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace EmployeeEntities.Data;

public class DataStoreClient<TEntity> : IDataStoreClient<TEntity> where TEntity : class
{
    private Container _container;

    public DataStoreClient(Container containerClient)
    {
        _container = containerClient;
    }

    private async Task<Result<TEntity>> TryCatch(Func<Task<ItemResponse<TEntity>>> action)
    {
        try
        {
            var item = await action();
            return new Result<TEntity>(item.StatusCode, item.Resource, null);
        }
        catch (CosmosException ex)
        {
            return new Result<TEntity>(ex.StatusCode, null, ex.Message);
        }
        catch (Exception ex)
        {
            return new Result<TEntity>(HttpStatusCode.InternalServerError, null, ex.Message);
        }
    }

    public async Task<Result<TEntity>> GetBtId(string id)
    {
        return await TryCatch(async () => await _container.ReadItemAsync<TEntity>(id, new PartitionKey(id)));
    }
    public async Task<Result<TEntity>> Create(TEntity data)
    {
        return await TryCatch(async () => await _container.CreateItemAsync(data));
    }
    public async Task<Result<TEntity>> Update(TEntity data)
    {
        return await TryCatch(async () => await _container.UpsertItemAsync(data));
    }
    public async Task<Result<TEntity>> DeleteById(string id)
    {
        return await TryCatch(async () => await _container.DeleteItemAsync<TEntity>(id, new PartitionKey(id)));
    }
}
