using System.ComponentModel.DataAnnotations;

namespace PrimeiraApi.Application.ViewModel;

public class EmployeeViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, 150)]
    public int Age { get; set; }

    [Required]
    public IFormFile? Photo { get; set; }
}