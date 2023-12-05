using EmployeeEntities.Models.Domain;

namespace EmployeeEntities.Models.Requests;

public record EmployeeRequest
{
    public string name { get; set;}
    public string lastName {get; set;}
    public Region region {get; set;}
}
