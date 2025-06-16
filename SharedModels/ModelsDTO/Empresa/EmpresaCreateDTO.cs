using System.ComponentModel.DataAnnotations;

namespace SharedModels.ModelsDTO.Empresa;

public class EmpresaCreateDTO
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; }

    [Required, MaxLength(200)]
    public string Direccion { get; set; }

    [Required, MaxLength(20)]
    public string Telefono { get; set; }
}
