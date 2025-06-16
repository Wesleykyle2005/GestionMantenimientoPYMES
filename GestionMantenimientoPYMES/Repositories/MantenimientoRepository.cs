using Newtonsoft.Json;
using SharedModels.ModelsDTO.Mantenimiento;
using GestionMantenimientoPYMES.Repositories.IRepositories;

namespace GestionMantenimientoPYMES.Repositories;

public class MantenimientoRepository : Repository<MantenimientoResponseDTO>, IMantenimientoRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public MantenimientoRepository(HttpClient httpClient, string endpoint)
        : base(httpClient, endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<IEnumerable<MantenimientoResponseDTO>> GetByEquipoIdAsync(int equipoId)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-equipo/{equipoId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MantenimientoResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<MantenimientoResponseDTO>> GetByEstadoAsync(string estado)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-estado/{estado}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MantenimientoResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<MantenimientoResponseDTO>> GetByEmpresaIdAsync(int empresaId)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-empresa/{empresaId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MantenimientoResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<MantenimientoResponseDTO>> GetProximosAsync(DateTime fechaLimite)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/proximos?fechaLimite={fechaLimite:yyyy-MM-dd}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MantenimientoResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }
}
