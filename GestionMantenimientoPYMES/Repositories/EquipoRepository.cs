using Newtonsoft.Json;
using SharedModels.ModelsDTO.Equipo;
using GestionMantenimientoPYMES.Repositories.IRepositories;

namespace GestionMantenimientoPYMES.Repositories;

public class EquipoRepository : Repository<EquipoResponseDTO>, IEquipoRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public EquipoRepository(HttpClient httpClient, string endpoint)
        : base(httpClient, endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<IEnumerable<EquipoResponseDTO>> GetByEmpresaIdAsync(int empresaId)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-empresa/{empresaId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<EquipoResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<EquipoResponseDTO>> GetByEstadoAsync(string estado)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-estado/{estado}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<EquipoResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }
}