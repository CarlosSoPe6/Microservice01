using EmployeeEntities.Data;
using EmployeeEntities.Data.Config;
using EmployeeEntities.Data.Interface;
using EmployeeEntities.Data.Mappers;
using EmployeeEntities.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appCofnigConnectionString = Environment.GetEnvironmentVariable("AZURE_APP_CONFIGURATION");
builder.Configuration.AddAzureAppConfiguration((options) =>
{
    options.Connect(appCofnigConnectionString)
        .Select("EmployeeEntities:*", LabelFilter.Null);

});
builder.Configuration.AddEnvironmentVariables();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAzureAppConfiguration();
builder.Services.AddControllers();
builder.Services.AddOptions();
builder.Services.AddLogging();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<EmployeeRequestMapping>();
});
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetRequiredSection("EmployeeEntities:DatabaseSettings"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache((config) =>
{
    var cacheSettings = builder.Configuration.GetRequiredSection("EmployeeEntities:CacheSettings");
    var connectionString = cacheSettings.GetValue<string>("ConnectionString");

    config.Configuration = connectionString;
});
builder.Services.AddTransient<IDataStoreClient<Employee>>((provider) =>
{
    var databseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    var cosmosClient = new CosmosClient(
        databseSettings.ConnectionString
    );
    var containerClient = cosmosClient.GetContainer(databseSettings.DatabaseName, databseSettings.ContainerName);
    return new DataStoreClient<Employee>(containerClient);
});

var app = builder.Build();
app.UseAzureAppConfiguration();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
// app.UseAuthorization();
app.MapControllers();
app.Run();
