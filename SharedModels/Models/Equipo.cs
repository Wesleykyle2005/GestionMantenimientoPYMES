using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels.Models;

public class Equipo
{
    [Key]
    public int EquipoId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, MaxLength(50)]
    public string Tipo { get; set; }

    [Required, MaxLength(50)]
    public string Marca { get; set; }

    [Required, MaxLength(50)]
    public string Modelo { get; set; }

    [Required, MaxLength(50)]
    public string Estado { get; set; }

    [ForeignKey("Empresa")]
    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

    public ICollection<Mantenimiento> Mantenimientos { get; set; }
}
