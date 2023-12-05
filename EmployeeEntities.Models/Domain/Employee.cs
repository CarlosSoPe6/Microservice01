using System.Text.Json.Serialization;

namespace EmployeeEntities.Models.Domain;
public class Employee
{
    public string id { get; set; }
    public string name { get; set;}
    public string lastName {get; set;}
    public Region region {get; set;}
}
