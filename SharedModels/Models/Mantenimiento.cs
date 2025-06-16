using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels.Models;

public class Mantenimiento
{
    [Key]
    public int MantenimientoId { get; set; }

    [ForeignKey("Equipo")]
    public int EquipoId { get; set; }
    public Equipo Equipo { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required, MaxLength(20)]
    public string Tipo { get; set; } // Preventivo o Correctivo

    [Required, MaxLength(200)]
    public string Descripcion { get; set; }

    [Required, MaxLength(50)]
    public string Estado { get; set; }
}
