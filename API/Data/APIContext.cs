using Microsoft.EntityFrameworkCore;
using SharedModels;
using SharedModels.Models;

namespace API.Data;

public class APIContext:DbContext
{
    public APIContext(DbContextOptions<APIContext> options) : base(options)
    {
    }
    
    public DbSet<Empresa> Empresas { get; set; } = null!;
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Equipo> Equipos { get; set; } = null!;
    public DbSet<Mantenimiento> Mantenimientos { get; set; } = null!;

}
