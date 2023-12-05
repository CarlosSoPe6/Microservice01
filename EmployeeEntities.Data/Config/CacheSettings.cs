namespace EmployeeEntities.Data.Config;

public record CacheSettings
{
    public string ConnectionString { get; init; }
    public double TimeToLive { get; init; }
}
