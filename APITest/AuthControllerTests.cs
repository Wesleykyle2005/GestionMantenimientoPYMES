using API.Controllers;
using API.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using SharedModels.Models;
using SharedModels.ModelsDTO.Usuario;
using System.Security.Cryptography;
using System.Text;

namespace APITest;

public class AuthControllerTests
{
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();

        var inMemorySettings = new Dictionary<string, string> {
            {"JwtSettings:Key", "ClaveSuperSecreta1234567890!@#API"},
            {"JwtSettings:Issuer", "https://localhost:7015"},
            {"JwtSettings:Audience", "https://localhost:7015"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _controller = new AuthController(_repoMock.Object, configuration);
    }


    [Fact]
    public async Task Login_ReturnsBadRequest_WhenModelStateInvalid()
    {
        _controller.ModelState.AddModelError("Email", "Required");
        var dto = new UsuarioLoginDTO { Email = "", Contraseña = "" };

        var result = await _controller.Login(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        var dto = new UsuarioLoginDTO { Email = "test@test.com", Contraseña = "123456" };
        _repoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((Usuario)null);

        var result = await _controller.Login(dto);

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Credenciales inválidas", unauthorized.Value);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIncorrect()
    {
        var dto = new UsuarioLoginDTO { Email = "test@test.com", Contraseña = "123456" };
        var usuario = new Usuario
        {
            UsuarioId = 1,
            Email = "test@test.com",
            Nombre = "Test",
            Rol = "Admin",
            Contraseña = Encoding.UTF8.GetBytes("otra") // Hash incorrecto a propósito
        };
        _repoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(usuario);

        var result = await _controller.Login(dto);

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Credenciales inválidas", unauthorized.Value);
    }

    [Fact]
    public async Task Login_ReturnsOk_WithToken_WhenCredentialsAreCorrect()
    {
        var password = "123456";
        var dto = new UsuarioLoginDTO { Email = "test@test.com", Contraseña = password };
        var usuario = new Usuario
        {
            UsuarioId = 1,
            Email = "test@test.com",
            Nombre = "Test",
            Rol = "Admin",
            Contraseña = HashPassword(password)
        };
        _repoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(usuario);

        var result = await _controller.Login(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var tokenProp = okResult.Value.GetType().GetProperty("token");
        Assert.NotNull(tokenProp);
        var token = tokenProp.GetValue(okResult.Value) as string;
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    // Helper para hashear igual que el controlador
    private static byte[] HashPassword(string password)
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
