using Newtonsoft.Json;
using SharedModels.ModelsDTO.Usuario;
using System.Text;
using GestionMantenimientoPYMES.Repositories.IRepositories;

namespace GestionMantenimientoPYMES.Repositories;

public class UsuarioRepository : Repository<UsuarioResponseDTO>, IUsuarioRepository
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public UsuarioRepository(HttpClient httpClient, string endpoint)
        : base(httpClient, endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
    }

    public async Task<UsuarioResponseDTO> GetByEmailAsync(string email)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-email/{email}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UsuarioResponseDTO>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<IEnumerable<UsuarioResponseDTO>> GetByEmpresaIdAsync(int empresaId)
    {
        var response = await _httpClient.GetAsync($"{_endpoint}/by-empresa/{empresaId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<UsuarioResponseDTO>>(content);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            throw new Exception(errorResponse);
        }
    }

    public async Task<string> AuthenticateUserAsync(string email, string password)
    {
        var loginDto = new UsuarioLoginDTO { Email = email, Contraseña = password };
        var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("Auth/login", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(responseData).token;
        }
        else
        {
            throw new Exception("Credenciales inválidas");
        }
    }
}