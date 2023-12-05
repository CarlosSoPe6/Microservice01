using AutoMapper;
using EmployeeEntities.Data.Interface;
using EmployeeEntities.Models.Domain;
using EmployeeEntities.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;

namespace EmployeeEntities.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IDataStoreClient<Employee> _dataStore;
    private readonly ILogger<EmployeeController> _logger;
    private readonly IMapper _mapper;
    private readonly ICacheClient _cache;

    public EmployeeController(
        IDataStoreClient<Employee> dataStore,
        ILogger<EmployeeController> logger,
        IMapper mapper,
        ICacheClient cache
    )
    {
        _dataStore = dataStore;
        _logger = logger;
        _mapper = mapper;
        _cache = cache;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Read(string id)
    {
        var cahcedData = await _cache.GetFromCache<Employee>(id);
        if (cahcedData != null )
        {
            StatusCode((int)HttpStatusCode.OK, cahcedData);
        }
        var result = await _dataStore.GetBtId(id);
        if (result.Success)
        {
            return StatusCode((int)result.Status, result.ErrorMessage);
        }
        await _cache.SetCache(id, result.Data);
        return StatusCode((int)result.Status, result.Data);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody]EmployeeRequest employee)
    {
        var domainEmployee = _mapper.Map<Employee>(employee);
        domainEmployee.id = new Guid().ToString();
        var result = await _dataStore.Create(domainEmployee);
        if (result.Success)
        {
            return StatusCode((int)result.Status, result.ErrorMessage);
        }
        return StatusCode((int)result.Status, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, [FromBody] EmployeeRequest employee)
    {
        var domainEmployee = _mapper.Map<Employee>(employee);
        domainEmployee.id = id;
        var result = await _dataStore.Update(domainEmployee);
        if (result.Success)
        {
            return StatusCode((int)result.Status, result.ErrorMessage);
        }
        await _cache.ClearCache(id);
        return StatusCode((int)result.Status, result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _dataStore.DeleteById(id);
        if (result.Success)
        {
            return StatusCode((int)result.Status, result.ErrorMessage);
        }
        await _cache.ClearCache(id);
        return StatusCode((int)result.Status);
    }
}
