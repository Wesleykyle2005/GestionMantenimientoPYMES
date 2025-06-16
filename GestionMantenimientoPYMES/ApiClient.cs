using System.Configuration;
using System.Net.Http.Headers;
using GestionMantenimientoPYMES.Repositories;
using GestionMantenimientoPYMES.Repositories.IRepositories;


namespace GestionMantenimientoPYMES;

public class ApiClient
{
    private readonly HttpClient _httpClient;
    public IUsuarioRepository Usuario { get; }
    public IEmpresaRepository Empresa { get; }
    public IEquipoRepository Equipo { get; }
    public IMantenimientoRepository Mantenimiento { get; }

    public ApiClient()
    {
        string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        _httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

        Usuario = new UsuarioRepository(_httpClient, "Usuario");
        Empresa = new EmpresaRepository(_httpClient, "Empresa");
        Equipo = new EquipoRepository(_httpClient, "Equipo");
        Mantenimiento = new MantenimientoRepository(_httpClient, "Mantenimiento");
    }

    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}