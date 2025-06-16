using API.Controllers;
using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedModels.Models;
using SharedModels.ModelsDTO.Usuario;

namespace APITest;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UsuarioController _controller;

    public UsuarioControllerTests()
    {
        _repoMock = new Mock<IUsuarioRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new UsuarioController(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenUsuarioExists()
    {
        var usuario = new Usuario { UsuarioId = 1, Nombre = "Test" };
        var dto = new UsuarioResponseDTO { UsuarioId = 1, Nombre = "Test" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usuario);
        _mapperMock.Setup(m => m.Map<UsuarioResponseDTO>(usuario)).Returns(dto);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenUsuarioDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Usuario)null);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var usuarios = new List<Usuario> { new Usuario { UsuarioId = 1 } };
        var dtos = new List<UsuarioResponseDTO> { new UsuarioResponseDTO { UsuarioId = 1 } };
        _repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(usuarios);
        _mapperMock.Setup(m => m.Map<List<UsuarioResponseDTO>>(usuarios)).Returns(dtos);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Nombre", "Required");

        var result = await _controller.Create(new UsuarioCreateDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenUsuarioExists()
    {
        var dto = new UsuarioCreateDTO { Email = "test@test.com", Contraseña = "123456" };
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Usuario, bool>>>())).ReturnsAsync(true);

        var result = await _controller.Create(dto);

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenUsuarioIsCreated()
    {
        var dto = new UsuarioCreateDTO { Email = "test@test.com", Contraseña = "123456" };
        var usuario = new Usuario { UsuarioId = 1, Email = "test@test.com" };
        var responseDto = new UsuarioResponseDTO { UsuarioId = 1, Email = "test@test.com" };

        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Usuario, bool>>>())).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<Usuario>(dto)).Returns(usuario);
        _repoMock.Setup(r => r.CreateAsync(usuario)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<UsuarioResponseDTO>(usuario)).Returns(responseDto);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(responseDto, createdResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var dto = new UsuarioUpdateDTO { UsuarioId = 2 };
        var result = await _controller.Update(1, dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenUsuarioDoesNotExist()
    {
        var dto = new UsuarioUpdateDTO { UsuarioId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Usuario)null);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdated()
    {
        var dto = new UsuarioUpdateDTO { UsuarioId = 1, Contraseña = "nueva" };
        var usuario = new Usuario { UsuarioId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usuario);
        _mapperMock.Setup(m => m.Map(dto, usuario));
        _repoMock.Setup(r => r.UpdateAsync(usuario)).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenUsuarioDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Usuario)null);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var usuario = new Usuario { UsuarioId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usuario);
        _repoMock.Setup(r => r.DeleteAsync(usuario)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetByEmail_ReturnsOk_WhenUsuarioExists()
    {
        var usuario = new Usuario { UsuarioId = 1, Email = "test@test.com" };
        var dto = new UsuarioResponseDTO { UsuarioId = 1, Email = "test@test.com" };
        _repoMock.Setup(r => r.GetByEmailAsync("test@test.com")).ReturnsAsync(usuario);
        _mapperMock.Setup(m => m.Map<UsuarioResponseDTO>(usuario)).Returns(dto);

        var result = await _controller.GetByEmail("test@test.com");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task GetByEmail_ReturnsNotFound_WhenUsuarioDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByEmailAsync("test@test.com")).ReturnsAsync((Usuario)null);

        var result = await _controller.GetByEmail("test@test.com");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetByEmpresaId_ReturnsOk_WithList()
    {
        var usuarios = new List<Usuario> { new Usuario { UsuarioId = 1, EmpresaId = 2 } };
        var dtos = new List<UsuarioResponseDTO> { new UsuarioResponseDTO { UsuarioId = 1 } };
        _repoMock.Setup(r => r.GetByEmpresaIdAsync(2)).ReturnsAsync(usuarios);
        _mapperMock.Setup(m => m.Map<List<UsuarioResponseDTO>>(usuarios)).Returns(dtos);

        var result = await _controller.GetByEmpresaId(2);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }
}
