using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeiraApi.Application.ViewModel;
using PrimeiraApi.Domain.DTOs;
using PrimeiraApi.Domain.Models;

namespace PrimeiraApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/employee")]
[ApiVersion("1.0")]
public class EmployeeController : ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly string _storagePath;
    private readonly IMapper _mapper;

    public EmployeeController(
        IEmployeeRepository employeeRepository,
        ILogger<EmployeeController> logger,
        IWebHostEnvironment environment,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _storagePath = Path.Combine(environment.ContentRootPath, "Storage");
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [Authorize]
    [HttpPost]
    public IActionResult Add([FromForm] EmployeeViewModel employeeView)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var photo = employeeView.Photo!;
        Directory.CreateDirectory(_storagePath);

        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(photo.FileName)}";
        var filePath = Path.Combine(_storagePath, fileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);
        photo.CopyTo(fileStream);

        var employee = new Employee(employeeView.Name, employeeView.Age, Path.Combine("Storage", fileName));
        _employeeRepository.Add(employee);

        return Ok(employee);
    }

    [Authorize]
    [HttpGet("{id:int}/download")]
    public IActionResult DownloadPhoto(int id)
    {
        var employee = _employeeRepository.Get(id);
        if (employee?.photo is null)
        {
            return NotFound();
        }

        var photoPath = Path.Combine(_storagePath, Path.GetFileName(employee.photo));
        if (!System.IO.File.Exists(photoPath))
        {
            return NotFound();
        }

        return PhysicalFile(photoPath, "application/octet-stream");
    }

    [HttpGet]
    public IActionResult Get([FromQuery] int pageNumber = 0, [FromQuery] int pageQuantity = 10)
    {
        if (pageNumber < 0 || pageQuantity is < 1 or > 100)
        {
            return BadRequest("pageNumber deve ser maior ou igual a zero e pageQuantity deve estar entre 1 e 100.");
        }

        var employees = _employeeRepository.Get(pageNumber, pageQuantity);
        _logger.LogInformation("{EmployeeCount} funcionários retornados.", employees.Count);
        return Ok(employees);
    }
    
    [HttpGet]
    [Route("{id}")]
    public IActionResult Search(int id)
    {
        _logger.Log(LogLevel.Error, "Teve um Erro");

        var employee = _employeeRepository.Get(id);
        if (employee is null)
        {
            return NotFound();
        }

        var employeeDTO = _mapper.Map<EmployeeDTO>(employee);

        _logger.LogInformation("Funcionário {EmployeeId} retornado.", id);

        return Ok(employeeDTO);

    }
    
}
