using Newtonsoft.Json;
using SharedModels.ModelsDTO.Empresa;
using GestionMantenimientoPYMES.Repositories.IRepositories;

namespace GestionMantenimientoPYMES.Repositories;

public class EmpresaRepository : Repository<EmpresaResponseDTO>, IEmpresaRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public EmpresaRepository(HttpClient httpClient, string endpoint)
        : base(httpClient, endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<EmpresaResponseDTO> GetByNombreAsync(string nombre)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-nombre/{nombre}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<EmpresaResponseDTO>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<EmpresaResponseDTO>> GetAllWithUsuariosAsync()
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/with-usuarios");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<EmpresaResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }
}