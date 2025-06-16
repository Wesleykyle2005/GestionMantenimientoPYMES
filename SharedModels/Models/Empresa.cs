using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models;

public class Empresa
{
    [Key]
    public int EmpresaId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, MaxLength(200)]
    public string Direccion { get; set; }

    [Required, MaxLength(20)]
    public string Telefono { get; set; }

    public ICollection<Usuario> Usuarios { get; set; }
    public ICollection<Equipo> Equipos { get; set; }
}
