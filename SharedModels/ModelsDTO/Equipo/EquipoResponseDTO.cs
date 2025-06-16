namespace SharedModels.ModelsDTO.Equipo;

public class EquipoResponseDTO
{
    public int EquipoId { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Estado { get; set; }
    public int EmpresaId { get; set; }
    public string EmpresaNombre { get; set; }
}
