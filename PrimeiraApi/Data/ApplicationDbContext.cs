using Microsoft.EntityFrameworkCore;
using PrimeiraApi.Domain.Models;

namespace PrimeiraApi.Data;

/// <summary>
/// Contexto do banco PostgreSQL da aplicação.
/// Adicione os DbSets conforme criar as entidades do projeto.
/// </summary>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
}
