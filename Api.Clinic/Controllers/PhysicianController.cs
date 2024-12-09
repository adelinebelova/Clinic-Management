using System;
using Microsoft.AspNetCore.Mvc;
using Library.Clinic.Models;
using Api.Clinic.Enterprise;
using Library.Clinic.DTO;

namespace Api.Clinic.Controllers;

[ApiController]
[Route("[controller]")]
public class PhysicianController : ControllerBase
{
    private readonly ILogger<PhysicianController> _logger;

    public PhysicianController(ILogger<PhysicianController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<PhysicianDTO> Get()
    {
        return new PhysicianEC().Physicians;
    }

    [HttpGet("{id}")]
    public PhysicianDTO? GetById(int id)
    {
        return new PhysicianEC().GetById(id);
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        new PhysicianEC().Delete(id);
    }

    [HttpPost("Search")]
    public List<PhysicianDTO> Search([FromBody] Query q)
    {
        return new PhysicianEC().Search(q?.Content ?? string.Empty)?.ToList() ?? new List<PhysicianDTO>();
    }

    [HttpPost("UpdatePhysician")]
    public Physician? Update([FromBody] Physician physician){
        return new PhysicianEC().Update(physician);
    }

    [HttpPost]
    public Physician? Add([FromBody] Physician physician){
        return new PhysicianEC().Add(physician);
    }

}

