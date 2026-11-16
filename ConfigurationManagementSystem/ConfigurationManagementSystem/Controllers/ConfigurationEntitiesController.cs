using ConfigurationManagementSystem.DbContext;
using ConfigurationManagementSystem.Repositories;
using ConfigurationManagementSystem.Repositories.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ConfigurationManagementSystem.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigurationEntitiesController : ControllerBase
{
    private ConfigurationEntitiesRepository _repository { get; set; }

    public ConfigurationEntitiesController(AppDbContext context)
    {
        _repository = new ConfigurationEntitiesRepository(context);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var data = _repository.GetAllAsync().Result;
        return Ok(data);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(ConfigurationEntity entity)
    {
        try
        {
            _repository.Add(entity);
            await _repository.SaveChangesAsync();
        }
        catch (Exception)
        {
            return Ok(entity);
        }

        return Ok(entity);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(ConfigurationEntity entity)
    {
        var existing = await FindAsync(entity.Key);

        if (existing == null)
            return NotFound();

        existing = entity;

        await _repository.SaveChangesAsync();
        return Ok(entity);
    }

    private async Task<ConfigurationEntity> FindAsync(string key)
    {
        using (_repository)
        {
            return await _repository.GetByKeyAsync(key);
        }
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _repository.GetByKeyAsync(id.ToString());

        return Ok();
    }

}
