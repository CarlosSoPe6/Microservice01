using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;

namespace EmployeeEntities.Data;
public record Result<T>(
    HttpStatusCode Status,
    bool Success,
    T Data,
    string? ErrorMessage
);
