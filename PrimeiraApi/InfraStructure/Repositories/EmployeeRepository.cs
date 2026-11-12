using PrimeiraApi.Data;
using PrimeiraApi.Domain.DTOs;
using PrimeiraApi.Domain.Models;

namespace PrimeiraApi.InfraStructure.Repositories;

public class EmployeeRepository(ApplicationDbContext context) : IEmployeeRepository {
    private readonly ApplicationDbContext _context = context;

    public void Add(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }

    public List<EmployeeDTO> Get(int pageNumber, int pageQuantity)
    {
        return _context.Employees.Skip(pageNumber * pageQuantity).Take(pageQuantity)
            .Select( b => new EmployeeDTO()
            {
                Id = b.id,
                NameEmployee = b.name,
                Photo = b.photo
                    
            })
            .ToList();
    }

    public Employee? Get(int id)
    {
        return _context.Employees.Find(id);
    }

}