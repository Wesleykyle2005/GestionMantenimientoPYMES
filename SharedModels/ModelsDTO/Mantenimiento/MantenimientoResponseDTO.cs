namespace SharedModels.ModelsDTO.Mantenimiento;

public class MantenimientoResponseDTO
{
    public int MantenimientoId { get; set; }
    public int EquipoId { get; set; }
    public string EquipoNombre { get; set; }
    public string EquipoTipo { get; set; }
    public int EmpresaId { get; set; }
    public string EmpresaNombre { get; set; }
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }
    public string Estado { get; set; }
}
