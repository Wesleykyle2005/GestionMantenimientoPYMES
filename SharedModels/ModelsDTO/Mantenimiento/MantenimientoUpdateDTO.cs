using System.ComponentModel.DataAnnotations;

namespace SharedModels.ModelsDTO.Mantenimiento;

public class MantenimientoUpdateDTO
{
    public int EquipoId { get; set; }

    public int MantenimientoId { get; set; }
    [Required]
    public DateTime Fecha { get; set; }

    [Required, MaxLength(20)]
    public string Tipo { get; set; }

    [Required, MaxLength(200)]
    public string Descripcion { get; set; }

    [Required, MaxLength(50)]
    public string Estado { get; set; }
}
