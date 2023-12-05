namespace EmployeeEntities.Data.Config;

public record DatabaseSettings
{
    public string ConnectionString { get; init; }
    public string DatabaseName { get; init; }
    public string ContainerName { get; init; }
}
