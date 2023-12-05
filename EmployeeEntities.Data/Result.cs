using System.Net;
using Microsoft.Azure.Cosmos;

namespace EmployeeEntities.Data;
public record Result<T>(
    HttpStatusCode Status,
    T Data,
    string? ErrorMessage
);
