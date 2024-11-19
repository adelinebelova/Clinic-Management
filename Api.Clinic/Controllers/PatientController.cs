using System;
using Microsoft.AspNetCore.Mvc;
using Library.Clinic.Models;
using Api.Clinic.Enterprise;

namespace Api.Clinic.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    private readonly ILogger<PatientController> _logger;

    public PatientController(ILogger<PatientController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Patient> Get()
    {
        return new PatientEC().Patients;
    }
}

