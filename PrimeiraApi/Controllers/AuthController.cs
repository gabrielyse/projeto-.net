using Microsoft.AspNetCore.Mvc;
using PrimeiraApi.Application.Services;
using PrimeiraApi.Domain.Models;

namespace PrimeiraApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Auth(string username, string password)
    {
        if (username == "filipe" && password == "123456")
        {
            var employee = new Employee("filipe", 25, "");
            var token = TokenService.GenerateToken(employee);

            return Ok(token);
        }

        return BadRequest("username or password invalid");
    }
}
